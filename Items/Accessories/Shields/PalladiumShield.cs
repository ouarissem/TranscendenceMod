using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TranscendenceMod.Items.Accessories.Shields
{
    [AutoloadEquip(EquipType.Shield)]
    public class PalladiumShield : BaseShield
    {
        public override int Cooldown => 120;

        public override int DefenseAmount => 6;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.LightRed;
            Item.width = 26;
            Item.height = 33;

            Item.damage = 85;
            Item.DamageType = DamageClass.Melee;

            Item.value = Item.sellPrice(gold: 7, silver: 25);
        }

        public override bool WeaponPrefix() => false;
        public override bool MeleePrefix() => false;

        public override void UpdateEquip(Player player)
        {
            base.UpdateEquip(player);
            player.GetModPlayer<TranscendencePlayer>().PalladiumShieldEquipped = true;
            player.GetModPlayer<TranscendencePlayer>().ShieldDamage = Item.damage;
        }
    }
}
