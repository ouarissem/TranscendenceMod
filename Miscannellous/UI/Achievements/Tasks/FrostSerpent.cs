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
using TranscendenceMod.Items.Consumables.Boss;
using TranscendenceMod.Items.Consumables.Placeables;

namespace TranscendenceMod.Miscanellous.UI.Achievements.Tasks
{
    public class FrostSerpent : TaskUIElement
    {
        public override Texture2D Icon => ModContent.Request<Texture2D>("TranscendenceMod/NPCs/Boss/FrostSerpent/FrostSerpent_Head_Head_Boss").Value;
        public override TaskIDs type => TaskIDs.FrostSerpent;
        public override bool Unlocked => Main.LocalPlayer.GetModPlayer<ModAchievementsHelper>().FrostSerpentUnlock;

        public override float x => -25f;

        public override float y => 100f;

        public override string col => "2764ba";

        public override CategoryIDs category => CategoryIDs.EaM;

        public override int reward => ItemID.LunarBar;
        public override int amount => 12;
    }
}

