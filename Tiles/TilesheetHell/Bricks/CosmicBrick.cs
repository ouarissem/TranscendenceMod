using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Tiles.TilesheetHell.Nature;

namespace TranscendenceMod.Tiles.TilesheetHell.Bricks
{
    public class CosmicBrick : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;

            Main.tileMerge[Type][ModContent.TileType<SpaceRock>()] = true;
            Main.tileMerge[Type][ModContent.TileType<SpaceRockGrass>()] = true;

            DustType = DustID.Lead;

            AddMapEntry(new Color(46, 42, 79));
            HitSound = SoundID.Tink;
            MinPick = 50;
            MineResist = 2f;
        }
    }
}
