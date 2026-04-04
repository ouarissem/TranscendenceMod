using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Dusts;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.NPCs
{
    public class TwistedTendril : ModProjectile
    {
        public int DustCh;
        public int Timer;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 20;
            ProjectileID.Sets.TrailingMode[Type] = 3;
        }
        public override void SetDefaults()
        {
            Projectile.aiStyle = 1;
            AIType = ProjectileID.Bullet;

            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.timeLeft = 180;
            Projectile.extraUpdates = 1;

            Projectile.hostile = true;
            Projectile.ignoreWater = true;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.ai[2] != 1)
            {
                int dmg = Projectile.damage;

                TranscendenceUtils.ProjectileRing(Projectile, 3, Projectile.GetSource_FromAI(), Projectile.Center,
                    Type, dmg, 0, 1f, 0, 1f, 1f, -1, 0f);
                TranscendenceUtils.ProjectileRing(Projectile, 3, Projectile.GetSource_FromAI(), Projectile.Center,
                    Type, dmg, 0, 1f, 0, -1f, 1f, -1, 0f);
            }
            return base.OnTileCollide(oldVelocity);
        }
        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 6; i++)
            {
                Dust.NewDust(Projectile.position, 4, 4, DustID.Phantasmal, Main.rand.NextFloat(), Main.rand.NextFloat());
            }
        }
        public override void OnSpawn(IEntitySource source)
        {
            TranscendenceUtils.NoExpertProjDamage(Projectile);

            if (Projectile.ai[2] == 1)
                Projectile.tileCollide = false;
        }
        public override Color? GetAlpha(Color lightColor) => Color.White;
        public override void AI()
        {
            if (Projectile.ai[2] == 1)
                Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(4f) * Projectile.ai[1]);

            if (++Timer % 1 == 0)
            {
                Vector2 pos1 = Projectile.Center + Vector2.One.RotatedBy(Projectile.velocity.ToRotation() + MathHelper.PiOver4) * (float)(Math.Sin(Main.GlobalTimeWrappedHourly * 5) * 15);
                int d = Dust.NewDust(pos1, 1, 1, DustID.Phantasmal, 0, 0, 0, Color.White, 1f);
                Main.dust[d].velocity = Vector2.Zero;
                Main.dust[d].noGravity = true;

                Vector2 pos2 = Projectile.Center + Vector2.One.RotatedBy(Projectile.velocity.ToRotation() - MathHelper.PiOver2 - MathHelper.PiOver4) * (float)(Math.Sin(Main.GlobalTimeWrappedHourly * 5) * 15);
                int d2 = Dust.NewDust(pos2, 1, 1, DustID.Phantasmal, 0, 0, 0, Color.White, 1f);
                Main.dust[d2].velocity = Vector2.Zero;
                Main.dust[d2].noGravity = true;

            }
        }
    }
}