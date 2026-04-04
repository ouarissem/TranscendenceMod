using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.WorldBuilding;
using TranscendenceMod.Dusts;
using TranscendenceMod.Items.Accessories.Other;
using TranscendenceMod.Items.Consumables.Placeables;
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.NPCs.Miniboss;
using TranscendenceMod.Projectiles;
using static TranscendenceMod.TranscendenceWorld;

namespace TranscendenceMod.Tiles.BigTiles
{
    public class InfernoChaliceTile : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = false;
            Main.tileSolidTop[Type] = false;
            Main.tileSpelunker[Type] = true;
            Main.tileFrameImportant[Type] = true;

            TileID.Sets.CanBeClearedDuringGeneration[Type] = false;
            TileID.Sets.PreventsTileReplaceIfOnTopOfIt[Type] = true;
            TileID.Sets.PreventsTileRemovalIfOnTopOfIt[Type] = true;
            TileID.Sets.FramesOnKillWall[Type] = true;
            TileID.Sets.PreventsSandfall[Type] = true;

            DustType = ModContent.DustType<HardmetalDust>();
            AddMapEntry(new Color(255, 194, 0), CreateMapEntryName());
            RegisterItemDrop(ModContent.ItemType<ForgottenInferno>());

            HitSound = SoundID.Dig;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
            TileObjectData.addTile(Type);
        }
        public override bool CanKillTile(int i, int j, ref bool blockDamaged)
        {
            if (!Downed.Contains(Bosses.FlameGuardian))
            {
                Tile altar = Main.tile[i, j];

                int x = i - altar.TileFrameX / 18;
                int y = j - altar.TileFrameY / 18;
                Vector2 pos = new Vector2(x + 0.5f, y).ToWorldCoordinates();

                if (Main.LocalPlayer.Distance(pos) < 250)
                {
                    Main.LocalPlayer.velocity = Main.LocalPlayer.DirectionTo(pos) * -10f;

                    int fg = ModContent.NPCType<FlameGuardian>();
                    if (!NPC.AnyNPCs(fg))
                        NPC.NewNPC(Main.LocalPlayer.GetSource_TileInteraction(i, j), (int)pos.X, (int)pos.Y - 125, fg);
                }
                Projectile.NewProjectile(Main.LocalPlayer.GetSource_TileInteraction(i, j), pos, Vector2.Zero, ModContent.ProjectileType<Shockwave>(), 500, 50, -1, 1f, 0.5f, 0);

                return false;
            }

            return base.CanKillTile(i, j, ref blockDamaged);
        }
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            base.PostDraw(i, j, spriteBatch);

            TranscendenceUtils.BigTileGlowmask(i, j, Texture + "_Glow", Vector2.Zero);

        }
        public override bool CanExplode(int i, int j) => false;
    }
}
