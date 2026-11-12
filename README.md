# Updating the nuget dependencies for nix:
* Step 1: generate the script if there's no one.
`nix build .#melodec.passthru.fetch-deps \[ -o .gcroots/fetch-deps \]`
* Step 2: run the generated script.
`./result` or `.gcroots/fetch-deps`

