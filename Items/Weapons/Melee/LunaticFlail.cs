using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using Terraria.Graphics.CameraModifiers;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Dusts;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Miscannellous.Rarities;

namespace TranscendenceMod.Items.Weapons.Melee
{
    public class LunaticFlail : ModItem
    {
        int projectile = ModContent.ProjectileType<LunaticFlailProj>();
        public override void SetStaticDefaults() => CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        public override void SetDefaults()
        {
            Item.damage = 775;
            Item.DamageType = DamageClass.MeleeNoSpeed;

            Item.width = 24;
            Item.height = 24;

            Item.useTime = 25;
            Item.useAnimation = 25;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 6f;

            Item.shoot = projectile;

            Item.value = Item.sellPrice(gold: 25);
            Item.rare = ModContent.RarityType<CosmicRarity>();

            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.channel = true;
        }
        public override bool CanShoot(Player player)
        {
            return player.ownedProjectileCounts[projectile] == 0;
        }
    }
    internal class LunaticFlailProj : ModProjectile
    {
        public override string Texture => "TranscendenceMod/Projectiles/NPCs/Bosses/SpaceBoss/Moon";
        public bool Released;
        public Vector2 vel;

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.scale = 0.25f;

            Projectile.DamageType = DamageClass.MeleeNoSpeed;

            Projectile.ownerHitCheck = true;
            Projectile.netImportant = true;

            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 5;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 7;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode, Projectile.Center);

            if (Projectile.ai[0] == 1)
                Projectile.velocity = Projectile.DirectionTo(target.Center) * -20f;

            Main.instance.CameraModifiers.Add(new PunchCameraModifier(new Vector2(Main.rand.Next(-10, 10)),
                new Vector2(Main.rand.NextFloatDirection()), Released ? 275 : 5, 35, 5, -1, null));
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.ai[0] != 1 && Released)
            {
                Projectile.velocity = -oldVelocity;
                Projectile.ai[0] = 1;

                SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode, Projectile.Center);

                Main.instance.CameraModifiers.Add(new PunchCameraModifier(new Vector2(Main.rand.Next(-10, 10)),
                    new Vector2(Main.rand.NextFloatDirection()), 275, 35, 5, -1, null));
            }
            return false;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) => targetHitbox.Distance(Projectile.Center) < (220 * Projectile.scale);
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            player.heldProj = Projectile.whoAmI;

            if (player.HeldItem.type == ModContent.ItemType<LunaticFlail>() && !player.dead)
                Projectile.timeLeft = 5;

            player.ChangeDir(Projectile.Center.X > player.Center.X ? 1 : -1);

            Projectile.tileCollide = Released && Projectile.ai[0] != 1;

            Projectile.rotation += 0.1f;

            if (Projectile.ai[0] == 1)
            {
                Projectile.scale = MathHelper.Lerp(Projectile.scale, 0.25f, 1f / 10f);

                if (Projectile.Distance(player.Center) < 75)
                    Projectile.Kill();
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, Projectile.DirectionTo(player.Center) * 28f, 0.075f);
            }

            if (player.channel && !Released || Projectile.ai[2] < 200)
            {
                Projectile.ai[2] += 25;
                Vector2 vec = Vector2.One.RotatedBy(MathHelper.ToRadians(Projectile.ai[2])) * 75f;
                Projectile.Center = player.Center + new Vector2(vec.X, vec.Y / 2f).RotatedBy(player.velocity.X * 0.05f);

                vel = player.DirectionTo(Main.MouseWorld) * 26f;
                return;
            }


            Released = true;

            int d = Dust.NewDust(Projectile.Center + Vector2.One.RotatedBy(Projectile.velocity.ToRotation() + MathHelper.PiOver4) * (180 * Projectile.scale), 1, 1, ModContent.DustType<ArenaDust>(),
                0, 0, 0, Color.Gray, 1f);
            Main.dust[d].velocity = Vector2.Zero;

            int d2 = Dust.NewDust(Projectile.Center + Vector2.One.RotatedBy(Projectile.velocity.ToRotation() - MathHelper.PiOver2 - MathHelper.PiOver4) * (180 * Projectile.scale), 1, 1, ModContent.DustType<ArenaDust>(),
                0, 0, 0, Color.Gray, 1f);
            Main.dust[d2].velocity = Vector2.Zero;


            player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, (player.MountedCenter - Projectile.Center).ToRotation() + MathHelper.PiOver2);

            if (++Projectile.ai[1] < 30)
            {
                if (Projectile.ai[0] != 1)
                    Projectile.velocity = vel;
                Projectile.scale = MathHelper.Lerp(Projectile.scale, 0.66f, 1f / 20f);
            }
            else Projectile.ai[0] = 1;
        }
        public override bool PreDrawExtras()
        {
            Asset<Texture2D> sprite = ModContent.Request<Texture2D>("TranscendenceMod/Items/Weapons/Melee/LunaticFlailProj_Chain");

            Vector2 ownerPos;
            Vector2 pos;
            Vector2 angleToOwner;
            float extenderRot;
            float distance;
            Player player = Main.player[Projectile.owner];

            ownerPos = player.MountedCenter;
            pos = Projectile.Center;
            angleToOwner = ownerPos - pos;
            extenderRot = angleToOwner.ToRotation() - MathHelper.PiOver2;
            distance = angleToOwner.Length();

            Rectangle? chainSourceRectangle = null;
            float chainHeightAdjustment = 0f;

            float chainSegmentLength = (chainSourceRectangle.HasValue ? chainSourceRectangle.Value.Height : sprite.Height()) + chainHeightAdjustment;
            if (chainSegmentLength == 0)
                chainSegmentLength = 10;

            //Hopefully fix infinite loops
            int am = 0;

            while (chainSegmentLength >= 0 && distance > 20 && !float.IsNaN(distance))
            {
                if (++am > 750)
                    break;

                angleToOwner /= distance;
                angleToOwner *= sprite.Height();

                pos += angleToOwner;
                angleToOwner = ownerPos - pos;
                distance = angleToOwner.Length();

                Main.EntitySpriteDraw(sprite.Value, pos - Main.screenPosition, sprite.Value.Bounds, Lighting.GetColor((int)pos.X / 16, (int)(pos.Y / 16f)), extenderRot, sprite.Size() * 0.5f, 1, SpriteEffects.None, 0);
            }
            return true;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 15;
            ProjectileID.Sets.TrailingMode[Type] = 2;

            TranscendenceUtils.DrawEntity(Projectile, Color.White, Projectile.scale, Texture, Projectile.rotation, Projectile.Center, null);

            return false;
        }
    }
}