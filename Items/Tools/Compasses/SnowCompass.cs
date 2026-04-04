using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace TranscendenceMod.Items.Tools.Compasses
{
    public class SnowCompass : Compass
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Blue;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Pos = TranscendenceWorld.snowNPCpos;
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddRecipeGroup(RecipeGroupID.IronBar, 8)
            .AddIngredient(ItemID.SnowBlock, 50)
            .AddIngredient(ItemID.IceBlock, 25)
            .AddIngredient(ItemID.ShiverthornSeeds, 6)
            .AddTile(TileID.DemonAltar)
            .Register();
        }
    }
}
