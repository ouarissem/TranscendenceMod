using Terraria;
using Terraria.ID;
using Terraria.Localization;

namespace TranscendenceMod.Items.Modifiers
{
    public class MothronLamp : BaseModifier
    {
        public override int RequiredItem => ItemID.LunarTabletFragment;
        public override int RequiredAmount => 6;
        public override ModifierIDs ModifierType => ModifierIDs.Luminous;
        public override bool CanBeApplied(Item item) => item.headSlot > 0 || item.bodySlot > 0 || item.legSlot > 0;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.width = 18;
            Item.height = 26;
            Item.value = Item.sellPrice(gold: 2, silver: 75);
            Item.rare = ItemRarityID.Yellow;
        }
    }
}
