using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous.GlobalStuff;

namespace TranscendenceMod.Projectiles.NPCs.Bosses.SpaceBoss
{
    public class FriendlyNucleusLaser : ModProjectile
    {
        public override string Texture => "TranscendenceMod/Miscannellous/Assets/Trail2";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 40;
            ProjectileID.Sets.TrailingMode[Type] = 3;
        }
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.timeLeft = 70;

            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;

            Projectile.extraUpdates = 2;
            Projectile.penetrate = 15;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 5;

            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }
        public override void OnSpawn(IEntitySource source)
        {
            
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D sprite = ModContent.Request<Texture2D>(Texture).Value;
            Vector2 origin = new Vector2(Projectile.width * 0.5f, Projectile.height * 0.5f);

            SpriteBatch sb = Main.spriteBatch;

            sb.End();
            sb.Begin(default, BlendState.Additive, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            for (int i = 1; i < Projectile.oldPos.Length; i++)
            {
                Vector2 pos = Projectile.oldPos[i] - Main.screenPosition + origin + new Vector2(0, Projectile.gfxOffY);

                Main.EntitySpriteDraw(sprite, pos, null, Color.Red, Projectile.oldRot[i] + MathHelper.PiOver2, sprite.Size() * 0.5f, Projectile.scale * 1.5f, SpriteEffects.None);
                Main.EntitySpriteDraw(sprite, pos, null, Color.White, Projectile.oldRot[i] + MathHelper.PiOver2, sprite.Size() * 0.5f, Projectile.scale * 1.25f, SpriteEffects.None);

            }

            sb.End();
            sb.Begin(default, BlendState.AlphaBlend, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            return false;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.damage = (int)(Projectile.damage * 0.95f);

            base.OnHitNPC(target, hit, damageDone);
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                if (targetHitbox.Distance(Projectile.oldPos[i]) < (2 * Projectile.scale))
                    return true;
            }
            return false;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.PiOver2;
            Projectile.spriteDirection = (int)Projectile.velocity.ToRotation();


            if (Projectile.timeLeft < 20)
                Projectile.scale = MathHelper.Lerp(Projectile.scale, 0f, 1f / 20f);

            if (Projectile.scale < 3f)
                Projectile.scale += 1f / 60f;

            if (Projectile.ai[1] == 1)
                if (++Projectile.ai[2] < 40 || Projectile.hostile)
                    Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(Main.rand.Next(-90, 90) * (1f + (float)Math.Sin(TranscendenceWorld.UniversalRotation * 2f) * 0.5f))) * (Projectile.velocity.Length() < 35f ? 1.15f : 1f);
                else
                    Projectile.velocity = Vector2.Lerp(Projectile.velocity, Projectile.DirectionTo(Main.MouseWorld) * 26f, 0.2f);
        }
    }
}