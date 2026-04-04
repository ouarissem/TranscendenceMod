using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Items.Materials.MobDrops;
using TranscendenceMod.Miscannellous.Rarities;
using TranscendenceMod.Tiles.BigTiles;

namespace TranscendenceMod.Items.Accessories.Shields
{
    public class DungeonShield : BaseShield
    {
        public override int Cooldown => 60;

        public override int DefenseAmount => 10;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ModContent.RarityType<Brown>();
            Item.width = 35;
            Item.height = 25;
            Item.value = Item.sellPrice(gold: 50);
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.CobaltShield)
            .AddIngredient(ItemID.SpectreBar, 15)
            .AddIngredient(ModContent.ItemType<PoseidonsTide>(), 18)
            .AddIngredient(ItemID.Bone, 200)
            .AddTile(ModContent.TileType<Oceation>())
            .Register();
        }
    }
}
