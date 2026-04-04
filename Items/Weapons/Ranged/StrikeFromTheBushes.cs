using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Buffs;
using TranscendenceMod.Items.Materials.LargeRecipes;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Miscannellous.GlobalStuff;

namespace TranscendenceMod.Items.Weapons.Ranged
{
    public class StrikeFromTheBushes : ModItem
    {
        int proj = ModContent.ProjectileType<StrikeFromTheBushesProj>();
        public override void SetStaticDefaults() => CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Ranged;
            Item.damage = 1555;

            Item.knockBack = 2.465f;
            Item.crit = 50;
            Item.channel = true;

            Item.useTime = 60;
            Item.useAnimation = 60;
            Item.autoReuse = true;

            Item.useStyle = ItemUseStyleID.None;
            Item.useAmmo = AmmoID.Bullet;

            Item.width = 45;
            Item.height = 32;
            Item.noMelee = true;

            Item.value = Item.sellPrice(gold: 22, silver: 50);
            Item.rare = ItemRarityID.Red;
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
            .AddIngredient(ItemID.SniperRifle)
            .AddIngredient(ItemID.ReconScope)
            .AddIngredient(ModContent.ItemType<LivingOrganicMatter>(), 2)
            .AddIngredient(ItemID.LunarBar, 12)
            .AddTile(TileID.LunarCraftingStation)
            .Register();
        }
    }
    public class StrikeFromTheBushesProj : ModProjectile
    {
        public int CD;
        public int TimeSinceSpawn;
        Player player;
        public int Timer2;
        public float Recoiling;
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
            Projectile.timeLeft = 60;

            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
        }
        public override bool? CanDamage()
        {
            return false;
        }
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D sprite = ModContent.Request<Texture2D>("TranscendenceMod/Items/Weapons/Ranged/StrikeFromTheBushes").Value;
            if (TimeSinceSpawn > 5)
            {
                if (Main.MouseWorld.Distance(player.Center) > 100)
                {
                    Vector2 vel = new Vector2(5 * player.direction, 2);
                    if (!player.HasBuff(ModContent.BuffType<SeraphTimeStop>())) vel = player.DirectionTo(Main.MouseWorld) * (4.5f - Recoiling);
                    Vector2 drawPos = player.Center + vel * 6.5f - new Vector2(-5 * player.direction, 8) - Main.screenPosition;

                    if (Projectile.ai[1] > 1 && Projectile.ai[1] < 7 && TimeSinceSpawn > 7)
                    {
                        TranscendenceUtils.DrawEntity(Projectile, Color.White, 0.3f + Projectile.ai[1] / 7, $"Terraria/Images/Projectile_664",
                            Projectile.DirectionTo(Main.MouseWorld).ToRotation() - MathHelper.PiOver2 + MathHelper.ToRadians(Recoiling * 3),
                            Projectile.Center - new Vector2(4, 12) + Projectile.velocity * ((28 - Recoiling) + (Projectile.ai[1] * 0.4f)), null);
                    }

                    Main.EntitySpriteDraw(sprite, drawPos, null, lightColor, vel.ToRotation() + MathHelper.ToRadians(Recoiling * 3f), sprite.Size() * 0.5f, 1f,
                        Main.MouseWorld.X < Projectile.Center.X ? SpriteEffects.FlipVertically : SpriteEffects.None);
                }
            }
            return false;
        }
        public static void VanillaAss()
        {
            VanillaIsSoPicky = new Item();
            VanillaIsSoPicky.channel = true;
            VanillaIsSoPicky.SetDefaults(ModContent.ItemType<StrikeFromTheBushes>(), true);
        }
        public override void OnSpawn(IEntitySource source)
        {
        }
        public override void AI()
        {
            player = Main.player[Projectile.owner];
            player.scope = true;

            if (player.HeldItem.type == ModContent.ItemType<StrikeFromTheBushes>() && !player.GetModPlayer<TranscendencePlayer>().CannotUseItems && !player.dead)
                Projectile.timeLeft = 5;

            Projectile.Center = player.Center + Projectile.velocity * (4.5f - Recoiling);
            player.heldProj = Projectile.whoAmI;
            TimeSinceSpawn++;

            if (Recoiling > 0)
                Recoiling -= 0.1f;

            VanillaAss();
            float rotation = (player.Center - Main.MouseWorld).ToRotation() + MathHelper.PiOver2 - player.fullRotation;

            Projectile.spriteDirection = (int)Projectile.rotation;
            Projectile.velocity = Projectile.DirectionTo(Main.MouseWorld) * (4.5f - Recoiling);
            Projectile.rotation = rotation;

            if (Main.MouseWorld.Distance(player.Center) > 100)
            {
                float rot = Main.MouseWorld.X < Projectile.Center.X ? 130 : -130;
                player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, rotation);

                player.SetCompositeArmBack(true, Player.CompositeArmStretchAmount.Full, rotation);
                player.direction = (Main.MouseWorld.X > player.Center.X ? 1 : -1);

                while (++Projectile.ai[1] > CD && player.HasAmmo(VanillaIsSoPicky) && player.controlUseItem && !player.mouseInterface && player.GetModPlayer<TranscendencePlayer>().CannotUseItemsTimer < 1)
                {
                    SoundEngine.PlaySound(ModSoundstyles.SFtB, Projectile.Center);
                    player.velocity += player.DirectionTo(Main.MouseWorld) * -2.5f;
                    player.PickAmmo(VanillaIsSoPicky, out int projToShoot, out float speed, out int damage, out float knockBack, out int usedAmmoItemId, false);

                    Vector2 vec = Projectile.Center + Projectile.velocity * 2 + Projectile.velocity.RotatedBy(MathHelper.ToRadians(rot)) * 2.4f;

                    Vector2 vel = Projectile.DirectionTo(Main.MouseWorld);

                    int gore = Mod.Find<ModGore>("SFtB_Shell").Type;
                    if (Main.netMode != NetmodeID.Server)
                    {
                        int g = Gore.NewGore(Projectile.GetSource_FromAI(), vec, vel * -2, gore);
                        Main.gore[g].timeLeft = 90;
                    }

                    int p = Projectile.NewProjectile(Projectile.GetSource_FromAI(), vec, vel
                        * 15, projToShoot, damage, knockBack, -1, 0, 0, 16);
                    Main.projectile[p].extraUpdates += 7;
                    Main.projectile[p].GetGlobalProjectile<TranscendenceProjectiles>().FromSFtB = true;
                    Recoiling = 2f;

                    CD = VanillaIsSoPicky.useAnimation;
                    Projectile.ai[1] = 0;
                }
            }
        }
    }
}