using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TranscendenceMod.Tiles.TilesheetHell.Bricks
{
    public class SunkenBrickGreen : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileDungeon[Type] = true;

            Main.tileMerge[Type][TileID.GreenDungeonBrick] = true;
            Main.tileMerge[Type][TileID.Stone] = true;

            DustType = DustID.DungeonGreen;

            AddMapEntry(new Color(82, 101, 86));
            HitSound = SoundID.Tink;
            MinPick = 100;
            MineResist = 3f;
        }
    }
}
