using Microsoft.Build.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Security.Permissions;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI;
using TranscendenceMod.Items.Consumables;
using TranscendenceMod.Items.Consumables.Boss;
using TranscendenceMod.Items.Consumables.Placeables;
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Items.Materials.MobDrops;
using TranscendenceMod.Items.Modifiers;
using TranscendenceMod.Items.Tools.Generic.Hardmetal;
using TranscendenceMod.Items.Weapons.Magic;
using TranscendenceMod.Miscannellous.UI.Achievements;
using static TranscendenceMod.TranscendenceWorld;

namespace TranscendenceMod.Miscanellous.UI.Achievements.Tasks
{
    public class ModAchievementsHelper : ModPlayer
    {
        public bool BeginUnlock;
        public bool ParryUnlock;
        public bool ModifierUnlock;
        public bool ModifierBagUnlock;
        public bool SnowmanUnlock;
        public bool CosmicNPCUnlock;
        public bool TimedialUnlock;
        public bool HardmetalUnlock;
        public bool ProcessUnlock;
        public bool VolcanicUnlock;
        public bool ChaliceUnlock;
        public bool sansUnlock;
        public bool MuramasaUnlock;
        public bool WallUnlock;
        public bool EmpressUnlock;
        public bool MoonlordUnlock;
        public bool FrostSerpentUnlock;
        public bool AtmospheronUnlock;
        public bool PoseidonUnlock;
        public bool NucleusUnlock1;
        public bool NucleusUnlock2;
        public bool ArtifactUnlock;
        public bool SeraphUnlock;
        public bool SeraphForgeUnlock;
        public bool EoLChallengeUnlock;
        public bool NucleusChallengeUnlock;
        public bool TwentyTwoUnlock;
        public bool DragonChalUnlock;

        public override void PostUpdate()
        {
            base.PostUpdate();

            if (QuestBookUIDrawing.Visible && !BeginUnlock)
            {
                BeginUnlock = true;
                CompleteAchievement(TaskIDs.Begin);
            }

            if (Main.LocalPlayer.GetModPlayer<TranscendencePlayer>().HasParry && !ParryUnlock)
            {
                ParryUnlock = true;
                CompleteAchievement(TaskIDs.Parry);
            }

            if (Main.LocalPlayer.GetModPlayer<TranscendencePlayer>().AmountSpentAtBlacksmith > 0 && !ModifierUnlock)
            {
                ModifierUnlock = true;
                CompleteAchievement(TaskIDs.Modifier);
            }

            if (Player.HasItem(ModContent.ItemType<ModifierContainer>()) && !ModifierBagUnlock)
            {
                ModifierBagUnlock = true;
                CompleteAchievement(TaskIDs.ModifierBag);
            }

            if (Main.LocalPlayer.GetModPlayer<TranscendencePlayer>().TalkedToSnowy && !SnowmanUnlock)
            {
                SnowmanUnlock = true;
                CompleteAchievement(TaskIDs.Snowman);
            }

            if (Main.LocalPlayer.GetModPlayer<TranscendencePlayer>().CosmicNPCQuests > 0 && !CosmicNPCUnlock)
            {
                CosmicNPCUnlock = true;
                CompleteAchievement(TaskIDs.CosmicNPC);
            }

            if (Player.HasItem(ModContent.ItemType<Timedial>()) && !TimedialUnlock)
            {
                TimedialUnlock = true;
                CompleteAchievement(TaskIDs.Timedial);
            }

            if (Player.HasItem(ModContent.ItemType<HardmetalOre>()) && !HardmetalUnlock)
            {
                HardmetalUnlock = true;
                CompleteAchievement(TaskIDs.Hardmetal);
            }

            if (Player.HasItem(ModContent.ItemType<VolcanicRemains>()) && !VolcanicUnlock)
            {
                VolcanicUnlock = true;
                CompleteAchievement(TaskIDs.Volcanic);
            }

            if (Downed.Contains(Bosses.FlameGuardian) && !ChaliceUnlock)
            {
                ChaliceUnlock = true;
                CompleteAchievement(TaskIDs.Chalice);
            }

            if (NPC.downedBoss3 && !sansUnlock)
            {
                sansUnlock = true;
                CompleteAchievement(TaskIDs.sans);
            }

            if (Downed.Contains(Bosses.Muramasa) && !MuramasaUnlock)
            {
                MuramasaUnlock = true;
                CompleteAchievement(TaskIDs.Muramasa);
            }

            if (Main.hardMode && !WallUnlock)
            {
                WallUnlock = true;
                CompleteAchievement(TaskIDs.Wall);
            }

            if (NPC.downedEmpressOfLight && !EmpressUnlock)
            {
                EmpressUnlock = true;
                CompleteAchievement(TaskIDs.Empress);
            }

            if (NPC.downedMoonlord && !MoonlordUnlock)
            {
                MoonlordUnlock = true;
                CompleteAchievement(TaskIDs.Moonlord);
            }

            if (Downed.Contains(Bosses.FrostSerpent) && !FrostSerpentUnlock)
            {
                FrostSerpentUnlock = true;
                CompleteAchievement(TaskIDs.FrostSerpent);
            }

            if (Downed.Contains(Bosses.Atmospheron) && !AtmospheronUnlock)
            {
                AtmospheronUnlock = true;
                CompleteAchievement(TaskIDs.Atmospheron);
            }

            if (Main.LocalPlayer.HasItem(ModContent.ItemType<PoseidonsTide>()) && !PoseidonUnlock)
            {
                PoseidonUnlock = true;
                CompleteAchievement(TaskIDs.PoseidonFrag);
            }

            if (Main.LocalPlayer.HasItem(ModContent.ItemType<NucleusSummonerItem>()) && !NucleusUnlock1)
            {
                NucleusUnlock1 = true;
                CompleteAchievement(TaskIDs.NucleusCaller);
            }

            if (Downed.Contains(Bosses.ProjectNucleus) && !NucleusUnlock2)
            {
                NucleusUnlock2 = true;
                CompleteAchievement(TaskIDs.Nucleus);
            }

            if (Main.LocalPlayer.HasItem(ModContent.ItemType<CosmicArtifact>()) && !ArtifactUnlock)
            {
                ArtifactUnlock = true;
                CompleteAchievement(TaskIDs.Artifact);
            }

            if (Downed.Contains(Bosses.CelestialSeraph) && !SeraphUnlock)
            {
                SeraphUnlock = true;
                CompleteAchievement(TaskIDs.Seraph);
            }

            if (Main.LocalPlayer.HasItem(ModContent.ItemType<StarcraftedForgeItem>()) && !SeraphForgeUnlock)
            {
                SeraphForgeUnlock = true;
                CompleteAchievement(TaskIDs.StarForge);
            }

        }

