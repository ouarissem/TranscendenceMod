using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.Weapons.Melee
{
    public class Starfray_Proj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 20;
            ProjectileID.Sets.TrailingMode[Type] = 3;
        }

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 94;
            Projectile.penetrate = 3;

            Projectile.timeLeft = 300;

            Projectile.friendly = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 7;

            Projectile.DamageType = DamageClass.Melee;
            Projectile.extraUpdates = 2;

            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            Projectile.velocity *= 1.005f;

            Lighting.AddLight(Projectile.Center, 0.75f, 0.66f, 0.25f);
            if (Main.rand.NextBool(5))
            {
                int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.AmberBolt, -Projectile.velocity.X, -Projectile.velocity.Y, 0, default, Main.rand.NextFloat(1f, 1.5f));
                Main.dust[d].noGravity = true;
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            TranscendenceUtils.DrawTrailProj(Projectile, Color.White, Projectile.scale, Texture, false, true, 1f, Vector2.Zero);
            TranscendenceUtils.DrawEntity(Projectile, Color.White, Projectile.scale, Texture, Projectile.rotation, Projectile.Center, null);

            return false;
        }
        public override bool PreKill(int timeLeft)
        {
            TranscendenceUtils.ParticleOrchestra(ParticleOrchestraType.Excalibur, Projectile.Center, Main.player[Projectile.owner].whoAmI);
            return base.PreKill(timeLeft);
        }
    }
}