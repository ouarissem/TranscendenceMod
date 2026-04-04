using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using TranscendenceMod.Items.Consumables.Placeables;
using TranscendenceMod.Items.Materials;

namespace TranscendenceMod.Miscanellous.UI.Achievements.Tasks
{
    public class SeraphForge : TaskUIElement
    {
        public override Texture2D Icon => TextureAssets.Item[ModContent.ItemType<StarcraftedForgeItem>()].Value;
        public override TaskIDs type => TaskIDs.StarForge;
        public override bool Unlocked => Main.LocalPlayer.GetModPlayer<ModAchievementsHelper>().SeraphForgeUnlock;

        public override float x =>25f;

        public override float y => 150f;

        public override string col => "5a6c9a";

        public override CategoryIDs category => CategoryIDs.Prog;

        public override int reward => ModContent.ItemType<StarcraftedAlloy>();
        public override int amount => 1;
    }
}

