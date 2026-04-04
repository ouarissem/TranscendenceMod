
using CollisionLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI.Chat;
using TranscendenceMod.Buffs;
using TranscendenceMod.Dusts;
using TranscendenceMod.Items.Armor.Hats;
using TranscendenceMod.Items.Consumables.LootBags;
using TranscendenceMod.Items.Consumables.Placeables;
using TranscendenceMod.Items.Consumables.Placeables.Decorations;
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Items.Materials.MobDrops;
using TranscendenceMod.Miscanellous.MiscSystems;
using TranscendenceMod.Miscanellous.UI.Achievements.Tasks;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Miscannellous.GlobalStuff;
using TranscendenceMod.Projectiles;
using TranscendenceMod.Projectiles.NPCs.Bosses.FrostSerpent;
using TranscendenceMod.Projectiles.NPCs.Bosses.Nucleus;
using TranscendenceMod.Projectiles.NPCs.Bosses.SpaceBoss;
using static System.Net.Mime.MediaTypeNames;
using static TranscendenceMod.TranscendenceWorld;
namespace TranscendenceMod.NPCs.Boss.Nucleus
{
    [AutoloadBossHead]
    public class ProjectNucleus : ModNPC
    {
        int nucleusBeam = ModContent.ProjectileType<NucleusGenericBeam>();
        int missile = ModContent.ProjectileType<GuidedMissile>();
        int slam = ModContent.ProjectileType<SlamShockwave>();
        int laser = ModContent.ProjectileType<NucleusLaser>();
        int mine = ModContent.ProjectileType<NucleusMine>();
        int plasma = ModContent.ProjectileType<PlasmaWave>();
        int heart = ModContent.ProjectileType<NucleusHeart>();
        int saw = ModContent.ProjectileType<Sawblade>();
        int blood = ModContent.ProjectileType<BloodFireball>();


        Player player;
        Player local = Main.LocalPlayer;

        public Vector2[] dashVels = new Vector2[10];

        //AI
        public int Timer_AI;

        public int AttackDuration;
        public int Phase;

        public int ProjectileCD;
        public int ProjectileCD2;
        public int ProjectileCD3;
        public int ProjectileCD4;

        public bool HasArena;

        public int HeartBeatTimer;
        public float HeartSize;

        public Vector2 Center;
        public float ScreenShake;

        public bool CanDealDamage;

        public int ColorResetTimer;
        public float RedAlpha = 1f;
        public float BlueAlpha = 0f;
        public float GrayAlpha = 0f;

        public int SqueezeWidthLoss;
        public int WiresMissing;

        public List<int> AttacksCompleted = new List<int>();
        public List<int> RecentAttacks = new List<int>();

        public NucleusAttacks Attacks;
        public int NextAttack = -1;

        public int[] BigHeartCDs = new int[2];
        public Vector2[] BigHeartVels = new Vector2[5];

        public int WithinLiquid;
        public float MissileContX;
        public int Phase2TransTimer;

        public bool NucleusChallenge;

        public int Damage;

