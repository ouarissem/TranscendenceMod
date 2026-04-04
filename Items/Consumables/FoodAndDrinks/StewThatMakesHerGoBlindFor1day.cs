using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Buffs.Items.Potions;
using TranscendenceMod.Items.Farming;
using TranscendenceMod.Items.Materials.Fish;
using TranscendenceMod.Items.Materials.MobDrops;
using TranscendenceMod.Miscannellous.Rarities;
using TranscendenceMod.Tiles.TerrestrialSecond;
using TranscendenceMod.Tiles.TilesheetHell.Nature.Farming;

namespace TranscendenceMod.Items.Consumables.FoodAndDrinks
{
    public class StewThatMakesHerGoBlindFor1day : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            Main.RegisterItemAnimation(Type, new DrawAnimationVertical(int.MaxValue, 3));

            ItemID.Sets.FoodParticleColors[Type] = new Color[2]
            {
                new Color(161, 255, 254),
                new Color(15, 67, 198)
            };
        }
        public override void SetDefaults()
        {
            Item.DefaultToFood(22, 22, ModContent.BuffType<Blind>(), 24 * 60 * 60, true);
            Item.width = 24;
            Item.height = 16;
            ItemID.Sets.IsFood[Type] = true;
            Item.value = Item.sellPrice(gold: 1, silver: 25);
            Item.rare = ModContent.RarityType<ModdedPurple>();
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<BlackholeFish>(), 4)
            .AddIngredient(ModContent.ItemType<OrbitalFish>(), 2)
            .AddIngredient(ModContent.ItemType<RedSpiderLily>(), 8)
            .AddIngredient(ModContent.ItemType<Potato>(), 8)
            .AddIngredient(ModContent.ItemType<Tomato>(), 8)
            .AddIngredient(ModContent.ItemType<VoidFragment>(), 4)
            .AddTile(ModContent.TileType<ExtraTerrestrialPot>())
            .Register();
        }
    }
}
