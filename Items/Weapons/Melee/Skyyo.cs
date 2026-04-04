using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Projectiles.Weapons.Melee;

namespace TranscendenceMod.Items.Weapons.Melee
{
    public class Skyyo : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            ItemID.Sets.Yoyo[Item.type] = true;
        }
        public override void SetDefaults()
        {
            Item.damage = 44;
            Item.DamageType = DamageClass.MeleeNoSpeed;

            Item.width = 16;
            Item.height = 16;

            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item1;

            Item.knockBack = 1.75f;
            Item.value = Item.sellPrice(gold: 3);
            Item.rare = ItemRarityID.Yellow;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.channel = true;
            Item.autoReuse = false;
            Item.shoot = ModContent.ProjectileType<SkyyoProj>();
            Item.crit = 12;
            Item.shootSpeed = 12f;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<SunburntAlloy>(), 6)
            .AddIngredient(ItemID.SunplateBlock, 10)
            .AddIngredient(ItemID.Feather, 4)
            .AddIngredient(ItemID.SoulofSight, 8)
            .AddTile(TileID.SkyMill)
            .Register();
        }
    }
}