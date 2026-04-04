using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Tiles.TerrestrialSecond;

namespace TranscendenceMod.Items.Modifiers
{
    public class CultistScroll : BaseModifier
    {
        public override int RequiredItem => ItemID.Ectoplasm;
        public override int RequiredAmount => 4;
        public override ModifierIDs ModifierType => ModifierIDs.CultistScroll;
        public override bool CanBeApplied(Item item) => item.headSlot > 0 || item.bodySlot > 0 || item.legSlot > 0;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.width = 26;
            Item.height = 18;
            Item.value = Item.sellPrice(gold: 2);
            Item.rare = ItemRarityID.Cyan;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.Silk, 8)
            .AddIngredient(ItemID.Ectoplasm, 12)
            .AddIngredient(ItemID.LunarTabletFragment, 8)
            .AddTile(ModContent.TileType<ExtraTerrestrialLoom>())
            .Register();
        }
    }
}
