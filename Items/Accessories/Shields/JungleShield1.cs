using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TranscendenceMod.Items.Accessories.Shields
{
    public class JungleShield1 : BaseShield
    {
        public override int Cooldown => 120;

        public override int DefenseAmount => 6;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Lime;
            Item.width = 26;
            Item.height = 33;
            Item.value = Item.sellPrice(gold: 5, silver: 75);
        }
        public override void UpdateEquip(Player player)
        {
            base.UpdateEquip(player);
            player.moveSpeed *= 0.85f;
            player.GetModPlayer<TranscendencePlayer>().TurtleShield = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<Shield>())
            .AddIngredient(ItemID.ChlorophyteBar, 10)
            .AddIngredient(ItemID.TurtleShell, 3)
            .AddTile(TileID.MythrilAnvil)
            .Register();
        }
    }
}
