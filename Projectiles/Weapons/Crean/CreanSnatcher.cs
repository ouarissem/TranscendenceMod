using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Items.Weapons;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.Weapons.Crean
{
    public class CreanSnatcher : ModProjectile
    {
        public Vector2 pos;
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 24;

            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;

            Projectile.DamageType = DamageClass.Generic;
            Projectile.timeLeft = 5;

            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            if (player != null && player.active && !player.dead && player.HeldItem.type == ModContent.ItemType<CreanStaff>() && !player.GetModPlayer<TranscendencePlayer>().CannotUseItems)
                Projectile.timeLeft = 5;

            Vector2 vec2 = Vector2.One.RotatedBy(player.DirectionTo(Main.MouseWorld).ToRotation() - MathHelper.PiOver4) * 575f;

            if (Main.MouseWorld.Distance(player.Center) > 75)
                Projectile.ai[0] = Projectile.DirectionTo(player.Center + vec2).ToRotation();

            Vector2 vec = Vector2.One.RotatedBy(MathHelper.TwoPi * Projectile.ai[1] / Projectile.ai[2] + TranscendenceWorld.UniversalRotation * 2f) * 75f;

            Projectile.Center = player.Center + new Vector2(vec.X / 2f, vec.Y).RotatedBy(Projectile.ai[0]);

            int timer = player.GetModPlayer<TranscendencePlayer>().CreanStaffClick;
            float am = timer > 20 ? (40 - timer) / 20f : timer / 20f;

            if (timer == 40)
                pos = player.Center + vec2;

            if (timer > 0)
                Projectile.Center = Vector2.Lerp(Projectile.Center, pos, am * 0.75f);

            Projectile.rotation = Projectile.ai[0] + MathHelper.PiOver2;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);

            target.AddBuff(BuffID.Frostburn2, 300);

            SoundEngine.PlaySound(SoundID.DD2_CrystalCartImpact, Projectile.Center);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            TranscendenceUtils.VeryBasicProjOutline(Projectile, Texture, 2f, 0f, 0.5f, 0.75f, 0.5f, false);
            TranscendenceUtils.DrawEntity(Projectile, Color.White, Projectile.scale, Texture, Projectile.rotation, Projectile.Center, null);

            return false;
        }
    }
}