// documented mode: the wrapper plugin decides the release (the version given to the workflow) and reads the
// GitHub release notes from CHANGELOG.md and the Steam change note from PUBLICATION.md, so no commit-analyzer or
// release-notes-generator and no feat:/fix: commit are needed.
const plugins = [
  '@semantic-release/github',
  ['./release-steam-plugin.mjs', { documented: true, appId: '294100', branchTargets: { "main": 'stable' }, mods: [{ name: "Many Happy Returns", path: 'Mod', workshopIds: { stable: "3806762201" } }] }],
];
export default { branches: ["main"], plugins };
