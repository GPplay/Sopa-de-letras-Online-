#!/bin/bash

# ========== CONFIG ==========

# Archivos específicos a eliminar del historial (puedes modificar esto)
FILES_TO_REMOVE=(
  "Sopa de letras/Logs/AssetImportWorker0.log"
  "Sopa de letras/Logs/AssetImportWorker1.log"
)

# Tu URL del repositorio remoto
REMOTE_URL="https://github.com/GPplay/Sopa-de-letras-Online-.git"

# Ruta al ejecutable de git-filter-repo (ajústala si es diferente)
GIT_FILTER_REPO="/c/Users/Gybram/AppData/Local/Packages/PythonSoftwareFoundation.Python.3.10_qbz5n2kfra8p0/LocalCache/local-packages/Python310/Scripts/git-filter-repo.exe"

# ========== PROCESO ==========

echo "🧹 Iniciando limpieza del historial de Git..."

# Construye la línea de paths para git-filter-repo
FILTER_PATHS=""
for file in "${FILES_TO_REMOVE[@]}"; do
  FILTER_PATHS+=" --path \"$file\""
done

# Ejecuta git-filter-repo
eval "$GIT_FILTER_REPO $FILTER_PATHS --invert-paths --force"

# Vuelve a agregar el remoto
echo "🔁 Restaurando remoto origin..."
git remote add origin "$REMOTE_URL"

# Confirmación para hacer push
echo "🚀 ¿Deseas hacer push --force al repositorio remoto ahora? (s/n)"
read -r PUSH_CONFIRM

if [[ "$PUSH_CONFIRM" == "s" || "$PUSH_CONFIRM" == "S" ]]; then
  git push origin master --force
  echo "✅ ¡Push forzado realizado!"
else
  echo "🛑 Push cancelado. Puedes hacerlo luego con:"
  echo "git push origin master --force"
fi
