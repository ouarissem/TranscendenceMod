using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.Map;
using Terraria.ModLoader;
using TranscendenceMod.Tiles.TilesheetHell.Nature.Farming;

namespace TranscendenceMod.Items.Farming.Seeds
{
    public abstract class BaseSeed : ModItem
    {
        public virtual bool allowed(Tile tile2) => false;
        public abstract int Tile { get; }
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 15;
        }
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(Tile);

            Item.maxStack = 9999;

            Item.width = 14;
            Item.height = 20;

            Item.useTime = Item.useAnimation;
        }

        public override bool CanUseItem(Player player)
        {
            Vector2 pos = Main.MouseWorld;
            pos /= 16;

            Tile tile = Main.tile[(int)pos.X, (int)pos.Y];
            Tile tile2 = Main.tile[(int)pos.X, (int)pos.Y + 1];

            if (!tile.HasTile && allowed(tile2) && player.Distance(Main.MouseWorld) < (4 * 16) && TranscendenceWorld.AmountCrops < 32)
                return true;

            return false;
        }

        public override bool ConsumeItem(Player player)
        {
            return false;
        }
    }
}
