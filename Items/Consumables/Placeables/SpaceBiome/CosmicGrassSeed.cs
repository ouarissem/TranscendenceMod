using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous.Rarities;
using TranscendenceMod.Tiles.TilesheetHell.Nature;

namespace TranscendenceMod.Items.Consumables.Placeables.SpaceBiome
{
    public class CosmicGrassSeed : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
        }
        public override void SetDefaults()
        {
            Item.width = 15;
            Item.height = 15;
            Item.rare = ModContent.RarityType<ModdedPurple>();
            Item.maxStack = 9999;
            Item.useAnimation = 10;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.consumable = true;
            Item.autoReuse = true;
            Item.value = Item.sellPrice(silver: 15);
        }
        public override bool? UseItem(Player player)
        {
            Tile tile = Framing.GetTileSafely(Player.tileTargetX, Player.tileTargetY);
            if (tile.TileType == ModContent.TileType<SpaceRock>() && tile.HasTile && (player.Distance(Main.MouseWorld) < ((7 + Item.tileBoost) * 16)))
            {
                Main.tile[Player.tileTargetX, Player.tileTargetY].TileType = (ushort)ModContent.TileType<SpaceRockGrass>();
                SoundEngine.PlaySound(SoundID.Tink);
                return true;
            }
            else return false;
        }
    }
}
