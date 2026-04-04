using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Buffs.Items.Potions;

namespace TranscendenceMod.Items.Consumables.FoodAndDrinks
{
    public class ExtraTerrestrialBrew : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 20;
        }
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.ArcheryPotion);
            Item.buffTime = 3 * 60 * 60;
            Item.buffType = ModContent.BuffType<ExtraTerrestrialBuff>();
            Item.width = 16;
            Item.height = 16;
            Item.value = Item.sellPrice(silver: 75);
            Item.rare = ItemRarityID.LightPurple;
        }
    }
}
