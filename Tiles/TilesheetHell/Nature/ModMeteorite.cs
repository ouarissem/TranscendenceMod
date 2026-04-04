using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Buffs;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Tiles.TilesheetHell.Nature
{
    public class ModMeteorite : ModTile
    {
        public override string Texture => $"Terraria/Images/Tiles_37";
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileSpelunker[Type] = true;
            DustType = DustID.Meteorite;

            RegisterItemDrop(ItemID.Meteorite);
            AddMapEntry(new Color(123, 76, 55));
            HitSound = SoundID.Tink;
            MinPick = 110;
            MineResist = 1f;
        }
        public override void NearbyEffects(int i, int j, bool closer)
        {
            int chance = 300;
            int dmg = Main.expertMode ? 18 : 28;

            if (closer && !Main.tile[i, j - 1].HasTile && !TranscendenceUtils.BossAlive() && Main.rand.NextBool(chance) && !Main.gameInactive && !Main.gamePaused)
            {
                Projectile.NewProjectile(new EntitySource_TileUpdate(i, j), new Vector2(i, j) * 16, new Vector2(Main.rand.NextFloat(-2f, 2f), -Main.rand.NextFloat(4f, 8f)), ProjectileID.GreekFire1, dmg, 2, -1);
            }
        }
        public override void FloorVisuals(Player player)
        {
            player.AddBuff(BuffID.Burning, 5);
            player.AddBuff(ModContent.BuffType<SpaceDebuff>(), 5);
        }
    }
}
