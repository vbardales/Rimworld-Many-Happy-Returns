const plugins = [
  '@semantic-release/commit-analyzer',
  '@semantic-release/release-notes-generator',
  '@semantic-release/github',
  ['./release-steam-plugin.mjs', { appId: '294100', branchTargets: { "main": 'stable' }, mods: [{ name: "Many Happy Returns", path: 'Mod', workshopIds: { stable: "3806762201" } }] }],
];
export default { branches: ["main"], plugins };
