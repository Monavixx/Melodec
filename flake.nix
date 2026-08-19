{
  description = "melodec";

  inputs = {
    nixpkgs.url = "github:NixOS/nixpkgs/nixos-unstable";
  };

  outputs =
    { self, nixpkgs, ... }:
    let
      system = "x86_64-linux";
      pkgs = import nixpkgs { inherit system; };
    in
    {
      packages.${system}.melodec = pkgs.callPackage ./pkgs/melodec.nix { };

      homeManagerModules.melodec =
        { pkgs, ... }@args:
        import ./modules/melodec.nix (
          args
          // {
            melodecPackage = self.packages.${pkgs.system}.melodec;
          }
        );

      homeManagerModules.default = self.homeManagerModules.melodec;
    };
}
