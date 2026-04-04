using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Items.Consumables.Placeables.SpaceBiome;
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Miscannellous.Rarities;
using TranscendenceMod.Tiles.BigTiles.Furniture;

namespace TranscendenceMod.Items.Accessories.Shields
{
    [AutoloadEquip(EquipType.Shield)]
    public class CelestialAegis : BaseShield
    {
        public override int Cooldown => 50;

        public override int DefenseAmount => 14;

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 500;
            Item.DamageType = DamageClass.Melee;

            Item.rare = ModContent.RarityType<CosmicRarity>();
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.sellPrice(platinum: 1, gold : 50);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            base.UpdateAccessory(player, hideVisual);
            player.GetModPlayer<TranscendencePlayer>().CosmicAegis = true;
            player.GetModPlayer<TranscendencePlayer>().AegisRamDamage = Item.damage;
        }
        public override bool MeleePrefix() => false;
        public override bool WeaponPrefix() => false;
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<DungeonShield>())
            .AddIngredient(ModContent.ItemType<LunarShield>())
            .AddIngredient(ModContent.ItemType<ChromaticAegis>())
            .AddIngredient(ModContent.ItemType<PalladiumShield>())
            .AddIngredient(ModContent.ItemType<StarcraftedAlloy>(), 3)
            .AddIngredient(ModContent.ItemType<CrystalItem>(), 35)
            .AddIngredient(ItemID.FallenStar, 50)
            .AddTile(ModContent.TileType<StarcraftedForge>())
            .Register();
        }
    }
}
