using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.Drawing;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.UI;
using Terraria.Graphics.CameraModifiers;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.UI.Chat;
using TranscendenceMod.Buffs;
using TranscendenceMod.Dusts;
using TranscendenceMod.Items;
using TranscendenceMod.Items.Accessories.Expert;
using TranscendenceMod.Items.Armor.Hats;
using TranscendenceMod.Items.Consumables.LootBags;
using TranscendenceMod.Items.Consumables.Placeables;
using TranscendenceMod.Items.Consumables.Placeables.Decorations;
using TranscendenceMod.Items.Cosmetics;
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Items.Materials.MobDrops;
using TranscendenceMod.Items.Pets;
using TranscendenceMod.Items.Weapons.Magic;
using TranscendenceMod.Items.Weapons.Melee;
using TranscendenceMod.Items.Weapons.Ranged;
using TranscendenceMod.Items.Weapons.Summoner;
using TranscendenceMod.Miscanellous.MiscSystems;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Miscannellous.Biomes;
using TranscendenceMod.Miscannellous.GlobalStuff;
using TranscendenceMod.Miscannellous.Skies;
using TranscendenceMod.Miscannellous.UI;
using TranscendenceMod.NPCs.Passive;
using TranscendenceMod.NPCs.SpaceBiome;
using TranscendenceMod.Projectiles;
using TranscendenceMod.Projectiles.NPCs.Bosses.SpaceBoss;
using TranscendenceMod.Projectiles.Weapons;
using static TranscendenceMod.TranscendenceWorld;
using Conditions = Terraria.GameContent.ItemDropRules.Conditions;
using CollisionLib;

namespace TranscendenceMod.NPCs.Boss.Seraph
{
    [AutoloadBossHead]

    public class CelestialSeraph : ModNPC
    {
        // if god was real he would have made a cute and pretty she/they celestial princess instead of a ugly and hideous man #truthnuke
        #region Numbers
        readonly int CelestialStar = ModContent.ProjectileType<CelestialStar>();
        readonly int stellarfireball = ModContent.ProjectileType<StellarFireball>();
        readonly int BombSpawner = ModContent.ProjectileType<SupernovaSpawner>();
        readonly int galaxyshard = ModContent.ProjectileType<GalaxyShard>();
        readonly int EventHorizonproj = ModContent.ProjectileType<Asteroid>();
        readonly int BigCrunchSpawnProj = ModContent.ProjectileType<BigCrunchStarSpawn>();
        readonly int cosmicball = ModContent.ProjectileType<CosmicSphere>();
        readonly int minion = ModContent.ProjectileType<SpaceBossMinion>();
        readonly int swordslash = ModContent.ProjectileType<SwordSlash>();
        readonly int moon = ModContent.ProjectileType<Moon>();
        readonly int moonrock = ModContent.ProjectileType<MoonFrag>();
        readonly int grow = ModContent.ProjectileType<GrowingOrb>();
        readonly int shrink = ModContent.ProjectileType<ShrinkingOrb>();
        readonly int mist = ModContent.ProjectileType<StarMist>();
        readonly int pillar = ModContent.ProjectileType<PillarOfCreation>();
        readonly int dashproj2 = ModContent.ProjectileType<DashBallProj2>();
        readonly int glyph = ModContent.ProjectileType<StarGlyph>();
        readonly int blackholevisual = ModContent.ProjectileType<EventHorizonVisual>();
        readonly int pillarlaser = ModContent.ProjectileType<PoCLaser>();
        readonly int starbirthlaser = ModContent.ProjectileType<StarBirthLaser>();
        readonly int starlaser = ModContent.ProjectileType<HomingStarLaser>();
        readonly int inferno = ModContent.ProjectileType<MegaInferno>();
        readonly int spear = ModContent.ProjectileType<DivineSpear>();
        readonly int sun = ModContent.ProjectileType<ArtificialSun>();
        readonly int trackingSword = ModContent.ProjectileType<PristineBlade>();
        readonly int impurityDetector = ModContent.ProjectileType<RemoteBlastTelegraph>();
        readonly int sunSupernova = ModContent.ProjectileType<P2SupernovaSun>();


        Player player;
        readonly Player local = Main.LocalPlayer;

        public float RotationTimer;
        public float RotationSpeed;

        public readonly int WingsFrameHeight = 290;
        public int WingsFrame;

        /// <summary>
        /// The angle for the Left arm
        /// </summary>
        public float HandRotationRight;
        /// <summary>
        /// The angle for the Right arm
        /// </summary>
        public float HandRotationLeft;

        public int DespawnTimer;
        public Color DustColor = Color.BlueViolet;
        public Vector2 arenaCenter;

        public int SmiteCountdown;
        public int SmiteCountdownMax = 1;
        public int SmiteAttackTimer;

        //Rain
        public int RainCD;
        public int RainSpeed;
        public int MinionShotCD;


        public int BoundarySize = 695;

        public float skyFade;
        public float twinkleTimer;

        public int ProjReverse = 1;
        public int SupernovaCD;
        public bool HasSetUpArena;

        //Dash
        public int DashSpeed;
        public float dashBallSize;
        public bool CanDash;
        public Vector2 DashVel;

        public int ShardDashDistance = 1300;
        public int DashShardDelay;
        public int ShardDashSpeed = 25;
        public Vector2 Dashpos;

        //Crush
        public int CrushProjDelayTimer;
        public int CrushSpeed = 27;
        public int CrushProjDelay;
        public int SwordAttackTightness;

        //Cosmic Mirror
        public float laserVomit;

        //Stars
        public float HomingStarAuraSize;
        public float HomingStarAuraOpacity = 1;
        public Vector2 StarsVel;

        //EventHorizon
        public float BlackholeAlpha;

        public float ArenaStarsDistance;

        //AI
        public int Timer_AI;
        public float NPCFade = 0;
        public float ProjectileMultiplier;
        public bool KillProjectiles;
        public float arenaSizeShrinkAnim;
        public float arenaVisualRotation;
        public int Phase2Timer = 0;
        public int Phase3Timer = 0;

        public float DeathFade = 0;
        public int AttackDuration;
        /// <summary>
        /// An array storing 6 slots, ranging from 0 to 5
        /// Used for attacks
        /// </summary>
        public int[] ProjectileCD = new int[6];
        public int Phase;
        public bool HasArena;
        public bool DeathAnim;
        public string CurrentAttack = "";
        public SeraphAttacks Attack = SeraphAttacks.Intro;
        public float SkyTransitionP2 = 0;
        public float SkyTransitionP3 = 0;
        public Vector2 StargazerPos;

        public bool ActivatedPhase2Sky = false;
        public bool ActivatedPhase3Skin = false;
        public bool Legendary = Main.getGoodWorld || Main.zenithWorld;

        public bool DealsDamage;

        public Vector2[] drawArenaPos = new Vector2[151];
        public static bool NotClassic => (Main.expertMode || Main.masterMode);

        public CollisionSurface[] collisionSurfaces;

        private void DefaultStats()
        {
            RainCD = 45;
            RainSpeed = 8;
            MinionShotCD = 18;

            ProjectileMultiplier = Phase > 1 ? 1.35f : 1;
            SupernovaCD = Phase == 2 ? 60 : 75;

            DashSpeed = Phase == 2 ? 45 : 35;
            DashShardDelay = Phase > 1 ? 5 : 6;
            CrushProjDelay = Phase == 3 ? 7 : 5;
            SwordAttackTightness = 13;
        }
        #endregion Numbers

