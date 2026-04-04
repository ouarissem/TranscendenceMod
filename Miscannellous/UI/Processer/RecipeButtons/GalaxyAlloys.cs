using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.UI;
using Terraria.UI;
using Terraria.UI.Chat;
using TranscendenceMod.Items.Consumables.Placeables.SpaceBiome;
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Items.Modifiers;
using TranscendenceMod.Items.Modifiers.Upgrades;
using TranscendenceMod.Items.Weapons.Melee;
using TranscendenceMod.Miscannellous.UI.Processer;

namespace TranscendenceMod.Miscannellous.UI.Processer.RecipeButtons
{
    public class GalaxyAlloys : BaseRecipeButton
    {
        public override int ItemType => ModContent.ItemType<GalaxyAlloy>();

        public override int xMod => 150;

        public override int yMod => 100;

        public override int resultAmount => ProcessRecipes.GalaxyAlloy()[16];

        public override void OnClick()
        {
            for (int i = 0; i < 16; i++)
            {
                ProcesserUI.PreviewItems[i] = ProcessRecipes.GalaxyAlloy()[i];
            }
        }
    }
}