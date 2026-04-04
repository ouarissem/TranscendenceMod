using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Items.Materials;

namespace TranscendenceMod.Items.Weapons.Ranged
{
    public class GoldenHarpy : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Ranged;
            Item.damage = 26;
            Item.knockBack = 3f;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.shootSpeed = 32f;

            Item.useTime = 14;
            Item.useAnimation = 14;
            Item.autoReuse = true;

            Item.UseSound = SoundID.Item41;
            Item.useAmmo = AmmoID.Bullet;
            Item.useStyle = ItemUseStyleID.Shoot;

            Item.width = 33;
            Item.height = 25;
            Item.noMelee = true;

            Item.rare = ItemRarityID.LightRed;
            Item.value = Item.sellPrice(gold: 2, silver: 21, copper: 3);

        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-7, 0);
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.PhoenixBlaster, 1)
            .AddIngredient(ModContent.ItemType<SunburntAlloy>(), 12)
            .AddIngredient(ItemID.Feather, 12)
            .AddTile(TileID.SkyMill)
            .Register();
        }
    }
}