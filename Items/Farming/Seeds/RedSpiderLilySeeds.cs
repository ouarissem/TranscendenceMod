using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Tiles.TilesheetHell.Nature.Farming;

namespace TranscendenceMod.Items.Farming.Seeds
{
    public class RedSpiderLilySeeds : BaseSeed
    {
        public override int Tile => ModContent.TileType<LilyCrop>();
        public override bool allowed(Tile tile2) => tile2.TileType == ModContent.TileType<Soil>() || tile2.TileType == ModContent.TileType<StarSoil>();

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.value = Item.sellPrice(silver: 15);
            Item.rare = ItemRarityID.Yellow;
        }
        public override void AddRecipes()
        {
            CreateRecipe(6)
            .AddIngredient(ItemID.DeathweedSeeds, 6)
            .AddIngredient(ItemID.Ectoplasm, 3)
            .AddIngredient(ModContent.ItemType<Flour>(), 4)
            .AddIngredient(ModContent.ItemType<Potato>(), 24)
            .AddTile(TileID.Blendomatic)
            .Register();
        }
    }
}
