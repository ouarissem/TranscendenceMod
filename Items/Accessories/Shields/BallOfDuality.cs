using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace TranscendenceMod.Items.Accessories.Shields
{
    public class BallOfDuality : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.width = 12;
            Item.height = 12;
            Item.accessory = true;
            Item.value = Item.sellPrice(gold: 10);
            Item.rare = ItemRarityID.LightRed;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<TranscendencePlayer>().DualBall = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.WhitePearl)
            .AddIngredient(ItemID.BlackPearl)
            .AddIngredient(ItemID.SoulofLight, 4)
            .AddIngredient(ItemID.SoulofNight, 4)
            .AddTile(TileID.GlassKiln)
            .Register();
        }
    }
}
