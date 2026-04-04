using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using TranscendenceMod.Items.Accessories.Other;

namespace TranscendenceMod.Tiles
{
    public class ProtectorStoneTile : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;
            DustType = DustID.GemRuby;

            RegisterItemDrop(ModContent.ItemType<EvasionStone>());
            AddMapEntry(new Color(122, 20, 25));
            HitSound = SoundID.Tink;

            MinPick = 65;
            MineResist = 2f;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.addTile(Type);
        }
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            int d = Dust.NewDust(new Vector2(i * 16 + 3, j * 16 + 3), 1, 1, DustID.GemRuby, 0, 0, 0, default, 1);
            Dust f = Dust.NewDustPerfect(new Vector2(i * 16 + 3 - 9, j * 16 + 6), DustID.TheDestroyer, new Vector2(-2.5f, -5), 0, default, 0.75f);
            f.noGravity = true;
            Dust e = Dust.NewDustPerfect(new Vector2(i * 16 + 3 + 16, j * 16 + 6), DustID.TheDestroyer, new Vector2(2.5f, 5), 0, default, 0.75f);
            e.noGravity = true;

            Dust dust = Main.dust[d];
            dust.noGravity = true;
            dust.velocity = Vector2.Zero;
        }
    }
}