        public static void CompleteAchievement(TaskIDs task)
        {
            if (!Main.dedServ)
            {
                string typeString = task.ToString();

                string text = Language.GetTextValue($"Mods.TranscendenceMod.Achievement.Notification") + " " +
                    Language.GetTextValue($"Mods.TranscendenceMod.Achievement.Steps.{typeString}.DisplayName");
                string chatText = $"[C/ffcd00:{Language.GetTextValue($"Mods.TranscendenceMod.Achievement.Notification")}]" + " " +
                    Language.GetTextValue($"Mods.TranscendenceMod.Achievement.Steps.{typeString}.DisplayName");

                Main.NewText(chatText);
                DialogUI.SpawnDialog(text, true, new Vector2(Main.screenWidth / 2f, Main.screenHeight - 200), 450, Color.Gold);
                SoundEngine.PlaySound(SoundID.AchievementComplete);
                Main.LocalPlayer.GetModPlayer<TranscendencePlayer>().NewAchievements.Add(task);
            }
        }

        public static void CompleteChallenge(Player player, TaskIDs task)
        {
            if (!Main.dedServ)
            {
                if (task == TaskIDs.EmpressChallenge)
                     player.GetModPlayer<ModAchievementsHelper>().EoLChallengeUnlock = true;
                if (task == TaskIDs.NucleusChallenge)
                    player.GetModPlayer<ModAchievementsHelper>().NucleusChallengeUnlock = true;
                if (task == TaskIDs.TwentyTwoChallenge)
                    player.GetModPlayer<ModAchievementsHelper>().TwentyTwoUnlock = true;
                if (task == TaskIDs.DragonChallenge)
                    player.GetModPlayer<ModAchievementsHelper>().DragonChalUnlock = true;

                string typeString = task.ToString();

                string text = Language.GetTextValue($"Mods.TranscendenceMod.Achievement.Notification") + " " +
                    Language.GetTextValue($"Mods.TranscendenceMod.Achievement.Steps.{typeString}.DisplayName");
                string chatText = $"[C/ffcd00:{Language.GetTextValue($"Mods.TranscendenceMod.Achievement.Notification")}]" + " " +
                    Language.GetTextValue($"Mods.TranscendenceMod.Achievement.Steps.{typeString}.DisplayName");

                Main.NewText(chatText);
                DialogUI.SpawnDialog(text, true, new Vector2(Main.screenWidth / 2f, Main.screenHeight - 200), 450, Color.Gold);
                SoundEngine.PlaySound(SoundID.AchievementComplete);
                Main.LocalPlayer.GetModPlayer<TranscendencePlayer>().NewAchievements.Add(task);
            }
        }

