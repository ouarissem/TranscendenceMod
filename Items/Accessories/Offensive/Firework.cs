using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace TranscendenceMod.Items.Accessories.Offensive
{
    public class Firework : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.height = 24;
            Item.width = 24;

            Item.accessory = true;
            Item.value = Item.sellPrice(gold: 1, silver: 25);
            Item.rare = ItemRarityID.Green;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<TranscendencePlayer>().RocketAcc = 1;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.Dynamite, 10)
            .AddIngredient(ItemID.BambooBlock, 20)
            .AddIngredient(ItemID.RedHusk)
            .AddTile(TileID.TinkerersWorkbench)
            .Register();
        }
    }
}
