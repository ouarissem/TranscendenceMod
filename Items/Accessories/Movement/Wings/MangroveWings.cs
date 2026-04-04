using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Items.Materials.LargeRecipes;
using TranscendenceMod.Miscannellous.Rarities;

namespace TranscendenceMod.Items.Accessories.Movement.Wings
{
    [AutoloadEquip(EquipType.Wings)]
    public class MangroveWings : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.rare = ModContent.RarityType<ModdedPurple>();
            Item.width = 29;
            Item.height = 24;
            Item.accessory = true;
            Item.value = Item.sellPrice(gold: 15);
            ArmorIDs.Wing.Sets.Stats[Item.wingSlot] = new WingStats(180, 9, 1f, true, 10);
        }
        public override void UpdateEquip(Player player)
        {
            base.UpdateEquip(player);
        }
        public override void HorizontalWingSpeeds(Player player, ref float speed, ref float acceleration)
        {
            speed = 10;
            acceleration = 1f;
            player.runSlowdown += 0.75f;
        }
        public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising, ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
        {
            ascentWhenFalling = 0.8f;
            ascentWhenRising = 1f;
            maxCanAscendMultiplier = 0.75f;
            maxAscentMultiplier = 1.5f;
            constantAscend = 0.375f;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.LeafWings)
            .AddIngredient(ModContent.ItemType<LivingOrganicMatter>(), 3)
            .AddIngredient(ItemID.SoulofFlight, 12)
            .AddTile(TileID.MythrilAnvil)
            .Register();
        }
    }
}
