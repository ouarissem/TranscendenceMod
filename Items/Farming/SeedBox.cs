using Terraria;
using Terraria.GameContent.Creative;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Items.Farming.Seeds;

namespace TranscendenceMod.Items.Farming
{
    public class SeedBox : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 10;
            ItemID.Sets.OpenableBag[Type] = true;
        }
        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Green;
            Item.maxStack = 9999;
            Item.consumable = true;
        }
        public override void ModifyResearchSorting(ref ContentSamples.CreativeHelper.ItemGroup itemGroup)
        {
            itemGroup = ContentSamples.CreativeHelper.ItemGroup.GoodieBags;
        }
        public override void ModifyItemLoot(ItemLoot itemLoot)
        {
            IItemDropRule[] seed = new IItemDropRule[]
            {
                ItemDropRule.Common(ModContent.ItemType<WheatSeed>(), 2),
                ItemDropRule.Common(ModContent.ItemType<PotatoSeed>(), 2),
                ItemDropRule.Common(ModContent.ItemType<CarrotSeeds>(), 3),
                ItemDropRule.Common(ModContent.ItemType<TomatoSeeds>(), 3),
                ItemDropRule.Common(ModContent.ItemType<CocoaBeanSeeds>(), 4),
                ItemDropRule.ByCondition(new Conditions.IsCrimson(), ItemID.CorruptSeeds, 3, 2, 6),
                ItemDropRule.ByCondition(new Conditions.IsCorruption(), ItemID.CrimsonSeeds, 3, 2, 6)
            };

            itemLoot.Add(new FewFromRulesRule(1, 3, seed));
        }
        public override bool CanRightClick() => true;
    }
}
