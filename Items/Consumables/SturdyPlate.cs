using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace TranscendenceMod.Items.Consumables
{
    public class SturdyPlate : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 20;
        }
        public override void SetDefaults()
        {
            Item.useTime = 20;
            Item.useAnimation = 20;

            Item.useStyle = ItemUseStyleID.HoldUp;

            Item.width = 18;
            Item.height = 18;
            Item.consumable = true;
            Item.maxStack = 9999;

            Item.value = Item.sellPrice(silver: 50);
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item37;
        }
        public override bool? UseItem(Player player)
        {
            if (player.ItemAnimationJustStarted)
            {
                if (player.GetModPlayer<TranscendencePlayer>().SturdyPlateTimer < (4 * 60 * 60))
                {
                    player.GetModPlayer<TranscendencePlayer>().SturdyPlateTimer = 8 * 60 * 60;
                }
                else
                {

                    return false;
                }

                for (int i = 0; i < 20; i++)
                {
                    Dust.NewDustPerfect(player.Center, DustID.PlatinumCoin, Main.rand.NextVector2Circular(7.5f, 15));
                }

                if (Item.stack == 1)
                    Item.TurnToAir();
                else Item.stack--;
            }
            return base.UseItem(player);
        }
    }
}
