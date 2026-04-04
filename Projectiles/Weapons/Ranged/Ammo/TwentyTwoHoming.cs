using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Dusts;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.Weapons.Ranged.Ammo
{
    public class TwentyTwoHoming : ModProjectile
    {
        public NPC npc;
        public override string Texture => "TranscendenceMod/Miscannellous/Assets/InvisSprite";
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 25;
            ProjectileID.Sets.TrailingMode[Type] = 3;
        }
        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;

            Projectile.aiStyle = -1;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 1;
            Projectile.extraUpdates = 1;

            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }
        public override void OnSpawn(IEntitySource source)
        {
            base.OnSpawn(source);
            npc = Main.npc[(int)Projectile.ai[0]];
        }
        public override void AI()
        {
            if (++Projectile.ai[2] % 2 == 0)
                Dust.NewDustPerfect(Projectile.Center, ModContent.DustType<HolyDust>(), Vector2.Zero);

            if (npc == null || !npc.active)
                Projectile.active = false;

            if (++Projectile.ai[1] < 20)
                return;


            Vector2 targetVelocity = Projectile.DirectionTo(npc.Center) * 12f;
            Projectile.velocity = Vector2.Lerp(Projectile.velocity, targetVelocity, 0.1f);
        }
        public override bool? CanHitNPC(NPC target) => target == npc;
        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
    }
}