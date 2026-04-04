using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Net.Http.Headers;
using System.Reflection;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Drawing;
using Terraria.Graphics.Light;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.NPCs.Boss.Seraph;

namespace TranscendenceMod.Miscanellous.MiscSystems
{
    public class SeraphTileDrawingSystem : ModSystem
    {
        public static int PhaseThroughTimer;
        public static bool PhaseThrough => PhaseThroughTimer > 0 && !Main.LocalPlayer.dead;
        public override void Load()
        {
            //Draw boundaries
            On_Main.DrawBackground += On_Main_DrawBackground;

            //Collision
            On_Collision.WetCollision += On_Collision_WetCollision;
            On_Collision.WaterCollision += On_Collision_WaterCollision;
            On_Collision.LavaCollision += On_Collision_LavaCollision;
            On_Collision.DrownCollision += On_Collision_DrownCollision;
            On_Collision.TileCollision += On_Collision_TileCollision;
            On_Collision.SlopeCollision += On_Collision_SlopeCollision;
            On_Collision.StepUp += On_Collision_StepUp;
            On_Collision.StepDown += On_Collision_StepDown;
            On_Player.DryCollision += On_Player_DryCollision;
            On_Collision.StickyTiles += On_Collision_StickyTiles;
            On_Collision.SwitchTilesNew += On_Collision_SwitchTilesNew;
            On_Collision.SolidCollision_Vector2_int_int += On_Collision_SolidCollision_Vector2_int_int;
            On_Collision.SolidCollision_Vector2_int_int_bool += On_Collision_SolidCollision_Vector2_int_int_bool;
            On_Player.UpdateTouchingTiles += On_Player_UpdateTouchingTiles;
            On_WorldGen.SolidOrSlopedTile_int_int += On_WorldGen_SolidOrSlopedTile_int_int;
            On_WorldGen.SolidOrSlopedTile_Tile += On_WorldGen_SolidOrSlopedTile_Tile;

            //Drawing
            On_Main.DrawBackgroundBlackFill += On_Main_DrawBackgroundBlackFill;
            On_Main.DrawWires += On_Main_DrawWires;
            On_Main.DoDraw_Tiles_Solid += On_Main_DoDraw_Tiles_Solid;
            On_Main.DoDraw_Tiles_NonSolid += On_Main_DoDraw_Tiles_NonSolid;
            On_Main.DoDraw_WallsAndBlacks += On_Main_DoDraw_WallsAndBlacks;
            On_Main.DoDraw_Waterfalls += On_Main_DoDraw_Waterfalls;
            On_Main.DrawMouseOver += On_Main_DrawMouseOver;
            On_Main.DrawLiquid += On_Main_DrawLiquid;
            On_Main.DrawInterface_40_InteractItemIcon += On_Main_DrawInterface_40_InteractItemIcon;
            On_TileDrawing.Draw += On_TileDrawing_Draw;
            On_Player.CanSeeShimmerEffects += On_Player_CanSeeShimmerEffects;

            //Lighting
            On_Lighting.Brightness += On_Lighting_Brightness;
            On_LegacyLighting.GetColor += On_LegacyLighting_GetColor;
            On_LightMap.BlurPass += On_LightMap_BlurPass;
            On_TileLightScanner.ExportTo += On_TileLightScanner_ExportTo;

            //Player Actions
            On_Player.PlaceThing += On_Player_PlaceThing;
            On_Player.ItemCheck_UseMiningTools += On_Player_ItemCheck_UseMiningTools;
            On_Player.FloorVisuals += On_Player_FloorVisuals;
            On_Player.CanMoveForwardOnRope += On_Player_CanMoveForwardOnRope;
            On_Minecart.OnTrack += On_Minecart_OnTrack;
            On_Minecart.GetOnTrack += On_Minecart_GetOnTrack;
            On_Player.ApplyTouchDamage += On_Player_ApplyTouchDamage;
            On_Player.CanAcceptItemIntoInventory += On_Player_CanAcceptItemIntoInventory;

            //Misc Updates
            On_WorldGen.CheckPot += On_WorldGen_CheckPot;
            On_WorldGen.KillTile_DropItems += On_WorldGen_KillTile_DropItems;
            On_Rain.MakeRain += On_Rain_MakeRain;
            On_Main.UpdateAudio += On_Main_UpdateAudio;
            On_Main.ShouldNormalEventsBeAbleToStart += On_Main_ShouldNormalEventsBeAbleToStart;
            On_Main.UpdateTime_StartDay += On_Main_UpdateTime_StartDay;
            On_Main.UpdateTime_StartNight += On_Main_UpdateTime_StartNight;
            On_Main.IsTileSpelunkable_int_int_ushort_short_short += On_Main_IsTileSpelunkable_int_int_ushort_short_short;
            On_Main.IsTileBiomeSightable_int_int_ushort_short_short_refColor += On_Main_IsTileBiomeSightable_int_int_ushort_short_short_refColor;
            On_Projectile.CutTilesAt += On_Projectile_CutTilesAt;
            On_DoorOpeningHelper.LookForDoorsToOpen += On_DoorOpeningHelper_LookForDoorsToOpen;
        }

