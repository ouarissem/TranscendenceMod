
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Security.Cryptography.Pkcs;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI.Chat;
using TranscendenceMod.Buffs;
using TranscendenceMod.Buffs.Items.Potions;
using TranscendenceMod.Dusts;
using TranscendenceMod.Items.Accessories.Expert;
using TranscendenceMod.Items.Accessories.Movement.Wings;
using TranscendenceMod.Items.Armor.Hats;
using TranscendenceMod.Items.Consumables.LootBags;
using TranscendenceMod.Items.Consumables.Placeables.Decorations;
using TranscendenceMod.Items.Farming;
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Items.Materials.MobDrops;
using TranscendenceMod.Items.Weapons.Melee;
using TranscendenceMod.Items.Weapons.Ranged;
using TranscendenceMod.Items.Weapons.Summoner;
using TranscendenceMod.Miscanellous.UI.Achievements.Tasks;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Miscannellous.UI;
using TranscendenceMod.Projectiles.NPCs.Bosses.Dragon;
using TranscendenceMod.Projectiles.NPCs.Bosses.SpaceBoss;
using static TranscendenceMod.TranscendenceWorld;
namespace TranscendenceMod.NPCs.Boss.Dragon
{
    [AutoloadBossHead]
    public class WindDragon : ModNPC
    {
        //Wall of Text has awoken!
        int ArenaSize = 100;
        public Vector2 arenaStartPos;
        Player player;
        Player local = Main.LocalPlayer;

        public static int slash => ModContent.ProjectileType<DragonSlash>();

        static public float RotationTimer;
        static public float RotationSpeed;
        static public int DespawnTimer;
        Vector2 RotationRadians = new Vector2(-4, 0) / 25;
        public int moveTwoAngle = 0;
        Vector2 arenaCenter;
        public Vector2 dashVel;
        public Vector2 dashPos;

        //Damage Stats
        static public int TornadoDamage = 90;
        static public int ThunderDamage = 95;
        static public int WaterDamage = 80;
        static public int WindDamage = 110;
        static public int ClawDamage = 100;
        static public int SnowDamage = 80;
        public int contactDMG = 250;

        //AI
        public int Timer_AI;
        /// <summary>
        /// How many dashes the boss has left
        /// Goes from 0 to max
        /// </summary>
        public int Stamina;
        /// <summary>
        /// Max number of dashes
        /// </summary>
        public int MaxStamina = 12;

        public int AttackDuration;
        public int[] ProjectileCD = new int[5];
        public int Phase;
        public bool HasArena;
        public string CurrentAttack = "";
        public float arenaVisualSize;
        public int Reversal = 1;
        public int StunnedFrame;
        public int StunnedFrameTimer;
        public int RandomizationTries;
        public float Phase3SkyOpacity = 0f;
        public float Opacity = 1f;
        public int RestDashCD;

        public Vector2 FlyPos;

        public List<int> AttacksCompleted = new List<int>();
        public List<int> RecentAttacks = new List<int>();
        public int NextAttack = -1;

        public bool ChallengeActive;

