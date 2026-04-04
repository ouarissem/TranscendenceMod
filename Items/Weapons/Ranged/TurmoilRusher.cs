using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Buffs;
using TranscendenceMod.Dusts;
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Items.Materials.MobDrops;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Miscannellous.Rarities;
using TranscendenceMod.Projectiles.Weapons.Ranged;
using TranscendenceMod.Tiles.BigTiles;

namespace TranscendenceMod.Items.Weapons.Ranged
{
    public class TurmoilRusher : ModItem
    {
        int proj = ModContent.ProjectileType<TurmoilRusherProj>();
        public override void SetStaticDefaults() => CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Ranged;
            Item.damage = 190;

            Item.knockBack = 3f;
            Item.crit = 10;
            Item.channel = true;
            Item.shootSpeed = 14f;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.autoReuse = true;

            Item.useStyle = ItemUseStyleID.None;
            Item.useAmmo = AmmoID.Bullet;

            Item.width = 45;
            Item.height = 32;
            Item.noMelee = true;

            Item.value = Item.sellPrice(gold: 17, silver: 50);
            Item.rare = ModContent.RarityType<Brown>();
        }
        public override void HoldItem(Player player)
        {
            if (player.ownedProjectileCounts[ModContent.ProjectileType<TurmoilRusherProj>()] == 0 && !player.GetModPlayer<TranscendencePlayer>().CannotUseItems)
            {
                Projectile.NewProjectile(player.GetSource_ItemUse(Item), player.Center, Vector2.Zero,
                     ModContent.ProjectileType<TurmoilRusherProj>(), Item.damage, Item.knockBack, player.whoAmI);
            }
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.Megashark)
            .AddIngredient(ModContent.ItemType<PoseidonsTide>(), 8)
            .AddIngredient(ItemID.GoldBar, 16)
            .AddIngredient(ItemID.Cog, 50)
            .AddTile(ModContent.TileType<Oceation>())
            .Register();
        }
    }
    public class TurmoilRusherProj : ModProjectile
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
            Texture2D sprite = ModContent.Request<Texture2D>("TranscendenceMod/Items/Weapons/Ranged/TurmoilRusher").Value;
            SpriteBatch spriteBatch = Main.spriteBatch;

            if (TimeSinceSpawn > 5)
            {
                if (Main.MouseWorld.Distance(player.Center) > 100)
                {
                    Vector2 vel = new Vector2(5 * player.direction, 2);
                    if (!player.HasBuff(ModContent.BuffType<SeraphTimeStop>())) vel = player.DirectionTo(Main.MouseWorld) * (4.5f - Recoiling);
                    Vector2 drawPos = player.Center + vel * 4.5f - new Vector2(-5 * player.direction, 8) - Main.screenPosition;

                    var eff = ModContent.Request<Effect>("TranscendenceMod/Miscannellous/Assets/Shaders/Effects/SeraphOutlineShader", AssetRequestMode.ImmediateLoad).Value;
                    eff.Parameters["uOpacity"].SetValue(MathHelper.Lerp(0f, 0.75f, Timer2 / 50f));

                    eff.Parameters["uRotation"].SetValue(0f);
                    eff.Parameters["uTime"].SetValue(0f);
                    eff.Parameters["uDirection"].SetValue(0.33f + Main.masterColor);
                    eff.Parameters["uSaturation"].SetValue(1f);

                    Main.EntitySpriteDraw(sprite, drawPos, null, lightColor, vel.ToRotation() + MathHelper.ToRadians(Recoiling * 3f), sprite.Size() * 0.5f, 1f,
                        Main.MouseWorld.X < Projectile.Center.X ? SpriteEffects.FlipVertically : SpriteEffects.None);

                    eff.CurrentTechnique.Passes["SeraphOutlineTechnique2"].Apply();

                    Main.EntitySpriteDraw(sprite, drawPos, null, Color.White, vel.ToRotation() + MathHelper.ToRadians(Recoiling * 3f), sprite.Size() * 0.5f, 1f,
                        Main.MouseWorld.X < Projectile.Center.X ? SpriteEffects.FlipVertically : SpriteEffects.None);

                    spriteBatch.End();
                    spriteBatch.Begin(default, BlendState.AlphaBlend, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);
                }
            }
            return false;
        }
        public static void VanillaAss()
        {
            VanillaIsSoPicky = new Item();
            VanillaIsSoPicky.channel = true;
            VanillaIsSoPicky.SetDefaults(ModContent.ItemType<TurmoilRusher>(), true);
        }
        public override void OnSpawn(IEntitySource source)
        {
        }
        public override void AI()
        {
            player = Main.player[Projectile.owner];
            player.scope = true;

            if (player.HeldItem.type == ModContent.ItemType<TurmoilRusher>() && !player.GetModPlayer<TranscendencePlayer>().CannotUseItems && !player.dead)
                Projectile.timeLeft = 5;

            Projectile.Center = player.Center + Projectile.velocity * (8f - Recoiling);
            player.heldProj = Projectile.whoAmI;
            TimeSinceSpawn++;

            if (Recoiling > 0)
                Recoiling -= 0.1f;

            VanillaAss();
            float rotation = (player.Center - Main.MouseWorld).ToRotation() + MathHelper.PiOver2 - player.fullRotation;

            Projectile.spriteDirection = (int)Projectile.rotation;
            Projectile.velocity = Projectile.DirectionTo(Main.MouseWorld) * (4.5f - Recoiling);
            Projectile.rotation = rotation;

            CD = (int)(VanillaIsSoPicky.useAnimation * (float)(1f - (Timer2 / 50f)));
            if (CD < 1)
                CD = 1;

            if (Main.MouseWorld.Distance(player.Center) > 100)
            {
                float rot = Main.MouseWorld.X < Projectile.Center.X ? 130 : -130;
                player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, rotation);

                player.SetCompositeArmBack(true, Player.CompositeArmStretchAmount.Full, rotation);
                player.direction = (Main.MouseWorld.X > player.Center.X ? 1 : -1);

                Vector2 vec = Projectile.Center + Projectile.velocity * 2 + Projectile.velocity.RotatedBy(MathHelper.ToRadians(rot)) * 2.4f;
                Vector2 vec2 = Projectile.Center + Projectile.velocity * -3 + Projectile.velocity.RotatedBy(MathHelper.ToRadians(rot)) * 2.4f;
                Vector2 vel = Projectile.DirectionTo(Main.MouseWorld);

                if (Timer2 > 20)
                    Dust.NewDustPerfect(vec2, ModContent.DustType<Smoke>(), vel.RotatedBy(MathHelper.PiOver4 / 2f * player.direction).RotatedByRandom(0.2f) * -8f, 0, Color.DarkGray * 0.5f,  0.275f);


                while (++Projectile.ai[1] > CD && player.HasAmmo(VanillaIsSoPicky) && player.controlUseItem && !player.mouseInterface && player.GetModPlayer<TranscendencePlayer>().CannotUseItemsTimer < 1)
                {
                    SoundEngine.PlaySound(ModSoundstyles.SFtB with { MaxInstances = 0, Volume = 0.33f, Pitch = 0.5f}, Projectile.Center);
                    player.PickAmmo(VanillaIsSoPicky, out int projToShoot, out float speed, out int damage, out float knockBack, out int usedAmmoItemId, false);

                    float speed3 = (float)((Timer2 + 25) / 30f);
                    for (int i = 0; i < 2; i++)
                    {
                        float speed2 = Main.rand.NextFloat(0.8f, 1.2f);
                        Projectile.NewProjectile(Projectile.GetSource_FromAI(), vec, vel.RotatedByRandom(0.2f) * speed * speed2 * speed3, ModContent.ProjectileType<TurmoilBullet>(), damage, knockBack, -1, 0, 0, 16);
                    }
                    Recoiling = 3f * (speed3 / 3f);

                    if (Timer2 < 50)
                        Timer2 += 2;
                    else Timer2 = 0;

                    Projectile.ai[1] = 0;
                }
            }
        }
    }
}