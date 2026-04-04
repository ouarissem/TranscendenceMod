using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using TranscendenceMod.Tiles.BigTiles.Rubbles;
using TranscendenceMod.Walls.Natural;
using TranscendenceMod.Tiles;
using System;
using TranscendenceMod.Items.Consumables.Placeables;
using TranscendenceMod.Items.Modifiers;
using System.Collections.Generic;
using TranscendenceMod.Tiles.TilesheetHell.Nature;
using Terraria.DataStructures;
using TranscendenceMod.NPCs.Passive;
using Steamworks;

namespace TranscendenceMod.Miscannellous
{
    public class CavinatorEX : GenPass
    {
        public CavinatorEX(string name, double loadWeight) : base(name, loadWeight)
        {
        }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = "Making Caves More Open";

            //Constant Wavy Caves
            double num956 = (double)Main.maxTilesX / 4200.0;
            num956 *= num956;
            int num957 = (int)(35.0 * num956);
            if (Main.remixWorld)
            {
                num957 /= 3;
            }
            int num958 = 0;
            int num959 = 80;
            for (int num960 = 0; num960 < num957; num960++)
            {
                double num961 = (double)num960 / (double)(num957 - 1);
                progress.Set(num961);
                int num962 = WorldGen.genRand.Next((int)Main.worldSurface + 100, Main.UnderworldLayer - 100);
                int num963 = 0;
                while (Math.Abs(num962 - num958) < num959)
                {
                    num963++;
                    if (num963 > 100)
                    {
                        break;
                    }
                    num962 = WorldGen.genRand.Next((int)Main.worldSurface + 100, Main.UnderworldLayer - 100);
                }
                num958 = num962;
                int num964 = 80;
                int startX = num964 + (int)((double)(Main.maxTilesX - num964 * 2) * num961);
                try
                {
                    WorldGen.WavyCaverer(startX, num962, 3f + WorldGen.genRand.Next(3, 6), 0.25 + WorldGen.genRand.NextDouble(), WorldGen.genRand.Next(300, 500), -1);
                }
                catch
                {
                }
            }

