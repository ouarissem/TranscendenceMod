using Terraria;
using Terraria.ID;
using Terraria.Localization;

namespace TranscendenceMod.Items.Modifiers
{
    public class SunshadeEgg : BaseModifier
    {
        public override int RequiredItem => ItemID.Cobweb;
        public override int RequiredAmount => 50;
        public override ModifierIDs ModifierType => ModifierIDs.Silky;
        public override bool CanBeApplied(Item item) => item.headSlot > 0 || item.bodySlot > 0 || item.legSlot > 0;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.width = 14;
            Item.height = 22;
            Item.value = Item.sellPrice(gold: 2, silver: 75);
            Item.rare = ItemRarityID.Yellow;
        }
    }
}
