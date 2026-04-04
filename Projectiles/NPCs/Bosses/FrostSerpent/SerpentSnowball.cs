using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.NPCs.Bosses.FrostSerpent
{
    public class SerpentSnowball : ModProjectile
    {
        public Vector2 pos;
        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 16;

            Projectile.tileCollide = false;
            Projectile.hostile = true;
            Projectile.timeLeft = 300;
        }
        public override void OnSpawn(IEntitySource source)
        {
            TranscendenceUtils.NoExpertProjDamage(Projectile);
        }
        public override void AI()
        {
            if (Projectile.timeLeft < 30 && Projectile.scale > 0f)
                Projectile.scale -= 1f / 30f;

            if (Projectile.timeLeft < 240)
                Projectile.velocity.Y = MathHelper.Lerp(Projectile.velocity.Y, 15, 0.0125f);

            if (Main.rand.NextBool(64))
                Dust.NewDustPerfect(Projectile.Center, DustID.Snow, new Vector2(0, 2.5f), 0, default, Main.rand.NextFloat(0.85f, 1.15f) * Projectile.scale);
        }
        public override Color? GetAlpha(Color lightColor) => Color.White;
    }
}