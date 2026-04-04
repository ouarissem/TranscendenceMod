using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TranscendenceMod.Tiles.TilesheetHell.Bricks
{
    public class SunkenBrickBlue : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileDungeon[Type] = true;

            Main.tileMerge[Type][TileID.BlueDungeonBrick] = true;
            Main.tileMerge[Type][TileID.Stone] = true;

            DustType = DustID.DungeonBlue;

            AddMapEntry(new Color(69, 78, 99));
            HitSound = SoundID.Tink;
            MinPick = 100;
            MineResist = 3f;
        }
    }
}
