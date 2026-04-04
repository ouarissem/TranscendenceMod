using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TranscendenceMod.Items.Accessories.Offensive;

namespace TranscendenceMod.Items.Modifiers.Upgrades
{
    public class SnowOfInfinity : BaseModifier
    {
        public override bool CanBeApplied(Item item) => item.type == ItemID.EyeoftheGolem;
        public override int RequiredItem => ItemID.SoulofMight;
        public override int RequiredAmount => 20;
        public override ModifierIDs ModifierType => ModifierIDs.EternityUpgrade;
        public override int CraftingResultItem => ModContent.ItemType<Eternal>();

        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.width = 18;
            Item.height = 24;

            Item.value = Item.sellPrice(gold: 5);
            Item.rare = ItemRarityID.Cyan;
        }
        public override Color? GetAlpha(Color lightColor) => Color.White;
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.Glass, 22)
            .AddIngredient(ItemID.SoulofMight, 18)
            .AddIngredient(ItemID.FrostCore, 2)
            .AddTile(TileID.MythrilAnvil)
            .Register();
        }
    }
}