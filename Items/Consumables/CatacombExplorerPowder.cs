using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Projectiles.Equipment;

namespace TranscendenceMod.Items.Consumables
{
    public class CatacombExplorerPowder : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 15;
        }
        public override void SetDefaults()
        {
            Item.width = 12;
            Item.height = 12;

            Item.shoot = ModContent.ProjectileType<CatacombExplorerDust>();
            Item.shootSpeed = 2;
            Item.rare = ItemRarityID.Green;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.noUseGraphic = true;
            Item.value = Item.sellPrice(silver: 10);

            Item.consumable = true;
            Item.maxStack = 9999;
            Item.autoReuse = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe(6)
            .AddIngredient(ItemID.Leather)
            .AddIngredient(ItemID.FallenStar, 2)
            .AddIngredient(ItemID.Bone)
            .AddTile(TileID.Anvils)
            .Register();
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int i = 0; i < 12; i++)
            {
                Projectile.NewProjectile(source, position, velocity * Main.rand.NextFloat(3, 5), type, damage, knockback, player.whoAmI);
            }
            return false;
        }
    }
}
