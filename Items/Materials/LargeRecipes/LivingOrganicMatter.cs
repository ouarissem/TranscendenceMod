using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Items.Materials.MobDrops;

namespace TranscendenceMod.Items.Materials.LargeRecipes
{
    public class LivingOrganicMatter : ModItem
    {
        public override void SetStaticDefaults() => CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 5;

        public override void SetDefaults()
        {
            Item.width = 25;
            Item.height = 25;
            Item.value = Item.sellPrice(gold: 8);
            Item.rare = ItemRarityID.Red;
            Item.maxStack = 9999;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.ChlorophyteBar, 12)
            .AddIngredient(ItemID.JungleSpores, 8)
            .AddIngredient(ItemID.BeeWax, 16)
            .AddIngredient(ItemID.Vine, 4)
            .AddIngredient(ModContent.ItemType<MosquitoVenom>(), 2)
            .AddIngredient(ItemID.PoopBlock, 40)
            .AddCondition(Condition.NearWater)
            .Register();
        }
    }
}
