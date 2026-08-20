{
  pkgs ? import <nixpkgs> { },
  # inputs,
  ...
}:

pkgs.buildDotnetModule rec {
  pname = "Melodec";
  version = "1.0.0";

  src = ../melodec-tool;
  projectFile = "../melodec-tool/melodec-tool.csproj";

  # Point to your local placeholder file
  # nugetDeps = inputs.nuget-packageslock2nix.lib {
  #   system = "x86_64-linux";
  #   name = pname;
  #   lockfiles = [ ../melodec-tool/packages.lock.json ];
  # };
  nugetDeps = ../melodec-tool/deps.json;

  dotnet-sdk = pkgs.dotnetCorePackages.sdk_10_0;
  dotnet-runtime = pkgs.dotnetCorePackages.runtime_10_0;
  nativeBuildInputs = [ pkgs.clang ];
  buildInputs = [ pkgs.zlib ];
  # executables = [ "melodec" ];
  selfContainedBuild = true;
}