        private void On_Main_DoDraw_Tiles_NonSolid(On_Main.orig_DoDraw_Tiles_NonSolid orig, Main self)
        {
            if (!PhaseThrough)
                orig(self);
        }

        private bool On_WorldGen_SolidOrSlopedTile_Tile(On_WorldGen.orig_SolidOrSlopedTile_Tile orig, Tile tile)
        {
            if (PhaseThrough)
                return false;

            return orig(tile);
        }

        private bool On_WorldGen_SolidOrSlopedTile_int_int(On_WorldGen.orig_SolidOrSlopedTile_int_int orig, int x, int y)
        {
            if (PhaseThrough)
                return false;

            return orig(x, y);
        }

        private void On_Player_UpdateTouchingTiles(On_Player.orig_UpdateTouchingTiles orig, Player self)
        {
            if (!PhaseThrough)
                orig(self);
        }

        private void On_TileDrawing_Draw(On_TileDrawing.orig_Draw orig, TileDrawing self, bool solidLayer, bool forRenderTargets, bool intoRenderTargets, int waterStyleOverride)
        {
            if (!PhaseThrough)
                orig(self, solidLayer, forRenderTargets, intoRenderTargets, waterStyleOverride);
        }

        private void On_Projectile_CutTilesAt(On_Projectile.orig_CutTilesAt orig, Projectile self, Vector2 boxPosition, int boxWidth, int boxHeight)
        {
            if (!PhaseThrough)
                orig(self, boxPosition, boxWidth, boxHeight);
        }

        private void On_Main_DrawInterface_40_InteractItemIcon(On_Main.orig_DrawInterface_40_InteractItemIcon orig, Main self)
        {
            if (!PhaseThrough)
                orig(self);
        }

        private void On_Main_DrawLiquid(On_Main.orig_DrawLiquid orig, Main self, bool bg, int waterStyle, float Alpha, bool drawSinglePassLiquids)
        {
            //Hitler Dead
            if (!PhaseThrough)
                orig(self, bg, waterStyle, Alpha, drawSinglePassLiquids);
        }

        private void On_TileLightScanner_ExportTo(On_TileLightScanner.orig_ExportTo orig, TileLightScanner self, Rectangle area, LightMap outputMap, TileLightScannerOptions options)
        {
            if (!PhaseThrough)
                orig(self, area, outputMap, options);
        }

        private bool On_Player_CanAcceptItemIntoInventory(On_Player.orig_CanAcceptItemIntoInventory orig, Player self, Item item)
        {
            if (PhaseThrough)
                return ItemID.Sets.IsAPickup[item.type];
            else return orig(self, item);
        }

        private Vector2 On_Collision_StickyTiles(On_Collision.orig_StickyTiles orig, Vector2 Position, Vector2 Velocity, int Width, int Height)
        {
            if (PhaseThrough)
                return -Vector2.One;
            else return orig(Position, Velocity, Width, Height);
        }

