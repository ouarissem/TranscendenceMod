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
using TranscendenceMod.Items.Tools.Generic.Hardmetal;
using TranscendenceMod.Items.Weapons.Magic;

namespace TranscendenceMod.Miscanellous.UI.Achievements.Tasks
{
    public class Empress : TaskUIElement
    {
        public override Texture2D Icon => TextureAssets.NpcHeadBoss[37].Value;
        public override TaskIDs type => TaskIDs.Empress;
        public override bool Unlocked => Main.LocalPlayer.GetModPlayer<ModAchievementsHelper>().EmpressUnlock;

        public override float x => -25f;

        public override float y => 50f;

        public override string col => "fed059";

        public override CategoryIDs category => CategoryIDs.Prog;

        public override int reward => ItemID.SoulofLight;
        public override int amount => 25;
    }
}

