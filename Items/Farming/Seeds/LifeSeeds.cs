using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Tiles.TilesheetHell.Nature.Farming;

namespace TranscendenceMod.Items.Farming.Seeds
{
    public class LifeSeeds : BaseSeed
    {
        public override int Tile => ModContent.TileType<LifeCrop>();
        public override bool allowed(Tile tile2) => tile2.TileType == ModContent.TileType<Soil>() || tile2.TileType == ModContent.TileType<StarSoil>();

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.value = Item.sellPrice(gold: 1);
            Item.rare = ItemRarityID.Lime;
        }
        public override void AddRecipes()
        {
            CreateRecipe(2)
            .AddIngredient(ItemID.LifeFruit)
            .AddIngredient(ItemID.JungleSpores, 6)
            .AddIngredient(ItemID.PixieDust, 6)
            .AddIngredient(ItemID.ChlorophyteOre, 8)
            .AddTile(TileID.Blendomatic)
            .DisableDecraft()
            .Register();
        }
    }
}
