using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace TranscendenceMod.Items.Accessories.Vanity
{
    public class LegCutter : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            SetupDrawing();
        }
        public override void SetDefaults()
        {
            Item.height = 12;
            Item.width = 18;

            Item.accessory = true;
            Item.vanity = true;

            Item.value = Item.sellPrice(gold: 5);
            Item.rare = ItemRarityID.Green;
        }
        public override void UpdateVanity(Player player)
        {
            player.GetModPlayer<TranscendencePlayer>().HideLegs = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<TranscendencePlayer>().HideLegs = true;
        }
        public override void Load()
        {
            if (Main.netMode != NetmodeID.Server)
            {
                EquipLoader.AddEquipTexture(Mod, $"{Texture}_Legs", EquipType.Legs, this);
            }
        }
        private void SetupDrawing()
        {
            if (Main.netMode != NetmodeID.Server)
            {
                int legs = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Legs);
                ArmorIDs.Legs.Sets.HidesBottomSkin[legs] = true;
                ArmorIDs.Legs.Sets.HidesTopSkin[legs] = true;
            }
        }
    }
}
