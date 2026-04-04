using Microsoft.Build.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using TranscendenceMod.Items.Accessories.Movement;
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Items.Materials.MobDrops;
using TranscendenceMod.Items.Tools;
using TranscendenceMod.Items.Tools.Generic.Hardmetal;

namespace TranscendenceMod.Miscanellous.UI.Achievements.Tasks
{
    public class Begin : TaskUIElement
    {
        public override Texture2D Icon => ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/UI/Achievements/AchievementBook").Value;
        public override TaskIDs type => TaskIDs.Begin;
        public override bool Unlocked => Main.LocalPlayer.GetModPlayer<ModAchievementsHelper>().BeginUnlock;

        public override float x => 0f;

        public override float y => -75f;

        public override string col => "4ad09c";

        public override CategoryIDs category => CategoryIDs.Prog;

        public override int reward => ModContent.ItemType<VineGun>();
        public override int amount => 1;
    }
}

