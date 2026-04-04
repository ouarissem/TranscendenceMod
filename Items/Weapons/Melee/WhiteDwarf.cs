using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Items.Consumables.Placeables.SpaceBiome;
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Items.Materials.MobDrops;
using TranscendenceMod.Miscannellous.Rarities;
using TranscendenceMod.Projectiles.Weapons.Melee;
using TranscendenceMod.Tiles.BigTiles;
using TranscendenceMod.Tiles.TilesheetHell.Nature;

namespace TranscendenceMod.Items.Weapons.Melee
{
    public class WhiteDwarf : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            ItemID.Sets.Yoyo[Item.type] = true;
        }
        public override void SetDefaults()
        {
            Item.damage = 175;
            Item.DamageType = DamageClass.MeleeNoSpeed;
            Item.width = 16;
            Item.height = 16;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 2;
            Item.value = Item.sellPrice(gold: 50);
            Item.rare = ModContent.RarityType<Brown>();
            Item.UseSound = SoundID.Item1;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.channel = true;
            Item.autoReuse = false;
            Item.shoot = ModContent.ProjectileType<WhiteDwarfProj>();
            Item.crit = 15;
            Item.shootSpeed = 16f;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<DysphoricThrow>())
            .AddIngredient(ModContent.ItemType<SpaceRockItem>(), 20)
            .AddIngredient(ItemID.FragmentVortex, 17)
            .AddIngredient(ModContent.ItemType<PoseidonsTide>(), 4)
            .AddTile(ModContent.TileType<ShimmerAltar>())
            .Register();
        }
    }
}