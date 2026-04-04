using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous.Rarities;

namespace TranscendenceMod.Items.Accessories.Defensive
{
    public class StormCloak : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.rare = ModContent.RarityType<Brown>();
            Item.width = 25;
            Item.height = 30;
            Item.accessory = true;
            Item.value = Item.sellPrice(gold: 17, silver: 50);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<TranscendencePlayer>().BatteryAcc = 2;
            player.longInvince = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.StarCloak)
            .AddIngredient(ModContent.ItemType<SquidAccessory>())
            .AddTile(TileID.TinkerersWorkbench)
            .Register();
        }
    }
}
