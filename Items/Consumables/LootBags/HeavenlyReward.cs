using Terraria;
using Terraria.GameContent.Creative;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Items.Consumables.Placeables.SpaceBiome;
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Items.Accessories.Shields;
using TranscendenceMod.Miscannellous.Rarities;
using TranscendenceMod.Items.Modifiers.Upgrades;
using TranscendenceMod.Items.Farming.Seeds;
using TranscendenceMod.Items.Materials.MobDrops;

namespace TranscendenceMod.Items.Consumables.LootBags
{
    public class HeavenlyReward : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
        }
        public override void SetDefaults()
        {
            Item.consumable = true;
            Item.width = 32;
            Item.height = 24;
            Item.value = Item.sellPrice(gold: 10);
            Item.rare = ModContent.RarityType<ModdedPurple>();
            Item.maxStack = 9999;
        }
        public override void ModifyItemLoot(ItemLoot itemLoot)
        {
            int box = ModContent.ItemType<PortalBox>();
            int shield = ModContent.ItemType<StardustShieldGenerator>();
            int scrap = ModContent.ItemType<SpaceScrap>();

            int rock = ModContent.ItemType<SpaceRockItem>();
            int root = ModContent.ItemType<AetherRootItem>();
            int crystal = ModContent.ItemType<CrystalItem>();

            IItemDropRule[] Generic = new IItemDropRule[] {
                ItemDropRule.Common(ItemID.MeteoriteBar, 2, 14, 20),
                ItemDropRule.Common(ItemID.LunarBar, 2, 6, 14),
                ItemDropRule.Common(ModContent.ItemType<SunburntAlloy>(), 2, 8, 12),

                ItemDropRule.Common(ModContent.ItemType<FlowerEssence>(), 2, 1, 3),
                ItemDropRule.Common(ModContent.ItemType<Starfruit>(), 2, 1, 2),
                ItemDropRule.Common(ModContent.ItemType<StarfruitSeeds>(), 4, 4, 6),

                ItemDropRule.Common(ItemID.FragmentSolar, 2, 4, 8),
                ItemDropRule.Common(ItemID.FragmentVortex, 2, 4, 8),
                ItemDropRule.Common(ItemID.FragmentNebula, 2, 4, 8),
                ItemDropRule.Common(ItemID.FragmentStardust, 2, 4, 8)
            };
            IItemDropRule[] Tiles = new IItemDropRule[] {
                ItemDropRule.Common(rock, 2, 9, 16),
                ItemDropRule.Common(root, 2, 9, 16),
                ItemDropRule.Common(crystal, 3, 5, 10),
            };

            ItemDropRule.Coins(Main.rand.Next(Item.sellPrice(gold: 12, silver: 50), Item.sellPrice(gold: 17, silver: 75)), false);
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<ApolloPiece>(), 1, 2, 5));

            itemLoot.Add(new AlwaysAtleastOneSuccessDropRule(Generic));
            itemLoot.Add(new AlwaysAtleastOneSuccessDropRule(Generic));

            itemLoot.Add(new AlwaysAtleastOneSuccessDropRule(Tiles));
            itemLoot.Add(new OneFromOptionsDropRule(3, 1, box, shield, scrap));
        }
        public override bool CanRightClick() => true;
    }
}
