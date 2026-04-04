using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using rail;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Dusts;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Miscannellous.GlobalStuff;
using static TranscendenceMod.TranscendenceWorld;

namespace TranscendenceMod.Projectiles.Weapons.Crean
{
    public class CreanStargazerLaser : ModProjectile
    {

        public int time = 30;
        public float rot;
        public Vector2 Center;
        public Projectile ChosenProjectile;
        public Player player;
        public Vector2 Pos;
        public float rotSpeed;
        public float lenght;
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.aiStyle = -1;
 
            Projectile.tileCollide = false;
            Projectile.scale = 0.25f;
            Projectile.timeLeft = 50;

            Projectile.penetrate = -1;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.extraUpdates = 1;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 4;
        }
        public override void AI()
        {
            lenght = 1500;

            Entity ent = ChosenProjectile;
            if (Projectile.ai[2] >= 1f)
            {
                for (int i = 0; i < 2; i++)
                {
                    int num = Main.rand.Next(0, 20);
                    Vector2 pos = Center + Vector2.One.RotatedBy(rot) * 75f;
                    Vector2 pos2 = Vector2.Lerp(pos, pos + Vector2.One.RotatedBy(rot) * 96.5f * 0.7f, num);
                    Color col = Main.hslToRgb(Main.rand.NextFloat(), 1f, 0.5f);

                    Dust.NewDustPerfect(pos2, ModContent.DustType<PlayerCosmicBlood>(), Main.rand.NextVector2CircularEdge(4f, 4f) * Main.rand.NextFloat(1f, 2f), 0, col, Main.rand.NextFloat(1f, 2f));
                    ent = Main.player[Projectile.owner];
                }
            }
            rot = ent.DirectionTo(Main.MouseWorld).ToRotation() - MathHelper.PiOver4 + Projectile.localAI[0];


            if (!ChosenProjectile.active)
                Projectile.Kill();

            if (Projectile.timeLeft < (time - 5) && ChosenProjectile != null && ChosenProjectile.active)
            {
                Center = ChosenProjectile.Center;
                Projectile.Center = Center + Vector2.One.RotatedBy(rot) * 100;
            }

            if (Projectile.timeLeft == time)
                Projectile.scale = Projectile.ai[2];

            if (Projectile.timeLeft < (time - 15) && Projectile.timeLeft > 10 && Projectile.scale < 1f)
                Projectile.scale += 1f / 15f;

            if (Projectile.timeLeft < 10 && Projectile.scale > 0)
            {
                Projectile.scale -= 1f / 10f;
                Projectile.Opacity -= 1f / 10f;
            }
        }
        public override void OnSpawn(IEntitySource source)
        {
            Center = Projectile.Center;
            rot = Projectile.velocity.ToRotation() - MathHelper.PiOver4;

            ChosenProjectile = Main.projectile[(int)Projectile.ai[1]];
            Projectile.timeLeft = time;
            
            player = Main.player[Projectile.owner];

        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);

            Projectile.damage = (int)(Projectile.damage * 0.95f);            
            int laser = ModContent.ProjectileType<CreanLaser>();

            if (player == null || !player.active || player.ownedProjectileCounts[laser] > 0 || Projectile.ai[2] < 1f || Projectile.ai[0] == 1f)
                return;

            for (int i = 0; i < 5; i++)
            {
                Projectile.NewProjectile(Projectile.GetSource_OnHit(target), target.Center, new Vector2(0, 3.75f).RotatedBy(MathHelper.TwoPi * i / 5f + MathHelper.Pi),
                    laser, Projectile.damage / 6, Projectile.knockBack, player.whoAmI, 0f, 0f, 1f);
                Projectile.NewProjectile(Projectile.GetSource_OnHit(target), target.Center, new Vector2(0, 3.75f).RotatedBy(MathHelper.TwoPi * i / 5f + MathHelper.Pi),
                    laser, Projectile.damage / 6, Projectile.knockBack, player.whoAmI, 0f, 1f, -1f);
            }
            Projectile.ai[0] = 1;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float reference = float.NaN;

