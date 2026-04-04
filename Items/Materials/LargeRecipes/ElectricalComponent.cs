using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Items.Materials.MobDrops;
using TranscendenceMod.Miscannellous.Rarities;

namespace TranscendenceMod.Items.Materials.LargeRecipes
{
    public class ElectricalComponent : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 20;
        }
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 22;
            Item.value = Item.sellPrice(gold: 3, silver: 75);
            Item.rare = ModContent.RarityType<Brown>();
            Item.maxStack = 9999;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddRecipeGroup(nameof(ItemID.TitaniumBar), 12)
            .AddRecipeGroup(nameof(ItemID.CopperBar), 8)
            .AddIngredient(ItemID.SoulofSight, 4)
            .AddIngredient(ItemID.SoulofMight, 4)
            .AddIngredient(ItemID.SoulofFright, 4)
            .AddIngredient(ModContent.ItemType<SoulOfKnight>(), 4)
            .AddIngredient(ModContent.ItemType<Lightning>(), 4)
            .AddTile(TileID.MythrilAnvil)
            .Register();
        }
    }
}
