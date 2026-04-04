using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Miscannellous.GlobalStuff;

namespace TranscendenceMod.Projectiles.Modifiers
{
    public class DragonFlame : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 20;

            Projectile.timeLeft = 240;
            Projectile.penetrate = 1;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 1;

            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Generic;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);

            target.AddBuff(BuffID.BetsysCurse, 240);
        }
        public override Color? GetAlpha(Color lightColor) => Color.White * 0.33f;
        public override void AI()
        {
            if (Projectile.timeLeft < 30)
                Projectile.scale = MathHelper.Lerp(Projectile.scale, 0f, 1f / 30f);
        }
    }
}