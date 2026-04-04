using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Items.Accessories.Movement;
using TranscendenceMod.Items.Accessories.Offensive.EoL;
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Items.Materials.LargeRecipes;
using TranscendenceMod.Items.Materials.MobDrops;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Items.Accessories.Defensive
{
    [AutoloadEquip(EquipType.Neck)]
    public class VampireToothNecklace : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Orange;
            Item.width = 24;
            Item.height = 24;
            Item.accessory = true;
            Item.value = Item.sellPrice(gold: 12, silver: 50);
        }

        public override bool MeleePrefix() => false;
        public override bool WeaponPrefix() => false;

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            TranscendenceUtils.DrawItemGlowmask(Item, rotation, scale, Texture);
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            ModKeybind mkb = TranscendenceWorld.InfectionAccessoryKeyBind;
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
            if (!Main.dedServ)
            {
                Main.LocalPlayer.TryGetModPlayer(out TranscendencePlayer mpr);

                if (mpr == null)
                    return;

                int amount = mpr.VampireHealAmount;
                TooltipLine line = tooltips.FirstOrDefault(x => x.Mod == "Terraria" && x.Text.Contains("(Healed Amount)"));

                if (line != null)
                    line.Text = line.Text.Replace("(Healed Amount)", amount.ToString());
            }
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<TranscendencePlayer>().Vampire = true;
        }


        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.CharmofMyths)
            .AddIngredient(ItemID.StingerNecklace)
            .AddIngredient(ModContent.ItemType<GalaxyAlloy>(), 12)
            .AddIngredient(ItemID.SoulofNight, 50)
            .AddIngredient(ItemID.FragmentNebula, 75)
            .AddIngredient(ItemID.Vertebrae, 125)
            .AddIngredient(ModContent.ItemType<LivingOrganicMatter>())
            .AddIngredient(ModContent.ItemType<VoidFragment>(), 20)
            .AddTile(TileID.LunarCraftingStation)
            .Register();
        }
    }
}
