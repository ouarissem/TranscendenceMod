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
    public class Hardmetal : TaskUIElement
    {
        public override Texture2D Icon => TextureAssets.Item[ModContent.ItemType<HardmetalOre>()].Value;
        public override TaskIDs type => TaskIDs.Hardmetal;
        public override bool Unlocked => Main.LocalPlayer.GetModPlayer<ModAchievementsHelper>().HardmetalUnlock;

        public override float x => -50f;

        public override float y => -75f;

        public override string col => "4674a3";

        public override CategoryIDs category => CategoryIDs.EaM;

        public override int reward => ItemID.Leather;
        public override int amount => 4;
    }
}

