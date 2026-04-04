using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous.Rarities;

namespace TranscendenceMod.Items.Materials
{
    public class ApolloPiece : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 10;
        }

        public override void SetDefaults()
        {
            Item.width = 12;
            Item.height = 12;
            Item.value = Item.sellPrice(gold: 1);
            Item.rare = ModContent.RarityType<CosmicRarity>();
            Item.maxStack = 9999;
        }
        public override Color? GetAlpha(Color lightColor) => Color.White;
    }
}
