using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using TranscendenceMod.Dusts;

namespace TranscendenceMod.Tiles
{
    public class UnbreakableBlockTile : ModTile
    {
        public override string Texture => "TranscendenceMod/Items/Consumables/Placeables/UnbreakableBlock";
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;
            DustType = ModContent.DustType<StarcraftedDust>();

            Main.tileShine[Type] = 1000;
            Main.tileFrameImportant[Type] = true;

            AddMapEntry(new Color(97, 113, 167));

            HitSound = SoundID.Tink;
            MinPick = 9999;
            MineResist = 99f;
        }
        public override bool RightClick(int i, int j)
        {
            WorldGen.KillTile(i, j);
            return base.RightClick(i, j);
        }
    }
}
