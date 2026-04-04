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
using TranscendenceMod.Items.Materials.LargeRecipes;
using TranscendenceMod.Items.Materials.MobDrops;
using TranscendenceMod.Items.Weapons.Melee;

namespace TranscendenceMod.Miscanellous.UI.Achievements.Tasks
{
    public class Nucleus2 : TaskUIElement
    {
        public override Texture2D Icon => ModContent.Request<Texture2D>("TranscendenceMod/NPCs/Boss/Nucleus/ProjectNucleus_Head_Boss").Value;
        public override TaskIDs type => TaskIDs.Nucleus;
        public override bool Unlocked => Main.LocalPlayer.GetModPlayer<ModAchievementsHelper>().NucleusUnlock2;

        public override float x => 50f;

        public override float y => 25f;

        public override string col => "b90d0d";

        public override CategoryIDs category => CategoryIDs.Prog;

        public override int reward => ModContent.ItemType<ElectricalComponent>();
        public override int amount => 1;
    }
}

