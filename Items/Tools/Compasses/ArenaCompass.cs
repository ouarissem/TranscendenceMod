using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Tiles;
using TranscendenceMod.Tiles.BigTiles;
using TranscendenceMod.Tiles.TilesheetHell.Nature;
using static TranscendenceMod.TranscendenceWorld;

namespace TranscendenceMod.Items.Tools.Compasses
{
    public class ArenaCompass : Compass
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Orange;
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

                        if (tile.TileType == ModContent.TileType<ProtectorStoneTile>() && !Downed.Contains(Bosses.FlameGuardian) ||
                            Downed.Contains(Bosses.FlameGuardian) && tile.TileType == TileID.Containers && tile.TileFrameX == 144)
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

                        if (tile.TileType == ModContent.TileType<ProtectorStoneTile>() && !Downed.Contains(Bosses.FlameGuardian) ||
                            Downed.Contains(Bosses.FlameGuardian) && tile.TileType == TileID.Containers && tile.TileFrameX == 144)
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
            .AddIngredient(ItemID.HellstoneBar, 12)
            .AddIngredient(ModContent.ItemType<CarbonOre>(), 12)
            .AddIngredient(ItemID.AshWood, 24)
            .AddTile(TileID.Hellforge)
            .Register();
        }
    }
}
