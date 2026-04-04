using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TranscendenceMod.Items.Materials
{
    public class FlowerEssence : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.value = Item.sellPrice(silver: 50);
            Item.maxStack = 9999;
            Item.rare = ItemRarityID.Expert;
        }
        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            Lighting.AddLight(Item.Center, 0.3f, 0.125f, 0f);
            return base.PreDrawInWorld(spriteBatch, lightColor, alphaColor, ref rotation, ref scale, whoAmI);
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.Daybloom)
            .AddIngredient(ItemID.Moonglow)
            .AddIngredient(ItemID.Deathweed)
            .AddIngredient(ItemID.Waterleaf)
            .AddIngredient(ItemID.Shiverthorn)
            .AddIngredient(ItemID.Fireblossom)
            .AddIngredient(ModContent.ItemType<ShimmerBlossom>())
            .AddCondition(Condition.NearShimmer)
            .Register();
        }
    }
}
