using SimpleConstructor.Content.Items.Bases;
using Terraria.ID;

namespace SimpleConstructor.Content.Items
{
    public class ArenaConstructor : ArenaConstructorBase
    {
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.DirtBlock, 1)
                .Register();
        }
    }
}