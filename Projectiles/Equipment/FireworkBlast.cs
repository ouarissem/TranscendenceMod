using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using TranscendenceMod.Buffs.Items.InfectionAccessories;

namespace TranscendenceMod.Projectiles.Equipment
{
    public class FireworkBlast : ModProjectile
    {
        public Color color = Main.DiscoColor;
        public override void SetStaticDefaults() => Main.projFrames[Projectile.type] = 7;
        public override void SetDefaults()
        {
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 18;

            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;

            Projectile.friendly = true;
            Projectile.scale = 6;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (target != null && target.active && !target.boss && target.realLife == -1)
                target.velocity += target.DirectionTo(Projectile.Center) * -12f * target.knockBackResist;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (targetHitbox.Distance(Projectile.Center) < 100 * Projectile.scale)
                return true;
            else return base.Colliding(projHitbox, targetHitbox);
        }
        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, Main.DiscoR / 50f, Main.DiscoG / 50f, Main.DiscoB / 50f);
        }
        public override bool PreDraw(ref Color lightColor) => false;
    }
}