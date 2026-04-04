using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace TranscendenceMod.Items.Farming
{
    public class RedSpiderLily : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 14;
            Item.height = 22;
            Item.maxStack = 9999;

            Item.value = Item.sellPrice(silver: 50);
            Item.rare = ItemRarityID.Yellow;
        }
    }
}
