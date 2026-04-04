using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Miscannellous.Rarities;
using TranscendenceMod.NPCs.Boss.FrostSerpent;

namespace TranscendenceMod.Items.Consumables.Boss
{
    public class HeartOfTheTundra : ModItem
    {
        readonly int serpent = ModContent.NPCType<FrostSerpent_Head>();
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
        }
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 12;

            Item.rare = ModContent.RarityType<ModdedPurple>();
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useTime = 42;
            Item.useAnimation = 42;
        }
        public override bool CanUseItem(Player player)
        {
            return !NPC.AnyNPCs(serpent) && player.ZoneSnow;
        }
        public override bool? UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer && player.ItemAnimationJustStarted)
            {
                NPC.SpawnOnPlayer(player.whoAmI, serpent);
            }
            return true;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.SnowBlock, 750)
            .AddIngredient(ItemID.IceBlock, 500)
            .AddIngredient(ItemID.SoulofLight, 5)
            .AddIngredient(ItemID.SoulofNight, 5)
            .AddIngredient(ItemID.LunarOre, 5)
            .AddTile(TileID.IceMachine)
            .Register();
        }
    }
}
