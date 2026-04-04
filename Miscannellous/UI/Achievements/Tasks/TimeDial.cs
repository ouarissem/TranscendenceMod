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
using TranscendenceMod.Items.Consumables;
using TranscendenceMod.Items.Consumables.LootBags;
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Items.Materials.MobDrops;
using TranscendenceMod.Items.Tools;
using TranscendenceMod.Items.Tools.Generic.Hardmetal;

namespace TranscendenceMod.Miscanellous.UI.Achievements.Tasks
{
    public class TimeDial : TaskUIElement
    {
        public override Texture2D Icon => TextureAssets.Item[ModContent.ItemType<Timedial>()].Value;
        public override TaskIDs type => TaskIDs.Timedial;
        public override bool Unlocked => Main.LocalPlayer.GetModPlayer<ModAchievementsHelper>().TimedialUnlock;

        public override float x => 100f;

        public override float y => -25f;

        public override string col => "476c97";

        public override CategoryIDs category => CategoryIDs.Misc;

        public override int reward => ModContent.ItemType<ExtraTerrestrialReward>();
        public override int amount => 3;
    }
}

