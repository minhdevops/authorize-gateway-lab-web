pipeline {
  agent any
  triggers { pollSCM('H/2 * * * *') }
  options {
    disableConcurrentBuilds()
    timestamps()
  }

  environment {
    IMAGE = 'dovanminh98/authorize-gateway-lab-web'
    VALUES_FILE = 'helm/values.yaml'
  }

  stages {
    stage('Checkout') {
      steps {
        checkout scm
        script {
          env.GIT_SHA = sh(script: 'git rev-parse --short=7 HEAD',
                           returnStdout: true).trim()
          env.SKIP_PIPELINE = (
            sh(script: "git log -1 --pretty=%B | grep -Fq '[skip ci]'",
               returnStatus: true) == 0
          ).toString()
        }
      }
    }

    stage('Validate') {
      when { expression { env.SKIP_PIPELINE != 'true' } }
      steps {
        sh '''
          test -f Dockerfile
          helm lint helm
          helm template authorize-gateway-lab-web helm             --namespace authorize-gateway-lab > /tmp/rendered.yaml
        '''
      }
    }

    stage('Build Image') {
      when { expression { env.SKIP_PIPELINE != 'true' } }
      steps { sh 'docker build -t ${IMAGE}:${GIT_SHA} .' }
    }
    stage('Push Image') {
      when { expression { env.SKIP_PIPELINE != 'true' } }
      steps {
        withCredentials([usernamePassword(
          credentialsId: 'dockerhub-creds',
          usernameVariable: 'DOCKERHUB_USER',
          passwordVariable: 'DOCKERHUB_TOKEN'
        )]) {
          sh '''
            echo "$DOCKERHUB_TOKEN" |               docker login -u "$DOCKERHUB_USER" --password-stdin
            docker push ${IMAGE}:${GIT_SHA}
          '''
        }
      }
    }

    stage('Update Helm Tag') {
      when { expression { env.SKIP_PIPELINE != 'true' } }
      steps {
        sh '''
          export GIT_SHA
          yq -i '.image.tag = strenv(GIT_SHA)' ${VALUES_FILE}
          git diff -- ${VALUES_FILE}
        '''
      }
    }

    stage('Commit GitOps Change') {
      when { expression { env.SKIP_PIPELINE != 'true' } }
      steps {
        withCredentials([string(
          credentialsId: 'github-token',
          variable: 'GITHUB_TOKEN'
        )]) {
          sh '''
            git config user.name "jenkins-bot"
            git config user.email "jenkins-bot@local.lab"
            git add ${VALUES_FILE}
            git commit -m "ci: deploy ${GIT_SHA} [skip ci]"
            git push               "https://x-access-token:${GITHUB_TOKEN}@github.com/minhdevops/authorize-gateway-lab-web.git"               HEAD:main
          '''
        }
      }
    }
  }

  post {
    always { sh 'docker logout || true' }
  }
}
