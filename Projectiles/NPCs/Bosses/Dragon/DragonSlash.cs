using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.NPCs.Bosses.Dragon
{
    public class DragonSlash : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 3;
        }
        public override void SetDefaults()
        {
            Projectile.width = 192;
            Projectile.height = 192;

            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 5;
            Projectile.scale = 5f;

            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
        }
        public override void OnSpawn(IEntitySource source)
        {
            base.OnSpawn(source);
            TranscendenceUtils.NoExpertProjDamage(Projectile);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            TranscendenceUtils.DrawProjAnimated(Projectile, Color.White, Projectile.scale, $"{Texture}", Projectile.rotation, Projectile.Center, false, false, false);
            return false;
        }
        public override void AI()
        {
            NPC npc = Main.npc[(int)Projectile.ai[1]];

            TranscendenceUtils.AnimateProj(Projectile, 2);

            Projectile.Center = npc.Center + (Projectile.velocity * 2f);
            Projectile.rotation = Projectile.velocity.ToRotation();

        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) =>
            targetHitbox.Distance(Projectile.Center) < (12 * Projectile.scale);
    }
}