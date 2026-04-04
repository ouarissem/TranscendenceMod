using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous.Rarities;

namespace TranscendenceMod.Items.Mounts
{
    public class AirPiercerBroomItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 35;
            Item.height = 35;

            Item.useTime = 15;
            Item.useAnimation = 15;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = Item.sellPrice(gold: 4);
            Item.rare = ModContent.RarityType<Brown>();
            Item.UseSound = SoundID.Item3;

            Item.noMelee = true;
            Item.mountType = ModContent.MountType<AirPiercerBroom>();
        }
    }
}
