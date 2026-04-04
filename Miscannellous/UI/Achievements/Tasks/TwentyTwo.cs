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
using TranscendenceMod.Items.Accessories.Offensive;
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Items.Tools;
using TranscendenceMod.Items.Tools.Generic.Hardmetal;
using TranscendenceMod.Items.Weapons.Ranged.Ammo;

namespace TranscendenceMod.Miscanellous.UI.Achievements.Tasks
{
    public class TwentyTwo : TaskUIElement
    {
        public override Texture2D Icon => TextureAssets.Item[ItemID.GrayCockatiel].Value;
        public override TaskIDs type => TaskIDs.TwentyTwoChallenge;
        public override bool Unlocked => Main.LocalPlayer.GetModPlayer<ModAchievementsHelper>().TwentyTwoUnlock;

        public override float x => -175f;

        public override float y => 35f;

        public override string col => "eda453";

        public override CategoryIDs category => CategoryIDs.Challenge;

        public override int reward => ItemID.GoldCoin;
        public override int amount => 25;
    }
}

