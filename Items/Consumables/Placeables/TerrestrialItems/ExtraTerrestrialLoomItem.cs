using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Items.Consumables.Placeables.SpaceBiome;
using TranscendenceMod.Miscannellous.Rarities;
using TranscendenceMod.Tiles.BigTiles;
using TranscendenceMod.Tiles.TerrestrialSecond;

namespace TranscendenceMod.Items.Consumables.Placeables.TerrestrialItems
{
    public class ExtraTerrestrialLoomItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 16;
            Item.value = Item.sellPrice(silver: 5);
            Item.rare = ModContent.RarityType<ModdedPurple>();
            Item.maxStack = 9999;
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.consumable = true;
            Item.autoReuse = true;
            Item.createTile = ModContent.TileType<ExtraTerrestrialLoom>();
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<AetherRootItem>(), 12)
            .AddTile(ModContent.TileType<ShimmerAltar>())
            .Register();
        }
    }
}
