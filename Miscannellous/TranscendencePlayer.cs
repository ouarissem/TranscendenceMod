using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Achievements;
using Terraria.GameContent.Drawing;
using Terraria.GameInput;
using Terraria.Graphics.CameraModifiers;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Renderers;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.ModLoader.UI;
using TranscendenceMod.Buffs;
using TranscendenceMod.Buffs.Items;
using TranscendenceMod.Buffs.Items.InfectionAccessories;
using TranscendenceMod.Buffs.Items.Potions;
using TranscendenceMod.Dusts;
using TranscendenceMod.Items;
using TranscendenceMod.Items.Accessories.Expert;
using TranscendenceMod.Items.Accessories.Movement.Wings;
using TranscendenceMod.Items.Accessories.Offensive.EoL;
using TranscendenceMod.Items.Accessories.Shields;
using TranscendenceMod.Items.Accessories.Vanity;
using TranscendenceMod.Items.Armor.Sets.Cosmic;
using TranscendenceMod.Items.Consumables;
using TranscendenceMod.Items.Consumables.FoodAndDrinks;
using TranscendenceMod.Items.Consumables.LootBags;
using TranscendenceMod.Items.Consumables.Placeables.SpaceBiome;
using TranscendenceMod.Items.Farming.Seeds;
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Items.Materials.Fish;
using TranscendenceMod.Items.Materials.MobDrops;
using TranscendenceMod.Items.Mounts;
using TranscendenceMod.Items.Tools;
using TranscendenceMod.Items.Weapons.Melee;
using TranscendenceMod.Items.Weapons.Ranged;
using TranscendenceMod.Miscanellous.MiscSystems;
using TranscendenceMod.Miscanellous.UI.Achievements.Tasks;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Miscannellous.Biomes;
using TranscendenceMod.Miscannellous.GlobalStuff;
using TranscendenceMod.Miscannellous.UI.Achievements;
using TranscendenceMod.Miscannellous.UI.ModifierUI;
using TranscendenceMod.Miscannellous.UI.Processer;
using TranscendenceMod.NPCs.Boss.FrostSerpent;
using TranscendenceMod.NPCs.Boss.Nucleus;
using TranscendenceMod.NPCs.Boss.Seraph;
using TranscendenceMod.NPCs.Passive;
using TranscendenceMod.NPCs.PreHard;
using TranscendenceMod.NPCs.SpaceBiome;
using TranscendenceMod.Projectiles.Equipment;
using TranscendenceMod.Projectiles.Equipment.Tools;
using TranscendenceMod.Projectiles.Modifiers;
using TranscendenceMod.Projectiles.NPCs.Bosses.Nucleus;
using TranscendenceMod.Projectiles.NPCs.Bosses.SpaceBoss;
using TranscendenceMod.Projectiles.Weapons.Crean;
using TranscendenceMod.Projectiles.Weapons.Ranged.Ammo;
using TranscendenceMod.Tiles.TilesheetHell.Nature;
using static Terraria.Player;
using static TranscendenceMod.TranscendenceWorld;
using SoundEngine = Terraria.Audio.SoundEngine;

namespace TranscendenceMod
{
    public class TranscendencePlayer : ModPlayer
    {
        /*Modifiers*/
        public float CritDamage;
        public float Luminosity;
        public bool DangerDetection;
        public int GiantSlayer;
        public int CultScrollsEquipped;
        public bool UsingCrateMagnet;
        public bool PearlMod;
        public int DragonScales;
        public int DragonScalesCD;
        public int SilkyEgg;
        public int[] SilkyCD = new int[3];
        public int Jolly;

        /*Accessories*/
        public bool CosmicAegis;
        public bool ChiselPotEquipped;
        public bool FairerMoonlord;

        public bool EverglowingCrownEquipped;
        public int EverglowingSunCD;

        public bool NohitMode;
        public bool RingOfBravery;

        public bool Possessing;
        public int PossessingTimer;
        public NPC PossessedNPC;
        /**/
        public bool ShowEolTransform;
        public bool HasEolProjectile;
        public bool EolNightDeathBomb;
        public int DeathBombTime;
        public int DeathBombCD;
        /**/
        public bool Sussy;
        public int HasSurvivorKnife;
        public bool Eternity;
        public bool CosmicWings;
        public bool DualBall;

        public bool VoidNecklaceAcc;
        public float VoidNecklaceAlpha;
        public bool VoidNecklaceWithinRange;

        public bool OverloadedCore;
        public int OCoreTimer;
        public int OCoreChargeTimer;

        /**/
        public bool AstronautHelmet;
        public bool ApolloHelmet;

        public int BatteryAcc;
        public int BatteryCooldown;
        public int RocketAcc;
        public int RocketCD;
        public bool ThrowingGlove;
        public int ThrowingGloveCD;
        public int WearingEarlygameShoes;

        public bool EvasionStoneEquipped;
        public bool EvasionStoneExists;
        public int EvasionStoneTimer;
        public int EvasionStoneMaxTimer;
        public int EvasionStoneGraze;

        public bool EmpoweringTabletEquipped;

        public bool HideLegs;

        public bool Stargazer;
        public int stargazerDamage;

        public bool FrozenMaw;
        public int FrozenMawDamage;

        public bool PerfectHorseshoe;
        public int HorseshoeBonusActive;

        public bool NucleusLens;
        public bool NucleusLensSocial;

        public bool FishronPerceptionAcc;
        public bool BrokenShield;

        
        /*Modifiers*/
        public Item ModifierUIModdable;
        public Item ModifierUIModifier;
        public Item ModifierUIIngredient;
        public int AmountSpentAtBlacksmith;

        /// <summary>
        /// The NPC assigned for the Modifier UI, used for closing the UI when too faraway and playing sounds
        /// </summary>
        public NPC ModifierUINPCPos;
        public bool ModifierUIOnPhone;

        public bool HasModifiersInventory;
        /// <summary>
        /// Higher = More Expensive
        /// </summary>
        public float TinkererHappiness;


        /*Shields*/
        public bool HasParry;
        public int ParryCD;
        public int ParryTimer;
        public int ParryTimerCD;
        public int ShieldID;
        /// <summary>
        /// 0f to 100f
        /// </summary>
        public float Focus;
        public float MaxFocus = 100f;
        public float FocusGatherSpeed = 0.075f;
        public float ParryFocusCost = 85f;
        public float FocusRegenDelay;

        public int TurtleCD;
        public int BeetleCD;
        public int GiantShellCD;
        public int GolemCD;

        public bool TurtleShield;
        public bool BeetleShield;
        public bool OrangeShell;
        public bool PurpleShell;
        public bool HealthyJewel;
        public bool MoltenShieldEquipped;
        public bool LihzardianBulwarkEquipped;
        public bool EolAegis;

        public int InsideShell;

        public bool InsideGolem;
        public int GolemJumpCD;
        public int GolemDustCD;
        public int GolemCrushTimer;

        public bool StardustShield;
        public bool CultistForcefield;

        public int ShieldDamage;
        public int AegisRamDamage;
        public bool PalladiumShieldEquipped;

        public int MuramasaShieldIFrames;
        public int ShieldIFrames;

        public int EolTimer = 0;
        public int EolWhipTimer;
        public bool Vampire;
        public int VampireBlood;
        public int VampireHealAmount;
        public int VampireMinTimer;
        public int VampireMinTime = 1200;
        public int CrimsonNecklaceBloodCD;
        public int CrimsonNecklaceMaxBlood;
        public bool CorruptWanderingKit;
        public bool UsingLunarGauntlet;
        public int AlternativeEolCrownStateEnabled;
        public bool AlternativeEolCrownState;

        public bool FishNeck;
        public int FishTrans;
        public int FishFrame;

        public bool LacewingTrans;
        public int LacewingFrame;
        public int LacewingTransCD;


        /*Controls*/
        public bool NucleusLensKeybind;
        public bool ShieldGuard;
        public bool InfectionAbility;
        public bool HyperDashKeybind;
        public bool LeftClicking;
        public bool ArmorKeybind;


        /*Buffs*/
        public bool MeltingBlood;
        public bool BloodDying;
        public bool EolBurn;
        public bool SpaceBossDot;
        public bool DragonClawBuff;
        public bool SpaceSuffocation;
        public bool HasJellyBuff;
        public bool FrostBite;
        public int IsBlind;

        public bool CannotUseItems;
        public int CannotUseItemsTimer;
        public bool NoHealing;
        public bool InfFlight;
        public bool ExtraTerrestrialEffects;
        public bool StarcraftedDrunk;
        public bool SuckedIn;

        public int SturdyPlateTimer;
        public bool CrystalRadiationPill;

        public bool SunMelt;
        /**/

        public int HitTimer;

        /*Items*/
        public int CosmoShardTimer = 0;
        public int MaxStarfruits = 20;
        public int EatenStarfruits;
        public bool ConsumedInferno;
        public int MuramasaTime;
        public int MovingCannon;
        public Projectile Cannon;
        public int PaintBounceCD;
        public int BrushCooloff;
        public bool WandOfReturnCD;
        public int LegendarySwordTimer;
        public int VoltageBeamTimer;
        public int ElectroPickCD;
        public int ConstellationsIndex;


        /*Armor*/
        public bool OcramHelmet;
        public int OcramTimer;
        public bool OcramBuff;

        public bool RaiderSetWear;

        public bool SharkscaleSetWear;

        public bool CosmicSetWear;
        public Vector2[] CosmicTPpositions = new Vector2[8];


        /*Environment*/
        public bool ZoneStar;
        public int ZoneStarTimer;
        public float StarFade;

        public int FanWindCD;
        public float VolcanoHeatwaveTimer;
        public bool ZoneVolcano => (Player.position.Y > ((Main.maxTilesY - 400) * 16) && !Player.ZoneUnderworldHeight && !Player.ZoneDungeon);
        public bool ZoneSpaceTemple;
        public int ZoneSpaceTempleTimer;
        public bool ZoneLandSite;
        public int ZoneLandSiteTimer;
        public int ZoneLandSiteWaterTimer;

        public bool ZoneSeraphMonolith;
        public int SeraphMonolithTimer;

        public int ZoneSerpentMonolith;

        public float ScaleMult = 1f;
        public int ScaleResetTimer;

        /*Screen Shader Timers*/
        public int SaturationTimer;
        public int DesaturationTimer;
        public int FlashBangTimer;
        public float FBa;
        public int InverseTimer;
        public int HotTimer;
        public int ColdTimer;
        public int FlipTimer;
        public int ScreenVignetteTimer;
        public int StaticTimer;

        /*Beetle Shell Crumble*/
        public int ShellCrumble;
        // One minute
        public readonly int ShellCrumbleMaxCD = 3600;
        public int ShellCrumbleCD;


        /*Misc*/
        public bool AchBookProg = true;
        public bool AchBookEaM = true;
        public bool AchBookMisc = true;
        public List<TaskIDs> NewAchievements = new List<TaskIDs>();

        public int FullRotResetCD;

        public bool TalkedToSnowy;
        public int FrostMoonHS;

        public int CosmicNPCQuests;

        public bool cameraModifier;
        public Vector2 cameraPos;

        public int NucleusConsumed;
        public int NucleusDeathAnim;
        public float NucleusZoom;

        public Item ModifierContainerItem;

        public Vector2[] PortalBoxPositions = new Vector2[3];

        public int CreanStaffClick;

        public Tile ProcesserTile;
        public Vector2 ProcesserPos;

