using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TranscendenceMod.Tiles.TilesheetHell.Bricks
{
    public class SunkenBrickPink : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileDungeon[Type] = true;

            Main.tileMerge[Type][TileID.PinkDungeonBrick] = true;
            Main.tileMerge[Type][TileID.Stone] = true;

            DustType = DustID.DungeonPink;

            AddMapEntry(new Color(113, 55, 85));
            HitSound = SoundID.Tink;
            MinPick = 100;
            MineResist = 3f;
        }
    }
}
