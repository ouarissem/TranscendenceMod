using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Miscannellous.GlobalStuff;

namespace TranscendenceMod.Projectiles.Equipment
{
    public class SoulEater : ModProjectile
    {
        public Vector2 startVel;
        NPC npc;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 20;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;

            Projectile.timeLeft = 240;
            Projectile.penetrate = 1;

            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Generic;
            Projectile.tileCollide = false;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.NewProjectile(Projectile.GetSource_FromAI(), target.Center, Main.rand.NextVector2Circular(4f, 4f), ProjectileID.VampireHeal, 1, 0, Projectile.owner, 0f, 4f);

            SoundEngine.PlaySound(SoundID.NPCDeath1 with { MaxInstances = 0 }, Projectile.Center);
            Projectile.Kill();
        }
        public override void AI()
        {
            Projectile.spriteDirection = Projectile.direction;
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
         
            Player player = Main.player[Projectile.owner];

            if (player == null || !player.active || player.dead || npc != null && !npc.active)
            {
                Projectile.Kill();
                return;
            }

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC n = Main.npc[i];
                if (n != null && n.active && n.Distance(Projectile.Center) < 750)
                    npc = n;
            }

            if (npc != null && npc.active)
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, Projectile.DirectionTo(npc.Center + Vector2.One.RotatedBy(Projectile.DirectionTo(npc.Center).ToRotation() - MathHelper.PiOver4) * 1000) * 17.5f, 0.0375f);
        }
        private float opac(Projectile projectile) => projectile.timeLeft < 30 ? projectile.timeLeft / 30f : 1f;
        public override Color? GetAlpha(Color lightColor) => Color.White * opac(Projectile);
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;
            var eff = ModContent.Request<Effect>("TranscendenceMod/Miscannellous/Assets/Shaders/Effects/SeraphOutlineShader", AssetRequestMode.ImmediateLoad).Value;

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, eff, Main.GameViewMatrix.TransformationMatrix);

            eff.Parameters["uOpacity"].SetValue(1f * opac(Projectile));
            eff.Parameters["uSaturation"].SetValue(1f);

            eff.Parameters["uRotation"].SetValue(1f);
            eff.Parameters["uTime"].SetValue(0.875f);
            eff.Parameters["uDirection"].SetValue(0f);

            if (Projectile.velocity.Length() > 2)
                TranscendenceUtils.BetterDrawTrailProj(Projectile, Color.DeepSkyBlue, Projectile.scale, Texture, 0.1f, true, 1f, Vector2.Zero, MathHelper.PiOver2);

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            return base.PreDraw(ref lightColor);
        }
    }
}