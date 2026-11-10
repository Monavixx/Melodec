{
  pkgs ? import <nixpkgs> { },
  # inputs,
  ...
}:

pkgs.buildDotnetModule rec {
  pname = "Melodec";
  version = "1.0.0";

  src = ../src;
  projectFile = "Melodec.Cli/Melodec.Cli.csproj";

  nugetDeps = ../deps.json;

  dotnet-sdk = pkgs.dotnetCorePackages.sdk_10_0;
  dotnet-runtime = pkgs.dotnetCorePackages.runtime_10_0;
  nativeBuildInputs = [ pkgs.clang ];
  buildInputs = [ pkgs.zlib ];
  selfContainedBuild = true;
  dotnetFlags = [
    "-p:StripSymbols=false"
  ];
  runtimeDeps = [
    pkgs.ffmpeg
    pkgs.yt-dlp
  ];
}
