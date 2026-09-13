// Requires Node.js, playwright, sharp and a local Chromium installation.
// Composes the native HTML overlay and downsizes the already generated icon.
const fs = require('node:fs/promises');
const path = require('node:path');
const { pathToFileURL } = require('node:url');
const { chromium } = require('playwright');
const sharp = require('sharp');
const root = path.resolve(__dirname, '..');
const at = p => path.join(root, p);
const luminance = rgb => rgb.map(v => v/255).map(v => v <= .04045 ? v/12.92 : ((v+.055)/1.055)**2.4).reduce((s,v,i) => s+v*[.2126,.7152,.0722][i],0);
const color = hex => hex.slice(1).match(/../g).map(v => parseInt(v,16));
const contrast = (a,b) => (Math.max(a,b)+.05)/(Math.min(a,b)+.05);
(async () => {
  const out = at('.build/art');
  await fs.mkdir(out, {recursive:true});
  const palette = JSON.parse(await fs.readFile(at('Art/preview-palette.json'),'utf8'));
  const about = await fs.readFile(at('Mod/About/About.xml'),'utf8');
  const versions = [...about.match(/<supportedVersions>([\s\S]*?)<\/supportedVersions>/)[1].matchAll(/<li>([\d.]+)<\/li>/g)].map(m=>m[1]);
  const version = versions.sort((a,b)=>parseFloat(b)-parseFloat(a))[0];
  const browser = await chromium.launch({executablePath: process.env.CHROME_PATH || 'C:/Program Files/Google/Chrome/Application/chrome.exe', headless:true});
  try {
    const page = await browser.newPage({viewport:{width:896,height:504},deviceScaleFactor:1});
    await page.goto(pathToFileURL(at('Art/Preview.html')).href);
    await page.evaluate(async ({palette, version}) => {
      await window.configurePreview(palette,version);
      await Promise.all([...document.images].map(i => i.decode()));
    }, {palette,version});
    const layout = await page.evaluate(() => {
      const box = selector => { const r=document.querySelector(selector).getBoundingClientRect(); return {x:r.x,y:r.y,width:r.width,height:r.height}; };
      return {font: getComputedStyle(document.querySelector('h1')).fontFamily, segoeAvailable:document.fonts.check('52px "Segoe UI"'), title:box('h1'), summary:box('p'), version:box('.version')};
    });
    if (!layout.segoeAvailable) throw new Error('Segoe UI unavailable');
    if (layout.title.x+layout.title.width>792 || layout.summary.y+layout.summary.height>504) throw new Error('Copy overflows its safe area');
    await page.screenshot({path:path.join(out,'Preview.png')});
    await page.evaluate(() => document.querySelector('.copy').style.visibility='hidden');
    await page.screenshot({path:path.join(out,'Preview-background.png')});
    const bg = await sharp(path.join(out,'Preview-background.png')).removeAlpha().raw().toBuffer({resolveWithObject:true});
    const checks={};
    for (const name of ['title','summary']) {
      const r=layout[name]; let worst=Infinity;
      for(let y=Math.floor(r.y); y<Math.ceil(r.y+r.height);y++) for(let x=Math.floor(r.x);x<Math.ceil(r.x+r.width);x++) {
        const i=(y*bg.info.width+x)*bg.info.channels;
        worst=Math.min(worst,contrast(luminance(color(palette.inkPrimary)),luminance([...bg.data.subarray(i,i+3)])));
      }
      checks[name]=worst;
      if(worst<4.5) throw new Error(`${name} contrast ${worst} below 4.5`);
    }
    checks.badge=contrast(luminance(color(palette.accent)),luminance(color(palette.badgeInk)));
    if(checks.badge<4.5) throw new Error('Badge contrast below 4.5');
    await sharp(path.join(out,'Preview.png')).png({compressionLevel:9}).toFile(at('Mod/About/Preview.png'));
    await fs.copyFile(at('Art/ModIcon-selected.png'), at('Mod/About/ModIcon.png'));
    await sharp(at('Mod/About/Preview.png')).resize(268).toFile(path.join(out,'Preview-268.png'));
    await sharp(at('Mod/About/ModIcon.png')).resize(32,32).toFile(path.join(out,'ModIcon-32.png'));
    const sizes={preview:(await fs.stat(at('Mod/About/Preview.png'))).size,icon:(await fs.stat(at('Mod/About/ModIcon.png'))).size};
    if(sizes.preview>=1000000) throw new Error('Preview exceeds mandatory size limit');
    const report={version,paletteSource:'Art/preview-palette.json',layout,contrast:checks,bytes:sizes};
    await fs.writeFile(path.join(out,'report.json'),JSON.stringify(report,null,2)+'\n');
    console.log(JSON.stringify(report,null,2));
  } finally { await browser.close(); }
})().catch(e=>{console.error(e);process.exitCode=1;});
