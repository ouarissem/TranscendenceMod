using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Projectiles.Equipment.Tools;
using TranscendenceMod.Tiles.BigTiles;

namespace TranscendenceMod.Items.Tools
{
    public class GiantAnchor : ModItem
    {
        public int proj = ModContent.ProjectileType<GiantAnchorProj>();
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;

            Item.width = 38;
            Item.height = 38;

            Item.value = Item.sellPrice(gold: 1);
            Item.rare = ItemRarityID.Green;

            Item.shoot = proj;
            Item.shootSpeed = 0;
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            velocity = new Vector2(0, 10);
        }
        public override bool CanShoot(Player player)
        {
            return player.ownedProjectileCounts[proj] == 0;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddRecipeGroup(RecipeGroupID.IronBar, 30)
            .AddIngredient(ItemID.Coral, 10)
            .AddIngredient(ItemID.Starfish, 10)
            .AddTile(ModContent.TileType<Oceation>())
            .Register();
        }
    }
}