        public override void SaveData(TagCompound tag)
        {
            if (CosmicNPCQuests > 0) tag["CosmicNPCQuests"] = CosmicNPCQuests;
            if (TalkedToSnowy) tag["TalkedToSnowy"] = true;
            if (ConsumedInferno) tag["ConsumedInferno"] = true;
            if (EatenStarfruits > 0) tag["EatenStarfruits"] = EatenStarfruits;
            if (FrostMoonHS > 0) tag["FrostMoonHS"] = FrostMoonHS;
            if (AmountSpentAtBlacksmith > 0) tag["AmountSpentAtBlacksmith"] = AmountSpentAtBlacksmith;
        }
        public override void LoadData(TagCompound tag)
        {
            CosmicNPCQuests = tag.GetInt("CosmicNPCQuests");
            TalkedToSnowy = tag.ContainsKey("TalkedToSnowy");
            ConsumedInferno = tag.ContainsKey("ConsumedInferno");
            EatenStarfruits = tag.GetInt("EatenStarfruits");
            FrostMoonHS = tag.GetInt("FrostMoonHS");
            AmountSpentAtBlacksmith = tag.GetInt("AmountSpentAtBlacksmith");
        }
        public void ShaderShit()
        {
            if (SaturationTimer > 0) SaturationTimer--;
            if (DesaturationTimer > 0) DesaturationTimer--;
            if (FlashBangTimer > 0)
            {
                FlashBangTimer--;
            }
            if (InverseTimer > 0) InverseTimer--;
            if (HotTimer > 0) HotTimer--;
            if (ColdTimer > 0) ColdTimer--;
            if (ScreenVignetteTimer > 0) ScreenVignetteTimer--;
            if (FlipTimer > 0) FlipTimer--;

            if (Main.netMode != NetmodeID.Server)
            {
                if (Filters.Scene["TranscendenceMod:ScreenFlip"].IsActive() && (FlipTimer == 0 || Player.dead)) Filters.Scene["TranscendenceMod:ScreenFlip"].Deactivate();
                if (Filters.Scene["TranscendenceMod:ColdScreen"].IsActive() && (ColdTimer == 0 || Player.dead)) Filters.Scene["TranscendenceMod:ColdScreen"].Deactivate();
                if (Filters.Scene["TranscendenceMod:HotScreen"].IsActive() && (HotTimer == 0 || Player.dead)) Filters.Scene["TranscendenceMod:HotScreen"].Deactivate();
                if (Filters.Scene["TranscendenceMod:SaturationShader"].IsActive() && (SaturationTimer == 0 || Player.dead)) Filters.Scene["TranscendenceMod:SaturationShader"].Deactivate();
                if (Filters.Scene["TranscendenceMod:DesaturationShader"].IsActive() && (DesaturationTimer == 0 || Player.dead)) Filters.Scene["TranscendenceMod:DesaturationShader"].Deactivate();
                if (Filters.Scene["TranscendenceMod:Static"].IsActive() && (StaticTimer == 0 || Player.dead)) Filters.Scene["TranscendenceMod:Static"].Deactivate();

                if (!Filters.Scene["TranscendenceMod:FlashbangShader"].IsActive())
                    Filters.Scene.Activate("TranscendenceMod:FlashbangShader");

                if (Filters.Scene["TranscendenceMod:FlashbangShader"].IsActive() && (FlashBangTimer == 0 || Player.dead))
                    Filters.Scene["TranscendenceMod:FlashbangShader"].GetShader().UseOpacity(0f);

                if (Filters.Scene["TranscendenceMod:InverseShader"].IsActive() && (InverseTimer == 0 || Player.dead)) Filters.Scene["TranscendenceMod:InverseShader"].Deactivate();
                if (Filters.Scene["TranscendenceMod:ScreenVignette"].IsActive() && ScreenVignetteTimer == 0) Filters.Scene["TranscendenceMod:ScreenVignette"].Deactivate();
            }
        }
        public override void ResetEffects()
        {
            if (FocusRegenDelay > 0)
                FocusRegenDelay--;

            if (!OverloadedCore)
                OCoreTimer--;

            MaxFocus = 100f;
            FocusGatherSpeed = 0.2f;
            ParryFocusCost = 40f;
            Jolly = 0;
            SilkyEgg = 0;
            DragonScales = 0;
            FrozenMaw = false;
            SunMelt = false;
            RingOfBravery = false;
            FrostBite = false;
            FairerMoonlord = false;
            PerfectHorseshoe = false;

            ShaderShit();

            if (HorseshoeBonusActive > 0)
                HorseshoeBonusActive--;

            if (LacewingTransCD > 0)
                LacewingTransCD--;

            if (CreanStaffClick > 0)
                CreanStaffClick--;

            if (NucleusConsumed > 0)
                NucleusConsumed--;

            if (!NPC.AnyNPCs(ModContent.NPCType<ProjectNucleus>()) || Player.GetModPlayer<NucleusGame>().Active)
                NucleusZoom = 0f;

            if (GolemCD > 0 && LihzardianBulwarkEquipped)
                GolemCD--;

            if (StaticTimer > 0)
                StaticTimer--;

            OverloadedCore = false;
            if (FishTrans > 0)
                FishTrans--;
            FishNeck = false;
            SharkscaleSetWear = false;
            FishronPerceptionAcc = false;
            HasJellyBuff = false;
            if (IsBlind > 0)
                IsBlind--;
            HasModifiersInventory = false;
            BrokenShield = false;
            LihzardianBulwarkEquipped = false;
            CrystalRadiationPill = false;
            EolAegis = false;
            NucleusLens = false;
            NucleusLensSocial = false;
            if (FullRotResetCD > 0)
                FullRotResetCD--;
            else Player.fullRotation = MathHelper.Lerp(Player.fullRotation, 0, 0.25f);

            if (ElectroPickCD > 0)
                ElectroPickCD--;

            if (ShieldIFrames > 0)
                ShieldIFrames--;

            if (VoltageBeamTimer > 0)
                VoltageBeamTimer--;

            if (FanWindCD > 0)
                FanWindCD--;

            CosmicAegis = false;
            DualBall = false;
            VoidNecklaceWithinRange = false;
            DragonClawBuff = false;
            DangerDetection = false;
            PearlMod = false;
            PalladiumShieldEquipped = false;
            CosmicWings = false;
            Stargazer = false;

            if (TurtleCD > 0)
                TurtleCD--;

            if (BeetleCD > 0)
                BeetleCD--;

            if (GiantShellCD > 0)
                GiantShellCD--;

            if (LegendarySwordTimer > 0)
                LegendarySwordTimer--;

            if (SturdyPlateTimer > 0)
                SturdyPlateTimer--;

            if (MovingCannon > 0)
            {
                Player.controlJump = false;
                Player.mount.Dismount(Player);
                MovingCannon--;
            }
            Cannon = null;

            if (ScaleResetTimer > 0) ScaleResetTimer--;
            else ScaleMult = 1f;


            if (BrushCooloff > 0)
                BrushCooloff--;

            if (InsideShell > 0)
            {
                CannotUseItems = true;
                CannotUseItemsTimer = 5;
                InsideShell--;
            }

            if (PaintBounceCD > 0)
                PaintBounceCD--;

            RaiderSetWear = false;

            float tpDistance = Player.Distance(Main.MouseWorld);
            if (tpDistance < 126)
                tpDistance = 125;

            if (tpDistance > 499)
                tpDistance = 500;

            if (CosmicSetWear && !Player.HasBuff(BuffID.ChaosState))
            {
                for (int i = 0; i < CosmicTPpositions.Length; i++)
                {
                    Vector2 vec = Vector2.One.RotatedBy(MathHelper.TwoPi * i / 8f + MathHelper.PiOver4) * tpDistance * 0.7f;
                    CosmicTPpositions[i] = Player.Center + vec;
                }

                void TP(Vector2 pos, bool jump)
                {
                    if ((jump || pos.Y < Player.position.Y) && !TranscendenceUtils.BossAlive())
                    {
                        if (jump)
                        {
                            int dir = pos.X > Player.Center.X ? 1 : -1;
                            Player.velocity.X = 30 * dir;
                        }
                        Player.velocity.Y = pos.Y < Player.position.Y ? (pos.X != Player.Center.X ? -15f : -22.5f) : -10f;
                    }

                    Projectile.NewProjectile(Player.GetSource_FromAI(), Player.Center, Vector2.Zero, ModContent.ProjectileType<CosmicPortal>(), 0, 0, Player.whoAmI, 0, 1);
                    Player.Teleport(pos, 6);

                    SoundEngine.PlaySound(new SoundStyle("TranscendenceMod/Miscannellous/Assets/Sounds/NPCs/Attack/SeraphTeleport"));

                    Player.AddBuff(BuffID.ChaosState, TranscendenceUtils.BossAlive() ? 450 : 120);
                    Projectile.NewProjectile(Player.GetSource_FromAI(), Player.Center, Vector2.Zero, ModContent.ProjectileType<CosmicPortal>(), 0, 0, Player.whoAmI, 0, 2);
                }
                if (ArmorKeybind)
                {
                    for (int i = 0; i < CosmicTPpositions.Length; i++)
                    {
                        bool hasTiles = Collision.SolidCollision(CosmicTPpositions[i] - (Player.Size / 2), Player.width, Player.height);

                        if (Main.MouseWorld.Distance(CosmicTPpositions[i]) < 50 && !hasTiles)
                        {
                            CannotUseItems = true;
                            CannotUseItemsTimer = 15;

                            if (Main.mouseLeft)
                                TP(CosmicTPpositions[i], i > 1 && i < 7 && i != 4);
                        }
                        else CosmicTPpositions[i] = Player.Center;
                    }
                }
            }

            VoidNecklaceAcc = false;

            if (!Vampire)
                VampireBlood = 0;
            Vampire = false;

            SpaceBossDot = false;
            MoltenShieldEquipped = false;
            ChiselPotEquipped = false;
            ShowEolTransform = false;
            EverglowingCrownEquipped = false;
            Sussy = false;
            BloodDying = false;
            MeltingBlood = false;
            ZoneLandSite = false;
            EvasionStoneEquipped = false;
            EmpoweringTabletEquipped = false;
            OcramHelmet = false;
            Eternity = false;
            WandOfReturnCD = false;
            SpaceSuffocation = false;

            HealthyJewel = false;
            StardustShield = false;
            CultistForcefield = false;

            if (AlternativeEolCrownStateEnabled > 0) AlternativeEolCrownStateEnabled--;

            GiantSlayer = 0;

            if (!cameraModifier)
                cameraPos = Player.Center;

            cameraModifier = false;


            if (HasSurvivorKnife > 0)
                HasSurvivorKnife--;

            EolBurn = false;
            EolNightDeathBomb = false;
            ExtraTerrestrialEffects = false;
            StarcraftedDrunk = false;
            TurtleShield = false;
            ThrowingGlove = false;
            CorruptWanderingKit = false;

            WearingEarlygameShoes = 0;
            if (ZoneSerpentMonolith > 0) ZoneSerpentMonolith--;
            if (SeraphMonolithTimer < 1) ZoneSeraphMonolith = false;
            if (CannotUseItemsTimer < 1) CannotUseItems = false;
            if (ZoneLandSiteTimer < 1) ZoneLandSite = false;
            if (ZoneSpaceTempleTimer < 1) ZoneSpaceTemple = false;
            OrangeShell = false;
            PurpleShell = false;
            NohitMode = false;
            NoHealing = false;
            CosmicSetWear = false;
            InfFlight = false;
            HasEolProjectile = false;
            AstronautHelmet = false;
            ApolloHelmet = false;
            HasParry = Player.HeldItem.type == ModContent.ItemType<LegendarySword>();
            BatteryAcc = 0;
            if (BatteryCooldown > 0)
                BatteryCooldown--;
            RocketAcc = 0;
            CritDamage = 0;
            Luminosity = 0;
            CultScrollsEquipped = 0;
            ZoneSpaceTemple = false;
            UsingCrateMagnet = false;
            UsingLunarGauntlet = false;
            HideLegs = false;
            if (!Player.HasBuff(ModContent.BuffType<BoABuff>())) OcramBuff = false;

            if (++PossessingTimer > 5)
            {
                Possessing = false;
                PossessedNPC = null;
                PossessingTimer = 0;
            }

            if (MuramasaTime > 0)
                MuramasaTime--;

            if (SuckedIn == true)
            {
                Player.headPosition = Vector2.Zero;
                Player.bodyPosition = Vector2.Zero;
                Player.legPosition = Vector2.Zero;
                SuckedIn = false;
            }

            BeetleShield = false;
        }

