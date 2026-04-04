using Terraria;
using Terraria.ID;
using Terraria.Localization;

namespace TranscendenceMod.Items.Modifiers
{
    public class EnchantedOrb : BaseModifier
    {
        public override int RequiredItem => ItemID.Damselfish;
        public override int RequiredAmount => 6;
        public override ModifierIDs ModifierType => ModifierIDs.EnchantedPearl;
        public override bool CanBeApplied(Item item) => item.fishingPole > 0;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.width = 16;
            Item.height = 16;
            Item.value = Item.sellPrice(gold: 2);
            Item.rare = ItemRarityID.Orange;
        }
    }
}
