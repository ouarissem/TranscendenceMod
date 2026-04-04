using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.Weapons.Magic
{
    public class MoonEclipse : ModProjectile
    {
        public Vector2 pos;
        public Vector2 revolvePos;
        public override void SetDefaults()
        {
            Projectile.width = 100;
            Projectile.height = 100;

            Projectile.timeLeft = 5;
            Projectile.usesLocalNPCImmunity = true;

            Projectile.tileCollide = false;
            Projectile.penetrate = -1;

            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
        }
        public override void OnSpawn(IEntitySource source) { pos = Main.MouseWorld; }
        public override bool PreKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Tink, Projectile.Center);
            for (int t = 0; t < 10; t++)
            {
                Dust dust = Dust.NewDustPerfect(Projectile.Center, DustID.Stone, Main.rand.NextVector2CircularEdge(1f, 1f) * 5, 0, Color.White, 1.25f);
                dust.noGravity = true;
            }
            return base.PreKill(timeLeft);
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (targetHitbox.Distance(revolvePos) < 25)
                return true;
            return base.Colliding(projHitbox, targetHitbox);
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            float amount = 0.125f;
            Projectile.rotation += amount;

            Vector2 ellipse = Vector2.One.RotatedBy(TranscendenceWorld.UniversalRotation) * 150f;
            revolvePos = pos + new Vector2(ellipse.X, ellipse.Y / 2f);
            pos = Vector2.Lerp(pos, Main.MouseWorld, 0.0125f);

            if (player.channel && player.statMana > player.HeldItem.mana)
                Projectile.timeLeft = 5;

            Vector2 pos2 = revolvePos + Vector2.One.RotatedBy(Projectile.rotation * -2f) * 100;
            Vector2 pos3 = pos2 + Vector2.One.RotatedBy(Projectile.rotation * 2.5f) * 50;
            Projectile.Center = pos3;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            for (int i = 0; i < 128; i++)
            {
                Vector2 pos2 = Vector2.One.RotatedBy((MathHelper.TwoPi * i / 128f) + (TranscendenceWorld.UniversalRotation * 1.1f));
                for (int j = 1; j < 4; j++)
                {
                    float area = 50f * j;
                    area += 100f;
                    TranscendenceUtils.DrawEntity(Projectile, Color.DarkGray * 0.25f, 0.5f, "TranscendenceMod/Miscannellous/Assets/Circle", 0, pos + new Vector2(pos2.X * area, pos2.Y * (area / 2f)), null);
                }
            }

            TranscendenceUtils.DrawEntity(Projectile, Color.White, 1f, "TranscendenceMod/Projectiles/Weapons/Magic/Earth", Projectile.rotation / 25f, revolvePos, null);
            TranscendenceUtils.DrawEntity(Projectile, Color.White, 3f, $"{Texture}", Projectile.rotation / 2f, Projectile.Center, null);

            return false;
        }
    }
}