        public override void UpdateDead()
        {
            base.UpdateDead();
            ShaderShit();

            VampireBlood = 0;
            if (Player.respawnTimer == 1)
            {
                Focus = MaxFocus / 2f;
                LacewingTrans = false;
                FishTrans = 0;
            }

            if (Filters.Scene["TranscendenceMod:Static"].IsActive())
                Filters.Scene["TranscendenceMod:Static"].Deactivate();

            if (!TranscendenceUtils.BossAlive() && Player.respawnTimer > 300)
            {
                Player.respawnTimer = 300;
            }
        }

        public override void OnHurt(Player.HurtInfo info)
        {
            EvasionStoneGraze = (int)(EvasionStoneGraze * 0.75f);
            EvasionStoneTimer = EvasionStoneMaxTimer;

            ExpendFocus(Player, 15f, 30f);

            if (OcramHelmet && info.Damage > 5)
            {
                SoundEngine.PlaySound(SoundID.Item14, Player.Center);
                Projectile.NewProjectile(Player.GetSource_OnHurt(info.DamageSource), Player.Center, Vector2.Zero, ModContent.ProjectileType<BoAExplosion>(), Player.statDefense * 25, 10, Player.whoAmI);

                //Grant the buff when under 50% life
                if (Player.statLife < (Player.statLifeMax2 / 2))
                    Player.AddBuff(ModContent.BuffType<BoABuff>(), 5);
            }

            if (NohitMode)
            {
                NetworkText death = NetworkText.FromKey("Mods.TranscendenceMod.Messages.NohitMode1", Player.name);
                switch (Main.rand.Next(1, 5))
                {
                    case 1: death = NetworkText.FromKey("Mods.TranscendenceMod.Messages.Death.NohitMode1", Player.name); break;
                    case 2: death = NetworkText.FromKey("Mods.TranscendenceMod.Messages.Death.NohitMode2", Player.name); break;
                    case 3: death = NetworkText.FromKey("Mods.TranscendenceMod.Messages.Death.NohitMode3", Player.name); break;
                    case 4: death = NetworkText.FromKey("Mods.TranscendenceMod.Messages.Death.NohitMode4", Player.name); break;
                }

                Player.KillMe(PlayerDeathReason.ByCustomReason(death), 9999999, 0);
            }
            
            if (BatteryAcc != 0 && BatteryCooldown == 0)
            {
                BatteryCooldown = 600;

                //Spawn the projectiles if wearing the cape
                if (BatteryAcc == 2)
                {
                    for (int j = 0; j < 12; j++)
                    {
                        Projectile.NewProjectile(Player.GetSource_FromAI(), new Vector2(Player.Center.X + Main.rand.Next(-160, 160),
                            Player.Center.Y - Main.screenHeight + Main.rand.Next(-80, 80)), new Vector2(Main.rand.Next(-2, 2), 30),
                            ModContent.ProjectileType<WindBlast>(), 160, 0, Player.whoAmI);
                    }
                }
                for (int i = 0; i < Main.maxProjectiles; i++)
                {
                    Projectile projectile = Main.projectile[i];
                    if (projectile.Distance(Player.Center) < (BatteryAcc == 2 ? 600 : 400)
                        && projectile.hostile && projectile.active && projectile.GetGlobalProjectile<TranscendenceProjectiles>().CanBeErased)
                    {
                        TranscendenceUtils.DustRing(projectile.Center, 10, DustID.Electric, 8, Color.White, 1.25f);
                        SoundEngine.PlaySound(SoundID.Item93 with { MaxInstances = 0 }, projectile.Center);

                        if (Main.rand.NextBool(BatteryAcc == 2 ? 9 : 12))                                   
                        {
                            Item.NewItem(projectile.GetSource_FromAI(), projectile.getRect(), ItemID.Star);
                        }

                        projectile.Kill();
                    }
                }
            }
        }
        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            if (NPC.AnyNPCs(ModContent.NPCType<CelestialSeraph>()))
                Player.Center = new Vector2(TranscendenceWorld.SpaceTempleX, 97 * 16);
            SeraphTileDrawingSystem.PhaseThroughTimer = 0;

            ShellCrumbleCD = 2700;

            if (Player.GetModPlayer<NucleusGame>().Active && Player.GetModPlayer<NucleusGame>().BossEdition)
                playSound = false;

            if (Possessing)
                PossessedNPC.StrikeInstantKill();

            if (LacewingTrans)
            {
                for (int i = 0; i < 24; i++)
                {
                    int d = Dust.NewDust(Player.position, Player.width, Player.height, ModContent.DustType<PlayerCosmicBlood>(),
                        Main.rand.NextFloat(-12f, 12f), Main.rand.NextFloat(-12f, 12f), 0, Main.hslToRgb(i / 8f, 1f, 0.5f), Main.rand.NextFloat(0.8f, 1.25f));
                    Main.dust[d].alpha = Main.rand.Next(0, 15);
                }

                SoundEngine.PlaySound(SoundID.NPCDeath1, Player.Center);
                playSound = false;
                genGore = false;
            }

            if (InsideShell > 0 && (BeetleShield || TurtleShield))
            {
                for (int i = 0; i < 16; i++)
                {
                    string path = (BeetleShield ? "Beetle" : "Turtle") + $"{Main.rand.Next(1, 3)}";
                    int gore = Mod.Find<ModGore>(path).Type;

                    SoundEngine.PlaySound(ModSoundstyles.BeetleDeath, Player.Center);
                    Gore.NewGore(Player.GetSource_Death(), Player.Center, Main.rand.NextVector2Circular(2f, 2f), gore);
                }
            }

            if (SuckedIn)
            {
                genGore = false;
                playSound = false;
            }

            if (InsideGolem)
            {
                SoundEngine.PlaySound(SoundID.Item14, Player.Center);
                for (int i = 0; i < 7; i++)
                {
                    int g = Gore.NewGore(Player.GetSource_Death(), Player.Center, Main.rand.NextVector2Circular(8f, 16f), 360 + i);
                    Main.gore[g].velocity.Y = Main.rand.NextFloat(-12f, -6f);
                }
            }

            if (damageSource.SourceOtherIndex == 8 && hitDirection == 0)
            {
                if (EverglowingCrownEquipped && CorruptWanderingKit ||
                CorruptWanderingKit && Vampire ||
                EverglowingCrownEquipped && Vampire)
                    damageSource = PlayerDeathReason.ByCustomReason(NetworkText.FromKey("Mods.TranscendenceMod.Messages.Death.BlackHole", Player.name));

                if (MeltingBlood) damageSource = PlayerDeathReason.ByCustomReason(NetworkText.FromKey($"Mods.TranscendenceMod.Messages.Death.MagmaBlood{Main.rand.Next(0, 2)}", Player.name));
                if (EolBurn) damageSource = PlayerDeathReason.ByCustomReason(NetworkText.FromKey("Mods.TranscendenceMod.Messages.Death.FairyFire", Player.name));
                if (BloodDying) damageSource = PlayerDeathReason.ByCustomReason(NetworkText.FromKey("Mods.TranscendenceMod.Messages.Death.Blood", Player.name));
                if (SpaceBossDot) damageSource = PlayerDeathReason.ByCustomReason(NetworkText.FromKey("Mods.TranscendenceMod.Messages.Death.SpaceBoss", Player.name));
                if (SuckedIn) damageSource = PlayerDeathReason.ByCustomReason(NetworkText.FromKey("Mods.TranscendenceMod.Messages.Death.BlackHole", Player.name));
            }
            return base.PreKill(damage, hitDirection, pvp, ref playSound, ref genGore, ref damageSource);
        }
        public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
        {
            //Draw empress transformation with full bright
            if (ShowEolTransform)
            {
                r = 0;
                g = 0;
                b = 0;
                fullBright = true;
            }
        }

        public override void ModifyMaxStats(out StatModifier health, out StatModifier mana)
        {
            float am = 0f;
            am += EatenStarfruits * 5f;

            health = StatModifier.Default with { Base = am };
            mana = StatModifier.Default with { Base = 0 };
        }

        public override void UpdateBadLifeRegen()
        {
            void DoT(int lifeLoss)
            {
                if (Player.lifeRegen > 0) Player.lifeRegen = 0;
                Player.lifeRegenTime = 0;
                Player.lifeRegen -= lifeLoss;
            }

            if (Possessing || SpaceSuffocation) DoT(0);
            if (SpaceBossDot && !CrystalRadiationPill) DoT(55);
            if (EolBurn) DoT(45);
            if (BloodDying) DoT(125);
            if (MeltingBlood) DoT(250);
            if (SunMelt) DoT(375);
            if (FrostBite) DoT(35);

            if (EverglowingCrownEquipped && CorruptWanderingKit ||
                CorruptWanderingKit && Vampire ||
                EverglowingCrownEquipped && Vampire)
                DoT(100);
        }
        public override void PostUpdateRunSpeeds()
        {
            base.PostUpdateRunSpeeds();

            if (CorruptWanderingKit && InsideShell < 1)
            {
                Player.jumpHeight = (int)(Player.jumpHeight * 1.5f);
                Player.jumpSpeed *= 1.5f;

                if (Player.controlDown && InfectionAbility && Focus > 0.15f)
                {
                    Player.gravity *= 2.75f;
                    Player.maxFallSpeed *= 2.75f;

                    ExpendFocus(Player, 0.1f, 5f);
                }

                Player.runSlowdown += 0.75f;
                Player.runAcceleration = 2;
            }

            if (CosmicSetWear)
            {
                Player.jumpHeight = (int)(Player.jumpHeight * 1.15f);
                Player.jumpSpeed *= 1.5f;
            }

            if (InsideGolem)
            {
                Player.jumpHeight = 40;
                Player.jumpSpeed = 15f;
            }

            if (FrostBite)
                Player.velocity *= 0.95f;

            if (MeltingBlood)
                Player.velocity *= 0.66f;

            if (SunMelt)
                Player.velocity *= 0.5f;

            if (DualBall && ParryTimer > 0)
                Player.velocity *= 0.8f;
        }
        public override bool ConsumableDodge(Player.HurtInfo info)
        {
            return base.ConsumableDodge(info);
        }
        public override void PostUpdateEquips()
        {
            base.PostUpdateEquips();

            if (PerfectHorseshoe)
            {
                Player.luck += 0.1f;
                Player.noFallDmg = true;

                if (Collision.SolidCollision(Player.position, Player.width, Player.height + 2, true) && Player.velocity.Y == 0f || HorseshoeBonusActive > 0)
                {
                    Player.moveSpeed += 6f;
                    Player.accRunSpeed += 12f;
                    Player.runSlowdown *= 1.5f;
                    Player.CancelAllBootRunVisualEffects();

                    if (Player.velocity.X > 5f || Player.velocity.X < -5f)
                    {
                        if (Player.runSoundDelay <= 0)
                        {
                            SoundEngine.PlaySound(ModSoundstyles.Horseshoe with { PitchRange = (-0.5f, 0f) }, Player.Bottom);
                            Player.runSoundDelay = 20;
                        }

                        Color col = Color.Lerp(Color.SaddleBrown, Color.Gold, Main.rand.NextFloat(0f, 1f));
                        if (Main.rand.NextBool(3))
                            col = Main.hslToRgb(Main.rand.NextFloat(0f, 1f), 1f, 0.5f);

                        if (TranscendenceWorld.Timer % 5 == 0)
                            Dust.NewDustPerfect(Player.Bottom, ModContent.DustType<Horsespark>(),
                                new Vector2(Player.direction * -Main.rand.NextFloat(0f, 1.5f), -Main.rand.NextFloat(0f, 5f)), 0, col, Main.rand.NextFloat(0.5f, 1f));
                    }
                }
            }

            if (CorruptWanderingKit)
                Player.moveSpeed *= 1.5f;

            if (SpaceBossDot && CrystalRadiationPill)
                Player.moveSpeed *= 1.25f;

            if (InsideGolem)
            {
                Player.moveSpeed *= 0f;
                if (Player.velocity.Y != 0)
                    Player.moveSpeed = 3f;
            }
            if (BeetleShield)
            {
                Player.moveSpeed *= 0.66f;
                Player.jumpSpeed *= 0.66f;
            }

            if (WearingEarlygameShoes > 0)
            {
                Player.fullRotation = Player.velocity.X * 0.025f;
                if (WearingEarlygameShoes == 2) Player.moveSpeed += 0.2f;
            }
        }
        public override void PostUpdateMiscEffects()
        {
            base.PostUpdateMiscEffects();
            SetStats();
        }

