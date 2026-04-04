using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Text;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TranscendenceMod.Buffs;
using TranscendenceMod.Items.Consumables.LootBags;
using TranscendenceMod.Items.Consumables.Placeables;
using TranscendenceMod.Items.Pets;
using TranscendenceMod.Items.Weapons.Magic;
using TranscendenceMod.Items.Weapons.Melee;
using TranscendenceMod.Items.Weapons.Ranged;
using TranscendenceMod.Items.Weapons.Summoner;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Miscannellous.UI;
using TranscendenceMod.Projectiles;
using TranscendenceMod.Projectiles.Equipment;
using TranscendenceMod.Projectiles.NPCs.Bosses.FrostSerpent;
using TranscendenceMod.Projectiles.Weapons.Magic;
using static TranscendenceMod.TranscendenceWorld;

namespace TranscendenceMod.NPCs.Boss.FrostSerpent
{
    [AutoloadBossHead]
    public class FrostSerpent_Head : HeadSegment
    {
        public Player player;
        public override int MaxSegments => 48;
        public int Segments2;
        public float rot;
        public float MawRot;
        public int AttackDuration;
        public int Timer;
        public float speed;
        public int ProjectileTimer;
        public int ProjectileTimer2;
        public Vector2 dashPos;
        public int DashDir;
        public float speedMult = 1f;
        public float extraRot;
        public int GoBackToPlayerState;
        public int Phase;
        public float DeathFade;

        public int Stamina;
        public int MaxStaminaAmount;
        public int RestTimer;

        public int NearEatable;
        public float SizeMult;
        public int SizeResetCD;

        public SerpentAttacks Attack = SerpentAttacks.HomingFrost;
        public int FrostBlastHoming = ModContent.ProjectileType<HomingFrostBlast>();
        public int FrostLaser = ModContent.ProjectileType<FrostLaser>();
        public int Icicle = ModContent.ProjectileType<SerpentIcicle>();

        public override int BodySegmentType => ModContent.NPCType<FrostSerpent_Body>();

        public override int TailSegmentType => ModContent.NPCType<FrostSerpent_Tail>();

        public override void OnSpawn(IEntitySource source)
        {
            NPC.ai[3] = 1f;
            NPC.ai[1] = -1;
            AttackDuration = 60;
        }

