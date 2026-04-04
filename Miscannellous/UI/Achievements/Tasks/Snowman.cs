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
using TranscendenceMod.Items.Accessories.Shields;
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Items.Materials.MobDrops;
using TranscendenceMod.Items.Tools;
using TranscendenceMod.Items.Tools.Generic.Hardmetal;

namespace TranscendenceMod.Miscanellous.UI.Achievements.Tasks
{
    public class Snowman : TaskUIElement
    {
        public override Texture2D Icon => ModContent.Request<Texture2D>("TranscendenceMod/NPCs/Passive/SnowmanNPC_Head").Value;
        public override TaskIDs type => TaskIDs.Snowman;
        public override bool Unlocked => Main.LocalPlayer.GetModPlayer<ModAchievementsHelper>().SnowmanUnlock;

        public override float x => 25f;

        public override float y => -50f;

        public override string col => "2764ba";

        public override CategoryIDs category => CategoryIDs.Misc;

        public override int reward => ModContent.ItemType<FishPendant>();
        public override int amount => 1;
    }
}