        public override void Load()
        {
            On_LegacyPlayerRenderer.DrawPlayerInternal += On_LegacyPlayerRenderer_DrawPlayerInternal1;
            On_Player.PickTile += On_Player_PickTile;
            On_Player.SpawnFastRunParticles += On_Player_SpawnFastRunParticles;

            On_Player.DryCollision += On_Player_DryCollision;
            On_Player.WaterCollision += On_Player_WaterCollision;
            On_Player.HoneyCollision += On_Player_HoneyCollision;
            On_Player.ShimmerCollision += On_Player_ShimmerCollision;
            On_Player.FloorVisuals += On_Player_FloorVisuals;
            On_Player.UpdateTouchingTiles += On_Player_UpdateTouchingTiles;
            On_Player.SlopingCollision += On_Player_SlopingCollision;
        }

        private void On_Player_SlopingCollision(On_Player.orig_SlopingCollision orig, Player self, bool fallThrough, bool ignorePlats)
        {
            if (!self.HasBuff(ModContent.BuffType<LacewingTransBuff>()))
                orig(self, fallThrough, ignorePlats);
        }

        private void On_Player_UpdateTouchingTiles(On_Player.orig_UpdateTouchingTiles orig, Player self)
        {
            if (!self.HasBuff(ModContent.BuffType<LacewingTransBuff>()))
                orig(self);
        }

        private void On_Player_FloorVisuals(On_Player.orig_FloorVisuals orig, Player self, bool Falling)
        {
            if (!self.HasBuff(ModContent.BuffType<LacewingTransBuff>()))
                orig(self, false);
        }

        private void On_Player_ShimmerCollision(On_Player.orig_ShimmerCollision orig, Player self, bool fallThrough, bool ignorePlats, bool noCollision)
        {
            if (!self.HasBuff(ModContent.BuffType<LacewingTransBuff>()) && self.GetModPlayer<TranscendencePlayer>().FishTrans == 0)
                orig(self, fallThrough, ignorePlats, noCollision);
            else
            {
                if (self.GetModPlayer<TranscendencePlayer>().FishronPerceptionAcc)
                    self.position += self.velocity;
                else self.KillMe(PlayerDeathReason.ByCustomReason(NetworkText.FromKey($"Mods.TranscendenceMod.Messages.Death.FairyFire", self.name)), 9999, -self.direction);
            }
        }

        private void On_Player_HoneyCollision(On_Player.orig_HoneyCollision orig, Player self, bool fallThrough, bool ignorePlats)
        {
            if (!self.HasBuff(ModContent.BuffType<LacewingTransBuff>()))
                orig(self, fallThrough, ignorePlats);
            else
                self.position += self.velocity;
        }

        private void On_Player_WaterCollision(On_Player.orig_WaterCollision orig, Player self, bool fallThrough, bool ignorePlats)
        {
            if (!self.HasBuff(ModContent.BuffType<LacewingTransBuff>()))
                orig(self, fallThrough, ignorePlats);
            else
                self.position += self.velocity;
        }

        private void On_Player_DryCollision(On_Player.orig_DryCollision orig, Player self, bool fallThrough, bool ignorePlats)
        {
            if (!self.HasBuff(ModContent.BuffType<LacewingTransBuff>()))
                orig(self, fallThrough, ignorePlats);
            else self.position += self.velocity;
        }

        private void On_Player_SpawnFastRunParticles(On_Player.orig_SpawnFastRunParticles orig, Player self)
        {
            if (PerfectHorseshoe)
                return;

            orig(self);
        }

        private void On_Player_PickTile(On_Player.orig_PickTile orig, Player self, int x, int y, int pickPower)
        {
            if (self.HeldItem.type == ModContent.ItemType<ElectroPickaxe>() && self.ItemAnimationJustStarted && self.GetModPlayer<TranscendencePlayer>().ElectroPickCD == 0)
            {
                int proj = ModContent.ProjectileType<ElectroPickaxeChainCenter>();
                Projectile.NewProjectile(self.GetSource_FromThis(), new Vector2((float)x, (float)y).ToWorldCoordinates(), Vector2.Zero, proj, 0, 0, self.whoAmI);
                self.GetModPlayer<TranscendencePlayer>().ElectroPickCD = 30;
            }
            orig(self, x, y, pickPower);
        }

        private void On_LegacyPlayerRenderer_DrawPlayerInternal1(On_LegacyPlayerRenderer.orig_DrawPlayerInternal orig, LegacyPlayerRenderer self, Terraria.Graphics.Camera camera, Player drawPlayer, Vector2 position, float rotation, Vector2 rotationOrigin, float shadow, float alpha, float scale, bool headOnly)
        {
            drawPlayer.TryGetModPlayer(out TranscendencePlayer TranscendencePlayer);

            if (TranscendencePlayer == null || orig == null)
                return;

            orig.Invoke(self, camera, drawPlayer, position, rotation, rotationOrigin, shadow, alpha, TranscendencePlayer.ScaleMult, headOnly);
        }

        public override void OnEnterWorld()
        {
            if (Main.maxTilesX == 4200)
                Main.NewText(Language.GetTextValue("Mods.TranscendenceMod.Messages.SmallWorldWarning"));
        }
        public override void PreUpdateMovement()
        {
            if (Player.GetModPlayer<NucleusGame>().Active || NucleusConsumed > 0)
            {
                Player.position = Player.oldPosition;
                Player.velocity = Vector2.Zero;
                Player.gravity *= 0f;
                Main.playerInventory = false;
                Main.mapFullscreen = false;
                Player.GetModPlayer<TranscendencePlayer>().CannotUseItems = true;
                Player.GetModPlayer<TranscendencePlayer>().CannotUseItemsTimer = 5;


                if (Player.GetModPlayer<NucleusGame>().BossEdition || NucleusConsumed > 0)
                {
                    if (Player.GetModPlayer<NucleusGame>().Active)
                        Player.GetModPlayer<TranscendencePlayer>().NucleusDeathAnim = 15;

                    for (int i = 0; i < Main.maxProjectiles; i++)
                    {
                        Projectile p = Main.projectile[i];
                        if (p != null && p.active && p.owner == Player.whoAmI && !p.hostile && p.Distance(Player.Center) < 2000 && p.type != ModContent.ProjectileType<PlayerGameTrans>() && p.type != ModContent.ProjectileType<NucleusDeathBoom>() && p.type != ModContent.ProjectileType<BloodLiquid>())
                            p.active = false;
                    }
                }

                return;
            }

            bool MeteoriteJetpack = (Player.wings == EquipLoader.GetEquipSlot(Mod, nameof(MeteorJetpack), EquipType.Wings));

            if (MeteoriteJetpack)
            {
                Player.flapSound = true;

                if (Player.controlJump && TranscendenceWorld.Timer % 2 == 0)
                {
                    Dust d = Dust.NewDustPerfect(Player.Center - new Vector2((Player.width - 2) * Player.direction, -8), DustID.Torch, new Vector2(-2 * Player.direction, 0.5f), 0, default, 2f);
                    d.noGravity = true;

                    Dust d2 = Dust.NewDustPerfect(Player.Center + new Vector2((Player.width - 6) * Player.direction, 8), DustID.Torch, new Vector2(-2 * Player.direction, 0.5f), 0, default, 2f);
                    d2.noGravity = true;
                }
                if (Player.wingFrame == 3 && TranscendenceWorld.Timer % 4 == 0)
                {
                    SoundEngine.PlaySound(SoundID.Item13, Player.Center);
                }
            }

            int increasedWidth = (int)(20 * ScaleMult);
            int increasedHeight = (int)((42 + (Player.mount.Active ? Player.mount.HeightBoost : 0)) * ScaleMult);

            Player.position = Player.Bottom;

            if (InsideShell == 0 && !InsideGolem && FishTrans == 0 && !Possessing && !LacewingTrans)
            {
                Player.width = increasedWidth;
                Player.height = increasedHeight;
            }
            else
            {
                if (FishTrans > 0)
                {
                    Player.width = 14;
                    Player.height = 14;
                }
                if (LacewingTrans)
                {
                    Player.width = 8;
                    Player.height = 8;
                }
                if (BeetleShield)
                {
                    Player.width = 30;
                    Player.height = 22;
                }
                if (InsideGolem)
                {
                    Player.width = 186;
                    Player.height = 172;
                }
                if (Possessing)
                {
                    Player.width = PossessedNPC.width;
                    Player.height = PossessedNPC.height;

                }
            }

            if (!Possessing)
                Player.Bottom = Player.position;
            else Player.Bottom = PossessedNPC.Bottom;
        }
        public override bool CanBeHitByNPC(NPC npc, ref int cooldownSlot)
        {
            if (Player.GetModPlayer<Dashes>().dashBounce > 0 && Player.GetModPlayer<Dashes>().ramTimer > 0)
                return false;

            return base.CanBeHitByNPC(npc, ref cooldownSlot);
        }
        public override bool CanBeHitByProjectile(Projectile proj)
        {
            return base.CanBeHitByProjectile(proj);
        }
        public override void PostHurt(HurtInfo info)
        {
            HitTimer = 5;
            if (InsideShell == 0 && !InsideGolem)
            {
                float decreases = 0;

                Focus -= decreases;
            }

            if (SilkyEgg > 0)
            {
                if (Player.mount.Active)
                    Player.AddBuff(BuffID.Dazed, 30 * SilkyEgg);
                else Player.AddBuff(BuffID.Webbed, 15 * SilkyEgg);
            }

            if (Possessing)
                SoundEngine.PlaySound(PossessedNPC.HitSound, Player.Center);

            if (ShowEolTransform && !LacewingTrans)
                SoundEngine.PlaySound(SoundID.FemaleHit, Player.Center);

            if (FishTrans > 0 || LacewingTrans)
                SoundEngine.PlaySound(SoundID.NPCHit1 with { MaxInstances = 0}, Player.Center);

            if (InsideShell > 0 && TurtleShield)
                SoundEngine.PlaySound(SoundID.NPCHit24, Player.Center);
            if (InsideShell > 0 && BeetleShield)
            {
                ShellCrumble += 15;
                ShellCrumbleCD = 0;
                SoundEngine.PlaySound(SoundID.NPCHit45, Player.Center);
            }
            if (InsideShell > 0 && (OrangeShell || PurpleShell))
                SoundEngine.PlaySound(SoundID.NPCHit38, Player.Center);
            if (InsideGolem)
                SoundEngine.PlaySound(SoundID.NPCHit4, Player.Center);
        }
        public override void ModifyHitByNPC(NPC npc, ref HurtModifiers modifiers)
        {
        }