        private bool On_Main_IsTileBiomeSightable_int_int_ushort_short_short_refColor(On_Main.orig_IsTileBiomeSightable_int_int_ushort_short_short_refColor orig, int tileX, int tileY, ushort type, short tileFrameX, short tileFrameY, ref Color sightColor)
        {
            if (PhaseThrough)
                return false;
            else return orig(tileX, tileY, type, tileFrameX, tileFrameY, ref sightColor);
        }

        private bool On_Main_IsTileSpelunkable_int_int_ushort_short_short(On_Main.orig_IsTileSpelunkable_int_int_ushort_short_short orig, int tileX, int tileY, ushort typeCache, short tileFrameX, short tileFrameY)
        {
            if (PhaseThrough)
                return false;   
            else return orig(tileX, tileY, typeCache, tileFrameX, tileFrameY);
        }

        private void On_LightMap_BlurPass(On_LightMap.orig_BlurPass orig, LightMap self)
        {
            if (!PhaseThrough)
                orig(self);
        }

        private void On_DoorOpeningHelper_LookForDoorsToOpen(On_DoorOpeningHelper.orig_LookForDoorsToOpen orig, DoorOpeningHelper self, Player player)
        {
            if (!PhaseThrough)
                orig(self, player);
        }

        private void On_Player_ApplyTouchDamage(On_Player.orig_ApplyTouchDamage orig, Player self, int tileId, int x, int y)
        {
            if (!PhaseThrough)
                orig(self, tileId, x, y);
        }

        private void On_Main_UpdateTime_StartNight(On_Main.orig_UpdateTime_StartNight orig, ref bool stopEvents)
        {
            if (PhaseThrough)
                stopEvents = true;

            orig(ref stopEvents);
        }

        private void On_Main_UpdateTime_StartDay(On_Main.orig_UpdateTime_StartDay orig, ref bool stopEvents)
        {
            if (PhaseThrough)
                stopEvents = true;
            
            orig(ref stopEvents);
        }

        private bool On_Main_ShouldNormalEventsBeAbleToStart(On_Main.orig_ShouldNormalEventsBeAbleToStart orig)
        {
            if (PhaseThrough)
                return false;
            else return orig();
        }

        private bool On_Minecart_GetOnTrack(On_Minecart.orig_GetOnTrack orig, int tileX, int tileY, ref Vector2 Position, int Width, int Height)
        {
            if (PhaseThrough)
                return false;
            else return orig(tileX, tileY, ref Position, Width, Height);
        }

        private bool On_Minecart_OnTrack(On_Minecart.orig_OnTrack orig, Vector2 Position, int Width, int Height)
        {
            if (PhaseThrough)
                return false;
            else return orig(Position, Width, Height);
        }

        private void On_WorldGen_KillTile_DropItems(On_WorldGen.orig_KillTile_DropItems orig, int x, int y, Tile tileCache, bool includeLargeObjectDrops, bool includeAllModdedLargeObjectDrops)
        {
            if (!PhaseThrough)
                orig(x, y, tileCache, includeLargeObjectDrops, includeAllModdedLargeObjectDrops);
        }

        private bool On_Player_CanMoveForwardOnRope(On_Player.orig_CanMoveForwardOnRope orig, Player self, int dir, int x, int y)
        {
            if (PhaseThrough)
                return false;
            else return orig(self, dir, x, y);
        }

        private bool On_Collision_SolidCollision_Vector2_int_int_bool(On_Collision.orig_SolidCollision_Vector2_int_int_bool orig, Vector2 Position, int Width, int Height, bool acceptTopSurfaces)
        {
            if (PhaseThrough)
                return false;
            else return orig(Position, Width, Height, acceptTopSurfaces);
        }

        private bool On_Collision_SolidCollision_Vector2_int_int(On_Collision.orig_SolidCollision_Vector2_int_int orig, Vector2 Position, int Width, int Height)
        {
            if (PhaseThrough)
                return false;
            else return orig(Position, Width, Height);
        }

        private bool On_Collision_SwitchTilesNew(On_Collision.orig_SwitchTilesNew orig, Collision self, Vector2 Position, int Width, int Height, Vector2 oldPosition, int objType)
        {
            if (PhaseThrough)
                return false;
            else return orig(self, Position, Width, Height, oldPosition, objType);
        }

