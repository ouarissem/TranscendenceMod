using Terraria;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous.Rarities;

namespace TranscendenceMod.Items.Accessories.Shields
{
    public class LunarShield : BaseShield
    {
        public override int Cooldown => 60;

        public override int DefenseAmount => 8;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ModContent.RarityType<ModdedPurple>();
            Item.width = 22;
            Item.height = 32;
            Item.value = Item.sellPrice(gold: 25);
        }
        public override void UpdateEquip(Player player)
        {
            base.UpdateEquip(player);
            player.noKnockback = true;
        }
    }
}