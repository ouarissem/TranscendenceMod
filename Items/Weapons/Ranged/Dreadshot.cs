using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Projectiles.Weapons.Ranged;

namespace TranscendenceMod.Items.Weapons.Ranged
{
    public class Dreadshot : ModItem
    {
        int proj = ModContent.ProjectileType<DreadshotProj>();
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Ranged;
            Item.damage = 14;
            Item.knockBack = 2.2f;
            Item.crit = 5;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.shootSpeed = 36f;

            Item.useTime = 5;
            Item.useAnimation = 20;
            Item.reuseDelay = 17;
            Item.autoReuse = true;
            Item.consumeAmmoOnFirstShotOnly = true;

            Item.useStyle = ItemUseStyleID.None;
            Item.useAmmo = AmmoID.Bullet;
            Item.UseSound = SoundID.Item41;

            Item.width = 40;
            Item.height = 33;
            Item.noMelee = true;
            Item.channel = true;

            Item.value = Item.sellPrice(gold: 2);
            Item.rare = ItemRarityID.Blue;

        }
        public override void HoldItem(Player player)
        {
            if (player.ownedProjectileCounts[proj] == 0 && !player.GetModPlayer<TranscendencePlayer>().CannotUseItems)
            {
                Projectile.NewProjectile(player.GetSource_ItemUse(Item), player.Center, Vector2.Zero,
                    proj, Item.damage, Item.knockBack, player.whoAmI);
            }
        }
    }
    public class DreadshotProj : ModProjectile
    {
        public float MaxCharge = 1.5f;
        public float Charge;
        public int CD = 45;
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
            VanillaIsSoPicky.SetDefaults(ModContent.ItemType<Dreadshot>(), true);
            VanillaIsSoPicky.useAmmo = AmmoID.Bullet;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D sprite = ModContent.Request<Texture2D>("TranscendenceMod/Items/Weapons/Ranged/Dreadshot").Value;
            if (TimeSinceSpawn > 5)
            {
                if (Main.MouseWorld.Distance(player.Center) > 20)
                {
                    //Main Gun
                    Main.EntitySpriteDraw(sprite, Projectile.Center - Main.screenPosition, null, Color.Lerp(lightColor, Color.Red, Charge / 2f), Projectile.rotation + player.fullRotation + MathHelper.PiOver2, sprite.Size() * 0.5f, 1f,
                        Main.MouseWorld.X < Projectile.Center.X ? SpriteEffects.FlipVertically : SpriteEffects.None);
                }
            }
            return base.PreDraw(ref lightColor);
        }
        public override void AI()
        {
            player = Main.player[Projectile.owner];

            if (player.HeldItem.type == ModContent.ItemType<Dreadshot>() && !player.GetModPlayer<TranscendencePlayer>().CannotUseItems && !player.dead)
                Projectile.timeLeft = 5;

            Vector2 pos = player.Center + Projectile.velocity * 2.65f;
            Projectile.Center = pos;
            player.heldProj = Projectile.whoAmI;
            TimeSinceSpawn++;

            VanillaAss();
            float rotation = (player.MountedCenter - Main.MouseWorld).ToRotation() + MathHelper.PiOver2 - player.fullRotation;

            Projectile.spriteDirection = (int)Projectile.rotation;
            Projectile.velocity = Projectile.DirectionTo(Main.MouseWorld) * 3.65f;
            Projectile.rotation = rotation;

            float rot = Main.MouseWorld.X < Projectile.Center.X ? 130 : -130;
            Vector2 vec = Projectile.Center + Projectile.velocity * 4 + Projectile.velocity.RotatedBy(MathHelper.ToRadians(rot)) * 2.4f;
            Vector2 vel = Projectile.DirectionTo(Main.MouseWorld);

            if (Main.MouseWorld.Distance(player.Center) > 20)
            {
                player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, rotation);
                player.direction = Main.MouseWorld.X > player.Center.X ? 1 : -1;
                int bullet = ModContent.ProjectileType<DreadshotBullet>();

                MaxCharge = 2f;
                if (++Projectile.ai[1] > CD && player.HasAmmo(VanillaIsSoPicky) && !player.GetModPlayer<TranscendencePlayer>().CannotUseItems)
                {
                    if (Main.mouseRight && Charge < MaxCharge)
                    {
                        Charge += 0.025f;
                        Vector2 pos2 = Vector2.One.RotatedBy(TranscendenceWorld.UniversalRotation * 25) * 5;
                        Projectile.Center = pos + new Vector2(pos2.X / 2f, pos2.Y).RotatedBy(Projectile.velocity.ToRotation());
                        Projectile.rotation = Projectile.DirectionTo(pos).ToRotation() - MathHelper.PiOver2;

                        if (TranscendenceWorld.Timer % 20 == 0)
                        SoundEngine.PlaySound(SoundID.DD2_MonkStaffSwing with { MaxInstances = 0, Volume = 0.33f, PitchVariance = 0.075f}, Projectile.Center);

                        if (Charge > 2)
                            SoundEngine.PlaySound(SoundID.Item149, Projectile.Center);
                    }
                    else
                    {
                        if (player.controlUseItem)
                        {
                            SoundEngine.PlaySound(Charge > 1 ? SoundID.Item38 : SoundID.Item11, Projectile.Center);
                            player.PickAmmo(VanillaIsSoPicky, out int projToShoot, out float speed, out int damage, out float knockBack, out int usedAmmoItemId, false);

                            Projectile.NewProjectile(Projectile.GetSource_FromAI(), vec, vel * 12, bullet, damage, knockBack, player.whoAmI, 0, Charge);

                            Charge = 0;
                            CD = 30;
                            Projectile.ai[1] = 0;
                        }
                    }
                }
            }
        }
    }
}