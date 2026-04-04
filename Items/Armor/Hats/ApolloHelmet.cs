using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Miscannellous.Rarities;

namespace TranscendenceMod.Items.Armor.Hats
{
    [AutoloadEquip(EquipType.Head)]
    public class ApolloHelmet : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            var EquipSlot = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Head);
            ArmorIDs.Head.Sets.DrawHead[EquipSlot] = false;
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 22;

            Item.defense = 22;
            Item.value = Item.sellPrice(gold: 10);
            Item.rare = ModContent.RarityType<CosmicRarity>();
        }
        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<TranscendencePlayer>().ApolloHelmet = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<AstronautHelmet>())
            .AddIngredient(ModContent.ItemType<ApolloPiece>(), 8)
            .AddIngredient(ModContent.ItemType<GalaxyAlloy>(), 2)
            .AddTile(TileID.LunarCraftingStation)
            .Register();
        }
    }
}
