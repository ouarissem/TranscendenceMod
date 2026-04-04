using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Items.Farming;

namespace TranscendenceMod.Items.Consumables.Boss
{
    public class PlanteraBulb : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
        }
        public override void SetDefaults()
        {
            Item.width = 13;
            Item.height = 13;
            Item.maxStack = 9999;

            Item.rare = ItemRarityID.Lime;
            Item.consumable = true;

            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useTime = 42;
            Item.useAnimation = 42;
        }
        public override bool CanUseItem(Player player)
        {
            return !NPC.AnyNPCs(NPCID.Plantera) && player.ZoneJungle && (player.ZoneDirtLayerHeight || player.ZoneRockLayerHeight);
        }
        public override bool? UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                NPC.SpawnOnPlayer(player.whoAmI, NPCID.Plantera);
            }
            return true;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.ChlorophyteBar, 8)
            .AddIngredient(ItemID.JungleSpores, 4)
            .AddIngredient(ModContent.ItemType<CocoaBean>(), 2)
            .AddCondition(Condition.NearWater)
            .Register();
        }
    }
}
