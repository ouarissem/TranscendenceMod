using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous.Rarities;

namespace TranscendenceMod.Items.Materials.MobDrops
{
    public class PulverizedPlanet : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 50;
        }
        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 12;
            Item.value = Item.sellPrice(silver: 75);
            Item.rare = ItemRarityID.Blue;
            Item.maxStack = 9999;
        }
    }
}
