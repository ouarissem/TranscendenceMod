using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TranscendenceMod.Tiles;

namespace TranscendenceMod.Items.Modifiers
{
    public class CrateMagnet : BaseModifier
    {
        public override int RequiredItem => ItemID.CratePotion;
        public override int RequiredAmount => 2;
        public override ModifierIDs ModifierType => ModifierIDs.CrateMagnet;
        public override bool CanBeApplied(Item item) => item.fishingPole > 0;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.width = 24;
            Item.height = 24;
            Item.value = Item.sellPrice(gold: 2, silver: 50);
            Item.rare = ItemRarityID.Orange;

            Item.useAnimation = 15;
            Item.useTime = 15;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<CrateMagnetTile>();
        }
    }
}
