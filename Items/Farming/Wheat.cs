using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace TranscendenceMod.Items.Farming
{
    public class Wheat : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 30;
        }
        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 24;
            Item.value = Item.sellPrice(silver: 10);
            Item.rare = ItemRarityID.Blue;
            Item.maxStack = 9999;
        }
    }
}
