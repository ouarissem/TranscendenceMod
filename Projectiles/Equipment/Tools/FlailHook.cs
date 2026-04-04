using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace TranscendenceMod.Projectiles.Equipment.Tools
{
    public class FlailHook : ModProjectile
    {
        Player player;
        float Rotation;
        bool Attached;
        public int Draw;
        public int Timer;

        Vector2 ownerPos;
        Vector2 pos;
        Vector2 angleToOwner;
        float extenderRot;
        float distance;

        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.tileCollide = false;
        }
        public override Color? GetAlpha(Color lightColor) => lightColor;
        public override bool PreDraw(ref Color lightColor)
        {
            Asset<Texture2D> sprite = TextureAssets.Chain16;

            ownerPos = player.MountedCenter;
            pos = Projectile.Center;
            angleToOwner = ownerPos - pos;
            extenderRot = angleToOwner.ToRotation() - MathHelper.PiOver2;
            distance = angleToOwner.Length();

            Rectangle? chainSourceRectangle = null;
            float chainHeightAdjustment = 0f;

            float chainSegmentLength = (chainSourceRectangle.HasValue ? chainSourceRectangle.Value.Height : sprite.Height()) + chainHeightAdjustment;
            if (chainSegmentLength == 0)
                chainSegmentLength = 10;

            //Hopefully fix infinite loops
            if (++Draw % 2 == 0)
            {
                while (chainSegmentLength >= 0 && distance > 20 && !float.IsNaN(distance))
                {
                    angleToOwner /= distance;
                    angleToOwner *= sprite.Height();

                    pos += angleToOwner;
                    angleToOwner = ownerPos - pos;
                    distance = angleToOwner.Length();

                    Main.EntitySpriteDraw(sprite.Value, pos - Main.screenPosition, sprite.Value.Bounds, Lighting.GetColor((int)pos.X / 16, (int)(pos.Y / 16f)), extenderRot, sprite.Size() * 0.5f, 1, SpriteEffects.None, 0);
                }
            }
            return true;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity *= 0.9f;
            if (++Projectile.localAI[0] > 2)
            {
                Projectile.velocity = Vector2.Zero;
                Attached = true;
            }
            return false;
        }
        public override void AI()
        {
            player = Main.player[Projectile.owner];
            player.heldProj = Projectile.whoAmI;

            if (!player.active || player.dead)
                Projectile.Kill();

            if (Projectile.Center.X > player.Center.X)
                player.ChangeDir(1);
            else if (Projectile.Center.X < player.Center.X)
                player.ChangeDir(-1);

            if (++Projectile.ai[1] < 60)
            {
                Projectile.localAI[1]++;
            }
            if (Projectile.ai[2] != 1)
            {
                Projectile.rotation = Projectile.DirectionTo(player.Center).ToRotation() + MathHelper.PiOver2 + MathHelper.Pi;
                Vector2 pos2 = Vector2.One.RotatedBy(Projectile.ai[1] / 4) * (Projectile.localAI[1] * 0.5f);
                Projectile.Center = player.Center + new Vector2(pos2.X, pos2.Y / 2);
                if (player.releaseUseItem && !Collision.SolidCollision(Projectile.position.ToWorldCoordinates(), 16, 16))
                {
                    Projectile.velocity = Projectile.DirectionTo(Main.MouseWorld) * (Projectile.localAI[1] * 0.75f);
                    Projectile.ai[2] = 1;
                }
            }
            else
            {
                if (++Timer > 2)
                    Projectile.tileCollide = true;
                if (!Attached)
                {
                    Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
                    Projectile.velocity.Y += 1.5f;
                }
            }
            if (Projectile.Distance(player.Center) > (Projectile.localAI[1] * 15) || Projectile.localAI[2] == 1)
            {
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2 + MathHelper.Pi;
                Projectile.velocity = Projectile.DirectionTo(player.Center) * 25;
                Projectile.ai[2] = 1;
                Projectile.localAI[2] = 1;
                Projectile.tileCollide = false;

               if (player.Distance(Projectile.Center) < 50)
                    Projectile.Kill();
            }
            if (Attached)
            {
                player.velocity += player.DirectionTo(Projectile.Center) * 1.25f;
                player.GetModPlayer<TranscendencePlayer>().CannotUseItems = true;
                player.GetModPlayer<TranscendencePlayer>().CannotUseItemsTimer = 15;
                if (player.controlUseItem || Collision.SolidCollision(Projectile.position.ToWorldCoordinates(), 16, 16))
                {
                    Projectile.localAI[2] = 1;
                    Projectile.tileCollide = false;
                }
            }
        }
    }
}