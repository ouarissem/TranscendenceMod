using Microsoft.Build.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using TranscendenceMod.Items.Accessories.Shields;
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Items.Materials.MobDrops;
using TranscendenceMod.Items.Modifiers;
using TranscendenceMod.Items.Modifiers.Upgrades;
using TranscendenceMod.Items.Tools;
using TranscendenceMod.Items.Tools.Generic.Hardmetal;

namespace TranscendenceMod.Miscanellous.UI.Achievements.Tasks
{
    public class Modifier : TaskUIElement
    {
        public override Texture2D Icon => ModContent.Request<Texture2D>("TranscendenceMod/NPCs/Passive/Tinkerer_Head").Value;
        public override TaskIDs type => TaskIDs.Modifier;
        public override bool Unlocked => Main.LocalPlayer.GetModPlayer<ModAchievementsHelper>().ModifierUnlock;

        public override float x => 50f;

        public override float y => -75f;

        public override string col => "ffcd00";

        public override CategoryIDs category => CategoryIDs.EaM;


        public override int reward => ModContent.ItemType<AbsolutePurity>();
        public override int amount => 4;
    }
}

