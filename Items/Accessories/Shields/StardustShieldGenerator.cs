using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous.Rarities;

namespace TranscendenceMod.Items.Accessories.Shields
{
    public class StardustShieldGenerator : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.rare = ModContent.RarityType<CosmicRarity>();
            Item.width = 18;
            Item.height = 22;
            Item.accessory = true;
            Item.value = Item.sellPrice(gold: 10);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<TranscendencePlayer>().StardustShield = true;
        }
    }
}
