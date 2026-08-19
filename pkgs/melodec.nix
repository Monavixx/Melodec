{
  pkgs ? import <nixpkgs> { },
  inputs,
  ...
}:

pkgs.buildDotnetModule rec {
  pname = "Melodec";
  version = "1.0.0";

  src = ../melodec-tool;

  # Point to your local placeholder file
  nugetDeps = inputs.nuget-packageslock2nix.lib {
    system = "x84_64-linux";
    name = pname;
    lockfiles = [ ../melodec-tool/packages.lock.json ];
  };

  dotnet-sdk = pkgs.dotnetCorePackages.sdk_10_0;
  dotnet-runtime = pkgs.dotnetCorePackages.runtime_10_0;
}
