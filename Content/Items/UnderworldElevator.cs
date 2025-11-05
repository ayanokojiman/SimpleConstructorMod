using Terraria.ID;
using SimpleConstructor.Content.Items.Bases;


namespace SimpleConstructor.Content.Items
{
    public class UnderworldElevator : ElevatorBase
    {
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.DirtBlock, 1)
                .Register();
        }

    }

}