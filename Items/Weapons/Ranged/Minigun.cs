using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Miscannellous.GlobalStuff;
using TranscendenceMod.Miscannellous.Rarities;

namespace TranscendenceMod.Items.Weapons.Ranged
{
    public class Minigun : ModItem
    {
        int proj = ModContent.ProjectileType<MinigunProj>();
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Ranged;
            Item.damage = 88;
            Item.knockBack = 0.75f;
            Item.crit = 5;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.shootSpeed = 25f;

            Item.useTime = 4;
            Item.useAnimation = 4;
            Item.autoReuse = true;

            Item.useStyle = ItemUseStyleID.None;
            Item.useAmmo = AmmoID.Bullet;

            Item.width = 46;
            Item.height = 28;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.channel = true;

            Item.value = Item.sellPrice(gold: 25);
            Item.rare = ModContent.RarityType<Brown>();

        }
        public override bool AltFunctionUse(Player player) => true;
        public override void HoldItem(Player player)
        {
            if (player.ownedProjectileCounts[proj] == 0 && !player.GetModPlayer<TranscendencePlayer>().CannotUseItems)
            {
                Projectile.NewProjectile(player.GetSource_ItemUse(Item), player.Center, Vector2.Zero,
                    proj, Item.damage, Item.knockBack, player.whoAmI);
            }
        }
    }
    public class MinigunProj : ModProjectile
    {
        public int TimeSinceSpawn;
        Player player;
        public int Timer2;
        public int TurnTimer;
        public float Recoiling;
        public static Item VanillaIsSoPicky = null;
        public float DroneDist;
        public bool AltMode;
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
            VanillaIsSoPicky.SetDefaults(ModContent.ItemType<Minigun>(), true);
            VanillaIsSoPicky.channel = true;
            VanillaIsSoPicky.useAmmo = AmmoID.Bullet;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D sprite = ModContent.Request<Texture2D>("TranscendenceMod/Items/Weapons/Ranged/Minigun").Value;
            Texture2D sprite2 = ModContent.Request<Texture2D>("TranscendenceMod/Items/Weapons/Ranged/MinigunDrone").Value;
            Texture2D sprite3 = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/BloomLine").Value;

            if (TimeSinceSpawn > 5)
            {
                if (Main.MouseWorld.Distance(player.Center) > 60)
                {
                    //Main Gun
                    Main.EntitySpriteDraw(sprite, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation + player.fullRotation + MathHelper.PiOver2 + MathHelper.ToRadians(Recoiling * 5f), sprite.Size() * 0.5f, 1f,
                        Main.MouseWorld.X < Projectile.Center.X ? SpriteEffects.FlipVertically : SpriteEffects.None);

                    Vector2 vecU = Projectile.Center + Vector2.One.RotatedBy(Projectile.velocity.ToRotation() + MathHelper.PiOver4) * DroneDist;
                    Vector2 vecD = Projectile.Center + Vector2.One.RotatedBy(Projectile.velocity.ToRotation() + MathHelper.PiOver4) * -DroneDist;

                    SpriteBatch spriteBatch = Main.spriteBatch;

                    spriteBatch.End();
                    spriteBatch.Begin(default, BlendState.Additive, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);

                    spriteBatch.Draw(sprite3, new Rectangle(
                        (int)(vecU.X - Main.screenPosition.X),
                        (int)(vecU.Y - Main.screenPosition.Y),
                        12,
                        (int)(vecU.Distance(player.Center) * 2f)), null,
                        Color.Red * 0.5f, vecU.DirectionTo(player.Center).ToRotation() + MathHelper.PiOver2,
                        sprite3.Size() * 0.5f,
                        SpriteEffects.None,
                        0);
                    
                    spriteBatch.Draw(sprite3, new Rectangle(
                        (int)(vecD.X - Main.screenPosition.X),
                        (int)(vecD.Y - Main.screenPosition.Y),
                        12,
                        (int)(vecD.Distance(player.Center) * 2f)), null,
                        Color.Red * 0.5f, vecD.DirectionTo(player.Center).ToRotation() + MathHelper.PiOver2,
                        sprite3.Size() * 0.5f,
                        SpriteEffects.None,
                        0);

                    spriteBatch.End();
                    spriteBatch.Begin(default, BlendState.AlphaBlend, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);

                    //Drones
                    Main.EntitySpriteDraw(sprite2, vecU - Main.screenPosition, null, lightColor, 0, sprite2.Size() * 0.5f, 1f,
                        Main.MouseWorld.X < Projectile.Center.X ? SpriteEffects.FlipVertically : SpriteEffects.None);
                    Main.EntitySpriteDraw(sprite2, vecD - Main.screenPosition, null, lightColor, 0, sprite2.Size() * 0.5f, 1f,
                        Main.MouseWorld.X < Projectile.Center.X ? SpriteEffects.FlipVertically : SpriteEffects.None);
                }
            }
            return base.PreDraw(ref lightColor);
        }
        public override void AI()
        {
            player = Main.player[Projectile.owner];

            if (player.HeldItem.type == ModContent.ItemType<Minigun>() && !player.GetModPlayer<TranscendencePlayer>().CannotUseItems && !player.dead)
                Projectile.timeLeft = 5;

            Vector2 pos = player.Center + Projectile.velocity * (2.65f - Recoiling);
            Projectile.Center = pos;
            player.heldProj = Projectile.whoAmI;
            TimeSinceSpawn++;

            VanillaAss();
            float rotation = (player.MountedCenter - Main.MouseWorld).ToRotation() + MathHelper.PiOver2 - player.fullRotation;

            Projectile.spriteDirection = (int)Projectile.rotation;
            Projectile.velocity = Projectile.DirectionTo(Main.MouseWorld) * (10f - Recoiling);
            Projectile.rotation = rotation;
            player.scope = false;

            float rot = Main.MouseWorld.X < Projectile.Center.X ? 130 : -130;
            int dir = Main.MouseWorld.X > player.Center.X ? 1 : -1;

            Vector2 vec = Projectile.Center + Projectile.velocity * 6f + Projectile.velocity.RotatedBy(MathHelper.ToRadians(rot) + MathHelper.PiOver4 * dir) * 0.2f;
            Vector2 vec2 = Projectile.Center + Vector2.One.RotatedBy(Projectile.velocity.ToRotation() + MathHelper.PiOver4) * DroneDist;
            Vector2 vec3 = Projectile.Center + Vector2.One.RotatedBy(Projectile.velocity.ToRotation() + MathHelper.PiOver4) * -DroneDist;

            Vector2 vec4 = Projectile.Center + Projectile.velocity * 0.125f + Projectile.velocity.RotatedBy(MathHelper.ToRadians(rot) + MathHelper.PiOver4 * dir) * -1f;

            if (player.altFunctionUse > 0)
                AltMode = !AltMode;

            if (AltMode)
                DroneDist = MathHelper.Lerp(DroneDist, -75f, 0.05f);
            else DroneDist = MathHelper.Lerp(DroneDist, 50f, 0.05f);

            if (Recoiling > 0)
                Recoiling -= 0.1f;
            Vector2 vel = Projectile.DirectionTo(Main.MouseWorld).RotatedBy(MathHelper.ToRadians(Recoiling));

            if (Main.MouseWorld.Distance(player.Center) > 60)
            {
                player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, rotation);
                player.direction = dir;

                if (++Projectile.ai[1] > 1 && player.HasAmmo(VanillaIsSoPicky) && !player.GetModPlayer<TranscendencePlayer>().CannotUseItems)
                {
                    if (player.controlUseItem && player.altFunctionUse == 0)
                    {

                        SoundEngine.PlaySound(SoundID.Item40, Projectile.Center);
                        player.PickAmmo(VanillaIsSoPicky, out int projToShoot, out float speed, out int damage, out float knockBack, out int usedAmmoItemId, Main.rand.Next(100) > 20);

                        int p = Projectile.NewProjectile(Projectile.GetSource_FromAI(), vec, vel.RotatedByRandom(0.25f) * 48f, projToShoot, Projectile.damage, knockBack, player.whoAmI);
                        int p2 = Projectile.NewProjectile(Projectile.GetSource_FromAI(), vec2, vel.RotatedByRandom(0.1f).RotatedBy(MathHelper.PiOver2 / -12f) * 64f, projToShoot, Projectile.damage, knockBack, player.whoAmI);
                        int p3 = Projectile.NewProjectile(Projectile.GetSource_FromAI(), vec3, vel.RotatedByRandom(0.1f).RotatedBy(MathHelper.PiOver2 / 12f) * 64f, projToShoot, Projectile.damage, knockBack, player.whoAmI);
                        Main.projectile[p].extraUpdates += 1;
                        Main.projectile[p].GetGlobalProjectile<TranscendenceProjectiles>().FromMinigun = true;

                        Main.projectile[p2].extraUpdates += 1;
                        Main.projectile[p2].GetGlobalProjectile<TranscendenceProjectiles>().FromMinigun = true;

                        Main.projectile[p3].extraUpdates += 1;
                        Main.projectile[p3].GetGlobalProjectile<TranscendenceProjectiles>().FromMinigun = true;

                        Recoiling = Main.rand.NextFloat(1.25f, 2f);

                        int gore = Mod.Find<ModGore>("Minigun_Shell").Type;
                        if (Main.netMode != NetmodeID.Server && Main.rand.NextBool(3))
                        {
                            int g = Gore.NewGore(Projectile.GetSource_FromAI(), vec4, vel * -2, gore);
                            Main.gore[g].timeLeft = 90;
                        }

                        Projectile.ai[1] = 0;
                    }
                }
            }
        }
    }
}