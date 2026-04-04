using Terraria;
using Terraria.GameContent.Creative;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Items.Armor.Hats;
using TranscendenceMod.Items.Consumables.Placeables.Decorations;
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Items.Weapons.Magic;
using TranscendenceMod.Items.Weapons.Melee;
using TranscendenceMod.Items.Weapons.Summoner;
using TranscendenceMod.Miscannellous.GlobalStuff;
using TranscendenceMod.Items.Accessories.Offensive;
using TranscendenceMod.NPCs.Boss.Seraph;
using TranscendenceMod.Items.Accessories.Expert;
using TranscendenceMod.Items.Cosmetics;
using TranscendenceMod.Items.Weapons.Ranged;
using TranscendenceMod.Items.Materials.MobDrops;

namespace TranscendenceMod.Items.Consumables.LootBags
{
    public class CelestialSeraphBag : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.BossBag[Type] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
        }
        public override void SetDefaults()
        {
            Item.GetGlobalItem<TranscendenceItem>().SeraphDifficultyItem = true;
            Item.rare = ItemRarityID.Expert;

            Item.expert = true;
            Item.consumable = true;
            Item.width = 32;
            Item.height = 32;
            Item.value = 0;
            Item.maxStack = 9999;
        }
        public override void ModifyItemLoot(ItemLoot itemLoot)
        {
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<VoidNecklace>()));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<ShimmerChunk>(), 1, 40, 52));

            itemLoot.Add(ItemDropRule.FewFromOptions(3, 1,
                ModContent.ItemType<LunaticFlail>(),
                ModContent.ItemType<SpaceBow>(),
                ModContent.ItemType<CelestialSeraphStaff>(),
                ModContent.ItemType<Constellations>(),
                ModContent.ItemType<Starfield>()));

            itemLoot.Add(ItemDropRule.FewFromOptions(1, 2, ModContent.ItemType<AngelicHairdye>(),
                ModContent.ItemType<EarthHairdye>(), ModContent.ItemType<CosmicFogDye>()));

            itemLoot.Add(ItemDropRule.NotScalingWithLuck(ModContent.ItemType<FaeMask>(), 3));
            itemLoot.Add(ItemDropRule.NotScalingWithLuck(ModContent.ItemType<FaeTrophyItem>(), 4));
            itemLoot.Add(ItemDropRule.CoinsBasedOnNPCValue(ModContent.NPCType<CelestialSeraph>()));
        }
        public override bool CanRightClick()
        {
            return true;
        }
    }
}
