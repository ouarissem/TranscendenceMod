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
using TranscendenceMod.Items.Weapons.Melee;

namespace TranscendenceMod.Miscanellous.UI.Achievements.Tasks
{
    public class Nucleus1 : TaskUIElement
    {
        public override Texture2D Icon => TextureAssets.Item[ModContent.ItemType<NucleusSummonerItem>()].Value;
        public override TaskIDs type => TaskIDs.NucleusCaller;
        public override bool Unlocked => Main.LocalPlayer.GetModPlayer<ModAchievementsHelper>().NucleusUnlock1;

        public override float x => 25f;

        public override float y => 50f;

        public override string col => "668086";

        public override CategoryIDs category => CategoryIDs.Prog;

        
        public override int reward => ItemID.SoulofNight;
        public override int amount => 25;
    }
}

