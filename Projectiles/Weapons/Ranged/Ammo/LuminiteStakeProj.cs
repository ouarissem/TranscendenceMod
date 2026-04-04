using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.Weapons.Ranged.Ammo
{
    public class LuminiteStakeProj : ModProjectile
    {
        public override string Texture => "TranscendenceMod/Items/Weapons/Ranged/Ammo/LuminiteStake";
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 25;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.Stake);
            Projectile.width = 16;
            Projectile.height = 16;

            Projectile.aiStyle = 1;
            AIType = ProjectileID.Stake;

            Projectile.penetrate = 15;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 5;
        }
        public override void OnSpawn(IEntitySource source) => Projectile.extraUpdates *= 3;
        public override Color? GetAlpha(Color lightColor) => Color.White;
        public override void AI()
        {
            Projectile.spriteDirection = -Projectile.direction;
            Projectile.rotation = Projectile.velocity.ToRotation();
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(default, BlendState.Additive, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            TranscendenceUtils.DrawTrailProj(Projectile, Color.White, 1, $"{Texture}", true, true, 1, Vector2.Zero);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(default, BlendState.AlphaBlend, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);
            return base.PreDraw(ref lightColor);
        }
        public override void OnKill(int timeLeft) => SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Main.player[Projectile.owner].ownedProjectileCounts[ModContent.ProjectileType<LuminiteStakeBolt>()] > 5)
                return;

            Projectile.NewProjectile(Projectile.GetSource_OnHit(target), target.Center, new Vector2(Main.rand.Next(-4, 4)),
                ModContent.ProjectileType<LuminiteStakeBolt>(), (int)(Projectile.damage * 0.15f), Projectile.knockBack * 2, Main.player[Projectile.owner].whoAmI);
        }
    }
    public class LuminiteStakeBolt : ModProjectile
    {
        public override string Texture => "TranscendenceMod/Miscannellous/Assets/InvisSprite";
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 35;
            ProjectileID.Sets.TrailingMode[Type] = 3;
        }
        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.aiStyle = -1;
            Projectile.DamageType = DamageClass.Ranged;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 16;
            Projectile.penetrate = 10;

            Projectile.extraUpdates = 3;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }
        public override void AI()
        {
            NPC npc = Projectile.FindTargetWithinRange(1800, true);
            Projectile.spriteDirection = -Projectile.direction;
            if (npc == null || ++Projectile.ai[1] < 15)
                return;
            Vector2 targetVelocity = Projectile.DirectionTo(npc.Center + Vector2.One.RotatedByRandom(360) * Main.rand.Next(20, 130)) * 24f;
            Projectile.velocity = Vector2.Lerp(Projectile.velocity, targetVelocity, 0.2f);
            //Projectile.velocity = Projectile.DirectionTo(npc.Center + new Vector2(Main.rand.Next(-60, 60))) * Main.rand.Next(4, 19);
            Projectile.ai[1] = 0;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(default, BlendState.Additive, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            TranscendenceUtils.BetterDrawTrailProj(Projectile, Color.Aqua, 0.125f, "TranscendenceMod/Miscannellous/Assets/GlowBloom", 0.065f, false, 1f, Vector2.Zero, 0);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(default, BlendState.AlphaBlend, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);
            return false;
        }
    }
}