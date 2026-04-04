using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Buffs;
using TranscendenceMod.Dusts;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Miscannellous.GlobalStuff;

namespace TranscendenceMod.Projectiles
{
    public class CosmosFlames : ModProjectile
    {
        Rectangle rec;
        public float Opacity = 1;
        public float Rotation;
        public Color col = Color.Magenta;
        public override string Texture => "TranscendenceMod/Miscannellous/Assets/InvisSprite";
        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;

            Projectile.aiStyle = 1;
            AIType = ProjectileID.Bullet;
            Projectile.extraUpdates = 2;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Ranged;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;

            Projectile.ownerHitCheck = true;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.timeLeft = 30;
            Projectile.Opacity = 0;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<SpaceDebuff>(), 180);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.projFrames[Type] = 3;

            Rotation += 0.125f;
            SpriteBatch sb = Main.spriteBatch;

            for (int i = 1; i < 5; i++)
            {
                col = Color.Lerp(Color.Lerp(new Color(1f, 0.125f, 0.25f, 0f), new Color(1f, 0.1f, 0.33f, 0f), (float)Math.Sin(Main.GlobalTimeWrappedHourly)), new Color(0.25f, 0.1f, 0.5f, 0f), i / 4f) * 0.75f;

                TranscendenceUtils.DrawProjAnimated(Projectile, col * 0.75f * Projectile.Opacity * (0.35f + (i / 4f)), 0.175f + i * 0.225f * Main.rand.NextFloat(0.66f, 1f), "TranscendenceMod/Miscannellous/Assets/Smoke",
                    Rotation, Projectile.Center + Projectile.velocity * -15 + Projectile.velocity * (i * 10), false, false, false);

                TranscendenceUtils.DrawEntity(Projectile, new Color(1f, 0.25f, 0.65f), 0.75f + (i * 0.5f) * Main.rand.NextFloat(0.6f, 1f), "bloom", 0, Projectile.Center + Projectile.velocity * (i * 4.5f), null);

                //TranscendenceUtils.DrawProjAnimated(Projectile, new Color(1f, 0.5f, 0.85f, 0f), 0.125f + i * 0.125f * Main.rand.NextFloat(0.66f, 1f), "TranscendenceMod/Dusts/Smoke",
                    //Rotation, Projectile.Center + Projectile.velocity * (i * 4), false, false, false);
            }

            return base.PreDraw(ref lightColor);
        }
        public override void AI()
        {
            TranscendenceUtils.AnimateProj(Projectile, 4);
            

            if (Projectile.Opacity < 1) Projectile.Opacity += 0.25f;
            Color col2 = new Color(0.25f, 0.1f, 0.5f) * 0.75f;

            if (Main.rand.NextBool(5))
            {
                int d = Dust.NewDust(Projectile.Center + new Vector2(Projectile.direction < 0 ? 100 : 50, 100) + Projectile.velocity * Main.rand.Next(10, 25), 1, 1, ModContent.DustType<Smoke>(), 10 * Projectile.direction, -15,
                    0, col2, Main.rand.NextFloat(0.4f, 0.9f));
                Main.dust[d].noGravity = true;
            }
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float reference = float.NaN;
            if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center + Projectile.velocity * 4,
                Projectile.Center + Projectile.velocity * 30, 128, ref reference))
            {
                return true;
            }
            else return false;
        }
    }
}