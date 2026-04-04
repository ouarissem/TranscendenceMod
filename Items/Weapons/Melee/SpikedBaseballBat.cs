using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace TranscendenceMod.Items.Weapons.Melee
{
    public class SpikedBaseballBat : ModItem
    {
        int projectile = ModContent.ProjectileType<SpikedBaseballBatProj>();
        public int timer;

        public override void SetStaticDefaults() => CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        public override void SetDefaults()
        {
            Item.damage = 25;
            Item.DamageType = DamageClass.Melee;

            Item.width = 24;
            Item.height = 24;

            Item.useTime = 17;
            Item.useAnimation = 17;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 8f;

            Item.shoot = projectile;

            Item.value = Item.sellPrice(gold: 10);
            Item.rare = ItemRarityID.Green;

            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.channel = true;
        }
        public override bool CanShoot(Player player)
        {
            return player.ownedProjectileCounts[projectile] == 0;
        }
    }
    internal class SpikedBaseballBatProj : BaseballBatProj
    {
        public override string Texture => "TranscendenceMod/Items/Weapons/Melee/SpikedBaseballBat";
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.Knockback *= (ChargeTimer * 0.33f);
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            player.heldProj = Projectile.whoAmI;
            player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, (player.MountedCenter - Projectile.Center).ToRotation() + MathHelper.PiOver2);

            if (player.HeldItem.type != ModContent.ItemType<SpikedBaseballBat>() || player.dead)
                Projectile.Kill();

            Projectile.Center = player.Center + (Projectile.velocity * 2 * Projectile.scale);
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
            player.ChangeDir(dir);

            if (player.channel && !Released)
            {
                Projectile.timeLeft = 15;
                if (ChargeTimer < 5)
                {
                    ChargeTimer += 0.15f;
                    Projectile.velocity = Projectile.velocity.RotatedBy(ChargeTimer / -60 * dir);
                }
                return;
            }

            Released = true;
            Projectile.velocity = Projectile.velocity.RotatedBy(Timer / 25);

            Timer += RotSpeed * 0.25f * dir;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D sprite = ModContent.Request<Texture2D>($"{Texture}").Value;
            Vector2 origin = new Vector2(sprite.Width * 0.5f, Projectile.height * 0.5f);

            float rot = Projectile.rotation + (boolean ? 0 : MathHelper.PiOver2);
            SpriteEffects spriteEffects = boolean ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            Main.EntitySpriteDraw(sprite, Projectile.Center + Projectile.velocity * 2.85f - Main.screenPosition, null, lightColor, rot, origin,
                Projectile.scale, spriteEffects);
            return false;
        }
    }
}