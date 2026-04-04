using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TranscendenceMod.Items.Consumables.Placeables.SpaceBiome;
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Miscannellous.Rarities;
using TranscendenceMod.Tiles.BigTiles.Furniture;

namespace TranscendenceMod.Items.Armor.Sets.Cosmic
{
    [AutoloadEquip(EquipType.Head)]
    public class CosmicHelm : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            var EquipSlot = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Head);
            ArmorIDs.Head.Sets.DrawHead[EquipSlot] = false;
        }
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.rare = ModContent.RarityType<CosmicRarity>();
            Item.defense = 10;
            Item.value = Item.sellPrice(gold: 18);
        }
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<CosmicChestplate>() && legs.type == ModContent.ItemType<CosmicBoots>();
        }
        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Melee) += 0.15f;
            player.GetCritChance(DamageClass.Generic) += 0.15f;
            player.statManaMax2 += 175;
        }
        public override void UpdateArmorSet(Player player)
        {
            player.GetModPlayer<TranscendencePlayer>().CosmicSetWear = true;
            player.GetDamage(DamageClass.Generic) += 0.2f;
            player.GetCritChance(DamageClass.Generic) += 0.2f;
            player.maxMinions += 2;
            player.setBonus = Language.GetTextValue("Mods.TranscendenceMod.Messages.Tooltips.ArmorSetBonuses.Cosmic");
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            base.ModifyTooltips(tooltips);

            ModKeybind mkb = TranscendenceWorld.ArmorSetBonus;
            if (!Main.dedServ && mkb != null)
            {
                List<string> keys = mkb.GetAssignedKeys();

                if (keys.Count > 0)
                {
                    StringBuilder sb = new StringBuilder(10);
                    sb.Append(keys[0]);

                    TooltipLine line = tooltips.FirstOrDefault(x => x.Mod == "Terraria" && x.Text.Contains("(Unbound Key)"));
                    if (line != null)
                        line.Text = line.Text.Replace("(Unbound Key)", sb.ToString());
                }
            }
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<StarcraftedAlloy>(), 3)
            .AddIngredient(ModContent.ItemType<AetherRootItem>(), 10)
            .AddIngredient(ItemID.BlueStarryGlassBlock, 12)
            .AddTile(ModContent.TileType<StarcraftedForge>())
            .Register();
        }
        public override Color? GetAlpha(Color lightColor) => Color.White;
    }
}
