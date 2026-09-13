# Artwork

`Preview-source.png` and `ModIcon-source.png` are the preserved original illustrations.
The user preferred the original icon style and requested its restoration on 2026-09-13.
`ModIcon-selected.png` preserves that exact original 128 x 128 delivered PNG, including its
ribbon, gift and confetti. The build copies it unchanged. This explicit user choice overrides
the general no-text icon guideline. `ModIcon-text-free-source.png` is an unused alternative.

Exact built-in edit prompt:

> Edit target: the attached Many Happy Returns icon. Remove the entire bottom ribbon and all letters. Preserve the distinctive orange round winking mascot with ponytail, thick near-black outline and cake to its left. Simplify for a 128x128 RimWorld mod icon readable at 32 pixels: retain only the mascot and the birthday cake, remove gift box and scattered confetti; retain one small four-point sparkle. Plain near-black background, flat cel shading, no glow, no gradients, no text, no lettering, no ribbon, no frame, no extra characters. Recenter the mascot and cake to occupy most of the square with a modest margin. Output square icon.

`Preview.html` composes the original scene with the title, summary and version from About.xml.
It uses `preview-palette.json` as its only palette source: a warm stone/brown veil and ochre
secondary ink from the dominant materials; the accent amplifies the cool blue floor family to
separate it from the warm scene. No prefix, suffix, connecting word or secondary tag is needed
for the title Many Happy Returns. The secondary color is reserved rather than artificially
adding a tag. Segoe UI is used; the font is available on the rendering machine.

Run `node _tools/build-art.cjs` with playwright and sharp available to Node and Chrome installed
(or set CHROME_PATH). This composes the code-based overlay; it does not regenerate the scene.
It checks contrast over the entire title/summary bounding rectangles against a render without
text, plus contrast on the opaque badge. Current minima: 7.55:1 title, 8.46:1 summary,
10.62:1 badge. Native-size and 268-pixel previews were inspected without clipping, overlaps or
a concrete camera concern. Generated QA images and the numerical report live in `.build/art/`.

Installed assets: `Mod/About/ModIcon.png` (128 x 128, 23,544 bytes),
`Mod/About/Preview.png` (896 x 504, 693,827 bytes). The 20–30 KB icon guidance is not a
minimum file-size requirement; padding an already readable icon would serve no purpose.
