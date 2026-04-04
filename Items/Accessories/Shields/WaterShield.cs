using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TranscendenceMod.Items.Accessories.Shields
{
    public class WaterShield : BaseShield
    {
        public override int Cooldown => 120;

        public override int DefenseAmount => 5;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Blue;
            Item.width = 24;
            Item.height = 30;
            Item.value = Item.sellPrice(gold: 2);
        }

        public override void AddRecipes()
        {
            Recipe iron = CreateRecipe();
            iron.AddIngredient(ItemID.Wood, 15);
            iron.AddIngredient(ItemID.IronBar, 10);
            iron.AddTile(TileID.Anvils);
            iron.Register();

            Recipe lead = CreateRecipe();
            lead.AddIngredient(ItemID.Wood, 15);
            lead.AddIngredient(ItemID.LeadBar, 10);
            lead.AddTile(TileID.Anvils);
            lead.Register();
        }
    }
}
