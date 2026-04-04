using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Projectiles.Weapons.Ranged.Ammo;

namespace TranscendenceMod.Items.Weapons.Ranged.Ammo
{
    public class TempleArrow : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
        }
        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Ranged;
            Item.damage = 17;
            Item.knockBack = 2;
            Item.ammo = AmmoID.Arrow;

            Item.shoot = ModContent.ProjectileType<TempleArrowProj>();
            Item.shootSpeed = 10;

            Item.consumable = true;
            Item.maxStack = 9999;
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(silver: 5);
            Item.rare = ItemRarityID.Yellow;
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            TranscendenceUtils.DrawItemGlowmask(Item, rotation, scale, Texture);
        }
        public override void AddRecipes()
        {
            CreateRecipe(200)
            .AddIngredient(ItemID.BoneArrow, 150)
            .AddIngredient(ItemID.LunarTabletFragment, 10)
            .AddTile(TileID.LunarCraftingStation)
            .Register();
        }
    }
}
