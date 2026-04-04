using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.Weapons.Melee
{
    public class SunBladeProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 40;
            ProjectileID.Sets.TrailingMode[Type] = 3;
        }

        public override void SetDefaults()
        {
            Projectile.width = 80;
            Projectile.height = 80;
            Projectile.penetrate = 1;

            Projectile.light = 0.5f;
            Projectile.alpha = 1;
            Projectile.timeLeft = 300;

            Projectile.friendly = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 3;

            Projectile.DamageType = DamageClass.Melee;

            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.Opacity = 0;
        }
        public static bool Tiles(Projectile projectile) => Collision.SolidCollision(projectile.Center - new Vector2(5), 10, 10) || projectile.ai[1] == 1;
        public Color col(Projectile projectile) => Tiles(projectile) ? Color.DarkGray * 0.25f : Color.White;
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;

            if (Tiles(Projectile) && Projectile.ai[1] != 1)
            {
                Projectile.damage /= 3;
                Projectile.ai[1] = 1;
            }

            Projectile.ai[2] += 0.5f;
            Projectile.velocity = Vector2.Lerp(Projectile.velocity, Projectile.velocity * 0.5f, Projectile.ai[2] / 40f);
            if (Projectile.Opacity < 1 && Projectile.timeLeft > 290) Projectile.Opacity += 0.1f;
            if (Projectile.timeLeft < 250)
            {
                Projectile.Opacity -= 0.1f;
                if (Projectile.Opacity < 0.1f)
                    Projectile.Kill();
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            TranscendenceUtils.ParticleOrchestra(ParticleOrchestraType.Excalibur, Projectile.Center, Main.player[Projectile.owner].whoAmI);
            target.AddBuff(BuffID.Daybreak, 30);
            
            Projectile.NewProjectile(Projectile.GetSource_OnHit(target), target.Center, Vector2.Zero, ModContent.ProjectileType<SunFlame>(), Projectile.damage / 4, 2, Main.player[Projectile.owner].whoAmI, 0, 1);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.Opacity > 0.75f)
                TranscendenceUtils.DrawTrailProj(Projectile, col(Projectile) * Projectile.Opacity, Projectile.scale, $"{Texture}", false, true, 2f, Vector2.Zero);
            return false;
        }
        public override bool PreKill(int timeLeft)
        {
            return base.PreKill(timeLeft);
        }
    }
    public class SunFlame : ModProjectile
    {
        public override string Texture => "TranscendenceMod/Miscannellous/Assets/InvisSprite";
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 35;
            ProjectileID.Sets.TrailingMode[Type] = 3;
        }
        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.aiStyle = 1;
            AIType = ProjectileID.Bullet;
            Projectile.DamageType = DamageClass.Melee;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 15;
            Projectile.penetrate = 2;
            Projectile.timeLeft = 450;

            Projectile.extraUpdates = 2;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }
        public override void AI()
        {
            NPC npc = Projectile.FindTargetWithinRange(500, true);
            //Projectile.rotation = Projectile.DirectionTo(Projectile.velocity).ToRotation();

            if (npc == null || ++Projectile.ai[1] < 15)
                return;

            Vector2 targetVelocity = Projectile.DirectionTo(npc.Center + Vector2.One.RotatedByRandom(360) * Main.rand.Next(20, 130)) * 20;
            Projectile.velocity = Vector2.Lerp(Projectile.velocity, targetVelocity, 0.15f);
            //Projectile.velocity = Projectile.DirectionTo(npc.Center + new Vector2(Main.rand.Next(-60, 60))) * Main.rand.Next(4, 19);
            Projectile.ai[1] = 0;
        }
        public override void OnKill(int timeLeft)
        {
            Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center, Vector2.Zero, ProjectileID.SolarWhipSwordExplosion, (int)(Projectile.damage * 1.25f), 4, Main.player[Projectile.owner].whoAmI, 0, 1);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            //TranscendenceUtils.DrawEntity(Projectile, Color.DeepSkyBlue * 0.85f, 0.65f, "bloom2", 0, Projectile.Center, null);
            TranscendenceUtils.DrawTrailProj(Projectile, Color.White, 1f, TextureAssets.Projectile[ProjectileID.DD2FlameBurstTowerT1Shot].Value, true, true, 3, Vector2.Zero);
            return false;
        }
    }
}