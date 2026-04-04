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
using TranscendenceMod.Items.Weapons.Melee;

namespace TranscendenceMod.Miscanellous.UI.Achievements.Tasks
{
    public class Artifact : TaskUIElement
    {
        public override Texture2D Icon => TextureAssets.Item[ModContent.ItemType<CosmicArtifact>()].Value;
        public override TaskIDs type => TaskIDs.Artifact;
        public override bool Unlocked => Main.LocalPlayer.GetModPlayer<ModAchievementsHelper>().ArtifactUnlock;

        public override float x => 25f;

        public override float y => 100f;

        public override string col => "ffa900";

        public override CategoryIDs category => CategoryIDs.Prog;

        public override int reward => ModContent.ItemType<ApolloPiece>();
        public override int amount => 4;
    }
}

