using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Dusts;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.NPCs.Boss.Seraph;
using TranscendenceMod.Projectiles;

namespace TranscendenceMod.Tiles.TilesheetHell.Nature
{
    public class SpaceCrystal : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileShine[Type] = 40;
            Main.tileSpelunker[Type] = true;

            DustType = ModContent.DustType<SpaceCrystalDust>();
            AddMapEntry(new Color(96, 9, 108));
            HitSound = SoundID.Shatter;
            MinPick = 110;
            MineResist = 5f;
        }

        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Main.tileNoSunLight[Type] = false;
            TileID.Sets.DrawsWalls[Type] = true;

            Texture2D sprite = ModContent.Request<Texture2D>(Texture).Value;

            Vector2 offScreen = new Vector2(Main.offScreenRange);
            if (Main.drawToScreen)
                offScreen = Vector2.Zero;

            Tile safe = Framing.GetTileSafely(i, j);
            int frameSizeY = safe.TileFrameY == 36 ? 18 : 16;

            int x = (int)((i * 16) - Main.screenPosition.X);
            int y = (int)((j * 16) - Main.screenPosition.Y);
            float opacity = (float)(0.5f + Math.Sin(Main.GlobalTimeWrappedHourly * 3) * 0.175f);

            if (safe.BlockType == BlockType.Solid)
            {
                Main.spriteBatch.Draw(sprite, new Vector2(x, y) + offScreen, new Rectangle(safe.TileFrameX, safe.TileFrameY, 16, frameSizeY), Color.White * opacity, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                return false;
            }
            else return base.PreDraw(i, j, spriteBatch);
        }

        public override void NearbyEffects(int i, int j, bool closer)
        {
            int chance = 500;

            if (closer && !NPC.AnyNPCs(ModContent.NPCType<CelestialSeraph>()) && Main.rand.NextBool(chance) && !Main.gameInactive && !Main.gamePaused)
            {
                Projectile.NewProjectile(new EntitySource_TileUpdate(i, j), new Vector2(i, j) * 16,
                    Vector2.Zero, ModContent.ProjectileType<CrystalRadiationCloud>(), 0, 0f, -1);
            }
        }

        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            Item item = player.GetBestPickaxe();

            if (item == null || item.pick < 110)
            {
                player.cursorItemIconEnabled = true;
                player.cursorItemIconID = Math.Sin(Main.GlobalTimeWrappedHourly * 4f) > 0 ? ItemID.PalladiumPickaxe : ItemID.CobaltPickaxe;
            }
        }
    }
}