        private Vector3 On_LegacyLighting_GetColor(On_LegacyLighting.orig_GetColor orig, LegacyLighting self, int x, int y)
        {
            if (PhaseThrough)
                return new Vector3(1f, 1f, 1f);
            else return orig(self, x, y);
        }

        private float On_Lighting_Brightness(On_Lighting.orig_Brightness orig, int x, int y)
        {
            if (PhaseThrough)
                return 1f;
            else return orig(x, y);
        }

        private bool On_Player_CanSeeShimmerEffects(On_Player.orig_CanSeeShimmerEffects orig, Player self)
        {
            if (PhaseThrough)
                return false;
            else return orig(self);
        }

        private void On_Rain_MakeRain(On_Rain.orig_MakeRain orig)
        {
            if (!PhaseThrough)
                orig();
        }

        private void On_WorldGen_CheckPot(On_WorldGen.orig_CheckPot orig, int i, int j, int type)
        {
            if (!PhaseThrough)
                orig(i, j, type);
        }

        private void On_Player_FloorVisuals(On_Player.orig_FloorVisuals orig, Player self, bool Falling)
        {
            if (!PhaseThrough)
                orig(self, Falling);
        }

        private void On_Collision_StepDown(On_Collision.orig_StepDown orig, ref Vector2 position, ref Vector2 velocity, int width, int height, ref float stepSpeed, ref float gfxOffY, int gravDir, bool waterWalk)
        {
            if (!PhaseThrough)
                gfxOffY = 0f;
            else
                orig(ref position, ref velocity, width, height, ref stepSpeed, ref gfxOffY, gravDir, waterWalk);
        }

        private void On_Collision_StepUp(On_Collision.orig_StepUp orig, ref Vector2 position, ref Vector2 velocity, int width, int height, ref float stepSpeed, ref float gfxOffY, int gravDir, bool holdsMatching, int specialChecksMode)
        {
            if (!PhaseThrough)
                orig(ref position, ref velocity, width, height, ref stepSpeed, ref gfxOffY, gravDir, holdsMatching, specialChecksMode);
        }

        private void On_Main_DoDraw_Waterfalls(On_Main.orig_DoDraw_Waterfalls orig, Main self)
        {
            if (!PhaseThrough)
                orig(self);
        }

        private void On_Main_DrawMouseOver(On_Main.orig_DrawMouseOver orig, Main self)
        {
            if (PhaseThrough)
                Main.signHover = -1;

            orig(self);
        }

        private void On_Main_UpdateAudio(On_Main.orig_UpdateAudio orig, Main self)
        {
            float av = Main.ambientVolume;

            if (PhaseThrough)
                Main.ambientVolume = 0f;

            orig(self);

            if (PhaseThrough)
                Main.ambientVolume = av;
        }

        private void On_Player_ItemCheck_UseMiningTools(On_Player.orig_ItemCheck_UseMiningTools orig, Player self, Item sItem)
        {
            if (!PhaseThrough)
                orig(self, sItem);
        }

        private void On_Player_PlaceThing(On_Player.orig_PlaceThing orig, Player self, ref Player.ItemCheckContext context)
        {
            if (!PhaseThrough)
                orig(self, ref context);
        }

        private void On_Main_DoDraw_WallsAndBlacks(On_Main.orig_DoDraw_WallsAndBlacks orig, Main self)
        {
            if (!PhaseThrough)
                orig(self);
        }

        /*private void On_Main_DoDraw_Tiles_NonSolid(On_Main.orig_DoDraw_Tiles_NonSolid orig, Main self)
        {
            if (!PhaseThrough)
                orig(self);
        }*/

        private void On_Main_DoDraw_Tiles_Solid(On_Main.orig_DoDraw_Tiles_Solid orig, Main self)
        {
            if (!PhaseThrough)
                orig(self);
        }

        private void On_Main_DrawWires(On_Main.orig_DrawWires orig, Main self)
        {
            if (!PhaseThrough)
                orig(self);
        }

