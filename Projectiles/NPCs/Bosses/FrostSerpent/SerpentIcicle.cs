using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.NPCs.Bosses.FrostSerpent
{
    public class SerpentIcicle : ModProjectile
    {
        public Vector2 pos;
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;

            Projectile.tileCollide = false;
            Projectile.hostile = true;
            Projectile.timeLeft = 180;
        }
        public override void OnSpawn(IEntitySource source)
        {
            TranscendenceUtils.NoExpertProjDamage(Projectile);
        }
        public override void AI()
        {
            if (Projectile.timeLeft < 30)
            {
                Projectile.scale -= 1f / 30f;
                Projectile.damage = 0;
            }
            Projectile.spriteDirection = Projectile.direction;
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player player = Main.player[i];
                if (player != null && player.active && player.Distance(Projectile.Center) < 1200)
                {
                    if (++Projectile.ai[2] % 4 == 0)
                    {
                        pos = player.Center + Vector2.One.RotatedBy(Projectile.DirectionTo(player.Center).ToRotation() - MathHelper.PiOver4) * 1500;
                    }
                    Vector2 targetVelocity = Projectile.DirectionTo(pos) * 16f;
                    
                    Projectile.velocity = Vector2.Lerp(Projectile.velocity, targetVelocity, 0.0375f);
                }
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            TranscendenceUtils.DrawEntity(Projectile, Color.MidnightBlue * 0.75f, 1.5f * Projectile.scale, "bloom", 0, Projectile.Center, null);
            TranscendenceUtils.DrawEntity(Projectile, Color.DeepSkyBlue * 0.75f, 0.75f * Projectile.scale, "bloom", 0, Projectile.Center, null);

            TranscendenceUtils.VeryBasicProjOutline(Projectile, Texture, 2, 1f, 1f, 1f, 1f, false);

            TranscendenceUtils.DrawEntity(Projectile, Color.White, Projectile.scale, Texture, Projectile.rotation, Projectile.Center, null);

            return false;
        }
    }
}