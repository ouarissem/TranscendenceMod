using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous.GlobalStuff;

namespace TranscendenceMod.Projectiles.NPCs.Bosses.Muramasa
{
    public class MuramasaDeathLaser : ModProjectile
    {
        public float rot;
        public Vector2 Center;
        public override string Texture => "TranscendenceMod/Miscannellous/Assets/InvisSprite";
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;

            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
 
            Projectile.tileCollide = false;
            Projectile.scale = 0f;
            Projectile.timeLeft = 130;
            Projectile.extraUpdates = 1;

            Projectile.hostile = true;
            Projectile.ignoreWater = true;
        }
        public override void AI()
        {
            
            Projectile.GetGlobalProjectile<TranscendenceProjectiles>().CanBeErased = false;

            Projectile.Center = Center;
            Projectile.Center = Center + Vector2.One.RotatedBy(rot) * 100;

            if (Projectile.timeLeft == 75)
                SoundEngine.PlaySound(SoundID.Item71 with { MaxInstances = 0 });

            float scale = (1f / Projectile.ai[2]);
            if (Projectile.timeLeft > 75 && Projectile.timeLeft < 90)
                Projectile.scale += scale / 15f;

            if (Projectile.timeLeft < 30 && Projectile.scale > 0)
                Projectile.scale -= (1f / Projectile.ai[2]) / 30f;
        }
        public override void OnSpawn(IEntitySource source)
        {
            Center = Projectile.Center;
            rot = Projectile.velocity.ToRotation() - MathHelper.PiOver4;
            Projectile.velocity = Vector2.Zero;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Player player = Main.player[Projectile.owner];

            float reference = float.NaN;
            if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Center, Center + Vector2.One.RotatedBy(rot + Projectile.ai[1] * Projectile.ai[0]) * 1350, 6 * Projectile.scale, ref reference))
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
            Vector2 pos2 = Projectile.Center + Vector2.One.RotatedBy(rot + Projectile.ai[1] * Projectile.ai[0]) * 1350f;

            //Request the Effect
            var eff = ModContent.Request<Effect>("TranscendenceMod/Miscannellous/Assets/Shaders/Effects/MovingNoiseTrans", AssetRequestMode.ImmediateLoad).Value;
            //Apply Shader Texture
            Texture2D shaderImage = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/SpiritShader").Value;
            Texture2D shaderImage2 = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/SerpentCrack").Value;

            Projectile.localAI[2] += 0.0125f;

            eff.Parameters["useExtraCol"].SetValue(false);
            eff.Parameters["useAlpha"].SetValue(false);

            sb.End();
            sb.Begin(default, BlendState.Additive, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            if (Projectile.timeLeft > 75)
            {
                float a = Projectile.timeLeft > 180 ? MathHelper.Lerp(0.5f, 0f, (Projectile.timeLeft - 240) / 60f) : 1f;

                sb.Draw(sprite, new Rectangle(
                    (int)(pos.X - Main.screenPosition.X), (int)(pos.Y - Main.screenPosition.Y), 75, (int)(pos.Distance(pos2) * 2f)), null,
                    Color.DeepSkyBlue * 0.66f * a, pos.DirectionTo(pos2).ToRotation() + MathHelper.PiOver2, sprite.Size() * 0.5f, SpriteEffects.None, 0);
            }

            sb.Draw(sprite, new Rectangle(
                (int)(pos.X - Main.screenPosition.X), (int)(pos.Y - Main.screenPosition.Y), (int)(245 * Projectile.scale), (int)(pos.Distance(pos2) * 2f)), null,
                Color.Blue * Projectile.scale, pos.DirectionTo(pos2).ToRotation() + MathHelper.PiOver2, sprite.Size() * 0.5f, SpriteEffects.None, 0);
            
            sb.Draw(sprite, new Rectangle(
                (int)(pos.X - Main.screenPosition.X), (int)(pos.Y - Main.screenPosition.Y), (int)(225 * Projectile.scale), (int)(pos.Distance(pos2) * 2f)), null,
                Color.DeepSkyBlue * 0.5f * Projectile.scale, pos.DirectionTo(pos2).ToRotation() + MathHelper.PiOver2, sprite.Size() * 0.5f, SpriteEffects.None, 0);

            eff.Parameters["uImageSize0"].SetValue(shaderImage.Size());

            for (int i = 0; i < 6; i++)
            {
                Main.instance.GraphicsDevice.Textures[1] = shaderImage;
                eff.Parameters["uImageSize1"].SetValue(shaderImage.Size() * 0.25f);

                eff.Parameters["uTime"].SetValue(Projectile.localAI[2] * 0.125f);
                eff.Parameters["yChange"].SetValue(Projectile.localAI[2]);

                sb.End();
                sb.Begin(default, BlendState.Additive, default, default, default, eff, Main.GameViewMatrix.TransformationMatrix);




                sb.Draw(sprite, new Rectangle(
                    (int)(pos.X - Main.screenPosition.X), (int)(pos.Y - Main.screenPosition.Y), (int)(85 * Projectile.scale), (int)(pos.Distance(pos2) * 2f)), null,
                    Color.White * Projectile.scale, pos.DirectionTo(pos2).ToRotation() + MathHelper.PiOver2, sprite.Size() * 0.5f, SpriteEffects.None, 0);



                eff.Parameters["uImageSize1"].SetValue(new Vector2(shaderImage.Width * 0.75f, shaderImage.Height * 0.125f));
                Main.instance.GraphicsDevice.Textures[1] = shaderImage2;

                eff.Parameters["uTime"].SetValue(Projectile.localAI[2] * 0.25f);
                eff.Parameters["yChange"].SetValue(Projectile.localAI[2] * 2.5f);

                sb.End();
                sb.Begin(default, BlendState.Additive, default, default, default, eff, Main.GameViewMatrix.TransformationMatrix);

                sb.Draw(sprite, new Rectangle(
                    (int)(pos.X - Main.screenPosition.X), (int)(pos.Y - Main.screenPosition.Y), (int)(75 * Projectile.scale), (int)(pos.Distance(pos2) * 2f)), null,
                    Color.White * Projectile.scale, pos.DirectionTo(pos2).ToRotation() + MathHelper.PiOver2, sprite.Size() * 0.5f, SpriteEffects.None, 0);
            }

            sb.End();
            sb.Begin(default, BlendState.AlphaBlend, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);

        }
    }
}