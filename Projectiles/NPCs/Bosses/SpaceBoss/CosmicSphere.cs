using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Dusts;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.NPCs.Bosses.SpaceBoss
{
    public class CosmicSphere : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 17;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.aiStyle = 1;
            AIType = ProjectileID.Stake;

            Projectile.width = 36;
            Projectile.height = 36;
            Projectile.timeLeft = 360;

            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }
        public override void AI()
        {
            if (Projectile.timeLeft < 30)
                Projectile.scale -= 1 / 30f;
        }
        public override void OnSpawn(IEntitySource source)
        {
            TranscendenceUtils.NoExpertProjDamage(Projectile);
            Projectile.rotation = Main.rand.Next(-360, 360);
            for (int i = 0; i < 5; i++)
            {
                Dust.NewDustPerfect(Projectile.Center, ModContent.DustType<NovaDust>(), new Vector2(0, 5).RotatedBy(MathHelper.TwoPi * i / 5 + MathHelper.ToRadians(Projectile.rotation)));
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            TranscendenceUtils.DrawEntity(Projectile, TranscendenceWorld.CosmicPurple * 0.8f, 1.25f * Projectile.scale, "bloom", 0, Projectile.Center, null);

            SpriteBatch sb = Main.spriteBatch;

            if (Projectile.velocity.Length() > 0f)
            {
                sb.End();
                sb.Begin(default, BlendState.Additive, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);

                TranscendenceUtils.BetterDrawTrailProj(Projectile, TranscendenceWorld.CosmicPurple, Projectile.scale * 1.5f,
                    "TranscendenceMod/Miscannellous/Assets/Trail", 0f, true, 4f * Projectile.scale, Vector2.Zero, MathHelper.PiOver2);

                TranscendenceUtils.BetterDrawTrailProj(Projectile, Color.White * 0.75f, Projectile.scale,
                    "TranscendenceMod/Miscannellous/Assets/Trail", 0f, true, 4f * Projectile.scale, Vector2.Zero, MathHelper.PiOver2);
            }

            TranscendenceUtils.VeryBasicProjOutline(Projectile, Texture, 2f, 1f, 1f, 1f, 1f, false);

            TranscendenceUtils.DrawEntity(Projectile, Color.White, Projectile.scale, $"{Texture}", Projectile.rotation, Projectile.Center, null);
            return false;
        }
    }
}