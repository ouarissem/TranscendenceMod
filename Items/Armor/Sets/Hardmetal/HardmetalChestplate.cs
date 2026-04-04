using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Items.Armor.Sets.Cosmic;
using TranscendenceMod.Items.Materials;

namespace TranscendenceMod.Items.Armor.Sets.Hardmetal
{
    [AutoloadEquip(EquipType.Body)]
    public class HardmetalChestplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            var EquipSlot = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Body);
            ArmorIDs.Body.Sets.HidesTopSkin[EquipSlot] = true;
            ArmorIDs.Body.Sets.HidesBottomSkin[EquipSlot] = true;
            ArmorIDs.Body.Sets.HidesArms[EquipSlot] = true;
        }
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 22;
            Item.rare = ItemRarityID.Green;
            Item.defense = 6;

            Item.value = Item.sellPrice(silver: 40);
        }
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<CosmicChestplate>() && legs.type == ModContent.ItemType<CosmicBoots>();
        }
        public override void UpdateEquip(Player player)
        {
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<HardmetalBar>(), 20)
            .AddIngredient(ItemID.Leather, 4)
            .AddTile(TileID.Anvils)
            .Register();
        }
    }
}
