using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Items.Weapons.Melee;

namespace TranscendenceMod.Items.Modifiers.Upgrades
{
    public class HardmetalSpike : BaseModifier
    {
        public override bool CanBeApplied(Item item) => item.type == ModContent.ItemType<BaseballBat>();
        public override int RequiredItem => ModContent.ItemType<HardmetalBar>();
        public override int RequiredAmount => 8;
        public override ModifierIDs ModifierType => ModifierIDs.BaseballBatUpgrade;
        public override int CraftingResultItem => ModContent.ItemType<SpikedBaseballBat>();

        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.width = 18;
            Item.height = 20;

            Item.value = Item.sellPrice(silver: 75);
            Item.rare = ItemRarityID.Green;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.Spike, 15)
            .AddIngredient(ModContent.ItemType<HardmetalBar>())
            .AddTile(TileID.Anvils)
            .Register();
        }
    }
}
