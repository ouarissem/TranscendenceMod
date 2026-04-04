using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Items.Materials.MobDrops;
using TranscendenceMod.Projectiles.Weapons.Ranged.Ammo;

namespace TranscendenceMod.Items.Weapons.Ranged.Ammo
{
    public class ToxicArrow : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
        }
        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Ranged;
            Item.damage = 16;
            Item.knockBack = 2;
            Item.ammo = AmmoID.Arrow;
            Item.shoot = ModContent.ProjectileType<ToxicArrowProj>();
            Item.shootSpeed = 10;
            Item.consumable = true;
            Item.maxStack = 9999;
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(silver: 5);
            Item.rare = ItemRarityID.Red;
        }
        public override void AddRecipes()
        {
            CreateRecipe(250)
            .AddIngredient(ItemID.ChlorophyteArrow, 250)
            .AddIngredient(ModContent.ItemType<MosquitoVenom>(), 2)
            .AddTile(TileID.LunarCraftingStation)
            .Register();
        }
    }
}
