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
using TranscendenceMod.Items.Modifiers;
using TranscendenceMod.Items.Tools;
using TranscendenceMod.Items.Tools.Generic.Hardmetal;
using TranscendenceMod.Items.Weapons.Magic;

namespace TranscendenceMod.Miscanellous.UI.Achievements.Tasks
{
    public class Muramasa : TaskUIElement
    {
        public override Texture2D Icon => TextureAssets.Item[ModContent.ItemType<WaterKunai>()].Value;
        public override TaskIDs type => TaskIDs.Muramasa;
        public override bool Unlocked => Main.LocalPlayer.GetModPlayer<ModAchievementsHelper>().MuramasaUnlock;

        public override float x => -100f;

        public override float y => 25f;

        public override string col => "0074ff";

        public override CategoryIDs category => CategoryIDs.EaM;

        public override int reward => ItemID.MagicConch;
        public override int amount => 1;
    }
}