        public override void HideDrawLayers(PlayerDrawSet drawInfo)
        {
            if (Possessing && !Player.dead || NucleusConsumed > 0)
            {
                drawInfo.hideEntirePlayer = true;
                foreach (var layer in PlayerDrawLayerLoader.Layers)
                {
                    layer.Hide();
                }
            }
            if (FishTrans > 0 && !Player.dead)
            {
                foreach (var layer in PlayerDrawLayerLoader.Layers)
                {
                    if (layer.Name != "FishTransform") layer.Hide();
                }
            }
            if (LacewingTrans)
            {
                foreach (var layer in PlayerDrawLayerLoader.Layers)
                {
                    if (layer.Name != "LacewingTransformation") layer.Hide();
                }
            }
            if (InsideGolem && !Player.dead)
            {
                foreach (var layer in PlayerDrawLayerLoader.Layers)
                {
                    if (layer.Name != "GolemBulwark") layer.Hide();
                }
            }
            if (InsideShell > 0 && !Player.dead)
            {
                foreach (var layer in PlayerDrawLayerLoader.Layers)
                {
                    if (layer.Name != "MountFront" && layer.Name != "MountBack" && layer.Name != "CaptureTheGem" && layer.Name != "BeetleBuff" && layer.Name != "IceBarrier" && layer.Name != "EyebrellaCloud" && layer.Name != "ElectrifiedDebuffFront") layer.Hide();
                }
            }
        }
        public override void ModifyHurt(ref Player.HurtModifiers modifiers)
        {
            if (StardustShield && ParryTimer > 0)
                modifiers.FinalDamage *= 0.5f;

            if (CultistForcefield && ParryTimer > 0)
                modifiers.FinalDamage *= 0.375f;

            ParryTimer = 0;

            if (SpaceSuffocation && Main.rand.NextBool(4))
                modifiers.FinalDamage *= 2f;

            if (OcramBuff)
                modifiers.FinalDamage *= 1.5f;

            if (EmpoweringTabletEquipped)
                modifiers.FinalDamage *= 1.25f;

            if (Possessing)
                modifiers.DisableSound();

            if (FishTrans > 0)
                modifiers.DisableSound();

            if (LacewingTrans)
                modifiers.DisableSound();

            if (ShowEolTransform)
            {
                for (int i = 0; i < 10; i++) Dust.NewDust(Player.Center, 16, 16, DustID.MushroomSpray);
                modifiers.DisableDust();
                modifiers.DisableSound();
            }

            if (InsideGolem)
            {
                modifiers.DisableSound();
                modifiers.FinalDamage *= 0.33f;
                modifiers.Knockback *= 0f;
            }

            if (InsideShell > 0)
            {
                modifiers.DisableSound();
                float amount = BeetleShield ? 0.2f : TurtleShield ? 0.5f : OrangeShell ? 0.6f : 0.7f;
                if (BeetleShield && ShellCrumble > 0)
                    amount += (ShellCrumble / 100f);
                modifiers.FinalDamage *= amount;
            }
        }
        public override void PreUpdate()
        {
            if (ZoneVolcano)
            {
                Filters.Scene.Activate("TranscendenceMod:HotScreen");
                HotTimer = 15;
            }

            if (ParryTimer < 0)
                ParryTimer = 0;

            if (ParryTimerCD < ParryCD)
            {
                if (!BrokenShield && Focus >= ParryFocusCost)
                    ParryTimerCD++;
            }
            else ParryTimerCD = ParryCD + 1;
            if (ParryTimer > 0)
                ParryTimer -= 1;

            if (CosmoShardTimer > 0)
                CosmoShardTimer--;

            if (HitTimer > 0)
                HitTimer--;

            if (ZoneSerpentMonolith > 0)
            {
                Terraria.Graphics.Effects.Filters.Scene.Activate("TranscendenceMod:ColdScreen");
                Main.LocalPlayer.GetModPlayer<TranscendencePlayer>().ColdTimer = 15;
            }
            else
            {
                SkyManager.Instance.Deactivate("TranscendenceMod:FrostSky");
                Filters.Scene.Deactivate("TranscendenceMod:ColdScreen");
            }

            if (ZoneSeraphMonolith)
                SkyManager.Instance.Activate("TranscendenceMod:SpaceMonolith", Player.Center);
            else SkyManager.Instance.Deactivate("TranscendenceMod:SpaceMonolith");


            if (SeraphMonolithTimer > 0)
                SeraphMonolithTimer--;

            if (ZoneLandSiteTimer > 0)
                ZoneLandSiteTimer--;

            if (CannotUseItemsTimer > 0)
                CannotUseItemsTimer--;

            if (ZoneSpaceTempleTimer > 0)
                ZoneSpaceTempleTimer--;

            if (EolBurn)
            {
                if (Main.rand.NextBool(3))
                {
                    Dust.NewDust(Player.position, Player.width, Player.height, ModContent.DustType<Rainbow>(), 0, 0, 0, Main.hslToRgb(Main.rand.NextFloat(), 1f, 0.5f, byte.MaxValue), 1.45f);
                }
            }
        }
        public override bool CanHitNPC(NPC target)
        {
            if (target.GetGlobalNPC<TranscendenceNPC>().PossessionAvaivable || target.GetGlobalNPC<TranscendenceNPC>().Possessed) return false;
            return base.CanHitNPC(target);
        }
        public void ExpendFocus(Player player, float am, float delay)
        {
            player.GetModPlayer<TranscendencePlayer>().Focus -= am;
            player.GetModPlayer<TranscendencePlayer>().FocusRegenDelay = delay;
        }
        public void SetStats()
        {
            List<Point> Edges = new List<Point>();
            Collision.GetEntityEdgeTiles(Edges, Player);
            foreach (Point touchedPoint in Edges)
            {
                Tile tile = Framing.GetTileSafely(touchedPoint);
                if (tile != null && tile.HasTile && !tile.IsActuated)
                {
                    if (tile.TileType == ModContent.TileType<VolcanicStone>() && !Player.fireWalk && !LacewingTrans)
                        Player.AddBuff(BuffID.Burning, 60);

                    if ((tile.TileType == ModContent.TileType<SpaceCrystal>() || tile.TileType == ModContent.TileType<ModMeteorite>()) && !LacewingTrans)
                        Player.AddBuff(ModContent.BuffType<SpaceDebuff>(), 5);

                }
            }

            int bulletam = 0;
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile p = Main.projectile[i];
                if (p != null && p.active && p.owner == Player.whoAmI && p.GetGlobalProjectile<TranscendenceProjectiles>().Timer < 30 && p.type == ModContent.ProjectileType<TwentyTwoHoming>())
                {
                    bulletam++;
                }
            }
            if (bulletam > 110 && !Player.GetModPlayer<ModAchievementsHelper>().TwentyTwoUnlock)
            {
                ModAchievementsHelper.CompleteChallenge(Player, TaskIDs.TwentyTwoChallenge);
            }

            if (LegendarySwordTimer > 0)
                ShieldID = ModContent.ItemType<LegendarySword>();

            if (FrostBite)
                Player.statDefense -= 20;

            if (ConsumedInferno)
                MaxFocus += 15f;

            if (EverglowingCrownEquipped)
                MaxFocus -= 25f;

            if (DualBall)
            {
                float increase = 0.3f * FocusGatherSpeed;
                FocusGatherSpeed = FocusGatherSpeed + increase;
            }

            float MoveMult = LacewingTrans ? 1f : Player.velocity.Length() > 10f ? 0f : MathHelper.Lerp(1f, 0f, Player.velocity.Length() / 10f);
            if (SpaceBossDot && FocusGatherSpeed > 0f)
                FocusGatherSpeed *= -1f;

            if (Focus < MaxFocus)
            {
                if (FocusRegenDelay <= 0f)
                    Focus += FocusGatherSpeed * MoveMult;
            }
            else Focus = MaxFocus;

            if (Focus <= ParryFocusCost)
                Player.AddBuff(ModContent.BuffType<ShieldBreak>(), 2);

            if (Focus < 0f)
                Focus = 0f;

            if (ExtraTerrestrialEffects)
            {
                Player.statManaMax2 += 150;
                Player.gravity *= 0.8f;
            }

