using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TranscendenceMod.Items.Consumables;
using TranscendenceMod.Items.Modifiers;

namespace TranscendenceMod.Items.Materials
{
    public class TerrariaDial : BaseModifier
    {
        public override bool CanBeApplied(Item item) => item.type == ModContent.ItemType<EtheriaDial>();
        public override int RequiredItem => ModContent.ItemType<ConstantDial>();
        public override int RequiredAmount => 1;
        public override ModifierIDs ModifierType => ModifierIDs.TimedialTerraria;
        public override int CraftingResultItem => ModContent.ItemType<Timedial>();

        public override Color? GetAlpha(Color lightColor) => Color.White;
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            ItemID.Sets.ItemNoGravity[Type] = true;
        }
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.width = 16;
            Item.height = 22;
            Item.value = Item.sellPrice(gold: 5);
            Item.rare = ItemRarityID.Lime;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            base.ModifyTooltips(tooltips);

            DateTime curTime = DateTime.UtcNow.AddHours(2);
            string timeString = Language.GetTextValue("Mods.TranscendenceMod.Messages.Timedials", curTime.ToLongTimeString());
            var time = new TooltipLine(Mod, "realTime", timeString);

            if (Item.type == ModContent.ItemType<TerrariaDial>())
                tooltips.Add(time);
        }
    }
}
