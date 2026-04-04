using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Tiles.TilesheetHell.Nature.Farming;

namespace TranscendenceMod.Items.Farming
{
    public abstract class BaseHoe : ModItem
    {
        public abstract ushort soil { get; }
        public abstract int range { get; }

        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Green;
            Item.width = 18;
            Item.height = 18;
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = Item.sellPrice(gold: 5);
            Item.autoReuse = true;

            Item.shoot = ProjectileID.PurificationPowder;
            Item.shootSpeed = 10;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 pos = Main.MouseWorld;
            pos /= 16;
            Tile tile = Main.tile[(int)pos.X, (int)pos.Y];

            bool soilBool = false;
            if (soil != ModContent.TileType<Soil>())
                soilBool = tile.TileType == (ushort)ModContent.TileType<Soil>();

            if ((tile.TileType == TileID.Dirt || tile.TileType == TileID.Grass || tile.TileType == TileID.Mud || soilBool) && tile.HasTile && player.Distance(Main.MouseWorld) < (range * 16))
            {
                WorldGen.PlaceTile((int)pos.X, (int)pos.Y, soil, false, true);
                WorldGen.SquareTileFrame((int)pos.X, (int)pos.Y);
            }
            return false;
        }
    }
}
