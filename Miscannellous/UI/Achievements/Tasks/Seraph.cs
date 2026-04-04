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
using TranscendenceMod.Items.Consumables.Placeables.SpaceBiome;
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Items.Tools;
using TranscendenceMod.Items.Weapons.Melee;

namespace TranscendenceMod.Miscanellous.UI.Achievements.Tasks
{
    public class Seraph : TaskUIElement
    {
        public override Texture2D Icon => ModContent.Request<Texture2D>("TranscendenceMod/NPCs/Boss/Seraph/CelestialSeraph_Head_Boss").Value;
        public override TaskIDs type => TaskIDs.Seraph;
        public override bool Unlocked => Main.LocalPlayer.GetModPlayer<ModAchievementsHelper>().SeraphUnlock;

        public override float x => 50f;

        public override float y => 125f;

        public override string col => "c7248f";

        public override CategoryIDs category => CategoryIDs.Prog;

        public override int reward => ModContent.ItemType<GalaxyAlloy>();
        public override int amount => 4;
    }
}

