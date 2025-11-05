using Terraria;
using Terraria.ID;

namespace SimpleConstructor.Utils
{
    public static class TileUtils
    {

        // A tile is considered to be protected from destruction if it cannot be destroyed by a bomb or is not a block
        public static bool IsTileProtected(this Tile tile)
        {
            if (tile == null) return true;
            switch (tile.TileType)
            {
                case TileID.DemonAltar:
                case TileID.ShadowOrbs:
                case TileID.Teleporter:
                case TileID.LunarBrick:
                case TileID.LihzahrdAltar:
                case TileID.BlueDungeonBrick:
                case TileID.GreenDungeonBrick:
                case TileID.PinkDungeonBrick:
                case TileID.LihzahrdBrick:
                case TileID.Cobalt:
                case TileID.Palladium:
                case TileID.Mythril:
                case TileID.Orichalcum:
                case TileID.Adamantite:
                case TileID.Titanium:
                case TileID.Chlorophyte:
                // --- Check 2: Inventory/Structure Tiles (Always Protected by default on TileType) Chests, etc. 
                case TileID.Containers:
                case TileID.Dressers:
                case TileID.TrashCan:
                case TileID.Mannequin:
                case TileID.Womannequin:
                case TileID.HatRack:
                    return true;
                default:
                    break;
            }

            return false;
        }
        public static bool PlaceTile(int x, int y, int tileId, bool forced = false, int styleId = 0)
        {
            Tile tile = Main.tile[x, y];
            bool canPlace = WorldGen.InWorld(x, y) && !IsTileProtected(tile);
            if (canPlace) return WorldGen.PlaceTile(x, y, tileId, false, forced, -1, styleId);
            return canPlace;
        }
    }
}