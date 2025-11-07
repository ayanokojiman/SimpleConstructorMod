using Terraria.ID;
using SimpleConstructor.Content.Items.Bases;
using Terraria;
using System;
using Microsoft.Xna.Framework;
using SimpleConstructor.Utils;

namespace SimpleConstructor.Content.Items
{
    public class HellishPlatform : ArenaConstructorBase
    {
        protected override int PlatformWidth => Main.maxTilesX;
        protected override int MaxStack => 1;
        protected override int? TopTilesToClear => 6;
        private const int MaxTilesYOffset = 200;
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.DirtBlock, 1)
                .Register();
        }
        protected override int? StartXOverride
        {
            get
            {
                return 0;
            }
        }
        protected override int? StartYOverride
        {
            get
            {
                Vector2 player = PlayerUtils.GetPlayerPos();
                return (int?)player.Y;
            }
        }
        protected override bool CanUse(Player player)
        {
            int centerY = (int)((player.position.Y + player.height) / 16f);
            if (Main.maxTilesY - MaxTilesYOffset > centerY)
            {
                Main.NewText("Hmm, I think you're not deep enough", Color.Yellow);
                return false;
            }

            return true;
        }
        protected override bool CanPlace(Tile tile)
        {
            return !TileUtils.IsTileProtected(tile);
        }
    }
}