using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TranscendenceMod.Items.Consumables.Placeables.SpaceBiome;
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Items.Weapons.Ranged;
using TranscendenceMod.Miscannellous.Rarities;

namespace TranscendenceMod.Items.Modifiers.Upgrades
{
    public class SpaceScrap : BaseModifier
    {
        public override bool CanBeApplied(Item item) => item.type == ItemID.RocketLauncher;
        public override int RequiredItem => ModContent.ItemType<ApolloPiece>();
        public override int RequiredAmount => 4;
        public override ModifierIDs ModifierType => ModifierIDs.CosmicCrystal;
        public override int CraftingResultItem => ModContent.ItemType<CosmosShardLauncher>();

        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.width = 24;
            Item.height = 24;

            Item.value = Item.sellPrice(gold: 5);
            Item.rare = ModContent.RarityType<CosmicRarity>();
        }
        public override Color? GetAlpha(Color lightColor) => Color.White;
    }
}