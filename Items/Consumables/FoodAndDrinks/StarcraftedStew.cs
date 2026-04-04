using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Items.Materials.Fish;
using TranscendenceMod.Miscannellous.Rarities;
using TranscendenceMod.Tiles.TerrestrialSecond;

namespace TranscendenceMod.Items.Consumables.FoodAndDrinks
{
    public class StarcraftedStew : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 5;
            Main.RegisterItemAnimation(Type, new DrawAnimationVertical(int.MaxValue, 3));

            ItemID.Sets.FoodParticleColors[Type] = new Color[2]
            {
                new Color(161, 255, 254),
                new Color(15, 67, 198)
            };
        }
        public override void SetDefaults()
        {
            Item.DefaultToFood(22, 22, BuffID.WellFed2, 12 * 60 * 60, true);
            Item.width = 24;
            Item.height = 16;
            ItemID.Sets.IsFood[Type] = true;
            Item.healMana = 200;
            Item.value = Item.sellPrice(gold: 1, silver: 25);
            Item.rare = ModContent.RarityType<ModdedPurple>();
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<OrbitalFish>(), 3)
            .AddTile(ModContent.TileType<ExtraTerrestrialPot>())
            .Register();
        }
    }
}
