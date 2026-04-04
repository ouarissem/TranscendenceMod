using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Dusts;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Miscannellous.GlobalStuff;
using TranscendenceMod.Projectiles.NPCs.Bosses.SpaceBoss;

namespace TranscendenceMod.Projectiles.Equipment
{
    public class EmpressLaser : ModProjectile
    {
        public float rot;
        public Vector2 Center;
        public float Height;
        private NPC npc;
        public override string Texture => "TranscendenceMod/Miscannellous/Assets/WaterBeam";
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 5000;
        }
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;

            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;

            Projectile.tileCollide = false;
            Projectile.timeLeft = 30;

            Projectile.DamageType = DamageClass.Generic;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 4;
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
 
            if (player == null || !player.active || player.dead)
            {
                Projectile.Kill();
                return;
            }

            if (Projectile.ai[0] > 0)
                Projectile.ai[0]--;


            player.GetModPlayer<TranscendencePlayer>().CannotUseItems = true;
            player.GetModPlayer<TranscendencePlayer>().CannotUseItemsTimer = 5;

            player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, rot - MathHelper.PiOver4);
            player.direction = Main.MouseWorld.X > player.Center.X ? 1 : -1;
            Center = player.Center;

            if (player.GetModPlayer<TranscendencePlayer>().InfectionAbility && player.GetModPlayer<TranscendencePlayer>().EverglowingCrownEquipped)
            {
                Projectile.scale = 1f;
                Projectile.timeLeft = 20;
            }
            if (Projectile.timeLeft < 15)
                Projectile.scale = MathHelper.Lerp(Projectile.scale, 0f, 1f / 15f);

            Projectile.Center = Center + Vector2.One.RotatedBy(rot) * 30f;
            rot = player.DirectionTo(Main.MouseWorld).ToRotation() - MathHelper.PiOver4;

            if (Projectile.ai[2] > 0)
                Projectile.ai[2]--;
            else
            {
                if (Height < 2000)
                    Height += 100;
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Vector2 pos = target.Center + (target.Center - Center);
            Height = Projectile.Distance(pos) / 3f;
            Projectile.ai[2] = 10;
        }
        public override void OnSpawn(IEntitySource source)
        {
            Center = Projectile.Center;
            rot = Projectile.velocity.ToRotation() - MathHelper.PiOver4;
            Projectile.velocity = Vector2.Zero;
            npc = null;

            Main.player[Projectile.owner].UpdateMaxTurrets();
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float reference = float.NaN;
            if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Center, Projectile.Center + Vector2.One.RotatedBy(rot) * Height, (int)((Lacewing() ? 7.5f : 15f) * Projectile.ai[2] * Projectile.scale), ref reference))
                return true;
            else return false;
        }
        public bool Lacewing()
        {
            Player player = Main.player[Projectile.owner];
            if (player != null && player.active && player.GetModPlayer<TranscendencePlayer>().LacewingTrans)
                return true;
            else return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch sb = Main.spriteBatch;

            //Request the Effect
            var eff = ModContent.Request<Effect>("TranscendenceMod/Miscannellous/Assets/Shaders/Effects/MovingNoiseTrans", AssetRequestMode.ImmediateLoad).Value;
            //Apply Shader Texture
            Texture2D shaderImage = TextureAssets.Extra[193].Value;
            Main.instance.GraphicsDevice.Textures[1] = shaderImage;

            eff.Parameters["uImageSize0"].SetValue(shaderImage.Size() * 2f);
            eff.Parameters["uImageSize1"].SetValue(shaderImage.Size());
            eff.Parameters["uTime"].SetValue(Main.GlobalTimeWrappedHourly);
            eff.Parameters["yChange"].SetValue(Main.GlobalTimeWrappedHourly);

            eff.Parameters["useExtraCol"].SetValue(true);
            eff.Parameters["extraCol"].SetValue(!Main.dayTime ? Main.DiscoColor.ToVector3() : Color.Gold.ToVector3());
            eff.Parameters["useAlpha"].SetValue(false);

            sb.End();
            sb.Begin(default, BlendState.Additive, Main.DefaultSamplerState, default, default, eff, Main.GameViewMatrix.TransformationMatrix);

            Texture2D sprite = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/WaterBeam").Value;

            Vector2 posB = Center + Vector2.One.RotatedBy(rot) * (Height / 2f + 30);
            Vector2 pos2B = Projectile.Center + Vector2.One.RotatedBy(rot) * Height;

            for (int i = 0; i < 3; i++)
            {
                sb.Draw(sprite, new Rectangle(
                    (int)(posB.X - Main.screenPosition.X), (int)(posB.Y - Main.screenPosition.Y), (int)(Projectile.scale * (Lacewing() ? 30f : 40f)), (int)(posB.Distance(pos2B) * 2f)), null,
                    Color.White * 0.66f, posB.DirectionTo(pos2B).ToRotation() + MathHelper.PiOver2, sprite.Size() * 0.5f, SpriteEffects.None, 0);
            }

            sb.End();
            sb.Begin(default, BlendState.Additive, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            for (int i = 0; i < 3; i++)
            {
                sb.Draw(sprite, new Rectangle(
                    (int)(posB.X - Main.screenPosition.X), (int)(posB.Y - Main.screenPosition.Y), (int)(Projectile.scale * (Lacewing() ? 10f : 20f)), (int)(posB.Distance(pos2B) * 2f)), null,
                    Color.White, posB.DirectionTo(pos2B).ToRotation() + MathHelper.PiOver2, sprite.Size() * 0.5f, SpriteEffects.None, 0);

                TranscendenceUtils.DrawEntity(Projectile, Color.White, 0.5f, TranscendenceMod.ASSET_PATH + "/GlowBloom", 0f, Projectile.Center, null);
                TranscendenceUtils.DrawEntity(Projectile, Color.White, 0.5f, TranscendenceMod.ASSET_PATH + "/GlowBloom", 0f, pos2B, null);
            }

            sb.End();
            sb.Begin(default, BlendState.AlphaBlend, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            return false;
        }
    }
}