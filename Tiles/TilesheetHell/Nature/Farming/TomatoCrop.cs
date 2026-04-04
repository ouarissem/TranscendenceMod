using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ObjectData;
using TranscendenceMod.Items.Farming;
using TranscendenceMod.Items.Farming.Seeds;

namespace TranscendenceMod.Tiles.TilesheetHell.Nature.Farming
{
    public class TomatoCrop : BaseCrop
    {
        public override int drop => ModContent.ItemType<Tomato>();

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            AddMapEntry(commonCol, ModContent.GetInstance<TomatoSeeds>().DisplayName);

            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.DrawYOffset = 2;

            ModTileEntity te = ModContent.GetInstance<TomatoTE>();
            TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(te.Hook_AfterPlacement, -1, 0, true);

            TileObjectData.newTile.UsesCustomCanPlace = true;
            TileObjectData.addTile(Type);
        }
        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            base.KillMultiTile(i, j, frameX, frameY);
            ModContent.GetInstance<TomatoTE>().Kill(i, j);
        }
    }
    public class TomatoTE : BaseCropEntity
    {
        public override int TileID => ModContent.TileType<TomatoCrop>();
        public override int GrowDelay => 10 * 60 * 60;
        public override bool GrowCondition => true;
    }
}
