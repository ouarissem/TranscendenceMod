using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Tiles.BigTiles;

namespace TranscendenceMod.Items.Consumables.Placeables
{
    public class OceationItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 12;
            Item.height = 26;
            Item.value = Item.sellPrice(gold: 5);
            Item.rare = ItemRarityID.Green;
            Item.maxStack = 9999;
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.consumable = true;
            Item.autoReuse = true;
            Item.createTile = ModContent.TileType<Oceation>();
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.CoralstoneBlock, 24)
            .AddIngredient(ItemID.Starfish, 12)
            .AddIngredient(ItemID.SharkFin, 8)
            .AddCondition(Condition.NearWater)
            .Register();
        }
    }
}
