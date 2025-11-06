using SimpleConstructor.Content.Items.Bases;
using Terraria.ID;

namespace SimpleConstructor.Content.Items
{
    public class ArenaConstructor : ArenaConstructorBase
    {

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.WoodPlatform, 100)
                .Register();
        }

    }
}