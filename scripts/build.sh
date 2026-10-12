#!/bin/bash
# Rasterises the SVGs in scripts/svg into Mod/Textures/ManyHappyReturns.
# Chrome headless is the rasteriser: no SVG rasteriser is installed on this machine.
set -e
cd "$(dirname "$0")/.."
CH="/c/Program Files/Google/Chrome/Application/chrome.exe"
B="$(pwd -W 2>/dev/null || pwd)"
mkdir -p Mod/Textures/ManyHappyReturns
for f in scripts/svg/*.svg; do
  n=$(basename "$f" .svg)
  "$CH" --headless --no-sandbox --disable-gpu --hide-scrollbars --window-size=64,64 \
    --default-background-color=00000000 \
    --screenshot="$B/Mod/Textures/ManyHappyReturns/$n.png" "file:///$B/scripts/svg/$n.svg" >/dev/null 2>&1
done
ls -l Mod/Textures/ManyHappyReturns
