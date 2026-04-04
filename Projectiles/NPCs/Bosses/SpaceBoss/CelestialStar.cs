using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Graphics;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Dusts;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.NPCs.Bosses.SpaceBoss
{
    public class CelestialStar : ModProjectile
    {
        public float ChaseSpeed = 22;
        public Vector2 pos;
        public int SegmentCount;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 30;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 360;
            Projectile.friendly = false;
            Projectile.light = 0.3f;
            Projectile.hostile = true;
            Projectile.tileCollide = false;
        }
        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item4, Projectile.Center);
            for (int i = 0; i < 2; i++)
            {
                Dust.NewDust(Projectile.position, 16, 16, ModContent.DustType<StarDust>(), Main.rand.NextFloat(), Main.rand.NextFloat());
            }
        }
        public override void OnSpawn(IEntitySource source)
        {
            TranscendenceUtils.NoExpertProjDamage(Projectile);
        }
        public override Color? GetAlpha(Color lightColor) => Color.White;
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.PiOver2 + MathHelper.PiOver4;

            if (++Projectile.localAI[2] % 30 == 0)
            {
                int d = Dust.NewDust(Projectile.Center, 1, 1, ModContent.DustType<StarDust>(), 0, 0, 0, Color.Gold * 0.75f, 0.75f);
                Main.dust[d].velocity = Vector2.Zero;
            }

            Projectile.spriteDirection = Projectile.direction;

            if (Projectile.localAI[2] > 150)
                Projectile.localAI[1] += 1f / 30f;

            if (Projectile.localAI[2] > 180)
                Projectile.Kill();

            if (Projectile.localAI[2] < 50)
            {
                Projectile.velocity *= 0.99f;
                return;
            }

            for (int p = 0; p < Main.maxPlayers; p++)
            {
                Player player = Main.player[p];

                if (player.Distance(Projectile.Center) < 2500 && Projectile.localAI[2] < 150 && Main.rand.NextBool(3))
                {
                    if (player.Distance(Projectile.Center) > 250)
                        ChaseSpeed = 30f;
                    else ChaseSpeed = MathHelper.Lerp(10f, 30f, player.Distance(Projectile.Center) / 250f);

                    Vector2 targetVelocity = Projectile.DirectionTo(player.Center + Vector2.One.RotatedByRandom(MathHelper.TwoPi) * Main.rand.NextFloat(5f, 25f)) * ChaseSpeed;
                    Projectile.velocity = Vector2.Lerp(Projectile.velocity, targetVelocity, 0.15f * Projectile.ai[2]);
                }
            }
        }
        private static VertexStrip _vertexStrip = new VertexStrip();

        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;

            Asset<Texture2D> sprite2 = TextureAssets.Extra[ExtrasID.MagicMissileTrailShape];

            MiscShaderData miscShaderData = GameShaders.Misc["LightDisc"];
            miscShaderData.UseImage1(sprite2);
            miscShaderData.UseSaturation(2.25f);
            miscShaderData.UseOpacity(2f);
            miscShaderData.Apply();

            _vertexStrip.PrepareStripWithProceduralPadding(Projectile.oldPos, Projectile.oldRot, StripColors, StripWidth, -Main.screenPosition + Projectile.Size / 2);
            _vertexStrip.DrawTrail();

            Main.pixelShader.CurrentTechnique.Passes[0].Apply();


            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.Additive, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            TranscendenceUtils.DrawEntity(Projectile, Color.Gold * 0.5f * (1f - Projectile.localAI[1]), 0.675f, "TranscendenceMod/Miscannellous/Assets/GlowBloomNoBG", Projectile.rotation, Projectile.Center, null);
            TranscendenceUtils.DrawEntity(Projectile, Color.White * (1f - Projectile.localAI[1]), 1f, Texture, Projectile.rotation, Projectile.Center, null);

            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.AlphaBlend, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            return false;
        }
        private Color StripColors(float progressOnStrip) => Color.Lerp(Color.White, Color.Lerp(Color.Gold, Color.Transparent, Projectile.localAI[1]), progressOnStrip + (Projectile.localAI[1] * 2f));
        private float StripWidth(float progressOnStrip) => 24f;
    }
}