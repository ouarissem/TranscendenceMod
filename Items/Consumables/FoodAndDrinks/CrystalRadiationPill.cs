using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Buffs.Items.Potions;
using TranscendenceMod.Miscannellous.Rarities;

namespace TranscendenceMod.Items.Consumables.FoodAndDrinks
{
    public class CrystalRadiationPill : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 20;
        }
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.ArcheryPotion);

            Item.buffTime = 4 * 60 * 60;
            Item.buffType = ModContent.BuffType<CrystalRadiationProt>();

            Item.width = 16;
            Item.height = 16;
            Item.value = Item.sellPrice(gold: 1);
            Item.UseSound = SoundID.Item2;
            Item.rare = ModContent.RarityType<CosmicRarity>();
        }
    }
}
