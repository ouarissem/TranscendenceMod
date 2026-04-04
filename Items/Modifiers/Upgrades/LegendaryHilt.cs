using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Items.Weapons.Melee;

namespace TranscendenceMod.Items.Modifiers.Upgrades
{
    public class LegendaryHilt : BaseModifier
    {
        public override bool CanBeApplied(Item item) => item.type == ItemID.WoodenSword;
        public override int RequiredItem => ItemID.PlatinumBar;
        public override int RequiredAmount => 12;
        public override ModifierIDs ModifierType => ModifierIDs.LegendarySword;
        public override int CraftingResultItem => ModContent.ItemType<LegendarySword>();


        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.width = 18;
            Item.height = 20;

            Item.value = Item.sellPrice(gold: 2, silver: 50);
            Item.rare = ItemRarityID.Blue;
        }
    }
}
