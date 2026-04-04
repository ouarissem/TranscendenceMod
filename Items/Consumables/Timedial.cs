using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace TranscendenceMod.Items.Consumables
{
    public class Timedial : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.height = 25;
            Item.width = 20;
            Item.value = Item.sellPrice(gold: 50);
            Item.rare = ItemRarityID.Quest;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            tooltips.FirstOrDefault(x => x.Name == "ItemName").OverrideColor = Color.Lerp(Color.Gold, Color.MidnightBlue * 2.25f, Main.cursorAlpha * 0.8f);
        }
    }
}