        public override void SetStaticDefaults()
        {

            NPCID.Sets.TrailCacheLength[Type] = 35;
            NPCID.Sets.TrailingMode[Type] = 1;

            NPCID.Sets.NeedsExpertScaling[Type] = true;
            NPCID.Sets.MustAlwaysDraw[Type] = true;

            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Shimmer] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Poisoned] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Venom] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Daybreak] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire3] = true;

            NPC.buffImmune[ModContent.BuffType<SpaceDebuff>()] = true;
            NPC.buffImmune[ModContent.BuffType<PrismaticBurn>()] = true;

        }
        public override void SetDefaults()
        {
            /*Stats*/
            NPC.lifeMax = 1075 * 1000;
            NPC.defense = 50;
            NPC.damage = 200;
            NPC.takenDamageMultiplier = 1;
            NPC.npcSlots = 6f;
            NPC.aiStyle = -1;
            NPC.value = Item.buyPrice(platinum: 1, gold: 25);

            /*Collision*/
            NPC.width = 126;
            NPC.height = 258;
            NPC.noGravity = true;
            NPC.noTileCollide = true;

            /*Audio*/
            NPC.HitSound = ModSoundstyles.SeraphHurt with
            {
                Volume = 0.66f,
                PitchRange = (-0.45f, 0.8f),
                MaxInstances = 0
            };
            Music = MusicLoader.GetMusicSlot("TranscendenceMod/Miscannellous/Assets/Sounds/Music/Silence");

            NPC.netUpdate = true;
            NPC.boss = true;
            NPC.friendly = false;
            NPC.knockBackResist = 0f;

            SpawnModBiomes = new int[1] { ModContent.GetInstance<Heaven>().Type };
        }
        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            scale = 1.75f;
            return NPCFade > 0.1f;
        }
        public override bool CheckActive() => false;
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * (Main.masterMode ? 0.525f : 0.575f));
            NPC.damage = (int)(NPC.damage * (Main.masterMode ? 0.525f : 0.625f));
        }
        public override void BossLoot(ref int potionType) => potionType = ItemID.SuperHealingPotion;
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            LeadingConditionRule normalMode = new LeadingConditionRule(new Conditions.NotExpert());

            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<StarPrize>()));

            /*Extra*/
            npcLoot.Add(ItemDropRule.NotScalingWithLuck(ModContent.ItemType<FaeTrophyItem>(), 4));
            normalMode.OnSuccess(ItemDropRule.NotScalingWithLuck(ModContent.ItemType<FaeMask>(), 3));
            /*Materials*/
            normalMode.OnSuccess(ItemDropRule.Common(ModContent.ItemType<ShimmerChunk>(), 1, 32, 44));
            /*Main Drops*/
            normalMode.OnSuccess(ItemDropRule.FewFromOptions(3, 1,
                ModContent.ItemType<LunaticFlail>(),
                ModContent.ItemType<SpaceBow>(),
                ModContent.ItemType<CelestialSeraphStaff>(),
                ModContent.ItemType<Constellations>(),
            ModContent.ItemType<Starfield>()));

            normalMode.OnSuccess(ItemDropRule.FewFromOptions(1, 2, ModContent.ItemType<AngelicHairdye>(),
                ModContent.ItemType<EarthHairdye>(), ModContent.ItemType<CosmicFogDye>()));

            npcLoot.Add(normalMode);
            /*Loot Bag, Relic and Pet*/
            npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<CelestialSeraphBag>()));
            npcLoot.Add(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<SeraphRelicItem>()));
            npcLoot.Add(ItemDropRule.MasterModeDropOnAllPlayers(ModContent.ItemType<SeraphPetItem>(), 3));
        }
        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            cooldownSlot = ImmunityCooldownID.Bosses;
            return DealsDamage && NPC.Distance(player.Center) < 100;
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;


            if (WingsFrame < (3 * WingsFrameHeight))
            {
                if (NPC.frameCounter % 10 == 0)
                    WingsFrame += WingsFrameHeight;
            }
            else
            {
                WingsFrame = 0;
            }
        }

        public override bool? CanBeHitByItem(Player player, Item item)
        {
            if (NPCFade > 0.1f)
                return base.CanBeHitByItem(player, item);
            else return false;
        }
        public override bool? CanBeHitByProjectile(Projectile projectile)
        {
            if (NPCFade > 0.1f)
                return base.CanBeHitByProjectile(projectile);
            else return false;
        }


        #region Behaviour
        public override void AI()
        {
            #region GeneralAI
            NPC.TargetClosest(true);

            player = Main.player[NPC.target];
            RotationTimer += RotationSpeed;
            arenaVisualRotation += 1.5f;

            NPC.ShowNameOnHover = NPCFade > 0.1f;
            SeraphTileDrawingSystem.PhaseThroughTimer = 5;


            Lighting.AddLight(NPC.Center, 4f, 3f, 0f);

            NPC.chaseable = true;

            if (Attack != SeraphAttacks.Intro)
                SkyManager.Instance.Activate("TranscendenceMod:CelestialSeraph", player.Center);

            //Teleport near player if too faraway
            if (!HasArena && player.Distance(NPC.Center) > 2050 && Attack != SeraphAttacks.SwordSlam && Attack != SeraphAttacks.SupernovaP2 && Attack != SeraphAttacks.NebulaMatter && !player.dead)
                Teleport(0, 600, -75);

            //Projectile stuff
            ProjectileManagerer();


            int area = (410 * 16);
            int sx = TranscendenceWorld.SpaceTempleX;
            if (collisionSurfaces == null || collisionSurfaces.Length < 4)
            {
                collisionSurfaces = new CollisionSurface[]
                {

                        //TopLeft to TopRight
                        new CollisionSurface(new Vector2(sx - area, 168 * 16), new Vector2(sx + area, 168 * 16), new int[]{ 1, 1, 1, 1 }),

                        //TopLeft to BottomLeft
                        new CollisionSurface(new Vector2(sx - area, 168 * 16), new Vector2(sx - area, 508 * 16), new int[]{ 1, 1, 1, 1 }),

                        //TopRight to BottomRight
                        new CollisionSurface(new Vector2(sx + area, 168 * 16), new Vector2(sx + area, 508 * 16), new int[]{ 1, 1, 1, 1 }),
                        
                        //BottomLeft to BottomRight
                        new CollisionSurface(new Vector2(sx - area, 508 * 16), new Vector2(sx + area, 508 * 16), new int[]{ 1, 1, 1, 1 })

                };
            }

            if (Attack != SeraphAttacks.DeathAnim || Timer_AI < (AttackDuration - 5))
            {
                for (int i = 0; i < 4; i++)
                {
                    collisionSurfaces[i].Update();
                    collisionSurfaces[i].DetectGrappleHookCollision();
                }
            }


            Timer_AI++;
            local.AddBuff(ModContent.BuffType<InfiniteFlight>(), 1);
            DefaultStats();

            //End attacks
            if (Timer_AI > (AttackDuration + 3))
            {
                NPCFade = 1f;

                KillClutter();
                AttackDefaultStats();
                DefaultStats();

                NPC.dontTakeDamage = false;

                if (Attack == SeraphAttacks.Stall && Phase == 2)
                {
                    //Start from any attack except Event Horizon
                    NPC.ai[1] = Main.rand.NextFromList(20, 21, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33);
                }
                else NPC.ai[1]++;
            }

            //Begin second phase at 0.666667% HP
            if (NPC.life <= (NPC.lifeMax * 0.666667f))
            {
                Phase2Timer++;
                if (SkyTransitionP2 < 1 && Phase2Timer < 60)
                {
                    KillClutter();
                    SkyTransitionP2 += 0.025f;
                    local.SetImmuneTimeForAllTypes(15);
                }
                if (Phase2Timer > 90 && Phase < 4)
                {
                    SkyTransitionP2 -= 0.05f;
                    ActivatedPhase2Sky = true;
                }

                if (Phase < 2)
                {
                    AttackDuration = 300;
                    NPC.ai[1] = 19;
                    Phase = 2;
                    DealsDamage = false;
                    NPCFade = 1f;

                    AttackDefaultStats();
                    SoundEngine.PlaySound(SoundID.Roar, NPC.Center);
                }
            }
            //Begin third phase at 33% HP
            if ((NPC.life <= (NPC.lifeMax * 0.33f) || Phase3Timer > 0) && NPC.ai[1] < 41 && Phase == 2)
            {
                NPCFade = 1f;
                Phase3Timer++;
                NPC.dontTakeDamage = true;
                Attack = SeraphAttacks.Stall;
                DealsDamage = false;
                HasArena = false;
                NPC.velocity *= 0.9f;

                if (NPC.Distance(player.Center) > 1500)
                    Teleport(3, 0, -500);

                if (skyFade > 0)
                    skyFade -= 0.1f;

                KillClutter();
                Timer_AI = AttackDuration - 30;

                local.GetModPlayer<TranscendencePlayer>().cameraModifier = true;
                local.GetModPlayer<TranscendencePlayer>().cameraPos = Vector2.SmoothStep(Main.screenPosition + new Vector2(Main.screenWidth / 2, Main.screenHeight / 2), NPC.Center, 0.125f);

                if (SkyTransitionP3 < 1 && Phase3Timer < 240)
                {
                    HandRotationLeft = MathHelper.Lerp(HandRotationLeft, 210, 0.125f);
                    HandRotationRight = MathHelper.Lerp(HandRotationRight, - 210, 0.125f);

                    if (Phase3Timer == 85) SoundEngine.PlaySound(SoundID.Shatter);

                    return;
                }
                if (Phase3Timer > 240 && Phase3Timer < 300)
                {
                    HandRotationLeft = MathHelper.Lerp(HandRotationLeft, 0, 0.1f);
                    HandRotationRight = MathHelper.Lerp(HandRotationRight, 0, 0.1f);

                    if (!ActivatedPhase3Skin)
                    {
                        for (int i = 0; i < 12; i++)
                        {
                            Item.NewItem(NPC.GetSource_FromAI(), NPC.Center + Main.rand.NextVector2Circular(375f, 375f), ItemID.NebulaPickup1);
                            TranscendenceUtils.ParticleOrchestra(ParticleOrchestraType.Excalibur, NPC.Center + Main.rand.NextVector2Circular(NPC.width / 2f, NPC.height * 0.75f), -1);
                        }

                        DialogUI.SpawnDialogCutscene("Mods.TranscendenceMod.Messages.SeraphBossDialog.Phase3", DialogBoxes.Seraph, 1, 3, NPC, new Vector2(0, -196), 60, Color.White);
                        ActivatedPhase3Skin = true;
                    }

                    return;
                }

                //Finish the transition (so proud of her)
                if (Phase3Timer > 670)
                {
                    NPC.life = (int)(NPC.lifeMax * 0.33f);
                    NPC.ai[1] = 40;
                    Phase = 3;
                }
                return;
            }


            if (Timer_AI > (AttackDuration - 25)) LaunchShards();
            //Attack Patterns
            switch (NPC.ai[1])
            {
                //Phase 1
                case 0: Intro(); break;

                case 1: Stare(); break;

                case 2: Crush(); break;

                case 3: Homing(); break;

                case 4: PillarStorm(); break;

                case 5: MeteorFall(); break;

                case 6: Rain(); break;

                case 7: BirthOfAStar(); break;

                case 8: Ballin(); break;

                case 9: Supernova(); break;

                case 10: NPC.ai[1] = 2; goto case 2;



                //Phase 2
                case 19: Stall(); break;

                case 20: BirthOfAStar(); break;

                case 21: GrowAndShrink(); break;

                case 22: SupernovaP2(); break;

                case 23: EventHorizon(); break;

                case 24: Crush(); break;

                case 25: MeteorFall(); break;

                case 26: StarBomb(); break;

                case 27: Rain(); break;

                case 28: Ballin(); break;

                case 29: DashBall(); break;

                case 30: Crush(); break;

                case 31: StarBomb(); break;

                case 32: Rain(); break;

                case 33: Homing(); break;

                case 34: NPC.ai[1] = 20; goto case 20;

                //Phase 3
                case 40: Stall(); break;
                case 41: DivineSpear(); break;
                case 42: TrackingBlades(); break;
                case 43: RoyalFlash(); break;
                case 44: ImpurityDetector(); break;
                case 45: DeathraysP3(); break;
                case 46: NPC.ai[1] = 41; goto case 41;

                case 101:
                    {
                        KillClutter();
                        DeathAnimation();
                        break;
                    }
                case 102:
                    {
                        NPC.Center = new Vector2(TranscendenceWorld.SpaceTempleX, 97 * 16);
                        player.Center = new Vector2(TranscendenceWorld.SpaceTempleX, 97 * 16);

                        NPC.StrikeInstantKill();
                        return;
                    }
            }


            //Prevent Out of Bounds entry
            if (NPC.ai[1] < 100)
            {
                if (player.position.Y >= (505 * 16))
                {
                    player.GetModPlayer<TranscendencePlayer>().HorseshoeBonusActive = 5;
                    if (player.position.Y >= (508 * 16))
                        player.Teleport(new Vector2(player.Center.X, 503 * 16), TeleportationStyleID.PotionOfReturn);
                }
                if (player.position.Y <= (170 * 16))
                    player.Teleport(new Vector2(player.Center.X, 175 * 16), TeleportationStyleID.PotionOfReturn);

                if (player.position.X <= (sx - area - (2 * 16)))
                    player.Teleport(new Vector2(sx - area + (5 * 16), player.Center.Y), TeleportationStyleID.PotionOfReturn);
                if (player.position.X >= (sx + area + (2 * 16)))
                    player.Teleport(new Vector2(sx + area - (5 * 16), player.Center.Y), TeleportationStyleID.PotionOfReturn);
            }


            if (Timer_AI < 45 && HasArena && arenaSizeShrinkAnim < 1)
                arenaSizeShrinkAnim += 0.0222222222222f;

            //Despawn
            if (player.dead)
            {
                NPC.Center = new Vector2(TranscendenceWorld.SpaceTempleX, 97 * 16);

                SoundEngine.PlaySound(ModSoundstyles.SeraphTeleport with
                {
                    Volume = 0.6f,
                    MaxInstances = 0
                });

                for (int j = 0; j < 5; j++)
                {
                    for (int i = 0; i < 32; i++)
                    {
                        float rot = MathHelper.TwoPi * i / 32f;
                        Vector2 vec = new Vector2(0, 5f + (j * 2f)).RotatedBy(rot + MathHelper.ToRadians(i / 2f));
                        Dust.NewDustPerfect(NPC.Center, ModContent.DustType<PlayerCosmicBlood>(), new Vector2(vec.X / 2f, vec.Y).RotatedBy(MathHelper.ToRadians(45f * j)), 0, Color.Gold, 4f);
                    }
                }

                NPC.active = false;
            }
            if (Timer_AI < 10)
                KillClutter();
            #endregion GeneralAI
        }

        //Set an arena for bullet hells
        private void Ring(Vector2 Position)
        {
            HasArena = true;
            arenaCenter = Position;
            
            twinkleTimer += 1.5f;

            if (ArenaStarsDistance > 0f && Timer_AI < 90)
            {
                ArenaStarsDistance -= MathHelper.Lerp(0f, 40f, Timer_AI / 90f);

                if (Timer_AI < 10)
                {
                    /* Teleport the arena ring away from the pocket dimension boundaries,
                    making it snap to the inside of the box */
                    float paddedArea = BoundarySize * 1.25f;
                    int area = (410 * 16);
                    int sx = TranscendenceWorld.SpaceTempleX;

                    if (arenaCenter.Y >= (508 * 16 - paddedArea))
                        Teleport(6, (int)NPC.Center.X, 504 * 16 - paddedArea);

                    if (arenaCenter.Y <= (168 * 16 + paddedArea))
                        Teleport(6, (int)NPC.Center.X, 172 * 16 + paddedArea);

                    if (arenaCenter.X >= (sx + area - paddedArea))
                        Teleport(6, (int)(sx + area - paddedArea + 64), NPC.Center.Y);

                    if (arenaCenter.X <= (sx - area + paddedArea))
                        Teleport(6, (int)(sx - area + paddedArea + 64), NPC.Center.Y);
                }

            }

            if (Timer_AI > (AttackDuration - 45))
            {
                ArenaStarsDistance += 5f;
                arenaSizeShrinkAnim -= 0.0222222222222f;
            }

            if (Timer_AI < 120)
            {
                if (!HasArena && !HasSetUpArena)
                    FollowPlayer(Vector2.Zero, 20, 100);

                if (NPC.Distance(player.Center) < 100)
                {
                    HasSetUpArena = true;
                    NPC.velocity *= 0.8f;
                    NPC.velocity.Y = 0;
                }
            }

            else
            {
                if (Attack != SeraphAttacks.HomingStars && Attack != SeraphAttacks.EventHorizon) NPC.velocity = Vector2.Zero;
            }

            for (int i = 0; i < 150; i++)
            {
                if (Timer_AI > 60 && local.Distance(drawArenaPos[i]) < 25 && arenaSizeShrinkAnim == 1 && Attack != SeraphAttacks.EventHorizon)
                {
                    local.velocity = Main.LocalPlayer.DirectionTo(arenaCenter);
                    local.Center = local.Center.MoveTowards(arenaCenter, 5);
                }
            }

            if (local.Distance(arenaCenter) > (BoundarySize * 1.5f) && Timer_AI > 120)
            {
                local.AddBuff(ModContent.BuffType<SpaceDebuff>(), 120);
                local.Center = local.Center.MoveTowards(arenaCenter, 20);
            }

            //Darken sky
            if (Attack != SeraphAttacks.EventHorizon && Attack != SeraphAttacks.BreathingStar)
            {
                if (skyFade < 1 && Timer_AI > 60 && Timer_AI < (AttackDuration - 60))
                    skyFade += 1f / 90f;

                else if (Timer_AI > (AttackDuration - 60))
                    skyFade -= 1f / 60f;
            }
        }

        private void Intro()
        {
            CurrentAttack = "The Angel's Gateway";
            Attack = SeraphAttacks.Intro;
            AttackDuration = 120;


            NPC.dontTakeDamage = true;
            local.GetModPlayer<AngelsGatewayPlayer>().ZoneAngelGateway = 5;

            if (Timer_AI > (AttackDuration - 45))
                ProjectileCD[0]++;

            AngelGatewaySky.Progress = ProjectileCD[0] / 30f;
        }

        private void Rain()
        {
            CurrentAttack = Phase == 2 ? Language.GetTextValue("Mods.TranscendenceMod.SeraphAttackNames.RainP2") :
                Language.GetTextValue("Mods.TranscendenceMod.SeraphAttackNames.RainP1");

            Attack = SeraphAttacks.StellarFirestorm;
            AttackDuration = 450;

            if (Timer_AI < (AttackDuration - 60) && skyFade < 1f)
                skyFade += 1f / 60f;

            //Fade bg out when the attack ends
            if (Timer_AI > (AttackDuration - 60))
                skyFade -= 1f / 60f;

            arenaCenter = NPC.Center + new Vector2(0, 500);
            DustColor = Color.MediumPurple * 0.75f;
            BoundarySize = 275;

            Vector2 rainPos = player.Center + new Vector2(0, -350);

            if (Timer_AI < 60) NPC.Center = NPC.Center.MoveTowards(rainPos, 30);
            else NPC.velocity = Vector2.Zero;

            if (arenaCenter.Distance(player.Center) > (Timer_AI > 90 ? 400 : 300))
            {
                arenaCenter = player.Center;
                NPC.Center = arenaCenter - new Vector2(0, 500);
            }

            if (++ProjectileCD[0] % 3 == 0)
            {
                int amt = 8;
                float rotater = Phase == 2 ? (float)Math.Sin(Timer_AI) : 1f;

                for (int i = 0; i < amt; i++)
                {
                    Vector2 pos = arenaCenter + Vector2.One.RotatedBy((MathHelper.TwoPi * i / amt) + (Math.Sin(TranscendenceWorld.UniversalRotation * 2f))) * BoundarySize;
                    Dust.NewDustPerfect(pos, ModContent.DustType<ArenaDust>(), Vector2.Zero, 0, Color.Magenta, 1f);
                    if (Timer_AI > 180)
                    {
                        int p = Projectile.NewProjectile(NPC.GetSource_FromAI(), pos, arenaCenter.DirectionTo(pos).RotatedBy(MathHelper.PiOver2 * rotater) * 2f, stellarfireball, 100, 2, -1, 0, NPC.whoAmI);
                        Main.projectile[p].timeLeft = 120;
                    }
                }
            }

            if (Timer_AI > 90)
            {
                if (++ProjectileCD[1] > RainCD)
                {
                    Dust.NewDustPerfect(NPC.Center - new Vector2(15, -8), ModContent.DustType<ArenaDust>(), new Vector2(-15, 0), 0, Color.BlueViolet, 3);
                    Dust.NewDustPerfect(NPC.Center + new Vector2(-15, 8), ModContent.DustType<ArenaDust>(), new Vector2(15, 0), 0, Color.BlueViolet, 3);

                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, new Vector2(-RainSpeed, 0), minion,
                            80, 0f, -1, 0, NPC.whoAmI, MinionShotCD);

                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, new Vector2(RainSpeed, 0), minion,
                            80, 0f, -1, 0, NPC.whoAmI, MinionShotCD);
                    ProjectileCD[1] = 0;
                }
                if (Timer_AI < 120)
                    return;

                if (ProjectileCD[1] > (RainCD - 20))
                {
                    HandRotationRight = MathHelper.Lerp(HandRotationRight, 5, 0.075f);
                    HandRotationLeft = MathHelper.Lerp(HandRotationLeft, -5, 0.075f);
                }

                else if (ProjectileCD[1] < 20)
                {
                    HandRotationRight = MathHelper.Lerp(HandRotationRight, -240, 0.05f);
                    HandRotationLeft = MathHelper.Lerp(HandRotationLeft, 240, 0.05f);
                }
            }
        }


        private void BirthOfAStar()
        {
            AttackDuration = Phase == 2 ? 900 : 660;
            CurrentAttack = Phase == 2 ? Language.GetTextValue("Mods.TranscendenceMod.SeraphAttackNames.BirthOfAStarP2") : Language.GetTextValue("Mods.TranscendenceMod.SeraphAttackNames.BirthOfAStar");
            Attack = SeraphAttacks.BirthOfAStar;

            Ring(NPC.Center);
            DustColor = new Color(0f, 0.25f, 0.66f);
            BoundarySize = 560;

            bool GlyphPhase = Timer_AI < 360;
            int GlyphCD = Phase == 2 ? 12 : 14;

            HandRotationLeft = MathHelper.Lerp(HandRotationLeft, 80, 0.05f);
            HandRotationRight = -HandRotationLeft;


            //Summon the deathrays for the first part
            if (Timer_AI == 60)
            {
                TranscendenceUtils.ProjectileRing(NPC, Phase == 2 ? 6 : 5, NPC.GetSource_FromAI(), NPC.Center, starbirthlaser, 75, 3,
                        2, 2, NPC.whoAmI, 140, player.whoAmI, Main.rand.NextFloat(MathHelper.TwoPi));
            }

            if (GlyphPhase)
            {
                if (++CrushProjDelayTimer > GlyphCD && Timer_AI > 60 && Timer_AI < 300)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        Vector2 pos = NPC.Center + Vector2.One.RotatedBy(MathHelper.TwoPi * i / 5f + NPC.DirectionTo(player.Center).ToRotation() - MathHelper.PiOver4) * ((BoundarySize + 60 + ArenaStarsDistance) * 1.1f);
                        Dust.NewDust(pos, 1, 1, ModContent.DustType<ArenaDust>(), 0, 0, 0, Color.DeepSkyBlue, 2f);

                        Projectile.NewProjectile(NPC.GetSource_FromAI(), pos, Vector2.Zero,
                            glyph, 75, 2, -1, 11, NPC.whoAmI, Phase > 1 ? 3 : 12);
                    }

                    CrushProjDelayTimer = 0;
                }
            }

            if (!GlyphPhase)
            {
                if (ProjectileCD[3] == 160)
                {
                    SoundEngine.PlaySound(ModSoundstyles.SeraphBomb);
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<Shockwave>(), 4000, 80f, -1, 0f, 0.5f, 1f);
                }
                if (ProjectileCD[3] < 1000)
                    ProjectileCD[3] += 20;
            }

            //Spawn the mist
            if (Timer_AI < (AttackDuration - 250))
            {
                if (TranscendenceWorld.CountProjectiles(mist) > 50)
                    return;

                Vector2 vel = Vector2.One.RotatedBy(RotationTimer / 7);
                float am = 48;
                if (Phase == 2)
                    am = 7;

                for (int i = 0; i < 7; i++)
                {
                    for (int j = 0; j < am; j++)
                    {
                        Vector2 pos = NPC.Center + Vector2.One.RotatedBy(MathHelper.TwoPi * i / 7f + MathHelper.ToRadians(j * 1.75f)) * (float)(Math.Sin(j / 16f) * 350f);
                        if (Phase == 2)
                            pos = NPC.Center + Vector2.One.RotatedByRandom(MathHelper.TwoPi) * Main.rand.NextFloat(0f, 500f);
                        int am2 = Phase == 2 ? j : 0;
                        int p = Projectile.NewProjectile(NPC.GetSource_FromAI(), pos, Vector2.Zero, mist, 90, 1, -1, Phase == 2 ? -1.75f : 2.325f, NPC.whoAmI, am2);
                        Main.projectile[p].timeLeft = AttackDuration - 60;
                    }
                }

                if (Timer_AI > 400) SoundEngine.PlaySound(SoundID.Item34 with { MaxInstances = -1 }, NPC.Center);
                ProjectileCD[1] = 0;
            }
        }


        private void Supernova()
        {
            CurrentAttack = Language.GetTextValue("Mods.TranscendenceMod.SeraphAttackNames.Supernova");
            Attack = SeraphAttacks.Supernova;
            AttackDuration = 730;


            if (Timer_AI < 100)
                skyFade += 0.75f / 100f;

            if (Timer_AI > (AttackDuration - 100))
                skyFade -= 0.75f / 100f;


            //Slow down right after lunging
            if (ProjectileCD[0] < 30)
                NPC.velocity *= 0.9f;


            //Follow the player
            if (NPC.Distance(player.Center) > 60)
                NPC.velocity = NPC.DirectionTo(player.Center) * (player.Distance(NPC.Center) / 25f);


            if (Timer_AI > (AttackDuration - 100))
            {
                HandRotationRight = -10;
                HandRotationLeft = 10;

                DealsDamage = false;

                NPCFade = 1f;


                return;
            }

            //Pre-Lunge action
            if (ProjectileCD[0] < SupernovaCD)
            {
                NPCFade = 1f;

                if (NPC.direction == 1)
                {
                    HandRotationRight = -25;
                    HandRotationLeft = 10;
                }
                else
                {
                    HandRotationLeft = 25;
                    HandRotationRight = -10;
                }
            }
            
            if (Timer_AI < 60)
                return;

            //Draw the blade out
            if (Timer_AI == 62)
                SoundEngine.PlaySound(ModSoundstyles.SeraphSwords_Draw);

            DealsDamage = ProjectileCD[0] > SupernovaCD;

            //Begin lunge
            if (ProjectileCD[0] == SupernovaCD)
            {
                Dashpos = player.Center;
                DashVel = NPC.DirectionTo(Dashpos) * 25;
                BlackholeAlpha = NPC.DirectionTo(Dashpos).ToRotation() - MathHelper.PiOver2;

                //Transform into a blade because I got frustrated with making the blade position correctly on the hand
                for (int i = 0; i < 6; i++)
                {
                    for (int j = 0; j < 32; j++)
                    {
                        Vector2 vel = new Vector2(0, 10f + (i * 3f) + (float)Math.Sin(j) * (2f + (i * 0.5f))).RotatedBy(MathHelper.TwoPi * j / 32f + MathHelper.PiOver4 / 2f + MathHelper.ToRadians(i * 45f));
                        Dust.NewDustPerfect(NPC.Center, ModContent.DustType<PlayerCosmicBlood>(), vel, 0, Color.Lerp(Color.Blue, Color.Magenta, i / 4f), 2f);
                    }
                }
            }

            //Lunge at the player
            if (ProjectileCD[0] > (SupernovaCD + 10))
            {
                if (ProjectileCD[0] == (SupernovaCD + 15))
                {
                    SoundEngine.PlaySound(ModSoundstyles.SeraphSwords_Charge with { Volume = 2, PitchVariance = 0.5f});
                }

                NPC.velocity = DashVel;
                Dust.NewDustPerfect(NPC.Center, ModContent.DustType<ArenaDust>(), -NPC.velocity, 0, Color.White, 1.5f);
            }

            if (++ProjectileCD[0] > (SupernovaCD + 60))
            {
                //Create gigantic explosion
                int p = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<SpaceBossBombBlast>(), 120, 8f, -1, 0, NPC.whoAmI, 12f);
                Main.projectile[p].extraUpdates *= 3;

                //Create a pattern of explosions
                TranscendenceUtils.ProjectileRing(NPC, (int)(10 * ProjectileMultiplier), NPC.GetSource_FromAI(), NPC.Center, BombSpawner,
                    100, 0, 45, 1f, 7, 1.5f, -1, 0);

                TranscendenceUtils.ProjectileRing(NPC, (int)(10 * ProjectileMultiplier), NPC.GetSource_FromAI(), NPC.Center, BombSpawner,
                    100, 0, 45, 0f, 7, -1.5f, -1, 0);

                SoundEngine.PlaySound(ModSoundstyles.SeraphSpear);
                ProjectileCD[0] = 0;
            }
        }


        private void Homing()
        {
            AttackDuration = 620;
            CurrentAttack = Phase == 2 ? Language.GetTextValue("Mods.TranscendenceMod.SeraphAttackNames.HomingP2") :
                Language.GetTextValue("Mods.TranscendenceMod.SeraphAttackNames.HomingP1");
            Attack = SeraphAttacks.HomingStars;

            if (HandRotationRight > -80)
                HandRotationRight -= 2.5f;
            HandRotationLeft = -HandRotationRight;

            BoundarySize = 750;
            Ring(NPC.Center);
            DustColor = Color.Lerp(new Color(255, 134, 245), new Color(235, 94, 255), (float)Math.Sin(TranscendenceWorld.UniversalRotation * 2f));

            if (Timer_AI == 5)
                Teleport(3, 0, -200);

            if (Timer_AI < 120)
            {
                HomingStarAuraSize += 0.016f;
                return;
            }
            else if (Timer_AI > (AttackDuration - 30)) HomingStarAuraSize -= 0.032f;

            if (Timer_AI > (AttackDuration - 60))
                return;

            if (TranscendenceWorld.CountProjectiles(starlaser) == 0)
            {
                int amount = Phase == 2 ? 5 : 6;
                float speed = (Phase == 2 ? 1.075f : 1) * ProjReverse;

                TranscendenceUtils.ProjectileRing(NPC, amount, NPC.GetSource_FromAI(), NPC.Center,
                    starlaser, 95, 1, 2, speed, NPC.whoAmI, Phase == 2 ? 1 : 0, -1, MathHelper.ToRadians(Timer_AI / 4f));

                if (Phase == 2)
                {
                    TranscendenceUtils.ProjectileRing(NPC, amount, NPC.GetSource_FromAI(), NPC.Center,
                        starlaser, 95, 1, 2, -speed, NPC.whoAmI, Phase == 2 ? 1 : 0, -1, MathHelper.ToRadians(Timer_AI / 4f));
                }

                ProjReverse = -ProjReverse;
            }

            int starCD = 90;
            if (++ProjectileCD[0] == starCD)
            {
                SoundEngine.PlaySound(SoundID.Item164 with { MaxInstances = 0 }, NPC.Center);
            }

            Vector2 circleSpread = new Vector2(-8, 0);

            if (ProjectileCD[0] > (starCD + 5))
            {
                if (Phase == 2)
                {
                    for (int i = 0; i < 3; i++)
                        TranscendenceUtils.ProjectileRing(NPC, 5, NPC.GetSource_FromAI(), NPC.Center, CelestialStar, 95, 1, 0.275f + skyFade + (i / 2.5f), 0, NPC.whoAmI, 1f, -1, MathHelper.ToRadians(i * 20f));

                    ProjectileCD[0] = 0;
                }
                else
                {
                    if (++ProjectileCD[3] % 6 == 0)
                    {
                        TranscendenceUtils.ProjectileRing(NPC, 2, NPC.GetSource_FromAI(), NPC.Center - new Vector2(150 - (ProjectileCD[3] * 7.5f), 100 + ((float)Math.Cos(ProjectileCD[3] / 5f) * 55)),
                            CelestialStar, 95, 1, 0.5f + skyFade, 0, NPC.whoAmI, skyFade, -1, 0);

                        if (ProjectileCD[0] > (starCD + 40))
                        {
                            if (skyFade < 1) skyFade += 0.33f;
                            ProjectileCD[3] = 0;
                            ProjectileCD[0] = 0;
                        }
                    }
                }
            }
        }

        private void EventHorizon()
        {
            BoundarySize = 1275;
            AttackDuration = 900;

            CurrentAttack = Language.GetTextValue("Mods.TranscendenceMod.SeraphAttackNames.Blackhole");
            Attack = SeraphAttacks.EventHorizon;

            DustColor = Color.DarkOrange;
            NPC.dontTakeDamage = true;

            NPCFade = 0f;
            if (Timer_AI < (AttackDuration - 60))
                skyFade = 1f;
            else skyFade -= 1f / 60f;

            if (Timer_AI > 180 && ArenaStarsDistance > -1200f)
            {
                ArenaStarsDistance -= suckStrenght() * 25f;
            }


            if (CrushProjDelayTimer < 120)
                CrushProjDelayTimer++;
            else CrushProjDelayTimer = 120;


            if (++ProjectileCD[1] > 90 && Timer_AI < AttackDuration)
            {
                ProjReverse = -ProjReverse;
                ProjectileCD[3] += 15;

                RotationSpeed = 0.075f * ProjReverse;
                ProjectileCD[1] = 0;
            }

            Ring(NPC.Center);

            if (Timer_AI > 120 && Timer_AI < (AttackDuration - 270) && ++ProjectileCD[0] % 2 == 0)
            {
                SoundEngine.PlaySound(SoundID.Item117 with { MaxInstances = 0, Volume = 0.15f, Pitch = -1f }, NPC.Center);
                BlackHoleSuck();
            }

            void BlackHoleSuck()
            {
                int visualChance = 15;
                if (Main.rand.NextBool(visualChance))
                {
                    Vector2 pos = NPC.Center + Vector2.One.RotatedByRandom(360) * BoundarySize;

                    Projectile.NewProjectile(NPC.GetSource_FromAI(), pos, Vector2.Zero, blackholevisual,
                        0, 0, -1, 0, NPC.whoAmI);
                }


                //Suck the player into the blackhole if they're too close
                if (local.Distance(NPC.Center) < 125)
                    {
                        local.velocity = local.DirectionTo(arenaCenter + Vector2.One.RotatedBy(MathHelper.ToRadians(Timer_AI * 5)) * 25) * 10;
                        local.AddBuff(ModContent.BuffType<BlackHoleDebuff>(), 5);
                    }

                for (int i = 0; i < 4; i++)
                {
                    Vector2 pos = NPC.Center + Vector2.One.RotatedBy((MathHelper.TwoPi * i / 4f) + Math.Sin(RotationTimer)) * BoundarySize;

                    Projectile.NewProjectile(NPC.GetSource_FromAI(), pos, Vector2.Zero, EventHorizonproj,
                        95, 0f, -1, 0, NPC.whoAmI, 12.5f);
                }
            }
        }


        private void Crush()
        {
            CurrentAttack = Language.GetTextValue("Mods.TranscendenceMod.SeraphAttackNames.SwordSlamP2");
            Attack = SeraphAttacks.SwordSlam;
            AttackDuration = Phase == 3 ? 290 : 145;

            //Draw the blade out
            if (Timer_AI == 5)
                SoundEngine.PlaySound(ModSoundstyles.SeraphSwords_Draw);

            if (Timer_AI < 10)
                return;

            DealsDamage = ProjectileCD[0] > 40;

            //Go above player within 40 frames
            if (++ProjectileCD[0] < 40)
            {
                Dashpos = player.Center + new Vector2(player.velocity.X * 10, 0) - new Vector2(0, 475);
                NPC.Center = NPC.Center.MoveTowards(Dashpos, 50);

                if (BlackholeAlpha < 1)
                    BlackholeAlpha += 1f / 15f;

            }
            else DashDown();

            void DashDown()
            {
                //Summon a wave of sword projectiles that gets less tighter the further it goes,
                //Also dash down while creating a galaxy shard wall

                if (BlackholeAlpha > 0)
                    BlackholeAlpha -= 1f / 30f;

                if (++ProjectileCD[1] == 10)
                {
                    SoundEngine.PlaySound(ModSoundstyles.SeraphSwords_Charge with { Volume = 2, PitchVariance = 0.5f });
                    if (Phase < 3)
                        SwordProjs();
                }
                if (ProjectileCD[0] < 115)
                {
                    NPC.velocity.X = 0;
                    NPC.velocity.Y = CrushSpeed;

                    //Creates two trails to give a nice sword effect
                    Dust.NewDustPerfect(NPC.Center + new Vector2(15, -60), DustID.PortalBoltTrail,
                        new Vector2(0, -45), 0, Color.White, 2.5f);

                    Dust.NewDustPerfect(NPC.Center + new Vector2(-15, -60), DustID.PortalBoltTrail,
                        new Vector2(0, -45), 0, Color.White, 2.5f);

                    if (++CrushProjDelayTimer > CrushProjDelay)
                    {
                        Vector2 ten = new Vector2(10, 0);

                        //Spawn the shards
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + ten, ten, galaxyshard,
                            105, 0f, -1, 0, 0, 1);

                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center - ten, -ten, galaxyshard,
                            105, 0f, -1, 0, 0, -1);

                        CrushProjDelayTimer = 0;
                    }
                }
            }

            //Return back to player
            if (ProjectileCD[0] > 101)
            {
                if (NPC.Distance(player.Center) > 350)
                    FollowPlayer(Vector2.Zero, 32, 500);
                else NPC.velocity *= 0.5f;

                HandRotationRight = -10;
                HandRotationLeft = -HandRotationRight;
            }
            else
            {
                float hand = (HandRotationRight < 0 ? -HandRotationRight : HandRotationRight) * 0.1f;
                HandRotationRight = MathHelper.Lerp(HandRotationRight, 10, 0.1f + hand);
                HandRotationLeft = -HandRotationRight;
            }

            if (ProjectileCD[0] > 130)
            {
                LaunchShards();
                ProjectileCD[0] = 0;
                ProjectileCD[1] = 0;
            }

            void SwordProjs()
            {
                for (int n = 0; n < (Phase == 3 ? 8 : 1); n++)
                {
                    for (int i = 0; i < 6; i++)
                    {
                        Vector2 vec1 = new Vector2(i * (SwordAttackTightness + (i * 40)), -375 - (i * 80));
                        Vector2 vec2 = new Vector2(i * (SwordAttackTightness + (i * 40)), 375 + (i * 80));

                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + vec1 - new Vector2(0, (i * 25) + (n * 155)),
                            new Vector2(0, 5), swordslash, 90, 2f, -1, 0, NPC.whoAmI);

                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center - vec2 - new Vector2(0, (i * 25) + (n * 155)),
                            new Vector2(0, 5), swordslash, 90, 2f, -1, 0, NPC.whoAmI);
                    }
                }
            }
        }
        public void DashBall()
        {
            CurrentAttack = Phase > 1 ? Language.GetTextValue("Mods.TranscendenceMod.SeraphAttackNames.NebulaMatterP2") : Language.GetTextValue("Mods.TranscendenceMod.SeraphAttackNames.NebulaMatterP1");
            Attack = SeraphAttacks.NebulaMatter;

            if (CrushProjDelayTimer < 100 && Timer_AI < 200)
                CrushProjDelayTimer++;
            if (Timer_AI > (AttackDuration - 60))
                CrushProjDelayTimer = (int)MathHelper.Lerp(CrushProjDelayTimer, 0, 1f / 60f);

            AttackDuration = Phase > 1 ? 520 : 905;
            if (NPCFade > 0.5f)
                NPCFade = 0.5f - (CrushProjDelayTimer / 100f);

            float projCount = 6 * ProjectileMultiplier;
            int cd = 25;

            if (Timer_AI < 45)
            {
                dashBallSize += 1.575f / 45f;
                return;
            }


            // End the attack
            if (Timer_AI > (AttackDuration - 60))
            {
                dashBallSize -= 1.575f / 45f;
                NPC.velocity *= 0.9f;
                return;
            }


            DealsDamage = true;


            void ShootDashProjectiles()
            {
                for (int i = 0; i < 16; i++)
                {
                    Vector2 vel = new Vector2(10f, 0f).RotatedBy(MathHelper.TwoPi * i / 16f);
                    vel.Y /= 2f;
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, vel.RotatedBy(DashVel.ToRotation() + MathHelper.PiOver2), dashproj2, 85, 2f, -1, 0, NPC.whoAmI, 1.25f);
                }
            }

            if (ProjectileCD[0] < (10 + cd))
            {
                DashVel = NPC.DirectionTo(player.Center - player.velocity * 10) * DashSpeed;
            }

            if (++ProjectileCD[0] > (5 + cd))
            {
                if (ProjectileCD[0] == (6 + cd))
                {
                    ProjectileCD[2] += 1;
                }

                else if (ProjectileCD[0] > (10 + cd))
                {
                    if (ProjectileCD[0] < (30 + cd))
                        NPC.velocity = DashVel;
                    else EndDash();
                }

                void EndDash()
                {
                    SoundEngine.PlaySound(SoundID.DD2_BetsyFireballImpact, NPC.Center);
                    ProjectileCD[1]++;

                    ShootDashProjectiles();

                    if (ProjectileCD[1] > 5)
                        ProjectileCD[1] = 0;

                    if (ProjectileCD[2] > 4)
                        ProjectileCD[2] = 0;

                    ProjectileCD[0] = 0;
                }
            }
            else NPC.velocity *= 0.75f;
        }


        //Celestial Seraph will never be ballin- *Spits cereal*
        public void Ballin()
        {
            Ring(Dashpos);
            BoundarySize = Phase == 2 ? 750 : 500;
            AttackDuration = Phase == 2 ? 570 : 600;
            DustColor = new Color(100, 100, 100);

            if (Timer_AI < 5)
                Dashpos = NPC.Center;

            CurrentAttack = Language.GetTextValue("Mods.TranscendenceMod.SeraphAttackNames.Moon");
            Attack = SeraphAttacks.Moons;

            //Dashpos = NPC.Center;

            if (Timer_AI > 30 && Timer_AI < (AttackDuration - 120) && player.ownedProjectileCounts[moon] < 1)
            {
                int p = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, new Vector2(0, -40).RotatedBy(
                    player.ownedProjectileCounts[moon] * 20), moon, 150, 5, player.whoAmI, 0, NPC.whoAmI);
                Main.projectile[p].localAI[0] = Phase == 2 ? 1f : 0f;
                Main.projectile[p].scale = Phase == 2 ? 1f : 0.6f;
            }
        }

        public void GrowAndShrink()
        {
            Ring(NPC.Center - new Vector2(0, 500));
            BoundarySize = 1000;
            AttackDuration = 900;
            DustColor = Color.Black;

            CurrentAttack = Language.GetTextValue("Mods.TranscendenceMod.SeraphAttackNames.BreathingStar");
            Attack = SeraphAttacks.BreathingStar;

            if (!TranscendenceWorld.AnyProjectiles(sun))
                Projectile.NewProjectile(NPC.GetSource_FromAI(), arenaCenter,
                    Vector2.Zero, sun, 40, 0f, -1, 0, NPC.whoAmI);


            Vector2 vel = arenaCenter.DirectionTo(player.Center);
            Vector2 vec = Vector2.One.RotatedBy(RotationTimer * 0.2f) * -325f * skyFade;

            if (Timer_AI > 205 && skyFade < 1f && Timer_AI < (AttackDuration - 90))
                skyFade += 0.0125f;

            if (++ProjectileCD[0] % 20 == 0)
            {
                for (int i = 0; i < 4; i++)
                {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), arenaCenter + Vector2.One.RotatedBy(MathHelper.TwoPi * i / 4f + RotationTimer * 0.05f) * (BoundarySize + 200),
                        Vector2.Zero, grow, 90, 7, -1, 0, NPC.whoAmI);

                    Projectile.NewProjectile(NPC.GetSource_FromAI(), arenaCenter + Vector2.One.RotatedBy(MathHelper.TwoPi * i / 4f - RotationTimer * 0.05f) * (BoundarySize + 200),
                        Vector2.Zero, shrink, 90, 7, -1, 0, NPC.whoAmI);
                }
            }


            if (Timer_AI > 200 && Timer_AI < (AttackDuration - 30))
            {
                if (++ProjectileCD[2] % 60 == 0)
                {
                    int am = 8;
                    for (int i = 0; i < am; i++)
                    {
                        Vector2 vel2 = vel.RotatedBy(MathHelper.TwoPi * i / (float)am);
                        int p = Projectile.NewProjectile(NPC.GetSource_FromAI(), arenaCenter + Vector2.One.RotatedBy(vel.ToRotation() - MathHelper.PiOver4 + MathHelper.TwoPi * i / am) * 220f * skyFade, vel2 * 4f, ModContent.ProjectileType<SunBeam>(), 140, 2f, -1, -90, NPC.whoAmI, ProjReverse);
                        Main.projectile[p].extraUpdates = 2;

                        int p2 = Projectile.NewProjectile(NPC.GetSource_FromAI(), arenaCenter + Vector2.One.RotatedBy(vel.ToRotation() - MathHelper.PiOver4 + MathHelper.TwoPi * i / am) * 220f * skyFade, vel2 * 4f, ModContent.ProjectileType<SunBeam>(), 140, 2f, -1, -90, NPC.whoAmI, -ProjReverse);
                        Main.projectile[p2].extraUpdates = 2;
                    }
                    ProjReverse = -ProjReverse;
                }

                Main.instance.CameraModifiers.Add(new PunchCameraModifier(new Vector2(Main.rand.Next(-10, 10)),
                    new Vector2(Main.rand.NextFloatDirection()), 2.5f, 5, 5, -1, null));

                //Make ambient noise
                SoundStyle style = Main.rand.NextBool(2) ? ModSoundstyles.SeraphStar01 : ModSoundstyles.SeraphStar02;
                if (Main.rand.NextBool(7))
                    SoundEngine.PlaySound(style with { Volume = 2.5f, MaxInstances = 0, PitchRange = (-1.5f, 0.1f) });

            }

            if (Timer_AI > (AttackDuration - 60))
                KillClutter();
        }

        public void PillarStorm()
        {
            AttackDuration = 300;
            int cd = 150;
            int cd2 = 90;

            CurrentAttack = Language.GetTextValue("Mods.TranscendenceMod.SeraphAttackNames.Pillars");
            Attack = SeraphAttacks.PillarsOfCreation;

            if (Timer_AI == 5)
                Teleport(3, 0, -350);

            //Tilt a bit when "moving"
            NPC.rotation = player.velocity.X * 0.025f;
            NPC.velocity = Vector2.Zero;

            bool boolean = ProjectileCD[0] == 170;

            if (Timer_AI > 15)
                NPC.Center = NPC.Center.MoveTowards(player.Center + new Vector2(player.velocity.X * 15, -350), 20);

            if (++ProjectileCD[0] == cd2)
            {
                SoundEngine.PlaySound(SoundID.AbigailUpgrade with
                {
                    MaxInstances = 0,
                    Volume = 0.95f
                }, NPC.Center);

                //Spawn pillars downwards
                for (int i = 0; i < 1; i++)
                {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(95 + (i * 330), -i * 170),
                        new Vector2(5, 35), pillar, 150, 5, -1, 1, 0, 1);
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center - new Vector2(95 + (i * 330), i * 170),
                        new Vector2(-5, 35), pillar, 150, 5, -1, 1, 0, -1);
                }
            }

            if ((ProjectileCD[0] > (cd - 20) && ProjectileCD[0] < cd || ProjectileCD[0] > (cd2 - 20) && ProjectileCD[0] < cd2) && Timer_AI > 20)
            {
                HandRotationRight -= 4;
                HandRotationLeft += 4;
            }

            else if ((ProjectileCD[0] < 20 || ProjectileCD[0] > cd && ProjectileCD[0] < (cd + 20)) && Timer_AI > 20)
            {
                HandRotationRight += 4;
                HandRotationLeft -= 4;
            }

            if (ProjectileCD[0] == cd || boolean)
            {
                SoundEngine.PlaySound(SoundID.AbigailUpgrade with
                {
                    MaxInstances = 0,
                    Volume = 0.95f
                }, NPC.Center);

                //Spawn pillars horizontally, the pillars will create "explosions" when intersecting
                if (!boolean)
                {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), player.Center + new Vector2(520, 85),
                    new Vector2(-12.5f, 0), pillar, 150, 5, -1, 1, 0, 0);
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), player.Center - new Vector2(520, -85),
                        new Vector2(12.5f, 0), pillar, 150, 5, -1, 1, 0, 0);
                }
                else
                {
                    if (NotClassic)
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(175, -180),
                                new Vector2(7.5f + (i * 25), 75f), pillarlaser, 110, 5, -1, 1, 0, 1);
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center - new Vector2(175, 180),
                                new Vector2(-7.5f - (i * 25), 75f), pillarlaser, 110, 5, -1, 1, 0, -1);
                        }
                    }
                }
            }
            if (ProjectileCD[0] > 231)
            {
                ProjectileCD[0] = 0;
            }
        }

        public void StarBomb()
        {
            AttackDuration = 620;
            NPC.velocity = Vector2.Zero;
            NPC.Center = NPC.Center;

            CurrentAttack = Language.GetTextValue("Mods.TranscendenceMod.SeraphAttackNames.BigCrunch");
            Attack = SeraphAttacks.BigCrunch;

            if (Timer_AI < 60) HomingStarAuraSize += 0.012f;
            else if (Timer_AI > (AttackDuration - 30)) HomingStarAuraSize -= 0.024f;

            Ring(NPC.Center);
            BoundarySize = 1050;
            DustColor = new Color(0, 100, 200);

            if (Timer_AI < 50 || Timer_AI > (AttackDuration - 45))
            {
                CrushProjDelayTimer = 225;
                return;
            }

            if (++CrushProjDelayTimer > 456)
            {
                ProjectileCD[2] += 45;
                SoundEngine.PlaySound(SoundID.Item14 with { Pitch = -0.5f, Volume = 2 });

                int amount = 8;
                int am = NotClassic ? 2 : 1;
                //Create a star shaped bullet hell
                for (int k = 0; k < am; k++)
                {
                    float rot = Main.rand.NextFloat(MathHelper.TwoPi);
                    for (int i = 0; i < 10; i++)
                    {
                        for (int j = 0; j < amount; j++)
                        {
                            Vector2 pos = NPC.Center + Vector2.One.RotatedBy(rot + MathHelper.TwoPi * j / amount + MathHelper.ToRadians(i * 8f + (k * 45f / 2f))) * (float)(50 + Math.Sin(i) * 45f);
                            int p = Projectile.NewProjectile(NPC.GetSource_FromAI(), pos, Vector2.Zero, stellarfireball, 115, 1f, -1, 0, NPC.whoAmI, 0.33f);
                            Main.projectile[p].velocity = pos.DirectionTo(NPC.Center) * ((-3.75f * (1f + (k * 0.66f))) - (float)Math.Sin(i / 6f));
                        }
                    }
                }

                SoundEngine.PlaySound(SoundID.AbigailUpgrade with
                {
                    MaxInstances = 0,
                    Volume = 0.95f
                }, NPC.Center);

                for (int i = 0; i < 48; i++)
                {
                    Main.instance.CameraModifiers.Add(new PunchCameraModifier(new Vector2(Main.rand.Next(-20, 20)),
                        new Vector2(Main.rand.NextFloatDirection()), 120, 15, 15, -1, null));
                }
                CrushProjDelayTimer = 0;
            }

            if (CrushProjDelayTimer == 85)
                SoundEngine.PlaySound(SoundID.Item43);

            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile proj = Main.projectile[i];
                if (CrushProjDelayTimer > 110 && CrushProjDelayTimer < 120 && proj.type == ModContent.ProjectileType<BigCrunchStar>() && proj.ai[2] == 0 && proj.active)
                {
                    // Return
                    int p = Projectile.NewProjectile(NPC.GetSource_FromAI(), proj.Center, Vector2.Zero, ModContent.ProjectileType<BigCrunchStar>(),
                        90, 0f, -1, proj.ai[0], NPC.whoAmI, 1);
                    Main.projectile[p].GetGlobalProjectile<TranscendenceProjectiles>().StupidInt = proj.GetGlobalProjectile<TranscendenceProjectiles>().StupidInt;

                    proj.Kill();
                }
                if (CrushProjDelayTimer == 436 && proj.type == BigCrunchSpawnProj && proj.active && proj != null)
                {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), proj.Center, NPC.DirectionTo(proj.Center), ModContent.ProjectileType<BigCrunchStar>(), 100, 0f, -1, proj.ai[0], NPC.whoAmI, 0f);
                    proj.Kill();
                }
            }

            if (CrushProjDelayTimer > 325 && CrushProjDelayTimer < 425)
            {
                float dist = (float)Math.Cos(CrushProjDelayTimer / 50f);

                for (int i = 0; i < 5; i++)
                {
                    Vector2 vec = Vector2.One.RotatedBy(MathHelper.TwoPi * i / 5f + MathHelper.ToRadians(Timer_AI * 4f)) * (dist * 900);
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + vec, Vector2.Zero, BigCrunchSpawnProj, 0, 1, -1, dist, NPC.whoAmI);
                }
            }
        }

        public float suckStrenght()
        {
            return MathHelper.Lerp(0.25f, 0.75f, 1f + (float)Math.Sin(TranscendenceWorld.UniversalRotation * 5f));
        }

        public void DeathAnimation()
        {
            AttackDuration = 930;
            CurrentAttack = "Death";
            Attack = SeraphAttacks.DeathAnim;

            KillClutter();
            NPCFade = 1f;

            Music = MusicLoader.GetMusicSlot("TranscendenceMod/Miscannellous/Assets/Sounds/Music/Silence");

            Vector2 pos = Vector2.SmoothStep(player.Center, NPC.Center, skyFade);
            local.GetModPlayer<TranscendencePlayer>().cameraModifier = true;
            local.GetModPlayer<TranscendencePlayer>().cameraPos = pos;

            if (Timer_AI < (AttackDuration - 90))
            {
                if (skyFade < 1f)
                    skyFade += 1f / 90f;
            }


            if (Timer_AI == 180)
            {
                string sentence = "Mods.TranscendenceMod.Messages.SeraphBossDialog.Death";
                DialogUI.SpawnDialogCutscene(sentence, DialogBoxes.Seraph, 1, 4, NPC, new Vector2(0, -196), 90, Color.White);
            }

            if (Timer_AI > (AttackDuration - 15))
            {
                Terraria.Graphics.Effects.Filters.Scene["TranscendenceMod:FlashbangShader"].GetShader().UseColor(Color.White);
                Terraria.Graphics.Effects.Filters.Scene["TranscendenceMod:FlashbangShader"].GetShader().UseOpacity(1f);
                local.GetModPlayer<TranscendencePlayer>().FlashBangTimer = 15;
            }

        }

        public void DivineSpear()
        {
            AttackDuration = 520;
            Attack = SeraphAttacks.DivineSpear;
            CurrentAttack = Language.GetTextValue("Mods.TranscendenceMod.SeraphAttackNames.DivineSpear");

            Phase3Flight();


            if (Timer_AI > (AttackDuration - 120))
                return;

            if (++ProjectileCD[0] < 120)
            {
                if (ProjectileCD[0] < 90)
                {
                    HandRotationLeft = MathHelper.Lerp(HandRotationLeft, 180, 0.1f);
                }
                else HandRotationLeft = MathHelper.Lerp(HandRotationLeft, 10, 0.1f);

                if (ProjectileCD[0] == 90)
                {
                    int p = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.DirectionTo(player.Center) * 14f, spear, 170, 5f, -1, 0, NPC.whoAmI);
                    Main.projectile[p].localAI[2] = 0;
                    SoundEngine.PlaySound(SoundID.DeerclopsRubbleAttack, NPC.Center);
                }
            }
            else
            {
                if (ProjectileCD[0] > 240)
                {
                    ProjReverse = -ProjReverse;
                    ProjectileCD[0] = 0;
                }
            } 
        }

        public void RoyalFlash()
        {
            AttackDuration = 180 * 4;
            Attack = SeraphAttacks.RoyalFlash;
            CurrentAttack = Language.GetTextValue("Mods.TranscendenceMod.SeraphAttackNames.RoyalFlash");

            Vector2 pos = Vector2.SmoothStep(player.Center, NPC.Center - new Vector2(0, 84), skyFade * 0.75f);
            local.GetModPlayer<TranscendencePlayer>().cameraModifier = true;
            local.GetModPlayer<TranscendencePlayer>().cameraPos = pos;

            if (Timer_AI < (AttackDuration - 60))
            {
                if (skyFade < 0.66f)
                    skyFade += 0.66f / 60f;
            }
            else
            {
                if (skyFade > 0f)
                    skyFade -= 0.66f / 60f;
            }

            if (Timer_AI > 120 && Timer_AI < (AttackDuration - 120))
                ProjectileCD[0]++;
            else return;

            if (ProjectileCD[0] > 0 && ProjectileCD[0] % 60 == 0)
            {
                if (ProjectileCD[0] % 180 == 0)
                {
                    TranscendenceUtils.ProjectileRing(NPC, 4, NPC.GetSource_FromAI(), NPC.Center - new Vector2(300, 250), ModContent.ProjectileType<GenericDivineLaser>(), 125, 0f, 1f, -120f, NPC.whoAmI, 5f, -1, Main.rand.NextFloat(MathHelper.TwoPi), 1);
                    TranscendenceUtils.ProjectileRing(NPC, 4, NPC.GetSource_FromAI(), NPC.Center - new Vector2(-300, 250), ModContent.ProjectileType<GenericDivineLaser>(), 125, 0f, 1f, -120f, NPC.whoAmI, 5f, -1, Main.rand.NextFloat(MathHelper.TwoPi), 1);
                }
                TranscendenceUtils.ProjectileRing(NPC, 16, NPC.GetSource_FromAI(), NPC.Center - new Vector2(0, 88), ModContent.ProjectileType<GenericDivineLaser>(), 125, 0f, 1f, -120f, NPC.whoAmI, 5f, -1, Main.rand.NextFloat(MathHelper.TwoPi), 1);
                ProjReverse = -ProjReverse;
            }
        }

        public void MeteorFall()
        {
            AttackDuration = 570;
            Attack = SeraphAttacks.MeteorShower;
            CurrentAttack = "Meteor Fall";

            if (Timer_AI < 60)
            {
                Dashpos = player.Center;
                return;
            }


            if (Timer_AI < (AttackDuration - 60))
            {
                if (skyFade < 0.66f)
                    skyFade += 0.66f / 60f;
            }
            else
            {
                if (skyFade > 0f)
                    skyFade -= 0.66f / 60f;
                return;
            }

            if (++ProjectileCD[1] % 10 == 0 && Phase == 2)
            {
                Projectile.NewProjectile(NPC.GetSource_FromAI(), player.Center - new Vector2(Main.rand.NextFloat(-750f, 750f), 750f), new Vector2(Main.rand.NextFloat(-4f, 4f), 12.5f), ModContent.ProjectileType<SpaceRubble>(),  95, 2f, -1, 0f, NPC.whoAmI);
            }

            HandRotationLeft = MathHelper.ToDegrees(NPC.DirectionTo(Dashpos).ToRotation() - MathHelper.PiOver2);

            float rotMult = Phase == 2 ? 1f : 1.25f;

            if (++ProjectileCD[0] % 5 == 0 && ProjectileCD[0] < 120) 
            {
                float am = Phase == 2 ? 6f : 8f;
                for (int i = 0; i < am; i++)
                {
                    Vector2 pos = Dashpos + Vector2.One.RotatedBy(MathHelper.ToRadians(ProjectileCD[5]) + MathHelper.TwoPi * i / am + TranscendenceWorld.UniversalRotation * rotMult) * (15f + (ProjectileCD[0] * 8f));

                    int p = Projectile.NewProjectile(NPC.GetSource_FromAI(), pos, Vector2.Zero, ModContent.ProjectileType<Meteor>(), 120, 2f, -1, 0f, NPC.whoAmI, 0f);
                    Main.projectile[p].hostile = true;

                    Vector2 pos2 = Dashpos + Vector2.One.RotatedBy(MathHelper.ToRadians(ProjectileCD[5]) + MathHelper.TwoPi * i / am - TranscendenceWorld.UniversalRotation * rotMult) * (15f + (ProjectileCD[0] * 8f));

                    int p2 = Projectile.NewProjectile(NPC.GetSource_FromAI(), pos2, Vector2.Zero, ModContent.ProjectileType<Meteor>(), 120, 2f, -1, 0f, NPC.whoAmI, 0f);
                    Main.projectile[p2].hostile = true;
                }
            }
            if (ProjectileCD[0] > 150)
            {
                Dashpos = player.Center;
                ProjectileCD[5] = Main.rand.Next(0, 361);
                ProjectileCD[0] = 0;
            }
        }


        public void TrackingBlades()
        {
            AttackDuration = 495;
            Attack = SeraphAttacks.TrackingBlades;
            CurrentAttack = Language.GetTextValue("Mods.TranscendenceMod.SeraphAttackNames.TrackingBlades");


            if (Timer_AI < (AttackDuration - 120))
            {
                if (skyFade < 0.66f)
                    skyFade += 0.75f / 60f;
            }
            else
            {
                if (skyFade > 0f)
                    skyFade -= 0.75f / 120f;
                return;
            }


            if (Timer_AI < 45 || Timer_AI > (AttackDuration - 90))
                return;

            int cd = (int)MathHelper.Lerp(40, 15, Timer_AI / (float)(AttackDuration - 90));
            if (++ProjectileCD[0] % cd == 0)
            {
                float rot = Main.rand.NextFloat(MathHelper.TwoPi);
                Projectile.NewProjectile(NPC.GetSource_FromAI(), player.Center, Vector2.Zero, trackingSword, 100, 2f, -1, rot, NPC.whoAmI, Main.rand.NextFromList(-1, 1));
            }
        }

        public void ImpurityDetector()
        {
            AttackDuration = 485;
            Attack = SeraphAttacks.RemoteBlast;
            CurrentAttack = Language.GetTextValue("Mods.TranscendenceMod.SeraphAttackNames.RemoteBlast");

            if (Timer_AI < 45 || Timer_AI > (AttackDuration - 140))
                return;


            Phase3Flight();

            if (++ProjectileCD[0] % 60 == 0)
            {
                for (int i = 0; i < 2; i++)
                    SoundEngine.PlaySound(SoundID.Item84 with { MaxInstances = 0}, player.Center);

                if (ProjectileCD[3] % 4 == 0)
                {
                    for (int i = 0; i < 6; i++)
                    {
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), player.Center + Vector2.One.RotatedBy(MathHelper.TwoPi * i / 6f) * 200f, Vector2.Zero, impurityDetector, 110, 2f, -1, 1f, NPC.whoAmI);
                    }
                }
                else
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), player.Center, Vector2.Zero, impurityDetector, 100, 2f, -1, 0f, NPC.whoAmI);

                ProjectileCD[3]++;

            }
        }

        public void SupernovaP2()
        {
            AttackDuration = 600;
            Attack = SeraphAttacks.SupernovaP2;
            CurrentAttack = Language.GetTextValue("Mods.TranscendenceMod.SeraphAttackNames.SupernovaP2");

            skyFade = 1f;
            if (NPCFade > 0f)
                NPCFade -= 1f / 60f;

            Vector2 pos = NPC.Center - new Vector2(0, 500);
            if (Timer_AI == 10)
            {
                Projectile.NewProjectile(NPC.GetSource_FromAI(), pos, pos.DirectionTo(player.Center) * 18f, sunSupernova, 0, 0f, -1, 0, NPC.whoAmI);

                for (int p = 0; p < Main.maxProjectiles; p++)
                {
                    Projectile projectile = Main.projectile[p];
                    if (projectile != null && projectile.active && projectile.ai[1] == NPC.whoAmI && projectile.type == sun)
                    {
                        projectile.Kill();
                    }
                }
            }
        }

        public void DeathraysP3()
        {
            AttackDuration = 500;
            Attack = SeraphAttacks.LaserGrid;
            CurrentAttack = Language.GetTextValue("Mods.TranscendenceMod.SeraphAttackNames.LaserGrid");

            if (Timer_AI < (AttackDuration - 120))
            {
                if (Timer_AI < 5)
                    ProjectileCD[2] = 1;

                if (skyFade < 0.66f)
                    skyFade += 0.75f / 60f;
            }
            else
            {
                if (skyFade > 0f)
                    skyFade -= 0.75f / 120f;
                return;
            }

            if (Timer_AI > 60)
            {
                if (++ProjectileCD[1] % 45 == 0)
                {
                    float rot = Main.rand.NextFloat(MathHelper.TwoPi);
                    for (int i = 0; i < 2; i++)
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), player.Center, Vector2.Zero, trackingSword, 120, 2f, -1, MathHelper.TwoPi * i / 2f + rot, NPC.whoAmI, ProjectileCD[2] * 2.5f);

                    ProjectileCD[2] = -ProjectileCD[2];
                }
            }
            
            if (Timer_AI > 60 && ++ProjectileCD[0] % 120 == 0)
            {
                float y = Main.rand.NextFloat(-375f, 375f);
                for (int i = -1500; i < 1750; i += 250)
                {
                    Vector2 pos = player.Center - new Vector2(2000 * ProjReverse, i + y);
                    int p = Projectile.NewProjectile(NPC.GetSource_FromAI(), pos, new Vector2(5f * ProjReverse, 0f), ModContent.ProjectileType<GenericDivineLaser>(), 120, 0f, -1, -110, NPC.whoAmI, 3.75f);
                    Main.projectile[p].extraUpdates = 2;
                }
                ProjReverse = -ProjReverse;
            }
        }

        public int P3FlyTimer;
        public int P3FlyTimer2;

        public void Phase3Flight()
        {
            if (P3FlyTimer > 0)
                P3FlyTimer--;
            if (P3FlyTimer2 > 0)
                P3FlyTimer2--;

            if (NPC.Distance(player.Center) > 575 || P3FlyTimer > 0)
            {
                Vector2 vel = NPC.DirectionTo(player.Center);

                NPC.velocity.X = MathHelper.Lerp(NPC.velocity.X, vel.X * 50f, 1f / 20f);
                NPC.velocity.Y = MathHelper.Lerp(NPC.velocity.Y, vel.Y * 20f, 1f / 10f);

                if (NPC.Distance(player.Center) > 500)
                    P3FlyTimer = 25;

            }
            else if (NPC.Distance(player.Center) < 650 || P3FlyTimer2 > 0)
            {
                Vector2 vel = -NPC.DirectionTo(player.Center);

                NPC.velocity.X = MathHelper.Lerp(NPC.velocity.X, vel.X * 50f, 1f / 20f);
                NPC.velocity.Y = MathHelper.Lerp(NPC.velocity.Y, vel.Y * 20f, 1f / 10f);


                if (NPC.Distance(player.Center) < 500)
                    P3FlyTimer2 = 25;
            }
        }


        public void Stare()
        {
            AttackDuration = 100000000;
            Attack = SeraphAttacks.Stare;

            NPC.dontTakeDamage = true;
            Music = MusicID.OtherworldlyTowers;

            if (Timer_AI < 45)
            {
                float a = Timer_AI >= 20 ? MathHelper.Lerp(1f, 0f, (Timer_AI - 20) / 25f) : 1f;
                Terraria.Graphics.Effects.Filters.Scene["TranscendenceMod:FlashbangShader"].GetShader().UseColor(Color.White);
                Terraria.Graphics.Effects.Filters.Scene["TranscendenceMod:FlashbangShader"].GetShader().UseOpacity(1f * a);
                local.GetModPlayer<TranscendencePlayer>().FlashBangTimer = 5;
            }

            local.GetModPlayer<TranscendencePlayer>().cameraModifier = true;
            local.GetModPlayer<TranscendencePlayer>().cameraPos = Vector2.SmoothStep(player.Center, NPC.Center, ++ProjectileCD[1] / 60f);

            if (Timer_AI == 5)
                Teleport(3, 0, -250);


            if (NPCFade < 1 && DespawnTimer < 1 && !DeathAnim)
                NPCFade += 0.01f;

            if (Timer_AI == 55)
                DialogUI.SpawnDialogCutscene("Mods.TranscendenceMod.Messages.SeraphBossDialog.Intro", DialogBoxes.Seraph, 1, 1, NPC, new Vector2(0, -196), 90, Color.White);

            if (Timer_AI > 150)
                Timer_AI = AttackDuration + 5;
        }

        public void Stall()
        {
            AttackDuration = ActivatedPhase3Skin ? 115 : 420;
            Attack = SeraphAttacks.Stall;

            NPC.dontTakeDamage = true;

            if (!ActivatedPhase3Skin)
            {
                float a = Timer_AI >= 120 ? 0f : Timer_AI >= 60 ? MathHelper.Lerp(1f, 0f, (Timer_AI - 60) / 60f) : 1f;
                Terraria.Graphics.Effects.Filters.Scene["TranscendenceMod:FlashbangShader"].GetShader().UseColor(Color.White);
                Terraria.Graphics.Effects.Filters.Scene["TranscendenceMod:FlashbangShader"].GetShader().UseOpacity(1f * a);
                local.GetModPlayer<TranscendencePlayer>().FlashBangTimer = 5;

                local.GetModPlayer<TranscendencePlayer>().cameraModifier = true;
                local.GetModPlayer<TranscendencePlayer>().cameraPos = Vector2.SmoothStep(player.Center, NPC.Center, Timer_AI / 60f);

                if (Timer_AI == 150)
                { 
                    string sentence = "Mods.TranscendenceMod.Messages.SeraphBossDialog.Phase2";
                    DialogUI.SpawnDialogCutscene(sentence, DialogBoxes.Seraph, 1, 2, NPC, new Vector2(0, -196), 120, Color.White);
                }
            }
        }
        #endregion Behaviour

        #region Managerers
        /// <summary>
        /// TeleportStyles:
        /// 1 = In front of the player
        /// 2 = Anywhere around the player
        /// 3 = player.Center + new Vector2(Distance, posY)
        /// 4 = 2 but distance is slightly randomized (0.85x-1.33x)
        /// 5 = NPC.Center + new Vector2(Distance, posY)
        /// 6 = Vector2(Distance, posY)
        /// </summary>
        /// <param name="TeleportStyle"></param>
        /// <param name="Distance"></param>
        /// <param name="posY"></param>
        public void Teleport(int TeleportStyle, int Distance, float posY)
        {
            Vector2 pos = player.Center + new Vector2(Distance * (player.velocity.X > 0f ? 1 : player.velocity.X < 0 ? -1 : 0), posY);

            if (TeleportStyle == 2) pos = player.Center + Vector2.One.RotatedByRandom(MathHelper.TwoPi) * Distance;
            if (TeleportStyle == 3) pos = player.Center + new Vector2(Distance, posY);
            if (TeleportStyle == 4) pos = player.Center + Vector2.One.RotatedByRandom(MathHelper.TwoPi) * (Distance * Main.rand.NextFloat(0.85f, 1.33f));
            if (TeleportStyle == 5) pos = NPC.Center + new Vector2(Distance, posY);
            if (TeleportStyle == 6) pos = new Vector2(Distance, posY);

            if (Attack != SeraphAttacks.Intro)
            {
                SoundEngine.PlaySound(ModSoundstyles.SeraphTeleport with
                {
                    Volume = 0.75f,
                }, player.Center);

                int amt = (int)(NPC.Distance(pos) / 20);
                for (int i = 0; i < amt; i++)
                {
                    Dust.NewDustPerfect(Vector2.Lerp(NPC.Center, pos, i / (float)amt), ModContent.DustType<HolyDust>(), Vector2.Zero);
                }
            }

            NPC.Center = pos;
        }

        public void FollowPlayer(Vector2 offSet, float speed, float MinDistance)
        {
            Vector2 pos = player.Center + offSet;
            if (NPC.Distance(pos) > MinDistance)
                NPC.velocity = NPC.DirectionTo(pos) * speed;
        }

        public void AttackDefaultStats()
        {
            NPC.velocity = Vector2.Zero;
            NPC.rotation = 0;
            DealsDamage = false;
            NPC.hide = false;
            NPC.scale = 1;
            NPC.ShowNameOnHover = true;
            ArenaStarsDistance = 500f;

            DashVel = player.Center;

            arenaSizeShrinkAnim = 0;
            HasSetUpArena = false;
            skyFade = 0;
            BlackholeAlpha = 0;

            HandRotationRight = -10;
            HandRotationLeft = 10;

            NPC.ai[2] = 0;
            NPC.ai[3] = 0;

            DustColor = Color.BlueViolet;

            ProjectileCD[0] = 0;
            ProjectileCD[1] = 0;
            ProjectileCD[2] = 0;
            ProjectileCD[3] = 0;
            CrushProjDelayTimer = 0;
            dashBallSize = 0;
            HomingStarAuraSize = 0;
            HomingStarAuraOpacity = 1;
            ProjReverse = 1;
            Timer_AI = 0;
            RotationTimer = 0;
            RotationSpeed = 0.25f;
            CanDash = false;
            HasArena = false;
        }

        private void LaunchShards()
        {
            for (int p = 0; p < Main.maxProjectiles; p++)
            {
                Projectile projectile = Main.projectile[p];

                if ((projectile.type == galaxyshard) && projectile.active && projectile.ai[0] != 1)
                {
                    SoundEngine.PlaySound(ModSoundstyles.SeraphShards with
                    {
                        Volume = 1.15f,
                        PitchRange = (-0.15f, 0.2f),
                        MaxInstances = 0
                    }, projectile.Center);
                    projectile.ai[0] = 1;
                }
            }
        }
        private void KillClutter()
        {
            for (int p = 0; p < Main.maxProjectiles; p++)
            {
                Projectile projectile = Main.projectile[p];
                /*Clear Clutter*/
                if (projectile.ai[1] == NPC.whoAmI && projectile.hostile && (projectile.type != sun || Phase3Timer > 0 || player.dead) || projectile.type == mist || projectile.type == galaxyshard || projectile.type == stellarfireball)
                {
                    projectile.Kill();
                }
            }
        }

        private void ProjectileManagerer()
        {
            for (int p = 0; p < Main.maxProjectiles; p++)
            {
                Projectile projectile = Main.projectile[p];

                if (projectile.active && projectile.type == ModContent.ProjectileType<P2SupernovaBlackhole>() && projectile.ai[1] == NPC.whoAmI)
                {
                    NPC.Center = projectile.Center;
                }

                if (projectile.active && projectile.type == ModContent.ProjectileType<GenericDivineLaser>() && Attack == SeraphAttacks.DivineSpear && ProjectileCD[0] == 160 && projectile.ai[0] == 0)
                {
                    projectile.ai[0] = 1;
                }

                if (projectile.active && projectile.ModProjectile is GenericDivineLaser proj && Attack == SeraphAttacks.RoyalFlash && projectile.ai[0] <= -15)
                {
                    proj.rot += MathHelper.Lerp(0f, 0.0375f, projectile.ai[0] / -105f) * ProjReverse;
                }

                if (projectile.type == ModContent.ProjectileType<BigCrunchStar>() && projectile.active && Attack == SeraphAttacks.BigCrunch && projectile.ai[2] == 0)
                {
                    if (projectile.GetGlobalProjectile<TranscendenceProjectiles>().Timer < 50)
                    {
                        Vector2 vel = projectile.GetGlobalProjectile<TranscendenceProjectiles>().baseVel;
                        projectile.velocity = Vector2.Lerp(projectile.velocity, vel * 13f, 1f / 50f);
                    }
                    if (CrushProjDelayTimer > 65 && CrushProjDelayTimer < 120)
                    {
                        projectile.velocity *= 0.95f;
                        projectile.ai[2] *= 0.985f;

                        projectile.GetGlobalProjectile<TranscendenceProjectiles>().StupidInt++;
                    }
                }

                //Make moon crash in the arena
                if (projectile.type == moon && projectile.active)
                {
                    if (projectile.Distance(arenaCenter) > (BoundarySize * 2f))
                    {
                        projectile.velocity = projectile.DirectionTo(arenaCenter) * 20f;
                    }

                    for (int s = 0; s < 150; s++)
                    {
                        if (projectile.Distance(drawArenaPos[s]) < 50 && projectile.ai[2] < 1)
                        {
                            projectile.localAI[2] = 55;
                            projectile.velocity = Vector2.Zero;
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), projectile.Center, Vector2.Zero, ModContent.ProjectileType<Shockwave>(), 1500, 40, -1, 150, 150, 150);

                            SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode, projectile.Center);

                            for (int j = 0; j < 20; j++)
                            {
                                Main.instance.CameraModifiers.Add(new PunchCameraModifier(new Vector2(Main.rand.Next(-10, 10)),
                                    new Vector2(Main.rand.NextFloatDirection()), 15, 15, 15, -1, null));
                            }

                            projectile.ai[2] = Phase == 2 ? 70 : 100;
                            int amount =  10;
                            int amount2 = Phase == 2 ? 48 : 12;

                            if (Phase == 2)
                            {
                                for (int i = 0; i < amount2; i++)
                                {
                                    Projectile.NewProjectile(NPC.GetSource_FromAI(), projectile.Center, new Vector2(0, 12.5f).RotatedBy(MathHelper.TwoPi * i / amount2), moonrock, 80, 1, -1, 0, NPC.whoAmI, 1);
                                }
                            }
                            else
                            {
                                for (int i = 0; i < amount; i++)
                                {
                                    for (int j = 2; j < amount2; j++)
                                    {
                                        float baseSpeed = 7.5f;
                                        int speed2 = i / 5;
                                        float speed = baseSpeed + speed2;
                                        float curve = 2.25f;
                                        Vector2 vel = (projectile.DirectionTo(player.Center) * speed);
                                        int p2 = Projectile.NewProjectile(NPC.GetSource_FromAI(), projectile.Center, vel.RotatedBy(MathHelper.TwoPi * j / amount2 + MathHelper.ToRadians(i * curve) + MathHelper.PiOver2), moonrock, 80, 1, -1, 0, NPC.whoAmI, 1);
                                        Main.projectile[p2].GetGlobalProjectile<TranscendenceProjectiles>().StellarDirection = i;
                                        Main.projectile[p2].GetGlobalProjectile<TranscendenceProjectiles>().SpaceBossPortalProjectile = j;
                                        Main.projectile[p2].GetGlobalProjectile<TranscendenceProjectiles>().StupidInt = Timer_AI;
                                    }
                                }
                            }
                        }
                    }
                }

                if (projectile.type == mist && Attack == SeraphAttacks.BirthOfAStar)
                {
                    if (Timer_AI > (AttackDuration - 50))
                    {
                        projectile.localAI[2] = MathHelper.Lerp(projectile.localAI[2], 0f, 1f / 50f);
                        projectile.localAI[1] = 0;
                    }
                    else
                    {
                        if (projectile.localAI[2] < 90 && projectile.Distance(NPC.Center) < ProjectileCD[3] && ProjectileCD[3] > 10)
                        {
                            projectile.localAI[2]++;
                            if (projectile.localAI[2] == 30)
                                TranscendenceUtils.ParticleOrchestra(ParticleOrchestraType.StardustPunch, projectile.Center, -1);
                        }
                    }
                }
            }
        }
        #endregion Managerers

        #region Drawing
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {

            if (Attack == SeraphAttacks.SwordSlam && ProjectileCD[0] < 40 && Timer_AI < (AttackDuration - 60) && !Legendary && Timer_AI > 15)
            {
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

                int width = (int)(Math.Sin(TranscendenceWorld.UniversalRotation * 25) * 10);
                TranscendenceUtils.DrawEntity(NPC, TranscendenceWorld.CosmicPurple * NPCFade, "TranscendenceMod/Miscannellous/Assets/ExpandingTelegraph", 0f, new Rectangle((int)(NPC.Center.X - Main.screenPosition.X), (int)(NPC.Center.Y + 1010 - Main.screenPosition.Y), 48 + width, 2000), null);

                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

            }

            if (HasArena)
            {
                float ArenaSize = BoundarySize + (int)(Math.Sin(TranscendenceWorld.UniversalRotation * 4f) * 25f);
                float size = ArenaSize * 1.45f;

                Texture2D starSprite = TextureAssets.Item[ItemID.FallenStar].Value;
                Texture2D lineSprite = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/BloomLine2").Value;

                Rectangle frame = new Rectangle(0, 0, starSprite.Width, starSprite.Height / 8);
                Vector2 origin = new Vector2(starSprite.Width * 0.5f, starSprite.Height * 0.5f / 8);
                float distMult = 0.95f;
                Color twinkleCol = Color.Gold;

                //Draw the vignette
                var eff = ModContent.Request<Effect>("TranscendenceMod/Miscannellous/Assets/Shaders/Effects/SeraphArenaShader", AssetRequestMode.ImmediateLoad).Value;
                eff.Parameters["uColor"].SetValue(new Vector3(DustColor.R / 255f, DustColor.G / 255f, DustColor.B / 255f));
                eff.Parameters["uOpacity"].SetValue(12f * arenaSizeShrinkAnim);
                eff.Parameters["uImageSize0"].SetValue(new Vector2(BoundarySize));
                eff.Parameters["uTime"].SetValue(Main.GlobalTimeWrappedHourly);
                //Apply Space Texture
                Texture2D shaderImage = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/Perlin2").Value;
                Texture2D shaderImage2 = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/SerpentCrack").Value;

                Main.instance.GraphicsDevice.Textures[1] = shaderImage;

                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, eff, Main.GameViewMatrix.TransformationMatrix);

                float mult = 14f;
                eff.Parameters["uImageSize1"].SetValue(shaderImage.Size() / 8f);

                TranscendenceUtils.DrawEntity(NPC, Color.White, size * 12f, "TranscendenceMod/Miscannellous/Assets/InvisSprite", 0, arenaCenter, null);

                eff.Parameters["uImageSize1"].SetValue(shaderImage.Size() / 12f);

                Main.instance.GraphicsDevice.Textures[1] = shaderImage2;
                eff.Parameters["uTime"].SetValue(Main.GlobalTimeWrappedHourly * -0.5f);

                TranscendenceUtils.DrawEntity(NPC, Color.White, size * mult, "TranscendenceMod/Miscannellous/Assets/InvisSprite", 0, arenaCenter, null);

                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

                float flash = (float)(Math.Sin(MathHelper.ToRadians(twinkleTimer * 4f)) / 2f);
                float starCurveMult = MathHelper.Lerp(7.5f, 30f, BoundarySize / 1000f);
                for (int i = 0; i < 150; i++)
                {
                    drawArenaPos[i] = arenaCenter + Vector2.One.RotatedBy(MathHelper.TwoPi * i / 150f + MathHelper.ToRadians(arenaVisualRotation / 4f)) * ((ArenaSize * distMult) + (float)(Math.Sin((i / 2f) + (arenaVisualRotation / 22f)) * starCurveMult + ArenaStarsDistance));
                    TranscendenceWorld.seraphStarsPos[i] = drawArenaPos[i];
                    Vector2 nextLinePos = i == 1 ? drawArenaPos[0] : i == 0 ? drawArenaPos[149] : drawArenaPos[i - 1];
                    Rectangle linePos = new Rectangle((int)(drawArenaPos[i].X - Main.screenPosition.X), (int)(drawArenaPos[i].Y - Main.screenPosition.Y), 5, (int)(drawArenaPos[i].Distance(nextLinePos) * 2));
                    float lineRot = drawArenaPos[i].DirectionTo(nextLinePos).ToRotation() - MathHelper.Pi;

                    TranscendenceUtils.DrawEntity(NPC, Color.Lerp(new Color(1f, 1f, 1f, 0f), new Color(twinkleCol.R / 255f, twinkleCol.G / 255f, twinkleCol.B / 255f, 0f), flash * 2f) * 0.5f * arenaSizeShrinkAnim, lineSprite, lineRot - MathHelper.PiOver2, linePos, null, lineSprite.Size() * 0.5f);
                    TranscendenceUtils.DrawEntity(NPC, twinkleCol * 0.175f * (1f + flash) * arenaSizeShrinkAnim, 2f + flash, starSprite, MathHelper.ToRadians(arenaVisualRotation * 3), drawArenaPos[i], frame, origin);
                    TranscendenceUtils.DrawEntity(NPC, Color.White * arenaSizeShrinkAnim, 1f, starSprite, MathHelper.ToRadians(arenaVisualRotation * 3), drawArenaPos[i], frame, origin);

                    spriteBatch.End();
                    spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, eff, Main.GameViewMatrix.TransformationMatrix);

                    TranscendenceUtils.DrawEntity(NPC, twinkleCol * 0.5f * (1f + flash) * arenaSizeShrinkAnim, 1f + flash, "bloom", MathHelper.ToRadians(arenaVisualRotation * 3), drawArenaPos[i], null);
                    TranscendenceUtils.DrawEntity(NPC, Color.White * 0.5f * (1f + flash) * arenaSizeShrinkAnim, 0.5f + flash, "bloom", MathHelper.ToRadians(arenaVisualRotation * 3), drawArenaPos[i], null);

                    spriteBatch.End();
                    spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                }
            }

            //Draw Trail
            if (NPC.velocity != Vector2.Zero)
            {
                var eff = ModContent.Request<Effect>("TranscendenceMod/Miscannellous/Assets/Shaders/Effects/SeraphOutlineShader", AssetRequestMode.ImmediateLoad).Value;

                eff.Parameters["uRotation"].SetValue(1f);
                eff.Parameters["uTime"].SetValue(0.75f);
                eff.Parameters["uDirection"].SetValue(0f);
                eff.Parameters["uSaturation"].SetValue(1f);

                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, eff, Main.GameViewMatrix.TransformationMatrix);

                for (int i = 0; i < NPC.oldPos.Length; i++)
                {
                    float Fade = (NPC.oldPos.Length - i) / (float)NPC.oldPos.Length;
                    eff.Parameters["uOpacity"].SetValue(Fade);

                    eff.CurrentTechnique.Passes["SeraphOutlineTechnique2"].Apply();

                    TranscendenceUtils.DrawEntity(NPC, Color.White * NPCFade, NPC.scale, $"{Texture}", NPC.rotation, NPC.oldPos[i] + (NPC.Size / 2f), null);

                }

                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            }

            void drawOutlines(Vector2 pos, float r, float g, float b, float alpha, BlendState blend)
            {
                var eff = ModContent.Request<Effect>("TranscendenceMod/Miscannellous/Assets/Shaders/Effects/SeraphOutlineShader", AssetRequestMode.ImmediateLoad).Value;
                eff.Parameters["uOpacity"].SetValue(alpha);

                eff.Parameters["uRotation"].SetValue(r);
                eff.Parameters["uTime"].SetValue(g);
                eff.Parameters["uDirection"].SetValue(b);

                float dist = 2f;

                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Deferred, blend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, eff, Main.GameViewMatrix.TransformationMatrix);

                for (int i = 0; i < 4; i++)
                {
                    Vector2 pos2 = pos + Vector2.One.RotatedBy(MathHelper.TwoPi * i / 4f + Main.GlobalTimeWrappedHourly * 6) * dist;

                    TranscendenceUtils.DrawEntity(NPC, Color.White * NPCFade, NPC.scale, $"{Texture}", NPC.rotation, pos2, NPC.frame, NPC.frame.Size() * 0.5f);
                }

                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            }


            if (Phase == 3)
                drawOutlines(NPC.Center, Main.masterColor, Main.masterColor * 0.4f, 0.3f, NPCFade, BlendState.AlphaBlend);



            if (Attack == SeraphAttacks.HomingStars || Attack == SeraphAttacks.BigCrunch) drawAura(NPC.Center);

            Rectangle wingFrame = new Rectangle(0, WingsFrame, 176, WingsFrameHeight);

            TranscendenceUtils.RestartSB(spriteBatch, BlendState.Additive, null);

            //Draw Wings
            for (int i = 0; i < 4; i++)
            {
                Vector2 vec = Vector2.One.RotatedBy(MathHelper.TwoPi * i / 4f) * 4f;
                TranscendenceUtils.DrawEntity(NPC, Color.White * 0.66f * NPCFade, NPC.scale * 2f, $"Terraria/Images/Extra_185", NPC.rotation, NPC.Center + new Vector2(-120, 0).RotatedBy(NPC.rotation) + vec, wingFrame, wingFrame.Size() * 0.5f);
                TranscendenceUtils.DrawEntity(NPC, Color.White * 0.66f * NPCFade, NPC.scale * 2f, TextureAssets.Extra[185].Value, NPC.rotation, NPC.Center + new Vector2(120, 0).RotatedBy(NPC.rotation) + vec, wingFrame, wingFrame.Size() * 0.5f, SpriteEffects.FlipHorizontally);
            }

            TranscendenceUtils.RestartSB(spriteBatch, BlendState.AlphaBlend, null);

            //Draw the Seraph
            string spritePath = ActivatedPhase3Skin ? Texture + "_P3" : Texture;
            TranscendenceUtils.DrawEntity(NPC, Color.White * NPCFade, NPC.scale, spritePath, NPC.rotation, NPC.Center, NPC.frame, NPC.frame.Size() * 0.5f);

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);


            //Draw Halo
            for (int i = 0; i < 80; i++)
            {
                Vector2 vec = Vector2.One.RotatedBy(MathHelper.TwoPi * i / 80f - MathHelper.ToRadians(i / 24f) - Main.GlobalTimeWrappedHourly * 6f) * 30f;
                Vector2 vec2 = new Vector2(vec.X, vec.Y / 3f).RotatedBy((float)Math.Sin(Main.GlobalTimeWrappedHourly * 3f) * 0.125f);
                float alpha = 1f - (i / 90f);

                TranscendenceUtils.DrawEntity(NPC, Color.Gold * alpha * NPCFade, NPC.scale * 2f, $"{Texture}_Halo", NPC.rotation, NPC.Center - (new Vector2(0, NPC.height * 0.475f) + vec2).RotatedBy(NPC.rotation), null);
                TranscendenceUtils.DrawEntity(NPC, Color.White * alpha * NPCFade, NPC.scale * 1f, $"{Texture}_Halo", NPC.rotation, NPC.Center - (new Vector2(0, NPC.height * 0.475f) + vec2).RotatedBy(NPC.rotation), null);
            }

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);



            //Draw Stuff
            if (Attack == SeraphAttacks.Supernova && Timer_AI > 30 && Timer_AI < (AttackDuration - 100))
                DrawSwordAttack();

            drawArms(NPC.Center);

            if (Attack == SeraphAttacks.NebulaMatter)
                DrawNebulaMatter();

            if (Attack == SeraphAttacks.EventHorizon)
                BlackholeDrawer.DrawBlackhole(NPC, 4f * skyFade, spriteBatch);

            if (Attack == SeraphAttacks.SwordSlam && ProjectileCD[0] < 101 && Timer_AI > 15)
                TranscendenceUtils.DrawEntity(NPC, Color.White, NPC.scale * 2f, $"{Texture}_Blade", MathHelper.ToRadians(180) - MathHelper.PiOver4, NPC.Center + new Vector2(0, 150), null);

            if (Attack == SeraphAttacks.RoyalFlash || Attack == SeraphAttacks.LaserGrid)
            {
                for (int i = -30; i < 40; i += 10)
                    TranscendenceUtils.DrawEntity(NPC, Color.White * (skyFade * 3f), NPC.scale * 0.75f, "bloom", NPC.rotation, NPC.Center - new Vector2(i, 86), null);
            }



            void DrawNebulaMatter()
            {
                float ballSize = (CrushProjDelayTimer / 100f);

                //Request the Effect
                var eff = ModContent.Request<Effect>("TranscendenceMod/Miscannellous/Assets/Shaders/Effects/MovingNoiseTrans", AssetRequestMode.ImmediateLoad).Value;
                //Apply Shader Texture
                Texture2D shaderImage = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/SpiritShader2").Value;
                Main.instance.GraphicsDevice.Textures[1] = shaderImage;

                float rot = (Timer_AI > 200 ? DashVel.ToRotation() : NPC.DirectionTo(player.Center).ToRotation()) + MathHelper.PiOver2;

                eff.Parameters["uImageSize0"].SetValue(shaderImage.Size());
                eff.Parameters["uImageSize1"].SetValue(shaderImage.Size() * 3f);
                eff.Parameters["uTime"].SetValue(0);
                eff.Parameters["yChange"].SetValue(Main.GlobalTimeWrappedHourly / 2f);
                eff.Parameters["useAlpha"].SetValue(false);
                eff.Parameters["useExtraCol"].SetValue(true);
                eff.Parameters["extraCol"].SetValue(new Vector3(2.5f, 0f, 2.25f));

                spriteBatch.End();
                spriteBatch.Begin(default, BlendState.NonPremultiplied, Main.DefaultSamplerState, default, default, eff, Main.GameViewMatrix.TransformationMatrix);
                
                for (int i = 0; i < NPC.oldPos.Length; i++)
                {
                    float fade = i / (float)NPC.oldPos.Length;
                    TranscendenceUtils.DrawEntity(NPC, Color.White, MathHelper.Lerp(ballSize * 2.75f, 0f, fade), "TranscendenceMod/Miscannellous/Assets/GlowBloomNoBG", 0, NPC.oldPos[i] + NPC.Size / 2f, null);

                }

                TranscendenceUtils.RestartSB(spriteBatch, BlendState.AlphaBlend, null);

            }


            void DrawSwordAttack()
            {
                if (ProjectileCD[0] < SupernovaCD && Timer_AI < (AttackDuration - 100))
                {
                    Vector2 pos = ProjectileCD[0] > SupernovaCD ? Dashpos : player.Center;

                    spriteBatch.End();
                    spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);

                    Texture2D sprite = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/BloomLine").Value;

                    spriteBatch.Draw(sprite, new Rectangle(
                        (int)(NPC.Center.X - Main.screenPosition.X), (int)(NPC.Center.Y - Main.screenPosition.Y), 64,
                        1500), null,
                        TranscendenceWorld.CosmicPurple, NPC.DirectionTo(pos).ToRotation() + MathHelper.PiOver2, sprite.Size() * 0.5f, SpriteEffects.None, 0);

                    spriteBatch.End();
                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);
                }

                if (ProjectileCD[0] < SupernovaCD)
                    TranscendenceUtils.DrawEntity(NPC, Color.White, NPC.scale * 2f, $"{Texture}_Blade", (26 * NPC.direction - MathHelper.PiOver4), NPC.Center + new Vector2(160 * NPC.direction, -4), null);
                else
                {
                    TranscendenceUtils.DrawEntity(NPC, Color.White, NPC.scale * 4f, $"{Texture}_Blade", BlackholeAlpha + MathHelper.Pi - MathHelper.PiOver4, NPC.Center, null);
                    NPCFade = 0f;
                }
            }


            void drawAura(Vector2 pos)
            {
                spriteBatch.End();
                spriteBatch.Begin(default, BlendState.Additive, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);


                string path = $"Terraria/Images/Extra_34";
                Color col = Color.White;

                for (int i = 0; i < 4; i++)
                {
                    TranscendenceUtils.DrawEntity(NPC, col * 0.33f * HomingStarAuraOpacity, HomingStarAuraSize * 1.5f,
                        path, MathHelper.ToRadians(RotationTimer) * 2, pos + Vector2.One.RotatedBy(MathHelper.TwoPi * i / 4f + TranscendenceWorld.UniversalRotation * 2f) * 12.5f, null);
                    TranscendenceUtils.DrawEntity(NPC, col * 0.5f * HomingStarAuraOpacity, HomingStarAuraSize,
                        path, MathHelper.ToRadians(RotationTimer) * 2, pos + Vector2.One.RotatedBy(MathHelper.TwoPi * i / 4f + TranscendenceWorld.UniversalRotation * 2f) * 32.5f, null);
                }

                TranscendenceUtils.DrawEntity(NPC, col * HomingStarAuraOpacity, HomingStarAuraSize,
                    path, MathHelper.ToRadians(RotationTimer) * 2, pos, null);

                spriteBatch.End();
                spriteBatch.Begin(default, BlendState.AlphaBlend, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);
            }

            void drawArms(Vector2 pos)
            {
                Vector2 vec = new Vector2(45f, -34).RotatedBy(NPC.rotation);
                Vector2 vec2 = new Vector2(45f, 34).RotatedBy(NPC.rotation);

                if (Attack == SeraphAttacks.DivineSpear && ProjectileCD[0] < 90 && ProjectileCD[0] > 15)
                {
                    spriteBatch.End();
                    spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);

                    Texture2D sprite = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/BloomLine").Value;
                    Vector2 spearPos = NPC.Center + new Vector2(NPC.direction == -1 ? -60 : -20, -130);

                    spriteBatch.Draw(sprite, new Rectangle(
                        (int)(spearPos.X - Main.screenPosition.X), (int)(spearPos.Y - Main.screenPosition.Y), 48,
                        1250), null,
                        Color.Orange, spearPos.DirectionTo(player.Center).ToRotation() + MathHelper.PiOver2, sprite.Size() * 0.5f, SpriteEffects.None, 0);

                    spriteBatch.Draw(sprite, new Rectangle(
                        (int)(spearPos.X - Main.screenPosition.X), (int)(spearPos.Y - Main.screenPosition.Y), 8,
                        1000), null,
                        Color.White, spearPos.DirectionTo(player.Center).ToRotation() + MathHelper.PiOver2, sprite.Size() * 0.5f, SpriteEffects.None, 0);


                    var eff = ModContent.Request<Effect>("TranscendenceMod/Miscannellous/Assets/Shaders/Effects/SeraphOutlineShader", AssetRequestMode.ImmediateLoad).Value;
                    eff.Parameters["uOpacity"].SetValue(1f);

                    eff.Parameters["uRotation"].SetValue(1f);
                    eff.Parameters["uTime"].SetValue(1f);
                    eff.Parameters["uDirection"].SetValue(1f);

                    float dist = 2f;

                    spriteBatch.End();
                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, eff, Main.GameViewMatrix.TransformationMatrix);

                    for (int i = 0; i < 4; i++)
                    {
                        Vector2 pos2 = Vector2.One.RotatedBy(MathHelper.TwoPi * i / 4f + Main.GlobalTimeWrappedHourly * 6) * dist;

                        TranscendenceUtils.DrawEntity(NPC, Color.White, NPC.scale, "TranscendenceMod/Projectiles/NPCs/Bosses/SpaceBoss/DivineSpear",
                            MathHelper.ToRadians(HandRotationLeft) - MathHelper.PiOver2, spearPos + pos2, null, NPC.direction == -1 ? SpriteEffects.FlipVertically : SpriteEffects.None);
                    }


                    spriteBatch.End();
                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);

                    TranscendenceUtils.DrawEntity(NPC, Color.White, NPC.scale, "TranscendenceMod/Projectiles/NPCs/Bosses/SpaceBoss/DivineSpear", MathHelper.ToRadians(HandRotationLeft) - MathHelper.PiOver2,
                        spearPos, null, NPC.direction == -1 ? SpriteEffects.FlipVertically : SpriteEffects.None);
                }


                //Animation Offset
                Vector2 off = new Vector2(0, NPC.frame.Y == 0 ? 0 : NPC.frame.Y >= 350 ? 2 : 0);

                string lHand = "_LeftHand";

                string rHand = "_RightHand";

                //Draw Right Hand
                TranscendenceUtils.DrawEntity(NPC, Color.White * NPCFade, NPC.scale, $"{Texture}" + rHand, MathHelper.ToRadians(HandRotationRight), pos + vec + off, null);

                //Draw Right Shoulder
                TranscendenceUtils.DrawEntity(NPC, Color.White * NPCFade, NPC.scale, $"{Texture}_Shoulders_R", NPC.rotation, pos + off, null);


                if (Attack == SeraphAttacks.MeteorShower)
                {
                    TranscendenceUtils.DrawEntity(NPC, Color.White, 1f, Texture + "_Staff",
                        MathHelper.ToRadians(HandRotationLeft) + MathHelper.PiOver2 + MathHelper.PiOver4, pos - vec2 + off + (MathHelper.ToRadians(HandRotationLeft) + MathHelper.PiOver2).ToRotationVector2() * 180f, null, SpriteEffects.None);
                }


                //Draw Left Hand
                TranscendenceUtils.DrawEntity(NPC, Color.White * NPCFade, NPC.scale, $"{Texture}" + lHand,
                    MathHelper.ToRadians(HandRotationLeft), pos - vec2 + off, null);


                //Draw Left Shoulder
                TranscendenceUtils.DrawEntity(NPC, Color.White * NPCFade, NPC.scale, $"{Texture}_Shoulders_L", NPC.rotation, pos + off, null);
            }

            return false;
        }
        #endregion Drawing

        #region Misc

        public override void OnSpawn(IEntitySource source)
        {
            if (!TranscendenceWorld.EncouteredSeraph)
                TranscendenceWorld.EncouteredSeraph = true;

            NPC.ai[1] = 0;
            Phase = 0;
            RotationTimer = 0;
            RotationSpeed = 0;
            AttackDuration = 180;
            DespawnTimer = 0;
            SkyTransitionP3 = 0;

            DeathFade = 0;
            SkyTransitionP2 = 0;
            SkyTransitionP3 = 0;

            CurrentAttack = " ";
            Attack = SeraphAttacks.Intro;

            HandRotationRight = -10;
            HandRotationLeft = 10;

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.type == ModContent.NPCType<LateGameNPC>() && npc.active && npc != null)
                {
                    EmoteBubble.NewBubble(EmoteID.EmoteFear, new WorldUIAnchor(npc), 120);
                }
            }

            DealsDamage = false;

            DefaultStats();
        }

        public override void ModifyHitByItem(Player player, Item item, ref NPC.HitModifiers modifiers)
        {
            if (item.DamageType == DamageClass.Melee)
                modifiers.FinalDamage *= 1.25f;
        }

        public override void ModifyHitByProjectile(Projectile projectile, ref NPC.HitModifiers modifiers)
        {
            if ((projectile.DamageType == DamageClass.Melee || projectile.DamageType == DamageClass.MeleeNoSpeed)
                && Main.player[projectile.owner].heldProj == projectile.whoAmI) modifiers.FinalDamage *= 1.25f;

            if (projectile.type == ModContent.ProjectileType<ExoticRayBowTrailingShot>())
                modifiers.FinalDamage *= 0.5f;
        }

        public override bool CheckDead()
        {
            if (Phase < 3)
            {
                NPC.life = 1;
                return false;
            }

            if (!DeathAnim)
            {
                AttackDefaultStats();
                NPC.life = 1;
                NPC.ai[1] = 101;
                NPC.velocity = Vector2.Zero;
                NPC.rotation = 0;
                Timer_AI = 0;
                NPC.dontTakeDamage = true;
                DeathAnim = true;

                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<Shockwave>(), 3250, 75, -1, 255, 255, 255);

                return false;
            }
            return true;
        }
        
        public override bool PreKill()
        {
            if (!Downed.Contains(Bosses.CelestialSeraph))
                Downed.Add(Bosses.CelestialSeraph);
            return true;
        }
        public override void BossHeadSlot(ref int index)
        {
            if (NPCFade < 0.5f)
                index = -1;
            else index = ModContent.GetModBossHeadSlot("TranscendenceMod/NPCs/Boss/Seraph/CelestialSeraph_Head_Boss");
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.TranscendenceMod.Messages.Bestiary.SpaceBoss")),
            });
            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new()
            {
                CustomTexturePath = "TranscendenceMod/Miscannellous/Assets/Bestiary/CelestialSeraphBestiary",
                PortraitScale = 0.33f,
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
        }
        #endregion Misc
    }
    public class SeraphHeadBlackOut : ModItem
    {
        public override string Texture => "TranscendenceMod/NPCs/Boss/Seraph/SeraphHeadBlackOut";
    }
    public class SeraphHeadRevealed : ModItem
    {
        public override string Texture => "TranscendenceMod/NPCs/Boss/Seraph/CelestialSeraph_Head_Boss";
    }
}