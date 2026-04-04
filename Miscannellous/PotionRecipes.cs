using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Items.Consumables.Placeables.SpaceBiome;
using TranscendenceMod.Items.Farming;
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Items.Materials.MobDrops;

namespace TranscendenceMod
{
    public class PotionRecipes : ModSystem
    {
        public override void AddRecipes()
        {
            CraftRecipe(ItemID.Sake, ModContent.ItemType<Wheat>(), 3);
            CraftRecipe(ItemID.SwiftnessPotion, ModContent.ItemType<CocoaBean>(), 3);
            CraftRecipe(ItemID.WrathPotion, ItemID.Ebonkoi, 3);
            CraftRecipe(ItemID.RagePotion, ItemID.Hemopiranha, 3);
            CraftRecipe(ItemID.IronskinPotion, ModContent.ItemType<HardmetalBar>(), 3);
            CraftRecipe(ItemID.RegenerationPotion, ItemID.Mushroom, 6);
            CraftRecipe(ItemID.GillsPotion, ItemID.Coral, 6);
            CraftRecipe(ItemID.HunterPotion, ItemID.SharkFin, 3);
            CraftRecipe(ItemID.WaterWalkingPotion, ItemID.Salmon, 3);
            CraftRecipe(ItemID.ArcheryPotion, ItemID.Lens, 3);
            CraftRecipe(ItemID.ThornsPotion, ItemID.Cactus, 12);
            CraftRecipe(ItemID.InvisibilityPotion, ItemID.AtlanticCod, 3);
            CraftRecipe(ItemID.ShinePotion, ItemID.GlowingMushroom, 12);
            CraftRecipe(ItemID.NightOwlPotion, ItemID.NeonTetra, 3);
            CraftRecipe(ItemID.BattlePotion, ItemID.AntlionMandible, 3);
            CraftRecipe(ItemID.CalmingPotion, ItemID.Damselfish, 3);
            CraftRecipe(ItemID.SpelunkerPotion, ItemID.GoldDust, 6);
            CraftRecipe(ItemID.ObsidianSkinPotion, ItemID.Obsidian, 9);
            CraftRecipe(ItemID.ManaRegenerationPotion, ItemID.FallenStar, 3);
            CraftRecipe(ItemID.MagicPowerPotion, ItemID.PixieDust, 3);
            CraftRecipe(ItemID.FeatherfallPotion, ItemID.Feather, 3);
            CraftRecipe(ItemID.GravitationPotion, ItemID.RainCloud, 9);
            CraftRecipe(ItemID.FishingPotion, ItemID.RedSnapper, 6);
            CraftRecipe(ItemID.CratePotion, ItemID.Amber, 3);
            CraftRecipe(ItemID.BuilderPotion, ItemID.RedBrick, 12);
            CraftRecipe(ItemID.TitanPotion, ItemID.JungleSpores, 3);
            CraftRecipe(ItemID.SummoningPotion, ItemID.BeeWax, 9);
            CraftRecipe(ItemID.HeartreachPotion, ItemID.Vine, 3);
            CraftRecipe(ItemID.FlipperPotion, ItemID.Starfish, 6);
            CraftRecipe(ItemID.MiningPotion, ModContent.ItemType<CarbonOre>(), 12);
            CraftRecipe(ItemID.SonarPotion, ItemID.VariegatedLardfish, 3);
            CraftRecipe(ItemID.TrapsightPotion, ItemID.Stinger, 3);
            CraftRecipe(ItemID.InfernoPotion, ItemID.FlarefinKoi, 3);
            CraftRecipe(ItemID.EndurancePotion, ItemID.ArmoredCavefish, 3);
            CraftRecipe(ItemID.LifeforcePotion, ItemID.PrincessFish, 3);
            CraftRecipe(ItemID.AmmoReservationPotion, ItemID.DoubleCod, 3);
            CraftRecipe(ItemID.WarmthPotion, ItemID.FrostMinnow, 3);
            CraftRecipe(ItemID.BiomeSightPotion, ItemID.HallowedSeeds, 3);
            CraftRecipe(ItemID.LuckPotionLesser, ItemID.WhitePearl, 3);
            CraftRecipe(ItemID.LuckPotion, ItemID.BlackPearl, 3);
            CraftRecipe(ItemID.LuckPotionGreater, ItemID.PinkPearl, 3);

            CraftRecipe(ItemID.RecallPotion, ItemID.SpecularFish, 3);
            CraftRecipe(ItemID.PotionOfReturn, ItemID.HellstoneBar, 6);
            CraftRecipe(ItemID.TeleportationPotion, ModContent.ItemType<CrystalItem>(), 12);


            // Uses special recipe, so it will not use the CraftRecipe() method
            Recipe transgernder = Recipe.Create(ItemID.GenderChangePotion);
            transgernder.AddIngredient(ItemID.BottledWater);
            transgernder.AddIngredient(ModContent.ItemType<FlowerEssence>());
            transgernder.AddCondition(Condition.NearShimmer);
            transgernder.DisableDecraft();
            transgernder.Register();




            void CraftRecipe(int Result, int Ingredient, int IngredientCount)
            {
                Recipe.Create(Result, 6)
                    .AddIngredient(ItemID.BottledWater, 6)
                    .AddIngredient(Ingredient, IngredientCount)
                    .AddIngredient(ModContent.ItemType<FlowerEssence>())
                    .AddCondition(Condition.NearShimmer)
                    .DisableDecraft()
                    .Register();
            }
        }
    }
}