            if (GiantSlayer > 0)
            {
                Player.GetArmorPenetration(DamageClass.Generic) += 15 * GiantSlayer;
                float am = 0.125f;

                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC n = Main.npc[i];
                    if (n != null && n.active && n.lifeMax > (Player.statLifeMax2 * 5) && !n.friendly && n.Distance(Player.Center) < 1000)
                    {
                        Player.velocity += Player.DirectionTo(n.Center) * (am * GiantSlayer);
                    }
                }
            }

            if (EverglowingCrownEquipped)
            {
                if ((InfectionAbility || LacewingTrans) && Player.controlMount && LacewingTransCD == 0)
                {
                    LacewingTrans = !LacewingTrans;

                    if (LacewingTransCD == 0)
                    {
                        for (int i = 0; i < 32; i++)
                            Dust.NewDustPerfect(Player.Center, ModContent.DustType<ArenaDust>(),
                                new Vector2(0, 5f + (float)Math.Sin(i) * 2f).RotatedBy(MathHelper.TwoPi * i / 32f + MathHelper.PiOver4 / 2f), 0, Main.hslToRgb(i / 32f, 1f, 0.5f), 3f);
                    }

                    LacewingTransCD = 30;
                }

                if (LacewingTrans)
                {
                    if (FishNeck && Player.wet)
                        Player.gills = true;

                    if (Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftShift))
                        Player.velocity *= 0.8f;

                    FocusGatherSpeed *= 2f;
                    CannotUseItems = true;
                    CannotUseItemsTimer = 5;

                    Player.statDefense *= 0f;
                    Player.lavaImmune = true;
                    Player.shimmerImmune = true;
                    Player.mount.SetMount(ModContent.MountType<LacewingTransformationMount>(), Player);

                    if (TranscendenceWorld.Timer % 10 == 0)
                    {
                        LacewingFrame += 24;
                        if (LacewingFrame > 48)
                            LacewingFrame = 0;
                    }
                }

                bool Danger = false;
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC n = Main.npc[i];
                    if (n != null && n.active && n.Distance(Player.Center) < 750 && !n.friendly && n.lifeMax > 5)
                        Danger = true;
                }
                if (Danger && ++EverglowingSunCD % (LacewingTrans ? 10 : 20) == 0)
                {
                    Projectile.NewProjectile(Player.GetSource_FromAI(), Player.Center, Player.DirectionTo(Main.MouseWorld) * 6f, ModContent.ProjectileType<EmpressSun>(), 200, 2f, Player.whoAmI);
                }
                if (TranscendenceWorld.InfectionAccessoryKeyBind.JustPressed)
                {
                    int laser = ModContent.ProjectileType<EmpressLaser>();
                    if (Player.ownedProjectileCounts[laser] == 0 && (!CannotUseItems || LacewingTrans))
                    {
                        int dmg = 280;
                        for (int i = 0; i < (Player.maxMinions + Player.maxTurrets); i++)
                            dmg += 5;

                        Projectile.NewProjectile(Player.GetSource_FromAI(), Player.Center, Player.DirectionTo(Main.MouseWorld) * 6f, laser, dmg, 2f, Player.whoAmI);
                    }
                }
            }
            else LacewingTrans = false;

            Tile tile2 = Main.tile[(int)Player.Center.X / 16, (int)Player.Center.Y / 16];

            bool cond = Player.wet && Player.controlUp && FishNeck && !LacewingTrans;
            if (cond)
            {
                if (FishTrans == 0)
                {
                    for (int i = 0; i < 32; i++)
                        Dust.NewDustPerfect(Player.Center, ModContent.DustType<ArenaDust>(), new Vector2(0, 5f + (float)Math.Sin(i) * 2f).RotatedBy(MathHelper.TwoPi * i / 32f + MathHelper.PiOver4 / 2f), 0, Color.Blue, 3f);
                }

                FishTrans = 5;
                FullRotResetCD = 5;

                CannotUseItems = true;
                CannotUseItemsTimer = 5;

                Player.gills = true;

                Player.mount.Dismount(Player);
                Player.velocity.X *= 0f;

                if (TranscendenceWorld.Timer % 10 == 0)
                {
                    FishFrame += 16;
                    if (FishFrame > 32)
                        FishFrame = 0;
                }

                if (Player.controlUp)
                {
                    Player.velocity = Player.DirectionTo(Main.MouseWorld) * 5f;
                    Player.fullRotationOrigin = new Vector2(8);
                    Player.fullRotation = Player.DirectionTo(Main.MouseWorld).ToRotation() + MathHelper.PiOver2;
                    Player.direction = Main.MouseWorld.X > Player.Center.X ? 1 : -1;
                }
                else
                {
                    Player.velocity *= 0.9f;
                    Player.gravity *= 0f;
                }

                Player.controlLeft = false;
                Player.controlRight = false;
                Player.controlDown = false;
                Player.controlUp = false;
                Player.controlJump = false;
                Player.wingTime = 0;
                Player.rocketTime = 0;
            }

            if (NucleusDeathAnim > 0 && !Player.dead)
                NucleusDeathAnim--;

            if (DragonScales > 0 && Player.velocity.Length() > 0f)
            {
                if (++DragonScalesCD > 5)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        int p = Projectile.NewProjectile(Player.GetSource_FromAI(),
                            Player.position + new Vector2(Main.rand.NextFloat(0f, Player.width), Main.rand.NextFloat(0f, Player.height)),
                            Main.rand.NextVector2Circular(0.5f, 0.5f) * (float)DragonScales, ModContent.ProjectileType<DragonFlame>(), 75 + (DragonScales * 25), 0f, Player.whoAmI);
                        Main.projectile[p].timeLeft = 45 + (DragonScales * 90);
                    }
                    DragonScalesCD = 0;
                }
            }

            if (IsBlind > 0)
            {
                Filters.Scene.Activate("TranscendenceMod:Blindness");
                Filters.Scene["TranscendenceMod:Blindness"].GetShader().UseOpacity(1f);
            }
            else
            {
                if (Filters.Scene["TranscendenceMod:Blindness"].IsActive())
                    Filters.Scene["TranscendenceMod:Blindness"].Deactivate();
            }

            if (SilkyEgg > 0)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (Player.armor[i] != null && Player.armor[i].TryGetGlobalItem(out ModifiersItem item) && item != null && item.Modifier == ModifierIDs.Silky)
                    {
                        if (SilkyCD[i] < (15 * 60))
                            SilkyCD[i]++;
                        else
                        {
                            int dmg = 155;
                            if (NPC.downedGolemBoss)
                                dmg = 205;
                            if (NPC.downedMoonlord)
                                dmg = 295;
                            if (Downed.Contains(Bosses.ProjectNucleus))
                                dmg = 360;

                            SoundEngine.PlaySound(SoundID.DD2_OgreSpit, Player.Center);

                            float ai = 0f;
                            if (Luminosity >= 0.3f)
                            {
                                ai = 1f;

                                dmg = 185;
                                if (NPC.downedGolemBoss)
                                    dmg = 235;
                                if (NPC.downedMoonlord)
                                    dmg = 325;
                                if (Downed.Contains(Bosses.ProjectNucleus))
                                    dmg = 400;
                            }

                            int p = Projectile.NewProjectile(Player.GetSource_FromAI(),
                                Player.Center, new Vector2(-5f * Player.direction, 2f),
                                ModContent.ProjectileType<MothBaby>(), dmg, 2f, Player.whoAmI, ai);
                            if (ai != 1f)
                                Main.projectile[p].scale += Luminosity;
                            else
                            {
                                Main.projectile[p].scale = 1f;
                                Main.projectile[p].width = 156;
                                Main.projectile[p].height = 120;
                            }

                            SilkyCD[i] = 0;
                        }
                    }
                }
            }
            else
            {
                SilkyCD[0] = 0;
                SilkyCD[1] = 0;
                SilkyCD[2] = 0;
            }

            if (NucleusLens)
            {
                Filters.Scene.Activate("TranscendenceMod:RetLens");

                Player.GetDamage(DamageClass.Ranged) += 0.1f;
                if (NucleusLensKeybind)
                {
                    cameraModifier = true;
                    cameraPos = Vector2.Lerp(cameraPos, Vector2.Lerp(Player.Center, Main.MouseWorld, 0.5f), 0.25f);

                    Player.GetDamage(DamageClass.Ranged) += 0.25f;
                    Player.GetCritChance(DamageClass.Ranged) += 25f;
                    Filters.Scene["TranscendenceMod:RetLens"].GetShader().UseOpacity(1f);
                }
                else Filters.Scene["TranscendenceMod:RetLens"].GetShader().UseOpacity(0f);
            }
            else Filters.Scene.Deactivate("TranscendenceMod:RetLens");

            EvasionStoneMaxTimer = 600;
            if (Main.hardMode)
                EvasionStoneMaxTimer = 900;

            if (ApolloHelmet)
            {
                Player.GetDamage(DamageClass.Generic) += 0.1f;
                Player.buffImmune[ModContent.BuffType<SpaceDebuff>()] = true;
            }

            if (SuckedIn)
            {
                Player.fullRotation += MathHelper.ToRadians(16);
                FullRotResetCD = 5;
                ScaleResetTimer = 15;
                
                if (ScaleMult < 0.1f)
                    Player.KillMe(PlayerDeathReason.ByCustomReason(NetworkText.FromKey("Mods.TranscendenceMod.Messages.Death.BlackHole", Player.name)), 1000, Player.direction);

                if (ScaleMult > 0f)
                    ScaleMult -= 0.025f;
            }

            if (FishronPerceptionAcc && Player.wet)
            {
                Player.ignoreWater = true;
                Player.shimmerImmune = true;

                int b = ModContent.ProjectileType<FishronBubble>();
                if (Player.ownedProjectileCounts[b] == 0)
                    Projectile.NewProjectile(Player.GetSource_FromAI(), Player.Center, Vector2.Zero, b, 0, 0, Player.whoAmI);
            }

            if (InsideGolem || InsideShell > 0)
            {
                CannotUseItems = true;
                CannotUseItemsTimer = 15;
            }

            if (!LihzardianBulwarkEquipped)
                InsideGolem = false;

            if (GolemCrushTimer > 0)
                GolemCrushTimer--;

            if (InsideGolem)
            {
                bool SLAM = GolemCrushTimer > 10;
                if (GolemJumpCD > 0)
                    GolemJumpCD--;
                if (GolemDustCD > 0)
                    GolemDustCD--;

                bool condy = GolemJumpCD > 58 && Collision.SolidCollision(Player.BottomLeft, Player.width, 2, true) && GolemDustCD == 0;

                if (condy || SLAM)
                {
                    for (int i = 0; i < Main.maxNPCs; i++)
                    {
                        NPC npc = Main.npc[i];
                        if (npc != null && npc.active && npc.type != NPCID.DD2EterniaCrystal && !npc.townNPC && !npc.dontTakeDamage)
                        {
                            if (Player.dontHurtCritters && npc.CountsAsACritter)
                                return;

                            for (int j = 0; j < 32; j++)
                            {
                                for (int k = 0; k < 4; k++)
                                {
                                    Vector2 pos = Vector2.Lerp(Player.BottomRight - new Vector2(SLAM ? 300 : 75, 150 - k * 50f), Player.BottomLeft + new Vector2(SLAM ? 300 : 75, -(150 - k * 50f)), j / 32f);

                                    if (npc.Distance(pos) < 128 && npc.GetGlobalNPC<TranscendenceNPC>().HitCD == 0)
                                    {
                                        npc.SimpleStrikeNPC((npc.realLife != -1 ? 333 : 1111) * (SLAM ? 3 : 1), npc.direction, Main.rand.Next(100) < Player.GetTotalCritChance(DamageClass.Generic), 10f, DamageClass.Generic, true, Player.luck, false);
                                        npc.GetGlobalNPC<TranscendenceNPC>().HitCD = 20;
                                        npc.GetGlobalNPC<TranscendenceNPC>().NotPossessTimer = 999;
                                    }
                                }
                            }
                        }
                    }
                }

                if (condy)
                {
                    SoundEngine.PlaySound(SoundID.Item14, Player.Bottom);

                    Main.instance.CameraModifiers.Add(new PunchCameraModifier(new Vector2(Main.rand.Next(-10, 10)),
                        new Vector2(Main.rand.NextFloatDirection()), SLAM ? 250 : 15, 15, 15, -1, null));

                    for (int i = 0; i < 32; i++)
                    {
                        Vector2 pos = Vector2.Lerp(Player.BottomLeft, Player.BottomRight, i / 32f);

                        Gore gore = Gore.NewGoreDirect(Player.GetSource_FromAI(), pos, new Vector2(Main.rand.Next(-15, 15), Main.rand.Next(5, 15)), Main.rand.Next(61, 64), Main.rand.NextFloat(1f, 1.75f) * (SLAM ? 2.5f : 1f));
                        gore.velocity.X = Main.rand.NextFloat(-6f, 6f);
                        gore.velocity.Y = Main.rand.NextFloat(-6f, 2f);
                    }
                    GolemDustCD = 30;
                }

                if (GolemJumpCD != 0)
                    Player.controlJump = false;

                if (Player.velocity.Y == 0)
                {
                    Player.velocity.X = 0;
                    Player.position.X = Player.oldPosition.X;
                }
                else
                {
                    Player.controlJump = false;
                    GolemJumpCD = 60;

                    if (Player.controlDown)
                    {
                        GolemCrushTimer += 5;
                        Player.velocity.Y += 25;
                        Player.maxFallSpeed *= 5;
                    }
                }

                Player.mount.Dismount(Player);
                Player.channel = false;
                Player.wingTime = 0;
                Player.wingTimeMax = 0;
                Player.rocketTime = 0;

                Player.buffImmune[BuffID.OnFire] = true;
                Player.buffImmune[BuffID.OnFire3] = true;
                Player.buffImmune[BuffID.Frostburn] = true;
                Player.buffImmune[BuffID.Frostburn2] = true;

                Player.npcTypeNoAggro[NPCID.Lihzahrd] = true;
                Player.npcTypeNoAggro[NPCID.LihzahrdCrawler] = true;
                Player.npcTypeNoAggro[NPCID.FlyingSnake] = true;
            }

            if (BeetleShield)
            {
                if (InsideShell > 0)
                {
                    Player.npcTypeNoAggro[NPCID.CochinealBeetle] = true;
                    Player.npcTypeNoAggro[NPCID.CyanBeetle] = true;
                    Player.npcTypeNoAggro[NPCID.LacBeetle] = true;

                    Player.npcTypeNoAggro[NPCID.Lihzahrd] = true;
                    Player.npcTypeNoAggro[NPCID.LihzahrdCrawler] = true;
                }
            }

            if (SpaceSuffocation)
            {
                Player.GetDamage(DamageClass.Generic) *= 0.75f;
                if (Player.lifeRegen > 0)
                    Player.lifeRegen /= 2;
            }

            if (Jolly > 0)
            {
                int score = (int)(FrostMoonHS / 2000f);
                if (FrostMoonHS > 20000)
                    score = 10;

                for (int i = 0; i < score; i++)
                    Player.statLifeMax2 += (int)((Player.statLifeMax2 / 200f) * Jolly);
                if (!Player.ZoneSnow)
                    Player.GetDamage(DamageClass.Generic) -= (0.1f * Jolly);
            }

            if (VoidNecklaceAlpha > 1f)
                VoidNecklaceAlpha = 1f;

            if (VoidNecklaceAcc && Player.statLife >= Player.statLifeMax2)
            {
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    bool eligibleForSuck = !npc.dontTakeDamage && !npc.immortal && !npc.friendly && npc.type != NPCID.TargetDummy && npc.lifeMax < 17500 && !npc.boss && npc.type != ModContent.NPCType<Blackhole>();

                    if (npc != null && npc.active && npc.Distance(Player.Center) < 250 && eligibleForSuck && !(npc.lifeMax == 5 && Player.dontHurtCritters))
                    {
                        npc.velocity = npc.DirectionTo(Player.Center) * 12.5f;
                        npc.GetGlobalNPC<TranscendenceNPC>().SuckedInNecklace = 5;
                        VoidNecklaceWithinRange = true;

                        SoundEngine.PlaySound(SoundID.Item117 with { MaxInstances = 0, Volume = 0.125f, Pitch = -1f }, Player.Center);

                        if (VoidNecklaceAlpha < 1f)
                            VoidNecklaceAlpha = MathHelper.Lerp(VoidNecklaceAlpha, 1f, 0.25f);

                        if (npc.Distance(Player.Center) < 35 && eligibleForSuck && !(npc.lifeMax == 5 && Player.dontHurtCritters))
                        {
                            npc.StrikeInstantKill();
                            if (!Player.HasBuff(BuffID.WellFed) && !Player.HasBuff(BuffID.WellFed2) && !Player.HasBuff(BuffID.WellFed3))
                                Player.AddBuff(BuffID.WellFed, (npc.width + npc.height) * 3);
                        }
                            //npc.SimpleStrikeNPC(VoidNecklaceDamage * 10, 0, false, 0, DamageClass.Generic, true, Player.luck);
                    }
                }
            }
            float fadeSpeed = MathHelper.Lerp(0.05f, 0.00005f, VoidNecklaceAlpha);

            if (fadeSpeed < 0)
                fadeSpeed = -fadeSpeed;

            if (!VoidNecklaceWithinRange && VoidNecklaceAlpha > 0f)
                VoidNecklaceAlpha = MathHelper.Lerp(VoidNecklaceAlpha, 0f, fadeSpeed);

            if (DragonClawBuff)
            {
                SoundEngine.PlaySound(SoundID.DD2_BetsyWindAttack with { MaxInstances = -1 }, Player.Center);

                TranscendenceUtils.DustRing(Player.Center, 20, DustID.RainCloud, 25, Color.White, 2);

                InfFlight = true;
                Player.velocity = Player.DirectionTo(Main.MouseWorld) * 25;

                FullRotResetCD = 5;
                Player.fullRotation += MathHelper.ToRadians(32);

                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];

                    if (npc != null && npc.active && !npc.friendly && !npc.dontTakeDamage && npc.Distance(Player.Center) < 300)
                    {
                        npc.SimpleStrikeNPC(200, -npc.direction, false, 7, DamageClass.Melee, true, Player.luck);
                    }
                }
            }

            if (RingOfBravery)
            {
                Player.maxMinions += 1;
                Player.GetAttackSpeed(DamageClass.SummonMeleeSpeed) += 0.25f;
            }

            VampireHealAmount = (int)(Player.statLifeMax2 * 0.2f) - (Player.statDefense / 2);

            if (InsideShell > 0 && (TurtleShield || BeetleShield))
            {
                if (!BeetleShield) Player.noKnockback = true;
                Player.channel = false;
            }
            if (ShellCrumble > 0 && BeetleShield)
            {
                if (ShellCrumbleCD < 2700)
                    ShellCrumbleCD++;
                else
                {
                    ShellCrumbleCD = 0;
                    ShellCrumble = 0;
                }
            }

            if (ShellCrumble < 0)
                ShellCrumble = 0;

            int stargazerLaser = ModContent.ProjectileType<StargazerLaserFriendly>();
            if (Stargazer && Player.ownedProjectileCounts[stargazerLaser] == 0)
            {
                Projectile.NewProjectile(Player.GetSource_FromAI(), Player.Center, new Vector2(0, 5), stargazerLaser, stargazerDamage, 0f, Player.whoAmI); 
            }

            int maw = ModContent.ProjectileType<FrozenMawProj>();
            if (FrozenMaw && Player.ownedProjectileCounts[maw] == 0)
            {
                for (int i = 0; i < 8; i++)
                    Projectile.NewProjectile(Player.GetSource_FromAI(), Player.Center, Vector2.Zero, maw, FrozenMawDamage, 0f, Player.whoAmI, 0f, i, 8f);
            }

            if (Luminosity > 0)
                Lighting.AddLight(Player.Center, 1f * Luminosity, 0.6f * Luminosity, 0.25f * Luminosity);

            if (InsideShell > 0 && Player.mount.Type == ModContent.MountType<JungleShell1>() && !TurtleShield)
            {
                Player.mount.Dismount(Player);
                TurtleCD = 60;
            }
            if (InsideShell > 0 && Player.mount.Type == ModContent.MountType<JungleShell2>() && !BeetleShield)
            {
                Player.mount.Dismount(Player);
                BeetleCD = 60;
            }
            if (InsideShell > 0 && (Player.mount.Type == ModContent.MountType<OrangeShellMount>() && !OrangeShell || (Player.mount.Type == ModContent.MountType<PurpleShellMount>() && !PurpleShell)))
            {
                Player.mount.Dismount(Player);
                GiantShellCD = 60;
            }

            if (LihzardianBulwarkEquipped && ShieldGuard && Player.controlUp && !InsideGolem && GolemCD == 0)
            {
                InsideGolem = true;
                Player.Center -= new Vector2(186 / 2.25f, 172 / 8f);
            }
            if (TurtleShield && ShieldGuard && Player.controlUp && InsideShell < 1 && TurtleCD == 0)
            {
                Player.AddBuff(ModContent.BuffType<TurtleShieldBuff>(), 15);
            }
            if (BeetleShield && ShieldGuard && Player.controlUp && InsideShell < 1 && BeetleCD == 0)
            {
                Player.AddBuff(ModContent.BuffType<BeetleshellBuff>(), 15);
            }
            if (OrangeShell && ShieldGuard && Player.controlUp && InsideShell < 1 && GiantShellCD == 0)
            {
                Player.AddBuff(ModContent.BuffType<OrangeShellBuff>(), 15);
            }
            if (PurpleShell && ShieldGuard && Player.controlUp && InsideShell < 1 && GiantShellCD == 0)
            {
                Player.AddBuff(ModContent.BuffType<PurpleShellBuff>(), 15);
            }

            if (Player.cursed)
            {
                CannotUseItems = true;
                CannotUseItemsTimer = 5;
            }

            if (((ModifierUINPCPos == null || !ModifierUINPCPos.active || ModifierUINPCPos.Distance(Player.Center) > 250) && !ModifierUIOnPhone || !Main.playerInventory) && ModifierApplierUIDrawing.Visible)
            {
                if (Main.netMode != NetmodeID.Server) SoundEngine.PlaySound(SoundID.MenuClose);
                ModifierApplierUIDrawing.Visible = false;
            }

            if (ProcesserTile == null || !ProcesserTile.HasTile || Player.Distance(ProcesserPos) > 175)
                ProcesserUIDrawing.Visible = false;

            if (Eternity)
            {
                Player.GetDamage(DamageClass.Generic) += Player.GetTotalCritChance(DamageClass.Generic) / 25f;
            }

            if (OcramBuff)
            {
                if (++OcramTimer % 180 == 0)
                {
                    Player.Heal(Player.statLifeMax2 / 20);
                    SoundEngine.PlaySound(new SoundStyle("TranscendenceMod/Miscannellous/Assets/Sounds/NPCs/Attack/Heartbeat"), Player.Center);
                }
            }
            if (UsingLunarGauntlet)
            {
                Player.GetKnockback(DamageClass.Melee) += 1f;
                Player.GetDamage(DamageClass.Melee) += 0.15f;
                Player.GetAttackSpeed(DamageClass.Melee) += 0.15f;
                Player.autoReuseGlove = true;
            }
            if (Possessing && PossessedNPC != null && !Player.dead)
            {
                Player.buffImmune[BuffID.Shimmer] = true;
                Player.Center = PossessedNPC.Top - new Vector2(0, 16);
                Player.mount.Dismount(Player);
                Player.ShimmerCollision(true, true, true);
                Player.stairFall = true;
                Player.controlDown = true;
                Player.aggro = -5000;
                Player.velocity = Player.oldVelocity;
            }
            if (Player.dead)
            {
                EvasionStoneGraze = 0;
                EvasionStoneTimer = EvasionStoneMaxTimer;
            }

            if (ThrowingGloveCD > 0)
                ThrowingGloveCD--;

            if (MuramasaShieldIFrames > 0)
                MuramasaShieldIFrames--;

            if (Player.wet && ZoneLandSiteTimer > 0)
            {
                Player.gills = true;
                Player.lifeRegen *= 3;

                if (ZoneLandSiteWaterTimer < 100)
                    ZoneLandSiteWaterTimer++;

                float water = (float)ZoneLandSiteWaterTimer / 100f;
                Lighting.AddLight((int)Player.Center.X / 16, (int)Player.Center.Y / 16, 0.7f * water, 0.88f * water, 0.9f * water);

                if (!Player.controlJump)
                {
                    Player.canFloatInWater = true;
                    Dust.NewDustPerfect(Player.Center + new Vector2(Main.rand.Next(-100, 100), Main.rand.Next(20, 75)),
                    ModContent.DustType<LandsiteDroplet>(), new Vector2(0, -3));
                }
            }
            else ZoneLandSiteWaterTimer = 0;

            int area = (Main.maxTilesY >= 2000 ? 460 : 390) * 16;
            int sx = TranscendenceWorld.SpaceTempleX;
            bool StargazerArmor = (Player.head == EquipLoader.GetEquipSlot(Mod, nameof(CosmicHelm), EquipType.Head));

            if (Player.position.Between(new Vector2(sx - area, 5), new Vector2(sx + area, 220 * 16)))
            {
                ZoneStar = true;
                ZoneStarTimer = 15;

                if (StarFade < 1 && !NPC.AnyNPCs(ModContent.NPCType<CelestialSeraph>()) && !TranscendenceWorld.AnyProjectiles(ModContent.ProjectileType<CelestialSeraphSummoner>()))
                    StarFade += 0.0125f;
            }
            else
            {
                if (StarFade > 0)
                    StarFade -= 0.0125f;
            }

            if (!NPC.AnyNPCs(ModContent.NPCType<CelestialSeraph>()))
                SkyManager.Instance.Deactivate("TranscendenceMod:CelestialSeraph");

            if (!AstronautHelmet && !ApolloHelmet && StarFade > 0 && ZoneStar && SeraphTileDrawingSystem.PhaseThroughTimer == 0)
                Player.AddBuff(ModContent.BuffType<CosmicSuffocation>(), 5);

            for (int i = 0; i < PortalBoxPositions.Length; i++)
            {
                if (PortalBoxPositions[i] != Vector2.Zero && Player.HasItem(ModContent.ItemType<PortalBox>()) && Main.rand.NextBool(8))
                {
                    int d = Dust.NewDust(PortalBoxPositions[i] - new Vector2(16, 48), 32, 96, ModContent.DustType<PlayerCosmicBlood>(), 0f, 0f, 0, Color.Lerp(Color.DeepSkyBlue, Color.Magenta, Main.rand.NextFloat(0f, 1f)), 2f);
                    Main.dust[d].velocity *= 0.2f;
                }
            }

            if (StarFade > 0)
            {
                if (!NPC.AnyNPCs(ModContent.NPCType<CelestialSeraph>()) && !TranscendenceWorld.AnyProjectiles(ModContent.ProjectileType<CelestialSeraphSummoner>()))
                    Player.gravity *= 0.75f;
            }

            if ((ZoneSpaceTempleTimer > 0 || ZoneStar || TranscendenceUtils.BossAlive() || CosmicSetWear || CosmicWings)
                && !Player.wet && !InfectionAbility && !EolNightDeathBomb)
                Player.gravity = Player.defaultGravity;

            if (ZoneStarTimer > 0)
                ZoneStarTimer--;
            else ZoneStar = false;

            if (Player.position.Between(new Vector2(sx - area, 50),
                new Vector2(sx + area, 505 * 16)))
            {
                if (!ZoneStar)
                {
                    ZoneLandSiteTimer = 5;
                    ZoneLandSite = true;
                }
            }
            if (Player.position.Between(new Vector2(sx - (48 * 16), 0), new Vector2(sx + (48 * 16), 120 * 16)))
            {
                if (Player.position.Between(new Vector2(sx - (32 * 16), 85 * 16), new Vector2(sx + (32 * 16), 110 * 16)))
                {
                    ZoneSpaceTemple = true;
                    ZoneSpaceTempleTimer = 5;
                }
                Player.AddBuff(BuffID.NoBuilding, 5);
                Player.noBuilding = true;
                Player.wireOperationsCooldown = 1;
            }

            if (Vampire)
            {
                CrimsonNecklaceMaxBlood = 125000;
                VampireMinTimer++;

                if (VampireBlood > CrimsonNecklaceMaxBlood)
                    VampireBlood = CrimsonNecklaceMaxBlood;

                if (CrimsonNecklaceBloodCD < 5)
                    CrimsonNecklaceBloodCD++;
            }
            if (EolNightDeathBomb)
            {
                Player.lifeRegen = (int)(Player.lifeRegen * 1.75f);
                HasEolProjectile = true;
            }
            if (NoHealing)
            {
                Player.potionDelay = 60;
                if (Player.lifeRegen > 0)
                    Player.lifeRegen = 0;
                Player.moonLeech = true;
            }

            if (InfFlight) Player.wingTime = Player.wingTimeMax;

            if (DeathBombTime > 0)
                DeathBombTime--;

            if (RocketAcc != 0 && RocketCD > 0)
                RocketCD--;
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (CultScrollsEquipped > 0)
            {
                if (target.boss)
                    modifiers.NonCritDamage -= (0.15f * CultScrollsEquipped);
                else modifiers.FinalDamage += (0.15f * CultScrollsEquipped);
            }

            modifiers.CritDamage += CritDamage;
        }
        public bool CanParry() => !CannotUseItems && !EmpoweringTabletEquipped && !QuestBookUIDrawing.Visible;
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (TranscendenceWorld.ArmorSetBonus.JustPressed) ArmorKeybind = true;
            if (TranscendenceWorld.ArmorSetBonus.JustReleased) ArmorKeybind = false;

            if (Player.controlUseItem) LeftClicking = true;
            if (Player.releaseUseItem) LeftClicking = false;

            if (TranscendenceWorld.RetLensKeybind.JustPressed) NucleusLensKeybind = !NucleusLensKeybind;

            /*Parrying*/
            if (TranscendenceWorld.Guard.JustPressed && HasParry && ParryTimer == 0)
            {
                /*Deactivate Golem Transformation*/
                if (InsideGolem)
                {
                    InsideGolem = false;
                    GolemCD = 600;
                }

                if (InsideShell > 0)
                {
                    Player.mount.Dismount(Player);
                    Player.AddBuff(ModContent.BuffType<ShieldBreak>(), 120);

                    if (BeetleShield) BeetleCD = 60;
                    if (TurtleShield) TurtleCD = 60;
                    if (OrangeShell || PurpleShell) GiantShellCD = 60;
                }

                if (CanParry() && Focus >= ParryFocusCost && !BrokenShield && Player.ownedProjectileCounts[ModContent.ProjectileType<ParryVisual>()] == 0)
                {
                    ShieldGuard = true;
                    ParryTimer = 15;

                    SoundEngine.PlaySound(SoundID.MaxMana, Player.Center);
                    Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, Vector2.Zero, ModContent.ProjectileType<ParryVisual>(), 0, 0, Player.whoAmI, ShieldID);
                }
            }
            if (TranscendenceWorld.Guard.JustReleased)
                ShieldGuard = false;

            /* Infection Accessories */
            if (TranscendenceWorld.InfectionAccessoryKeyBind.JustPressed)
            {
                InfectionAbility = true;

                if (VampireBlood >= CrimsonNecklaceMaxBlood && Vampire && VampireMinTimer >= VampireMinTime)
                {
                    Player.Heal(VampireHealAmount);
                    VampireMinTimer = 0;
                    VampireBlood = 0;
                }
            }
            if (TranscendenceWorld.InfectionAccessoryKeyBind.JustReleased)
            {
                EolTimer = 0;
                InfectionAbility = false;
            }

            /*Dashing*/
            Player.TryGetModPlayer(out Dashes dashes);
            if (TranscendenceWorld.HyperDash.JustPressed)
                HyperDashKeybind = true;
            if (TranscendenceWorld.HyperDash.JustReleased)
            {
                OCoreChargeTimer = 0;
                HyperDashKeybind = false;
            }
        }
        public override void ModifyScreenPosition()
        {
            /* Camera Modification*/
            if (cameraModifier)
            {
                Main.screenPosition = new Vector2(cameraPos.X - Main.screenWidth / 2, cameraPos.Y - Main.screenHeight / 2);
            }
        }
        public override void ModifyFishingAttempt(ref FishingAttempt attempt)
        {
            /*Pearl Modifier, Upgrades fishing attempts*/
            if (PearlMod && attempt.rare && Main.rand.NextBool(3))
            {
                for (int i = 0; i < Main.maxProjectiles; i++)
                {
                    Projectile p = Main.projectile[i];
                    if (p != null && p.active && p.owner == Player.whoAmI && p.aiStyle == ProjAIStyleID.Bobber)
                    {
                        for (int j = 0; j < 8; j++)
                        {
                            Dust.NewDustPerfect(p.Center, ModContent.DustType<ArenaDust>(), new Vector2(0, 7.5f).RotatedBy(MathHelper.TwoPi * j / 8), 0, Color.DeepSkyBlue, 1.5f);
                        }
                        SoundEngine.PlaySound(SoundID.AchievementComplete, Player.Center);
                    }
                }
                attempt.legendary = true;
            }

            /*Magnet Modifier, Increases crate chance*/
            if (UsingCrateMagnet && !attempt.crate)
            {
                if (Main.rand.NextBool(4))
                {
                    for (int i = 0; i < Main.maxProjectiles; i++)
                    {
                        Projectile p = Main.projectile[i];
                        if (p != null && p.active && p.owner == Player.whoAmI && p.aiStyle == ProjAIStyleID.Bobber)
                        {
                            TranscendenceUtils.DustRing(p.Center, 10, ModContent.DustType<ArenaDust>(), 2, Color.Red, 1);
                            SoundEngine.PlaySound(SoundID.MaxMana, Player.Center);
                        }
                    }
                    attempt.crate = true;
                }
            }
        }
        public override void CatchFish(FishingAttempt attempt, ref int itemDrop, ref int npcSpawn, ref AdvancedPopupRequest sonar, ref Vector2 sonarPosition)
        {
            /*Add Fishing loot*/
            if (ZoneLandSite || ZoneStar)
            {
                if (attempt.common)
                {
                    List<int> commonItems = new List<int>()
                    {
                       ModContent.ItemType<PulverizedPlanet>(),
                       ModContent.ItemType<AetherRootItem>()
                    };

                    itemDrop = commonItems[Main.rand.Next(commonItems.Count)];
                }

                if (attempt.uncommon)
                {
                    List<int> uncommonItems = new List<int>()
                    {
                        ItemID.Meteorite,
                        ModContent.ItemType<OrbitalFish>(),
                        ModContent.ItemType<BlackholeFish>(),
                        ModContent.ItemType<CosmicJelly>()
                    };

                    if (Main.hardMode)
                        uncommonItems.Add(ModContent.ItemType<CrystalItem>());

                    itemDrop = uncommonItems[Main.rand.Next(uncommonItems.Count)];
                }

                if (attempt.rare)
                {
                    List<int> rareItems = new List<int>()
                    {
                        ItemID.StarinaBottle,
                        ItemID.FragmentSolar,
                        ItemID.FragmentVortex,
                        ItemID.FragmentNebula,
                        ItemID.FragmentStardust
                    };

                    itemDrop = rareItems[Main.rand.Next(0, 5)];
                }

                if (attempt.crate && attempt.rare)
                {
                    int item = ModContent.ItemType<CrystalCrate>();
                    if (Main.hardMode)
                        item = ModContent.ItemType<SeraphicCrate>();
                    itemDrop = item;
                }
            }
        }
        public override void OnHitByNPC(NPC npc, HurtInfo hurtInfo)
        {
            base.OnHitByNPC(npc, hurtInfo);

            if (PurpleShell && InsideShell > 0)
            {
                int dmg2 = (int)(npc.damage * 1.33f);
                int dmg = dmg2 > 400 ? 532 : dmg2;
                npc.SimpleStrikeNPC(dmg, -npc.direction, false, 3f, DamageClass.Generic, true, Player.luck, false);
            }

            if (TurtleShield && InsideShell > 0)
            {
                int dmg = (npc.damage * 2) > 1500 ? 1500 : npc.damage * 2;
                npc.SimpleStrikeNPC(dmg, -npc.direction, false, 7f, DamageClass.Generic, true, Player.luck, false);
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (target.CanBeChasedBy() && Vampire && VampireBlood < CrimsonNecklaceMaxBlood && Player.Distance(target.Center) < 1000 && CrimsonNecklaceBloodCD >= 5)
            {
                VampireBlood += damageDone;

                for (int i = 0; i < 2; i++)
                {
                    Dust.NewDust(target.Center, 1, 1, ModContent.DustType<BetterBlood>(),
                        Main.rand.NextFloat(-4f, 4f), Main.rand.NextFloat(4f, 4f), 0, Color.White, 1f);
                }
            }
        }
        public override void FrameEffects()
        {
            /*Eol Transformation*/
            if (ShowEolTransform)
            {
                Player.head = EquipLoader.GetEquipSlot(Mod, ModContent.GetInstance<EverglowingTiara>().Name, EquipType.Head);
                Player.body = EquipLoader.GetEquipSlot(Mod, ModContent.GetInstance<EverglowingTiara>().Name, EquipType.Body);
                Player.legs = EquipLoader.GetEquipSlot(Mod, ModContent.GetInstance<EverglowingTiara>().Name, EquipType.Legs);
                Player.back = EquipLoader.GetEquipSlot(Mod, ModContent.GetInstance<EverglowingTiara>().Name, EquipType.Back);

                if (Main.dayTime)
                {
                    Player.head = EquipLoader.GetEquipSlot(Mod, "Day", EquipType.Head);
                    Player.legs = EquipLoader.GetEquipSlot(Mod, "Day", EquipType.Legs);
                    Player.back = EquipLoader.GetEquipSlot(Mod, "Day", EquipType.Back);
                }
            }

            /*Hide Legs*/
            if (HideLegs)
                Player.legs = EquipLoader.GetEquipSlot(Mod, ModContent.GetInstance<LegCutter>().Name, EquipType.Legs);

            /*Make it stop make it stop make it stop make it*/
            if (Sussy)
            {
                Player.head = EquipLoader.GetEquipSlot(Mod, ModContent.GetInstance<SuspiciousLookingEmergencyButton>().Name, EquipType.Head);
                Player.body = EquipLoader.GetEquipSlot(Mod, ModContent.GetInstance<SuspiciousLookingEmergencyButton>().Name, EquipType.Body);
                Player.legs = EquipLoader.GetEquipSlot(Mod, ModContent.GetInstance<SuspiciousLookingEmergencyButton>().Name, EquipType.Legs);
            }
        }
    }
}