using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using TranscendenceMod.Dusts;
using TranscendenceMod.Items.Consumables.Placeables;
using TranscendenceMod.Miscanellous.MiscSystems;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.NPCs.Boss.Nucleus;

namespace TranscendenceMod.Tiles.BigTiles
{
    public class ArcadeCabinet : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = false;
            Main.tileSolidTop[Type] = false;
            Main.tileSpelunker[Type] = true;
            Main.tileFrameImportant[Type] = true;

            TileID.Sets.HasOutlines[Type] = true;
            TileID.Sets.CanBeClearedDuringGeneration[Type] = false;
            TileID.Sets.PreventsTileReplaceIfOnTopOfIt[Type] = true;
            TileID.Sets.PreventsTileRemovalIfOnTopOfIt[Type] = true;
            TileID.Sets.FramesOnKillWall[Type] = true;
            TileID.Sets.PreventsSandfall[Type] = true;

            DustType = ModContent.DustType<NucleusBlood>();
            AddMapEntry(new Color(77, 98, 113), CreateMapEntryName());

            HitSound = SoundID.Dig;
            MineResist = 3f;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
            TileObjectData.newTile.Height = 3;
            TileObjectData.newTile.Origin = new Point16(1, 2);
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16 };
            TileObjectData.addTile(Type);
        }
        public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings) => true;
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            base.PostDraw(i, j, spriteBatch);

            TranscendenceUtils.BigTileGlowmask(i, j, Texture + "_Glow", Vector2.Zero);
        }
        public override bool RightClick(int i, int j)
        {
            Tile altar = Main.tile[i, j];
            Player local = Main.LocalPlayer;

            int price = Item.buyPrice(gold: 10);
            if (local.CanAfford(price))
            {
                local.BuyItem(price);
                local.GetModPlayer<NucleusGame>().Active = true;
            }

            return base.RightClick(i, j);
        }
        public override bool CanExplode(int i, int j) => false;
        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            player.cursorItemIconEnabled = true;

            player.cursorItemIconID = ItemID.GoldCoin;
        }
    }
}
