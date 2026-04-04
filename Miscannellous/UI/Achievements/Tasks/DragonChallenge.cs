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
using TranscendenceMod.Items.Accessories.Movement;
using TranscendenceMod.Items.Accessories.Offensive;
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Items.Tools.Generic.Hardmetal;
using TranscendenceMod.Items.Weapons.Ranged.Ammo;

namespace TranscendenceMod.Miscanellous.UI.Achievements.Tasks
{
    public class DragonChallenge : TaskUIElement
    {
        public override Texture2D Icon => TextureAssets.Item[ItemID.Blindfold].Value;
        public override TaskIDs type => TaskIDs.DragonChallenge;
        public override bool Unlocked => Main.LocalPlayer.GetModPlayer<ModAchievementsHelper>().DragonChalUnlock;

        public override float x => -175f;

        public override float y => 0f;

        public override string col => "484c6d";

        public override CategoryIDs category => CategoryIDs.Challenge;

        public override int reward => ItemID.PlatinumCoin;
        public override int amount => 1;
    }
}

