using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.Equipment
{
    public class StargazerLaserFriendly : ModProjectile
    {
        public int time = 150;
        public float rot;
        public float rot2;
        public Vector2 Center;
        public Player player;
        public Vector2 Pos;
        public float rotSpeed;
        public float lenght;
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;

            Projectile.DamageType = DamageClass.Generic;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 8;
            Projectile.ownerHitCheck = true;

            Projectile.tileCollide = false;
            Projectile.timeLeft = 120;

            Projectile.friendly = true;
            Projectile.ignoreWater = true;
        }
        public override void AI()
        {
            lenght = 1000;
            Projectile.timeLeft = 120;

            //Dust.NewDustPerfect(player.Center + Vector2.One.RotatedBy(rot) * lenght * 1.325f, ModContent.DustType<ArenaDust>(), Vector2.Zero, 0, Color.Purple, 2f);

            player = Main.player[Projectile.owner];
            if (player == null || !player.active || player.dead || !player.GetModPlayer<TranscendencePlayer>().Stargazer)
                Projectile.Kill();

            if (player.velocity.Length() > 1f) rot = player.velocity.ToRotation() - MathHelper.PiOver4 + MathHelper.Pi;
            Center = player.Center - new Vector2(20 * player.direction, player.height * 0.5f);
            Projectile.Center = Center + Vector2.One.RotatedBy(rot) * 100;
        }
        public override void OnSpawn(IEntitySource source)
        {
            Center = Projectile.Center;
            rot = MathHelper.PiOver4;
            Projectile.velocity = Vector2.Zero;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float reference = float.NaN;

            if (Collision.CheckAABBvLineCollision(targetHitbox.Left(), targetHitbox.Size(), player.Center, player.Center + Vector2.One.RotatedBy(rot) * lenght, 8, ref reference))
                return true;

            else return false;
        }
        public override bool? CanCutTiles() => false;
        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
        public override void PostDraw(Color lightColor)
        {
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 2000;
            SpriteBatch sb = Main.spriteBatch;

            Vector2 pos = Center + Vector2.One.RotatedBy(rot) * 50;
            Vector2 pos2 = Center + Vector2.One.RotatedBy(rot) * 11;

            for (int i = 0; i < 4; i++)
            {
                float pi = MathHelper.TwoPi * i / 4;
                float rot = pi += MathHelper.ToRadians(++Projectile.localAI[2] + 4);

                Vector2 pos3 = Center + Vector2.One.RotatedBy(rot) * 4;

                TranscendenceUtils.DrawProjAnimated(Projectile, Color.Purple * 0.66f, Projectile.scale,
                    "TranscendenceMod/Miscannellous/Assets/StargazerEye", Projectile.rotation, pos3, false, false, false);
            }
            TranscendenceUtils.DrawEntity(Projectile, Color.White, 1f, "TranscendenceMod/Miscannellous/Assets/StargazerEye", 0f, Center, null);

            for (float f = 0; f < 40; f++)
            {
                Vector2 pos3 = Vector2.Lerp(pos, pos + Vector2.One.RotatedBy(rot) * lenght, (f * 261) / 3840f);

                sb.Draw(ModContent.Request<Texture2D>($"{Texture}2").Value, new Rectangle(
                (int)(pos3.X - Main.screenPosition.X), (int)(pos3.Y - Main.screenPosition.Y), 16, 96), null,
                Color.White * 0.25f, Projectile.DirectionTo(Center).ToRotation() + MathHelper.PiOver2, ModContent.Request<Texture2D>($"{Texture}2").Value.Size() * 0.5f, SpriteEffects.None, 0);
            }

            sb.Draw(ModContent.Request<Texture2D>($"{Texture}").Value, new Rectangle(
                (int)(pos2.X - Main.screenPosition.X), (int)(pos2.Y - Main.screenPosition.Y), 16, 14), null,
                Color.White * 0.25f, Projectile.DirectionTo(Center).ToRotation() + MathHelper.PiOver2, ModContent.Request<Texture2D>($"{Texture}").Value.Size() * 0.5f, SpriteEffects.None, 0);
        }
    }
}