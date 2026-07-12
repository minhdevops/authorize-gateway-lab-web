{{/*
Tên ngắn của ứng dụng
*/}}
{{- define "authorize-gateway-lab-web.name" -}}
{{- default .Chart.Name .Values.nameOverride | trunc 63 | trimSuffix "-" }}
{{- end }}

{{/*
Tên đầy đủ của resource.
Sử dụng releaseName từ ArgoCD.
*/}}
{{- define "authorize-gateway-lab-web.fullname" -}}
{{- .Release.Name | trunc 63 | trimSuffix "-" }}
{{- end }}

{{/*
Các label dùng chung
*/}}
{{- define "authorize-gateway-lab-web.labels" -}}
helm.sh/chart: {{ .Chart.Name }}-{{ .Chart.Version | replace "+" "_" }}
app.kubernetes.io/name: {{ include "authorize-gateway-lab-web.name" . }}
app.kubernetes.io/instance: {{ .Release.Name }}
app.kubernetes.io/version: {{ .Chart.AppVersion | quote }}
app.kubernetes.io/managed-by: {{ .Release.Service }}
{{- end }}

{{/*
Label dùng cho selector
*/}}
{{- define "authorize-gateway-lab-web.selectorLabels" -}}
app.kubernetes.io/name: {{ include "authorize-gateway-lab-web.name" . }}
app.kubernetes.io/instance: {{ .Release.Name }}
{{- end }}