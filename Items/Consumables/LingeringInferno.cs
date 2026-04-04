using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Items.Materials.MobDrops;

namespace TranscendenceMod.Items.Consumables
{
    public class LingeringInferno : ModItem
    {
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(15);
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.ManaCrystal);
            Item.value = Item.sellPrice(gold: 25);
            Item.rare = ItemRarityID.LightRed;
            Item.useStyle = ItemUseStyleID.EatFood;
            Item.UseSound = SoundID.Item2;
            Item.maxStack = 1;

            Item.width = 14;
            Item.height = 28;
        }
        public override bool? UseItem(Player player)
        {
            if (player.GetModPlayer<TranscendencePlayer>().ConsumedInferno)
                return null;

            else if (player.itemAnimation == player.itemAnimationMax) {
                player.GetModPlayer<TranscendencePlayer>().ConsumedInferno = true;
            }

            return true;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            tooltips.FirstOrDefault(x => x.Name == "ItemName").OverrideColor = Color.Lerp(Color.OrangeRed, Color.DarkSlateBlue, Main.cursorAlpha);
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<ForgottenInferno>())
            .AddIngredient(ModContent.ItemType<VolcanicRemains>(), 12)
            .AddIngredient(ItemID.Torch, 20000)
            .AddTile(TileID.Hellforge)
            .Register();
        }
    }
}
