using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Security.Policy;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TranscendenceMod.Buffs.Items.Potions;
using TranscendenceMod.Items.Armor.Hats;
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Items.Materials.Fish;
using static System.Net.Mime.MediaTypeNames;

namespace TranscendenceMod.Items.Consumables.FoodAndDrinks
{
    public class BottomlessPotion : ModItem
    {
        public enum BuffTypes
        {
            Wrath, Rage, Endurance, Summoning, Inferno
        }
        public BuffTypes SelectedBuff = BuffTypes.Wrath;
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.DrinkLiquid;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.UseSound = SoundID.Item3;

            Item.width = 16;
            Item.height = 16;
            Item.value = Item.sellPrice(gold: 2, silver: 75);
            Item.rare = ItemRarityID.LightPurple;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            base.ModifyTooltips(tooltips);

            string text = "";
            switch (SelectedBuff)
            {
                case BuffTypes.Wrath: text = Lang.GetBuffName(BuffID.Wrath) + " [i:WrathPotion]"; break;
                case BuffTypes.Rage: text = Lang.GetBuffName(BuffID.Rage) + " [i:RagePotion]"; break;
                case BuffTypes.Endurance: text = Lang.GetBuffName(BuffID.Endurance) + " [i:EndurancePotion]"; break;
                case BuffTypes.Summoning: text = Lang.GetBuffName(BuffID.Summoning) + " [i:SummoningPotion]"; break;
                case BuffTypes.Inferno: text = Lang.GetBuffName(BuffID.Inferno) + " [i:InfernoPotion]"; break;
            }

            var tt = new TooltipLine(Mod, "BotPotSelect", Language.GetTextValue("Mods.TranscendenceMod.Messages.Select") + " " + text);
            tooltips.Add(tt);
        }

        public override bool CanRightClick() => true;
        public override void RightClick(Player player)
        {
            base.RightClick(player);

            // Cycle through the avaivable buffs
            SelectedBuff += 1;
            if ((int)SelectedBuff > 4)
                SelectedBuff = 0;

        }
        public override bool ConsumeItem(Player player) => false;
        public int cd => ModContent.BuffType<BottomlessPotSickness>();
        public override bool CanUseItem(Player player) => !player.HasBuff(cd);
        public override bool? UseItem(Player player)
        {
            int duration = 5 * 60 * 60;

            if (player.ItemAnimationJustStarted)
            {
                switch (SelectedBuff)
                {
                    case BuffTypes.Inferno:
                        player.AddBuff(BuffID.Inferno, duration); break;

                    case BuffTypes.Summoning:
                        player.AddBuff(BuffID.Summoning, duration); break;

                    case BuffTypes.Endurance:
                        player.AddBuff(BuffID.Endurance, duration); break;

                    case BuffTypes.Rage:
                        player.AddBuff(BuffID.Rage, duration); break;

                    case BuffTypes.Wrath:
                        player.AddBuff(BuffID.Wrath, duration); break;
                }

                player.AddBuff(cd, duration * 2);
            }


            return base.UseItem(player);
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.BottledWater)
            .AddIngredient(ModContent.ItemType<FlowerEssence>(), 5)
            .AddIngredient(ModContent.ItemType<BlackholeFish>(), 4)
            .AddTile(TileID.Bottles)
            .Register();
        }
    }
}
