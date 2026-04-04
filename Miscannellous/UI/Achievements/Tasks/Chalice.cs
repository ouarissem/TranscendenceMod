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
using TranscendenceMod.Items.Materials.MobDrops;
using TranscendenceMod.Items.Tools;
using TranscendenceMod.Items.Tools.Generic.Hardmetal;

namespace TranscendenceMod.Miscanellous.UI.Achievements.Tasks
{
    public class Chalice : TaskUIElement
    {
        public override Texture2D Icon => TextureAssets.Item[ModContent.ItemType<ForgottenInferno>()].Value;
        public override TaskIDs type => TaskIDs.Chalice;
        public override bool Unlocked => Main.LocalPlayer.GetModPlayer<ModAchievementsHelper>().ChaliceUnlock;

        public override float x => -100f;

        public override float y => -75f;

        public override string col => "db7a1a";

        public override CategoryIDs category => CategoryIDs.EaM;

        public override int reward => ModContent.ItemType<VolcanicRemains>();
        public override int amount => 8;
    }
}

