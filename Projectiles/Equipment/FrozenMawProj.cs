using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Dusts;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Miscannellous.GlobalStuff;

namespace TranscendenceMod.Projectiles.Equipment
{
    public class FrozenMawProj : ModProjectile
    {
        public override string Texture => "TranscendenceMod/Items/Accessories/Expert/FrozenMaw";

        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            Projectile.width = 28;
            Projectile.height = 50;

            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;

            Projectile.DamageType = DamageClass.Generic;
            Projectile.timeLeft = 5;

            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            if (player != null && player.active && !player.dead && player.GetModPlayer<TranscendencePlayer>().FrozenMaw)
                Projectile.timeLeft = 5;

            Projectile.Center = player.Center + Vector2.One.RotatedBy(MathHelper.TwoPi * Projectile.ai[1] / Projectile.ai[2] + TranscendenceWorld.UniversalRotation * 3f) * 75f;
            Projectile.rotation = Projectile.DirectionTo(player.Center).ToRotation() + MathHelper.PiOver2;

            if (Projectile.GetGlobalProjectile<TranscendenceProjectiles>().Timer < 3)
                Dust.NewDustPerfect(Projectile.Center, ModContent.DustType<ArenaDust>(), Vector2.Zero, 0, Color.DeepSkyBlue, 1f);
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);

            target.AddBuff(BuffID.Frostburn2, 300);

            for (int i = 0; i < 5; i++)
            {
                Dust.NewDustPerfect(target.Center, ModContent.DustType<SnowflakeDust>(), Main.rand.NextVector2Circular(2.5f, 5f));
            }

            SoundEngine.PlaySound(SoundID.Shatter, Projectile.Center);
            Projectile.Kill();
        }
        public override bool PreDraw(ref Color lightColor)
        {
            TranscendenceUtils.VeryBasicProjOutline(Projectile, Texture, 2f, 1f, 1f, 1f, 1f, false);
            TranscendenceUtils.DrawEntity(Projectile, Color.White, Projectile.scale, Texture, Projectile.rotation, Projectile.Center, null);

            return false;
        }
    }
}