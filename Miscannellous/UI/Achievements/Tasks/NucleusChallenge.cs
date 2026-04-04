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
using TranscendenceMod.Items.Consumables.Placeables;
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Items.Tools.Generic.Hardmetal;

namespace TranscendenceMod.Miscanellous.UI.Achievements.Tasks
{
    public class NucleusChallenge : TaskUIElement
    {
        public override Texture2D Icon => TextureAssets.Item[ModContent.ItemType<ArcadeCabinetItem>()].Value;
        public override TaskIDs type => TaskIDs.NucleusChallenge;
        public override bool Unlocked => Main.LocalPlayer.GetModPlayer<ModAchievementsHelper>().NucleusChallengeUnlock;

        public override float x => -175f;

        public override float y => -35f;

        public override string col => "ff4127";

        public override CategoryIDs category => CategoryIDs.Challenge;

        public override int reward => ItemID.PlatinumCoin;
        public override int amount => 1;
    }
}

