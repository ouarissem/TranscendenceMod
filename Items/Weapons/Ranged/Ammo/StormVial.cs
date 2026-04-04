using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Items.Materials.MobDrops;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Projectiles.Weapons.Ranged.Ammo;

namespace TranscendenceMod.Items.Weapons.Ranged.Ammo
{
    public class StormVial : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
        }
        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Ranged;
            Item.damage = 18;
            Item.knockBack = 2;
            Item.ammo = Item.type;
            Item.shoot = ModContent.ProjectileType<ToxicArrowProj>();
            Item.shootSpeed = 10;
            Item.consumable = true;
            Item.maxStack = 9999;
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(silver: 5);
            Item.rare = ItemRarityID.Red;
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            TranscendenceUtils.DrawItemGlowmask(Item, rotation, scale, Texture);
        }
        public override void AddRecipes()
        {
            CreateRecipe(175)
            .AddIngredient(ModContent.ItemType<Lightning>(), 2)
            .AddIngredient(ItemID.FragmentVortex, 4)
            .AddTile(TileID.MythrilAnvil)
            .Register();
        }
    }
}
