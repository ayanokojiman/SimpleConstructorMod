using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using SimpleConstructor.Utils;


namespace SimpleConstructor.Content.Items.Bases
{
    public abstract class ArenaConstructorBase : ModItem
    {

        protected virtual int PlatformTileID => TileID.Platforms;
        protected virtual int PlatformTileStyleID => 0;
        protected virtual int PlatformWidth => 50;
        protected virtual int MaxStack => 99;
        protected virtual int? StartXOverride => null;
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

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.WoodPlatform, 100)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
        public override bool AltFunctionUse(Player player) => true;
        public override bool? UseItem(Player player)
        {
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

            for (int x = 0; x < PlatformWidth; x++)
            {
                int currentX = startX + x;

                if (WorldGen.InWorld(currentX, mouseY) && !Framing.GetTileSafely(currentX, mouseY).HasTile)
                {
                    WorldGen.PlaceTile(currentX, mouseY, PlatformTileID, forced: true, style: PlatformTileStyleID);
                }
            }

            return true;
        }
    }
}