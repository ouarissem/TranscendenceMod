using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Dusts;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Miscannellous.Rarities;
using TranscendenceMod.Projectiles.Weapons.Melee;

namespace TranscendenceMod.Items.Weapons.Melee
{
    public class MountaintopGlacier : ModItem
    {
        int projectile = ModContent.ProjectileType<MountaintopGlacier_Proj>();
        int projectile2 = ModContent.ProjectileType<MountaintopGlacier_Proj2>();
        int combo;

        public override void SetStaticDefaults()
        {
            ItemID.Sets.Spears[Type] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.damage = 195;
            Item.crit = 20;
            Item.DamageType = DamageClass.Melee;

            Item.width = 18;
            Item.height = 18;

            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;

            Item.knockBack = 4;
            Item.shoot = projectile;
            Item.shootSpeed = 12;

            Item.value = Item.sellPrice(gold: 20);
            Item.rare = ModContent.RarityType<ModdedPurple>();

            Item.noUseGraphic = true;
            Item.noMelee = true;
        }
        public override bool CanShoot(Player player)
        {
            return true;
        }
        public override void ModifyShootStats(Player player, ref Vector2 position,
            ref Vector2 velocity, ref int type, ref int damage, ref float knockback) => type = ProjectileID.None;
        public override bool CanUseItem(Player player)
        {
            if (player.ownedProjectileCounts[projectile] > 0 || player.ownedProjectileCounts[projectile2] > 0)
                return false;
            else return base.CanUseItem(player);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float ai = combo % 2 == 0 ? 1 : -1;

            if (combo > 7)
                combo = 0;

            Vector2 vel = combo < 7 ? velocity.RotatedBy(MathHelper.PiOver2 * -ai) : velocity;
            if (player.ownedProjectileCounts[projectile] < 1 && player.ownedProjectileCounts[projectile2] < 1)
                Projectile.NewProjectile(source, position, velocity, combo < 7 ? projectile : projectile2, (int)(combo < 7 ? damage * 1.5f : damage), knockback, player.whoAmI, 0, 0, ai);
            combo++;
            return false;
        }
    }
    internal class MountaintopGlacier_Proj : ModProjectile
    {
        public float Timer;
        public float RotSpeed = 0.025f;
        public bool boolean;
        public float vel = 1f;

        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 118;
            Projectile.DamageType = DamageClass.MeleeNoSpeed;

            Projectile.ownerHitCheck = true;
            Projectile.netImportant = true;
            Projectile.noEnchantmentVisuals = true;

            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 4;

