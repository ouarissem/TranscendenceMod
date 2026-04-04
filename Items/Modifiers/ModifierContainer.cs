using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using TranscendenceMod.Miscannellous.UI.ModifierUI;

namespace TranscendenceMod.Items.Modifiers
{
    public class ModifierContainer : ModItem
    {
        public Item[] ItemsInside = new Item[28];
        public int CurSelection;

        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.reuseDelay = 60;

            Item.useStyle = ItemUseStyleID.HoldUp;

            Item.width = 16;
            Item.height = 24;

            Item.value = Item.sellPrice(gold: 25);
            Item.rare = ItemRarityID.Orange;

            ItemsInside = new Item[28];
            CurSelection = 0;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            base.ModifyTooltips(tooltips);
            for (int i = 0; i < ItemsInside.Length; i++)
            {
                Item item = ItemsInside[i];

                if (item == null || item.IsAir)
                    continue;

                string arrow = CurSelection == i ? ">" : "";
                var tt = new TooltipLine(Mod, "ModifierContainerPreview", "| " + arrow + item.Name + $" [i:TranscendenceMod/{ItemLoader.GetItem(item.type).Name}]");

                if (CurSelection == i)
                    tt.OverrideColor = Color.Gold;
                else tt.OverrideColor = Color.Gray;

                tooltips.Add(tt);
            }

            if (ItemsInside[CurSelection] != null && !ItemsInside[CurSelection].IsAir && ItemsInside[CurSelection].ModItem is BaseModifier mod)   
            {
                var blank = new TooltipLine(Mod, "Blank", "---");
                blank.OverrideColor = Color.Transparent;
                tooltips.Add(blank);

                var modifier = new TooltipLine(Mod, "Modifier Name", Language.GetTextValue($"Mods.TranscendenceMod.Messages.Tooltips.Modifiers.{mod.ModifierType}"));
                tooltips.Add(modifier);

                string plural = Language.GetTextValue("Mods.TranscendenceMod.Messages.Tooltips.Modifiers.RequiredPlural", mod.RequiredAmount, mod.RequiredItem);
                string single = Language.GetTextValue("Mods.TranscendenceMod.Messages.Tooltips.Modifiers.RequiredSingle", mod.RequiredAmount, mod.RequiredItem);

                var reqs = new TooltipLine(Mod, "Requirements", mod.RequiredAmount > 1 ? plural : single);
                tooltips.Add(reqs);
            }
        }
        public override bool? UseItem(Player player)
        {
            bool HasItem = false;
            for (int i = 0; i < 10; i++)
            {
                Item it = player.inventory[i];
                if (it != null && !it.IsAir && it == Item)
                    HasItem = true;
            }

            if (player.ItemTimeIsZero && !ModifierContainerUIDrawing.Visible && HasItem)
            {
                player.GetModPlayer<TranscendencePlayer>().ModifierContainerItem = Item;
                ModifierContainerUIDrawing.Visible = true;
                Main.CloseNPCChatOrSign();
                Main.playerInventory = true;
            }

            if (!HasItem && player.ItemAnimationJustStarted)
                DialogUI.SpawnDialog(Language.GetTextValue("Mods.TranscendenceMod.Messages.ModifierBagError"), player.Top - new Vector2(0, 32), 30, Color.Red);

            return base.UseItem(player);
        }
        public override void SaveData(TagCompound tag)
        {
            for (int i = 0; i < ItemsInside.Length; i++)
            {
                tag["ItemsInside" + i] = ItemsInside[i];
            }
            tag["CurSelection"] = CurSelection;
        }
        public override void LoadData(TagCompound tag)
        {
            for (int i = 0; i < ItemsInside.Length; i++)
            {
                ItemsInside[i] = tag.Get<Item>("ItemsInside" + i);
            }
            CurSelection = tag.Get<int>("CurSelection");
        }
    }
}