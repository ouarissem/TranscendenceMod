using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Items.Consumables.Placeables.SpaceBiome;
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Items.Materials.MobDrops;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Items.Modifiers
{
    public class AbsolutePurity : BaseModifier
    {
        public override ModifierIDs ModifierType => ModifierIDs.None;
        public override bool CanBeApplied(Item item) => item.GetGlobalItem<ModifiersItem>().Modifier > 0 && item.ModItem is not BaseModifier;
        public override int RequiredItem => ItemID.FallenStar;
        public override int RequiredAmount => 2;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.width = 24;
            Item.height = 18;
            Item.value = Item.sellPrice(silver: 50);
            Item.rare = ItemRarityID.Blue;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<AetherRootItem>(), 8)
            .AddIngredient(ModContent.ItemType<PulverizedPlanet>(), 2)
            .AddTile(TileID.WorkBenches)
            .Register();
        }
    }
}
