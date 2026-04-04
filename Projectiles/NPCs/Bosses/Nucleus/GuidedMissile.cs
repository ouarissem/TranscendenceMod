using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Graphics;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.NPCs.Boss.Nucleus;

namespace TranscendenceMod.Projectiles.NPCs.Bosses.Nucleus
{
    public class GuidedMissile : ModProjectile
    {
        public int Timer;
        public int Timer2;
        public Vector2 pos;
        public float Speed = 0;
        public Player player;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 25;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.tileCollide = false;

            Projectile.aiStyle = -1;
            Projectile.timeLeft = 240;
            Projectile.hostile = true;

            ProjectileID.Sets.TrailCacheLength[Type] = 30;
            ProjectileID.Sets.TrailingMode[Type] = 3;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            if (Speed < 28.5f)
                Speed += 1.5f;

            Timer2++;

            NPC npc = Main.npc[(int)Projectile.ai[1]];
            if (npc != null && npc.active && npc.ModNPC is ProjectNucleus boss)
            {
                if (Projectile.Center.X > (boss.Center.X + 974) || Projectile.Center.X < (boss.Center.X - 974))
                    Projectile.Kill();

                if (Projectile.Center.Y > (boss.Center.Y + 527) || Projectile.Center.Y < (boss.Center.Y - 527))
                    Projectile.Kill();
            }

            bool InsideLiquid = false;

            for (int p = 0; p < Main.maxProjectiles; p++)
            {
                if (p < Main.maxPlayers)
                {
                    Player player2 = Main.player[p];

                    if (player2 != null)
                    {
                        player = player2;
                        break;
                    }
                }

                Projectile projectile = Main.projectile[p];

                if (projectile != null && projectile.active && projectile.ModProjectile is BloodLiquid liquid && liquid.InsideLiquid(Projectile))
                    InsideLiquid = true;
            }



            if (player != null && player.active)
            {
                if (player.Distance(Projectile.Center) < 5000 && Timer2 > 30 && Timer2 < 120)
                {
                    if (Timer2 % 8 == 0)
                    {
                        pos = player.Center + Vector2.One.RotatedBy(Projectile.DirectionTo(player.Center).ToRotation() - MathHelper.PiOver4) * 1750;
                    }
                    Vector2 targetVelocity = Projectile.DirectionTo(pos + Vector2.One.RotatedByRandom(360) * Main.rand.Next(65, 175)) * Speed;
                    Projectile.velocity = Vector2.Lerp(Projectile.velocity * (InsideLiquid ? 0.6f : 1f), targetVelocity, 0.0325f);
                    Timer = 0;
                }
            } 
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (targetHitbox.Distance(Projectile.Center) < (15 * Projectile.scale))
                return true;
            else return false;
        }
        public override void OnSpawn(IEntitySource source)
        {
            TranscendenceUtils.NoExpertProjDamage(Projectile);
        }
        public override void OnKill(int timeLeft)
        {
            TranscendenceUtils.DustRing(Projectile.Center, 8, DustID.TheDestroyer, Color.White, 1f, 2f, 4f);
            int p = Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<NucleusDeathBoom>(), 0, 0f);
            Main.projectile[p].scale = 1f;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;

            TranscendenceUtils.VeryBasicProjOutline(Projectile, Texture, 2, 1f, 0f, 0f, 0.33f, false);


            Asset<Texture2D> sprite2 = TextureAssets.Extra[194];

            MiscShaderData miscShaderData = GameShaders.Misc["LightDisc"];
            miscShaderData.UseImage1(sprite2);
            miscShaderData.UseSaturation(1f);
            miscShaderData.UseOpacity(1.5f);
            miscShaderData.Apply();

            _vertexStrip.PrepareStripWithProceduralPadding(Projectile.oldPos, Projectile.oldRot, StripColors, StripWidth, -Main.screenPosition + Projectile.Size / 2);
            _vertexStrip.DrawTrail();

            Main.pixelShader.CurrentTechnique.Passes[0].Apply();


            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.AlphaBlend, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            TranscendenceUtils.DrawEntity(Projectile, Color.White, Projectile.scale, Texture, Projectile.rotation, Projectile.Center, null);

            return false;
        }

        private static VertexStrip _vertexStrip = new VertexStrip();
        private Color StripColors(float progressOnStrip) =>
            progressOnStrip < 0.05f ? Color.Transparent : Color.Lerp(Color.Red, Color.DarkMagenta, progressOnStrip);
        private float StripWidth(float progressOnStrip) => 24f;
    }
}