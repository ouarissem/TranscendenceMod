using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Items.Weapons.Ranged;

namespace TranscendenceMod.Items.Modifiers.Upgrades
{
    public class ClockworkCog : BaseModifier
    {
        public override bool CanBeApplied(Item item) => item.type == ItemID.ClockworkAssaultRifle;
        public override int RequiredItem => ItemID.Cog;
        public override int RequiredAmount => 99;
        public override ModifierIDs ModifierType => ModifierIDs.ClockworkUpgrade;
        public override int CraftingResultItem => ModContent.ItemType<Backfirer>();

        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.width = 18;
            Item.height = 20;

            Item.value = Item.sellPrice(gold: 1);
            Item.rare = ItemRarityID.LightRed;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.Cog, 50)
            .AddRecipeGroup(nameof(ItemID.TitaniumBar), 10)
            .AddIngredient(ItemID.SoulofFright, 10)
            .AddTile(TileID.Anvils)
            .Register();
        }
    }
}
