using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using TranscendenceMod.Tiles.BigTiles.Decorations;

namespace TranscendenceMod.Items.Consumables.Placeables.Decorations
{
    public class AncientCultist : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<Cultist>());
            Item.value = Item.sellPrice(silver: 10);
            Item.maxStack = 9999;
        }
    }
}
