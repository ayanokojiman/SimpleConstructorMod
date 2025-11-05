using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SimpleConstructor.Utils;

namespace SimpleConstructor.Content.Items
{
    public class HellishPlatform : ModItem
    {
        public override void SetDefaults()
        {
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Dig;
            Item.consumable = false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.DirtBlock, 1)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
        private const int PlatformTileID = TileID.Platforms;
        private const int DesiredPlatformStyle = 0;

        public override bool AltFunctionUse(Player player) => true;

        public override bool? UseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                return null;
            }

            int tileX = Player.tileTargetX;
            int tileY = Player.tileTargetY;

            static bool TryPlacePlatformTile(int x, int y)
            {
                bool canPlace = WorldGen.InWorld(x, y);
                if (canPlace) WorldGen.PlaceTile(x, y, PlatformTileID, forced: true, style: DesiredPlatformStyle);
                return canPlace;
            }
            TryPlacePlatformTile(tileX, tileY);

            bool canPlace = true;
            int leftX = tileX;
            int rightX = tileX;
            while (canPlace)
            {
                leftX -= 1;
                rightX += 1;
                bool canPlaceLeft = TryPlacePlatformTile(leftX, tileY);
                bool canPlaceRight = TryPlacePlatformTile(rightX, tileY);
                canPlace = canPlaceLeft || canPlaceRight;
            }
            return true;
        }
    }
}