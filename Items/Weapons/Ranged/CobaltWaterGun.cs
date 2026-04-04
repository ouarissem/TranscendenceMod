using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Projectiles.Weapons.Ranged;

namespace TranscendenceMod.Items.Weapons.Ranged
{
    public class CobaltWaterGun : ModItem
    {
        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(12, 4));
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
        }
        public override void SetDefaults()
        {
            Item.damage = 14;
            Item.DamageType = DamageClass.Ranged;
            Item.knockBack = 1f;
            Item.crit = 5;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 16;
            Item.useAnimation = 16;
            Item.UseSound = SoundID.Item96;
            Item.autoReuse = true;
            Item.width = 24;
            Item.height = 16;
            Item.value = Item.sellPrice(gold: 10);
            Item.rare = ItemRarityID.Green;

            Item.shoot = ModContent.ProjectileType<CobaltWater>();
            Item.shootSpeed = 15;
            Item.useAmmo = AmmoID.Bullet;
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            Projectile.NewProjectile(player.GetSource_ItemUse(Item), position, velocity, type, damage, knockback, player.whoAmI);

            type = ModContent.ProjectileType<CobaltWater>();

            Vector2 pos = player.Center + Vector2.One.RotatedBy(player.DirectionTo(Main.MouseWorld).ToRotation() - MathHelper.PiOver4) * 55;
            position = pos;

            base.ModifyShootStats(player, ref position, ref velocity, ref type, ref damage, ref knockback);
        }
    }
}
