using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using TranscendenceMod.Items.Farming;
using TranscendenceMod.Items.Farming.Seeds;

namespace TranscendenceMod.Tiles.TilesheetHell.Nature.Farming
{
    public class CocoaCrop : BaseCrop
    {
        public override int drop => ModContent.ItemType<CocoaBean>();

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            AddMapEntry(commonCol, ModContent.GetInstance<CocoaBeanSeeds>().DisplayName);

            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.DrawYOffset = 2;

            ModTileEntity te = ModContent.GetInstance<CocoaTE>();
            TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(te.Hook_AfterPlacement, -1, 0, true);

            TileObjectData.newTile.UsesCustomCanPlace = true;
            TileObjectData.addTile(Type);
        }
        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            base.KillMultiTile(i, j, frameX, frameY);
            ModContent.GetInstance<CocoaTE>().Kill(i, j);
        }
    }
    public class CocoaTE : BaseCropEntity
    {
        public override int TileID => ModContent.TileType<CocoaCrop>();
        public override int GrowDelay => 12 * 60 * 60;
        public override bool GrowCondition => true;
    }
}
