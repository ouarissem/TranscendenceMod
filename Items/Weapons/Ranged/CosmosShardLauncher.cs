using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Dusts;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Miscannellous.Rarities;
using TranscendenceMod.Projectiles;
using TranscendenceMod.Projectiles.Weapons.Ranged;

namespace TranscendenceMod.Items.Weapons.Ranged
{
    public class CosmosShardLauncher : ModItem
    {
        int proj = ModContent.ProjectileType<CosmosShardLauncherProj>();
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Ranged;
            Item.damage = 875;
            Item.knockBack = 4.5f;
            Item.crit = 15;
            Item.channel = true;

            Item.useTime = 60;
            Item.useAnimation = 60;
            Item.autoReuse = true;

            Item.useStyle = ItemUseStyleID.None;
            Item.useAmmo = AmmoID.Rocket;

            Item.width = 36;
            Item.height = 28;
            Item.noMelee = true;

            Item.value = Item.sellPrice(gold: 50);
            Item.rare = ModContent.RarityType<Brown>();

        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            TranscendenceUtils.DrawItemGlowmask(Item, rotation, scale, Texture);
        }
        public override bool AltFunctionUse(Player player) => true;
        public override void HoldItem(Player player)
        {
            if (player.ownedProjectileCounts[proj] == 0)
            {
                Projectile.NewProjectile(player.GetSource_ItemUse(Item), player.Center, Vector2.Zero,
                    proj, Item.damage, Item.knockBack, player.whoAmI);
            }
        }
    }

    public class CosmosShardLauncherProj : ModProjectile
    {
        public int CD;
        public int TimeSinceSpawn;
        Player player;
        public int Timer2;
        public int TurnTimer;
        public bool FlamethrowerState = false;
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
            VanillaIsSoPicky.SetDefaults(ModContent.ItemType<CosmosShardLauncher>(), true);
        }
        public override void AI()
        {
            player = Main.player[Projectile.owner];
            player.scope = false;

            if (player.HeldItem.type == ModContent.ItemType<CosmosShardLauncher>() && !player.GetModPlayer<TranscendencePlayer>().CannotUseItems && !player.dead)
                Projectile.timeLeft = 5;

            Projectile.Center = player.Center + Projectile.velocity * 6.5f;
            player.heldProj = Projectile.whoAmI;
            TimeSinceSpawn++;

            VanillaAss();
            float rotation = (player.MountedCenter - Main.MouseWorld).ToRotation() + MathHelper.PiOver2 - player.fullRotation;

            Projectile.spriteDirection = (int)Projectile.rotation;
            Projectile.velocity = Projectile.DirectionTo(Main.MouseWorld) * 6.65f;
            Projectile.rotation = rotation;

            float rot = Main.MouseWorld.X < Projectile.Center.X ? 130 : -130;
            Vector2 vec = Projectile.Center + Projectile.velocity * 4 + Projectile.velocity.RotatedBy(MathHelper.ToRadians(rot)) * 2.4f;
            Vector2 vec2 = Projectile.Center + Projectile.velocity * -6 + Projectile.velocity.RotatedBy(MathHelper.ToRadians(rot)) * 2.4f;
            Vector2 vel = Projectile.DirectionTo(Main.MouseWorld);

            if (FlamethrowerState && player.HeldItem.type == ModContent.ItemType<CosmosShardLauncher>())
            {
                player.HeldItem.useAmmo = AmmoID.Gel;
                VanillaIsSoPicky.useAmmo = AmmoID.Gel;
            }
            else if (!FlamethrowerState && player.HeldItem.type == ModContent.ItemType<CosmosShardLauncher>())
            {
                player.HeldItem.useAmmo = AmmoID.Rocket;
                VanillaIsSoPicky.useAmmo = AmmoID.Rocket;
            }

            if (Projectile.ai[1] == (CD - 1) && !FlamethrowerState)
            {
                SoundEngine.PlaySound(SoundID.MaxMana, player.Center);
            }

            if (Main.MouseWorld.Distance(player.Center) > 100)
            {
                player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, rotation);

                player.SetCompositeArmBack(true, Player.CompositeArmStretchAmount.Full, rotation);
                player.direction = (Main.MouseWorld.X > player.Center.X ? 1 : -1);

                if (player.altFunctionUse == 1)
                {
                    player.GetModPlayer<TranscendencePlayer>().CannotUseItemsTimer = 15;
                    if (!FlamethrowerState)
                    {
                        if (player.GetModPlayer<TranscendencePlayer>().CosmoShardTimer < 1)
                        {
                            int gore = Mod.Find<ModGore>("CosmosShardGore").Type;
                            if (Main.netMode != NetmodeID.Server)
                            {
                                int g = Gore.NewGore(Projectile.GetSource_FromAI(), vec, vel * 8, gore);
                                Main.gore[g].light = 0.075f;
                            }
                            FlamethrowerState = true;
                            player.GetModPlayer<TranscendencePlayer>().CosmoShardTimer = -1;
                        }
                    }
                    else
                    {
                        FlamethrowerState = false;
                        player.GetModPlayer<TranscendencePlayer>().CosmoShardTimer = 0;
                        VanillaIsSoPicky.useAmmo = AmmoID.Rocket;
                    }
                }


                player.PickAmmo(VanillaIsSoPicky, out int projToShoot, out float speed, out int damage, out float knockBack, out int usedAmmoItemId, true);
                int useCD = VanillaIsSoPicky.useAnimation;
                CD = useCD;

                if (CD < 1 || player.GetModPlayer<TranscendencePlayer>().CannotUseItemsTimer > 0)
                    return;

                if (!FlamethrowerState && ++Projectile.ai[1] > CD && player.HasAmmo(VanillaIsSoPicky) && player.controlUseItem && player.altFunctionUse == 0)
                {
                    SoundEngine.PlaySound(new SoundStyle("TranscendenceMod/Miscannellous/Assets/Sounds/Weapons/Cosmoshard"), Projectile.Center);

                    player.PickAmmo(VanillaIsSoPicky, out projToShoot, out speed, out damage, out knockBack, out usedAmmoItemId, false);

                    Projectile.NewProjectile(Projectile.GetSource_FromAI(), vec, vel * 12, ModContent.ProjectileType<CosmosShard>(), damage, knockBack, player.whoAmI);

                    //Rocket Jumping :D
                    if (Main.MouseWorld.Between(player.Center - new Vector2(200, 150), player.Center + new Vector2(200, Main.screenHeight)) && player.controlUp)
                    {
                        player.velocity += player.DirectionTo(Main.MouseWorld) * -20f;
                        Vector2 dpos = player.Center + Vector2.One.RotatedBy(player.DirectionTo(Main.MouseWorld).ToRotation() - MathHelper.PiOver4) * 35;
                        for (int i = 0; i < 30; i++)
                        {
                            float drot = MathHelper.ToRadians(Main.rand.Next(-50, 50));
                            Dust dust = Dust.NewDustPerfect(dpos, ModContent.DustType<ArenaDust>(), player.DirectionTo(Main.MouseWorld).RotatedBy(drot) * 4, 0, Color.Purple, 2);
                            Dust dust2 = Dust.NewDustPerfect(dpos, ModContent.DustType<ExtraTerrestrialDust>(), player.DirectionTo(Main.MouseWorld).RotatedBy(drot) * 15, 0, default, 3);
                            dust.noGravity = true;
                            dust2.noGravity = true;
                        }
                    }
                    else player.velocity += player.DirectionTo(Main.MouseWorld) * -4.75f;

                    player.GetModPlayer<TranscendencePlayer>().CosmoShardTimer = useCD;
                    Projectile.ai[1] = 0;
                }
                if (FlamethrowerState && player.HasAmmo(VanillaIsSoPicky) && ++Projectile.ai[1] > CD && player.controlUseItem)
                {
                    string stringer = Main.rand.NextBool(4) ? "TranscendenceMod/Miscannellous/Assets/Sounds/NPCs/Attack/SeraphSun02" : "TranscendenceMod/Miscannellous/Assets/Sounds/NPCs/Attack/SeraphSun01";
                    if (Main.rand.NextBool(5))
                        SoundEngine.PlaySound(new SoundStyle(stringer) with { Volume = 0.5f, MaxInstances = 0, PitchRange = (-1.5f, 0.1f) }, Projectile.Center);
                    
                    player.PickAmmo(VanillaIsSoPicky, out projToShoot, out speed, out damage, out knockBack, out usedAmmoItemId, false);

                    Projectile.NewProjectile(Projectile.GetSource_FromAI(), vec2, vel * 10, ModContent.ProjectileType<CosmosFlames>(), Projectile.damage, 0, player.whoAmI);

                    Projectile.ai[1] = 0;
                }
            }
        }
    }
}