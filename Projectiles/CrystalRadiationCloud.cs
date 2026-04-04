using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using TranscendenceMod.Buffs;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles
{
    public class CrystalRadiationCloud : ModProjectile
    {
        public int Timer;
        public Vector2[] pos = new Vector2[5];
        public float fade;
        public float endFade;
        public override string Texture => "TranscendenceMod/Miscannellous/Assets/Smoke";
        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;

            Projectile.timeLeft = 90;
            Projectile.aiStyle = -1;

            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
        }
        public override void AI()
        {
            if (Projectile.timeLeft < 60)
                endFade -= 1 / 60f;
            if (Timer > 10)
                fade -= 1 / 15f;
            if (++Timer > 20)
            {
                for (int i = 0; i < 5; i++)
                    pos[i] = Projectile.Center + Vector2.One.RotatedByRandom(360) * Main.rand.NextFloat(25f, 45f) * endFade;
                Timer = 0;
            }
            if (Timer < 5)
                fade += 1 / 5f;

            if (Main.LocalPlayer.Distance(Projectile.Center) < 150)
                Main.LocalPlayer.AddBuff(ModContent.BuffType<SpaceDebuff>(), 180);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            for (int i = 0; i < 4; i++)
            {
                SpriteBatch sb = Main.spriteBatch;
                sb.End();
                sb.Begin(default, BlendState.Additive, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);

                TranscendenceUtils.DrawEntity(Projectile, Color.Magenta * 0.2f * fade * endFade, 2f * endFade, Texture, 0, pos[i] + new Vector2(0, 400 * endFade), new Rectangle(0, 0, 250, 200));

                sb.End();
                sb.Begin(default, BlendState.AlphaBlend, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);
            }
            return false;
        }
        public override void OnSpawn(IEntitySource source)
        {
            base.OnSpawn(source);
            Timer = 44;
            endFade = 1f;
        }
    }
}