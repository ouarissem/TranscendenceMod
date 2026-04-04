using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Items.Materials.MobDrops;
using TranscendenceMod.Miscannellous.Rarities;

namespace TranscendenceMod.Items.Armor.Hats
{
    [AutoloadEquip(EquipType.Head)]
    public class AstronautHelmet : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            var EquipSlot = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Head);
            ArmorIDs.Head.Sets.DrawHead[EquipSlot] = false;
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 18;

            Item.defense = 10;
            Item.value = Item.sellPrice(gold: 5);
            Item.rare = ItemRarityID.LightPurple;
        }
        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<TranscendencePlayer>().AstronautHelmet = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddRecipeGroup(nameof(ItemID.TitaniumBar), 10)
            .AddIngredient(ModContent.ItemType<PulverizedPlanet>(), 10)
            .AddIngredient(ItemID.BlueStarryGlassBlock, 10)
            .AddTile(TileID.MythrilAnvil)
            .Register();
        }
    }
}
