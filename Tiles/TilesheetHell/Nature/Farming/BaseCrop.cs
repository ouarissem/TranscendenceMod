using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SteelSeries.GameSense;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.Map;
using Terraria.ModLoader;
using Terraria.ObjectData;
using TranscendenceMod.Items.Farming;
using TranscendenceMod.Items.Farming.Seeds;

namespace TranscendenceMod.Tiles.TilesheetHell.Nature.Farming
{
    public abstract class BaseCrop : ModTile
    {
        public float rotation;
        public abstract int drop { get; }

        public Color commonCol = new Color(165, 234, 112);

        // ts is so israeli oh my fucking god FUUUUUUCKKKKKKK SUCK MY DICK ASssdfgdfgdgfhn FUCK YOu MDODATILEENTIYFICKYOU<rfgyiokdzfhgtik0+okfghxcl,+¨hkmip,p
        // WHY THE FUFCK DOES IT NOT SPAWN IVE DONE THIS PREVIOUSLY WHATS THE PROBLEM NOW???????
        // FYUCVD KJDS =OSYoi0 +édrtji9drfgjiom¨pdchbn¨jidcxofbkicxfävlbkmnfcdgl'nkl,fgx'oåljntmflr~~sg<s
        // THANK FUCKING GOD FINALLY
        // FUCK

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            DustType = DustID.Grass;
            HitSound = SoundID.Dig;
            MineResist = 1;

            Main.tileFrameImportant[Type] = true;
            TileID.Sets.IgnoredInHouseScore[Type] = true;
            TileID.Sets.IgnoredByGrowingSaplings[Type] = false;
            TileID.Sets.SwaysInWindBasic[Type] = true;
        }

        public override bool CanDrop(int i, int j) => false;

        /// <summary>
        /// 0 = Baby
        /// 1 = Stem
        /// 2 = Grass
        /// 3 = Leaves
        /// 4 = Grown
        /// </summary>
        public int GetAge(int i, int j)
        {
            if (!TileEntity.TryGet(i, j, out BaseCropEntity te))
                return -1;

            return te.GrowStage;
        }
        public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY)
        {
            if (TileEntity.TryGet(i, j, out BaseCropEntity te))
                tileFrameX = (short)(18 * te.GrowStage);

        }
        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Tile crop = Main.tile[i, j];

            int x = i - crop.TileFrameX / 18;
            int y = j - crop.TileFrameY / 18;
            Vector2 pos = new Vector2(x + 12, y + 11).ToWorldCoordinates();

            rotation += MathHelper.ToRadians(1f + (float)Math.Sin(Main.GlobalTimeWrappedHourly * 3f)) * 0.5f;

            if (GetAge(i, j) >= 4)
            {
                Main.EntitySpriteDraw(TextureAssets.Cursors[3].Value,
                    pos + new Vector2(0, (float)(Math.Sin(Main.GlobalTimeWrappedHourly * 2f) * 4f)) - Main.screenPosition,
                    null, Color.White, rotation, TextureAssets.Cursors[3].Value.Size() * 0.5f,
                    0.5f, SpriteEffects.None);
            }
            return base.PreDraw(i, j, spriteBatch);
        }

        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            if (GetAge(i, j) >= 4)
                Item.NewItem(new EntitySource_TileBreak(i, j), new Vector2(i, j).ToWorldCoordinates(), drop);

            base.KillTile(i, j, ref fail, ref effectOnly, ref noItem);
        }
    }
    public abstract class BaseCropEntity : ModTileEntity
    {
        public abstract int TileID { get; }
        public int Growth;
        /// <summary>
        /// Amount of frames it takes for the tile to reach next stage of growth
        /// </summary>
        public abstract int GrowDelay { get; }
        /// <summary>
        /// 0 = Baby
        /// 1 = Stem
        /// 2 = Grass
        /// 3 = Leaves
        /// 4 = Grown
        /// </summary>
        public int GrowStage;
        public abstract bool GrowCondition { get; }
        public override bool IsTileValidForEntity(int x, int y)
        {
            Tile tile = Main.tile[x, y];
            return tile.HasTile && tile.TileType == TileID;
        }

        public override void Update()
        {
            TranscendenceWorld.AmountCrops++;

            if (GrowCondition && ++Growth > GrowDelay && GrowStage < 4)
            {
                GrowStage++;
                Growth = 0;
            }

            base.Update();
        }

        public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction, int alternate)
        {
            int place = Place(i, j);
            return place;
        }
    }
}
