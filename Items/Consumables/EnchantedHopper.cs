using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.NPCs.Passive;

namespace TranscendenceMod.Items.Consumables
{
    public class EnchantedHopper : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.EnchantedNightcrawler);
            Item.bait = 30;
            Item.value = Item.sellPrice(silver: 15);
            Item.makeNPC = ModContent.NPCType<Nighthopper>();
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.Grasshopper, 1)
            .AddIngredient(ItemID.FallenStar, 1)
            .Register();
        }
    }
}
