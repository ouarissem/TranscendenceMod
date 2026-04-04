using Terraria;
using Terraria.ID;

namespace TranscendenceMod.Items.Accessories.Shields
{
    public class GiantShellOrange : BaseShield
    {
        public override int Cooldown => 90;

        public override int DefenseAmount => 7;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Green;
            Item.width = 24;
            Item.height = 24;
            Item.value = Item.sellPrice(gold: 3, silver: 75);
        }
        public override void UpdateEquip(Player player)
        {
            base.UpdateEquip(player);
            player.moveSpeed *= 0.75f;
            player.GetModPlayer<TranscendencePlayer>().OrangeShell = true;
        }
    }
}
