using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.Modifiers
{
    public class MothBaby : ModProjectile
    {
        public override string Texture => $"Terraria/Images/NPC_479";
        public Vector2 vel;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 20;
            ProjectileID.Sets.TrailingMode[Type] = 3;
        }

        public override void SetDefaults()
        {
            Projectile.width = 62;
            Projectile.height = 68;

            Projectile.aiStyle = -1;
            Projectile.ArmorPenetration = 10;

            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Generic;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;

            Projectile.penetrate = -1;
            Projectile.timeLeft = 900;
        }
        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.NPCDeath46 with { MaxInstances = 0 }, Projectile.Center);
            if (Projectile.ai[0] == 1f)
            {
                for (int i = 0; i < 5; i++)
                {
                    Gore.NewGore(Projectile.GetSource_Death(), Projectile.Center, Main.rand.NextVector2Circular(6f, 6f), 687 + i);
                }
            }
            else
            {
                for (int i = 0; i < 3; i++)
                {
                    Gore.NewGore(Projectile.GetSource_Death(), Projectile.Center, Main.rand.NextVector2Circular(6f, 6f), 681 + i);
                }
            }

            base.OnKill(timeLeft);
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (Projectile.ai[0] == 1f)
                return targetHitbox.Distance(Projectile.Center) < (100f * Projectile.scale);

            return targetHitbox.Distance(Projectile.Center) < (20f * Projectile.scale);
        }
        public override void AI()
        {
            //Face in a direction
            Projectile.direction = Projectile.velocity.X > 0f ? -1 : 1;
            Projectile.spriteDirection = Projectile.direction;

            Player player = Main.player[Projectile.owner];

            if (player == null || !player.active || player.dead || player.GetModPlayer<TranscendencePlayer>().SilkyEgg < 1)
            {
                Projectile.Kill();
                return;
            }

            if (++Projectile.ai[1] < 30f)
            {
                Projectile.velocity.Y += 0.025f;
                Projectile.rotation += 0.7f;
                return;
            }


            if (Projectile.ai[0] == 1f)
            {
                if (++Projectile.frameCounter > 5)
                {
                    Projectile.frameCounter = 0;
                    if (++Projectile.frame >= 6)
                    {
                        Projectile.frame = 0;
                    }
                }
            }
            else
            {
                if (++Projectile.frameCounter > 5)
                {
                    Projectile.frameCounter = 0;
                    if (++Projectile.frame >= 3)
                    {
                        Projectile.frame = 0;
                    }
                }
            }

            //Tilt a bit while moving
            Projectile.rotation = MathHelper.Lerp(Projectile.rotation, Projectile.velocity.X * 0.025f, 0.1f);

            float distance = 1250f;
            NPC npc = null;

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC n = Main.npc[i];
                if (n != null && n.active && n.Distance(player.Center) < distance && n.chaseable && !n.friendly && !n.dontTakeDamage)
                {
                    distance = Projectile.Distance(n.Center);
                    npc = n;
                }
            }

            if (npc != null && npc.active)
            {
                float speed = 8f;

                if (Projectile.ai[0] == 1f)
                    speed = 32f;

                if (++Projectile.ai[2] < 120)
                {
                    Projectile.velocity = Vector2.Lerp(Projectile.velocity, Projectile.DirectionTo(npc.Center).RotatedByRandom(0.0125f) * speed, 1f / 30f);
                }
                else
                {
                    Projectile.velocity *= 0.8f;
                    if (Projectile.ai[2] > 150)
                        Projectile.ai[2] = 0f;
                }
            }
            else
            {
                if (Projectile.Distance(player.Center) > 375f)
                    Projectile.velocity = Vector2.Lerp(Projectile.velocity, Projectile.DirectionTo(player.Center) * 24f, 1f / 60f);
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, Main.rand.NextVector2CircularEdge(16f, 16f), 0.025f);
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.ai[1] < 30f)
            {
                TranscendenceUtils.DrawEntity(Projectile, lightColor, Projectile.scale, $"Terraria/Images/NPC_478",
                    Projectile.rotation, Projectile.Center, null, Projectile.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally);

                return false;
            }

            SpriteEffects se = SpriteEffects.None;
            if (Projectile.direction == 1)
                se = SpriteEffects.FlipHorizontally;

            string Sprite = Texture;

            if (Projectile.ai[0] == 1f)
            {
                Sprite = $"Terraria/Images/NPC_477";
                Rectangle rec = new Rectangle(0, Projectile.frame * Projectile.height, Projectile.width, Projectile.height);

                if (Projectile.ai[2] > 60f)
                {
                    Texture2D sprite = ModContent.Request<Texture2D>(Sprite).Value;

                    for (int i = 1; i < Projectile.oldPos.Length; i++)
                    {
                        Vector2 pos = Projectile.oldPos[i] + Projectile.Size * 0.5f - Main.screenPosition;
                        float Fade = (Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length;
                        Main.EntitySpriteDraw(sprite, pos, rec, new Color(100, 100, 100) * 0.375f * Fade, Projectile.rotation, rec.Size() * 0.5f, Projectile.scale, se);
                    }
                }

                TranscendenceUtils.DrawEntity(Projectile, lightColor, Projectile.scale, ModContent.Request<Texture2D>(Sprite).Value, Projectile.rotation,
                    Projectile.Center, rec, rec.Size() * 0.5f, se);
            }
            else
            {
                Rectangle rec = new Rectangle(0, Projectile.frame * 68, 62, 68);

                if (Projectile.ai[2] > 60f)
                {
                    Texture2D sprite = ModContent.Request<Texture2D>(Sprite).Value;

                    for (int i = 1; i < Projectile.oldPos.Length; i++)
                    {
                        Vector2 pos = Projectile.oldPos[i] + Projectile.Size * 0.5f - Main.screenPosition;
                        float Fade = (Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length;
                        Main.EntitySpriteDraw(sprite, pos, rec, new Color(100, 100, 100) * 0.375f * Fade, Projectile.rotation, rec.Size() * 0.5f, Projectile.scale, se);
                    }
                }

                TranscendenceUtils.DrawEntity(Projectile, lightColor, Projectile.scale, ModContent.Request<Texture2D>(Sprite).Value, Projectile.rotation,
                    Projectile.Center, rec, rec.Size() * 0.5f, se);
            }

            return false;
        }
    }
}