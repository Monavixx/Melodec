{
  config,
  lib,
  melodecPackage,
  pkgs,
  ...
}:
let
  cfg = config.programs.melodec;
in
{
  options.programs.melodec = {
    enable = lib.mkEnableOption "Enable melodec";
    package = lib.mkOption {
      type = lib.types.package;
      default = melodecPackage;
    };
    musicPath = lib.mkOption {
      type = lib.types.path; # toString config.musicPath
      default = "${config.home.homeDirectory}/Music";
    };
    tracks = lib.mkOption {
      type = lib.types.listOf (
        lib.types.submodule {
          options = {
            url = lib.mkOption {
              type = lib.types.str;
            };
            author = lib.mkOption {
              type = lib.types.nullOr lib.types.str;
              default = null;
            };
            title = lib.mkOption {
              type = lib.types.nullOr lib.types.str;
              default = null;
            };
            filename = lib.mkOption {
              type = lib.types.nullOr lib.types.str;
              default = null;
              description = "Output filename without extension";
            };
          };
          default = [ ];
        }
      );
    };
    playlists = lib.mkOption {
      type = lib.types.listOf lib.types.submodule {
        options = {
          url = lib.mkOption {
            type = lib.types.str;
          };
          author = lib.mkOption {
            type = lib.types.nullOr lib.types.str;
            default = null;
          };
          title = lib.mkOption {
            type = lib.types.nullOr lib.types.str;
            default = null;
          };
          directoryName = lib.mkOption {
            type = lib.types.nullOr lib.types.str;
            default = null;
            description = "Playlist directory name";
          };
        };
      };
    };
  };
  config = lib.mkIf cfg.enable {
    home.packages = [ cfg.package ];
    xdg.configFile."melodec/config.json".source =
      let
        jsonFormat = pkgs.formats.json { };
        trackToJson =
          t:
          lib.filterAttrs (_: v: v != null) {
            url = t.url;
            author = t.author;
            title = t.title;
            filename = t.filename;
          };

        playlistToJson =
          p:
          lib.filterAttrs (_: v: v != null) {
            url = p.url;
            author = p.author;
            title = p.title;
            directory_name = p.directoryName;
          };
        configJson = {
          music_path = toString cfg.musicPath;
          tracks = map trackToJson cfg.tracks;
          playlists = map playlistToJson cfg.playlists;
        };
      in
      jsonFormat.generate "melodec-config.json" configJson;
  };
}
