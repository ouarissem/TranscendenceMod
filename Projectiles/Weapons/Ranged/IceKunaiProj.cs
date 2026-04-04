using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.Weapons.Ranged
{
    public class IceKunaiProj : ModProjectile
    {
        public NPC npc;
        public override string Texture => "TranscendenceMod/Items/Weapons/Ranged/IceKunai";
        public override void SetDefaults()
        {
            Projectile.width = 34;
            Projectile.height = 34;

            Projectile.penetrate = -1;
            Projectile.timeLeft = 120;

            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 18;
        }
        public override void AI()
        {
            if (Projectile.timeLeft < 30)
                Projectile.scale = MathHelper.Lerp(Projectile.scale, 0f, 1f / 30f);

            Dust d = Dust.NewDustPerfect(Projectile.Center, DustID.IceRod, Vector2.Zero);
            d.noGravity = true;

            if (npc != null)
            {
                if (!npc.active)
                    Projectile.Kill();

                Projectile.Center = npc.Center + new Vector2(Projectile.ai[0], Projectile.ai[1]);
                Projectile.rotation = Projectile.DirectionTo(npc.Center).ToRotation() + MathHelper.PiOver4;
            }
            else Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Projectile.ai[2] != 1)
            {
                npc = target;

                Projectile.ai[0] = (Projectile.Center - npc.Center).X;
                Projectile.ai[1] = (Projectile.Center - npc.Center).Y;

                npc = target;
                Projectile.ai[2] = 1;
                Projectile.velocity = Vector2.Zero;
            }
        }
        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item50, Projectile.Center);

            for (int i = 0; i < 5; i++)
                Dust.NewDust(Projectile.Center, 1, 1, DustID.IceRod, Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 2f));
        }
        public override bool PreDraw(ref Color lightColor)
        {
            TranscendenceUtils.DrawEntity(Projectile, Color.White, Projectile.scale, Texture, Projectile.rotation, Projectile.Center, null);
            return false;
        }
    }
}