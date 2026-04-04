using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Projectiles.Weapons;

namespace TranscendenceMod.Items.Weapons
{
    public class Atbowphere : ModItem
    {
        int proj = ModContent.ProjectileType<AtbowphereProj>();
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 105;
            Item.DamageType = ModContent.GetInstance<MagicRangedDamageClass>();

            Item.width = 30;
            Item.height = 42;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 25;
            Item.useAnimation = 25;

            Item.noUseGraphic = true;
            Item.mana = 35;

            Item.knockBack = 2;
            Item.value = Item.sellPrice(gold: 20);
            Item.rare = ItemRarityID.Yellow;
            Item.autoReuse = true;
            Item.noMelee = true;

            Item.shootSpeed = 20;
            Item.crit = 20;
            Item.useAmmo = AmmoID.Arrow;

        }
        public override void ModifyManaCost(Player player, ref float reduce, ref float mult) => mult *= 0;
        public override void HoldItem(Player player)
        {
            if (player.ownedProjectileCounts[proj] == 0)
            {
                Projectile.NewProjectile(player.GetSource_FromAI(), player.Center, Vector2.Zero, proj, Item.damage, Item.knockBack, player.whoAmI);
            }
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-3f, 0f);
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<ElementalBow>())
            .AddIngredient(ItemID.HallowedRepeater)
            .AddIngredient(ItemID.Ectoplasm, 20)
            .AddTile(TileID.MythrilAnvil)
            .Register();
        }
    }

    public class AtbowphereProj : ModProjectile
    {
        public int ChargeTimeForShot;
        public int TimeSinceSpawn;
        Player player;
        public bool FirstShot;
        public bool Powered;
        public int Timer2;
        public int TurnTimer;
        public Vector2 pos = Vector2.Zero;
        public static Item VanillaIsSoPicky = null;
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
            Projectile.DamageType = ModContent.GetInstance<MagicRangedDamageClass>();
        }
        public override bool? CanDamage()
        {
            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D sprite = ModContent.Request<Texture2D>(Texture).Value;
            if (TimeSinceSpawn > 5)
            {
                if (Main.MouseWorld.Distance(pos) > 60 && player != null && ChargeTimeForShot > 0)
                {
                    float DefaultOffset1 = -0.5f;
                    float DefaultOffset2 = 1.2f;
                    float DefaultOffset3 = -1.1f;

                    float ChargedOffset1 = -0.4f;
                    float ChargedOffset2 = 0.3f;
                    float ChargedOffset3 = -1f;

                    float Offset1Amount = MathHelper.Lerp(DefaultOffset1, ChargedOffset1, Projectile.ai[1] / ChargeTimeForShot);
                    float Offset2Amount = MathHelper.Lerp(DefaultOffset2, ChargedOffset2, Projectile.ai[1] / ChargeTimeForShot);
                    float Offset3Amount = MathHelper.Lerp(DefaultOffset3, ChargedOffset3, Projectile.ai[1] / ChargeTimeForShot);

                    Vector2 offset1 = Vector2.One.RotatedBy(Projectile.rotation - MathHelper.PiOver2 + Offset1Amount) * 5;
                    Vector2 offset2 = Vector2.One.RotatedBy(Projectile.rotation + Offset2Amount) * -5;
                    Vector2 offset3 = Vector2.One.RotatedBy(Projectile.rotation - MathHelper.PiOver2 + Offset3Amount) * 5;

                    Vector2 pos2 = player.Center + offset1 + offset1 + offset1 + offset1;
                    Vector2 pos3 = player.Center + offset2;
                    Vector2 pos4 = player.Center + offset2 + (offset2 * 0.5f);
                    Vector2 pos5 = player.Center - offset3 - offset3 - offset3 - (offset3 * 0.8f);

                    Color col = Projectile.ai[1] > ChargeTimeForShot ? Color.Gold : Color.White;

                    Utils.DrawLine(Main.spriteBatch, pos2, pos3, col, col, 2);
                    Utils.DrawLine(Main.spriteBatch, pos4, pos5, col, col, 2);

                    Main.EntitySpriteDraw(sprite, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, sprite.Size() * 0.5f, 1,
    Main.MouseWorld.X < Projectile.Center.X ? SpriteEffects.FlipVertically : SpriteEffects.None);
                }
            }
            return false;
        }
        public static void VanillaAss()
        {
            VanillaIsSoPicky = new Item();
            VanillaIsSoPicky.useAmmo = AmmoID.Arrow;
            VanillaIsSoPicky.useTime = 20;
            VanillaIsSoPicky.useAnimation = 20;
        }
        public override void AI()
        {
            player = Main.player[Projectile.owner];

            Projectile.timeLeft = 60;

            if (player.HeldItem.type != ModContent.ItemType<Atbowphere>() || player.GetModPlayer<TranscendencePlayer>().CannotUseItems || player.dead)
                Projectile.Kill();

            pos = player.Center;
            if (pos == Vector2.Zero)
                return;

            player.heldProj = Projectile.whoAmI;
            TimeSinceSpawn++;

            VanillaAss();
            VanillaIsSoPicky.useAmmo = AmmoID.Arrow;

            Player.CompositeArmStretchAmount amount = Player.CompositeArmStretchAmount.Full;

            if (Projectile.ai[1] > (ChargeTimeForShot / 3))
                amount = Player.CompositeArmStretchAmount.ThreeQuarters;

            if (Projectile.ai[1] > (ChargeTimeForShot / 2))
                amount = Player.CompositeArmStretchAmount.Quarter;

            player.SetCompositeArmFront(true, amount, Projectile.rotation - MathHelper.PiOver2);
            player.SetCompositeArmBack(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation - MathHelper.PiOver2);
            player.direction = Projectile.direction;

            Projectile.Center = pos + Projectile.velocity;
            Projectile.spriteDirection = (int)Projectile.rotation;
            Projectile.velocity = Projectile.DirectionTo(Main.MouseWorld) * 5.5f;
            Projectile.rotation = Projectile.AngleTo(Main.MouseWorld);

            ChargeTimeForShot = 20;

            if (player.statMana < player.HeldItem.mana)
                return;

            if (Main.mouseLeft && Projectile.ai[1] < (ChargeTimeForShot + 5)) Projectile.ai[1]++;
            else if (!Main.mouseLeft && Projectile.ai[1] > 0) Projectile.ai[1]--;

            if (Projectile.ai[1] == ChargeTimeForShot)
                SoundEngine.PlaySound(SoundID.MaxMana);

            if (Main.MouseWorld.Distance(pos) > 150)
            {
                if (player.GetModPlayer<TranscendencePlayer>().LeftClicking && Projectile.ai[1] > ChargeTimeForShot && (FirstShot || player.HasAmmo(VanillaIsSoPicky)))
                {
                    SoundEngine.PlaySound(SoundID.Item102, Projectile.Center);
                    player.PickAmmo(VanillaIsSoPicky, out int projID, out float shootSpeed, out int damage, out float kb, out _, false);
                    player.statMana -= player.HeldItem.mana;

                    Vector2 Shooting = player.DirectionTo(Main.MouseWorld);
                    for (int j = 0; j < 2; j++)
                    {
                        float rot = Shooting.ToRotation();

                        if (j != 0)
                        {
                            Vector2 pos2 = player.Center + Vector2.One.RotatedBy(rot + MathHelper.PiOver4) * (0 + (j * 22.5f));
                            Projectile.NewProjectile(Projectile.GetSource_FromAI(), pos2, Shooting * player.HeldItem.shootSpeed, ModContent.ProjectileType<Terrarrow>(), Projectile.damage, Projectile.knockBack, -1, 1, 1.85f, 0.75f);
                        }
                        Vector2 pos3 = player.Center + Vector2.One.RotatedBy(rot - MathHelper.PiOver2 - MathHelper.PiOver4) * (0 + (j * 22.5f));
                        Projectile.NewProjectile(Projectile.GetSource_FromAI(), pos3, Shooting * player.HeldItem.shootSpeed, ModContent.ProjectileType<Terrarrow>(), Projectile.damage, Projectile.knockBack, -1, 1, 1.85f, 0.75f);
                    }
                    Projectile.ai[1] = 0;
                }
            }
        }
    }
}