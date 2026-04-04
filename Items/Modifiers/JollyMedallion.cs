using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TranscendenceMod.Items.Materials;

namespace TranscendenceMod.Items.Modifiers
{
    public class JollyMedallion : BaseModifier
    {
        public override int RequiredItem => ItemID.SoulofFright;
        public override int RequiredAmount => 5;
        public override ModifierIDs ModifierType => ModifierIDs.Jolly;
        public override bool CanBeApplied(Item item) => item.headSlot > 0 || item.bodySlot > 0 || item.legSlot > 0;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.width = 12;
            Item.height = 12;
            Item.value = Item.buyPrice(gold: 20);
            Item.rare = ItemRarityID.Orange;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<HardmetalBar>(), 8)
            .AddIngredient(ItemID.Coral, 12)
            .AddTile(TileID.Anvils)
            .Register();
        }
    }
}
