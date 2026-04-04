using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Items.Consumables;
using TranscendenceMod.Tiles;
using TranscendenceMod.Tiles.BigTiles;

namespace TranscendenceMod.Items.Materials
{
    public class ForgottenInferno : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 14;
            Item.height = 16;
            Item.value = Item.sellPrice(gold: 3, silver: 75);
            Item.rare = ItemRarityID.LightRed;

            Item.useAnimation = 15;
            Item.useTime = 15;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<InfernoChaliceTile>();
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            tooltips.FirstOrDefault(x => x.Name == "ItemName").OverrideColor = Color.Lerp(Color.OrangeRed, Color.DarkSlateBlue, Main.cursorAlpha);
        }
    }
}
