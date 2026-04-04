using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TranscendenceMod.Items.Materials;

namespace TranscendenceMod.Items.Armor.Sets.Hardmetal
{
    [AutoloadEquip(EquipType.Head)]
    public class HardmetalHelmet : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            var EquipSlot = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Head);
            ArmorIDs.Head.Sets.DrawHead[EquipSlot] = false;
        }
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.rare = ItemRarityID.Green;
            Item.defense = 5;

            Item.value = Item.sellPrice(silver: 40);
        }
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<HardmetalChestplate>() && legs.type == ModContent.ItemType<HardmetalLeggings>();
        }
        public override void UpdateEquip(Player player)
        {
        }
        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = Language.GetTextValue("Mods.TranscendenceMod.Messages.Tooltips.ArmorSetBonuses.Raider");
            player.GetModPlayer<TranscendencePlayer>().RaiderSetWear = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<HardmetalBar>(), 12)
            .AddTile(TileID.Anvils)
            .Register();
        }
    }
}
