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
using TranscendenceMod.Items.Tools.Generic.Hardmetal;

namespace TranscendenceMod.Miscanellous.UI.Achievements.Tasks
{
    public class EmpressChallenge : TaskUIElement
    {
        public override Texture2D Icon => TextureAssets.Item[ItemID.EmpressButterfly].Value;
        public override TaskIDs type => TaskIDs.EmpressChallenge;
        public override bool Unlocked => Main.LocalPlayer.GetModPlayer<ModAchievementsHelper>().EoLChallengeUnlock;

        public override float x => -175f;

        public override float y => -70f;

        public override string col => "ee60c1";

        public override CategoryIDs category => CategoryIDs.Challenge;

        public override int reward => ItemID.PlatinumCoin;
        public override int amount => 1;
    }
}

