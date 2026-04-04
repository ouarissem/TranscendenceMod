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
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Items.Materials.MobDrops;
using TranscendenceMod.Items.Tools;
using TranscendenceMod.Items.Weapons.Melee;

namespace TranscendenceMod.Miscanellous.UI.Achievements.Tasks
{
    public class Poseidon : TaskUIElement
    {
        public override Texture2D Icon => TextureAssets.Item[ModContent.ItemType<PoseidonsTide>()].Value;
        public override TaskIDs type => TaskIDs.PoseidonFrag;
        public override bool Unlocked => Main.LocalPlayer.GetModPlayer<ModAchievementsHelper>().PoseidonUnlock;

        public override float x => -25f;

        public override float y => 150f;

        public override string col => "0074ff";

        public override CategoryIDs category => CategoryIDs.EaM;

        public override int reward => ItemID.BottomlessBucket;
        public override int amount => 1;
    }
}

