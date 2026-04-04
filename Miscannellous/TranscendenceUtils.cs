using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using Terraria.ModLoader;

namespace TranscendenceMod.Miscannellous
{
    public class TranscendenceUtils
    {
        public static bool InsideInventory(Item item)
        {
            for (int i = 0; i < Main.LocalPlayer.inventory.Length; i++)
            {
                Item it = Main.LocalPlayer.inventory[i];
                if (it == item)
                    return true;
            }
            return false;
        }

        public static bool BossAlive()
        {
            return Main.CurrentFrameFlags.AnyActiveBossNPC || NPC.AnyNPCs(NPCID.EaterofWorldsHead) || NPC.AnyNPCs(NPCID.DD2Betsy) || NPC.AnyNPCs(NPCID.DD2EterniaCrystal);
        }

        public static int PlayerSentryCount(Player player)
        {
            int count = 0;
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile p = Main.projectile[i];
                if (p != null && p.active && p.owner == player.whoAmI && p.sentry)
                    count++;
            }

            return count;
        }

        public static bool IsADungeonWall(Tile tile)
        {
            return
                tile.WallType == WallID.BlueDungeonSlabUnsafe ||
                tile.WallType == WallID.BlueDungeonTileUnsafe ||
                tile.WallType == WallID.BlueDungeonUnsafe ||
                tile.WallType == WallID.GreenDungeonSlabUnsafe ||
                tile.WallType == WallID.GreenDungeonTileUnsafe ||
                tile.WallType == WallID.GreenDungeonUnsafe ||
                tile.WallType == WallID.PinkDungeonSlabUnsafe ||
                tile.WallType == WallID.PinkDungeonTileUnsafe ||
                tile.WallType == WallID.PinkDungeonUnsafe;
        }

        public static bool GeneralParryConditions(Player player)
        {
            player.TryGetModPlayer(out TranscendencePlayer mp);
            if (mp == null)
                return false;

            return mp.InsideShell == 0 && !mp.InsideGolem && (mp.HasParry && mp.ParryTimer > 0 || mp.LegendarySwordTimer > 0)
                && mp.ParryTimerCD > mp.ParryCD;
        }

