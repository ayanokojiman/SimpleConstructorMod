using Terraria.ID;
using SimpleConstructor.Content.Items.Bases;
using Terraria;

namespace SimpleConstructor.Content.Items
{
    public class WorldPlatform : ArenaConstructorBase
    {
        protected override int PlatformWidth => Main.maxTilesX;
        protected override int? StartXOverride => 0;
        protected override int MaxStack => 1;
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.DirtBlock, 1)
                .Register();
        }
    }
}