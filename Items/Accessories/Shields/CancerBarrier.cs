using Terraria;
using Terraria.ID;

namespace TranscendenceMod.Items.Accessories.Shields
{
    public class CancerBarrier : BaseShield
    {
        public override int Cooldown => 60;

        public override int DefenseAmount => -4;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Green;
            Item.width = 24;
            Item.height = 24;
            Item.value = Item.sellPrice(gold: 5);
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.DemoniteBar, 18)
            .AddIngredient(ItemID.ShadowScale, 24)
            .AddIngredient(ItemID.RottenChunk, 8)
            .AddTile(TileID.MythrilAnvil)
            .Register();
        }
    }
}