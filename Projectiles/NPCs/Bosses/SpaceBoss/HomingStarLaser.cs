using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Graphics.CameraModifiers;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Miscannellous.GlobalStuff;
using TranscendenceMod.NPCs.Boss.Seraph;

namespace TranscendenceMod.Projectiles.NPCs.Bosses.SpaceBoss
{
    public class HomingStarLaser : ModProjectile
    {
        public int time = 600;
        public float rot;
        public Vector2 Center;
        public NPC ChosenNPC;
        public Vector2 Pos;
        public float telegraphSpeed = 1f;
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;

            Projectile.aiStyle = -1;
 
            Projectile.tileCollide = false;
            Projectile.scale = 0f;
            Projectile.timeLeft = 150;

            Projectile.hostile = true;
            Projectile.ignoreWater = true;
        }
        public override void AI()
        {
            
            Projectile.GetGlobalProjectile<TranscendenceProjectiles>().CanBeErased = false;

            int am = 90;

            if (Projectile.timeLeft < (am + 60))
            {
                telegraphSpeed -= 1f / 60f;
            }

            if (Projectile.timeLeft > am)
                rot += 0.075f * telegraphSpeed * Projectile.ai[0];
            else
            {
                if (Projectile.ai[2] < 1350f)
                    Projectile.ai[2] += 50f;
            }

            if (Projectile.timeLeft < 175 && ChosenNPC != null && ChosenNPC.active && ChosenNPC.ModNPC is CelestialSeraph boss)
            {
                Center = ChosenNPC.Center;
                Projectile.Center = Center + Vector2.One.RotatedBy(rot) * 100;
            }

            if (Projectile.timeLeft == 90)
            {
                Main.instance.CameraModifiers.Add(new PunchCameraModifier(new Vector2(Main.rand.Next(-10, 10)),
                    new Vector2(Main.rand.NextFloatDirection()), 100, 5, 5, -1, null));

                SoundEngine.PlaySound(ModSoundstyles.SeraphBomb with { MaxInstances = 0 });
            }
            if (Projectile.timeLeft > 75 && Projectile.timeLeft < 90)
                Projectile.scale += 1 / 15f;

            if (Projectile.timeLeft < 30 && Projectile.scale > 0)
                Projectile.scale -= 1 / 30f;
        }
        public override void OnSpawn(IEntitySource source)
        {
            Center = Projectile.Center;
            rot = Projectile.velocity.ToRotation() - MathHelper.PiOver4;
            Projectile.velocity = Vector2.Zero;

            NPC npc = Main.npc[(int)Projectile.ai[1]];

            if (npc != null && npc.active)
            {
                ChosenNPC = npc;
                Projectile.Center = npc.Center;
            }

            TranscendenceUtils.NoExpertProjDamage(Projectile);
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Player player = Main.player[Projectile.owner];
            float reference = float.NaN;

            Vector2 pos = Center + Vector2.One.RotatedBy(rot) * 50f;

            if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), pos, pos + Vector2.One.RotatedBy(rot + MathHelper.PiOver4 * Projectile.ai[0]) * 2000f, 12f * Projectile.scale, ref reference))
                return true;
            else return false;
        }
        public override bool? CanDamage() => Projectile.timeLeft < 75 && Projectile.timeLeft > 30;
        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
        public override void PostDraw(Color lightColor)
        {
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 4000;
            SpriteBatch sb = Main.spriteBatch;

            Texture2D sprite = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/BloomLine3").Value;

            Vector2 pos = Center + Vector2.One.RotatedBy(rot) * 50f;
            Vector2 pos2 = Projectile.Center + Vector2.One.RotatedBy(rot + MathHelper.PiOver4 * Projectile.ai[0]) * (Projectile.timeLeft > 90 ? 1350f : Projectile.ai[2]);

            //Request the Effect
            var eff = ModContent.Request<Effect>("TranscendenceMod/Miscannellous/Assets/Shaders/Effects/MovingNoiseTrans", AssetRequestMode.ImmediateLoad).Value;
            //Apply Shader Texture
            Texture2D shaderImage = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/SeraphDesperationFog").Value;
            Texture2D shaderImage2 = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/SeraphForcefieldShader").Value;

            Projectile.localAI[2] += 0.0125f;

            eff.Parameters["useExtraCol"].SetValue(false);
            eff.Parameters["useAlpha"].SetValue(false);

            sb.End();
            sb.Begin(default, BlendState.Additive, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            if (Projectile.timeLeft > 90)
            {
                float a = Projectile.timeLeft > 120 ? MathHelper.Lerp(0.5f, 0f, (Projectile.timeLeft - 150) / 20f) : 1f;

                sb.Draw(sprite, new Rectangle(
                    (int)(pos.X - Main.screenPosition.X), (int)(pos.Y - Main.screenPosition.Y), 175, (int)(pos.Distance(pos2) * 2f)), null,
                    Color.DarkMagenta * 0.66f * a, pos.DirectionTo(pos2).ToRotation() + MathHelper.PiOver2, sprite.Size() * 0.5f, SpriteEffects.None, 0);
            }

            float sin = (float)Math.Sin(Main.GlobalTimeWrappedHourly * 10f);
            Color col = Color.Lerp(Color.Magenta * 2f, Color.DarkViolet, sin);

            sb.Draw(sprite, new Rectangle(
                (int)(pos.X - Main.screenPosition.X), (int)(pos.Y - Main.screenPosition.Y), (int)(485 * Projectile.scale * (1f + (sin * 0.125f))), (int)(pos.Distance(pos2) * 2f)), null,
                col * (0.75f + (sin * 0.25f)) * Projectile.scale, pos.DirectionTo(pos2).ToRotation() + MathHelper.PiOver2, sprite.Size() * 0.5f, SpriteEffects.None, 0);
            
            sb.Draw(sprite, new Rectangle(
                (int)(pos.X - Main.screenPosition.X), (int)(pos.Y - Main.screenPosition.Y), (int)(385 * Projectile.scale), (int)(pos.Distance(pos2) * 2f)), null,
                Color.DeepPink * 0.5f * Projectile.scale, pos.DirectionTo(pos2).ToRotation() + MathHelper.PiOver2, sprite.Size() * 0.5f, SpriteEffects.None, 0);

            eff.Parameters["uImageSize0"].SetValue(shaderImage.Size());

            for (int i = 0; i < 12; i++)
            {
                Main.instance.GraphicsDevice.Textures[1] = shaderImage;
                eff.Parameters["uImageSize1"].SetValue(shaderImage.Size() * 2f);

                eff.Parameters["uTime"].SetValue(Projectile.localAI[2] * 0.125f);
                eff.Parameters["yChange"].SetValue(Projectile.localAI[2]);

                sb.End();
                sb.Begin(default, BlendState.Additive, default, default, default, eff, Main.GameViewMatrix.TransformationMatrix);




                sb.Draw(sprite, new Rectangle(
                    (int)(pos.X - Main.screenPosition.X), (int)(pos.Y - Main.screenPosition.Y), (int)(275 * Projectile.scale), (int)(pos.Distance(pos2) * 2f)), null,
                    Color.White * Projectile.scale, pos.DirectionTo(pos2).ToRotation() + MathHelper.PiOver2, sprite.Size() * 0.5f, SpriteEffects.None, 0);



                eff.Parameters["uImageSize1"].SetValue(new Vector2(shaderImage.Width * 0.5f, shaderImage.Height * 0.25f));
                Main.instance.GraphicsDevice.Textures[1] = shaderImage2;

                eff.Parameters["uTime"].SetValue(Projectile.localAI[2] * 0.25f);
                eff.Parameters["yChange"].SetValue(Projectile.localAI[2] * 2.5f);

                sb.End();
                sb.Begin(default, BlendState.Additive, default, default, default, eff, Main.GameViewMatrix.TransformationMatrix);

                sb.Draw(sprite, new Rectangle(
                    (int)(pos.X - Main.screenPosition.X), (int)(pos.Y - Main.screenPosition.Y), (int)(225 * Projectile.scale), (int)(pos.Distance(pos2) * 2f)), null,
                    Color.White * Projectile.scale, pos.DirectionTo(pos2).ToRotation() + MathHelper.PiOver2, sprite.Size() * 0.5f, SpriteEffects.None, 0);
            }

            sb.End();
            sb.Begin(default, BlendState.Additive, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            for (int i = 0; i < 4; i++)
            {
                sb.Draw(sprite, new Rectangle(
                    (int)(pos.X - Main.screenPosition.X), (int)(pos.Y - Main.screenPosition.Y), (int)(100 * Projectile.scale), (int)(pos.Distance(pos2) * 2f)), null,
                    Color.White * Projectile.scale, pos.DirectionTo(pos2).ToRotation() + MathHelper.PiOver2, sprite.Size() * 0.5f, SpriteEffects.None, 0);
            }

            sb.End();
            sb.Begin(default, BlendState.AlphaBlend, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);

        }
    }
}