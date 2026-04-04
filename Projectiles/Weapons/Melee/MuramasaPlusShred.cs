using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.Weapons.Melee
{
    public class MuramasaPlusShred : ModProjectile
    {
        public override string Texture => "TranscendenceMod/Miscannellous/Assets/InvisSprite";
        public int HomeTimer;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 60;
            ProjectileID.Sets.TrailingMode[Type] = 3;
        }
        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;

            Projectile.timeLeft = 25;
            Projectile.penetrate = 12;
            Projectile.extraUpdates = 3;

            Projectile.aiStyle = 1;
            AIType = ProjectileID.Stake;
            Projectile.ArmorPenetration = 25;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;

            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Melee;
        }
        public override void AI()
        {
            NPC target = Main.npc[(int)Projectile.ai[1]];

            if (target == null)
            {
                //if (++HomeTimer > 3) Projectile.Kill();
                return;
            }

            if (++HomeTimer < 5)
            {
                Projectile.velocity = Projectile.DirectionTo(target.Center) * 27.5f;
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 150;
            ProjectileID.Sets.TrailingMode[Type] = 3;

            SpriteBatch spriteBatch = Main.spriteBatch;
            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.Additive, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            TranscendenceUtils.DrawTrailProj(Projectile, Color.White, 3.5f, $"Terraria/Images/Projectile_977", true, false, 4.5f, Vector2.Zero);
            TranscendenceUtils.DrawTrailProj(Projectile, Color.DeepSkyBlue, 3.75f, $"Terraria/Images/Projectile_977", true, false, 4.5f, Vector2.Zero);
            TranscendenceUtils.DrawTrailProj(Projectile, Color.White, 1.5f, $"Terraria/Images/Projectile_977", true, false, 4.5f, Vector2.Zero);

            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.AlphaBlend, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);
            return false;
        }
    }
}