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
using TranscendenceMod.Items.Accessories.Shields;
using TranscendenceMod.Items.Materials.MobDrops;
using TranscendenceMod.Items.Tools;
using TranscendenceMod.Items.Tools.Generic.Hardmetal;

namespace TranscendenceMod.Miscanellous.UI.Achievements.Tasks
{
    public class Parry : TaskUIElement
    {
        public override Texture2D Icon => ModContent.Request<Texture2D>("TranscendenceMod/Buffs/ShieldBreak").Value;
        public override TaskIDs type => TaskIDs.Parry;
        public override bool Unlocked => Main.LocalPlayer.GetModPlayer<ModAchievementsHelper>().ParryUnlock;

        public override float x => -25f;

        public override float y => -50f;

        public override string col => "00ff41";

        public override CategoryIDs category => CategoryIDs.EaM;

        public override int reward => ModContent.ItemType<FlailingHook>();
        public override int amount => 1;
    }
}

