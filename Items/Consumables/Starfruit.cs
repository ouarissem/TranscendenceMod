using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Items.Consumables
{
    public class Starfruit : ModItem
    {
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(5);
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 10;
        }
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.LifeFruit);
            Item.value = Item.sellPrice(gold: 5);
            Item.rare = ItemRarityID.Cyan;
            Item.useStyle = ItemUseStyleID.EatFood;
            Item.UseSound = SoundID.Item2;
        }
        public override Color? GetAlpha(Color lightColor) => Color.White;
        public override bool CanUseItem(Player player) => player.ConsumedLifeCrystals == Player.LifeCrystalMax && player.ConsumedLifeFruit == Player.LifeFruitMax;
        public override bool? UseItem(Player player)
        {
            if (player.GetModPlayer<TranscendencePlayer>().EatenStarfruits >= player.GetModPlayer<TranscendencePlayer>().MaxStarfruits)
                return null;

            else if (player.itemAnimation == player.itemAnimationMax)
            {
                SoundEngine.PlaySound(new SoundStyle("TranscendenceMod/Miscannellous/Assets/Sounds/NPCs/Attack/Heartbeat")
                {
                    Volume = 1.25f,
                    MaxInstances = 0,
                }, player.Center);

                TranscendenceUtils.DustRing(player.Center, 10, DustID.YellowStarDust, Color.White, 2, 2f, 5f);
                TranscendenceUtils.DustRing(player.Center, 10, DustID.BlueCrystalShard, Color.White, 2, 2f, 5f);

                player.UseHealthMaxIncreasingItem(5);
                player.GetModPlayer<TranscendencePlayer>().EatenStarfruits++;
            }
            return true;
        }
    }
}
