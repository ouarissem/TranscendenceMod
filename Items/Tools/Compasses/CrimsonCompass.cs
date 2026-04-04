using Microsoft.CodeAnalysis.Operations;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace TranscendenceMod.Items.Tools.Compasses
{
    public class CrimsonCompass : Compass
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.LightRed;
        }
        public bool corruptTile(Tile tile)
        {
            return tile.TileType == TileID.CrimsonGrass || tile.TileType == TileID.Crimsand || tile.TileType == TileID.Crimstone || tile.TileType == TileID.FleshIce || tile.TileType == TileID.CrimsonVines ||
                tile.TileType == TileID.CrimsonHardenedSand || tile.TileType == TileID.CrimsonJungleGrass || tile.TileType == TileID.CrimsonSandstone || tile.TileType == TileID.CrimsonThorns || tile.TileType == TileID.CrimsonPlants;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int x = (int)player.position.X / 16;
            int y = (int)player.position.Y / 16;
            int xDest = player.direction > 0 ? Main.maxTilesX - 55 : 55;
            bool found = false;

            if (player.direction == 1)
            {
                for (int i = x; i < xDest; i++)
                {
                    if (found)
                        break;

                    for (int j = 55; j < (Main.maxTilesY - 55); j++)
                    {
                        Tile tile = Main.tile[i, j];

                        if (tile.HasTile && corruptTile(tile))
                        {
                            Pos = new Vector2(i, j) * 16;
                            found = true;
                        }
                    }
                }
            }
            else
            {
                for (int i = x; i > xDest; i--)
                {
                    if (found)
                        break;

                    for (int j = 55; j < (Main.maxTilesY - 55); j++)
                    {
                        Tile tile = Main.tile[i, j];

                        if (tile.HasTile && corruptTile(tile))
                        {
                            Pos = new Vector2(i, j) * 16;
                            found = true;
                        }
                    }
                }
            }
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddRecipeGroup(RecipeGroupID.IronBar, 8)
            .AddIngredient(ItemID.CrimstoneBlock, 50)
            .AddIngredient(ItemID.CrimtaneBar, 12)
            .AddIngredient(ItemID.CrimsonSeeds, 6)
            .AddTile(TileID.DemonAltar)
            .Register();
        }
    }
}