        public override void SetStaticDefaults()
        {
            NPCID.Sets.MustAlwaysDraw[Type] = true;

            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Shimmer] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Venom] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Poisoned] = true;
        }
        public override void SetDefaults()
        {
            /*Stats*/
            NPC.lifeMax = 685 * 1000;
            NPC.damage = 150;
            NPC.defense = 50;
            NPC.npcSlots = 8f;
            NPC.value = Item.buyPrice(platinum: 1);
            /*Collision*/
            NPC.width = 188;
            NPC.height = 252;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            /*Audio*/
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = ModSoundstyles.SeraphBomb;
            Music = MusicID.Boss2;

            NPC.aiStyle = 0;
            NPC.netUpdate = true;
            NPC.boss = true;
            NPC.friendly = false;
            NPC.knockBackResist = 0f;
        }
        public override bool CheckActive() => false;
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * (Main.masterMode ? 0.55f : 0.575f));
        }
        public override void BossLoot(ref int potionType) => potionType = ItemID.SuperHealingPotion;
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            LeadingConditionRule normalMode = new LeadingConditionRule(new Conditions.NotExpert());

            /*Materials*/
            normalMode.OnSuccess(ItemDropRule.Common(ModContent.ItemType<SoulOfKnight>(), 1, 8, 16));
            normalMode.OnSuccess(ItemDropRule.Common(ItemID.HallowedBar, 1, 30, 45));
            normalMode.OnSuccess(ItemDropRule.Common(ItemID.LunarOre, 1, 30, 50));

            normalMode.OnSuccess(ItemDropRule.NotScalingWithLuck(ModContent.ItemType<NucleusMask>(), 7));
            normalMode.OnSuccess(ItemDropRule.NotScalingWithLuck(ModContent.ItemType<NucleusTrophyItem>(), 10));
            
            npcLoot.Add(normalMode);

            /* LootBag and Relic */
            npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<NucleusBag>()));
            npcLoot.Add(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<NucleusRelicItem>()));
        }
        public override bool CanHitPlayer(Player target, ref int cooldownSlot) => CanDealDamage;

                public CollisionSurface[] collisionSurfaces;

        public override void AI()
        {
            player = Main.player[NPC.target];

            CanDealDamage = false;

            if (player.dead)
            {
                NPC.velocity.Y -= 0.5f;
                if (NPC.Center.Y < (Center.Y - 750))
                    NPC.active = false;
                return;
            }

            Timer_AI++;

            if (NPC.ai[0] > 0)
                NPC.ai[0]--;

            if (WithinLiquid > 0)
                WithinLiquid--;

            if (collisionSurfaces == null || collisionSurfaces.Length < 4)
            {
                collisionSurfaces = new CollisionSurface[]
                {

                        //TopLeft to TopRight
                        new CollisionSurface(new Vector2(Center.X - 942, Center.Y - 495), new Vector2(Center.X + 942, Center.Y - 495), new int[]{ 1, 1, 1, 1 }),

                        //TopLeft to BottomLeft
                        new CollisionSurface(new Vector2(Center.X - 942, Center.Y - 495), new Vector2(Center.X - 942, Center.Y + 495), new int[]{ 1, 1, 1, 1 }),

                        //TopRight to BottomRight
                        new CollisionSurface(new Vector2(Center.X + 942, Center.Y - 495), new Vector2(Center.X + 942, Center.Y + 495), new int[]{ 1, 1, 1, 1 }),
                        
                        //BottomLeft to BottomRight
                        new CollisionSurface(new Vector2(Center.X - 942, Center.Y + 495), new Vector2(Center.X + 942, Center.Y + 495), new int[]{ 1, 1, 1, 1 })

                };
            }

            if (!player.dead)
            {
                for (int i = 0; i < 4; i++)
                {
                    collisionSurfaces[i].Update();
                    collisionSurfaces[i].DetectGrappleHookCollision();
                }

            }


            if (player.Center.X > (Center.X + 974) || player.Center.X < (Center.X - 974))
                player.position.X -= 24 * (player.Center.X > Center.X).ToDirectionInt();

            if (player.Center.Y > (Center.Y + 527) || player.Center.Y < (Center.Y - 527))
                player.position.Y -= (player.Center.Y > Center.Y).ToDirectionInt() * 24;

            if (player.Center.Y >= Center.Y + 470)
                player.GetModPlayer<TranscendencePlayer>().HorseshoeBonusActive = 5;



            if (HeartSize > 0f)
                HeartSize = MathHelper.Lerp(HeartSize, 0f, 1f / 60f);
            if (HeartSize < 0.075f)
                HeartSize = 0f;


            // Challenge Achievement
            if (NPC.ai[1] > 0 && NPC.ai[1] < 99 && !player.GetModPlayer<TranscendencePlayer>().NucleusLensKeybind)
                NucleusChallenge = false;

            if (NPC.ai[1] < 99 && ++HeartBeatTimer == 90)
            {
                SoundEngine.PlaySound(new SoundStyle("TranscendenceMod/Miscannellous/Assets/Sounds/NPCs/Attack/Heartbeat"));

                HeartSize += 0.25f;
                HeartBeatTimer = 0;
            }

            if (SqueezeWidthLoss > 0)
                SqueezeWidthLoss = (int)MathHelper.Lerp(SqueezeWidthLoss, 0, 1f / 60f);
            if (SqueezeWidthLoss < 5)
                SqueezeWidthLoss = 0;

            if (ColorResetTimer > 0)
                ColorResetTimer--;
            else ChangeColour(Colours.Red);

            if (ScreenShake > 0)
                ScreenShake = MathHelper.Lerp(ScreenShake, 0f, 0.1f);

            bool zoomedIn = NPC.ai[1] == 100 && ProjectileCD > 280;
            Vector2 zoomed = Vector2.Lerp(Center, NPC.Center + new Vector2(0, 46), player.GetModPlayer<TranscendencePlayer>().NucleusZoom);
            Vector2 normal = Center + Vector2.One.RotatedByRandom(MathHelper.TwoPi) * Main.rand.NextFloat(0f, ScreenShake);

            local.GetModPlayer<TranscendencePlayer>().cameraModifier = true;
            local.GetModPlayer<TranscendencePlayer>().cameraPos = zoomedIn ? zoomed : normal;

            //Projectile stuff
            ProjectileManagerer();

            NPC.rotation = MathHelper.Lerp(NPC.rotation, NPC.velocity.X * 0.05f, 1f / 20f);

            WiresMissing = (int)MathHelper.Lerp(12, 0, NPC.life / (float)NPC.lifeMax);


            if (Timer_AI == 2 && NPC.ai[1] != 99)
            {
                if (NPC.life < (NPC.lifeMax * 0.66f) && Phase < 2)
                {
                    SoundEngine.PlaySound(SoundID.Roar);
                    Phase = 2;

                    if (Main.expertMode || Main.masterMode)
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), Center + new Vector2(0, 1750), Vector2.Zero, ModContent.ProjectileType<BloodLiquid>(), 90, 2f, -1, 1f, NPC.whoAmI);

                    if (Main.netMode != NetmodeID.Server)
                    {
                        int gore = Mod.Find<ModGore>("Nucleus_CannonGore").Type;

                        Gore.NewGore(NPC.GetSource_FromAI(), NPC.Left, Main.rand.NextVector2Circular(2f, 2f), gore);
                        Gore.NewGore(NPC.GetSource_FromAI(), NPC.Right, Main.rand.NextVector2Circular(2f, 2f), gore);
                    }
                }

                int max = Phase == 2 ? 13 : 9;

                //Determine the next attack, no repeats
                NextAttack = Main.rand.Next(1, max);


                if (AttacksCompleted.Count >= (max - 2))
                    AttacksCompleted.Clear();

                int tries = 0;
                while (NextAttack == NPC.ai[1] || AttacksCompleted.Count > 0 && AttacksCompleted.Contains(NextAttack) || RecentAttacks.Count > 0 && RecentAttacks.Contains(NextAttack))
                {
                    //Include failsafe to prevent any freezes
                    if (++tries < 10000)
                        NextAttack = Main.rand.Next(1, max);
                    else break;
                }
            }

            void Reset()
            {
                NPC.velocity = Vector2.Zero;
                NPC.rotation = 0;
                NPC.damage = Damage;

                NPC.alpha = 0;
                CanDealDamage = false;

                NPC.ai[2] = 0;
                NPC.ai[3] = 0;

                for (int i = 0; i < 10; i++)
                    dashVels[i] = Vector2.Zero;

                ProjectileCD = 0;
                ProjectileCD2 = 0;
                ProjectileCD3 = 0;
                ProjectileCD4 = 0;
            }

            if (Timer_AI > (AttackDuration + 3) && NPC.ai[1] != 99)
            {
                Reset();

                if (MissileContX < 0.1f)
                    MissileContX = 0;

                if (MissileContX > 0)
                {
                    MissileContX = MathHelper.Lerp(MissileContX, 0, 1f / 30f);
                    return;
                }


                NPC.ai[1] = NextAttack > 0 ? NextAttack : NPC.ai[1] + 1;
                if (NPC.ai[1] > 0)
                {
                    AttacksCompleted.Add((int)NPC.ai[1]);
                    if (RecentAttacks.Count > 3)
                        RecentAttacks.RemoveAt(0);
                    RecentAttacks.Add((int)NPC.ai[1]);
                }


                Timer_AI = 0;
            }




            if (NPC.life < (NPC.lifeMax * 0.1f) && NPC.ai[1] < 99)
            {
                NPC.dontTakeDamage = true;

                if (NPC.life > 1)
                {
                    NPC.life = (int)MathHelper.Lerp(NPC.life, 1, 1f / 60f);
                    if (NPC.life < (NPC.lifeMax * 0.025f))
                        NPC.life = 1;
                    return;
                }

                NPC.ai[1] = 100;
                Timer_AI = 0;

                Reset();
                Phase = 3;
            }



            if (Phase == 2 && Phase2TransTimer < 150 && (Main.expertMode || Main.masterMode))
            {
                Phase2TransTimer++;


                Timer_AI = 0;
                Reset();


                if (NPC.Distance(Center) < 25)
                    NPC.Center = NPC.Center.MoveTowards(Center, 1f / 30f);

                // I was gonna make the blood change colour alongside the boss, but I forgot I had a white mode for the boss...
                Dust.NewDustPerfect(NPC.Center + new Vector2(58, 12), ModContent.DustType<NucleusBlood>(), new Vector2(1.5f, -1f), 0, Color.Red, 1.5f);
                Dust.NewDustPerfect(NPC.Center - new Vector2(58, -12), ModContent.DustType<NucleusBlood>(), new Vector2(-1.5f, -1f), 0, Color.Red, 1.5f);

                return;
            }

            if ((Main.expertMode || Main.masterMode) && Phase == 2)
            {
                local.AddBuff(ModContent.BuffType<InfiniteFlight>(), 1);

                if (Main.rand.NextBool(15))
                {
                    int dir = Main.rand.NextFromList(-1, 1);
                    Dust.NewDustPerfect(NPC.Center + new Vector2(62 * dir, 12), ModContent.DustType<NucleusBlood>(), new Vector2(1.5f * dir, -1f), 0, Color.Red, 1f);
                }
            }

            //Attack Patterns
            switch (NPC.ai[1])
            {
                case 0: Intro(); break;
                case (int)NucleusAttacks.Homing: Homing(); break;
                case (int)NucleusAttacks.RingBeams: RingBeams(); break;
                case (int)NucleusAttacks.Slam: Slam(); break;
                case (int)NucleusAttacks.AimedBeams: Beams(); break;
                case (int)NucleusAttacks.Swing: Swing(); break;
                case (int)NucleusAttacks.Minefield: MineField(); break;
                case (int)NucleusAttacks.PlasmaShots: PlasmaShots(); break;
                case (int)NucleusAttacks.GridSlam: LaserPillars(); break;
                case (int)NucleusAttacks.Hearts1: Hearts1(); break;
                case (int)NucleusAttacks.Saws: Sawblades(); break;
                case (int)NucleusAttacks.FleshPrison: ThisPrisonToHoldMe(); break;
                case (int)NucleusAttacks.LaserBarrage: LaserBarrage(); break;

                case 99: DeathAnim(); break;
                case 100: Finale(); break;
            }

            //Despawn
            if (player.active == false || player.dead == true)
            {
                NPC.active = false;
            }

        }

        public void Intro()
        {
            AttackDuration = 200;

            if (++ProjectileCD < 120)
                NPC.Center = Vector2.Lerp(NPC.Center, Center, 1f / 30f);

            if (ProjectileCD > 150 && ProjectileCD % 10 == 0)
            {
                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<Shockwave>(), 2500, 50, -1, 1f, 0, 0);
                SoundEngine.PlaySound(SoundID.ForceRoarPitched with { MaxInstances = 0 }, NPC.Center);
            }
        }

        public void Phase2HeartPassive()
        {
            if (++BigHeartCDs[0] > 150)
            {
                Vector2 randPos = Vector2.One.RotatedByRandom(MathHelper.TwoPi) * 1000f;

                BigHeartVels[0] = (Center + randPos).DirectionTo(Center);
                BigHeartVels[1] = Center + randPos;

                for (int i = 0; i < 32; i++)
                {
                    for (int j = -1; j < 2; j += 2)
                    {
                        if (j == 1 && i < 18 && i > 2 || j == -1 && (i < 6 || i > 22))
                            continue;

                        Vector2 startPos = Center + randPos;
                        Vector2 vel = startPos.DirectionTo(Center) * 12.5f;

                        Vector2 vec = Vector2.One.RotatedBy(MathHelper.TwoPi * i / 32f);

                        float mult = 1f;
                        if (j == 1 && (i == 17 || i == 18) || j == -1 && (i == 22 || i == 23))
                            mult = 1.2f;

                        Vector2 pos = new Vector2(vec.X * 175f, vec.Y * 75f * mult).RotatedBy(MathHelper.PiOver4 * j + vel.ToRotation() + MathHelper.PiOver2);

                        int p = Projectile.NewProjectile(NPC.GetSource_FromAI(), startPos + pos, vel, heart, 95, 2f, -1, BigHeartCDs[1], NPC.whoAmI, i);
                        Main.projectile[p].timeLeft = 240;
                        Main.projectile[p].localAI[2] = 1;
                    }
                }

                BigHeartCDs[1] = BigHeartCDs[1] == 0 ? 2 : BigHeartCDs[1] == 2 ? 1 : 2;
                BigHeartCDs[0] = 0;
            }
        }

        public void MissileHearts()
        {
            for (int i = 0; i < 32; i++)
            {
                for (int j = -1; j < 2; j += 2)
                {
                    if (j == 1 && i < 18 && i > 2 || j == -1 && (i < 6 || i > 22))
                        continue;

                    Vector2 startPos = NPC.Center + new Vector2(0, 62);
                    Vector2 vec = Vector2.One.RotatedBy(MathHelper.TwoPi * i / 32f);

                    Vector2 pos = new Vector2(vec.X * 45f, vec.Y * 15f).RotatedBy(MathHelper.PiOver4 * j + MathHelper.Pi);
                    Vector2 pos2 = startPos + pos;
                    Vector2 vel = pos2.DirectionTo(startPos) * -2f;

                    Projectile.NewProjectile(NPC.GetSource_FromAI(), pos2, vel, missile, 95, 2f, -1, 1f, NPC.whoAmI);
                }
            }
        }

        public void Homing()
        {
            AttackDuration = 140;
            Attacks = NucleusAttacks.Homing;

            if (++ProjectileCD % 45 == 0)
                TranscendenceUtils.ProjectileRing(NPC, 8, NPC.GetSource_FromAI(), NPC.Center, missile, 95, 2f, 1f, 0f, NPC.whoAmI, 0f, -1, Main.rand.NextFloat(MathHelper.TwoPi));
        }

        public void Beams()
        {
            AttackDuration = 130;
            Attacks = NucleusAttacks.AimedBeams;

            if (++ProjectileCD % 30 == 0 && Timer_AI > 2)
                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.DirectionTo(player.Center) * 4f, nucleusBeam, 125, 2f, -1, -20, NPC.whoAmI);
        }

        public void Slam()
        {
            AttackDuration = 270;
            Attacks = NucleusAttacks.Slam;

            NPC.damage = Damage;
            ChangeColour(Colours.Gray);

            NPC.velocity *= 0.9f;

            if (Timer_AI < 45)
            {
                NPC.velocity = NPC.DirectionTo(player.Center + player.velocity * 20f - new Vector2(0, 425)) * 10f;
            }

            if (Timer_AI > 60 && Timer_AI < 80)
                NPC.velocity.Y = -15f;

            if (Timer_AI > 55 && NPC.ai[3] != 1)
                SqueezeWidthLoss = 100;

            if (Timer_AI > 80)
            {
                if (NPC.ai[3] != 1)
                {
                    CanDealDamage = true;
                    NPC.velocity.Y = 100f;

                    if (NPC.Center.Y > (Center.Y + 465) && NPC.position.Y > (player.position.Y - 20))
                    {
                        SoundEngine.PlaySound(ModSoundstyles.SeraphSpear);

                        ScreenShake = 250f;

                        if (WithinLiquid > 0)
                        {
                            for (int i = 0; i < 36; i++)
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Main.rand.NextVector2Unit(MathHelper.PiOver4, MathHelper.PiOver2) * -Main.rand.NextFloat(10f, 16f), blood, 95, 2f, -1, -20, NPC.whoAmI);
                        }
                        else Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Top, Vector2.Zero, slam, 200, 2f, -1, -20, NPC.whoAmI);

                        NPC.velocity.Y = 0;
                        NPC.ai[3] = 1;
                    }
                }
                else
                {
                    if (Timer_AI > 200 && (NPC.Center.Y > (Center.Y + 50) || NPC.Center.Y < (Center.Y - 50)))
                        NPC.velocity.Y = NPC.DirectionTo(Center).Y * 6f;
                }
            }
        }

        public void RingBeams()
        {
            AttackDuration = 180;
            Attacks = NucleusAttacks.RingBeams;

            if (Timer_AI == 90)
                TranscendenceUtils.ProjectileRing(NPC, 12, NPC.GetSource_FromAI(), NPC.Center, nucleusBeam, 130, 2f, 1f, -30, NPC.whoAmI, 0, -1, Main.rand.NextFloat(MathHelper.TwoPi));
        }

        public void Swing()
        {
            AttackDuration = 450;
            Attacks = NucleusAttacks.Swing;

            if (Timer_AI < 90)
            {
                ProjectileCD = -45;
                if (NPC.Distance(Center) > 50)
                    NPC.velocity = NPC.DirectionTo(Center) * 12f;
                else NPC.velocity *= 0.9f;
            }

            else
            {
                if (ProjectileCD2 < 125)
                    ProjectileCD2++;

                CanDealDamage = Timer_AI > 180;


                ProjectileCD4 = (int)(player.Center.Y - 200 - Center.Y);

                if (ProjectileCD2 > 124)
                {
                    dashVels[3].X = MathHelper.Lerp(dashVels[3].X, player.Center.X - Center.X, 1f / 60f);
                    dashVels[3].Y = MathHelper.Lerp(dashVels[3].Y, ProjectileCD4, 1f / 90f);
                }

                if (ProjectileCD4 < 10)
                    ProjectileCD4 = 10;


                Vector2 vec = Vector2.One.RotatedBy(NPC.ai[3] + MathHelper.PiOver2) * ProjectileCD2;
                vec.X *= 2.5f;

                float speed = 3.75f;
                NPC.rotation = NPC.DirectionTo(Center + (new Vector2(dashVels[3].X, dashVels[3].Y) * (ProjectileCD2 / 125f)) - new Vector2(0f, 625f)).ToRotation() + MathHelper.PiOver2;
                NPC.ai[3] = MathHelper.ToRadians(ProjectileCD + (float)(Math.Sin(TranscendenceWorld.UniversalRotation * speed) * 80f));
                NPC.Center = Center + new Vector2(dashVels[3].X, dashVels[3].Y) + vec;
            }
        }

        public void MineField()
        {
            AttackDuration = 165;
            Attacks = NucleusAttacks.Minefield;

            if (++ProjectileCD % 3 == 0 && Timer_AI < 120)
            {
                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Main.rand.NextVector2Circular(95f, 95f), mine, 150, 1, -1, 0, NPC.whoAmI);
            }
        }

        public void LaserPillars()
        {
            AttackDuration = 285;
            Attacks = NucleusAttacks.GridSlam;

            if (ProjectileCD2 != 1)
                NPC.velocity.Y = -10f;

            if (NPC.Center.Y < (Center.Y - 625) && ProjectileCD2 != 1)
            {
                bool l = Main.rand.NextBool();
                for (int i = -1000; i < 1000; i += 200)
                {
                    Vector2 pos = Center - new Vector2(i, 650);
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), pos, new Vector2(0, 5), nucleusBeam, 130, 0f, -1, -45, NPC.whoAmI);
                }

                NPC.position.X = player.Center.X;

                ProjectileCD4 = 1;
                ProjectileCD2 = 1;
            }
            
            if (ProjectileCD2 == 1)
            {
                ProjectileCD++;

                if (NPC.ai[3] != 1)
                    SqueezeWidthLoss = 100;

                if (ProjectileCD > 58)
                {
                    if (NPC.ai[3] != 1)
                    {
                        CanDealDamage = true;
                        NPC.velocity.Y = 40f;

                        if (NPC.Center.Y > (Center.Y + 465))
                        {
                            SoundEngine.PlaySound(ModSoundstyles.SeraphSpear);
                            ScreenShake = 175f;

                            for (int l = -1000; l < 1400; l += 400)
                            {
                                ProjectileCD4 = -ProjectileCD4;

                                Vector2 randPos = new Vector2(l, 750);

                                for (int i = 0; i < 16; i++)
                                {
                                    for (int j = -1; j < 2; j += 2)
                                    {
                                        if (j == 1 && i < 11 && i > 3 || j == -1 && i > 1 && i < 9)
                                            continue;

                                        Vector2 startPos = Center + randPos;
                                        Vector2 vel = new Vector2(0f, ProjectileCD4 < 0 ? -20f : -17.5f);

                                        Vector2 vec = Vector2.One.RotatedBy(MathHelper.TwoPi * i / 16f);

                                        Vector2 pos = new Vector2(vec.X * 105f, vec.Y * 55f).RotatedBy(MathHelper.PiOver4 * j + vel.ToRotation() + MathHelper.Pi);

                                        Projectile.NewProjectile(NPC.GetSource_FromAI(), startPos + pos, vel, heart, 120, 2f, -1, 1f, NPC.whoAmI, i);
                                    }
                                }
                            }

                            NPC.velocity.Y = 0;
                            NPC.ai[3] = 1;
                        }
                    }
                    else
                    {
                        if (Timer_AI > 200 && (NPC.Center.Y > (Center.Y + 50) || NPC.Center.Y < (Center.Y - 50)))
                            NPC.velocity.Y = NPC.DirectionTo(Center).Y * 6f;
                    }
                }
            }
        }

        public void PlasmaShots()
        {
            AttackDuration = 287;
            Attacks = NucleusAttacks.PlasmaShots;

            ChangeColour(Colours.Blue);

            if (Timer_AI < 75)
                return;

            if (++ProjectileCD < 150 && ++ProjectileCD2 % 20 == 0 && Timer_AI < (AttackDuration - 30))
            {
                if (Phase == 2)
                {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.DirectionTo(player.Center) * 13f, plasma, 85, 3f, -1, 0, NPC.whoAmI);
                }
                else
                {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), (NPC.Center + new Vector2(70, 12)), (NPC.Center + new Vector2(70, 12)).DirectionTo(player.Center).RotatedBy(MathHelper.ToRadians(2f)) * 10f, plasma, 100, 2f, -1, 0, NPC.whoAmI);
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), (NPC.Center + new Vector2(-70, 12)), (NPC.Center + new Vector2(-70, 12)).DirectionTo(player.Center).RotatedBy(MathHelper.ToRadians(-2f)) * 10f, plasma, 100, 2f, -1, 0, NPC.whoAmI);
                }
            }
            if (ProjectileCD > 200)
            {
                TranscendenceUtils.ProjectileRing(NPC, 8, NPC.GetSource_FromAI(), NPC.Center, missile, 105, 2f, 0.5f, 0f, NPC.whoAmI, 0f, -1, Main.rand.NextFloat(MathHelper.TwoPi));

                ProjectileCD3 = 0;
                ProjectileCD = 0;
            }

        }
        public void Hearts1()
        {
            AttackDuration = 240;
            Attacks = NucleusAttacks.Hearts1;

            
            if (++ProjectileCD % 20 == 0)
            {
                Vector2 randPos = new Vector2(Main.rand.NextFromList(-1250f, 1250f), Main.rand.NextFloat(-500f, 500f));

                for (int i = 0; i < 16; i++)
                {
                    for (int j = -1; j < 2; j += 2)
                    {
                        if (j == 1 && i < 11 && i > 3 || j == -1 && i > 1 && i < 9)
                            continue;

                        Vector2 startPos = Center + randPos;
                        Vector2 vel = new Vector2(randPos.X > 0f ? -10f : 10f, 0f);

                        Vector2 vec = Vector2.One.RotatedBy(MathHelper.TwoPi * i / 16f);

                        float mult = 1f;

                        Vector2 pos = new Vector2(vec.X * 75f, vec.Y * 30f * mult).RotatedBy(MathHelper.PiOver4 * j + vel.ToRotation() + MathHelper.Pi);

                        Projectile.NewProjectile(NPC.GetSource_FromAI(), startPos + pos, vel, heart, 95, 2f, -1, vel.X > 0f ? 2f : 1f, NPC.whoAmI, i);
                    }
                }
            }
        }

        public void Sawblades()
        {
            AttackDuration = 60 + 75;
            Attacks = NucleusAttacks.Saws;

            ChangeColour(Colours.Red);

            if (Timer_AI > 60 && ++ProjectileCD == 15)
            {
                for (int i = -45; i < 90; i += 45)
                {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.DirectionTo(player.Center) * 14f, saw, 110, 2f, -1, 0f, NPC.whoAmI, MathHelper.ToRadians(i));
                }
            }
            if (ProjectileCD > 75)
                ProjectileCD = 0;
        }

        public void ThisPrisonToHoldMe()
        {
            AttackDuration = 300;
            Attacks = NucleusAttacks.FleshPrison;

            if (Timer_AI < 60)
            {
                if (NPC.Distance(Center) > 50)
                    NPC.velocity = NPC.DirectionTo(Center) * 12f;
                else NPC.velocity *= 0.9f;
            }
            else NPC.velocity *= 0.9f;

            ProjectileCD++;

            for (int i = 0; i < 6; i++)
            {
                Vector2 pos = NPC.Center + Vector2.One.RotatedBy(MathHelper.TwoPi * i / 6f) * 175f;

                if (Timer_AI < 91 && ProjectileCD % 5 == 0)
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), pos, Vector2.Zero, ModContent.ProjectileType<PlayerGameTransRing>(), 0, 0, -1, 0, NPC.whoAmI);
                if (Timer_AI == 120)
                    NPC.NewNPC(NPC.GetSource_FromAI(), (int)pos.X, (int)pos.Y, ModContent.NPCType<HealHeart>(), 0, 0f, NPC.whoAmI);
            }
        }

        public void LaserBarrage()
        {
            AttackDuration = 380;
            Attacks = NucleusAttacks.LaserBarrage;

            ProjectileCD++;

            if (ProjectileCD % 10  == 0 && Timer_AI < (AttackDuration - 120))
            {
                SoundEngine.PlaySound(SoundID.Mech with { MaxInstances = 0 }, player.Center);
                Projectile.NewProjectile(NPC.GetSource_FromAI(), player.Center + Main.rand.NextVector2Circular(500f, 500f), Vector2.Zero, ModContent.ProjectileType<NucleusTarget>(), 0, 0, -1, TranscendenceWorld.Timer, NPC.whoAmI);
            }
        }

        public void Finale()
        {
            AttackDuration = 999999999;
            Attacks = NucleusAttacks.Hearts1;

            HeartSize = 0f;
            HeartBeatTimer = 0;
            SqueezeWidthLoss = 0;

            if (Timer_AI < 60)
            {
                if (NPC.Distance(Center) > 50)
                    NPC.velocity = NPC.DirectionTo(Center) * 12f;
                else NPC.velocity *= 0.9f;
                return;
            }
            else NPC.velocity *= 0.9f;

            NPC.Center = Center;
            Timer_AI = 90;

            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile p = Main.projectile[i];
                if (p != null && p.active && (p.ai[1] == NPC.whoAmI && p.hostile || p.type == ProjectileID.FallingStarSpawner || p.type == ProjectileID.FallingStar) && p.type != ModContent.ProjectileType<PlayerGameTrans>() && p.type != ModContent.ProjectileType<NucleusDeathBoom>() && p.type != ModContent.ProjectileType<BloodLiquid>())
                    p.Kill();
            }

            Vector2 randPos = NPC.Center + Main.rand.NextVector2Circular(128f, 128f);
            if (++ProjectileCD % 18 == 0)
                Projectile.NewProjectile(NPC.GetSource_FromAI(), randPos, Vector2.Zero, ModContent.ProjectileType<NucleusDeathBoom>(), 0, 0, -1, 0);

            if (++ProjectileCD3 % 57 == 0 && ProjectileCD < 401)
            {
                ProjectileCD4++;

                string text = Language.GetTextValue($"Mods.TranscendenceMod.Messages.Nucleus.Desperation.{ProjectileCD4}");
                Main.NewText($"[{NPC.FullName}] " + text, Color.Red);
                DialogUI.SpawnDialog(text, randPos, 90, Color.Red);
            }

            if (ProjectileCD % 5 == 0 && ProjectileCD > 60 && ProjectileCD < 400)
            {
                Projectile.NewProjectile(NPC.GetSource_FromAI(), player.Center, Vector2.Zero, ModContent.ProjectileType<PlayerGameTransRing>(), 0, 0, -1, 0, NPC.whoAmI);
            }

            if (ProjectileCD > 410 && ProjectileCD < 416)
            {
                if (ProjectileCD == 412)
                    SoundEngine.PlaySound(ModSoundstyles.SeraphBomb, player.Center);

                for (int i = 0; i < 10; i++)
                {
                    Dust.NewDustPerfect(player.Center, DustID.Blood, Main.rand.NextVector2Circular(8f, 8f), 0, default, Main.rand.NextFloat(1f, 2f));

                    if (i < 4)
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), player.Center, Main.rand.NextVector2Circular(5f, 5f), ModContent.ProjectileType<PlayerGameTrans>(), 0, 0, -1, 0, NPC.whoAmI);
                }
                player.GetModPlayer<TranscendencePlayer>().NucleusConsumed = 350;
            }

            if (ProjectileCD > 480 && ProjectileCD < 600)
            {
                float am = MathHelper.Lerp(0f, 1f, (ProjectileCD - 480) / 120f);

                player.GetModPlayer<TranscendencePlayer>().NucleusZoom = am;
                Main.GameViewMatrix.Zoom = Vector2.SmoothStep(Main.GameViewMatrix.Zoom, Main.GameViewMatrix.Zoom * 2f, am);
            }

            if (ProjectileCD == 600)
            {
                Music = MusicID.Boss1;

                player.oldPosition = NPC.Center;
                player.Center = NPC.Center;
                player.GetModPlayer<NucleusGame>().Active = true;
                player.GetModPlayer<NucleusGame>().BossEdition = true;
                player.GetModPlayer<NucleusGame>().Time = player.GetModPlayer<NucleusGame>().MaxTime;
            }
        }

        public void DeathAnim()
        {
            AttackDuration = 200;
            Attacks = NucleusAttacks.DeathAnim;

            NPC.noTileCollide = false;
            NPC.velocity.Y += 0.025f;
            NPC.rotation += 0.00325f;

            RedAlpha = MathHelper.Lerp(RedAlpha, 0f, 0.05f);
            BlueAlpha = MathHelper.Lerp(BlueAlpha, 0f, 0.05f);
            GrayAlpha = MathHelper.Lerp(GrayAlpha, 0f, 0.05f);

            if (++ProjectileCD % Main.rand.Next(3, 8) == 0)
                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + Main.rand.NextVector2Circular(128f, 128f), Vector2.Zero, ModContent.ProjectileType<NucleusDeathBoom>(), 0, 0, -1, 0, NPC.whoAmI);

            if (Timer_AI > 120)
            {
                Terraria.Graphics.Effects.Filters.Scene.Activate("TranscendenceMod:FlashbangShader");
                Terraria.Graphics.Effects.Filters.Scene["TranscendenceMod:FlashbangShader"].GetShader().UseColor(Color.White);
                Terraria.Graphics.Effects.Filters.Scene["TranscendenceMod:FlashbangShader"].GetShader().UseOpacity(MathHelper.Lerp(0f, 1f, (Timer_AI - 120) / 60f));
                Terraria.Graphics.Effects.Filters.Scene["TranscendenceMod:FlashbangShader"].GetShader().UseIntensity(1f);
                local.GetModPlayer<TranscendencePlayer>().FlashBangTimer = 5;
            }
            if (Timer_AI > (AttackDuration - 5))
            {
                for (int i = 0; i < 3; i++)
                {
                    int p = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<NucleusDeathGigaBoom>(), 0, 0, -1, 0, NPC.whoAmI);
                    Main.projectile[p].extraUpdates = i;
                }

                NPC.dontTakeDamage = false;
                if (!Downed.Contains(Bosses.ProjectNucleus))
                    Downed.Add(Bosses.ProjectNucleus);

                if (NucleusChallenge)
                    ModAchievementsHelper.CompleteChallenge(player, TaskIDs.NucleusChallenge);

                NPC.StrikeInstantKill();
            }
        }

        public enum Colours
        {
            Red, Blue, Gray
        }
        public void ChangeColour(Colours col)
        {
            if (col == Colours.Red)
            {
                RedAlpha = MathHelper.Lerp(RedAlpha, 1f, 0.05f);
                BlueAlpha = MathHelper.Lerp(BlueAlpha, 0f, 0.05f);
                GrayAlpha = MathHelper.Lerp(GrayAlpha, 0f, 0.05f);
            }
            else ColorResetTimer = 15;

            if (col == Colours.Blue)
            {
                RedAlpha = MathHelper.Lerp(RedAlpha, 0f, 0.05f);
                BlueAlpha = MathHelper.Lerp(BlueAlpha, 1f, 0.05f);
                GrayAlpha = MathHelper.Lerp(GrayAlpha, 0f, 0.05f);
            }

            if (col == Colours.Gray)
            {
                RedAlpha = MathHelper.Lerp(RedAlpha, 0f, 0.05f);
                BlueAlpha = MathHelper.Lerp(BlueAlpha, 0f, 0.05f);
                GrayAlpha = MathHelper.Lerp(GrayAlpha, 1f, 0.05f);
            }
        }

        private void ProjectileManagerer()
        {
            for (int p = 0; p < Main.maxProjectiles; p++)
            {
                Projectile projectile = Main.projectile[p];

                if (p < Main.maxNPCs)
                {
                    NPC n = Main.npc[p];
                    if (n != null && n.active && n.type == ModContent.NPCType<HealHeart>() && n.ai[3] > 1020)
                    {
                        NPC.ai[0] = 15f;
                        if (ScreenShake < 5f)
                            ScreenShake = 5f;
                    }
                }

                if (projectile != null && projectile.active)
                {
                    if (projectile.ModProjectile is NucleusTarget && projectile.ai[2] == 45)
                    {
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.DirectionTo(projectile.Center) * 24f, laser, 0, 2f, -1, projectile.ai[0], NPC.whoAmI);
                    }

                    if (projectile.ModProjectile is BloodLiquid liquid && liquid.InsideLiquid(NPC))
                    {
                        NPC.velocity *= 0.5f;
                        WithinLiquid = 5;
                    }

                    if (Attacks == NucleusAttacks.Hearts1 && projectile.type == nucleusBeam && projectile.ai[2] != 0 && projectile.ModProjectile is NucleusGenericBeam beam2)
                    {
                        beam2.rot = projectile.GetGlobalProjectile<TranscendenceProjectiles>().baseVel.ToRotation() - MathHelper.PiOver4 + (float)(Math.Sin(TranscendenceWorld.UniversalRotation * 3f * (projectile.ai[2] / -45f)) * 0.2f + MathHelper.ToRadians(projectile.ai[2]) * 0.1f);
                    }

                    if (projectile.ai[1] == NPC.whoAmI && projectile.type == nucleusBeam && projectile.ModProjectile is NucleusGenericBeam beam)
                    {
                        beam.col = new Vector3(0.825f, 0f, 0.125f);
                        beam.telegraphCol = Color.Red;

                        if (BlueAlpha > 0f)
                        {
                            beam.col = Vector3.Lerp(beam.col, new Vector3(0f, 0.5f, 0.75f), BlueAlpha);
                            beam.telegraphCol = Color.Lerp(beam.telegraphCol, Color.DeepSkyBlue, BlueAlpha);
                        }
                    }

                    if (projectile.ai[1] == NPC.whoAmI && projectile.type == mine && Timer_AI > (AttackDuration - 45) && projectile.active)
                    {
                        Vector2 pos = Vector2.Lerp(NPC.Center, projectile.Center, projectile.ai[0] / 40f);

                        Dust d = Dust.NewDustPerfect(pos, DustID.TheDestroyer, Vector2.Zero);
                        d.velocity = Vector2.Zero;
                        d.noGravity = true;

                        if (++projectile.ai[0] > 40)
                            projectile.ai[2] = 1;
                    }
                }
            }
        }
        public override bool? CanFallThroughPlatforms() => true;

        public override void OnSpawn(IEntitySource source)
        {
            Phase = 0;
            Center = NPC.Center;
            NucleusChallenge = true;

            Damage = NPC.damage;
            NPC.position.Y -= 850;
            ProjectileCD = 0;
        }

        public override void ModifyHitByProjectile(Projectile projectile, ref NPC.HitModifiers modifiers)
        {
            if (projectile.type == ProjectileID.FinalFractal) modifiers.FinalDamage *= 0.2f;
            if (projectile.IsMinionOrSentryRelated) modifiers.FinalDamage *= 0.66f;
        }
        public override void ModifyHitByItem(Player player, Item item, ref NPC.HitModifiers modifiers)
        {
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            base.HitEffect(hit);

            if (NPC.life <= 0)
            {
                if (Main.netMode != NetmodeID.Server)
                {
                    for (int i = 1; i != 4; i++)
                    {
                        int gore = Mod.Find<ModGore>($"Nucleus_Gore{i}").Type;
                        Gore.NewGore(NPC.GetSource_Death(), NPC.Center, Main.rand.NextVector2Circular(8f, 8f), gore);
                    }
                }
            }
        }

        public override bool PreKill()
        {
            if (!Downed.Contains(Bosses.ProjectNucleus))
                Downed.Add(Bosses.ProjectNucleus);

            return true;
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.TranscendenceMod.Messages.Bestiary.Nucleus")),
            });
            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new()
            {
                CustomTexturePath = "TranscendenceMod/Miscannellous/Assets/Bestiary/ProjectNucleus",
                PortraitScale = 0.66f,
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
        }
        public Color col;
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            /*int max = Phase == 2 ? 12 : 9;

            string att = AttacksCompleted.Count == 0 ? "-" : string.Join(", ", AttacksCompleted.ToArray());
            string ratt = RecentAttacks.Count == 0 ? "-" : string.Join(", ", RecentAttacks.ToArray());

            string t = "[c/ff0000:Attack Pattern:] " + att + $"   [c/4387e9:Next up: {NextAttack} ({(NucleusAttacks)NextAttack})]" +
                $"   [c/31e24d:Attacks Left: {AttacksCompleted.Count} / {max}]  " + ratt;
            ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.MouseText.Value, t, new Vector2(35, Main.screenHeight - 40), Color.White, 0f, Vector2.Zero, Vector2.One);

            */col = Color.Red;

            if (BlueAlpha > 0f)
                SetColours(Color.Lerp(col, Color.DeepSkyBlue, BlueAlpha));
            if (GrayAlpha > 0f)
                SetColours(Color.Lerp(col, Color.Gray, GrayAlpha));

            Texture2D block = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/SeraphBoundary").Value;

            TranscendenceUtils.RestartSB(spriteBatch, BlendState.Additive, null);

            Vector2 pos2 = Center - new Vector2(59 * 16, 31 * 16);
            for (int i = 0; i < (59 * 2); i++)
            {
                spriteBatch.Draw(block, pos2 + new Vector2(i * 16, 0) - Main.screenPosition, null, col, 0f, block.Size() * 0.5f, 1f, SpriteEffects.None, 0);
                spriteBatch.Draw(block, pos2 + new Vector2(i * 16, 31 * 32) - Main.screenPosition, null, col, 0f, block.Size() * 0.5f, 1f, SpriteEffects.None, 0);
            }
            for (int j = 0; j < (31 * 2 + 1); j++)
            {
                spriteBatch.Draw(block, pos2 + new Vector2(0, j * 16) - Main.screenPosition, null, col, 0f, block.Size() * 0.5f, 1f, SpriteEffects.None, 0);
                spriteBatch.Draw(block, pos2 + new Vector2(59 * 32, j * 16) - Main.screenPosition, null, col, 0, block.Size() * 0.5f, 1f, SpriteEffects.None, 0);
            }

            TranscendenceUtils.RestartSB(spriteBatch, BlendState.AlphaBlend, null);

            if (Phase == 2 && BigHeartCDs[0] < 30)
            {
                Texture2D telegraph = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/BloomLine").Value;

                Color tCol = new Color(1f, 0f, 0f, 0f);
                if (BigHeartCDs[1] == 2)
                    tCol = new Color(0f, 0.75f, 1f, 0f);

                spriteBatch.Draw(telegraph, new Rectangle(
                    (int)(BigHeartVels[1].X - Main.screenPosition.X),
                    (int)(BigHeartVels[1].Y - Main.screenPosition.Y),
                    1175,
                    3500), null,
                    tCol * 0.5f * 1f, BigHeartVels[0].ToRotation() + MathHelper.PiOver2,
                    telegraph.Size() * 0.5f,
                    SpriteEffects.None,
                    0);
            }

            if (NPC.Center.Y > (Center.Y - 575) && NPC.ai[1] != 99)
            {
                Vector2 startVec = NPC.Center - new Vector2(0, 825f);
                Vector2 vec = startVec - Main.screenPosition;

                for (int i = 0; i < 9; i++)
                {
                    Color wireCol = Color.Lerp(col, Color.Black, 0.5f);

                    Vector2 vec2 = new Vector2(60f - (SqueezeWidthLoss / 4.7f) * NPC.scale, 40f * NPC.scale).RotatedBy(MathHelper.TwoPi * i / (12f - WiresMissing) + TranscendenceWorld.UniversalRotation * 2f);

                    int x = (int)(vec.X + vec2.X);
                    int y = (int)(vec.Y + vec2.Y);

                    Rectangle rec = new Rectangle(x, y, 2, (int)(startVec.Distance(NPC.Center) * 2f));

                    if (i >= WiresMissing)
                    {
                        spriteBatch.Draw(TextureAssets.BlackTile.Value, rec, null, wireCol, startVec.DirectionTo(NPC.Center).ToRotation() + MathHelper.PiOver2, TextureAssets.BlackTile.Value.Size() * 0.5f, SpriteEffects.None, 0f);
                    }
                }
                for (int i = 0; i < 12; i++)
                {
                    Color wireCol = col;

                    Vector2 vec2 = new Vector2(60f - (SqueezeWidthLoss / 4.7f) * NPC.scale, 40f * NPC.scale).RotatedBy(WiresMissing == 11 ? MathHelper.PiOver4 * 1.25f : MathHelper.TwoPi * i / (12f - WiresMissing) + TranscendenceWorld.UniversalRotation * 2f + MathHelper.PiOver4);

                    int x = (int)(vec.X + vec2.X);
                    int y = (int)(vec.Y + vec2.Y);

                    Rectangle rec = new Rectangle(x, y, 2, (int)(startVec.Distance(NPC.Center) * 2f));


                    if (i >= WiresMissing)
                    {
                        spriteBatch.Draw(TextureAssets.BlackTile.Value, rec, null, wireCol, startVec.DirectionTo(NPC.Center).ToRotation() + MathHelper.PiOver2, TextureAssets.BlackTile.Value.Size() * 0.5f, SpriteEffects.None, 0f);
                    }
                }

            }

            void SetColours(Color colour) => col = colour;


            Rectangle dest = new Rectangle((int)NPC.Center.X, (int)NPC.Center.Y, (int)((NPC.width - SqueezeWidthLoss) * (NPC.scale + HeartSize)), (int)(NPC.height * (NPC.scale + HeartSize)));
            Color bossCol = NPC.ai[1] > 98 ? Color.Lerp(drawColor, Color.DarkRed, (float)Math.Sin(Main.GlobalTimeWrappedHourly * 1.5f)) : drawColor;

            TranscendenceUtils.DrawEntity(NPC, bossCol, Texture + "_TV_BG", NPC.rotation, dest, null, spriteBatch);

            if (NextAttack != -1)
            {
                Rectangle tvRec = new Rectangle(0, NextAttack * 28 - 28, 48, 28);
                Texture2D finaleTV = ModContent.Request<Texture2D>(Texture + "_TV_Final").Value;

                if (NPC.ai[1] == 100)
                    TranscendenceUtils.DrawEntity(NPC, Color.White, NPC.scale + HeartSize, finaleTV, NPC.rotation, NPC.Center + new Vector2(0, 44).RotatedBy(NPC.rotation), null, finaleTV.Size() * 0.5f);

                if (NPC.ai[1] < 99)
                    TranscendenceUtils.DrawEntity(NPC, Color.White, NPC.scale + HeartSize, ModContent.Request<Texture2D>(Texture + "_TV").Value, NPC.rotation, NPC.Center + new Vector2(0, 46).RotatedBy(NPC.rotation), tvRec, tvRec.Size() * 0.5f);
            }

            Rectangle destL = new Rectangle((int)NPC.Center.X - (int)MissileContX, (int)NPC.Center.Y, (int)((NPC.width - SqueezeWidthLoss) * (NPC.scale + HeartSize)), (int)(NPC.height * (NPC.scale + HeartSize)));
            Rectangle destR = new Rectangle((int)NPC.Center.X + (int)MissileContX, (int)NPC.Center.Y, (int)((NPC.width - SqueezeWidthLoss) * (NPC.scale + HeartSize)), (int)(NPC.height * (NPC.scale + HeartSize)));

            TranscendenceUtils.DrawEntity(NPC, bossCol, Texture + "_MissileBG", NPC.rotation, dest, null, spriteBatch);
            TranscendenceUtils.DrawEntity(NPC, bossCol, Texture + "_MissileLeft", NPC.rotation, destL, null, spriteBatch);
            TranscendenceUtils.DrawEntity(NPC, bossCol, Texture + "_MissileRight", NPC.rotation, destR, null, spriteBatch);

            TranscendenceUtils.DrawEntity(NPC, bossCol, Texture, NPC.rotation, dest, null, spriteBatch);


            //Glowmasks
            if (RedAlpha > 0f)
                TranscendenceUtils.DrawEntity(NPC, Color.White * RedAlpha, Texture + "_Glow_Red", NPC.rotation, dest, null, spriteBatch);
            if (BlueAlpha > 0f)
                TranscendenceUtils.DrawEntity(NPC, Color.White * BlueAlpha, Texture + "_Glow_Blue", NPC.rotation, dest, null, spriteBatch);
            if (GrayAlpha > 0f || NPC.ai[1] == 99)
                TranscendenceUtils.DrawEntity(NPC, NPC.ai[1] == 99 ? new Color(20, 20, 20) : Color.White * GrayAlpha, Texture + "_Glow_Grey", NPC.rotation, dest, null, spriteBatch);

            if (SqueezeWidthLoss < 2 && Phase < 2 && NPC.active && !NPC.IsABestiaryIconDummy)
            {
                Color cannonCol = Attacks == NucleusAttacks.PlasmaShots ? Color.White : drawColor;

                Vector2 posL = NPC.Center + new Vector2(70, 12).RotatedBy(NPC.rotation);
                Vector2 posR = NPC.Center + new Vector2(-70, 12).RotatedBy(NPC.rotation);

                if (Attacks == NucleusAttacks.PlasmaShots)
                {
                    TranscendenceUtils.VeryBasicNPCOutline(NPC, Texture + "_PlasmaCannon", 2f, 0f, 0.75f, 1f, 0.66f, posL, NPC.scale + HeartSize, posL.DirectionTo(player.Center).ToRotation() + MathHelper.ToRadians(2f));
                    TranscendenceUtils.VeryBasicNPCOutline(NPC, Texture + "_PlasmaCannon", 2f, 0f, 0.75f, 1f, 0.66f, posR, NPC.scale + HeartSize, posR.DirectionTo(player.Center).ToRotation() - MathHelper.ToRadians(2f));
                }

                TranscendenceUtils.DrawEntity(NPC, cannonCol, NPC.scale + HeartSize, Texture + "_PlasmaCannon", posL.DirectionTo(player.Center).ToRotation() + MathHelper.ToRadians(2f), posL, null);
                TranscendenceUtils.DrawEntity(NPC, cannonCol, NPC.scale + HeartSize, Texture + "_PlasmaCannon", posR.DirectionTo(player.Center).ToRotation() - MathHelper.ToRadians(2f), posR, null);

                TranscendenceUtils.DrawEntity(NPC, Color.White, NPC.scale + HeartSize, Texture + "_PlasmaCannon_Glow", posL.DirectionTo(player.Center).ToRotation() + MathHelper.ToRadians(2f), posL, null);
                TranscendenceUtils.DrawEntity(NPC, Color.White, NPC.scale + HeartSize, Texture + "_PlasmaCannon_Glow", posR.DirectionTo(player.Center).ToRotation() - MathHelper.ToRadians(2f), posR, null);
            }

            if (Phase > 1 && NPC.ai[1] != 99)
            {
                Rectangle dest2 = new Rectangle((int)NPC.Center.X, (int)NPC.Center.Y, (int)((220 - SqueezeWidthLoss) * (NPC.scale + HeartSize)), (int)(NPC.height * (NPC.scale + HeartSize)));

                TranscendenceUtils.RestartSB(spriteBatch, BlendState.Additive, null);


                TranscendenceUtils.DrawEntity(NPC, col, Texture + "_Glow_Bloom", NPC.rotation, dest2, null, spriteBatch);


                TranscendenceUtils.RestartSB(spriteBatch, BlendState.AlphaBlend, null);
            }

            if (NPC.ai[0] > 0)
            {
                float healA = 0.5f + (float)Math.Sin(Main.GlobalTimeWrappedHourly * 16f) * 0.5f;
                TranscendenceUtils.DrawEntity(NPC, Color.White * healA, Texture + "_Glow_Green", NPC.rotation, dest, null, spriteBatch);
            }


            return false;
        }
    }
}