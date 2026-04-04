using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.Weapons.Magic
{
    public class DreamSealOrb : ModProjectile
    {
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;

            Projectile.aiStyle = -1;

            Projectile.tileCollide = false;
            Projectile.penetrate = -1;

            Projectile.friendly = true;
            Projectile.ignoreWater = true;
        }
        public override Color? GetAlpha(Color lightColor) => Color.White;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.rotation += 0.125f;

            Projectile.timeLeft = 60;

            if (player.Distance(Projectile.Center) > 260)
                Projectile.ai[2] = 1;
            else
            {
                if (player.statMana < player.statManaMax2)
                    player.statMana += 2;
                player.statDefense += 20;
            }

            if (Projectile.ai[2] == 1)
                Projectile.scale -= 1f / 30f;

            if (Projectile.scale < 0.1f)
                Projectile.Kill();

            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile p = Main.projectile[i];
                if (p != null && p.active && p.type == ModContent.ProjectileType<DreamSealProj>() && p.Distance(Projectile.Center) < 540 && Projectile.ai[2] != 1)
                {
                    if (p.ai[1] == 0 && p.Distance(Projectile.Center) > 240)
                    {
                        p.ai[1] = 1;
                        p.ArmorPenetration = 40;

                        p.velocity = -p.velocity;
                        p.Center = Projectile.Center + Vector2.One.RotatedBy(p.DirectionTo(Projectile.Center).ToRotation() + MathHelper.PiOver4 + MathHelper.PiOver2) * 360f;
                    }
                    if (p.ai[1] == 1 && p.Distance(Projectile.Center) < 260)
                    {
                        p.ai[1] = 2;

                        p.velocity = -p.velocity;
                        p.Center = Projectile.Center + Vector2.One.RotatedBy(p.DirectionTo(Projectile.Center).ToRotation() + MathHelper.PiOver4 + MathHelper.PiOver2) * 380f;

                        p.damage *= 2;
                    }
                }
            }
        }
        public override void PostDraw(Color lightColor)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;

            //Draw the boundaries

            var eff = ModContent.Request<Effect>("TranscendenceMod/Miscannellous/Assets/Shaders/Effects/DreamBoundaryShader", AssetRequestMode.ImmediateLoad).Value;
            eff.Parameters["uColor"].SetValue(new Vector3(0.33f, 0f, 0.5f));
            eff.Parameters["uOpacity"].SetValue(12f * Projectile.scale);
            eff.Parameters["uImageSize0"].SetValue(new Vector2(1000));
            eff.Parameters["uImageSize1"].SetValue(new Vector2(325));
            eff.Parameters["uTime"].SetValue(Main.GlobalTimeWrappedHourly);

            //Apply Texture
            Texture2D shaderImage = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/SeraphForcefieldShader").Value;

            Main.instance.GraphicsDevice.Textures[1] = shaderImage;


            TranscendenceUtils.RestartSB(spriteBatch, BlendState.Additive, eff);

            TranscendenceUtils.DrawEntity(Projectile, Color.White, 1600f, "TranscendenceMod/Miscannellous/Assets/Pixel", 0, Projectile.Center, null);
            TranscendenceUtils.DrawEntity(Projectile, Color.White, 800f, "TranscendenceMod/Miscannellous/Assets/Pixel", 0, Projectile.Center, null);


            TranscendenceUtils.RestartSB(spriteBatch, BlendState.Additive, null);
        }
    }
}