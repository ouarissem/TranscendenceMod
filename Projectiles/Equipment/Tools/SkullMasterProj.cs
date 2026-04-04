using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Tiles.TilesheetHell.Nature.Farming;
using static TranscendenceMod.Tiles.TilesheetHell.Nature.Farming.BaseCrop;
namespace TranscendenceMod.Projectiles.Equipment.Tools
{
    public class SkullMasterProj : ModProjectile
    {
        public float Fade = 1f;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 20;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.width = 28;
            Projectile.height = 28;

            Projectile.timeLeft = 150;
            Projectile.tileCollide = false;
        }
        public override void AI()
        {
            Projectile.velocity *= 0.975f;
            Projectile.rotation += 0.075f;

            if (Projectile.timeLeft < 30)
                Fade -= 1 / 30f;

            if (Projectile.ai[2] > 0)
                Projectile.ai[2]--;

            //4x4 rectangle
            for (int i = -32; i < 32; i++)
            {
                for (int j = -32; j < 32; j++)
                {
                    Vector2 pos = Projectile.Center + new Vector2(i, j);
                    Tile tile = Framing.GetTileSafely((int)pos.X / 16, (int)pos.Y / 16);

                    if (Projectile.owner == Main.myPlayer && tile.HasTile && Main.player[Projectile.owner] != null && tile != null && Projectile.ai[2] == 0)
                    {
                        ModTile mt = ModContent.GetModTile(tile.TileType);
                        if (mt != null && mt is BaseCrop crop && tile.TileFrameX >= 72)
                        {
                            Main.player[Projectile.owner].PickTile((int)(pos.X / 16), (int)(pos.Y / 16), 155);
                            Projectile.ai[2] = 2;
                        }
                    }
                }
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;
            TranscendenceUtils.RestartSB(spriteBatch, BlendState.Additive, null);

            TranscendenceUtils.DrawTrailProj(Projectile, Color.White * Fade, Projectile.scale * Fade, Texture, false, true, 1, Vector2.Zero);

            TranscendenceUtils.RestartSB(spriteBatch, BlendState.AlphaBlend, null);
            return false;
        }
    }
}