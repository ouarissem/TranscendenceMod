using Terraria;
using Terraria.GameContent.Creative;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Items.Armor.Hats;
using TranscendenceMod.Items.Consumables.Placeables.Decorations;
using TranscendenceMod.Items.Weapons.Magic;
using TranscendenceMod.Items.Weapons.Melee;
using TranscendenceMod.NPCs.Boss.FrostSerpent;
using TranscendenceMod.Items.Weapons.Ranged;
using TranscendenceMod.Items.Consumables.Placeables;
using TranscendenceMod.Items.Accessories.Expert;
using TranscendenceMod.Items.Weapons.Summoner;

namespace TranscendenceMod.Items.Consumables.LootBags
{
    public class FrostSerpentBag : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.BossBag[Type] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
        }
        public override void SetDefaults()
        {
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
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<FrozenMaw>()));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<FrostMonolithItem>(), 3));
            itemLoot.Add(ItemDropRule.Common(ItemID.LunarOre, 1, 25, 40));

            itemLoot.Add(ItemDropRule.FewFromOptions(2, 1,
                ModContent.ItemType<FrozenMaws>(),
                ModContent.ItemType<IceKunai>(),
                ModContent.ItemType<MountaintopGlacier>()));

            itemLoot.Add(ItemDropRule.NotScalingWithLuck(ModContent.ItemType<FaeMask>(), 7));
            itemLoot.Add(ItemDropRule.NotScalingWithLuck(ModContent.ItemType<FaeTrophyItem>(), 10));
            itemLoot.Add(ItemDropRule.CoinsBasedOnNPCValue(ModContent.NPCType<FrostSerpent_Head>()));
        }
        public override bool CanRightClick()
        {
            return true;
        }
    }
}