            for (int i = 0; i < Main.maxTilesX; i++)
            {
                for (int j = 0 + (int)(Main.maxTilesY * 0.4f); j < (int)(Main.maxTilesY * 0.25f); j++)
                {
                    if (Main.rand.NextBool(4)) WorldGen.Cavinator(i, j, 15);

                    if (Main.rand.NextBool(20))
                    {
                        for (int k = 0; k < 40; k++)
                        {
                            WorldGen.Cavinator(i, j + (k * 15), 15);
                        }
                    }
                }
            }
        }
    }
    public class CrateMagnets : GenPass
    {
        public CrateMagnets(string name, double loadWeight) : base(name, loadWeight)
        {
        }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = "Littering the Ocean";

            for (int i = 5; i < (Main.maxTilesX - 5); i++)
            {
                if (i < 400 || i > (Main.maxTilesX - 400))
                {
                    for (int j = 0; j < Main.maxTilesY / 3; j++)
                    {
                        Tile tile = Main.tile[i, j];
                        Tile tile2 = Main.tile[i, j + 1];
                        if (tile.LiquidAmount == 255 && tile2.TileType == TileID.Sand && tile2.HasTile && Main.rand.NextBool(40))
                        {
                            if (Main.rand.NextBool(2))
                                WorldGen.PlaceTile(i, j, ModContent.TileType<CrateMagnetTile>(), false, true);

                            else WorldGen.PlaceTile(i, j, ModContent.TileType<FishPendantTile>(), false, true);
                        }
                    }
                }
            }

            for (int index = 0; index < Main.maxChests; index++)
            {
                if (index != -1)
                {
                    Chest chest = Main.chest[index];
                    if (chest != null)
                    {
                        Tile chestTile = Main.tile[chest.x, chest.y];

                        if (chestTile.TileType == TileID.Containers && chestTile.TileFrameX == 17 * 36)
                        {
                            for (int invSlot = 0; invSlot < Chest.maxItems; invSlot++)
                            {
                                if (chest.item[invSlot].type == ItemID.None)
                                {
                                    if (Main.rand.NextBool(4))
                                        chest.item[invSlot].SetDefaults(ModContent.ItemType<OceationItem>());
                                    if (Main.rand.NextBool(3))
                                        chest.item[invSlot].SetDefaults(ModContent.ItemType<EnchantedOrb>());
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
    public class Ores : GenPass
    {
        public Ores(string name, double loadWeight) : base(name, loadWeight)
        {
        }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = "Volcanic Caves";

            //Carbon Ore
            for (int i = 200; i < (Main.maxTilesX - 200); i++)
            {
                for (int j = 0 + (int)(Main.maxTilesY / 3f); j < (Main.maxTilesY - 200); j++)
                {
                    Tile tile = Main.tile[i, j];
                    if ((tile.TileType == TileID.HardenedSand && Main.rand.NextBool(225)) && tile.HasTile)
                    {
                        WorldGen.OreRunner(i, j, 7, 22, (ushort)ModContent.TileType<CarbonOreTile>());
                    }
                }
            }

            //Volcanic Cave
            for (int i = 0; i < Main.maxTilesX; i++)
            {
                for (int j = Main.maxTilesY - 400; j < (Main.maxTilesY - 200); j++)
                {
                    Tile tile = Main.tile[i, j];
                    if (tile.TileType == TileID.Stone && tile.HasTile && !Main.rand.NextBool(4))
                    {
                        WorldGen.PlaceTile(i, j, ModContent.TileType<VolcanicStone>(), false, true);
                    }

                    //Turn soft blocks into ash
                    if ((tile.TileType == TileID.Dirt || tile.TileType == TileID.Silt || tile.TileType == TileID.Mud) && tile.HasTile)
                        WorldGen.PlaceTile(i, j, TileID.Ash, false, true);

                    //Turn grass into ash variants
                    if ((tile.TileType == TileID.Grass || tile.TileType == TileID.JungleGrass || tile.TileType == TileID.MushroomGrass) && tile.HasTile)
                        WorldUtils.Gen(new Point(i, j), new Shapes.Rectangle(1, 1), new Actions.SetTileKeepWall(TileID.AshGrass));

                    //Convert ores into Carbon
                    if ((tile.TileType == TileID.Copper || tile.TileType == TileID.Tin || tile.TileType == TileID.Tungsten || tile.TileType == TileID.Silver || tile.TileType == TileID.Iron
                        || tile.TileType == TileID.Lead || tile.TileType == TileID.Gold || tile.TileType == TileID.Platinum || tile.TileType == TileID.Demonite || tile.TileType == TileID.Crimtane) && tile.HasTile)
                    {
                        WorldGen.PlaceTile(i, j, ModContent.TileType<CarbonOreTile>(), false, true);
                    }

                    //Convert gems into Ruby
                    if ((tile.TileType == TileID.Emerald || tile.TileType == TileID.Topaz || tile.TileType == TileID.Diamond
                        || tile.TileType == TileID.Amethyst || tile.TileType == TileID.Sapphire) && tile.HasTile)
                    {
                        WorldGen.PlaceTile(i, j, TileID.Ruby, false, true);
                    }

                    //Hellstone veins
                    if (tile.TileType == ModContent.TileType<VolcanicStone>() && tile.HasTile && Main.rand.NextBool(120))
                        WorldGen.OreRunner(i, j, 4, 17, TileID.Hellstone);
                }
            }
        }
    }

    public class SpaceBiomeGenPass : GenPass
    {
        public SpaceBiomeGenPass(string name, double loadWeight) : base(name, loadWeight)
        {
        }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = "Revealing the Cosmos... Cosmic Islands";

            int sx = (int)(Main.maxTilesX / 3.75f);
            int sy = 110;
            int spy = 360;
            TranscendenceWorld.sx = sx;
            Point spacepoint = new Point(sx, sy);

            WorldUtils.Gen(new Point(sx - 275, spy - 275), new Shapes.Rectangle(550, 170), new Actions.Clear());

            //Generate islands
            for (int i = 0; i < 10; i++)
            {
                ShapeData shape = new ShapeData();


                WorldUtils.Gen(spacepoint, new Shapes.Circle(20, 6),
                    Actions.Chain(new GenAction[]
                    {
                        new Modifiers.Offset(-15 + (i * 30), i + (int)(Math.Sin(i) * 35)),
                        new Modifiers.Dither(0.2f),
                        new Actions.Blank().Output(shape)
                    }));
                
                WorldUtils.Gen(spacepoint, new Shapes.Circle(17, 5),
                    Actions.Chain(new GenAction[]
                    {
                        new Modifiers.Offset(-15 + (i * 30), i + (int)(Math.Sin(i) * 35)),
                        new Actions.Blank().Output(shape)
                    }));



                WorldUtils.Gen(spacepoint, new Shapes.Circle(20, 6),
                    Actions.Chain(new GenAction[]
                    {
                        new Modifiers.Offset(15 - (i * 30), i + (int)(Math.Sin(i) * 35)),
                        new Modifiers.Dither(0.2f),
                        new Actions.Blank().Output(shape)
                    }));
                
                WorldUtils.Gen(spacepoint, new Shapes.Circle(17, 5),
                    Actions.Chain(new GenAction[]
                    {
                        new Modifiers.Offset(15 - (i * 30), i + (int)(Math.Sin(i) * 35)),
                        new Actions.Blank().Output(shape)
                    }));
                


                WorldUtils.Gen(spacepoint, new Shapes.Circle(12, 3),
                    Actions.Chain(new GenAction[]
                    {
                        new Modifiers.Offset(25 + (i * 22), i - (int)(Math.Sin(i / 2f) * 45)),
                        new Modifiers.Dither(0.125f),
                        new Actions.Blank().Output(shape)
                    }));

                WorldUtils.Gen(spacepoint, new Shapes.Circle(12, 3),
                    Actions.Chain(new GenAction[]
                    {
                        new Modifiers.Offset(-25 - (i * 22), i - (int)(Math.Sin(i / 2f) * 45)),
                        new Modifiers.Dither(0.125f),
                        new Actions.Blank().Output(shape)
                    }));

                ushort rockTile = (ushort)ModContent.TileType<SpaceRock>();
                WorldUtils.Gen(new Point(spacepoint.X, spacepoint.Y), new ModShapes.All(shape), new Actions.SetTileKeepWall(rockTile));
            }



            for (int a = sx - 440; a < (sx + 440); a++)
            {
                for (int b = 5; b < (spy + 155); b++)
                {
                    if (Main.tile[a, b].TileType == TileID.Dirt && !Main.tile[a, b - 1].HasTile)
                    {
                        if (!Main.tile[a - 1, b].HasTile && !Main.tile[a + 1, b].HasTile)
                        {
                            Main.tile[a, b].ClearTile();
                            WorldGen.SquareTileFrame(a, b, true);
                            NetMessage.SendTileSquare(-1, a, b, 5);
                        }
                    }
                    if (Main.tile[a, b].TileType == ModContent.TileType<SpaceRock>() && !Main.tile[a, b - 1].HasTile)
                    {
                        //Place grass
                        WorldGen.PlaceTile(a, b - 1, (ushort)ModContent.TileType<SpaceRockGrass>());
                        if (Main.rand.NextBool(2))
                        {
                            WorldUtils.Gen(new Point(a, b), new Shapes.Rectangle(1, 1),
                                new Actions.SetTileKeepWall((ushort)ModContent.TileType<SpaceRockGrass>()));
                        }
                        WorldGen.SquareTileFrame(a, b, true);
                        NetMessage.SendTileSquare(-1, a, b, 5);
                    }
                }
            }

            for (int a = sx - 320; a < (sx - 40); a++)
            {
                for (int b = 10; b < 180; b++)
                {
                    if (Main.tile[a, b + 1].HasTile && Main.tile[a, b + 2].HasTile && !Main.tile[a, b - 2].HasTile && !Main.tile[a, b - 4].HasTile && !Main.tile[a, b - 6].HasTile && Main.tile[a, b].TileType == ModContent.TileType<SpaceRockGrass>() && Main.rand.NextBool(45))
                    {
                        int dir = Main.rand.NextFromList(-1, 1);
                        for (int e = 0; e < 15; e++)
                        {
                            WorldGen.PlaceTile(a + e * dir, b + 2 - 1 - e, ModContent.TileType<SpaceCrystal>(), true, true);
                            WorldGen.PlaceTile(a + e * dir, b + 2 - e, ModContent.TileType<SpaceCrystal>(), true, true);
                            WorldGen.PlaceTile(a + e * dir, b + 2 + 1 - e, ModContent.TileType<SpaceCrystal>(), true, true);
                            WorldGen.PlaceTile(a - (1 * dir) + e * dir, b + 2 + 3 - e, ModContent.TileType<SpaceCrystal>(), true, true);
                            WorldGen.PlaceTile(a - (2 * dir) + e * dir, b + 2 + 5 - e, ModContent.TileType<SpaceCrystal>(), true, true);
                            WorldGen.PlaceTile(a - (3 * dir) + e * dir, b + 2 + 7 - e, ModContent.TileType<SpaceCrystal>(), true, true);
                            WorldGen.PlaceTile(a - (4 * dir) + e * dir, b + 2 + 9 - e, ModContent.TileType<SpaceCrystal>(), true, true);
                        }
                    }

                    int am = 0;
                    for (int i = -6; i < 6; i++)
                    {
                        if (!Main.tile[a + i, b + i].HasTile)
                        {
                            am++;
                        }

                    }

                    if (am > 10 && Main.rand.NextBool(1350))
                    {
                        WorldUtils.Gen(new Point(a, b), new Shapes.Circle(5), Actions.Chain(new GenAction[]
                        {
                                new Modifiers.RadialDither(2, 4),
                                new Actions.PlaceTile((ushort)ModContent.TileType<ModMeteorite>())
                        }));
                    }

                    if (Main.tile[a, b].HasTile && Main.tile[a, b].TileType == ModContent.TileType<SpaceRock>())
                    {
                        if (Main.rand.NextBool(14) && Main.tile[a, b + 2].HasTile && Main.tile[a + 2, b + 2].HasTile && Main.tile[a - 2, b + 2].HasTile)
                        {
                            WorldUtils.Gen(new Point(a, b + 5), new Shapes.Circle(Main.rand.Next(4, 8)), new Actions.PlaceWall((ushort)ModContent.WallType<SpaceRockWall>()));
                        }
                        if (Main.rand.NextBool(20) && !Main.tile[a, b + 1].HasTile && !Main.tile[a, b + 2].HasTile && !Main.tile[a, b + 3].HasTile && !Main.tile[a, b + 4].HasTile
                            && Main.tile[a, b - 1].HasTile && Main.tile[a, b - 2].HasTile)
                        {
                            Dictionary<ushort, int> dictionary = new Dictionary<ushort, int>();
                            WorldUtils.Gen(new Point(a, b), new Shapes.Circle(30), new Actions.TileScanner((ushort)ModContent.TileType<AetherRoot>()).Output(dictionary));
                            int rootAmount = dictionary[(ushort)ModContent.TileType<AetherRoot>()];

                            if (rootAmount < 5)
                            {
                                for (int i = 0; i < 7; i++)
                                {
                                    int size = i > 4 ? 2 : 4;
                                    WorldUtils.Gen(new Point(a + Main.rand.Next(-2, 2), b + 2 + (i * (am - 1))), new Shapes.Circle(size - 1, size + 4), new Actions.PlaceTile((ushort)ModContent.TileType<AetherRoot>()));
                                }
                            }
                        }
                    }

                    if (Main.tile[a, b].TileType == ModContent.TileType<SpaceRockGrass>())
                    {
                        //Generate a big rubble if there are 3 Space Rocks below it
                        if (Main.tile[a - 1, b].HasTile && Main.tile[a + 1, b].HasTile && Main.rand.NextBool(4))
                        {
                            int rubble = Main.rand.NextBool(3) ? ModContent.TileType<SpaceBig01Natural>() : ModContent.TileType<SpaceBig02Natural>();
                            WorldGen.PlaceTile(a, b - 1, rubble, false, true);
                        }

                        //Generate a medium rubble if there are 2 Space Rocks below it
                        if ((Main.tile[a - 1, b].HasTile || Main.tile[a + 1, b].HasTile) && Main.rand.NextBool(6))
                        {
                            int rubble = Main.rand.NextBool(3) ? ModContent.TileType<SpaceMedium01Natural>() : ModContent.TileType<SpaceMedium02Natural>();
                            WorldGen.PlaceTile(a, b - 1, rubble, false, true);
                        }
                    }
                }
            }

            for (int a = sx + 40; a < (sx + 320); a++)
            {
                for (int b = 10; b < 180; b++)
                {
                    if (Main.tile[a, b + 1].HasTile && Main.tile[a, b + 2].HasTile && !Main.tile[a, b - 2].HasTile && !Main.tile[a, b - 4].HasTile && !Main.tile[a, b - 6].HasTile && Main.tile[a, b].TileType == ModContent.TileType<SpaceRockGrass>() && Main.rand.NextBool(45))
                    {
                        int dir = Main.rand.NextFromList(-1, 1);
                        for (int e = 0; e < 15; e++)
                        {
                            WorldGen.PlaceTile(a + e * dir, b + 2 - 1 - e, ModContent.TileType<SpaceCrystal>(), true, true);
                            WorldGen.PlaceTile(a + e * dir, b + 2 - e, ModContent.TileType<SpaceCrystal>(), true, true);
                            WorldGen.PlaceTile(a + e * dir, b + 2 + 1 - e, ModContent.TileType<SpaceCrystal>(), true, true);
                            WorldGen.PlaceTile(a - (1 * dir) + e * dir, b + 2 + 3 - e, ModContent.TileType<SpaceCrystal>(), true, true);
                            WorldGen.PlaceTile(a - (2 * dir) + e * dir, b + 2 + 5 - e, ModContent.TileType<SpaceCrystal>(), true, true);
                            WorldGen.PlaceTile(a - (3 * dir) + e * dir, b + 2 + 7 - e, ModContent.TileType<SpaceCrystal>(), true, true);
                            WorldGen.PlaceTile(a - (4 * dir) + e * dir, b + 2 + 9 - e, ModContent.TileType<SpaceCrystal>(), true, true);
                        }
                    }

                    int am = 0;
                    for (int i = -6; i < 6; i++)
                    {
                        if (!Main.tile[a + i, b + i].HasTile)
                        {
                            am++;
                        }

                    }

                    if (am > 10 && Main.rand.NextBool(1350))
                    {
                        WorldUtils.Gen(new Point(a, b), new Shapes.Circle(5), Actions.Chain(new GenAction[]
                        {
                                new Modifiers.RadialDither(2, 4),
                                new Actions.PlaceTile((ushort)ModContent.TileType<ModMeteorite>())
                        }));
                    }

                    if (Main.tile[a, b].HasTile && Main.tile[a, b].TileType == ModContent.TileType<SpaceRock>())
                    {
                        if (Main.rand.NextBool(14) && Main.tile[a, b + 2].HasTile && Main.tile[a + 2, b + 2].HasTile && Main.tile[a - 2, b + 2].HasTile)
                        {
                            WorldUtils.Gen(new Point(a, b + 5), new Shapes.Circle(Main.rand.Next(4, 8)), new Actions.PlaceWall((ushort)ModContent.WallType<SpaceRockWall>()));
                        }
                        if (Main.rand.NextBool(20) && !Main.tile[a, b + 1].HasTile && !Main.tile[a, b + 2].HasTile && !Main.tile[a, b + 3].HasTile && !Main.tile[a, b + 4].HasTile
                            && Main.tile[a, b - 1].HasTile && Main.tile[a, b - 2].HasTile)
                        {
                            Dictionary<ushort, int> dictionary = new Dictionary<ushort, int>();
                            WorldUtils.Gen(new Point(a, b), new Shapes.Circle(30), new Actions.TileScanner((ushort)ModContent.TileType<AetherRoot>()).Output(dictionary));
                            int rootAmount = dictionary[(ushort)ModContent.TileType<AetherRoot>()];

                            if (rootAmount < 5)
                            {
                                for (int i = 0; i < 7; i++)
                                {
                                    int size = i > 4 ? 2 : 4;
                                    WorldUtils.Gen(new Point(a + Main.rand.Next(-2, 2), b + 2 + (i * (am - 1))), new Shapes.Circle(size - 1, size + 4), new Actions.PlaceTile((ushort)ModContent.TileType<AetherRoot>()));
                                }
                            }
                        }
                    }
                    if (Main.tile[a, b].TileType == ModContent.TileType<SpaceRockGrass>())
                    {
                        //Generate a rubble if there are 3 Space Rocks below it
                        if (Main.tile[a - 1, b].HasTile && Main.tile[a + 1, b].HasTile && Main.rand.NextBool(6))
                        {
                            int rubble = Main.rand.NextBool(3) ? ModContent.TileType<SpaceBig01Natural>() : ModContent.TileType<SpaceMedium01Natural>();
                            WorldGen.PlaceTile(a, b - 1, rubble, false, true);
                        }
                    }
                }
            }
        }
    }
    public class Structures : GenPass
    {
        public bool PlacedSnowman;
        public Structures(string name, double loadWeight) : base(name, loadWeight)
        {
        }

        [JITWhenModsEnabled("StructureHelper")]
        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = "Creating Transcendence Structures";
            int sx = (int)(Main.maxTilesX / 3.75f);
            int sy = 135;

            // Church
            StructureHelper.API.Generator.GenerateStructure("Miscannellous/CosmicChurch", new Point16(sx - 30, (sy - 78) - 15), TranscendenceMod.Instance, false, true);

            int quarterX = (int)(Main.maxTilesX * 0.25f);
            int quarterY = (int)(Main.maxTilesY * 0.25f);

            // Flame Arena
            int x = WorldGen.genRand.Next(quarterX, Main.maxTilesX - quarterX);
            int y = WorldGen.genRand.Next(Main.maxTilesY - 380, Main.maxTilesY - 220);
            StructureHelper.API.Generator.GenerateStructure("Miscannellous/InfernoArena", new Point16(x, y), TranscendenceMod.Instance, false, false);

            for (int i = quarterX; i < (Main.maxTilesX - quarterX); i++)
            {
                // Snowman House
                if (!PlacedSnowman)
                {
                    for (int j = (int)(quarterY * 1.5f); j < (Main.maxTilesY - quarterY); j++)
                    {
                        int snowManCount = 0;
                        Tile tile = Main.tile[i, j];

                        if (tile.TileType == TileID.SnowBlock || tile.TileType == TileID.IceBlock)
                        {
                            Point point = new Point(i - 10, j - 7);
                            Ref<int> solidTiles = new Ref<int>(0);

                            WorldUtils.Gen(point, new Shapes.Rectangle(20, 14), Actions.Chain(new GenAction[]
                            {
                            new Actions.ContinueWrapper(Actions.Chain(new GenAction[]
                            {
                                new Modifiers.IsSolid(),
                                new Actions.Custom((i, j, args) => {
                                    snowManCount++;
                                    return true; }),
                                new Actions.Scanner(solidTiles)
                            }))
                            }));

                            if (snowManCount >= 70)
                            {
                                StructureHelper.API.Generator.GenerateStructure("Miscannellous/SnowmanHouse", new Point16(i, j), TranscendenceMod.Instance, false, false);

                                TranscendenceWorld.snowNPCpos = new Vector2(i + 7, j + 13).ToWorldCoordinates();
                                NPC.NewNPC(NPC.GetSource_None(), (int)TranscendenceWorld.snowNPCpos.X, (int)TranscendenceWorld.snowNPCpos.Y, ModContent.NPCType<SnowmanNPC>());

                                PlacedSnowman = true;
                                break;
                            }
                        }
                    }
                }

                
            }
        }
    }
    public class CosmicValleyGenPass : GenPass
    {
        public CosmicValleyGenPass(string name, double loadWeight) : base(name, loadWeight)
        {
        }
        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = "Revealing the Cosmos... Cosmic Valley";

            int sx = (int)(Main.maxTilesX / 3.75f);
            int spy = Main.maxTilesY >= 2000 ? 460 : 390;
            TranscendenceWorld.sx = sx;

            //Flattening ground in preparation for the forest below the Space Biome
            WorldUtils.Gen(new Point(sx - 400, 5), new Shapes.Rectangle(800, 490), new Actions.Clear());
            WorldUtils.Gen(new Point(sx - 440, spy), new Shapes.Rectangle(885, 180), new Actions.SetTile(TileID.Dirt));

            WorldUtils.Gen(new Point(sx - 394, spy), new Shapes.Rectangle(794, 120), new Actions.PlaceTile(TileID.Dirt));
            WorldUtils.Gen(new Point(sx - 250, spy - 150), new Shapes.Rectangle(500, 200), new Actions.Clear());

            WorldGen.SquareTileFrame(sx, spy);
            NetMessage.SendTileSquare(-1, sx, spy, 250);

            //Hills at the edge
            WorldUtils.Gen(new Point(sx + 385, spy), new Shapes.Slime(59), new Actions.SetTileKeepWall(TileID.Dirt));
            WorldUtils.Gen(new Point(sx - 380, spy), new Shapes.Slime(59), new Actions.SetTileKeepWall(TileID.Dirt));

            //The actual hole
            for (int r = 0; r < 12; r++)
            {
                float amount = 18;
                WorldUtils.Gen(new Point(sx + (350 - (r * 30)), (int)(spy - (50 - (r * (amount - (r * 1.25f)))))), new Shapes.Circle(25, 50), new Actions.Clear());
                WorldUtils.Gen(new Point(sx - (350 - (r * 30)), (int)(spy - (50 - (r * (amount - (r * 1.25)))))), new Shapes.Circle(25, 50), new Actions.Clear());
            }

            //Place rocks, seperated so that trees and life crystals don't get replaced
            for (int a = sx - 350; a < (sx + 350); a++)
            {
                for (int b = spy - 250; b < (spy + 155); b++)
                {
                    if (Main.tile[a, b].TileType == TileID.Dirt && !Main.tile[a, b - 1].HasTile)
                    {
                        if (Main.rand.NextBool(25) && Main.tile[a, b + 2].HasTile && Main.tile[a + 2, b + 1].HasTile && Main.tile[a - 2, b + 1].HasTile)
                        {
                            WorldUtils.Gen(new Point(a, b - 1), new Shapes.Circle(Main.rand.Next(4, 9)), new Actions.SetTileKeepWall(TileID.Stone));
                        }
                    }
                }
            }

            //Place water, seperated so that trees and life crystals don't get fucked up   EDIT: NOW they shouldn't fuck up EDIT: They still fuck up
            for (int a = sx - 240; a < (sx + 240); a++)
            {
                for (int b = spy - 250; b < (spy + 155); b++)
                {
                    if (Main.tile[a, b].TileType == TileID.Dirt && !Main.tile[a, b - 1].HasTile && !Main.tile[a, b - 1].CheckingLiquid)
                    {
                        if (Main.rand.NextBool(35) && Main.tile[a, b + 2].HasTile && Main.tile[a + 2, b + 1].HasTile && Main.tile[a - 2, b + 1].HasTile)
                        {
                            int rand = Main.rand.Next(7, 16);
                            WorldGen.digTunnel(a, b, 0.75f, 0.25f, 10, 7, true);
                            /*WorldUtils.Gen(new Point(a, b + 4), new Shapes.Circle(rand, 7), Actions.Chain(
                                new Modifiers.Dither(0.1f), new Actions.ClearTile()));
                            WorldUtils.Gen(new Point(a, b + 4), new Shapes.Circle(rand, 7), Actions.Chain(
                                new Modifiers.Dither(0.1f), new Actions.SetLiquid(LiquidID.Water)));*/
                        }
                    }
                }
            }


            for (int a = sx - 360; a < (sx + 360); a++)
            {
                for (int b = spy - 250; b < (spy + 155); b++)
                {
                    if (Main.tile[a, b].TileType == TileID.Stone && Main.tile[a, b - 1].LiquidAmount == 0 && !Main.tile[a + 1, b - 1].HasTile && !Main.tile[a - 1, b - 1].HasTile && !Main.tile[a, b - 1].HasTile)
                    {
                        if (Main.rand.NextBool(6)) WorldGen.AddBuriedChest(new Point(a, b - 1), 0, true, 0);
                        else if (Main.rand.NextBool(20)) WorldGen.AddLifeCrystal(a, b);
                        //WorldGen.PlaceTile(a, b - 1, TileID.Pots, true, false, -1, 1);
                    }

                    if (Main.tile[a, b + 1].TileType == TileID.Dirt && Main.tile[a, b].TileType == TileID.Grass && !Main.tile[a, b - 1].HasTile)
                    {
                        if (Main.tile[a - 1, b].HasTile && Main.tile[a + 1, b].HasTile && Main.rand.NextBool(8))
                        {
                            int rubble = Main.rand.NextBool(2) ? TileID.LargePiles2 : TileID.LargePiles;
                            WorldGen.PlaceTile(a, b - 1, rubble, false, true, -1, rubble == TileID.LargePiles2 ? Main.rand.Next(14, Main.rand.NextBool(2) ? 18 : 17) : Main.rand.Next(9, 13));
                        }

                        if (Main.rand.NextBool(2) && Main.tile[a, b + 2].HasTile && Main.tile[a + 3, b + 2].HasTile && Main.tile[a - 3, b + 2].HasTile)
                        {
                            WorldUtils.Gen(new Point(a, b + 5), new Shapes.Circle(Main.rand.Next(3, 11)), new Actions.PlaceWall(WallID.LivingLeaf));
                        }

                        if (Main.rand.NextBool(2) && !Main.tile[a + 1, b - 1].HasTile && !Main.tile[a - 1, b - 1].HasTile && !Main.tile[a, b].CheckingLiquid)
                            WorldGen.GrowTree(a, b);
                        if (Main.rand.NextBool(2) && !Main.tile[a + 1, b - 1].HasTile && !Main.tile[a - 1, b - 1].HasTile && !Main.tile[a, b].CheckingLiquid)
                            WorldGen.GrowEpicTree(a, b);
                        if (Main.rand.NextBool(4) && !Main.tile[a + 1, b - 1].HasTile && !Main.tile[a, b - 1].HasTile)
                            WorldGen.PoundTile(a, b);

                    }
                }
            }

            for (int a = sx - 360; a < (sx + 360); a++)
            {
                for (int b = spy - 250; b < (spy + 155); b++)
                {
                    //Unfuck flying chests and life crystals
                    if ((Main.tile[a, b].TileType == TileID.Containers && Main.tile[a, b].TileType == TileID.Heart) && !Main.tile[a, b + 3].HasTile)
                        WorldGen.KillTile(a, b);
                }
            }
        }
    }
}