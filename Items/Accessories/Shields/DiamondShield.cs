using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TranscendenceMod.Items.Accessories.Shields
{
    [AutoloadEquip(EquipType.Shield)]
    public class DiamondShield : BaseShield
    {
        public override int Cooldown => 150;

        public override int DefenseAmount => 3;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Green;
            Item.width = 35;
            Item.height = 32;
            Item.value = Item.sellPrice(silver: 50);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<Shield>())
            .AddIngredient(ItemID.Diamond, 20)
            .AddTile(TileID.Anvils)
            .Register();
        }
    }
}
