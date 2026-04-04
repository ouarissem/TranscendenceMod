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
    public class CarrotCrop : BaseCrop
    {
        public override int drop => ModContent.ItemType<Carrot>();

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            AddMapEntry(commonCol, ModContent.GetInstance<CarrotSeeds>().DisplayName);

            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.DrawYOffset = 2;

            ModTileEntity te = ModContent.GetInstance<CarrotTE>();
            TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(te.Hook_AfterPlacement, -1, 0, true);

            TileObjectData.newTile.UsesCustomCanPlace = true;
            TileObjectData.addTile(Type);
        }
        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            base.KillMultiTile(i, j, frameX, frameY);
            ModContent.GetInstance<CarrotTE>().Kill(i, j);
        }
    }
    public class CarrotTE : BaseCropEntity
    {
        public override int TileID => ModContent.TileType<CarrotCrop>();
        public override int GrowDelay => 10 * 60 * 60;
        public override bool GrowCondition => true;
    }
}
