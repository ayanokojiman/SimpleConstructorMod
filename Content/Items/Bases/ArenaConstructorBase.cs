using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SimpleConstructor.Utils;


namespace SimpleConstructor.Content.Items.Bases
{
    public abstract class ArenaConstructorBase : ModItem
    {

        protected virtual int PlatformTileID => TileID.Platforms;
        protected virtual int PlatformTileStyleID => 0;
        protected virtual int PlatformWidth => 50;
        protected virtual int MaxStack => 99;
        protected virtual int LiquidTilesToClear => 10;
        protected virtual int? TopTilesToClear => null;
        protected virtual int? StartXOverride
        {
            get { return null; }
        }
        protected virtual int? StartYOverride
        {
            get { return null; }
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 30;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Dig;
            Item.maxStack = MaxStack;
            Item.consumable = true;
        }
        protected virtual bool CanUse(Player player)
        {
            return true;
        }
        // Overrides the default no-tile check  
        protected virtual bool CanPlace(Tile tile)
        {
            return false;
        }
        protected virtual void ClearTopTiles(int x, int y)
        {
            if (TopTilesToClear == null) return;

            for (int i = (int)(y - TopTilesToClear); i <= y; ++i)
            {
                Tile tile = Framing.GetTileSafely(x, i);
                if (tile == null) continue;

                if (CanPlace(tile))
                    WorldGen.KillTile(x, i, fail: false, effectOnly: false, noItem: true);
            }
        }
        private void ClearLiquids(int startX, int startY)
        {
            for (int y = startY - (TopTilesToClear ?? 0); y < startY + LiquidTilesToClear; y++)
            {
                Tile tile = Framing.GetTileSafely(startX, y);
                if (tile != null && tile.HasTile)
                {
                    tile.LiquidAmount = 0;
                    tile.SkipLiquid = true;
                    WorldGen.SquareTileFrame(startX, y);
                }
            }

        }
        protected virtual void BuildPlatform(int startX, int startY)
        {
            for (int x = 0; x < PlatformWidth; x++)
            {
                int currentX = startX + x;
                if (!WorldGen.InWorld(currentX, startY)) continue;

                Tile tile = Framing.GetTileSafely(currentX, startY);
                if (!tile.HasTile || CanPlace(tile))
                {
                    ClearLiquids(currentX, startY - 1);
                    ClearTopTiles(currentX, startY);
                    WorldGen.PlaceTile(currentX, startY, PlatformTileID, forced: true, style: PlatformTileStyleID);
                }
            }
        }
        public override bool? UseItem(Player player)
        {
            if (!CanUse(player))
            {
                return false;
            }
            // Only proceed on left-click (primary use)
            if (player.altFunctionUse == 2)
            {
                return null;
            }

            int mouseX = Player.tileTargetX;
            int mouseY = Player.tileTargetY;

            string dir = PlayerUtils.GetFacingDirection(player);
            if (dir == "neutral")
            {
                return false;
            }

            int startX = StartXOverride ?? (dir == "left" ? mouseX - PlatformWidth + 1 : mouseX);

            BuildPlatform(startX, StartYOverride ?? mouseY);

            return true;
        }
    }
}