            Projectile.timeLeft = 64;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];
            target.AddBuff(BuffID.Frostburn2, 90);
        }
        public override void OnSpawn(IEntitySource source)
        {
            Timer = MathHelper.PiOver2 * Projectile.ai[2];
            SoundEngine.PlaySound(SoundID.Item1, Projectile.Center);
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float reference = float.NaN;
            if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center,
                Projectile.Center + Projectile.velocity * 40 * (1 * 0.275f * Projectile.scale), 32, ref reference))
            {
                return true;
            }
            else return false;
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            player.heldProj = Projectile.whoAmI;
            player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, (player.MountedCenter - Projectile.Center).ToRotation() + MathHelper.PiOver2);

            if (player.HeldItem.type != ModContent.ItemType<MountaintopGlacier>() || player.dead)
                Projectile.Kill();

            Projectile.scale += 0.025f;
            Vector2 dustPos = Projectile.Center + Projectile.velocity * 20 * (1 * 0.275f * Projectile.scale);

            if (Projectile.scale > 2.2f)
                Projectile.noEnchantmentVisuals = false;
            else Projectile.noEnchantmentVisuals = true;

            Projectile.EmitEnchantmentVisualsAt(Projectile.Center + Vector2.One.RotatedBy(Projectile.velocity.ToRotation() - MathHelper.PiOver4) * 170, 1, 1);
            Projectile.Center = player.Center + (Projectile.velocity * vel * Projectile.scale);
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
            
            Projectile.velocity = Projectile.velocity.RotatedBy(Timer / 40f);
            if (Projectile.timeLeft < 52)
            {
                int d = Dust.NewDust(dustPos, 1, 1, ModContent.DustType<Smoke>(), 0, 0, 0, Color.DeepSkyBlue * 0.25f, 0.75f);
                Main.dust[d].noGravity = true;
                Main.dust[d].velocity = Vector2.Zero;

                if (++Projectile.ai[0] % 20 == 0)
                {
                    Projectile.NewProjectile(Projectile.GetSource_FromAI(), dustPos, dustPos.DirectionTo(Main.MouseWorld) * 12f, ModContent.ProjectileType<GlacierSnow>(),
                        Projectile.damage / 4, Projectile.knockBack / 2f, player.whoAmI, 0, 0, -1);
                }
            }

            if (Projectile.timeLeft < 18)
                vel -= 1f / 18f;

            Timer = MathHelper.Lerp(MathHelper.PiOver2 * Projectile.ai[2], -MathHelper.PiOver2 * Projectile.ai[2], RotSpeed);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D sprite = ModContent.Request<Texture2D>(Texture).Value;
            Texture2D sprite2 = ModContent.Request<Texture2D>(Texture + "_Glow").Value;

            Vector2 origin = new Vector2(sprite.Width * 0.5f, Projectile.height * 0.5f);

            float rot = Projectile.rotation + (boolean ? 0 : MathHelper.PiOver2);
            SpriteEffects spriteEffects = boolean ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            for (int i = 0; i < 35; i++)
            {
                float smear = MathHelper.Lerp(0, -MathHelper.PiOver2 * Projectile.ai[2], i / 35f) * vel;
                Main.EntitySpriteDraw(sprite2, Projectile.Center + Projectile.velocity.RotatedBy(smear) * 6 - Main.screenPosition, null, Color.Lerp(Color.DarkGray * 0.05f, Color.Transparent, i / 35f), rot + smear, origin,
                    Projectile.scale, spriteEffects);
            }

            Main.EntitySpriteDraw(sprite, Projectile.Center + Projectile.velocity * 6 - Main.screenPosition, null, Color.White, rot, origin,
                Projectile.scale, spriteEffects);
            return false;
        }
    }
    public class MountaintopGlacier_Proj2 : ModProjectile
    {
        public override string Texture => "TranscendenceMod/Items/Weapons/Melee/MountaintopGlacier_Proj";
        public float vel = 0;
        public float RotSpeed = 0.5f;
        public float Timer;
        public int dir;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.NoMeleeSpeedVelocityScaling[Type] = true;
        }
        public override void SetDefaults()
        {
            Projectile.width = 118;
            Projectile.height = 118;
            Projectile.DamageType = DamageClass.MeleeNoSpeed;
            Projectile.noEnchantmentVisuals = true;

            Projectile.ownerHitCheck = true;
            Projectile.netImportant = true;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 6;
            Projectile.extraUpdates = 1;

            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 78;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Frostburn2, 120);
        }
        public override void OnSpawn(IEntitySource source)
        {
            Player player = Main.player[Projectile.owner];
            SoundEngine.PlaySound(ModSoundstyles.SeraphSwords_Charge, Projectile.Center);
            dir = player.direction;
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.hide = true;
            player.heldProj = Projectile.whoAmI;

            if (player.HeldItem.type != ModContent.ItemType<MountaintopGlacier>() || player.dead)
                Projectile.Kill();

            player.direction = dir;
            player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, (player.MountedCenter - Projectile.Center).ToRotation() + MathHelper.PiOver2);

            Projectile.Center = player.Center + (Projectile.velocity * vel);
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
            Projectile.velocity = Projectile.velocity;

            Vector2 dustPos = Projectile.Center + Projectile.velocity * 20 * (0.25f * Projectile.scale);
            int d = Dust.NewDust(dustPos, 1, 1, ModContent.DustType<Smoke>(),
                0, 0, 0, Color.DeepSkyBlue * 0.125f, 0.75f);
            Main.dust[d].noGravity = true;
            Main.dust[d].velocity = Vector2.Zero;

            if (Projectile.ai[2] == 27)
            {
                TranscendenceUtils.ProjectileRing(Projectile, 16, Projectile.GetSource_FromAI(), dustPos,
                    ModContent.ProjectileType<GlacierSnow>(), (int)(Projectile.damage * 0.66f), 1, 2, 0, 0, 1, player.whoAmI, 0);
            }

            if (Projectile.ai[2] > 8 && Projectile.ai[2] < 33)
                Projectile.scale += 0.075f;

            if (Projectile.ai[2] > 2 && Projectile.ai[2] < 24)
            {
                vel += MathHelper.Lerp(0.2f, 0.7f, Projectile.ai[2] / 24f);
            }

            if (Projectile.ai[2] > 52)
            {
                vel -= 0.5f;
                if (Projectile.scale > 1f)
                    Projectile.scale -= 0.075f;
            }
            Projectile.ai[2]++;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float reference = float.NaN;
            if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center + Projectile.velocity * (3 + (Projectile.ai[2] / 10f) * Projectile.scale * 0.5f),
                Projectile.Center + Projectile.velocity * (7 + (Projectile.ai[2] / 10f) * Projectile.scale * 0.75f), 12, ref reference))
            {
                return true;
            }
            else return false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;

            Texture2D sprite = ModContent.Request<Texture2D>(Texture).Value;

            bool boolean = Main.player[Projectile.owner].direction == 1;

            Main.EntitySpriteDraw(sprite, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation + (boolean ? 0 : MathHelper.PiOver2), sprite.Size() * 0.5f,
              Projectile.scale, boolean ? SpriteEffects.None : SpriteEffects.FlipHorizontally);

            return false;
        }
    }
}