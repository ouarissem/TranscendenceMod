using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Projectiles.Equipment;

namespace TranscendenceMod.Items.Consumables
{
    public class MiningPowder : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 15;
        }
        public override void SetDefaults()
        {
            Item.width = 14;
            Item.height = 14;

            Item.shoot = ModContent.ProjectileType<MiningDustProj>();
            Item.shootSpeed = 2;
            Item.rare = ItemRarityID.Green;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.noUseGraphic = true;
            Item.value = Item.sellPrice(silver: 20);

            Item.consumable = true;
            Item.maxStack = 9999;
            Item.autoReuse = true;
        }
        public override void AddRecipes()
        {
            Recipe gold = CreateRecipe(10);
            gold.AddIngredient(ItemID.Leather, 2);
            gold.AddIngredient(ItemID.ShadowScale, 5);
            gold.AddIngredient(ModContent.ItemType<CarbonOre>());
            gold.AddTile(TileID.Anvils);
            gold.Register();

            Recipe plat = CreateRecipe(10);
            plat.AddIngredient(ItemID.Leather, 2);
            plat.AddIngredient(ItemID.TissueSample, 5);
            plat.AddIngredient(ModContent.ItemType<CarbonOre>());
            plat.AddTile(TileID.Anvils);
            plat.Register();
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int i = 0; i < 20; i++)
            {
                Projectile.NewProjectile(source, position, velocity * Main.rand.NextFloat(2, 4), type, damage, knockback, player.whoAmI);
            }
            return false;
        }
    }
}
