using Terraria;
using Terraria.ID;
using Terraria.Localization;

namespace TranscendenceMod.Items.Modifiers
{
    public class SlayerOfGiants : BaseModifier
    {
        public override int RequiredItem => ItemID.LunarBar;
        public override int RequiredAmount => 5;
        public override ModifierIDs ModifierType => ModifierIDs.GiantSlayer;
        public override bool CanBeApplied(Item item) => item.headSlot > 0 || item.bodySlot > 0 || item.legSlot > 0;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.width = 22;
            Item.height = 22;
            Item.value = Item.sellPrice(gold: 2);
            Item.rare = ItemRarityID.Red;
        }
    }
}
