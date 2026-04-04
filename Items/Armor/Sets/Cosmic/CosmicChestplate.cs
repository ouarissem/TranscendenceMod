using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Items.Consumables.Placeables.SpaceBiome;
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Miscannellous.Rarities;
using TranscendenceMod.Tiles.BigTiles.Furniture;

namespace TranscendenceMod.Items.Armor.Sets.Cosmic
{
    [AutoloadEquip(EquipType.Body)]
    public class CosmicChestplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            var EquipSlot = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Body);
            ArmorIDs.Body.Sets.HidesTopSkin[EquipSlot] = true;
            ArmorIDs.Body.Sets.HidesBottomSkin[EquipSlot] = true;
            ArmorIDs.Body.Sets.HidesArms[EquipSlot] = true;
        }
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.rare = ModContent.RarityType<CosmicRarity>();
            Item.defense = 28;
            Item.value = Item.sellPrice(gold: 22);
        }
        public override void UpdateEquip(Player player)
        {
            player.endurance += 0.18f;
            player.maxMinions += 1;
            player.whipRangeMultiplier += 0.2f;
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
            .AddIngredient(ModContent.ItemType<StarcraftedAlloy>(), 4)
            .AddIngredient(ModContent.ItemType<AetherRootItem>(), 18)
            .AddTile(ModContent.TileType<StarcraftedForge>())
            .Register();
        }
        public override Color? GetAlpha(Color lightColor) => Color.White;
    }
}
