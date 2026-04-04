using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Projectiles.Equipment.Tools;

namespace TranscendenceMod.Items.Tools
{
    public class FlailingHook : ModItem
    {
        public int proj = ModContent.ProjectileType<FlailHook>();
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.Mace);
            Item.damage = 0;
            Item.knockBack = 0;
            Item.shoot = proj;
            Item.shootSpeed = 0;
            Item.value = Item.sellPrice(gold: 1);
            Item.width = 15;
            Item.height = 15;
        }
        public override bool CanShoot(Player player)
        {
            return player.ownedProjectileCounts[proj] == 0;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddRecipeGroup(RecipeGroupID.IronBar, 10)
            .AddIngredient(ItemID.Rope, 25)
            .AddTile(TileID.Anvils)
            .Register();
        }
    }
}
