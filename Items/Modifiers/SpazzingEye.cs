using Terraria;
using Terraria.ID;
using Terraria.Localization;

namespace TranscendenceMod.Items.Modifiers
{
    public class SpazzingEye : BaseModifier
    {
        public override int RequiredItem => ItemID.SoulofSight;
        public override int RequiredAmount => 5;
        public override ModifierIDs ModifierType => ModifierIDs.Spazzy;
        public override bool CanBeApplied(Item item) => item.headSlot > 0;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.width = 20;
            Item.height = 20;
            Item.value = Item.sellPrice(gold: 1, silver: 25);
            Item.rare = ItemRarityID.LightRed;
        }
    }
}
