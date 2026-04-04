using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TranscendenceMod.Items.Materials;

namespace TranscendenceMod.Items.Modifiers
{
    public class HardmetalHook : BaseModifier
    {
        public override int RequiredItem => ItemID.SharkFin;
        public override int RequiredAmount => 6;
        public override ModifierIDs ModifierType => ModifierIDs.Hooked;
        public override bool CanBeApplied(Item item) => item.fishingPole > 0;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.width = 16;
            Item.height = 16;
            Item.value = Item.sellPrice(silver: 50);
            Item.rare = ItemRarityID.Orange;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<HardmetalBar>(), 8)
            .AddIngredient(ItemID.Coral, 12)
            .AddTile(TileID.Anvils)
            .Register();
        }
    }
}
