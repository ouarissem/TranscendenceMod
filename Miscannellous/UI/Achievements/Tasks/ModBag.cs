using Microsoft.Build.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using TranscendenceMod.Items.Accessories.Shields;
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Items.Materials.MobDrops;
using TranscendenceMod.Items.Modifiers;
using TranscendenceMod.Items.Tools;
using TranscendenceMod.Items.Tools.Generic.Hardmetal;

namespace TranscendenceMod.Miscanellous.UI.Achievements.Tasks
{
    public class ModBag : TaskUIElement
    {
        public override Texture2D Icon => TextureAssets.Item[ModContent.ItemType<ModifierContainer>()].Value;
        public override TaskIDs type => TaskIDs.ModifierBag;
        public override bool Unlocked => Main.LocalPlayer.GetModPlayer<ModAchievementsHelper>().ModifierBagUnlock;

        public override float x => 75f;

        public override float y => -100f;

        public override string col => "7e3f33";

        public override CategoryIDs category => CategoryIDs.EaM;


        public override int reward => ModContent.ItemType<CarbonOre>();
        public override int amount => 20;
    }
}

