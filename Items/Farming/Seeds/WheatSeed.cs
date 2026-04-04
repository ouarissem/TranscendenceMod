using Terraria;
using Terraria.ID;
using Terraria.Map;
using Terraria.ModLoader;
using TranscendenceMod.Tiles.TilesheetHell.Nature.Farming;

namespace TranscendenceMod.Items.Farming.Seeds
{
    public class WheatSeed : BaseSeed
    {
        public override int Tile => ModContent.TileType<WheatCrop>();
        public override bool allowed(Tile tile2) => tile2.TileType == ModContent.TileType<Soil>() || tile2.TileType == ModContent.TileType<StarSoil>();

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.value = Item.sellPrice(silver: 5);
            Item.rare = ItemRarityID.White;
        }
    }
}
