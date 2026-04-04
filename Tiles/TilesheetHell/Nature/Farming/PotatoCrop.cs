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
    public class PotatoCrop : BaseCrop
    {
        public override int drop => ModContent.ItemType<Potato>();

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            AddMapEntry(commonCol, ModContent.GetInstance<PotatoSeed>().DisplayName);

            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.DrawYOffset = 2;

            ModTileEntity te = ModContent.GetInstance<PotatoTE>();
            TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(te.Hook_AfterPlacement, -1, 0, true);

            TileObjectData.newTile.UsesCustomCanPlace = true;
            TileObjectData.addTile(Type);
        }
        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            base.KillMultiTile(i, j, frameX, frameY);
            ModContent.GetInstance<PotatoTE>().Kill(i, j);
        }
    }
    public class PotatoTE : BaseCropEntity
    {
        public override int TileID => ModContent.TileType<PotatoCrop>();
        public override int GrowDelay => 8 * 60 * 60;
        public override bool GrowCondition => true;
    }
}