        public override void SetStaticDefaults()
        {
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Shimmer] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;

            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Poisoned] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Venom] = true;

            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Frostburn] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Frostburn2] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire3] = true;
            NPC.buffImmune[ModContent.BuffType<JungleRingBuff>()] = true;
        }

        public override void SetDefaults()
        {
            NPC.boss = true;
            NPC.lifeMax = 1012755;
            NPC.defense = 20;
            NPC.damage = 225;
            NPC.knockBackResist = 0;
            NPC.takenDamageMultiplier = 3f;

            NPC.width = 64;
            NPC.height = 74;

            NPC.aiStyle = -1;
            NPC.noGravity = true;
            NPC.noTileCollide = true;

            NPC.HitSound = SoundID.NPCHit5;
            NPC.DeathSound = SoundID.Shatter;
            Music = MusicID.OtherworldlyInvasion;
            NPC.BossBar = ModContent.GetInstance<FrostSerpentBossBar>();

            NPC.friendly = false;
            NPC.value = Item.buyPrice(gold: 75);
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            base.OnHitPlayer(target, hurtInfo);
            target.AddBuff(ModContent.BuffType<FrostBite>(), 180);
        }
        public override void BossLoot(ref int potionType) => potionType = ItemID.SuperHealingPotion;
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            LeadingConditionRule normalMode = new LeadingConditionRule(new Conditions.NotExpert());

            normalMode.OnSuccess(ItemDropRule.Common(ModContent.ItemType<FrostMonolithItem>(), 3));
            normalMode.OnSuccess(ItemDropRule.Common(ItemID.LunarOre, 1, 55, 70));

            normalMode.OnSuccess(ItemDropRule.FewFromOptions(2, 1,
                ModContent.ItemType<MountaintopGlacier>(),
                ModContent.ItemType<IceKunai>(),
                ModContent.ItemType<FrozenMaws>()));

            /*Loot Bag, Relic and Pet*/
            npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<FrostSerpentBag>()));
            npcLoot.Add(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<SerpentRelicItem>()));
            npcLoot.Add(ItemDropRule.MasterModeDropOnAllPlayers(ModContent.ItemType<FrozenChunk>(), 3));
        }
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.6f);
            NPC.damage = (int)(NPC.damage * 0.525f);
        }
        public List<int> curItems = new List<int>();
        public override void AI()
        {
            NPC.TargetClosest();
            player = Main.player[NPC.target];
            Main.LocalPlayer.ZoneSnow = true;

            MaxStaminaAmount = 2;

            if (player.dead)
            {
                NPC.rotation = NPC.velocity.ToRotation() + MathHelper.PiOver2;
                NPC.velocity = new Vector2(0f, 20f);

                if (NPC.Distance(player.Center) > 2000)
                    NPC.active = false;
                return;
            }

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc2 = Main.npc[i];
                if (npc2 != null && npc2.active && (npc2.type == ModContent.NPCType<FrostSerpent_Body>() || npc2.type == ModContent.NPCType<FrostSerpent_Tail>()))
                {
                    if (npc2.ai[1] == (int)(36 - (SizeResetCD / 2.5)) && SizeResetCD > 30)
                    {
                        npc2.scale = MathHelper.Lerp(1f, 2.5f * SizeMult, (SizeResetCD - 30) / 120f);
                    }
                    if (NPC.life < 15)
                    {
                        npc2.life = 1;
                        npc2.realLife = 1;
                        npc2.dontTakeDamage = true;
                        npc2.active = true;
                    }
                }
            }

            if (NPC.life < (NPC.lifeMax * 0.5f) && Phase != 2 && Timer < 5)
            {
                SoundEngine.PlaySound(SoundID.Roar, NPC.Center);
                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<Shockwave>(), 4500, 120, -1, 0, 100, 255);

                Stamina = 0;
                RestTimer = 0;
                NPC.ai[1] = 19;
                Phase = 2;
            }

            speed += 0.25f;
            if (speed > 50)
                speed = 20;

            if (Attack != SerpentAttacks.DeathAnim)
            {
                SkyManager.Instance.Activate("TranscendenceMod:FrostSky", player.Center);
                if (Phase == 2)
                {
                    Terraria.Graphics.Effects.Filters.Scene.Activate("TranscendenceMod:ColdScreen");
                    Main.LocalPlayer.GetModPlayer<TranscendencePlayer>().ColdTimer = 15;
                }
            }
            else SkyManager.Instance.Deactivate("TranscendenceMod:FrostSky");

            if (GoBackToPlayerState > 0)
                GoBackToPlayerState--;

            float speed2 = (float)Math.Sin(speed) * 50f;

            if (extraRot == 0 && Attack != SerpentAttacks.DeathAnim) NPC.rotation = NPC.velocity.ToRotation() + MathHelper.PiOver2;
            NPC npc = NPC;
            rot += 0.075f;

            if (SizeResetCD > 0)
                SizeResetCD--;

            NPC.width = (int)(64 * NPC.scale);
            NPC.height = (int)(74 * NPC.scale);

            if (Segments2 < MaxSegments)
            {
                int n = NPC.NewNPC(npc.GetSource_FromAI(), (int)npc.Center.X, (int)npc.Center.Y, BodySegmentType, npc.whoAmI);
                Main.npc[n].lifeMax = NPC.lifeMax;
                Main.npc[n].defense = NPC.defense;
                Main.npc[n].ai[0] = npc.whoAmI;
                Main.npc[n].ai[1] = Segments2;
                Main.npc[n].ai[2] = MaxSegments;

                if (Segments2 % 2 == 0 && Main.rand.NextBool(2) && curItems.Count < 7)
                {
                    int r = Main.rand.Next(1, 8);

                    int tries = 0;
                    while (curItems.Contains(r))
                    {
                        //Include failsafe to prevent any freezes
                        if (++tries < 2500)
                        {
                            r = Main.rand.Next(1, 8);
                            //Main.NewText(r);
                        }
                        else
                        {
                            //Main.NewText("FAIL");
                            break;
                        }
                    }

                    curItems.Add(r);
                    Main.npc[n].localAI[1] = r;
                }

                Main.npc[n].realLife = npc.whoAmI;

                if (Segments2 == MaxSegments - 1)
                {
                    int n2 = NPC.NewNPC(npc.GetSource_FromAI(), (int)npc.Center.X, (int)npc.Center.Y, TailSegmentType, npc.whoAmI);
                    Main.npc[n2].lifeMax = NPC.lifeMax;
                    Main.npc[n2].defense = NPC.defense;
                    Main.npc[n2].ai[0] = npc.whoAmI;
                    Main.npc[n2].ai[1] = MaxSegments;
                    Main.npc[n2].realLife = npc.whoAmI;
                }
                Segments2++;
            }

            if (player.Distance(NPC.Center) < (175 * NPC.scale) && Attack != SerpentAttacks.Laser && Attack != SerpentAttacks.MultiRays && Attack != SerpentAttacks.DeathAnim)
                NearEatable = 5;

            if (NearEatable == 0)
                MawRot = MathHelper.Lerp(MawRot, 0, 0.05f);
            else
            {
                MawRot = MathHelper.Lerp(MawRot, MathHelper.PiOver4 * 1.25f, 0.1f);
                NearEatable -= 1;
            }

            if (Attack != SerpentAttacks.Laser && Attack != SerpentAttacks.MultiRays && Attack != SerpentAttacks.DeathAnim && Attack != SerpentAttacks.Slam && SizeResetCD < 75 && !player.dead)
            {
                for (int i = 0; i < Main.maxProjectiles; i++)
                {
                    Projectile p = Main.projectile[i];
                    if (p != null && p.active && (p.type == ModContent.ProjectileType<MagicalSnowflake>() && p.scale < 2f && p.ai[2] > 90 && p.ai[1] == NPC.whoAmI || p.friendly && (Main.getGoodWorld || Main.zenithWorld)) && p.Distance(NPC.Center) < (125 * NPC.scale) && Attack != SerpentAttacks.Laser)
                    {
                        NearEatable = 5;
                        NPC.velocity = NPC.DirectionTo(p.Center) * 12f;

                        if (p.Distance(NPC.Center) < (50 * NPC.scale))
                        {
                            for (int j = 0; j < 12; j++)
                            {
                                Dust d = Dust.NewDustPerfect(NPC.Center, DustID.DungeonSpirit, (NPC.rotation - MathHelper.PiOver2).ToRotationVector2().RotatedByRandom(0.35f) * Main.rand.NextFloat(6f, 16f), 0, default, 2.25f);
                                d.noGravity = true;
                            }
                            int healAmount = (int)(NPC.lifeMax / 200f);

                            if (NPC.life < (NPC.lifeMax - healAmount))
                                NPC.life += healAmount;
                            else NPC.life = NPC.lifeMax;

                            NPC.HealEffect(healAmount);

                            SizeMult = 1.25f;
                            SizeResetCD = 90;

                            SoundEngine.PlaySound(SoundID.Shatter, NPC.Center);
                            p.Kill();
                        }
                    }
                }
            }

            if (Attack != SerpentAttacks.Slam && Attack != SerpentAttacks.Charges && Attack != SerpentAttacks.MultiRays && Attack != SerpentAttacks.DeathAnim)
            {
                float changeSpeed = 0.01f;
                float dist = Phase == 2 ? 275f : 375f;

                NPC.velocity = Vector2.Lerp(NPC.velocity, NPC.DirectionTo(player.Center + Vector2.One.RotatedBy(rot * (float)Math.Tan(rot / 50f)) * dist) * ((NPC.Distance(player.Center) * 0.1f) + speed2 * speedMult) * NPC.scale, changeSpeed * speedMult);

                if (NPC.Distance(player.Center) > 2500)
                {
                    NPC.velocity = NPC.DirectionTo(player.Center) * (NPC.Distance(player.Center) / 75f);
                    GoBackToPlayerState = 60;
                }
                if (GoBackToPlayerState < 55 && GoBackToPlayerState > 5)
                    NPC.velocity *= 0.8f;
            }

            if (Stamina != 0) Timer++;
            else
            {
                Attack = SerpentAttacks.Idle;
                int am = Phase == 2 ? 90 : 150;
                if (++RestTimer > am)
                {
                    Stamina = MaxStaminaAmount;
                    RestTimer = 0;
                }
                return;
            }

            if (Timer > AttackDuration)
            {
                if (NPC.ai[3] > 0.9f)
                {
                    Attack = SerpentAttacks.Idle;
                    NPC.ai[1]++;
                    NPC.ai[3] = 1f;
                    speedMult = 1f;
                    extraRot = 0f;
                    ProjectileTimer = 0;
                    ProjectileTimer2 = 0;
                    Stamina--;
                    RestTimer = 0;
                    Timer = 0;
                }
                else
                {
                    NPC.ai[3] = MathHelper.Lerp(NPC.ai[3], 1f, 0.033f);
                }
                return;
            }

            if (GoBackToPlayerState > 0)
                return;
            
            ProjectileTimer++;

            switch (NPC.ai[1])
            {
                case 0: HomingFrost(); break;
                case 1: ProjectileSpam(); break;
                case 2: AimedProjs(); break;
                case 3: AggressiveCharges(); break;
                case 4: CreepingSnowflakes(); break;
                case 5: Slam(); break;
                case 6: AggressiveCharges(); break;
                case 7: ProjectileSpam(); break;
                case 8: AggressiveCharges(); break;
                case 9: CreepingSnowflakes(); break;
                case 10: NPC.ai[1] = 0; goto case 0;

                case 20: ProjectileSpam(); break;
                case 21: AimedProjs(); break;
                case 22: Icicles(); break;
                case 23: DeathraysMultiple(); break;
                case 24: Slam(); break;
                case 25: AggressiveCharges(); break;
                case 26: SnowFountain(); break;
                case 27: CreepingSnowflakes(); break;
                case 28: Slam(); break;
                case 29: AggressiveCharges(); break;
                case 30: ProjectileSpam(); break;
                case 31: HomingFrost(); break;
                case 32: NPC.ai[1] = 20; goto case 20;

                case 99: DeathAnim(); break;
                case 100: DeathAnim(); break;
                case 101: DeathAnim(); break;
            }
        }
        public void HomingFrost()
        {
            AttackDuration = 330;
            Attack = SerpentAttacks.HomingFrost;

            if (ProjectileTimer % 180 == 0)
            {
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC n = Main.npc[i];
                    
                    if (n != null && n.active && n.type == BodySegmentType && n.ai[1] < (MaxSegments * 0.66f) && n.ai[1] % 3 == 0)
                    {
                        TranscendenceUtils.ProjectileRing(NPC, 4, NPC.GetSource_FromAI(), n.Center, FrostBlastHoming, 80, 1, 0.5f, 1, NPC.whoAmI, 1, -1, 0);
                    }
                }
            }
        }
        public void AimedProjs()
        {
            AttackDuration = 220;
            Attack = SerpentAttacks.Laser;

            speedMult = MathHelper.Lerp(speedMult, 0.25f, 0.05f);

            NearEatable = 5;

            if (NPC.Distance(player.Center) > 750)
                NPC.velocity = NPC.DirectionTo(player.Center) * 20;
            else NPC.velocity *= 0.95f;

            if (NPC.ai[3] > 0.25f)
                NPC.ai[3] -= 0.05f;

            if (Timer > 90)
                extraRot += 0.0125f;

            if (Timer == 90)
            {
                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.DirectionTo(player.Center), FrostLaser, 120, 200, -1, 0, NPC.whoAmI);
            }
            if (Timer > 90 && ProjectileTimer % 30 == 0 && Phase == 2)
            {
                TranscendenceUtils.ProjectileRing(NPC, 6, NPC.GetSource_FromAI(), NPC.Center, Icicle, 90, 1, 2.25f, 1, NPC.whoAmI, 1, -1, 0);
            }
        }
        public void ProjectileSpam()
        {
            AttackDuration = Phase == 2 ? 220 : 300;
            Attack = SerpentAttacks.Spam;

            int CD = Phase == 2 ? 6 : 10;
            if (++ProjectileTimer % CD == 0)
            {
                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Main.rand.NextVector2Circular(35.5f, 35.5f), ModContent.ProjectileType<MagicalSnowflake>(), 85, 1, -1, 0, NPC.whoAmI);
            }
        }
        public void SnowFountain()
        {
            AttackDuration = 320;
            Attack = SerpentAttacks.Fountain;

            if (Timer < 120)
            {
                dashPos = new Vector2(player.Center.X, player.Center.Y + 1000);
                return;
            }

            if (Main.masterMode)
                dashPos.X = MathHelper.Lerp(dashPos.X, player.Center.X, 0.01125f);

            Vector2 sine = new Vector2((float)Math.Sin(Timer * 0.125f) * -150f, 0);
            Projectile.NewProjectile(NPC.GetSource_FromAI(), dashPos + sine, new Vector2(0, -25), ModContent.ProjectileType<SerpentSnowball>(), 95, 1, -1, 0, NPC.whoAmI);
            Projectile.NewProjectile(NPC.GetSource_FromAI(), dashPos - sine, new Vector2(0, -25), ModContent.ProjectileType<SerpentSnowball>(), 95, 1, -1, 0, NPC.whoAmI);

            Projectile.NewProjectile(NPC.GetSource_FromAI(), dashPos + sine + new Vector2((float)Math.Sin(Timer * 0.25f) * 75f, 0), new Vector2(0, -25), ModContent.ProjectileType<SerpentSnowball>(), 95, 1, -1, 0, NPC.whoAmI);
            Projectile.NewProjectile(NPC.GetSource_FromAI(), dashPos - sine - new Vector2((float)Math.Sin(Timer * 0.25f) * 75f, 0), new Vector2(0, -25), ModContent.ProjectileType<SerpentSnowball>(), 95, 1, -1, 0, NPC.whoAmI);
        }
        public void Slam()
        {
            AttackDuration = 145;
            Attack = SerpentAttacks.Slam;

            if (ProjectileTimer < 90)
            {
                NPC.velocity = Vector2.Lerp(NPC.velocity, new Vector2(NPC.DirectionTo(player.Center).X, -0.75f) * 25, 0.125f);
                return;
            }

            if (ProjectileTimer2 != 1)
            {
                if (Collision.SolidCollision(NPC.position, NPC.width, NPC.height))
                {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<FrostBlast>(), 95, 1, -1, 0, NPC.whoAmI, 16f);

                    SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode);

                    ProjectileTimer2 = 1;
                }
                DashDir = NPC.Center.X > player.Center.X ? -1 : 1;
                NPC.velocity = Vector2.Lerp(NPC.velocity, new Vector2(0, Phase == 2 ? 70f : 45f), 0.075f);
                Timer = 95;
            }
            else NPC.velocity = Vector2.Lerp(NPC.velocity, new Vector2(DashDir * 0.5f, -1f) * 35, 0.075f);

        }
        public void AggressiveCharges()
        {
            AttackDuration = Phase == 2 ? 505 : 325;
            Attack = SerpentAttacks.Charges;

            if (Timer < 45 || Timer > (AttackDuration - 45))
            {
                if (NPC.Distance(player.Center) > (Timer > 45 ? 500 : 750))
                    NPC.velocity = NPC.DirectionTo(player.Center) * (NPC.Distance(player.Center) / 50f);
                else NPC.velocity *= 0.7f;

                return;
            }

            if (ProjectileTimer < 90)
            {
                if (++ProjectileTimer2 < 45)
                {
                    NPC.velocity = NPC.DirectionTo(player.Center) * (NPC.Distance(player.Center) > 750 && Timer > 120 ? 30f : -20f);
                    dashPos = NPC.DirectionTo(player.Center + player.velocity * 45f);
                }
                else
                {
                    NPC.velocity = dashPos * 90f;
                    for (int i = 0; i < 5; i++)
                        Dust.NewDust(NPC.Center, 1, 1, DustID.SnowSpray);

                    if (TranscendenceWorld.CountProjectiles(ModContent.ProjectileType<MagicalSnowflake>()) < 350 || ProjectileTimer % 2 == 0)
                    {
                        int p = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.velocity * -0.05f, ModContent.ProjectileType<MagicalSnowflake>(), 85, 1, -1, 0, NPC.whoAmI);
                        Main.projectile[p].scale = 2f;
                        Main.projectile[p].width = 104;
                        Main.projectile[p].height = 104;
                        Main.projectile[p].timeLeft = 1200;
                    }

                }
            }
            else
            {
                ProjectileTimer2 = 0;
                ProjectileTimer = 0;
            }
        }
        public void CreepingSnowflakes()
        {
            AttackDuration = 345;
            Attack = SerpentAttacks.CreepingIce;

            if (NPC.Distance(player.Center) > 1250)
                NPC.velocity = NPC.DirectionTo(player.Center) * (NPC.Distance(player.Center) / 100f);

            int CD = Phase == 2 ? 4 : 5;
            Vector2 vel = Phase == 2 ? Main.rand.NextVector2CircularEdge(3f, 3f) : Vector2.Zero;

            if (ProjectileTimer % CD == 0)
            {
                Projectile.NewProjectile(NPC.GetSource_FromAI(), player.Center, vel, ModContent.ProjectileType<CreepingIndicator>(), 0, 0f);

                if (Timer > 90)
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), player.Center, vel, ModContent.ProjectileType<MagicalSnowflake>(), 85, 1f, -1, 0, NPC.whoAmI);
            }
        }
        public void Icicles()
        {
            AttackDuration = 300;
            Attack = SerpentAttacks.Icicles;

            speedMult = MathHelper.Lerp(speedMult, 0.33f, 0.05f);

            if (NPC.Distance(player.Center) > 250)
                NPC.velocity = NPC.DirectionTo(player.Center) * (NPC.Distance(player.Center) / 25f);
            else NPC.velocity *= 0.95f;

            if (ProjectileTimer > 60)
            {
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    if (npc != null && npc.active && npc.type == BodySegmentType && npc.ai[1] % 8 == 0)
                    {
                        TranscendenceUtils.ProjectileRing(npc, 4, npc.GetSource_FromAI(), npc.Center, Icicle, 85, 1, 3f, 0, NPC.whoAmI, 2f, -1, npc.DirectionTo(player.Center).ToRotation() - MathHelper.PiOver2);
                    }
                }
                ProjectileTimer = 0;
            }
        }
        public void DeathraysMultiple()
        {
            AttackDuration = 600;
            Attack = SerpentAttacks.MultiRays;

            speedMult = MathHelper.Lerp(speedMult, 0.33f, 0.05f);
            Main.LocalPlayer.AddBuff(ModContent.BuffType<InfiniteFlight>(), 5);

            Vector2 pos = player.Center + Vector2.One.RotatedBy(MathHelper.ToRadians(Timer) * 6f) * 575f;

            if (NPC.ai[3] > 0.25f)
                NPC.ai[3] -= 0.05f;

            if (++ProjectileTimer2 < 120)
            {
                NPC.velocity = NPC.DirectionTo(pos) * 60f;
                return;
            }
            else NPC.velocity *= 0.8f;

            NPC.rotation = NPC.DirectionTo(player.Center).ToRotation() + MathHelper.PiOver2;

            if (ProjectileTimer == 130)
            {
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    if (npc != null && npc.active && npc.type == BodySegmentType && npc.ai[1] % 2 == 0)
                    {
                        Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, npc.DirectionTo(player.Center).RotatedBy(MathHelper.PiOver4 * 1.25f), ModContent.ProjectileType<FrostLaserStatic>(), 125, 0, -1, 0, npc.whoAmI);
                        Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, npc.DirectionTo(player.Center).RotatedBy(-MathHelper.PiOver4 * 1.25f), ModContent.ProjectileType<FrostLaserStatic>(), 125, 0, -1, 0, npc.whoAmI);
                    }
                }
            }
            if (ProjectileTimer > 300)
            {
                ProjectileTimer2 = 0;
                ProjectileTimer = 0;
            }

            if (ProjectileTimer < 240 && ProjectileTimer % 30 == 0)
            {
                TranscendenceUtils.ProjectileRing(NPC, 2, NPC.GetSource_FromAI(), NPC.Center, Icicle, 85, 1, 1f, 1, NPC.whoAmI, 1, -1, 0);
            }
        }
        public void DeathAnim()
        {
            AttackDuration = 500;
            Attack = SerpentAttacks.DeathAnim;

            if (Timer > 430)
                Timer = 430;

            speedMult = MathHelper.Lerp(speedMult, 0.125f, 0.075f);
            NPC.ai[3] = 0.975f;
            extraRot = 0;

            if (NPC.Distance(player.Center) > 500)
                NPC.velocity = Vector2.Lerp(NPC.velocity, NPC.DirectionTo(player.Center) * 10f, 1f / 15f);
            else NPC.velocity *= 0.975f;

            if (DeathFade < 0.75f)
                DeathFade += 1f / 425f;

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                Projectile p = Main.projectile[i];
                if (p != null && p.active && p.hostile && p.ai[1] == NPC.whoAmI)
                    p.Kill();
            }

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (npc != null && npc.active && (npc.type == BodySegmentType || npc.type == TailSegmentType))
                {
                    if (npc.ai[1] == 1)
                        NPC.rotation = npc.rotation;

                    npc.dontTakeDamage = true;
                    npc.ai[3] = 0.975f;

                    if (Timer % Main.rand.Next(8, 21) == 0)
                    {
                        int rand = Main.rand.Next(0, MaxSegments);
                        if (npc.ai[1] == rand)
                        {
                            for (int j = 0; j < 7; j++)
                            {
                                Dust.NewDustPerfect(npc.position, DustID.SnowflakeIce, Main.rand.NextVector2Circular(6f, 6f));
                                SoundEngine.PlaySound(SoundID.Shatter, npc.Center);
                            }
                        }
                    }

                    if (Timer > 419)
                    {
                        npc.chaseable = false;
                        npc.dontTakeDamage = false;
                    }
                }
            }
            if (Timer > 419)
            {
                NPC.chaseable = false;
                NPC.dontTakeDamage = false;

                if (!Downed.Contains(Bosses.FrostSerpent))
                    Downed.Add(Bosses.FrostSerpent);
            }
        }
        public override bool CheckDead()
        {
            if (Attack != SerpentAttacks.DeathAnim)
            {
                NPC.life = 5000;
                NPC.dontTakeDamage = true;
                Phase = 2;

                Stamina = 100;
                RestTimer = 0;
                Timer = 0;
                NPC.ai[1] = 100;

                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<Shockwave>(), 3250, 75, -1, 0, 100, 255);

                return false;
            }
            return true;
        }
        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            cooldownSlot = ImmunityCooldownID.Bosses;

            if (NPC.ai[3] == 1f && !target.solarDashing)
                return base.CanHitPlayer(target, ref cooldownSlot);
            else return false;
        }
        public override bool CanHitNPC(NPC target) => NPC.ai[3] == 1f;
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            NPCID.Sets.MustAlwaysDraw[Type] = true;

            if (Attack == SerpentAttacks.Fountain && Timer < 120)
            {
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);

                int width = (int)MathHelper.Lerp(0, 300, Timer / 90f);
                if (Timer > 90)
                    width = 300;

                Texture2D sprite = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/ExpandingTelegraph").Value;
                spriteBatch.Draw(sprite, new Rectangle(
                    (int)(dashPos.X - Main.screenPosition.X), (int)(dashPos.Y - Main.screenPosition.Y), width,
                    3000), null,
                    Color.DeepSkyBlue * 0.66f * (Timer < 20 ? Timer / 20f : 1f), 0, sprite.Size() * 0.5f, SpriteEffects.None, 0);

                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);
            }

            if (Attack == SerpentAttacks.Charges && Timer > 45 && ProjectileTimer2 < 45 && Timer < (AttackDuration - 60))
            {
                Vector2 pos = player.Center + player.velocity * 30f;
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);

                Texture2D sprite = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/BloomLine").Value;
                spriteBatch.Draw(sprite, new Rectangle(
                    (int)(NPC.Center.X - Main.screenPosition.X), (int)(NPC.Center.Y - Main.screenPosition.Y), 200,
                    (int)(NPC.Distance(pos) * 4f)), null,
                    Color.Red * 0.66f, NPC.DirectionTo(pos).ToRotation() + MathHelper.PiOver2, sprite.Size() * 0.5f, SpriteEffects.None, 0);

                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);
            }

            //Request the Effect
            var eff = ModContent.Request<Effect>("TranscendenceMod/Miscannellous/Assets/Shaders/Effects/SerpentDeath", AssetRequestMode.ImmediateLoad).Value;
            //Apply Shader Texture
            Texture2D shaderImage = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/SeraphForcefieldShader").Value;
            Main.instance.GraphicsDevice.Textures[1] = shaderImage;

            eff.Parameters["uImageSize0"].SetValue(NPC.Size);
            eff.Parameters["uImageSize1"].SetValue(shaderImage.Size() * 0.15f);
            eff.Parameters["uOpacity"].SetValue(DeathFade);

            Effect effect = Attack == SerpentAttacks.DeathAnim ? eff : null;
            float a = Attack == SerpentAttacks.DeathAnim ? 1f : NPC.ai[3];

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, default, default, default, effect, Main.GameViewMatrix.TransformationMatrix);

            TranscendenceUtils.DrawEntity(NPC, Color.White * 0.66f * a, NPC.scale, Texture, NPC.rotation, NPC.Center, null);

            Texture2D maw = ModContent.Request<Texture2D>(Texture + "_Maws").Value;
            Vector2 mawDir = (NPC.rotation - MathHelper.PiOver2).ToRotationVector2();
            Vector2 mawPos = NPC.Center + (mawDir * 6f);

            //Maws
            TranscendenceUtils.DrawEntity(NPC, Color.White * 0.66f * a, NPC.scale, maw, NPC.rotation - MawRot, mawPos - (NPC.rotation.ToRotationVector2() * (float)(Math.Sin(MawRot) * 30f * NPC.scale)), null, new Vector2(1f, 0.5f) * maw.Size(), SpriteEffects.None);
            TranscendenceUtils.DrawEntity(NPC, Color.White * 0.66f * a, NPC.scale, maw, NPC.rotation + MawRot, mawPos + (NPC.rotation.ToRotationVector2() * (float)(Math.Sin(MawRot) * 30f * NPC.scale)), null, new Vector2(0f, 0.5f) * maw.Size(), SpriteEffects.FlipHorizontally);

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            if (Attack == SerpentAttacks.Slam)
            {
                float alpha = ProjectileTimer / 90f;
                TranscendenceUtils.VeryBasicNPCOutline(NPC, Texture, 2f, 1f, 1f, 1f, alpha, NPC.Center, NPC.scale, NPC.rotation, 1f);
            }

            return false;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
        }
        public override bool CheckActive() => false;
        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            scale = 1.5f;
            return (int)NPC.ai[1] == (int)(NPC.ai[2] / 2);
        }
        public override void BossHeadRotation(ref float rotation)
        {
            rotation = NPC.rotation;
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Snow,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundSnow,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.Blizzard,
                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.TranscendenceMod.Messages.Bestiary.FrostSerpent")),
            });
            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new()
            {
                CustomTexturePath = "TranscendenceMod/Miscannellous/Assets/Bestiary/FrostSerpentBestiary",
                PortraitScale = 0.66f,
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
        }
    }
    [AutoloadBossHead]
    public class FrostSerpent_Body : BodySegment
    {
        public override float DistanceBetweenSegments => 0.75f;

        public override void AI()
        {
            BodyAI(NPC);
            NPC.ai[3] = Main.npc[(int)NPC.ai[0]].ai[3];

        }
        public override void SetStaticDefaults()
        {
            TranscendenceUtils.BeGoneBestiary(NPC);

            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Shimmer] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;

            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Poisoned] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Venom] = true;

            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Frostburn] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Frostburn2] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire3] = true;
            NPC.buffImmune[ModContent.BuffType<JungleRingBuff>()] = true;
        }
        public override void SetDefaults()
        {
            NPC.lifeMax = 10000000;
            NPC.defense = 20;
            NPC.damage = 105;
            NPC.knockBackResist = 0;

            NPC.width = 44;
            NPC.height = 56;
            NPC.takenDamageMultiplier = 0.5f;

            NPC.aiStyle = -1;
            NPC.noGravity = true;
            NPC.noTileCollide = true;

            NPC.HitSound = SoundID.NPCHit5;
            NPC.DeathSound = SoundID.Shatter;

            NPC.BossBar = ModContent.GetInstance<FrostSerpentBossBar>();
            NPC.friendly = false;
        }
        public override bool CheckDead()
        {
            NPC.dontTakeDamage = true;
            NPC.life = 1;
            NPC.active = true;

            return false;
        }
        public override void ModifyHitByProjectile(Projectile projectile, ref NPC.HitModifiers modifiers)
        {
            if (projectile.type == ProjectileID.StardustDragon1 || projectile.type == ProjectileID.StardustDragon2 || projectile.type == ProjectileID.StardustDragon3 || projectile.type == ProjectileID.StardustDragon4)
                modifiers.FinalDamage *= 0.375f;
            if (projectile.type == ProjectileID.LastPrismLaser)
                modifiers.FinalDamage *= 0.575f;
            if (projectile.type == ProjectileID.LunarFlare)
                modifiers.FinalDamage *= 0.45f;
            if (projectile.type == ProjectileID.MoonlordTurretLaser)
                modifiers.FinalDamage *= 0.33f;
            if (projectile.type == ProjectileID.RainbowCrystalExplosion)
                modifiers.FinalDamage *= 0.33f;
            if (projectile.type == ModContent.ProjectileType<AngelicLaser_Friendly>())
                modifiers.FinalDamage *= 0.25f;
            if (projectile.type == ModContent.ProjectileType<DreamSealProj>())
                modifiers.FinalDamage *= 0.75f;
            if (projectile.type == ModContent.ProjectileType<EmpressSun>())
                modifiers.FinalDamage *= 0.33f;
        }
        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            cooldownSlot = ImmunityCooldownID.Bosses;

            if (NPC.ai[3] == 1f && !target.solarDashing)
                return base.CanHitPlayer(target, ref cooldownSlot);
            else return false;
        }
        public override bool CanHitNPC(NPC target) => NPC.ai[3] == 1f;
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (Main.npc[(int)NPC.ai[0]].ModNPC is FrostSerpent_Head boss)
            {
                //Request the Effect
                var eff = ModContent.Request<Effect>("TranscendenceMod/Miscannellous/Assets/Shaders/Effects/SerpentDeath", AssetRequestMode.ImmediateLoad).Value;

                //Apply Shader Texture
                Texture2D shaderImage = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/SeraphForcefieldShader").Value;
                Main.instance.GraphicsDevice.Textures[1] = shaderImage;

                eff.Parameters["uImageSize0"].SetValue(NPC.Size);
                eff.Parameters["uImageSize1"].SetValue(shaderImage.Size() * 0.15f);
                eff.Parameters["uOpacity"].SetValue(boss.DeathFade);

                Effect effect = boss.Attack == SerpentAttacks.DeathAnim ? eff : null;
                float a = boss.Attack == SerpentAttacks.DeathAnim ? 1f : NPC.ai[3];

                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, default, default, default, effect, Main.GameViewMatrix.TransformationMatrix);

                if (NPC.localAI[1] == 1)
                    TranscendenceUtils.DrawEntity(NPC, Color.White * a * 2f, NPC.scale, "TranscendenceMod/Items/Weapons/Ranged/IceKunai", NPC.rotation + MathHelper.PiOver4 / 2f, NPC.Center + Vector2.One.RotatedBy(NPC.rotation - MathHelper.PiOver4) * -16f, null);

                if (NPC.localAI[1] == 2)
                    TranscendenceUtils.DrawEntity(NPC, Color.White * a * 2f, NPC.scale, "TranscendenceMod/Items/Weapons/Melee/MountaintopGlacier", NPC.rotation + MathHelper.Pi, NPC.Center, null);

                if (NPC.localAI[1] == 3)
                    TranscendenceUtils.DrawEntity(NPC, Color.White * a * 2f, NPC.scale, "TranscendenceMod/Items/Weapons/Magic/Lumimyrsky", NPC.rotation + MathHelper.PiOver2, NPC.Center, null);

                if (NPC.localAI[1] == 4)
                    TranscendenceUtils.DrawEntity(NPC, Color.White * a * 2f, NPC.scale, $"Terraria/Images/Item_725", NPC.rotation + MathHelper.PiOver4 * 0.75f, NPC.Center + Vector2.One.RotatedBy(NPC.rotation - MathHelper.PiOver4) * 16f, null);

                if (NPC.localAI[1] == 5)
                    TranscendenceUtils.DrawEntity(NPC, Color.White * a * 2f, NPC.scale, $"Terraria/Images/Item_3460", NPC.rotation + MathHelper.PiOver4 * 0.33f, NPC.Center + Vector2.One.RotatedBy(NPC.rotation - MathHelper.PiOver4) * -12f, null);

                if (NPC.localAI[1] == 6)
                    TranscendenceUtils.DrawEntity(NPC, Color.White * a * 2f, NPC.scale, $"Terraria/Images/Item_1274", NPC.rotation, NPC.Center, null);

                if (NPC.localAI[1] == 7)
                    TranscendenceUtils.DrawEntity(NPC, Color.White * a * 2f, NPC.scale, $"Terraria/Images/Item_3456", 0, NPC.Center, null);

                TranscendenceUtils.DrawEntity(NPC, Color.White * 0.66f * a, NPC.scale, Texture, NPC.rotation, NPC.Center, null);

                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);

                if (boss.Attack == SerpentAttacks.Icicles && NPC.ai[1] % 3 == 0)
                {
                    float alpha = boss.ProjectileTimer > 30 ? MathHelper.Lerp(0f, 1f, (boss.ProjectileTimer - 30) / 30f) : 0f;
                    TranscendenceUtils.DrawEntity(NPC, Color.Blue * 0.75f * alpha, NPC.scale * 4f, "bloom", NPC.rotation, NPC.Center, null);
                    TranscendenceUtils.DrawEntity(NPC, Color.DeepSkyBlue * 2f * alpha, NPC.scale * 2f, "bloom", NPC.rotation, NPC.Center, null);
                    TranscendenceUtils.DrawEntity(NPC, Color.White * alpha, NPC.scale, "bloom", NPC.rotation, NPC.Center, null);
                }

                if (boss.Attack == SerpentAttacks.Slam)
                {
                    float alpha = boss.ProjectileTimer / 90f;
                    TranscendenceUtils.VeryBasicNPCOutline(NPC, Texture, 2f, 1f, 1f, 1f, alpha, NPC.Center, NPC.scale, NPC.rotation, 1f);
                }
            }

            return false;
        }
        public override void BossHeadRotation(ref float rotation)
        {
            rotation = NPC.rotation;
        }
        public override void BossHeadSlot(ref int index)
        {
            if (NPC.ai[1] % 3 == 0 && NPC.ai[1] != 0) { }
            else index = -1;
        }
        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position) => false;
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.66f);
            NPC.damage = (int)(NPC.damage * 0.55f);
        }
    }
    [AutoloadBossHead]
    public class FrostSerpent_Tail : TailSegment
    {
        public override float DistanceBetweenSegments => 0.5f;
        public override void SetStaticDefaults()
        {
            TranscendenceUtils.BeGoneBestiary(NPC);

            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Shimmer] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;

            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Poisoned] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Venom] = true;

            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Frostburn] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Frostburn2] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire3] = true;
            NPC.buffImmune[ModContent.BuffType<JungleRingBuff>()] = true;
        }
        public override void AI()
        {
            BodyAI(NPC);
            NPC.ai[3] = Main.npc[(int)NPC.ai[0]].ai[3];
        }
        public override bool CheckDead()
        {
            NPC.dontTakeDamage = true;
            NPC.life = 1;
            NPC.active = true;

            return false;
        }
        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            cooldownSlot = ImmunityCooldownID.Bosses;

            if (NPC.ai[3] == 1f && !target.solarDashing)
                return base.CanHitPlayer(target, ref cooldownSlot);
            else return false;
        }
        public override bool CanHitNPC(NPC target) => NPC.ai[3] == 1f;
        public override void SetDefaults()
        {
            NPC.lifeMax = 87200;
            NPC.defense = 20;
            NPC.damage = 105;
            NPC.knockBackResist = 0;

            NPC.width = 48;
            NPC.height = 48;

            NPC.aiStyle = -1;
            NPC.noGravity = true;
            NPC.noTileCollide = true;

            NPC.HitSound = SoundID.NPCHit5;
            NPC.DeathSound = SoundID.Shatter;

            NPC.BossBar = ModContent.GetInstance<FrostSerpentBossBar>();
            NPC.friendly = false;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (Main.npc[(int)NPC.ai[0]].ModNPC is FrostSerpent_Head boss)
            {
                //Request the Effect
                var eff = ModContent.Request<Effect>("TranscendenceMod/Miscannellous/Assets/Shaders/Effects/SerpentDeath", AssetRequestMode.ImmediateLoad).Value;
                //Apply Shader Texture
                Texture2D shaderImage = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/SeraphForcefieldShader").Value;
                Main.instance.GraphicsDevice.Textures[1] = shaderImage;

                eff.Parameters["uImageSize0"].SetValue(NPC.Size);
                eff.Parameters["uImageSize1"].SetValue(shaderImage.Size() * 0.15f);
                eff.Parameters["uOpacity"].SetValue(boss.DeathFade);

                Effect effect = boss.Attack == SerpentAttacks.DeathAnim ? eff : null;
                float a = boss.Attack == SerpentAttacks.DeathAnim ? 1f : NPC.ai[3];

                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, default, default, default, effect, Main.GameViewMatrix.TransformationMatrix);

                TranscendenceUtils.DrawEntity(NPC, Color.White * 0.66f * a, NPC.scale, Texture, NPC.rotation, NPC.Center, null);

                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);

                if (boss.Attack == SerpentAttacks.Slam)
                {
                    float alpha = boss.ProjectileTimer / 90f;
                    TranscendenceUtils.VeryBasicNPCOutline(NPC, Texture, 2f, 1f, 1f, 1f, alpha, NPC.Center, NPC.scale, NPC.rotation, 1f);
                }
            }

            return false;
        }
        public override void BossHeadRotation(ref float rotation)
        {
            rotation = NPC.rotation;
        }
        public override void BossHeadSlot(ref int index)
        {
        }
        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position) => false;
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.66f);
            NPC.damage = (int)(NPC.damage * 0.55f);
        }
    }
}

