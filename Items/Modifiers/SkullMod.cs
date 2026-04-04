using Terraria;
using Terraria.ID;
using Terraria.Localization;

namespace TranscendenceMod.Items.Modifiers
{
    public class SkullMod : BaseModifier
    {
        public override int RequiredItem => ItemID.PoisonedKnife;
        public override int RequiredAmount => 25;
        public override ModifierIDs ModifierType => ModifierIDs.DangerDetecting;
        public override bool CanBeApplied(Item item) => item.headSlot > 0;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.width = 12;
            Item.height = 12;
            Item.value = Item.sellPrice(gold: 1);
            Item.rare = ItemRarityID.Green;
        }
    }
}
