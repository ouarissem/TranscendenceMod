using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using TranscendenceMod.Items.Consumables.Placeables.SpaceBiome;
using TranscendenceMod.Miscannellous.Rarities;
using TranscendenceMod.Tiles.BigTiles;
using TranscendenceMod.Tiles.TerrestrialSecond;

namespace TranscendenceMod.Items.Consumables.Placeables.TerrestrialItems
{
    public class ExtraTerrestrialChairItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 12;
            Item.height = 18;
            Item.value = Item.sellPrice(silver: 5);
            Item.rare = ModContent.RarityType<ModdedPurple>();
            Item.DefaultToPlaceableTile(ModContent.TileType<ExtraTerrestrialChair>());
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<SpaceRockItem>(), 6)
            .AddTile(ModContent.TileType<ShimmerAltar>())
            .Register();
        }
    }
}
