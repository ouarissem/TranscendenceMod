using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Dusts;
using TranscendenceMod.Miscannellous;
namespace TranscendenceMod.Projectiles.Equipment
{
    public class FireworkProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 25;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 22;

            Projectile.aiStyle = 1;
            AIType = ProjectileID.Stake;
            Projectile.timeLeft = 60;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Generic;
            Projectile.noEnchantmentVisuals = true;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }
        public override void AI()
        {
            SoundEngine.PlaySound(SoundID.DD2_KoboldIgnite with { MaxInstances = 5 }, Projectile.Center);

            Dust.NewDustPerfect(Projectile.Center, DustID.Torch, Vector2.Zero);
        }
        public override bool PreKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.DD2_KoboldExplosion, Projectile.Center);

            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Vector2 pos = Projectile.Center + Vector2.One.RotatedBy(MathHelper.TwoPi * j / 8 + MathHelper.ToRadians(i * 9)) * (float)(10 + Math.Sin(i) * 50);
                    Dust.NewDustPerfect(pos, ModContent.DustType<ArenaDust>(), pos.DirectionTo(Projectile.Center) * -15, 0, Main.hslToRgb(i / 8f, 1f, 0.5f), 3f);
                }
            }

            Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center, Vector2.Zero,
                ModContent.ProjectileType<FireworkBlast>(), Projectile.damage, 0f, Main.player[Projectile.owner].whoAmI);

            return base.PreKill(timeLeft);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            TranscendenceUtils.DrawTrailProj(Projectile, lightColor, Projectile.scale, $"{Texture}", false, true, 1, Vector2.Zero);
            return base.PreDraw(ref lightColor);
        }
    }
}