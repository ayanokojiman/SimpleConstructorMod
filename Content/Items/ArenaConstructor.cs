using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SimpleConstructor.Utils;

namespace SimpleConstructor.Content.Items
{
    public class ArenaConstructor : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 30;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Dig;
            Item.autoReuse = true;
            Item.maxStack = 1;
            Item.consumable = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.WoodPlatform, 100)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
        private const int PlatformTileID = TileID.Platforms;
        private const int DesiredPlatformStyle = 0;

        public override bool AltFunctionUse(Player player) => true;

        public override bool? UseItem(Player player)
        {
            // Only proceed on left-click (primary use)
            if (player.altFunctionUse == 2)
            {
                return null;
            }




            int tileX = Player.tileTargetX;
            int tileY = Player.tileTargetY;

            const int platformWidth = 50;

            string dir = PlayerUtils.GetFacingDirection(player);
            if (dir == "neutral")
            {
                return false;
            }


            int startX = dir == "left" ? tileX - platformWidth + 1 : tileX;

            for (int x = 0; x < platformWidth; x++)
            {
                int currentX = startX + x;

                if (WorldGen.InWorld(currentX, tileY) && !Framing.GetTileSafely(currentX, tileY).HasTile)
                {
                    WorldGen.PlaceTile(currentX, tileY, PlatformTileID, forced: true, style: DesiredPlatformStyle);
                }
            }

            return true;
        }
    }
}