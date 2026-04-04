using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous.Rarities;

namespace TranscendenceMod.Items.Accessories.Offensive
{
    public class Stargazer : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.damage = 85;
            Item.DamageType = DamageClass.Generic;

            Item.height = 26;
            Item.width = 24;
            Item.accessory = true;
            Item.value = Item.sellPrice(gold: 3, silver: 75);
            Item.rare = ItemRarityID.Green;
        }
        public override bool WeaponPrefix() => false;
        public override bool MagicPrefix() => false;
        public override bool RangedPrefix() => false;
        public override bool MeleePrefix() => false;
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<TranscendencePlayer>().Stargazer = true;
            player.GetModPlayer<TranscendencePlayer>().stargazerDamage = Item.damage;
        }
    }
}
