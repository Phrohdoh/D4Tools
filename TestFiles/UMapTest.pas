{ Note: This directive exists for FPC (this could also be done with the -Mdelphi flag) }
{$MODE Delphi}

unit UMapTest;
  interface

    uses
      SysUtils,
      Classes
      ;

    var
      listOf, variables: integer;

    const MAX_X = 32;
    const MAX_Y = 32;

    type
      TMapTile = record
        TileType: word;
        W,
        X, Y: byte;
        procedure foo();
      end; // TMapTile

      TMap = class(TObject)
        private
          _width: word;
          _height: word;

        public
          property Width: word read _width;
          property Height: word read _height;

        private
          procedure _ChangeTile(x, y: byte; newTileType: word);

        public
          function GetTileType(x: byte; y: byte): word;
      end; // TMap

      TMyInteger = type Integer;

    var
      Map: TMap;

  implementation

    procedure TMap._ChangeTile(x, y: byte; newTileType: word);
    begin
      // Placeholder
    end; // TMap._ChangeTile

    function TMap.GetTileType(x: byte; y: byte): word;
    begin
      result := 8;

      if false then
        result := 10;
    end; // TMap.GetTileType

end. // UMapTest