        public override void SetStaticDefaults()
        {
            //Main.npcFrameCount[Type] = 3;
            NPCID.Sets.TrailCacheLength[Type] = 15;
            NPCID.Sets.TrailingMode[Type] = 1;
            NPCID.Sets.MustAlwaysDraw[Type] = true;

            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Shimmer] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.BetsysCurse] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Daybreak] = true;
        }
        public override void SetDefaults()
        {
            /*Stats*/
            NPC.lifeMax = 475 * 1000;
            NPC.damage = contactDMG;
            NPC.defense = 30;
            NPC.npcSlots = 8f;
            NPC.value = Item.buyPrice(gold: 75);
            /*Collision*/
            NPC.width = 42;
            NPC.height = 42;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.scale = 3f;
            /*Audio*/
            NPC.HitSound = SoundID.DD2_BetsyHurt with { Volume = 0.5f, MaxInstances = 5 };
            NPC.DeathSound = SoundID.NPCDeath1;
            Music = MusicID.DukeFishron;

            NPC.aiStyle = -1;
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

            /*Weapons*/
            normalMode.OnSuccess(ItemDropRule.FewFromOptions(2, 1, ModContent.ItemType<WindDragonsClaw>(),
            ModContent.ItemType<StormBow>(), ModContent.ItemType<CelestialSeraphStaff>(),
            ModContent.ItemType<Starfield>(), ModContent.ItemType<SpaceBossWings>()));
            /*Extra*/
            normalMode.OnSuccess(ItemDropRule.NotScalingWithLuck(ModContent.ItemType<AtmospheronMask>(), 7));
            normalMode.OnSuccess(ItemDropRule.NotScalingWithLuck(ModContent.ItemType<FaeTrophyItem>(), 10));

            npcLoot.Add(normalMode);
            /*LootBag (No lootbag exists atm youll get the expert mode instead)*/
            npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<PerfectHorseshoe>()));
        }
        public bool CanAttack;
        public override bool CanHitPlayer(Player target, ref int cooldownSlot) => false;
        public override void HitEffect(NPC.HitInfo hit)
        {
            base.HitEffect(hit);
        }
        public override void AI()
        {
            player = Main.player[NPC.target];
            RotationRadians = Vector2.One.RotatedBy(RotationTimer);
            RotationTimer += RotationSpeed;

            NPC.TargetClosest(true);
            SkyManager.Instance.Activate("TranscendenceMod:DragonSky", player.Center);

            if (player != null && player.active && (!player.HasBuff(ModContent.BuffType<Blind>()) || Main.dayTime) && NPC.ai[1] > 0)
                ChallengeActive = false;

            //This thing switches attacks and adjusts some things
            if (Timer_AI == 2)
            {
                int max = 6;

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

            if (Timer_AI > (AttackDuration + 3))
            {
                NPC.velocity = Vector2.Zero;
                NPC.rotation = 0;
                NPC.hide = false;
                moveTwoAngle = 0;

                NPC.alpha = 0;
                Opacity = 1f;

                NPC.ai[2] = 0;
                NPC.ai[3] = 0;
                Stamina -= 1;

                for (int i = 0; i < 5; i++)
                    ProjectileCD[i] = 0;

                Timer_AI = 0;
                RotationTimer = 0;
                RotationSpeed = 0.25f;
                HasArena = false;

                NPC.ai[1] = NextAttack > 0 ? NextAttack : NPC.ai[1] + 1;
                if (NPC.ai[1] > 0)
                {
                    AttacksCompleted.Add((int)NPC.ai[1]);
                    if (RecentAttacks.Count > 1)
                        RecentAttacks.RemoveAt(0);
                    RecentAttacks.Add((int)NPC.ai[1]);
                }
            }

            //Projectile stuff
            ProjectileManagerer();

            if (Stamina <= 0)
            {
                // Animate Tired Icon
                if (++StunnedFrameTimer > 5)
                {
                    StunnedFrame += 22;
                    if (StunnedFrame > 66)
                        StunnedFrame = 0;
                    StunnedFrameTimer = 0;
                }

                if (++RestDashCD > 120)
                {
                    Stamina = MaxStamina;
                }
                return;
            }
            else RestDashCD = 0;

            Timer_AI++;

            //Attack Patterns
            if (NPC.ai[0] == 0)
            {
                switch (NPC.ai[1])
                {
                    case 0: Intro(); break;
                    case 1: Dashes(); break;
                    case 2: RainbowDrizzle(); break;
                    case 3: Slam(); break;
                    case 4: Roar(); break;
                    case 5: TornadoDash(); break;
                    case 6: NPC.ai[1] = 1; goto case 1;
                }
            }

            //Despawn
            if (player.active == false || player.dead == true)
            {
                if (++DespawnTimer > 60)
                {
                    NPC.active = false;
                    DespawnTimer = 0;
                    SoundEngine.PlaySound(SoundID.Item4);
                    TranscendenceUtils.DustRing(NPC.Center, 25, ModContent.DustType<ArenaDust>(), 24, Color.White, 2);
                }
            }

        }

        public void Intro()
        {
            AttackDuration = 30;

        }

        public void Dashes()
        {
            AttackDuration = 100;

            if (Timer_AI < 45)
            {
                TranscendenceUtils.DustRing(NPC.Center, 20, DustID.AmberBolt, 5f, Color.White, 2f);
                NPC.velocity = NPC.DirectionTo(player.Center) * -3.75f;

                return;
            }

            if (Timer_AI >= (AttackDuration - 10))
            {
                NPC.velocity *= 0.8f;
                return;
            }

            if (++ProjectileCD[0] < 5)
            {
                dashVel = NPC.DirectionTo(player.Center) * 50f;
                NPC.rotation = dashVel.ToRotation() + (NPC.direction == -1 ? MathHelper.Pi : 0f);

                NPC.velocity *= 0.8f;
            }
            if (ProjectileCD[0] == 10)
            {
                NPC.velocity = dashVel;

                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, dashVel, slash, 120, 2f, -1, 0, NPC.whoAmI);
                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<DashLaser>(), 100, 0f, -1, 0, NPC.whoAmI);
            }
            if (ProjectileCD[0] >= 15)
            {
                ProjectileCD[0] = 0;
            }
        }

        public void RainbowDrizzle()
        {
            AttackDuration = 80;

            if (Timer_AI < 45)
            {
                if (Timer_AI == 1)
                {
                    int dir = NPC.Center.X > player.Center.X ? 500 : -500;
                    NPC.Teleport(player.Center + new Vector2(dir, 0));
                }

                return;
            }

            if (++ProjectileCD[0] % 5 == 0)
            {
                Vector2 vel = NPC.DirectionTo(player.Center);
                float off = Main.rand.NextFloat(-75f, 75f);

                float rot = vel.ToRotation();
                Vector2 pos = NPC.Center + new Vector2(0, off).RotatedBy(rot);

                SoundEngine.PlaySound(SoundID.Item85 with { MaxInstances = 0 }, pos);
                for (int i = 0; i < 5; i++)
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), pos, vel.RotatedByRandom(0.0375f) * (37.5f + i), ModContent.ProjectileType<RainbowShot>(), 100, 2f, -1, 0, NPC.whoAmI, (Timer_AI - 45) / 35f);
            }
        }

        public void Slam()
        {
            AttackDuration = 98;

            if (Timer_AI >= 45 && Timer_AI < 85)
            {
                NPC.Center = player.Center - new Vector2(0, 375);
            }
            if (Timer_AI >= 90)
            {
                NPC.velocity = new Vector2(0, 55f);

                if (Timer_AI == 91)
                {
                    int p = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, new Vector2(0, 5), slash, 140, 2f, -1, 0, NPC.whoAmI);
                    Main.projectile[p].scale = 8f;
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<DashLaser>(), 100, 0f, -1, 0, NPC.whoAmI);
                }
            }
        }

        public void Roar()
        {
            AttackDuration = 90;

            if (Timer_AI < 30)
            {
                for (int i = 0; i < 32; i++)
                {
                    Vector2 pos = NPC.Center + Vector2.One.RotatedBy(MathHelper.TwoPi * i / 32f + MathHelper.ToRadians(Timer_AI)) * (Timer_AI * 25f);
                    Dust d = Dust.NewDustPerfect(pos, Timer_AI < 10 ? DustID.GreenFairy : DustID.GemRuby, NPC.DirectionTo(pos) * 5f, 0, default, 1.5f);
                    d.noGravity = true;

                    Vector2 pos2 = NPC.Center + Vector2.One.RotatedBy(MathHelper.TwoPi * i / 32f - MathHelper.ToRadians(Timer_AI)) * (Timer_AI * 25f);
                    Dust d2 = Dust.NewDustPerfect(pos2, Timer_AI < 10 ? DustID.GreenFairy : DustID.GemRuby, NPC.DirectionTo(pos2) * 5f, 0, default, 1.5f);
                    d2.noGravity = true;

                }
                return;
            }

            if (Timer_AI == 45)
            {
                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<RoarShockwave>(), 130, 2f, -1, 0f, NPC.whoAmI);
                SoundEngine.PlaySound(SoundID.DD2_BetsyScream with { Volume = 2f}, NPC.Center);
            }
        }

        public void TornadoDash()
        {
            AttackDuration = 130;

            if (Timer_AI == 10)
            {
                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<MeleeTornado>(), 120, 2f, -1, 0f, NPC.whoAmI);
            }
        }


        private void ProjectileManagerer()
        {
            for (int p = 0; p < Main.maxProjectiles; p++)
            {
                Projectile projectile = Main.projectile[p];
            }
        }

        public override void OnSpawn(IEntitySource source)
        {
            Phase = 0;
            RotationTimer = 0;
            RotationSpeed = 0;
            AttackDuration = 90;
            Stamina = MaxStamina + 1;
            ChallengeActive = true;

            if (!TranscendenceWorld.EncouteredAtmospheron)
                TranscendenceWorld.EncouteredAtmospheron = true;
        }

        public override void ModifyHitByProjectile(Projectile projectile, ref NPC.HitModifiers modifiers)
        {
            if (projectile.type == ProjectileID.FinalFractal) modifiers.FinalDamage *= 0.25f;
            if (projectile.IsMinionOrSentryRelated) modifiers.FinalDamage *= 0.66f;

            if ((projectile.DamageType == DamageClass.Melee || projectile.DamageType == DamageClass.MeleeNoSpeed)
                && Main.player[projectile.owner].heldProj == projectile.whoAmI) modifiers.FinalDamage *= 1.33f;
        }
        public override void ModifyHitByItem(Player player, Item item, ref NPC.HitModifiers modifiers)
        {
            if (item.DamageType == DamageClass.Melee)
                modifiers.FinalDamage *= 1.33f;
        }

        public override void OnHitByItem(Player player, Item item, NPC.HitInfo hit, int damageDone)
        {
        }
        public override void OnHitByProjectile(Projectile projectile, NPC.HitInfo hit, int damageDone)
        {
            //Dust.NewDust(NPC.Center, 1, 1, ModContent.DustType<BetterBlood>(),
                //Main.rand.NextFloat(-4f, 4f), Main.rand.NextFloat(2f, 4f), 0, Color.White, 1f);
        }

        public override bool PreKill()
        {
            //int arm = Mod.Find<ModGore>("Gore1").Type;
            //int head = Mod.Find<ModGore>("Gore2").Type;
            //int shoulder = Mod.Find<ModGore>("Gore3").Type;
            //int lowerbody = Mod.Find<ModGore>("Gore4").Type;


            if (!Downed.Contains(Bosses.Atmospheron))
            {
                Main.NewText(Language.GetTextValue("Mods.TranscendenceMod.Messages.DragonDeath"), 50, 255, 130);
                Downed.Add(Bosses.Atmospheron);
            }

            if (ChallengeActive)
                ModAchievementsHelper.CompleteChallenge(player, TaskIDs.DragonChallenge);

            /*if (Main.netMode != NetmodeID.Server)
            {
                for (int i = 0; i < 6; i++)
                {
                    Gore.NewGore(NPC.GetSource_Death(), NPC.Center, new Vector2(Main.rand.Next(-7, 7)), arm);
                }
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, new Vector2(Main.rand.Next(-3, 3)), head);
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, new Vector2(Main.rand.Next(-5, 5)), shoulder);
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, new Vector2(Main.rand.Next(-2, 2)), lowerbody);
            }*/
            return true;
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.TranscendenceMod.Messages.Bestiary.Atmospheron")),
            });
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            int max = Phase == 2 ? 12 : 9;

            string att = AttacksCompleted.Count == 0 ? "-" : string.Join(", ", AttacksCompleted.ToArray());
            string ratt = RecentAttacks.Count == 0 ? "-" : string.Join(", ", RecentAttacks.ToArray());

            string t = "[c/ff0000:Attack Pattern:] " + att + $"   [c/4387e9:Next up: {NextAttack} ({(NucleusAttacks)NextAttack})]" +
                $"   [c/31e24d:Attacks Left: {AttacksCompleted.Count} / {max}]  " + ratt;
            ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.MouseText.Value, t, new Vector2(35, Main.screenHeight - 40), Color.White, 0f, Vector2.Zero, Vector2.One);

            Player player = Main.player[NPC.target];

            NPCID.Sets.TrailCacheLength[Type] = 25;
            NPCID.Sets.TrailingMode[Type] = 1;

            if (NPC.velocity.Length() > 1f)
                TranscendenceUtils.DrawTrailNPC(NPC, NPC.GetAlpha(drawColor) * Opacity, NPC.scale, $"{Texture}", false, true, 1, NPC.Size / 4, NPC.direction != 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally);

            float opac = MathHelper.Lerp(0f, 1f, ProjectileCD[1] / 50f) * 0.5f;

            TranscendenceUtils.DrawEntity(NPC, Color.White * Opacity, NPC.scale, Texture, NPC.rotation, NPC.Center, null, NPC.direction != 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally);

            if (Stamina == 0)
            {
                TranscendenceUtils.DrawEntity(NPC, Color.White, 1f, TranscendenceMod.ASSET_PATH + "/Stunned", 0,
                    NPC.Center - new Vector2(0, NPC.height * 0.2f), new Rectangle(0, StunnedFrame, 36, 22));
            }

            return false;
        }
    }
}