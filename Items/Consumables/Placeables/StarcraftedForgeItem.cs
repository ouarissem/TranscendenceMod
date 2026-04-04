using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Items.Consumables.Placeables.SpaceBiome;
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Miscannellous.Rarities;
using TranscendenceMod.Tiles.BigTiles;
using TranscendenceMod.Tiles.BigTiles.Furniture;

namespace TranscendenceMod.Items.Consumables.Placeables
{
    public class StarcraftedForgeItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 14;
            Item.height = 16;
            Item.value = Item.sellPrice(gold: 35);
            Item.rare = ModContent.RarityType<CosmicRarity>();
            Item.maxStack = 9999;
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.consumable = true;
            Item.autoReuse = true;
            Item.createTile = ModContent.TileType<StarcraftedForge>();
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.LunarCraftingStation)
            .AddRecipeGroup(nameof(ItemID.MythrilAnvil))
            .AddRecipeGroup(nameof(ItemID.AdamantiteForge))
            .AddIngredient(ModContent.ItemType<StarcraftedAlloy>(), 4)
            .AddTile(ModContent.TileType<ShimmerAltar>())
            .Register();
        }
    }
}
