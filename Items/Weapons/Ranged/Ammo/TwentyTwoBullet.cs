using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Items.Materials.MobDrops;
using TranscendenceMod.Miscannellous.Rarities;
using TranscendenceMod.Projectiles.Weapons.Ranged.Ammo;

namespace TranscendenceMod.Items.Weapons.Ranged.Ammo
{
    public class TwentyTwoBullet : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 222;
        }
        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Ranged;

            Item.damage = 22;
            Item.knockBack = 2.2f;

            Item.ammo = AmmoID.Bullet;
            Item.shoot = ModContent.ProjectileType<TwentyTwoBulletProj>();
            Item.shootSpeed = 10f;

            Item.consumable = true;
            Item.maxStack = 9999;

            Item.width = 16;
            Item.height = 24;

            Item.value = Item.sellPrice(silver: 15);
            Item.rare = ModContent.RarityType<Brown>();
        }
        public override void AddRecipes()
        {
            CreateRecipe(222)
            .AddIngredient(ItemID.HighVelocityBullet, 222)
            .AddIngredient(ItemID.BeeWax, 2)
            .AddIngredient(ModContent.ItemType<SoulOfKnight>())
            .AddTile(TileID.LunarCraftingStation)
            .Register();
        }
    }
}
