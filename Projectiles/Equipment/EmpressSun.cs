using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Buffs;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.NPCs.Boss.Seraph;
using TranscendenceMod.Projectiles.NPCs.Bosses.Nucleus;
using TranscendenceMod.Projectiles.NPCs.Bosses.SpaceBoss;

namespace TranscendenceMod.Projectiles.Equipment
{
    public class EmpressSun : ModProjectile
    {
        public bool LacewingMode;
        public override string Texture => "TranscendenceMod/Miscannellous/Assets/Trail";

        public override void SetDefaults()
        {
            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;

            Projectile.width = 46;
            Projectile.height = 46;
            Projectile.timeLeft = 450;

            Projectile.DamageType = DamageClass.Generic;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.extraUpdates = 1;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 15;

            ProjectileID.Sets.TrailCacheLength[Type] = 20;
            ProjectileID.Sets.TrailingMode[Type] = 3;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;
            Player player = Main.player[Projectile.owner];

            Vector2 origin = new Vector2(Projectile.width, Projectile.height) / 2f;
            if (Projectile.velocity != Vector2.Zero)
            {
                spriteBatch.End();
                spriteBatch.Begin(default, BlendState.Additive, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);

                for (int i = 0; i < Projectile.oldPos.Length; i++)
                {
                    if (i > 1)
                    {
                        TranscendenceUtils.DrawEntity(Projectile, Main.dayTime ? Color.Gold : Main.hslToRgb(i / (float)Projectile.oldPos.Length, 1f, 0.5f), Projectile.scale, Texture, Projectile.oldRot[i] - MathHelper.PiOver2, Projectile.oldPos[i] + origin, null);
                        TranscendenceUtils.DrawEntity(Projectile, Color.White, Projectile.scale * 0.75f, Texture, Projectile.oldRot[i] - MathHelper.PiOver2, Projectile.oldPos[i] + origin, null);
                    }
                }

                spriteBatch.End();
                spriteBatch.Begin(default, BlendState.AlphaBlend, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);
            }
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);

            Projectile.velocity *= 0.9f;
            Projectile.damage = (int)(Projectile.damage * 0.6f);

            SoundEngine.PlaySound(SoundID.DD2_DarkMageHealImpact with { MaxInstances = 0 }, target.Center);
            TranscendenceUtils.ParticleOrchestra(Main.dayTime ? ParticleOrchestraType.TrueExcalibur : ParticleOrchestraType.RainbowRodHit, target.Center, Projectile.owner);

            if (Projectile.timeLeft < 299)
                Projectile.Kill();
        }

        public override void OnSpawn(IEntitySource source)
        {
            base.OnSpawn(source);
            Projectile.scale = 0f;
            Projectile.ai[1] = Main.rand.NextFloat(0.0375f, 0.0675f);
            Projectile.ai[2] = 1f;

            if (Main.player[Projectile.owner].GetModPlayer<TranscendencePlayer>().LacewingTrans)
            {
                Projectile.ai[2] /= 2f;
                Projectile.width /= 2;
                Projectile.height /= 2;
            }
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            if (player == null || !player.active || player.dead)
                Projectile.Kill();

            if (Projectile.timeLeft < 120)
                Projectile.scale = MathHelper.Lerp(Projectile.scale, 0f, 1f / 120f);
            else if (Projectile.scale < 1f)
                Projectile.scale = MathHelper.Lerp(Projectile.scale, Projectile.ai[2], 1f / 45f);

            if (Projectile.timeLeft > 300)
            {
                Projectile.ai[0] += Projectile.ai[1] * 2f;
                Vector2 vec = Vector2.One.RotatedBy(Projectile.ai[0]) * (175f * Projectile.ai[2]);

                Projectile.Center = player.Center + new Vector2(vec.X / 2f, vec.Y).RotatedBy(MathHelper.PiOver2 + player.velocity.X * 0.05f);
            }

            Vector2 vel = player.Center;
            float distance = 1250f;

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC n = Main.npc[i];
                if (n != null && n.active && n.Distance(player.Center) < distance && n.chaseable && !n.friendly && !n.dontTakeDamage)
                {
                    distance = Projectile.Distance(n.Center);
                    vel = n.Center;
                }
            }

            float speed = 22f;

            if (Projectile.timeLeft == 299)
                Projectile.velocity = Main.rand.NextVector2CircularEdge(8f, 8f);

            if (Projectile.timeLeft < 289)
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, Projectile.DirectionTo(vel) * speed, 1f / 15f);
        }
    }
}