            if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), ChosenProjectile.Center, ChosenProjectile.Center + Vector2.One.RotatedBy(rot) * lenght, 16 * Projectile.scale, ref reference))
                return Projectile.timeLeft > 2;

            else return false;
        }
        public override bool? CanDamage() => Projectile.timeLeft > 2 && Projectile.timeLeft < (time - 5);
        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
        public override void PostDraw(Color lightColor)
        {
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 2000;
            SpriteBatch sb = Main.spriteBatch;

            Vector2 pos = Center + Vector2.One.RotatedBy(rot) * 70;
            Vector2 pos2 = Center + Vector2.One.RotatedBy(rot) * 32;

            if (Projectile.timeLeft > (time - 10))
                return;

            Texture2D sprite = ModContent.Request<Texture2D>($"{Texture}").Value;
            Texture2D sprite2 = ModContent.Request<Texture2D>($"{Texture}2").Value;
            Color glowCol = new Color(0f, 0.275f, 0.475f) * 2f;

            Texture2D bloom = ModContent.Request<Texture2D>(TranscendenceMod.ASSET_PATH + "/BloomLine2").Value;
            Texture2D bloom2 = ModContent.Request<Texture2D>(TranscendenceMod.ASSET_PATH + "/GlowBloom").Value;

            SpriteBatch spriteBatch = Main.spriteBatch;

            TranscendenceUtils.RestartSB(spriteBatch, BlendState.Additive, null);

            sb.Draw(bloom, new Rectangle(
                (int)(pos.X - Main.screenPosition.X), (int)(pos.Y - Main.screenPosition.Y), (int)(96f * Projectile.scale), 2000), null,
                glowCol * Projectile.Opacity, Projectile.DirectionTo(Center).ToRotation() - MathHelper.PiOver2, bloom.Size() * 0.5f, SpriteEffects.None, 0);

            TranscendenceUtils.RestartSB(spriteBatch, BlendState.AlphaBlend, null);

            if (Projectile.localAI[2] < 40)
                Projectile.localAI[2]++;
            else Projectile.localAI[2] = 0;
            
            for (float f = 0; f < 40; f++)
            {
                float dist = f < 3 ? MathHelper.Lerp(5f, 25f, f / 3f) : 25f;

                Vector2 pos3 = Vector2.Lerp(pos, pos + Vector2.One.RotatedBy(rot) * 96.5f * 0.7f, f);
                Vector2 pos4 = pos3 + Vector2.One.RotatedBy(MathHelper.ToRadians(f * 45f) - Main.GlobalTimeWrappedHourly * 4f) * dist;

                if (Projectile.ai[2] >= 1f)
                {
                    Vector2 npos3 = Vector2.Lerp(pos, pos + Vector2.One.RotatedBy(rot) * 96 * 0.7f, f + 1);
                    Vector2 npos4 = npos3 + Vector2.One.RotatedBy(MathHelper.ToRadians((f + 1) * 45f) - Main.GlobalTimeWrappedHourly * 4f) * dist;

                    Color col = Main.hslToRgb((f - Projectile.localAI[2]) / 40f, 1f, 0.5f) * 2f;

                    sb.Draw(bloom2, new Rectangle(
                        (int)(pos4.X - Main.screenPosition.X), (int)(pos4.Y - Main.screenPosition.Y), (int)(64f * Projectile.scale), (int)pos4.Distance(npos4) * 2), null,
                        new Color(col.R / 255f, col.G / 255f, col.B / 255f, 0f) * Projectile.Opacity, npos4.DirectionTo(pos4).ToRotation() + MathHelper.PiOver2, bloom2.Size() * 0.5f, SpriteEffects.None, 0);

                }

                sb.Draw(sprite2, new Rectangle(
                    (int)(pos3.X - Main.screenPosition.X), (int)(pos3.Y - Main.screenPosition.Y), (int)(24f * Projectile.scale), 96), null,
                    Color.White * Projectile.Opacity, Projectile.DirectionTo(Center).ToRotation() + MathHelper.PiOver2, sprite2.Size() * 0.5f, SpriteEffects.None, 0);

            }

            sb.Draw(sprite, new Rectangle(
                (int)(pos2.X - Main.screenPosition.X), (int)(pos2.Y - Main.screenPosition.Y), (int)(24f * Projectile.scale), 14), null,
                Color.White * Projectile.Opacity, Projectile.DirectionTo(Center).ToRotation() + MathHelper.PiOver2, sprite.Size() * 0.5f, SpriteEffects.None, 0);
        }
    }
}