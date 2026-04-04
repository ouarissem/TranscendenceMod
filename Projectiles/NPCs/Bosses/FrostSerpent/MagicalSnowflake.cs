using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Miscannellous.GlobalStuff;
using TranscendenceMod.NPCs.Boss.FrostSerpent;

namespace TranscendenceMod.Projectiles.NPCs.Bosses.FrostSerpent
{
    public class MagicalSnowflake : ModProjectile
    {
        public float Fade;
        public override void SetDefaults()
        {
            Projectile.width = 52;
            Projectile.height = 52;

            Projectile.tileCollide = false;
            Projectile.hostile = true;
            Projectile.timeLeft = 600;

            Projectile.GetGlobalProjectile<TranscendenceProjectiles>().ModUnparryable = true;
        }
        public override void OnSpawn(IEntitySource source)
        {
            TranscendenceUtils.NoExpertProjDamage(Projectile);
            Projectile.rotation = Main.rand.NextFloat(MathHelper.TwoPi);
        }
        public override bool CanHitPlayer(Player target)
        {
            return Fade > 0.9f && Projectile.scale > 0.9f && Projectile.ai[2] > 90;
        }
        public override void AI()
        {
            if (!NPC.AnyNPCs(ModContent.NPCType<FrostSerpent_Head>()))
                Projectile.Kill();

            if (++Projectile.ai[2] > 30)
            {
                Projectile.velocity *= 0.95f;

                if (Fade < 1f)
                    Fade += 0.075f;
            }
            if (Projectile.timeLeft < 30 && Projectile.scale > 0f)
                Projectile.scale = MathHelper.Lerp(Projectile.scale, 0f, 1f / 30f);

        }
        public override bool PreDraw(ref Color lightColor)
        {
            TranscendenceUtils.DrawEntity(Projectile, Color.Blue * 0.75f, 2.5f * Projectile.scale, "bloom", 0, Projectile.Center, null);
            TranscendenceUtils.DrawEntity(Projectile, Color.DeepSkyBlue * 0.875f, 1.5f * Projectile.scale, "bloom", 0, Projectile.Center, null);

            TranscendenceUtils.VeryBasicProjOutline(Projectile, Texture, 2, 1f, 1f, 1f, 1f, false);

            TranscendenceUtils.DrawEntity(Projectile, Color.Lerp(Color.Blue * 0.33f, Color.White, Fade), Projectile.scale, Texture, Projectile.rotation, Projectile.Center, null);

            return false;
        }
    }
}