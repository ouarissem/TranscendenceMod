using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Projectiles.Weapons.Ranged;

namespace TranscendenceMod.Items.Weapons.Ranged
{
    public class Backfirer : ModItem
    {
        int proj = ModContent.ProjectileType<TinkererGunBullet>();
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Ranged;
            Item.damage = 68;
            Item.knockBack = 2.7f;
            Item.crit = 10;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.shootSpeed = 10f;

            Item.useTime = 90;
            Item.useAnimation = 90;
            Item.autoReuse = true;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAmmo = AmmoID.Bullet;
            Item.UseSound = SoundID.Item38;

            Item.width = 37;
            Item.height = 15;
            Item.noMelee = true;

            Item.value = Item.sellPrice(gold: 25);
            Item.rare = ItemRarityID.Pink;

        }
        public override Vector2? HoldoutOffset() => new Vector2(-4f, -6f);
        public override bool? UseItem(Player player)
        {
            if (player.ItemAnimationJustStarted)
            {
                Vector2 pos = player.Center + Vector2.One.RotatedBy(player.DirectionTo(Main.MouseWorld).ToRotation() - MathHelper.PiOver4) * 35;
                player.velocity = player.DirectionTo(Main.MouseWorld) * -20f;

                for (int i = 0; i < 15; i++)
                {
                    float rot = MathHelper.ToRadians(Main.rand.Next(-50, 50));
                    Dust dust = Dust.NewDustPerfect(pos, DustID.InfernoFork, player.DirectionTo(Main.MouseWorld).RotatedBy(rot) * 4, 0, default, 3);
                    Dust dust2 = Dust.NewDustPerfect(pos, DustID.Smoke, player.DirectionTo(Main.MouseWorld).RotatedBy(rot) * 15, 0, default, 4);
                    dust.noGravity = true;
                    dust2.noGravity = true;
                }
            }
            return base.UseItem(player);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 pos = player.Center + Vector2.One.RotatedBy(player.DirectionTo(Main.MouseWorld).ToRotation() - MathHelper.PiOver4) * 45;
            SoundEngine.PlaySound(SoundID.Mech, player.Center);
            for (int i = 1; i < 6; i++)
            {
                Projectile.NewProjectile(source, pos, player.DirectionTo(Main.MouseWorld).RotatedBy(MathHelper.Lerp(-MathHelper.PiOver4, MathHelper.PiOver4, i / 5f) / 8f) * (float)(Item.shootSpeed + Math.Cos(i * 3f) / 2f), proj, damage, knockback, player.whoAmI);
            }
            return false;
        }
    }
}