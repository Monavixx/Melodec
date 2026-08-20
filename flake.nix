{
  description = "melodec";

  inputs = {
    nixpkgs.url = "github:NixOS/nixpkgs/nixos-unstable";
    # nuget-packageslock2nix = {
    #   url = "github:mdarocha/nuget-packageslock2nix";
    #   inputs.nixpkgs.follows = "nixpkgs";
    # };
  };

  outputs =
    { self, nixpkgs, ... }@inputs:
    let
      system = "x86_64-linux";
      pkgs = import nixpkgs { inherit system; };
    in
    {
      packages.${system}.melodec = pkgs.callPackage ./pkgs/melodec.nix { inherit inputs pkgs; };

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
