using Microsoft.Build.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using TranscendenceMod.Items.Farming;
using TranscendenceMod.Items.Materials.MobDrops;
using TranscendenceMod.Items.Tools;
using TranscendenceMod.Items.Tools.Generic.Hardmetal;

namespace TranscendenceMod.Miscanellous.UI.Achievements.Tasks
{
    public class sans : TaskUIElement
    {
        public override Texture2D Icon => TextureAssets.NpcHeadBoss[19].Value;
        public override TaskIDs type => TaskIDs.sans;
        public override bool Unlocked => Main.LocalPlayer.GetModPlayer<ModAchievementsHelper>().sansUnlock;

        public override float x => -75f;

        public override float y => 0f;

        public override string col => "d5b78a";

        public override CategoryIDs category => CategoryIDs.Prog;

        public override int reward => ModContent.ItemType<SeedBox>();
        public override int amount => 3;
    }
}

