#!/bin/bash
# Rasterise les SVG de _tools/svg vers Textures/ManyHappyReturns.
# Chrome headless sert de rasteriseur : aucun rasteriseur SVG n'est installe sur la machine.
set -e
cd "$(dirname "$0")/.."
CH="/c/Program Files/Google/Chrome/Application/chrome.exe"
B="$(pwd -W 2>/dev/null || pwd)"
mkdir -p Textures/ManyHappyReturns
for f in _tools/svg/*.svg; do
  n=$(basename "$f" .svg)
  "$CH" --headless --no-sandbox --disable-gpu --hide-scrollbars --window-size=64,64 \
    --default-background-color=00000000 \
    --screenshot="$B/Textures/ManyHappyReturns/$n.png" "file:///$B/_tools/svg/$n.svg" >/dev/null 2>&1
done
ls -l Textures/ManyHappyReturns