        public override void SaveData(TagCompound tag)
        {
            base.SaveData(tag);

            if (BeginUnlock) tag["BeginUnlock"] = true;

            if (ParryUnlock) tag["ParryUnlock"] = true;

            if (ModifierUnlock) tag["ModifierUnlock"] = true;

            if (ModifierBagUnlock) tag["ModifierBagUnlock"] = true;

            if (SnowmanUnlock) tag["SnowmanUnlock"] = true;

            if (CosmicNPCUnlock) tag["CosmicNPCUnlock"] = true;

            if (TimedialUnlock) tag["TimedialUnlock"] = true;

            if (HardmetalUnlock) tag["HardmetalUnlock"] = true;

            if (ProcessUnlock) tag["ProcessUnlock"] = true;

            if (VolcanicUnlock) tag["VolcanicUnlock"] = true;

            if (sansUnlock) tag["sansUnlock"] = true;

            if (MuramasaUnlock) tag["MuramasaUnlock"] = true;

            if (WallUnlock) tag["WallUnlock"] = true;

            if (EmpressUnlock) tag["EmpressUnlock"] = true;

            if (MoonlordUnlock) tag["MoonlordUnlock"] = true;

            if (FrostSerpentUnlock) tag["FrostSerpentUnlock"] = true;

            if (AtmospheronUnlock) tag["AtmospheronUnlock"] = true;

            if (PoseidonUnlock) tag["PoseidonUnlock"] = true;

            if (NucleusUnlock1) tag["NucleusUnlock1"] = true;

            if (NucleusUnlock2) tag["NucleusUnlock2"] = true;

            if (ArtifactUnlock) tag["ArtifactUnlock"] = true;

            if (SeraphUnlock) tag["SeraphUnlock"] = true;

            if (SeraphForgeUnlock) tag["SeraphForgeUnlock"] = true;

            if (EoLChallengeUnlock) tag["EoLChallengeUnlock"] = true;

            if (NucleusChallengeUnlock) tag["NucleusChallengeUnlock"] = true;

            if (TwentyTwoUnlock) tag["TwentyTwoUnlock"] = true;

            if (DragonChalUnlock) tag["DragonChalUnlock"] = true;
        }

        public override void LoadData(TagCompound tag)
        {
            base.LoadData(tag);

            BeginUnlock = tag.ContainsKey("BeginUnlock");

            ParryUnlock = tag.ContainsKey("ParryUnlock");

            ModifierUnlock = tag.ContainsKey("ModifierUnlock");

            ModifierBagUnlock = tag.ContainsKey("ModifierBagUnlock");

            SnowmanUnlock = tag.ContainsKey("SnowmanUnlock");

            CosmicNPCUnlock = tag.ContainsKey("CosmicNPCUnlock");

            TimedialUnlock = tag.ContainsKey("TimedialUnlock");

            HardmetalUnlock = tag.ContainsKey("HardmetalUnlock");

            ProcessUnlock = tag.ContainsKey("ProcessUnlock");

            VolcanicUnlock = tag.ContainsKey("VolcanicUnlock");

            sansUnlock = tag.ContainsKey("sansUnlock");

            MuramasaUnlock = tag.ContainsKey("MuramasaUnlock");

            WallUnlock = tag.ContainsKey("WallUnlock");

            EmpressUnlock = tag.ContainsKey("EmpressUnlock");

            MoonlordUnlock = tag.ContainsKey("MoonlordUnlock");

            FrostSerpentUnlock = tag.ContainsKey("FrostSerpentUnlock");

            AtmospheronUnlock = tag.ContainsKey("AtmospheronUnlock");

            PoseidonUnlock = tag.ContainsKey("PoseidonUnlock");

            NucleusUnlock1 = tag.ContainsKey("NucleusUnlock1");

            NucleusUnlock2 = tag.ContainsKey("NucleusUnlock2");

            ArtifactUnlock = tag.ContainsKey("ArtifactUnlock");

            SeraphUnlock = tag.ContainsKey("SeraphUnlock");

            SeraphForgeUnlock = tag.ContainsKey("SeraphForgeUnlock");

            EoLChallengeUnlock = tag.ContainsKey("EoLChallengeUnlock");

            NucleusChallengeUnlock = tag.ContainsKey("NucleusChallengeUnlock");

            TwentyTwoUnlock = tag.ContainsKey("TwentyTwoUnlock");

            DragonChalUnlock = tag.ContainsKey("DragonChalUnlock");
        }
    }
}

