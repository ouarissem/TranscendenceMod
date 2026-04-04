using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Tiles;

namespace TranscendenceMod.Items.Materials
{
    public class HardmetalBar : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 10;
            Item.value = Item.sellPrice(silver: 20);
            Item.rare = ItemRarityID.Blue;
            Item.maxStack = 9999;

            Item.useAnimation = 10;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.consumable = true;
            Item.autoReuse = true;
            Item.createTile = ModContent.TileType<HardmetalBarTile>();
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<HardmetalOre>(), 8)
            .AddIngredient(ItemID.Leather)
            .AddTile(TileID.Anvils)
            .Register();
        }
    }
}
