using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Items.Materials.MobDrops;

namespace TranscendenceMod.Items.Accessories.Shields
{
    public class ForcefieldOfTheImpendingDoom : ModItem
    {
        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 18));
            ItemID.Sets.AnimatesAsSoul[Type] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Cyan;
            Item.width = 20;
            Item.height = 28;
            Item.accessory = true;
            Item.value = Item.sellPrice(gold: 20);
        }
        public override Color? GetAlpha(Color lightColor) => Color.White;
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<TranscendencePlayer>().CultistForcefield = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<StardustShieldGenerator>())
            .AddIngredient(ItemID.LunarTabletFragment, 12)
            .AddIngredient(ItemID.FragmentStardust, 12)
            .AddTile(TileID.LunarCraftingStation)
            .Register();
        }
    }
}
