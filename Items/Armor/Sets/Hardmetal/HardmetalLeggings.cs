using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Items.Materials;

namespace TranscendenceMod.Items.Armor.Sets.Hardmetal
{
    [AutoloadEquip(EquipType.Legs)]
    public class HardmetalLeggings : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            var EquipSlot = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Legs);
            ArmorIDs.Legs.Sets.OverridesLegs[EquipSlot] = true;
        }
        public override void SetDefaults()
        {
            Item.width = 14;
            Item.height = 22;
            Item.rare = ItemRarityID.Green;
            Item.defense = 5;

            Item.value = Item.sellPrice(silver: 40);
        }
        public override void UpdateEquip(Player player)
        {
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<HardmetalBar>(), 16)
            .AddTile(TileID.Anvils)
            .Register();
        }
    }
}
