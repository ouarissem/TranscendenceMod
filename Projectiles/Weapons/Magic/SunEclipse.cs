using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.Weapons.Magic
{
    public class SunEclipse : ModProjectile
    {
        public override string Texture => "TranscendenceMod/Miscannellous/Assets/InvisSprite";
        public override void SetDefaults()
        {
            Projectile.width = 114;
            Projectile.height = 114;
            Projectile.scale = 2f;

            Projectile.timeLeft = 5;
            Projectile.usesLocalNPCImmunity = true;

            Projectile.tileCollide = false;
            Projectile.penetrate = -1;

            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
        }
        public override void OnSpawn(IEntitySource source) { Projectile.Center = Main.MouseWorld; }
        public override bool PreKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.DD2_FlameburstTowerShot, Projectile.Center);
            for (int t = 0; t < 10; t++)
            {
                Dust dust = Dust.NewDustPerfect(Projectile.Center, DustID.SolarFlare, Main.rand.NextVector2CircularEdge(1f, 1f) * 5, 0, Color.White, 1.25f);
                dust.noGravity = true;
            }
            return base.PreKill(timeLeft);
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return targetHitbox.Distance(Projectile.Center) < 40;
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            Projectile.rotation -= 0.033f;
            Projectile.Center = Vector2.Lerp(Projectile.Center, Main.MouseWorld, 0.0125f);

            if (player.channel && player.statMana > player.HeldItem.mana)
                Projectile.timeLeft = 5;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            TranscendenceUtils.DrawEntity(Projectile, Color.White, Projectile.scale * 2f, TextureAssets.Sun.Value, Projectile.rotation, Projectile.Center, null);
            return false;
        }
    }
}