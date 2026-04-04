using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.Equipment
{
    public class ParryVisual : ModProjectile
    {
        public override string Texture => "TranscendenceMod/Miscannellous/Assets/InvisSprite";

        public override void SetDefaults()
        {
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 60;

            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 2;

            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
        }
        public int id;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            id = (int)Projectile.ai[0];

            Projectile.Center = player.Center;

            if (Projectile.ai[2] < 1f)
                Projectile.ai[2] += 1f / 15f;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D sprite = TextureAssets.Item[id].Value;

            float a = Projectile.timeLeft > 30 ? 1f : MathHelper.Lerp(0f, 1f, Projectile.timeLeft / 30f);
            float a2 = Projectile.ai[2] * a;

            TranscendenceUtils.DrawEntity(Projectile, Color.Gray * 0.25f * a, Projectile.scale * 3f * a2 * 1.5f, sprite, 0f, Projectile.Center, null);
            TranscendenceUtils.DrawEntity(Projectile, Color.White * a, Projectile.scale * 3f * a2, sprite, 0f, Projectile.Center, null);

            return false;
        }
    }
}