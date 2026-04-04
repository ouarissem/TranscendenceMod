using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.Graphics.Renderers;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Projectiles.Weapons.Melee;

namespace TranscendenceMod.Items.Weapons.Melee
{
    public class BladeOfReflection : ModItem
    {
        public int proj = ModContent.ProjectileType<BladeOfReflectionProj>();
        public int reflection = ModContent.ProjectileType<Reflection>();
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            ItemID.Sets.Spears[Type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 120;
            Item.DamageType = DamageClass.Melee;
            Item.width = 24;
            Item.height = 24;
            Item.useTime = 18;
            Item.useAnimation = 18;
            Item.useStyle = ItemUseStyleID.Rapier;
            Item.shoot = proj;
            Item.shootSpeed = 8;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.knockBack = 2;
            Item.value = Item.sellPrice(gold: 12, silver: 50);
            Item.rare = ItemRarityID.Yellow;
            Item.UseSound = SoundID.Item60;
            Item.autoReuse = true;
            Item.crit = 20;
        }
        public override bool CanShoot(Player player)
        {
            if (player.ownedProjectileCounts[proj] > 0)
                return false;
            else return true;
        }
        public override void ModifyShootStats(Player player, ref Vector2 position,
            ref Vector2 velocity, ref int type, ref int damage, ref float knockback) => type = ProjectileID.None;
        public override bool CanUseItem(Player player)
        {
            if (player.ownedProjectileCounts[proj] > 0)
                return false;
            else return base.CanUseItem(player);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.ownedProjectileCounts[proj] < 1)
            {
                Projectile.NewProjectile(source, position, velocity * 2f, proj, damage, knockback, player.whoAmI);
                Projectile.NewProjectile(source, position, velocity * -2f, proj, damage, knockback, player.whoAmI, 1);
            }
            return false;
        }
        public override void HoldItem(Player player)
        {
            if (player.ownedProjectileCounts[reflection] == 0)
            {
                Projectile.NewProjectile(player.GetSource_ItemUse(Item), player.Center, Vector2.Zero, reflection, Item.damage, Item.knockBack, player.whoAmI);
            }
        }
    }
    public class Reflection : ModProjectile
    {
        public float vel = 0;

        public override string Texture => "TranscendenceMod/Items/Weapons/Melee/BladeOfReflection";
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.NoMeleeSpeedVelocityScaling[Type] = true;
        }
        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 60;
            Projectile.DamageType = DamageClass.Generic;

            Projectile.ownerHitCheck = true;
            Projectile.netImportant = true;

            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 60;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
        }
        public override bool? CanDamage() => false;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            player.heldProj = Projectile.whoAmI;

            if (player.HeldItem.type == ModContent.ItemType<BladeOfReflection>() && !player.dead)
            {
                Projectile.timeLeft = 5;
            }

            Projectile.hide = true;
            Projectile.direction = -player.direction;

            float x = -(player.Center.X - Main.MouseWorld.X);
            Projectile.Center = player.Center + new Vector2(x, 0);
            player.direction = (Main.MouseWorld.X > player.Center.X).ToDirectionInt();
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Player p = Main.player[Projectile.owner];
            Player p2 = Main.playerVisualClone[Projectile.owner] ??= new();

            p2.CopyVisuals(p);
            p2.socialIgnoreLight = true;
            p2.isFirstFractalAfterImage = true;
            p2.firstFractalAfterImageOpacity = 1;

            p2.ResetEffects();
            p2.ResetVisibleAccessories();
            p2.DisplayDollUpdate();
            p2.UpdateDyes();
            p2.UpdateSocialShadow();
            p2.PlayerFrame();

            p2.wingFrame = p.wingFrame;
            p2.compositeFrontArm = p.compositeFrontArm;
            p2.compositeFrontArm.rotation = p.compositeFrontArm.rotation + MathHelper.Pi;

            p2.Center = Projectile.Center;
            p2.direction = -p.direction;

            Main.PlayerRenderer.DrawPlayer(Main.Camera, p2, p2.position, -p.fullRotation, p.fullRotationOrigin, 0, 1);

            return false;
        }
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            overPlayers.Add(index);
        }
    }
    public class BladeOfReflectionProj : ModProjectile
    {
        public float vel = 0;

        public override string Texture => "TranscendenceMod/Items/Weapons/Melee/BladeOfReflection";
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.NoMeleeSpeedVelocityScaling[Type] = true;
        }
        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.DamageType = DamageClass.MeleeNoSpeed;

            Projectile.ownerHitCheck = true;
            Projectile.netImportant = true;

            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 25;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Vector2 pos = Vector2.SmoothStep(Projectile.Center, target.Center, 0.5f);
            for (int i = 0; i < Main.rand.Next(4, 7); i++)
            {
                int proj = ModContent.ProjectileType<MirrorBladeShrapnel>();
                if (Main.player[Projectile.owner].ownedProjectileCounts[proj] >= 20)
                    continue;

                Projectile.NewProjectile(Projectile.GetSource_OnHit(target), pos, Main.rand.NextVector2Circular(2.5f, 5),
                    proj, (int)(Projectile.damage * 0.33f), Projectile.knockBack, Main.player[Projectile.owner].whoAmI);
            }
            for (int i = 0; i < 10; i++)
            {
                Dust.NewDustPerfect(pos, DustID.MagicMirror, Main.rand.NextVector2Circular(2.5f, 5));
            }
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            player.heldProj = Projectile.whoAmI;

            if (player.HeldItem.type != ModContent.ItemType<BladeOfReflection>() || player.dead)
                Projectile.Kill();

            Projectile.Center = player.Center + (Projectile.velocity * 2 * vel);
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;

            if (Projectile.ai[0] == 1)
            {
                float x = -(player.Center.X - Main.MouseWorld.X);
                Projectile.Center = player.Center + new Vector2(x, 0) + (Projectile.velocity * 2 * vel);
            }
            else
            {
                Projectile.usesLocalNPCImmunity = true;
                Projectile.localNPCHitCooldown = 5;
                player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, (player.MountedCenter - Projectile.Center).ToRotation() + MathHelper.PiOver2);
            }

            if (Projectile.timeLeft < 16 && Projectile.timeLeft > 6)
                vel = 1;
            else vel += 0.125f;

            if (Projectile.timeLeft < 6)
            {
                vel -= 0.25f;
                if (Projectile.Distance(player.Center) < 5)
                    Projectile.Kill();
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            return true;
        }
    }
}