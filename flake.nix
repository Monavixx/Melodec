{
  description = "melodec";

  inputs = {
    nixpkgs.url = "github:NixOS/nixpkgs/nixos-unstable";
    flake-utils.url = "github:numtide/flake-utils";
  };

  outputs =
    {
      self,
      nixpkgs,
      flake-utils,
      ...
    }@inputs:
    let
      homeManagerModules = rec {
        melodec =
          { pkgs, ... }@args:
          import ./modules/melodec.nix (
            args
            // {
              melodecPackage = self.packages.${pkgs.system}.melodec;
            }
          );
        default = melodec;
      };
    in
    (flake-utils.lib.eachDefaultSystem (
      system:
      let
        pkgs = import nixpkgs { inherit system; };

        dotnet = pkgs.dotnetCorePackages.sdk_10_0;

        run = pkgs.writeShellScriptBin "run" ''
          #!/usr/bin/env bash
          set -euo pipefail
          dotnet run --project "melodec-tool.csproj"
        '';
      in
      {
        devShells.default = pkgs.mkShell {
          name = "dotnet-dev";

          packages = [
            dotnet
            run
          ];

          DOTNET_ROOT = "${dotnet}";
          DOTNET_CLI_TELEMETRY_OPTOUT = "1";
          DOTNET_NOLOGO = "1";

          LD_LIBRARY_PATH = pkgs.lib.makeLibraryPath [
            pkgs.zlib
            pkgs.openssl
            pkgs.icu
          ];

          shellHook = ''
            export TZDIR="${pkgs.tzdata}/share/zoneinfo"
            export NUGET_PACKAGES="''${DIRENV_DIR:-$PWD}/.nuget-packages"
            docker compose up -d
            echo "dotnet dev shell ready"
            echo "  dotnet:  $(dotnet --version)"
          '';
        };

        packages.melodec = pkgs.callPackage ./pkgs/melodec.nix { inherit inputs pkgs; };
        packages.default = self.packages.${system}.melodec;
      }
    ))
    // {
      inherit homeManagerModules;
    };
}
