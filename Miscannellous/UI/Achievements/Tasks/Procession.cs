using Microsoft.Build.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Items.Tools;
using TranscendenceMod.Items.Tools.Generic.Hardmetal;

namespace TranscendenceMod.Miscanellous.UI.Achievements.Tasks
{
    public class Procession : TaskUIElement
    {
        public override Texture2D Icon => TextureAssets.Item[ModContent.ItemType<SunburntAlloy>()].Value;
        public override TaskIDs type => TaskIDs.Procession;
        public override bool Unlocked => Main.LocalPlayer.GetModPlayer<ModAchievementsHelper>().ProcessUnlock;

        public override float x => -100f;

        public override float y => -25f;

        public override string col => "927062";

        public override CategoryIDs category => CategoryIDs.Prog;

        public override int reward => ModContent.ItemType<ScavengerChisel>();
        public override int amount => 2;
    }
}

