using Terraria.ID;
using SimpleConstructor.Content.Items.Bases;
using Terraria;


namespace SimpleConstructor.Content.Items
{
    public class LuxuriousUnderworldElevator : ElevatorBase
    {
        protected override int TunnelWidth => 10;
        protected override int? TunnelWallID => 21;
        protected override int FloorStyleID => 29;
        protected override int LightSourceStyleID => 7;
        protected override int TunnelBlockID => 357;
        protected override bool PlaceRope => false;
        protected override int TunnelWallWidth => 3;
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.DirtBlock, 1)
                .Register();
        }

    }

}