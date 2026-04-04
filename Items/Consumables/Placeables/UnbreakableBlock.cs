using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Items.Materials.MobDrops;
using TranscendenceMod.Items.Weapons;
using TranscendenceMod.Tiles;

namespace TranscendenceMod.Items.Consumables.Placeables
{
    public class UnbreakableBlock : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.rare = ItemRarityID.Blue;

            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.useStyle = ItemUseStyleID.Swing;

            Item.useTurn = true;
            Item.autoReuse = true;
            Item.maxStack = 9999;

            Item.createTile = ModContent.TileType<UnbreakableBlockTile>();
            Item.consumable = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.StoneBlock, 25)
            .AddRecipeGroup(RecipeGroupID.IronBar, 4)
            .AddIngredient(ModContent.ItemType<PulverizedPlanet>(), 4)
            .AddIngredient(ItemID.FallenStar, 2)
            .AddTile(TileID.MythrilAnvil)
            .Register();
        }
    }
}
