using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous.Rarities;

namespace TranscendenceMod.Items
{
    public class StarPrize : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 17;
            Item.height = 17;

            Item.value = Item.sellPrice(gold: 5);
            Item.rare = ModContent.RarityType<CosmicRarity>();

        }
    }
}