        private void On_Main_DrawBackgroundBlackFill(On_Main.orig_DrawBackgroundBlackFill orig, Main self)
        {
            if (!PhaseThrough)
                orig(self);
        }

        private void On_Player_DryCollision(On_Player.orig_DryCollision orig, Player self, bool fallThrough, bool ignorePlats)
        {
            if (!PhaseThrough)
                orig(self, fallThrough, ignorePlats);
            else self.position += self.velocity;
        }

        private Vector4 On_Collision_SlopeCollision(On_Collision.orig_SlopeCollision orig, Vector2 Position, Vector2 Velocity, int Width, int Height, float gravity, bool fall)
        {
            if (PhaseThrough)
                return new Vector4(Position.X, Position.Y, Velocity.X, Velocity.Y);

            return orig(Position, Velocity, Width, Height, gravity, fall);
        }

        private Vector2 On_Collision_TileCollision(On_Collision.orig_TileCollision orig, Vector2 Position, Vector2 Velocity, int Width, int Height, bool fallThrough, bool fall2, int gravDir)
        {
            if (PhaseThrough)
                return Velocity;

            return orig(Position, Velocity, Width, Height, fallThrough, fall2, gravDir);
        }

        private bool On_Collision_DrownCollision(On_Collision.orig_DrownCollision orig, Vector2 Position, int Width, int Height, float gravDir, bool includeSlopes)
        {
            if (PhaseThrough)
                return false;

            return orig(Position, Width, Height, gravDir, includeSlopes);
        }

        private bool On_Collision_LavaCollision(On_Collision.orig_LavaCollision orig, Vector2 Position, int Width, int Height)
        {
            if (PhaseThrough)
                return false;

            return orig(Position, Width, Height);
        }

        private bool On_Collision_WetCollision(On_Collision.orig_WetCollision orig, Vector2 Position, int Width, int Height)
        {
            if (PhaseThrough)
            {
                Collision.honey = false;
                Collision.shimmer = false;

                return false;
            }

            return orig(Position, Width, Height);
        }

        private Vector2 On_Collision_WaterCollision(On_Collision.orig_WaterCollision orig, Vector2 Position, Vector2 Velocity, int Width, int Height, bool fallThrough, bool fall2, bool lavaWalk)
        {
            if (PhaseThrough)
                return Velocity;

            return orig(Position, Velocity, Width, Height, fallThrough, fall2, lavaWalk);
        }

        private void On_Main_DrawBackground(On_Main.orig_DrawBackground orig, Main self)
        {
            DrawSeraphBoundaries();
            if (!PhaseThrough)
                orig(self);
        }

        public void DrawSeraphBoundaries()
        {
            SpriteBatch spriteBatch = Main.spriteBatch;

            if (PhaseThrough)
            {
                Texture2D block = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/SeraphBoundary").Value;

                int sx = TranscendenceWorld.SpaceTempleX;
                Vector2 pos1 = new Vector2(sx - (398 * 16), 180 * 16);

                //X
                for (int i = 0; i < 820; i++)
                {
                    spriteBatch.Draw(block, pos1 + new Vector2(i * 16, 0) - Main.screenPosition, null, Color.White * 0.2f, 0f, block.Size() * 0.5f, 1f, SpriteEffects.None, 0);
                    spriteBatch.Draw(block, pos1 + new Vector2(i * 16, 340 * 16) - Main.screenPosition, null, Color.White * 0.2f, 0f, block.Size() * 0.5f, 1f, SpriteEffects.None, 0);
                }

                //Y
                for (int j = 0; j < 341; j++)
                {
                    spriteBatch.Draw(block, pos1 + new Vector2(0, j * 16) - Main.screenPosition, null, Color.White * 0.2f, 0f, block.Size() * 0.5f, 1f, SpriteEffects.None, 0);
                    spriteBatch.Draw(block, pos1 + new Vector2(820 * 16, j * 16) - Main.screenPosition, null, Color.White * 0.2f, 0, block.Size() * 0.5f, 1f, SpriteEffects.None, 0);
                }

            }
        }
    }
}