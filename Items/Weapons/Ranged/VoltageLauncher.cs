using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Items.Materials.LargeRecipes;
using TranscendenceMod.Items.Weapons.Ranged.Ammo;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Miscannellous.Rarities;
using TranscendenceMod.Projectiles.Weapons.Ranged;
using TranscendenceMod.Items.Materials.MobDrops;

namespace TranscendenceMod.Items.Weapons.Ranged
{
    public class VoltageLauncher : ModItem
    {
        int proj = ModContent.ProjectileType<VoltageLauncherProj>();
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Ranged;
            Item.damage = 245;
            Item.knockBack = 0.5f;
            Item.crit = 10;
            Item.channel = true;

            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.autoReuse = true;

            Item.useStyle = ItemUseStyleID.None;
            Item.useAmmo = AmmoID.Rocket;

            Item.width = 24;
            Item.height = 12;
            Item.noMelee = true;

            Item.value = Item.sellPrice(gold: 25);
            Item.rare = ModContent.RarityType<Brown>();

        }
        public override void HoldItem(Player player)
        {
            if (player.ownedProjectileCounts[proj] == 0 && !player.GetModPlayer<TranscendencePlayer>().CannotUseItems)
            {
                Projectile.NewProjectile(player.GetSource_ItemUse(Item), player.Center, Vector2.Zero,
                    proj, Item.damage, Item.knockBack, player.whoAmI);
            }
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.ElectrosphereLauncher)
            .AddIngredient(ItemID.LunarBar, 20)
            .AddIngredient(ModContent.ItemType<ElectricalComponent>())
            .AddIngredient(ModContent.ItemType<Lightning>(), 14)
            .AddTile(TileID.LunarCraftingStation)
            .Register();
        }
    }

    public class VoltageLauncherProj : ModProjectile
    {
        public int CD = 10;
        public int TimeSinceSpawn;
        Player player;
        public int Timer2;
        public int TurnTimer;
        public static Item VanillaIsSoPicky = null;
        public override string Texture => "TranscendenceMod/Miscannellous/Assets/InvisSprite";
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.aiStyle = -1;

            Projectile.tileCollide = false;
            Projectile.timeLeft = 15;

            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
        }
        public override bool? CanDamage() => false;
        public static void VanillaAss()
        {
            VanillaIsSoPicky = new Item();
            VanillaIsSoPicky.channel = true;
            VanillaIsSoPicky.SetDefaults(ModContent.ItemType<VoltageLauncher>(), true);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D sprite = ModContent.Request<Texture2D>("TranscendenceMod/Items/Weapons/Ranged/VoltageLauncher").Value;
            Texture2D sprite2 = ModContent.Request<Texture2D>("TranscendenceMod/Items/Weapons/Ranged/VoltageLauncher_Glow").Value;
            if (TimeSinceSpawn > 5)
            {
                if (Main.MouseWorld.Distance(player.Center) > 20)
                {
                    //Main Gun
                    Main.EntitySpriteDraw(sprite, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation + player.fullRotation + MathHelper.PiOver2, sprite.Size() * 0.5f, 1f,
                        Main.MouseWorld.X < Projectile.Center.X ? SpriteEffects.FlipVertically : SpriteEffects.None);
                    //Glowmask
                    Main.EntitySpriteDraw(sprite2, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation + player.fullRotation + MathHelper.PiOver2, sprite2.Size() * 0.5f, 1f,
                        Main.MouseWorld.X < Projectile.Center.X ? SpriteEffects.FlipVertically : SpriteEffects.None);
                }
            }
            return base.PreDraw(ref lightColor);
        }
        public override void AI()
        {
            player = Main.player[Projectile.owner];

            if (player.HeldItem.type == ModContent.ItemType<VoltageLauncher>() && !player.GetModPlayer<TranscendencePlayer>().CannotUseItems && !player.dead)
                Projectile.timeLeft = 15;

            Projectile.Center = player.Center + Projectile.velocity * 3.65f;
            player.heldProj = Projectile.whoAmI;
            TimeSinceSpawn++;

            VanillaAss();
            float rotation = (player.MountedCenter - Main.MouseWorld).ToRotation() + MathHelper.PiOver2 - player.fullRotation;

            Projectile.spriteDirection = (int)Projectile.rotation;
            Projectile.velocity = Projectile.DirectionTo(Main.MouseWorld) * 3.65f;
            Projectile.rotation = rotation;

            float rot = Main.MouseWorld.X < Projectile.Center.X ? 130 : -130;
            Vector2 vec = Projectile.Center + Projectile.velocity * 4 + Projectile.velocity.RotatedBy(MathHelper.ToRadians(rot)) * 2.4f;
            Vector2 vec2 = Projectile.Center + Projectile.velocity * -6 + Projectile.velocity.RotatedBy(MathHelper.ToRadians(rot)) * 2.4f;
            Vector2 vel = Projectile.DirectionTo(Main.MouseWorld);
            
            if (player.HeldItem.type == ModContent.ItemType<VoltageLauncher>())
                player.HeldItem.useAmmo = ModContent.ItemType<StormVial>();
            VanillaIsSoPicky.useAmmo = ModContent.ItemType<StormVial>();

            if (Main.MouseWorld.Distance(player.Center) > 20)
            {
                player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, rotation);

                player.SetCompositeArmBack(true, Player.CompositeArmStretchAmount.Full, rotation);
                player.direction = (Main.MouseWorld.X > player.Center.X ? 1 : -1);
                int laser = ModContent.ProjectileType<VoltageBeam>();

                while (++Projectile.ai[1] > CD && player.HasAmmo(VanillaIsSoPicky) && player.controlUseItem && !player.GetModPlayer<TranscendencePlayer>().CannotUseItems)
                {
                    player.PickAmmo(VanillaIsSoPicky, out int projToShoot, out float speed, out int damage, out float knockBack, out int usedAmmoItemId, true);
                    player.PickAmmo(VanillaIsSoPicky, out projToShoot, out speed, out damage, out knockBack, out usedAmmoItemId, player.IsAmmoFreeThisShot(VanillaIsSoPicky, ContentSamples.ItemsByType[usedAmmoItemId], projToShoot));

                    if (player.ownedProjectileCounts[laser] == 0)
                        Projectile.NewProjectile(Projectile.GetSource_FromAI(), vec, vel * 12, laser, damage, knockBack, player.whoAmI);
                    else player.GetModPlayer<TranscendencePlayer>().VoltageBeamTimer = 15;

                    CD = 15;
                    Projectile.ai[1] = 0;
                }
            }
        }
    }
}