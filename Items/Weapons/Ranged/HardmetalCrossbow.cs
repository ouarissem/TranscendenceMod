using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Items.Materials;

namespace TranscendenceMod.Items.Weapons.Ranged
{
    public class HardmetalCrossbow : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Ranged;
            Item.damage = 10;
            Item.knockBack = 1.3f;
            Item.crit = 0;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.shootSpeed = 36f;

            Item.useTime = 18;
            Item.useAnimation = 18;
            Item.reuseDelay = 15;
            Item.autoReuse = true;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAmmo = AmmoID.Arrow;
            Item.UseSound = SoundID.Item5;

            Item.width = 20;
            Item.height = 8;
            Item.noMelee = true;

            Item.value = Item.sellPrice(silver: 75);
            Item.rare = ItemRarityID.Green;

        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(7f, 2f);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity * 0.33f - new Vector2(0, 0.5f), type, damage, knockback, player.whoAmI);
            return base.Shoot(player, source, position, velocity, type, damage, knockback);
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            position += Vector2.Normalize(velocity) * 8;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
             .AddIngredient(ModContent.ItemType<HardmetalBar>(), 15)
             .AddTile(TileID.Anvils)
             .Register();
        }
    }
}