        public static void DrawItemGlowmask(Item Item, float rotat, float size, string texture)
        {
            Texture2D GlowMask = ModContent.Request<Texture2D>($"{texture}_Glow").Value;
            Vector2 pos = new Vector2(Item.Center.X, Item.position.Y + Item.height - GlowMask.Height * 0.5f);
            Rectangle rec = new Rectangle(0, 0, GlowMask.Width, GlowMask.Height);
            Main.EntitySpriteDraw(GlowMask, pos - Main.screenPosition, rec, Color.White, rotat, GlowMask.Size() * 0.5f, size, SpriteEffects.None);
        }
        internal static void DrawItemGlowmask(Item Item, float rotat, float size, string texture, Vector2 pos2)
        {
            Texture2D GlowMask = ModContent.Request<Texture2D>($"{texture}_Glow").Value;
            Vector2 pos = new Vector2(Item.Center.X, Item.position.Y + Item.height - GlowMask.Height * 0.5f);
            Rectangle rec = new Rectangle(0, 0, GlowMask.Width, GlowMask.Height);
            Main.EntitySpriteDraw(GlowMask, pos + pos2 - Main.screenPosition, rec, Color.White, rotat, GlowMask.Size() * 0.5f, size, SpriteEffects.None);
        }
        public static void DrawTrailNPC(NPC npc, Color color, float size, string texture, bool SizeFadeOut, bool ColorFadeOut, float DistanceBetweenPoints, Vector2 extraPos)
        {
            Texture2D sprite = ModContent.Request<Texture2D>(texture).Value;
            Vector2 origin = new Vector2(sprite.Width * 0.5f, npc.height * 0.5f);

            for (int i = 0; i < (npc.oldPos.Length / DistanceBetweenPoints); i++)
            {
                Vector2 pos = npc.oldPos[(int)(i * DistanceBetweenPoints)] - Main.screenPosition + origin + new Vector2(0, npc.gfxOffY);
                float Fade = (npc.oldPos.Length - i) / (float)npc.oldPos.Length;

                if (SizeFadeOut) size *= Fade;
                if (ColorFadeOut) color *= Fade;

                Main.EntitySpriteDraw(sprite, pos + extraPos, npc.frame, color, npc.rotation, origin, size, SpriteEffects.None);
            }
        }
        public static void DrawTrailNPC(NPC npc, Color color, float size, string texture, bool SizeFadeOut, bool ColorFadeOut, float DistanceBetweenPoints, Vector2 extraPos, float rotation)
        {
            Texture2D sprite = ModContent.Request<Texture2D>(texture).Value;
            Vector2 origin = new Vector2(sprite.Width * 0.5f, npc.height * 0.5f);

            for (int i = 0; i < (npc.oldPos.Length / DistanceBetweenPoints); i++)
            {
                Vector2 pos = npc.oldPos[(int)(i * DistanceBetweenPoints)] - Main.screenPosition + origin + new Vector2(0, npc.gfxOffY);
                float Fade = (npc.oldPos.Length - i) / (float)npc.oldPos.Length;

                if (SizeFadeOut) size *= Fade;
                if (ColorFadeOut) color *= Fade;

                Main.EntitySpriteDraw(sprite, pos + extraPos, npc.frame, color, rotation, origin, size, SpriteEffects.None);
            }
        }
        public static void DrawTrailNPC(NPC npc, Color color, float size, string texture, bool SizeFadeOut, bool ColorFadeOut, float DistanceBetweenPoints, Vector2 extraPos, float extraRotation, bool useless)
        {
            Texture2D sprite = ModContent.Request<Texture2D>(texture).Value;
            Vector2 origin = new Vector2(sprite.Width * 0.5f, npc.height * 0.5f);

            for (int i = 0; i < (npc.oldPos.Length / DistanceBetweenPoints); i++)
            {
                Vector2 pos = npc.oldPos[(int)(i * DistanceBetweenPoints)] - Main.screenPosition + origin + new Vector2(0, npc.gfxOffY);
                float Fade = (npc.oldPos.Length - i) / (float)npc.oldPos.Length;

                if (SizeFadeOut) size *= Fade;
                if (ColorFadeOut) color *= Fade;

                Main.EntitySpriteDraw(sprite, pos + extraPos, npc.frame, color, npc.oldRot[(int)(i * DistanceBetweenPoints)] + extraRotation, origin, size, SpriteEffects.None);
            }
        }
        public static void DrawTrailNPC(NPC npc, Color color, float size, string texture, bool SizeFadeOut, bool ColorFadeOut, float DistanceBetweenPoints, Vector2 extraPos, SpriteEffects se)
        {
            Texture2D sprite = ModContent.Request<Texture2D>(texture).Value;
            Vector2 origin = npc.frame.Size() * 0.5f;

            for (int i = 0; i < (npc.oldPos.Length / DistanceBetweenPoints); i++)
            {
                Vector2 pos = npc.oldPos[(int)(i * DistanceBetweenPoints)] - Main.screenPosition + origin + new Vector2(0, npc.gfxOffY);
                float Fade = (npc.oldPos.Length - i) / (float)npc.oldPos.Length;

                if (SizeFadeOut) size *= Fade;
                if (ColorFadeOut) color *= Fade;

                Main.EntitySpriteDraw(sprite, pos + extraPos, npc.frame, color, npc.rotation, origin, size, se);
            }
        }
        public static void DrawTrailNPC(NPC npc, Color color, float size, string texture, bool SizeFadeOut, bool ColorFadeOut, float DistanceBetweenPoints, Vector2 extraPos, bool NoFrame)
        {
            Texture2D sprite = ModContent.Request<Texture2D>(texture).Value;
            Vector2 origin = sprite.Size() * 0.5f;

            for (int i = 0; i < (npc.oldPos.Length / DistanceBetweenPoints); i++)
            {
                Vector2 pos = npc.oldPos[(int)(i * DistanceBetweenPoints)] - Main.screenPosition + origin + new Vector2(0, npc.gfxOffY);
                float Fade = (npc.oldPos.Length - i) / (float)npc.oldPos.Length;

                if (SizeFadeOut) size *= Fade;
                if (ColorFadeOut) color *= Fade;

                Main.EntitySpriteDraw(sprite, pos + extraPos, null, color, npc.rotation, origin, size, SpriteEffects.None);
            }
        }
        public static void DrawTrailNPC(NPC npc, Color color, float size, string texture, bool SizeFadeOut, bool ColorFadeOut, float DistanceBetweenPoints, Vector2 extraPos, Vector2 origin, float rotation)
        {
            Texture2D sprite = ModContent.Request<Texture2D>(texture).Value;
            for (int i = 0; i < (npc.oldPos.Length / DistanceBetweenPoints); i++)
            {
                Vector2 pos = npc.oldPos[(int)(i * DistanceBetweenPoints)] - Main.screenPosition + new Vector2((float)(npc.width / 2));
                float Fade = (npc.oldPos.Length - i) / (float)npc.oldPos.Length;

                if (SizeFadeOut) size *= Fade;
                if (ColorFadeOut) color *= Fade;

                Main.EntitySpriteDraw(sprite, pos + extraPos, npc.frame, color, rotation, origin, size, SpriteEffects.None);
            }
        }
        public static void DrawTrailProj(Projectile proj, Color color, float size, string texture, bool FadeSize, bool FadeColor, float DistanceBetweenPoints, Vector2 position)
        {
            Texture2D sprite = ModContent.Request<Texture2D>(texture).Value;
            Vector2 origin = new Vector2(sprite.Width * 0.5f, proj.height * 0.5f);
            for (int i = 0; i < (proj.oldPos.Length / DistanceBetweenPoints); i++)
            {
                Vector2 pos = proj.oldPos[(int)(i * DistanceBetweenPoints)] + position - Main.screenPosition + origin + new Vector2(0, proj.gfxOffY);
                float Fade = (proj.oldPos.Length - i) / (float)proj.oldPos.Length;

                if (FadeSize) size *= Fade;
                if (FadeColor) color *= Fade;

                Main.EntitySpriteDraw(sprite, pos, null, color, proj.rotation, origin, size, SpriteEffects.None);
            }
        }
        public static void BetterDrawTrailProj(Projectile proj, Color color, float size, string texture, float sizeFadeMult, bool FadeColor, float DistanceBetweenPoints, Vector2 position, float rot)
        {
            Texture2D sprite = ModContent.Request<Texture2D>(texture).Value;
            Vector2 origin = new Vector2(sprite.Width * 0.5f, proj.height * 0.5f);
            for (int i = 1; i < proj.oldPos.Length; i++)
            {
                Vector2 pos2 = new Vector2((proj.position - proj.oldPos[i]).Length() / sprite.Width, (proj.position - proj.oldPos[i]).Length() / sprite.Height);
                Vector2 pos = proj.oldPos[i] + pos2 * 0.125f + position - Main.screenPosition + origin;
                float Fade = (proj.oldPos.Length - i) / (float)proj.oldPos.Length;
                float sizeFade = (proj.oldPos.Length - (i * sizeFadeMult)) / (float)proj.oldPos.Length;

                size *= sizeFade;
                if (FadeColor) color *= Fade;

                Main.EntitySpriteDraw(sprite, pos, null, color, (proj.position - proj.oldPos[i]).ToRotation() + rot, origin, size, SpriteEffects.None);
            }
        }
        internal static void BetterDrawTrailProj(Projectile proj, Color color, float size, string texture, float sizeFadeMult, bool FadeColor, float DistanceBetweenPoints, Vector2 position, float rot, Vector2 origin)
        {
            Texture2D sprite = ModContent.Request<Texture2D>(texture).Value;
            for (int i = 1; i < proj.oldPos.Length; i++)
            {
                Vector2 pos2 = new Vector2((proj.position - proj.oldPos[i]).Length() / sprite.Width, (proj.position - proj.oldPos[i]).Length() / sprite.Height);
                Vector2 pos = proj.oldPos[i] + pos2 * 0.125f + position - Main.screenPosition + origin;
                float Fade = (proj.oldPos.Length - i) / (float)proj.oldPos.Length;
                float sizeFade = (proj.oldPos.Length - (i * sizeFadeMult)) / (float)proj.oldPos.Length;

                size *= sizeFade;
                if (FadeColor) color *= Fade;

                Main.EntitySpriteDraw(sprite, pos, null, color, (proj.position - proj.oldPos[i]).ToRotation() + rot, origin, size, SpriteEffects.None);
            }
        }
        public static void DrawTrailProj(Projectile proj, Color color, float size, string texture, bool FadeSize, bool FadeColor, float DistanceBetweenPoints, Vector2 position, bool oldRot, float rot)
        {
            Texture2D sprite = ModContent.Request<Texture2D>(texture).Value;
            Vector2 origin = new Vector2(sprite.Width * 0.5f, proj.height * 0.5f);
            for (int i = 1; i < (proj.oldPos.Length / DistanceBetweenPoints); i++)
            {
                Vector2 pos = proj.oldPos[(int)(i * DistanceBetweenPoints)] + position - Main.screenPosition + origin + new Vector2(0, proj.gfxOffY);
                float Fade = (proj.oldPos.Length - i) / (float)proj.oldPos.Length;

                if (FadeSize) size *= Fade;
                if (FadeColor) color *= Fade;

                Main.EntitySpriteDraw(sprite, pos, null, color, proj.oldRot[(int)(i * DistanceBetweenPoints)] + rot, origin, size, SpriteEffects.None);
            }
        }
        internal static void DrawTrailProj(Projectile proj, Color color, float size, string texture, bool FadeSize, bool FadeColor, float DistanceBetweenPoints, Vector2 position, float rot, SpriteEffects se)
        {
            Texture2D sprite = ModContent.Request<Texture2D>(texture).Value;
            Vector2 origin = new Vector2(sprite.Width * 0.5f, proj.height * 0.5f);
            for (int i = 1; i < (proj.oldPos.Length / DistanceBetweenPoints); i++)
            {
                Vector2 pos = proj.oldPos[(int)(i * DistanceBetweenPoints)] + position - Main.screenPosition + origin + new Vector2(0, proj.gfxOffY);
                float Fade = (proj.oldPos.Length - i) / (float)proj.oldPos.Length;

                if (FadeSize) size *= Fade;
                if (FadeColor) color *= Fade;

                Main.EntitySpriteDraw(sprite, pos, null, color, proj.oldRot[(int)(i * DistanceBetweenPoints)] + rot, origin, size, se);
            }
        }
        internal static void DrawTrailProj(Projectile proj, Color color, float size, string texture, bool FadeSize, bool FadeColor, float DistanceBetweenPoints, Vector2 position, int width, int height)
        {
            Texture2D sprite = ModContent.Request<Texture2D>(texture).Value;
            Vector2 origin = new Vector2(sprite.Width * 0.5f, proj.height * 0.5f);
            for (int i = 0; i < (proj.oldPos.Length / DistanceBetweenPoints); i++)
            {
                Vector2 pos = proj.oldPos[(int)(i * DistanceBetweenPoints)] + position - Main.screenPosition + origin + new Vector2(0, proj.gfxOffY);
                float Fade = (proj.oldPos.Length - i) / (float)proj.oldPos.Length;

                if (FadeSize) size *= Fade;
                if (FadeColor) color *= Fade;

                Main.spriteBatch.Draw(sprite, new Rectangle((int)pos.X, (int)pos.Y, width, height), null, color, proj.rotation, origin, SpriteEffects.None, 0f);
            }
        }
        internal static void DrawTrailProj(Projectile proj, Color color, float size, Texture2D texture, bool FadeSize, bool FadeColor, float DistanceBetweenPoints, Vector2 position)
        {
            Vector2 origin = new Vector2(texture.Width * 0.5f, proj.height * 0.5f);
            for (int i = 0; i < (proj.oldPos.Length / DistanceBetweenPoints); i++)
            {
                Vector2 pos = proj.oldPos[(int)(i * DistanceBetweenPoints)] + position - Main.screenPosition + origin + new Vector2(0, proj.gfxOffY);
                float Fade = (proj.oldPos.Length - i) / (float)proj.oldPos.Length;

                if (FadeSize) size *= Fade;
                if (FadeColor) color *= Fade;

                Main.EntitySpriteDraw(texture, pos, null, color, proj.rotation, origin, size, SpriteEffects.None);
            }
        }
        internal static void DrawTrailProj(Projectile proj, Color color, float size, Texture2D texture, bool FadeSize, bool FadeColor, float DistanceBetweenPoints, Vector2 position, Vector2 origin)
        {
            for (int i = 0; i < (proj.oldPos.Length / DistanceBetweenPoints); i++)
            {
                Vector2 pos = proj.oldPos[(int)(i * DistanceBetweenPoints)] + position - Main.screenPosition + origin + new Vector2(0, proj.gfxOffY);
                float Fade = (proj.oldPos.Length - i) / (float)proj.oldPos.Length;

                if (FadeSize) size *= Fade;
                if (FadeColor) color *= Fade;

                Main.EntitySpriteDraw(texture, pos, null, color, proj.rotation, origin, size, SpriteEffects.None);
            }
        }
        public static void RestartSB(SpriteBatch spriteBatch, BlendState bs, Effect eff)
        {
            spriteBatch.End();
            spriteBatch.Begin(default, bs, Main.DefaultSamplerState, default, default, eff, Main.GameViewMatrix.TransformationMatrix);
        }
        public static void BigTileGlowmask(int i, int j, string spriteString, Vector2 extraPos)
        {
            Tile tile = Main.tile[i, j];

            int x = i * 16;
            int y = j * 16;

            Vector2 offScreen = new Vector2(Main.offScreenRange);
            if (Main.drawToScreen)
                offScreen = Vector2.Zero;

            Texture2D sprite = ModContent.Request<Texture2D>(spriteString).Value;

            Main.spriteBatch.Draw(sprite, new Vector2(x, y) - Main.screenPosition + offScreen + extraPos, new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, 16), Color.White);
        }
        internal static void BigTileGlowmask(int i, int j, string spriteString, Color glowColour, Vector2 extraPos)
        {
            Tile tile = Main.tile[i, j];

            int x = i * 16;
            int y = j * 16;

            Vector2 offScreen = new Vector2(Main.offScreenRange);
            if (Main.drawToScreen)
                offScreen = Vector2.Zero;

            Texture2D sprite = ModContent.Request<Texture2D>(spriteString).Value;

            Main.EntitySpriteDraw(sprite, new Vector2(x, y) - Main.screenPosition + offScreen + extraPos, new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, 16), glowColour, 0, sprite.Size() * 0.5f, 1, SpriteEffects.None);
        }
        public static void DrawEntity(Entity entity, Color color, float size, string texture, float rotation, Vector2 pos, Rectangle? rec)
        {
            string bloomstring = "TranscendenceMod/Miscannellous/Assets/GlowBloom";
            if (texture == "bloom2")
                bloomstring = "TranscendenceMod/Miscannellous/Assets/Bloom";
            else if (texture == "bloom3")
                bloomstring = "TranscendenceMod/Miscannellous/Assets/OldGlowBloom";
            else if (texture == "bloom4")
                bloomstring = "TranscendenceMod/Miscannellous/Assets/GlowBloomNoBG";

            Texture2D bloomsprite = ModContent.Request<Texture2D>(bloomstring).Value;

            if (!texture.Contains("bloom"))
            {
                Texture2D sprite = ModContent.Request<Texture2D>(texture).Value;
                Vector2 origin = sprite.Size() * 0.5f;
                Main.EntitySpriteDraw(sprite, pos - Main.screenPosition, rec, color, rotation, origin, size, SpriteEffects.None);
            }
            else
            {
                SpriteBatch sb = Main.spriteBatch;
                sb.End();
                sb.Begin(default, blendState: texture == "bloom4" ? BlendState.NonPremultiplied : BlendState.Additive, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);

                sb.Draw(bloomsprite, pos - Main.screenPosition, null, color, 0, bloomsprite.Size() * 0.5f,
                    texture == "bloom" ? size / 3 : size, SpriteEffects.None, 0);

                sb.End();
                sb.Begin(default, blendState: BlendState.AlphaBlend, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);
            }
        }
        internal static void DrawEntity(Entity entity, Color color, string texture, float rotation, Rectangle dest, Rectangle? rec, SpriteBatch sb)
        {
            Texture2D sprite = ModContent.Request<Texture2D>(texture).Value;
            Vector2 origin = sprite.Size() * 0.5f;

            dest.X -= (int)Main.screenPosition.X;
            dest.Y -= (int)Main.screenPosition.Y;

            sb.Draw(sprite, dest, rec, color, rotation, origin, SpriteEffects.None, 0f);
        }
        internal static void DrawEntity(Entity entity, Color color, Texture2D texture, float rotation, Rectangle destRec, Rectangle? rec, Vector2 origin)
        {
            SpriteBatch sb = Main.spriteBatch;
            sb.Draw(texture, destRec, rec, color, rotation, origin, SpriteEffects.None, 0);
        }
        internal static void DrawEntity(Entity entity, Color color, string texture, float rotation, Rectangle destRec, Rectangle? rec)
        {
            SpriteBatch sb = Main.spriteBatch;
            Texture2D sprite = ModContent.Request<Texture2D>(texture).Value;
            sb.Draw(sprite, destRec, rec, color, rotation, sprite.Size() * 0.5f, SpriteEffects.None, 0);
        }
        internal static void DrawEntity(Entity entity, Color color, float size, Texture2D texture, float rotation, Vector2 pos, Rectangle? rec)
        {
            Vector2 origin = texture.Size() * 0.5f;
            Main.EntitySpriteDraw(texture, pos - Main.screenPosition, rec, color, rotation, origin, size, SpriteEffects.None);
        }
        internal static void DrawEntity(Entity entity, Color color, float size, string texture, float rotation, Vector2 pos, Rectangle? rec, Vector2 origin)
        {
            Texture2D sprite = ModContent.Request<Texture2D>(texture).Value;
            Main.EntitySpriteDraw(sprite, pos - Main.screenPosition, rec, color, rotation, origin, size, SpriteEffects.None);
        }
        internal static void DrawEntity(Entity entity, Color color, float size, string texture, float rotation, Vector2 pos, Rectangle? rec, SpriteEffects se)
        {
            Texture2D sprite = ModContent.Request<Texture2D>(texture).Value;
            Main.EntitySpriteDraw(sprite, pos - Main.screenPosition, rec, color, rotation, sprite.Size() * 0.5f, size, se);
        }
        internal static void DrawEntity(Entity entity, Color color, float size, Texture2D texture, float rotation, Vector2 pos, Rectangle? rec, Vector2 origin)
        {
            Main.EntitySpriteDraw(texture, pos - Main.screenPosition, rec, color, rotation, origin, size, SpriteEffects.None);
        }
        internal static void DrawEntity(Entity entity, Color color, float size, Texture2D texture, float rotation, Vector2 pos, Rectangle? rec, Vector2 origin, SpriteEffects se)
        {
            Main.EntitySpriteDraw(texture, pos - Main.screenPosition, rec, color, rotation, origin, size, se);
        }
        public static void CircularAoETelegraph(SpriteBatch spriteBatch, Vector2 pos, Color col, float scale, int colCount)
        {
            Texture2D sprite3 = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/GlowBloom").Value;

            //Request effect
            var eff = ModContent.Request<Effect>("TranscendenceMod/Miscannellous/Assets/Shaders/Effects/Pixelation", AssetRequestMode.ImmediateLoad).Value;

            eff.Parameters["uImageSize1"].SetValue(new Vector2(sprite3.Width * scale, sprite3.Height * scale));
            eff.Parameters["maxColors"].SetValue(colCount);



            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.Additive, Main.DefaultSamplerState, default, default, eff, Main.GameViewMatrix.TransformationMatrix);

            Main.EntitySpriteDraw(sprite3, pos - Main.screenPosition, null, col,
                0, sprite3.Size() * 0.5f, scale, SpriteEffects.None);

            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.AlphaBlend, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);
        }
        public static void DrawProjAnimated(Projectile proj, Color color, float size, string texture, float rotation, Vector2 pos, bool Trail, bool FadeSize, bool FadeColor)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;
            Texture2D sprite = ModContent.Request<Texture2D>(texture).Value;
            int frameSizeY = sprite.Height / Main.projFrames[proj.type];
            int defaultPos = frameSizeY * proj.frame;
            Rectangle rec = new Rectangle(0, defaultPos, sprite.Width, frameSizeY);
            Vector2 origin = rec.Size() * 0.5f;
            if (!Trail)
                spriteBatch.Draw(sprite, pos - Main.screenPosition + new Vector2(0, proj.gfxOffY), rec, color, rotation, origin, size, SpriteEffects.None, 0);
            else
            {
                for (int i = 0; i < proj.oldPos.Length; i++)
                {
                    Vector2 position = pos + proj.oldPos[i] - Main.screenPosition + origin + new Vector2(0, proj.gfxOffY);
                    float Fade = (proj.oldPos.Length - i) / (float)proj.oldPos.Length;
                    if (FadeSize) size *= Fade;
                    if (FadeColor) color *= Fade;

                    Main.EntitySpriteDraw(sprite, position, rec, color, proj.oldRot[i] + rotation, origin, size, SpriteEffects.None);
                }
            }
        }
        internal static void DrawProjAnimated(Projectile proj, Color color, float size, string texture, float rotation, Vector2 pos, bool Trail, bool FadeSize, bool FadeColor, SpriteEffects se)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;
            Texture2D sprite = ModContent.Request<Texture2D>(texture).Value;
            int frameSizeY = sprite.Height / Main.projFrames[proj.type];
            int defaultPos = frameSizeY * proj.frame;
            Rectangle rec = new Rectangle(0, defaultPos, sprite.Width, frameSizeY);
            Vector2 origin = rec.Size() * 0.5f;
            if (!Trail)
                spriteBatch.Draw(sprite, pos - Main.screenPosition + new Vector2(0, proj.gfxOffY), rec, color, rotation, origin, size, se, 0);
            else
            {
                for (int i = 0; i < proj.oldPos.Length; i++)
                {
                    Vector2 position = pos + proj.oldPos[i] - Main.screenPosition + origin + new Vector2(0, proj.gfxOffY);
                    float Fade = (proj.oldPos.Length - i) / (float)proj.oldPos.Length;
                    if (FadeSize) size *= Fade;
                    if (FadeColor) color *= Fade;

                    Main.EntitySpriteDraw(sprite, position, rec, color, rotation, origin, size, se);
                }
            }
        }

        internal static void DrawProjAnimated(Projectile proj, Color color, float size, string texture, float rotation, Vector2 pos, float TrailLenght, bool FadeSize, bool FadeColor, Vector2 origin, SpriteEffects se)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;
            Texture2D sprite = ModContent.Request<Texture2D>(texture).Value;
            int frameSizeY = sprite.Height / Main.projFrames[proj.type];
            int defaultPos = frameSizeY * proj.frame;
            Rectangle rec = new Rectangle(0, defaultPos, sprite.Width, frameSizeY);

                for (int i = 0; i < (proj.oldPos.Length / TrailLenght); i++)
                {
                    Vector2 position = pos + proj.oldPos[(int)(i * TrailLenght)] - Main.screenPosition + origin + new Vector2(0, proj.gfxOffY);
                    float Fade = (proj.oldPos.Length - (i * TrailLenght)) / (float)proj.oldPos.Length;
                    if (FadeSize) size *= Fade;
                    if (FadeColor) color *= Fade;

                    Main.EntitySpriteDraw(sprite, position, rec, color, rotation, origin, size, se);
                }
        }
        public static void DrawEntityOutlines(Entity entity, Color color, float size, string texture, float rotation, Vector2 pos, Rectangle? rec)
        {
            Texture2D sprite = ModContent.Request<Texture2D>(texture).Value;
            Vector2 origin = new Vector2(sprite.Width * 0.5f, sprite.Height * 0.5f);
            Main.EntitySpriteDraw(sprite, pos + new Vector2(-1, 0) - Main.screenPosition, rec, color, rotation, origin, size, SpriteEffects.None);
            Main.EntitySpriteDraw(sprite, pos + new Vector2(1, 0) - Main.screenPosition, rec, color, rotation, origin, size, SpriteEffects.None);
            Main.EntitySpriteDraw(sprite, pos + new Vector2(0, -1) - Main.screenPosition, rec, color, rotation, origin, size, SpriteEffects.None);
            Main.EntitySpriteDraw(sprite, pos + new Vector2(0, 1) - Main.screenPosition, rec, color, rotation, origin, size, SpriteEffects.None);
        }
        internal static void DrawEntityOutlines(Entity entity, Color color, float size, string texture, float rotation, Vector2 pos, Rectangle? rec, int dist)
        {
            Texture2D sprite = ModContent.Request<Texture2D>(texture).Value;
            Vector2 origin = new Vector2(sprite.Width * 0.5f, sprite.Height * 0.5f);

            Main.EntitySpriteDraw(sprite, pos + new Vector2(-dist, dist) - Main.screenPosition, rec, color, rotation, origin, size, SpriteEffects.None);
            Main.EntitySpriteDraw(sprite, pos + new Vector2(dist, -dist) - Main.screenPosition, rec, color, rotation, origin, size, SpriteEffects.None);
            Main.EntitySpriteDraw(sprite, pos - new Vector2(dist, dist) - Main.screenPosition, rec, color, rotation, origin, size, SpriteEffects.None);
            Main.EntitySpriteDraw(sprite, pos + new Vector2(dist, dist) - Main.screenPosition, rec, color, rotation, origin, size, SpriteEffects.None);
        }
        internal static void DrawEntityOutlines(Color color, float size, Texture2D texture, float rotation, Vector2 pos, Rectangle? rec, Vector2 origin, int dist, SpriteEffects se)
        {
            Main.EntitySpriteDraw(texture, pos + new Vector2(-dist, dist) - Main.screenPosition, rec, color, rotation, origin, size, se);
            Main.EntitySpriteDraw(texture, pos + new Vector2(dist, -dist) - Main.screenPosition, rec, color, rotation, origin, size, se);
            Main.EntitySpriteDraw(texture, pos - new Vector2(dist, dist) - Main.screenPosition, rec, color, rotation, origin, size, se);
            Main.EntitySpriteDraw(texture, pos + new Vector2(dist, dist) - Main.screenPosition, rec, color, rotation, origin, size, se);
        }
        public static int Timer;
        public static void sell(NPCShop shop, int item, int Price, params Condition[] condition)
            => shop.Add(new Item(item) { shopCustomPrice = Price }, condition);
        internal static void sell(NPCShop shop, int item, params Condition[] condition)
            => shop.Add(item, condition);
        public static void BeGoneBestiary(NPC npc)
        {
            NPCID.Sets.NPCBestiaryDrawModifiers hide = new(0)
            {
                Hide = true
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(npc.type, hide);
        }

        public static void VeryBasicProjOutline(Projectile projectile, string Texture, float dist, float r, float g, float b, float a, bool Animated)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;
            var eff = ModContent.Request<Effect>("TranscendenceMod/Miscannellous/Assets/Shaders/Effects/SeraphOutlineShader", AssetRequestMode.ImmediateLoad).Value;
            eff.Parameters["uOpacity"].SetValue(a);
            eff.Parameters["uSaturation"].SetValue(1f);

            eff.Parameters["uRotation"].SetValue(r);
            eff.Parameters["uTime"].SetValue(g);
            eff.Parameters["uDirection"].SetValue(b);

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, eff, Main.GameViewMatrix.TransformationMatrix);

            for (int i = 0; i < 4; i++)
            {
                Vector2 pos = projectile.Center + Vector2.One.RotatedBy(MathHelper.TwoPi * i / 4f + Main.GlobalTimeWrappedHourly * 6) * dist;
                if (Animated)
                    DrawProjAnimated(projectile, Color.White, projectile.scale, Texture, projectile.rotation, pos, false, false, false);
                else
                    DrawEntity(projectile, Color.White, projectile.scale, Texture, projectile.rotation, pos, null);
            }
            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.AlphaBlend, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);
        }

        internal static void VeryBasicProjOutline(Projectile projectile, string Texture, float dist, float r, float g, float b, float a, bool Animated, Rectangle frame, SpriteEffects se, Vector2 position)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;
            var eff = ModContent.Request<Effect>("TranscendenceMod/Miscannellous/Assets/Shaders/Effects/SeraphOutlineShader", AssetRequestMode.ImmediateLoad).Value;
            eff.Parameters["uOpacity"].SetValue(a);
            eff.Parameters["uSaturation"].SetValue(1f);

            eff.Parameters["uRotation"].SetValue(r);
            eff.Parameters["uTime"].SetValue(g);
            eff.Parameters["uDirection"].SetValue(b);

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, eff, Main.GameViewMatrix.TransformationMatrix);

            for (int i = 0; i < 4; i++)
            {
                Vector2 pos = position + Vector2.One.RotatedBy(MathHelper.TwoPi * i / 4f + Main.GlobalTimeWrappedHourly * 6) * dist;
                if (Animated)
                    DrawProjAnimated(projectile, Color.White, projectile.scale, Texture, projectile.rotation, pos, false, false, false, se);
                else
                    DrawEntity(projectile, Color.White, projectile.scale, ModContent.Request<Texture2D>(Texture).Value, projectile.rotation, pos, frame, frame.Size() * 0.5f, se);
            }
            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.AlphaBlend, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);
        }

        public static void VeryBasicNPCOutline(NPC npc, string Texture, float dist, float r, float g, float b, float a, Vector2 vec, float scale, float rotation)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;

            var eff = ModContent.Request<Effect>("TranscendenceMod/Miscannellous/Assets/Shaders/Effects/SeraphOutlineShader", AssetRequestMode.ImmediateLoad).Value;
            Texture2D sprite = ModContent.Request<Texture2D>(Texture).Value;

            eff.Parameters["uOpacity"].SetValue(a);
            eff.Parameters["uSaturation"].SetValue(1f);

            eff.Parameters["uRotation"].SetValue(r);
            eff.Parameters["uTime"].SetValue(g);
            eff.Parameters["uDirection"].SetValue(b);

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, eff, Main.GameViewMatrix.TransformationMatrix);

            Vector2 origin = sprite.Size() * 0.5f;

            for (int i = 0; i < 4; i++)
            {
                Vector2 pos = vec + Vector2.One.RotatedBy(MathHelper.TwoPi * i / 4f + Main.GlobalTimeWrappedHourly * 6) * dist;
                DrawEntity(npc, Color.White, scale, sprite, rotation, pos, null, origin, SpriteEffects.None);
            }
            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.AlphaBlend, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);
        }

        public static void VeryBasicNPCOutline(NPC npc, string Texture, float dist, float r, float g, float b, float a, Vector2 vec, float scale, float rotation, float amount)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;

            var eff = ModContent.Request<Effect>("TranscendenceMod/Miscannellous/Assets/Shaders/Effects/SeraphOutlineShader", AssetRequestMode.ImmediateLoad).Value;
            Texture2D sprite = ModContent.Request<Texture2D>(Texture).Value;

            eff.Parameters["uOpacity"].SetValue(a);
            eff.Parameters["uSaturation"].SetValue(1f);

            eff.Parameters["uRotation"].SetValue(r);
            eff.Parameters["uTime"].SetValue(g);
            eff.Parameters["uDirection"].SetValue(b);

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, eff, Main.GameViewMatrix.TransformationMatrix);

            Vector2 origin = sprite.Size() * 0.5f;

            for (int i = 0; i < amount; i++)
            {
                Vector2 pos = vec + Vector2.One.RotatedBy(MathHelper.TwoPi * i / 4f + Main.GlobalTimeWrappedHourly * 6) * dist;
                if (amount == 1)
                    pos = vec;
                DrawEntity(npc, Color.White, scale, sprite, rotation, pos, null, origin, SpriteEffects.None);
            }
            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.AlphaBlend, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);
        }

        public static void ProjectileRing(Entity entity, int circleNumProj, IEntitySource source, Vector2 position,
            int projtype, int damage, float kb, float speed, float ai0, float ai1, float ai2, int owner, float rot)
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                float projCount = circleNumProj;
                Vector2 vec = new Vector2(-4, 0);

                for (int i = 0; i < projCount; i++)
                {
                    float pi = MathHelper.Pi * 2 / projCount;
                    Vector2 vel = vec.RotatedBy(pi * i);
                    vel = vel.RotatedBy(rot);
                    int proj = Projectile.NewProjectile(source, position, vel * speed, projtype, damage, kb, owner, ai0, ai1, ai2);
                    //Main.projectile[proj].velocity *= speed;
                }
            }
        }
        internal static void ProjectileRing(Entity entity, int circleNumProj, IEntitySource source, Vector2 position,
    int projtype, int damage, float kb, float speed, float ai0, float ai1, float ai2, int owner, float rot, int extraUpdates)
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                float projCount = circleNumProj;
                Vector2 vec = new Vector2(-4, 0);

                for (int i = 0; i < projCount; i++)
                {
                    float pi = MathHelper.Pi * 2 / projCount;
                    Vector2 vel = vec.RotatedBy(pi * i);
                    vel = vel.RotatedBy(rot);
                    int proj = Projectile.NewProjectile(source, position, vel * speed, projtype, damage, kb, owner, ai0, ai1, ai2);
                    Main.projectile[proj].extraUpdates = extraUpdates;
                }
            }
        }
        internal static void ProjectileRing(Entity entity, int circleNumProj, IEntitySource source, Vector2 position,
            int projtype, int damage, float kb, float speed, float ai0, float ai1, float ai2, int owner, float rot, float lAI0, float lAI1, float lAI2)
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                float projCount = circleNumProj;
                Vector2 vec = new Vector2(-4, 0);

                for (int i = 0; i < projCount; i++)
                {
                    float pi = MathHelper.Pi * 2 / projCount;
                    Vector2 vel = vec.RotatedBy(pi * i);
                    vel = vel.RotatedBy(rot);
                    int proj = Projectile.NewProjectile(source, position, vel * speed, projtype, damage, kb, owner, ai0, ai1, ai2);
                    Main.projectile[proj].localAI[0] = lAI0;
                    Main.projectile[proj].localAI[1] = lAI1;
                    Main.projectile[proj].localAI[2] = lAI2;
                }
            }
        }
        internal static void ProjectileRing(Entity entity, int circleNumProj, IEntitySource source, Vector2 position,
    int projtype, int damage, float kb, float speed, float ai0, float ai1, float ai2, int owner, float rot, bool SpiritWheel)
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                float projCount = circleNumProj;
                Vector2 vec = new Vector2(-4, 0);

                for (int i = 0; i < projCount; i++)
                {
                    float pi = MathHelper.Pi * 2 / projCount;
                    Vector2 vel = vec.RotatedBy(pi * i);
                    vel = vel.RotatedBy(rot);
                    int proj = Projectile.NewProjectile(source, position, vel * speed, projtype, damage, kb, owner, ai0, ai1, ai2 + i);
                    //Main.projectile[proj].velocity *= speed;
                }
            }
        }
        public static void BasicShotgun(IEntitySource source, Vector2 center, Vector2 vel, int type, int damage, float kb, int amount, float spread, float speed)
        {
            for (int i = -amount; i < amount; i++)
            {
                float vel2 = vel.ToRotation() + i * MathHelper.Pi / spread;

                if (Main.netMode != 1)
                {
                    Projectile.NewProjectile(source, center, vel2.ToRotationVector2() * speed, type, damage, kb);
                }
            }
        }
        internal static void BasicShotgun(IEntitySource source, Vector2 center, Vector2 vel, int type, int damage, float kb, int amount, float spread, float speed, int owner, float ai0, float ai1, float ai2)
        {
            for (int i = -amount; i < amount; i++)
            {
                float vel2 = vel.ToRotation() + i * MathHelper.Pi / spread;

                if (Main.netMode != 1)
                {
                    Projectile.NewProjectile(source, center, vel2.ToRotationVector2() * speed, type, damage, kb, owner, ai0, ai1, ai2);
                }
            }
        }
        public static void ProjectileShotgun(Vector2 vel, Vector2 pos, IEntitySource source, int type, int dmg, float kb, float speed, int Amount,
            float Spread, int owner, float ai0, float ai1, float ai2, int Side)
        {
            for (int i = 0; i < Amount; i++)
            {
                Vector2 vel0 = vel.RotatedBy(MathHelper.ToRadians(Spread / 3));
                Vector2 vel1 = vel.RotatedBy(MathHelper.ToRadians(-Spread / 3));

                Vector2 vel2 = vel.RotatedBy(MathHelper.ToRadians(i) * Spread);
                Vector2 vel3 = vel.RotatedBy(MathHelper.ToRadians(-i) * Spread);

                if (Main.netMode != 1)
                {
                    if (Side == 0)
                    {
                        Projectile.NewProjectile(source, pos, (i > 0 ? vel2 : vel0) * speed, type, dmg, kb, owner, ai0, ai1, ai2);
                        Projectile.NewProjectile(source, pos, (i > 0 ? vel3 : vel1) * speed, type, dmg, kb, owner, ai0, ai1, ai2);
                    }
                    else
                    {
                        Projectile.NewProjectile(source, pos, (Side == 1 ? vel2 : vel3) * speed, type, dmg, kb, owner, ai0, ai1, ai2);
                    }
                }
            }
        }
        internal static void ProjectileShotgun(Vector2 vel, Vector2 pos, IEntitySource source, int type, int dmg, float kb, float speed, int Amount,
    float Spread, int owner, float ai0, float ai1, float ai2, int Side, int extraUpdates, float scale)
        {
            for (int i = 0; i < Amount; i++)
            {
                Vector2 vel0 = vel.RotatedBy(MathHelper.ToRadians(Spread / 3));
                Vector2 vel1 = vel.RotatedBy(MathHelper.ToRadians(-Spread / 3));

                Vector2 vel2 = vel.RotatedBy(MathHelper.ToRadians(i) * Spread);
                Vector2 vel3 = vel.RotatedBy(MathHelper.ToRadians(-i) * Spread);

                if (Main.netMode != 1)
                {
                    if (Side == 0)
                    {
                        int p = Projectile.NewProjectile(source, pos, (i > 0 ? vel2 : vel0) * speed, type, dmg, kb, owner, ai0, ai1, ai2);
                        Main.projectile[p].extraUpdates = extraUpdates; Main.projectile[p].scale = scale;
                        int p2 = Projectile.NewProjectile(source, pos, (i > 0 ? vel3 : vel1) * speed, type, dmg, kb, owner, ai0, ai1, ai2);
                        Main.projectile[p2].extraUpdates = extraUpdates; Main.projectile[p2].scale = scale;
                    }
                    else
                    {
                        int p = Projectile.NewProjectile(source, pos, (Side == 1 ? vel2 : vel3) * speed, type, dmg, kb, owner, ai0, ai1, ai2);
                        Main.projectile[p].extraUpdates = extraUpdates; Main.projectile[p].scale = scale;
                    }
                }
            }
        }
        public static void ParticleOrchestra(ParticleOrchestraType type, Vector2 pos, int owner)
        {
            ParticleOrchestrator.RequestParticleSpawn(false, type,
              new ParticleOrchestraSettings { PositionInWorld = pos }, owner);
        }
        public static void NoExpertProjDamage(Projectile projectile)
        {
            if (Main.expertMode || Main.masterMode) projectile.damage /= 2;
        }
        public static void DustRing(Vector2 pos, int amount, int DustType, float speed, Color color, float size)
        {
            for (int t = 0; t < amount; t++)
            {
                Dust dust = Dust.NewDustPerfect(pos, DustType,
                 Main.rand.NextVector2CircularEdge(1f, 1f) * speed, 0, color, size);
                dust.noGravity = true;
            }
        }
        internal static void DustRing(Vector2 pos, int amount, int DustType, Color color, float size, float minium, float maxium)
        {
            for (int t = 0; t < amount; t++)
            {
                Dust dust = Dust.NewDustPerfect(pos, DustType,
                 Main.rand.NextVector2CircularEdge(1f, 1f) * Main.rand.NextFloat(minium, maxium), 0, color, size);
                dust.noGravity = true;
            }
        }
        public static void FindNPC(int var, int npc)
        {
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npci = Main.npc[i];
                if (npci.type == npc) var = npc;
            }
        }
        public static void AnimateProj(Projectile proj, int FrameDelay)
        {
            if (++proj.frameCounter > FrameDelay)
            {
                proj.frameCounter = 0;
                if (++proj.frame >= Main.projFrames[proj.type])
                {
                    proj.frame = 0;
                }
            }
        }
    }
}