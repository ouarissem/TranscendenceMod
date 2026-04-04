using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Dusts;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.NPCs.Guardians
{
    public class FlameSummon : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 20;
            ProjectileID.Sets.TrailingMode[Type] = 3;
        }
        public override void SetDefaults()
        {
            Projectile.aiStyle = -1;

            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.timeLeft = 85;

            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }
        public override void OnSpawn(IEntitySource source)
        {
            TranscendenceUtils.NoExpertProjDamage(Projectile);

            if (Projectile.ai[0] == 1f)
            {
                Projectile.width = 24;
                Projectile.height = 22;
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            string str = Projectile.ai[0] == 1f ? $"Terraria/Images/Item_1274" : Texture;

            TranscendenceUtils.DrawTrailProj(Projectile, Color.White, Projectile.scale, str, false, true, 1f, Vector2.Zero);
            TranscendenceUtils.DrawEntity(Projectile, Color.White, Projectile.scale, str, Projectile.rotation, Projectile.Center, null);

            return false;
        }
        public override void OnKill(int timeLeft)
        {
            base.OnKill(timeLeft);

            if (Projectile.ai[0] == 1f)
            {
                int n = NPC.NewNPC(Projectile.GetSource_Death(), (int)Projectile.Center.X, (int)Projectile.Center.Y, NPCID.SmallSkeleton);
                Main.npc[n].SpawnedFromStatue = true;
                Main.npc[n].defense = 0;

                float rot = Main.rand.NextFloat(0f, MathHelper.TwoPi);
                for (int j = 0; j < 12; j++)
                {
                    Vector2 vec = Vector2.One.RotatedBy(MathHelper.TwoPi * j / 12f) * 25f;

                    Vector2 pos = Projectile.Center + new Vector2(vec.X, vec.Y / 2.5f).RotatedBy(rot) + Vector2.One.RotatedBy(rot + MathHelper.PiOver4) * 25f;
                    Dust d = Dust.NewDustPerfect(pos, DustID.Torch, Vector2.Zero, 0, Color.White, 2f);
                    d.noGravity = true;
                }
            }
        }
        public override void AI()
        {
            Projectile.rotation += 0.05f * Projectile.velocity.Length();

            if (Projectile.timeLeft < 50 && Projectile.timeLeft > 20)
                Projectile.velocity *= 0.9f;

            if (Projectile.timeLeft < 20)
                Projectile.velocity = new Vector2(0, Projectile.ai[0] == 1f ? 5f : 10f);
        }
    }
}