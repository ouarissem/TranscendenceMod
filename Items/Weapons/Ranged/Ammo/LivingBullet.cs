using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Items.Materials.LargeRecipes;
using TranscendenceMod.Projectiles.Weapons.Ranged.Ammo;

namespace TranscendenceMod.Items.Weapons.Ranged.Ammo
{
    public class LivingBullet : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
        }
        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Ranged;
            Item.damage = 10;
            Item.knockBack = 2f;
            Item.ammo = AmmoID.Bullet;
            Item.shoot = ModContent.ProjectileType<LivingBulletProj>();
            Item.consumable = true;
            Item.maxStack = 9999;
            Item.width = 20;
            Item.height = 20;
            Item.value = Item.sellPrice(silver: 10);
            Item.rare = ItemRarityID.Red;
        }
        public override void AddRecipes()
        {
            CreateRecipe(666)
            .AddIngredient(ItemID.ChlorophyteBullet, 666)
            .AddIngredient(ModContent.ItemType<LivingOrganicMatter>())
            .AddTile(TileID.LunarCraftingStation)
            .Register();
        }
    }
}
