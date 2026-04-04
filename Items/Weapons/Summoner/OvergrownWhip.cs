using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Items.Materials.LargeRecipes;
using TranscendenceMod.Items.Materials.MobDrops;
using TranscendenceMod.Miscannellous.Rarities;
using TranscendenceMod.Projectiles.Weapons.Summoner;

namespace TranscendenceMod.Items.Weapons.Summoner
{
    public class OvergrownWhip : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 145;
            Item.DamageType = DamageClass.SummonMeleeSpeed;
            Item.knockBack = 4;
            Item.crit = 10;

            Item.width = 16;
            Item.height = 18;

            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.UseSound = SoundID.Item1;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.noUseGraphic = true;
            Item.autoReuse = true;

            Item.value = Item.sellPrice(gold: 15);
            Item.rare = ModContent.RarityType<ModdedPurple>();
            Item.shoot = ModContent.ProjectileType<OvergrownWhipProj>();
            Item.shootSpeed = 8f;
        }
        public override bool MeleePrefix() => true;
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.ThornWhip)
            .AddIngredient(ModContent.ItemType<LivingOrganicMatter>(), 2)
            .AddIngredient(ModContent.ItemType<MosquitoVenom>(), 6)
            .AddTile(TileID.LunarCraftingStation)
            .Register();
        }
    }
}