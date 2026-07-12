pipeline {
  agent any

  triggers {
    pollSCM('H/2 * * * *')
  }

  options {
    disableConcurrentBuilds()
    timestamps()
  }

  environment {
    IMAGE = 'dovanminh98/authorize-gateway-lab-web'
    VALUES_FILE = 'helm/values.yaml'
    GITHUB_REPO = 'github.com/minhdevops/authorize-gateway-lab-web.git'
  }

  stages {
    stage('Checkout') {
      steps {
        checkout scm

        script {
          env.GIT_SHA = sh(
            script: 'git rev-parse --short=7 HEAD',
            returnStdout: true
          ).trim()

          env.SKIP_PIPELINE = (
            sh(
              script: '''
                git log -1 --pretty=%B | grep -Fq '[skip ci]'
              ''',
              returnStatus: true
            ) == 0
          ).toString()

          echo "Git SHA: ${env.GIT_SHA}"
          echo "Skip pipeline: ${env.SKIP_PIPELINE}"
        }
      }
    }

    stage('Validate') {
      when {
        expression {
          env.SKIP_PIPELINE != 'true'
        }
      }

      steps {
        sh '''
          set -e

          echo "Kiem tra cac file bat buoc..."
          test -f Dockerfile
          test -f "${VALUES_FILE}"
          test -f helm/Chart.yaml

          echo "Kiem tra Helm chart..."
          helm lint helm

          echo "Render Helm manifest..."
          helm template authorize-gateway-lab-web helm \
            --namespace authorize-gateway-lab \
            > /dev/null

          echo "Validate thanh cong."
        '''
      }
    }

    stage('Build Image') {
      when {
        expression {
          env.SKIP_PIPELINE != 'true'
        }
      }

      steps {
        sh '''
          set -e

          echo "Build image: ${IMAGE}:${GIT_SHA}"

          docker build \
            -t "${IMAGE}:${GIT_SHA}" \
            .
        '''
      }
    }

    stage('Push Image') {
      when {
        expression {
          env.SKIP_PIPELINE != 'true'
        }
      }

      steps {
        withCredentials([
          usernamePassword(
            credentialsId: 'dockerhub-creds',
            usernameVariable: 'DOCKERHUB_USER',
            passwordVariable: 'DOCKERHUB_TOKEN'
          )
        ]) {
          sh '''
            set -e

            echo "$DOCKERHUB_TOKEN" |
              docker login \
                -u "$DOCKERHUB_USER" \
                --password-stdin

            docker push "${IMAGE}:${GIT_SHA}"

            echo "Da push image: ${IMAGE}:${GIT_SHA}"
          '''
        }
      }
    }

    stage('Update Helm Tag') {
      when {
        expression {
          env.SKIP_PIPELINE != 'true'
        }
      }

      steps {
        sh '''
          set -e

          export GIT_SHA

          echo "Cap nhat image tag thanh: ${GIT_SHA}"

          yq -i \
            '.image.tag = strenv(GIT_SHA)' \
            "${VALUES_FILE}"

          echo "Noi dung image trong values.yaml:"
          yq '.image' "${VALUES_FILE}"

          echo "Git diff:"
          git diff -- "${VALUES_FILE}"
        '''
      }
    }

    stage('Commit GitOps Change') {
      when {
        expression {
          env.SKIP_PIPELINE != 'true'
        }
      }

      steps {
        withCredentials([
          usernamePassword(
            credentialsId: 'github-creds',
            usernameVariable: 'GITHUB_USER',
            passwordVariable: 'GITHUB_TOKEN'
          )
        ]) {
          sh '''
            set -e

            git config user.name "jenkins-bot"
            git config user.email "jenkins-bot@local.lab"

            git add "${VALUES_FILE}"

            if git diff --cached --quiet; then
              echo "image.tag khong thay doi."
              echo "Khong can commit va push."
              exit 0
            fi

            git commit \
              -m "ci: deploy ${GIT_SHA} [skip ci]"

            AUTHENTICATED_REPO="https://${GITHUB_USER}:${GITHUB_TOKEN}@${GITHUB_REPO}"

            echo "Lay thay doi moi nhat tu nhanh main..."

            git pull --rebase \
              "$AUTHENTICATED_REPO" \
              main

            echo "Push Helm tag moi len GitHub..."

            git push \
              "$AUTHENTICATED_REPO" \
              HEAD:main

            echo "Da cap nhat GitOps thanh cong."
          '''
        }
      }
    }
  }

  post {
    success {
      echo """
Pipeline thanh cong.

Docker image:
${IMAGE}:${GIT_SHA}

Helm values:
${VALUES_FILE}
"""
    }

    failure {
      echo 'Pipeline that bai. Hay kiem tra stage mau do.'
    }

    always {
      sh '''
        docker logout || true
      '''

      cleanWs(
        deleteDirs: true,
        notFailBuild: true
      )
    }
  }
}