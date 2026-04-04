using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Items.Accessories.Defensive;
using TranscendenceMod.Items.Accessories.Offensive.EoL;
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Items.Materials.LargeRecipes;
using TranscendenceMod.Items.Materials.MobDrops;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Items.Accessories.Movement
{
    [AutoloadEquip(EquipType.Wings)]
    public class CorruptedWanderingKit : ModItem
    {
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            ModKeybind mkb = TranscendenceWorld.InfectionAccessoryKeyBind;
            if (!Main.dedServ && mkb != null)
            {
                for (int i = 0; i < 3; i++)
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
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            TranscendenceUtils.DrawItemGlowmask(Item, rotation, scale, Texture);
        }
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Lime;
            Item.width = 25;
            Item.height = 30;
            Item.accessory = true;
            Item.value = Item.sellPrice(gold: 75);
            ArmorIDs.Wing.Sets.Stats[Item.wingSlot] = new WingStats(300, 12, 0.5f, true, 10);
        }

        public override void HorizontalWingSpeeds(Player player, ref float speed, ref float acceleration)
        {
            speed = 10;
            acceleration = 1f;

            if (player.controlDown && player.controlJump && player.wingTime > 0 && player.GetModPlayer<TranscendencePlayer>().Focus > 0.15f)
            {
                acceleration = 2.5f;
                speed = 18;

                player.position.Y -= player.velocity.Y;

                if (player.velocity.Y > 0.1f)
                    player.velocity.Y = 0.1f;

                else if (player.velocity.Y < -0.1f)
                    player.velocity.Y = -0.1f;

                player.GetModPlayer<TranscendencePlayer>().ExpendFocus(player, 0.15f, 5f);
            }
        }
        public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising, ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
        {
            ascentWhenFalling = 0.8f;
            ascentWhenRising = 1f;
            maxCanAscendMultiplier = 0.75f;
            maxAscentMultiplier = 1.325f;
            constantAscend = 0.2f;

            if (player.controlUp && player.controlJump && player.GetModPlayer<TranscendencePlayer>().Focus > 0.1f)
            {
                constantAscend = 1f;
                ascentWhenRising = 1.75f;
                maxAscentMultiplier = 2.5f;

                player.GetModPlayer<TranscendencePlayer>().ExpendFocus(player, 0.1f, 5f);
            }
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<TranscendencePlayer>().CorruptWanderingKit = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.LongRainbowTrailWings)
            .AddIngredient(ItemID.MasterNinjaGear)
            .AddIngredient(ModContent.ItemType<GalaxyAlloy>(), 12)
            .AddIngredient(ItemID.SoulofNight, 50)
            .AddIngredient(ItemID.FragmentVortex, 75)
            .AddIngredient(ItemID.RottenChunk, 125)
            .AddIngredient(ModContent.ItemType<LivingOrganicMatter>())
            .AddIngredient(ModContent.ItemType<VoidFragment>(), 20)
            .AddTile(TileID.LunarCraftingStation)
            .Register();
        }
    }
}
