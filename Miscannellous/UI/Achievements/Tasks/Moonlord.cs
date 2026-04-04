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
using TranscendenceMod.Items.Consumables.Placeables;
using TranscendenceMod.Items.Materials;

namespace TranscendenceMod.Miscanellous.UI.Achievements.Tasks
{
    public class Moonlord : TaskUIElement
    {
        public override Texture2D Icon => TextureAssets.NpcHeadBoss[8].Value;
        public override TaskIDs type => TaskIDs.Moonlord;
        public override bool Unlocked => Main.LocalPlayer.GetModPlayer<ModAchievementsHelper>().MoonlordUnlock;

        public override float x => 0f;

        public override float y => 75f;

        public override string col => "43c3ac";

        public override CategoryIDs category => CategoryIDs.Prog;


        public override int reward => ItemID.LunarBar;
        public override int amount => 8;
    }
}

