using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using ReLogic.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Graphics;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI.Chat;
using TranscendenceMod.Items.Consumables.Placeables;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.NPCs;
using TranscendenceMod.NPCs.Boss.Nucleus;
using TranscendenceMod.Projectiles.Equipment;
using TranscendenceMod.Projectiles.NPCs.Bosses.Nucleus;

namespace TranscendenceMod.Miscanellous.MiscSystems
{
    public class NucleusGame : ModPlayer
    {
        // 4:3 Aspect Ratio, 16 x 12
        /// <summary>
        /// 1 = Player<br></br>
        /// 2 = Tile<br></br>
        /// 3 = Spike
        /// 4 = Door
        /// </summary>
        public int[] pixel = new int[192];
        public int room;
        public bool Active;
        public bool BossEdition;
        public int MoveCD;
        public int playerPos;
        public int upcomingPos;
        public int Wait;
        public int Time;
        public int moveDelay;
        public int MaxTime = 3600;
        public bool CollectedReward;

        public int RhythmBlockTimer;
        public bool RhythmBlockON => RhythmBlockTimer >= 60;


        public enum Direction
        {
            Left, Right, Up, Down
        }

        public override void PreUpdate()
        {
            MaxTime = 60 * 60;
            if (Main.expertMode)
                MaxTime = 50 * 60;
            if (Main.masterMode)
                MaxTime = 40 * 60;

            if (!NPC.AnyNPCs(ModContent.NPCType<ProjectNucleus>()))
                BossEdition = false;

            if (!Active && Player.GetModPlayer<TranscendencePlayer>().NucleusDeathAnim == 0)
            {
                Time = MaxTime;
                room = 0;
                BossEdition = false;

                return;
            }


            if (moveDelay > 0)
                moveDelay--;
            if (moveDelay == 1)
            {
                playerPos = upcomingPos;

                if (!Main.dedServ)
                    SoundEngine.PlaySound(SoundID.Dig with { MaxInstances = 0 });

                if (pixel[upcomingPos] == 4)
                {
                    if (room >= 5 && BossEdition || room >= 15 && !BossEdition)
                    {
                        if (BossEdition)
                        {
                            if (CollectedReward)
                                Item.NewItem(Player.GetSource_FromAI(), Player.getRect(), ModContent.ItemType<ArcadeCabinetItem>());

                            for (int i = 0; i < Main.maxNPCs; i++)
                            {
                                NPC n = Main.npc[i];
                                if (n != null && n.active && n.ModNPC is ProjectNucleus boss)
                                {
                                    Player.velocity = new Vector2(20f, 5f);
                                    n.ai[1] = 99;
                                    boss.Timer_AI = 0;
                                    boss.ProjectileCD = 0;
                                }
                            }
                        }
                        else Projectile.NewProjectile(Player.GetSource_FromAI(), Player.Center, new Vector2(0f, -3.75f), ModContent.ProjectileType<FireworkProjectile>(), 0, 0, -1, 0, Player.whoAmI);
                        Active = false;
                    }
                    else ConstructLevels();
                }
            }

            if (room == 0)
                ConstructLevels();


            if (MoveCD > 0)
                MoveCD--;

            if (pixel[playerPos] == 3 || pixel[playerPos] == 5 && RhythmBlockON)
            {
                Wait++;
                GameFail();

                return;
            }


            if (++RhythmBlockTimer > 120)
                RhythmBlockTimer = 0;


            if (BossEdition)
            {
                if (room > 1 && Time > 0)
                    Time--;

                if (Time < 1)
                    GameFail();
            }


            if (Player.controlLeft)
                Move(Direction.Left);

            if (Player.controlRight)
                Move(Direction.Right);

            if (Player.controlUp)
                Move(Direction.Up);

            if (Player.controlDown)
                Move(Direction.Down);
        }

        public void Move(Direction dir)
        {
            if (MoveCD > 0 || Player.dead)
                return;

            int targetPos = 0;

            if (dir == Direction.Left)
                targetPos = playerPos - 1;

            if (dir == Direction.Right)
                targetPos = playerPos + 1;

            if (dir == Direction.Up)
                targetPos = playerPos - 16;

            if (dir == Direction.Down)
                targetPos = playerPos + 16;


            if (pixel[targetPos] == 2)
                return;

            if (pixel[targetPos] == 6)
            {
                CollectedReward = true;

                if (!Main.dedServ)
                    SoundEngine.PlaySound(SoundID.AchievementComplete);
            }

            upcomingPos = targetPos;
            moveDelay = 5;
            MoveCD = 10;
        }

        public void GameFail()
        {
            RhythmBlockTimer = 61;

            if (Wait < 60 && Time >= 1)
                return;


            if (BossEdition)
            {
                Player.KillMe(PlayerDeathReason.ByCustomReason(NetworkText.FromKey("Mods.TranscendenceMod.Messages.Death.NucleusGame", Player.name)), 9999, 1);
                Active = false;
            }
            Active = false;
        }

        public void ConstructLevels()
        {
            room += 1;
            MoveCD = 30;
            Wait = 0;
            CollectedReward = false;

            switch (room)
            {
                case 0: pixel = new int[]
                {
                2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
                2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2,
                2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2,
                2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2,
                2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2,
                2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2,
                2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2,
                2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2,
                2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2,
                2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2,
                2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2,
                2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2
                }; break;

                case 1: pixel = new int[]
                {
                2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
                2, 0, 0, 0, 0, 0, 2, 3, 3, 2, 0, 0, 0, 0, 0, 2,
                2, 0, 1, 0, 0, 0, 2, 2, 2, 2, 0, 0, 0, 0, 0, 2,
                2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2,
                2, 0, 0, 0, 0, 0, 2, 2, 2, 2, 0, 0, 0, 0, 0, 2,
                2, 2, 2, 2, 2, 2, 2, 3, 3, 2, 2, 0, 3, 0, 2, 2,
                2, 3, 3, 3, 3, 3, 2, 3, 3, 2, 2, 0, 3, 0, 2, 2,
                2, 0, 0, 0, 0, 0, 2, 3, 3, 2, 0, 0, 0, 0, 0, 2,
                2, 0, 4, 0, 0, 0, 2, 3, 3, 2, 0, 0, 0, 0, 0, 2,
                2, 0, 0, 0, 0, 0, 2, 2, 2, 2, 0, 0, 0, 0, 0, 2,
                2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2,
                2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2
                }; break;

                case 2: pixel = new int[]
                {
                2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
                2, 0, 0, 0, 0, 0, 0, 0, 0, 2, 3, 2, 2, 2, 3, 2,
                2, 0, 0, 0, 0, 0, 0, 0, 0, 2, 3, 0, 0, 0, 3, 2,
                2, 2, 2, 3, 3, 3, 3, 3, 3, 2, 3, 0, 2, 0, 3, 2,
                2, 3, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 3, 2,
                2, 3, 2, 0, 2, 2, 2, 2, 2, 2, 2, 2, 2, 0, 3, 2,
                2, 2, 2, 0, 2, 3, 3, 3, 3, 3, 3, 3, 2, 0, 3, 2,
                2, 0, 0, 0, 2, 2, 2, 2, 2, 2, 2, 2, 2, 0, 2, 2,
                2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 2,
                2, 3, 3, 0, 0, 0, 0, 0, 0, 4, 0, 2, 0, 1, 0, 2,
                2, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 2,
                2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2
                }; break;

                case 3: pixel = new int[]
                {
                2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
                2, 0, 0, 0, 0, 2, 0, 0, 0, 0, 2, 3, 2, 2, 3, 2,
                2, 2, 2, 2, 2, 2, 3, 2, 2, 3, 2, 3, 3, 3, 3, 2,
                2, 0, 0, 0, 0, 0, 0, 2, 2, 0, 0, 0, 0, 0, 3, 2,
                2, 2, 0, 0, 2, 2, 0, 2, 2, 0, 2, 3, 0, 0, 3, 2,
                2, 0, 0, 0, 0, 2, 0, 2, 2, 0, 2, 3, 0, 0, 3, 2,
                2, 0, 3, 3, 3, 2, 1, 2, 2, 4, 2, 3, 0, 0, 3, 2,
                2, 0, 0, 0, 0, 2, 2, 2, 2, 2, 2, 0, 0, 0, 0, 2,
                2, 3, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2,
                2, 3, 3, 3, 3, 2, 3, 3, 3, 3, 3, 3, 3, 3, 3, 2,
                2, 3, 3, 3, 3, 2, 3, 3, 3, 3, 3, 3, 3, 3, 3, 2,
                2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2
                }; break;

                case 4: pixel = new int[]
                {
                2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
                2, 0, 0, 0, 3, 0, 2, 0, 3, 3, 0, 0, 0, 0, 0, 2,
                2, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 2,
                2, 2, 3, 0, 3, 0, 2, 0, 3, 3, 0, 0, 0, 0, 0, 2,
                2, 0, 0, 0, 2, 0, 2, 0, 2, 2, 2, 2, 3, 0, 3, 2,
                2, 0, 3, 2, 2, 0, 2, 0, 2, 0, 3, 0, 3, 0, 3, 2,
                2, 0, 0, 0, 2, 0, 2, 0, 2, 0, 0, 4, 3, 0, 3, 2,
                2, 2, 3, 0, 2, 0, 2, 0, 2, 0, 3, 0, 3, 0, 3, 2,
                2, 0, 0, 0, 2, 0, 3, 0, 2, 0, 3, 3, 3, 0, 0, 2,
                2, 0, 1, 0, 2, 0, 0, 0, 2, 0, 0, 0, 3, 3, 0, 2,
                2, 0, 0, 0, 2, 0, 3, 0, 2, 3, 3, 0, 0, 0, 0, 2,
                2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2
                }; break;

                case 5:
                    {
                        if (BossEdition)
                        {
                            pixel = new int[]
                            {
                                3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3,
                                3, 0, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 3,
                                3, 3, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 3, 3,
                                3, 0, 0, 0, 2, 2, 0, 0, 0, 0, 3, 3, 0, 0, 0, 3,
                                3, 0, 0, 2, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 3,
                                3, 0, 0, 2, 0, 2, 2, 0, 0, 3, 0, 3, 3, 0, 0, 3,
                                3, 0, 0, 2, 0, 1, 2, 0, 0, 3, 0, 4, 3, 0, 0, 3,
                                3, 0, 0, 0, 2, 2, 0, 0, 0, 0, 3, 3, 0, 0, 0, 3,
                                3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3,
                                3, 3, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 3, 3,
                                3, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 3,
                                3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3
                            };
                        }
                        else
                        {
                            pixel = new int[]
                            {
                                3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3,
                                3, 0, 3, 0, 0, 3, 0, 0, 0, 0, 3, 0, 0, 3, 0, 3,
                                3, 3, 3, 0, 0, 3, 0, 3, 3, 0, 3, 0, 0, 3, 3, 3,
                                3, 0, 0, 0, 0, 3, 0, 2, 2, 0, 3, 0, 0, 0, 0, 3,
                                3, 0, 0, 0, 0, 3, 0, 2, 2, 0, 0, 0, 0, 0, 0, 3,
                                3, 0, 0, 0, 0, 3, 0, 2, 2, 3, 3, 0, 0, 0, 0, 3,
                                3, 0, 0, 0, 0, 2, 1, 2, 0, 4, 2, 0, 0, 0, 0, 3,
                                3, 0, 0, 0, 0, 2, 2, 2, 0, 2, 2, 0, 0, 0, 0, 3,
                                3, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 3,
                                3, 3, 3, 0, 0, 0, 3, 3, 3, 3, 0, 0, 0, 3, 3, 3,
                                3, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 3,
                                3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3
                            };
                        }

                        break;
                    }

                case 6: pixel = new int[]
                {
                2, 2, 2, 2, 3, 3, 3, 2, 2, 2, 2, 2, 2, 2, 2, 2,
                2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 2, 2, 2, 2, 2,
                2, 0, 3, 3, 3, 3, 0, 0, 0, 0, 3, 3, 3, 3, 2, 2,
                2, 0, 0, 3, 2, 3, 0, 0, 0, 0, 0, 0, 0, 3, 2, 2,
                2, 3, 4, 3, 2, 3, 3, 3, 3, 3, 3, 3, 0, 3, 2, 2,
                2, 3, 0, 3, 2, 3, 3, 3, 3, 3, 2, 3, 0, 3, 2, 2,
                2, 2, 2, 2, 2, 3, 0, 0, 0, 3, 2, 3, 0, 3, 2, 2,
                2, 2, 2, 2, 2, 3, 0, 3, 0, 3, 2, 3, 0, 3, 2, 2,
                2, 3, 0, 3, 3, 3, 0, 3, 0, 3, 2, 3, 0, 3, 2, 2,
                2, 0, 0, 0, 0, 0, 0, 3, 0, 3, 3, 3, 0, 3, 2, 2,
                2, 0, 1, 0, 2, 3, 3, 2, 0, 0, 0, 0, 0, 3, 2, 2,
                2, 2, 2, 2, 2, 2, 2, 2, 3, 3, 3, 3, 3, 3, 2, 2
                }; break;

                case 7: pixel = new int[]
                {
                2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
                2, 3, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2,
                2, 0, 3, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 2,
                2, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2,
                2, 0, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 2,
                2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2,
                2, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 0, 2,
                2, 0, 0, 0, 0, 0, 0, 2, 2, 2, 3, 0, 0, 0, 0, 2,
                2, 0, 4, 0, 3, 3, 0, 2, 2, 2, 0, 0, 0, 0, 0, 2,
                2, 0, 0, 0, 2, 2, 0, 3, 3, 3, 0, 0, 0, 3, 3, 2,
                2, 3, 0, 3, 2, 2, 0, 0, 0, 0, 0, 0, 0, 3, 0, 2,
                2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2
                }; break;

                case 8: pixel = new int[]
                {
                2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
                2, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 2,
                2, 0, 2, 0, 2, 0, 0, 0, 0, 0, 0, 3, 0, 4, 0, 2,
                2, 0, 2, 0, 2, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 2,
                2, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 2,
                2, 0, 2, 2, 2, 3, 3, 3, 3, 0, 3, 3, 0, 3, 3, 2,
                2, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 2,
                2, 2, 2, 0, 2, 3, 3, 0, 3, 3, 3, 3, 3, 0, 3, 2,
                2, 0, 0, 0, 2, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 2,
                2, 0, 1, 0, 2, 3, 0, 3, 3, 3, 0, 3, 3, 3, 3, 2,
                2, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 2,
                2, 2, 2, 2, 2, 3, 3, 3, 3, 3, 3, 2, 2, 2, 2, 2
                }; break;

                case 9: pixel = new int[]
                {
                2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
                2, 4, 3, 0, 0, 0, 0, 0, 0, 3, 0, 3, 0, 0, 0, 2,
                2, 0, 3, 0, 3, 3, 3, 3, 0, 3, 0, 0, 0, 0, 0, 2,
                2, 0, 3, 0, 3, 0, 0, 3, 0, 3, 0, 3, 0, 0, 0, 2,
                2, 0, 3, 0, 3, 0, 0, 3, 0, 3, 0, 3, 0, 0, 0, 2,
                2, 0, 3, 0, 3, 0, 0, 3, 0, 3, 0, 3, 0, 0, 0, 2,
                2, 0, 3, 0, 3, 0, 0, 3, 0, 3, 0, 3, 0, 0, 0, 2,
                2, 0, 3, 0, 3, 3, 3, 3, 0, 3, 0, 3, 0, 0, 0, 2,
                2, 0, 0, 0, 2, 0, 0, 2, 0, 0, 0, 3, 0, 0, 0, 2,
                2, 0, 0, 2, 2, 2, 2, 2, 2, 3, 0, 3, 0, 1, 0, 2,
                2, 0, 0, 2, 0, 0, 0, 0, 0, 3, 0, 3, 0, 0, 0, 2,
                2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2
                }; break;

                case 10: pixel = new int[]
                {
                3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3,
                3, 4, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 2, 2, 2, 3,
                3, 0, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 0, 1, 2, 3,
                3, 0, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 0, 2, 2, 3,
                3, 0, 3, 3, 3, 0, 3, 0, 3, 3, 3, 3, 0, 0, 0, 3,
                3, 0, 3, 3, 0, 3, 0, 3, 0, 3, 3, 3, 3, 3, 0, 3,
                3, 0, 3, 3, 0, 3, 3, 3, 0, 3, 3, 3, 3, 3, 0, 3,
                3, 0, 3, 3, 3, 0, 3, 0, 3, 3, 3, 3, 3, 3, 0, 3,
                3, 0, 3, 3, 3, 3, 0, 3, 3, 3, 3, 3, 3, 3, 0, 3,
                3, 0, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 0, 3,
                3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3,
                3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3
                }; break;

                case 11: pixel = new int[]
                {
                2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
                2, 0, 0, 0, 0, 3, 0, 0, 3, 0, 0, 0, 3, 0, 0, 2,
                2, 0, 1, 0, 3, 3, 3, 0, 3, 0, 0, 0, 3, 0, 0, 2,
                2, 0, 0, 0, 3, 0, 3, 0, 3, 0, 0, 0, 3, 0, 0, 2,
                2, 0, 0, 0, 3, 0, 3, 0, 3, 3, 3, 3, 3, 0, 0, 2,
                2, 0, 0, 0, 3, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 2,
                2, 0, 3, 0, 3, 0, 3, 0, 0, 3, 3, 3, 0, 0, 0, 2,
                2, 0, 0, 0, 3, 0, 3, 0, 0, 3, 0, 3, 0, 3, 0, 2,
                2, 3, 0, 3, 3, 0, 3, 0, 0, 3, 0, 3, 0, 0, 0, 2,
                2, 0, 0, 0, 3, 3, 3, 0, 0, 3, 3, 3, 0, 4, 0, 2,
                2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 2,
                2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2
                }; break;

                case 12: pixel = new int[]
                {
                2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
                2, 0, 0, 0, 2, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2,
                2, 0, 1, 0, 2, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2,
                2, 0, 0, 0, 2, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2,
                2, 5, 5, 5, 2, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2,
                2, 0, 0, 0, 2, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2,
                2, 0, 0, 0, 2, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2,
                2, 0, 0, 0, 2, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2,
                2, 0, 0, 0, 2, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2,
                2, 0, 0, 0, 2, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 2,
                2, 0, 0, 0, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
                2, 3, 0, 0, 5, 0, 5, 0, 5, 0, 5, 0, 5, 0, 4, 2,
                2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2
                }; break;

                case 13: pixel = new int[]
                {
                2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
                2, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 0, 3, 0, 3, 2,
                2, 0, 0, 0, 0, 0, 0, 0, 5, 5, 3, 2, 3, 0, 4, 2,
                2, 3, 3, 5, 3, 3, 3, 3, 3, 5, 3, 2, 3, 0, 3, 2,
                2, 0, 0, 0, 5, 5, 2, 2, 3, 0, 3, 2, 3, 0, 3, 2,
                2, 3, 3, 3, 3, 5, 3, 2, 3, 0, 3, 2, 3, 5, 3, 2,
                2, 0, 0, 0, 0, 0, 3, 0, 3, 5, 5, 0, 3, 5, 3, 2,
                2, 3, 3, 3, 3, 0, 3, 0, 3, 2, 3, 0, 3, 5, 3, 2,
                2, 0, 0, 0, 3, 0, 3, 0, 3, 2, 3, 0, 3, 5, 3, 2,
                2, 0, 1, 0, 5, 0, 3, 0, 3, 2, 3, 0, 3, 0, 3, 2,
                2, 0, 0, 0, 3, 0, 3, 0, 3, 0, 3, 0, 5, 0, 3, 2,
                2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2
                }; break;

                case 14: pixel = new int[]
                {
                2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
                2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2,
                2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2,
                2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2,
                2, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2,
                2, 0, 4, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2,
                2, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2,
                2, 5, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
                2, 5, 5, 5, 0, 3, 5, 5, 0, 5, 5, 5, 0, 0, 0, 2,
                2, 0, 5, 3, 5, 3, 5, 3, 5, 3, 5, 5, 0, 1, 0, 2,
                2, 5, 5, 3, 5, 5, 0, 5, 3, 5, 0, 5, 0, 0, 0, 2,
                2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2
                }; break;

                case 15: pixel = new int[]
                {
                2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 0,
                2, 5, 0, 5, 5, 5, 5, 3, 5, 0, 5, 3, 0, 0, 2, 0,
                2, 5, 3, 5, 5, 5, 0, 5, 5, 3, 5, 5, 0, 1, 2, 0,
                2, 5, 3, 5, 5, 5, 5, 3, 5, 0, 5, 3, 0, 0, 2, 0,
                2, 5, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
                2, 0, 5, 5, 5, 0, 5, 5, 5, 0, 5, 5, 5, 0, 5, 2,
                0, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 5, 2,
                2, 2, 3, 3, 3, 0, 3, 5, 0, 5, 3, 5, 0, 5, 5, 2,
                2, 4, 5, 5, 5, 5, 5, 5, 3, 5, 0, 5, 3, 3, 3, 2,
                2, 4, 5, 5, 5, 5, 5, 5, 3, 5, 0, 5, 3, 0, 0, 0,
                2, 2, 3, 3, 3, 0, 3, 5, 0, 5, 3, 5, 3, 0, 0, 0,
                0, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 0, 0, 0
                }; break;
            }
            for (int i = 0; i < pixel.Length; i++)
            {
                if (pixel[i] == 1)
                {
                    playerPos = i;
                    upcomingPos = i;
                    break;
                }
            }
        }
    }

    public class HeartGameDrawingSystem : ModSystem
    {
        public override void PostDrawInterface(SpriteBatch spriteBatch)
        {
            Player p = Main.LocalPlayer;

            if (p != null && p.active && !p.dead && p.TryGetModPlayer(out NucleusGame game) && game != null && (game.Active || p.GetModPlayer<TranscendencePlayer>().NucleusDeathAnim > 0))
            {
                int xOff = 0;
                int yOff = 0;
                int oFA = 0;

                int pixelSize = 32;

                int[] overflow = new int[16];

                var crt = ModContent.Request<Effect>("TranscendenceMod/Miscannellous/Assets/Shaders/Effects/CRTshader", AssetRequestMode.ImmediateLoad).Value;
                crt.Parameters["uOpacity"].SetValue(Main.rand.NextFloat(50f, 151f));

                if (game.BossEdition)
                    spriteBatch.Draw(TextureAssets.BlackTile.Value, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.Black);


                int x = Main.screenWidth / 2 - (pixelSize * 8);
                int y = Main.screenHeight / 2 - (pixelSize * 8);
                Rectangle gameRecB = new Rectangle(x - 18, y - 18, 16 * pixelSize + 4, 12 * pixelSize + 4);

                spriteBatch.Draw(TextureAssets.BlackTile.Value, gameRecB, Color.DarkRed);



                TranscendenceUtils.RestartSB(spriteBatch, BlendState.AlphaBlend, crt);


                int intensity = !game.BossEdition ? 0 : game.Time < 180 ? 16 : (int)MathHelper.Lerp(4, 1, game.Time / (float)game.MaxTime);

                Vector2 shake = Main.rand.NextVector2Circular(intensity, intensity);
                x += (int)shake.X;
                y += (int)shake.Y;


                if (game.Time != -1 && game.Time > 30)
                {
                    Rectangle gameRec = new Rectangle(x - 16, y - 16, 16 * pixelSize, 12 * pixelSize);
                    spriteBatch.Draw(TextureAssets.BlackTile.Value, gameRec, Color.Black);


                    for (int i = 0; i < game.pixel.Length; i++)
                    {
                        Texture2D sprite = TextureAssets.BlackTile.Value;
                        Color col = Color.DarkRed;


                        if (game.pixel[i] == 2)
                        {
                            sprite = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/UI/NucleusGame/Tile").Value;
                        }
                        if (game.pixel[i] == 3)
                            sprite = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/UI/NucleusGame/Spike").Value;

                        if (game.pixel[i] == 4)
                            sprite = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/UI/NucleusGame/Exit").Value;

                        if (game.pixel[i] == 5)
                        {
                            sprite = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/UI/NucleusGame/RhythmBlock").Value;
                            col = game.RhythmBlockON ? col : Color.Black;

                            if (game.playerPos == i)
                                col = Color.DarkRed;

                            if (game.RhythmBlockTimer > 45 && game.RhythmBlockTimer < 61 && game.RhythmBlockTimer % 5 == 0)
                                col = Color.Red;

                            if (game.RhythmBlockTimer > 105 && game.RhythmBlockTimer < 121 && game.RhythmBlockTimer % 5 == 0)
                                col = Color.Black;
                        }

                        if (game.pixel[i] == 6)
                            sprite = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/UI/NucleusGame/Reward").Value;

                        float rot = 0f;
                        if (game.upcomingPos == i)
                        {
                            sprite = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/UI/NucleusGame/Upcoming").Value;

                            if (game.upcomingPos == game.playerPos - 1)
                                rot = MathHelper.Pi + MathHelper.PiOver2;
                            if (game.upcomingPos == game.playerPos + 1)
                                rot = MathHelper.PiOver2;
                            if (game.upcomingPos > game.playerPos + 1)
                                rot = MathHelper.Pi;
                        }

                        if (game.playerPos == i)
                        {
                            sprite = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/UI/NucleusGame/Player").Value;
                            if (game.pixel[i] == 3 || game.pixel[i] == 5 && game.RhythmBlockON)
                                sprite = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/UI/NucleusGame/Dead").Value;
                        }

                        if (game.pixel[i] == 6 && game.CollectedReward && game.playerPos != i) { }
                        else
                        {
                            if (game.pixel[i] > 1 || game.playerPos == i || game.upcomingPos == i)
                            {
                                spriteBatch.Draw(sprite, new Rectangle(x + xOff - 2, y + yOff, pixelSize, pixelSize), null, Color.Blue, rot, sprite.Size() * 0.5f, SpriteEffects.None, 0f);
                                spriteBatch.Draw(sprite, new Rectangle(x + xOff + 2, y + yOff, pixelSize, pixelSize), null, Color.Lime, rot, sprite.Size() * 0.5f, SpriteEffects.None, 0f);
                                spriteBatch.Draw(sprite, new Rectangle(x + xOff, y + yOff, pixelSize, pixelSize), null, col, rot, sprite.Size() * 0.5f, SpriteEffects.None, 0f);
                            }
                        }

                        xOff += pixelSize;
                        overflow[oFA] += 1;

                        //Change row
                        if (overflow[oFA] >= 16)
                        {
                            oFA += 1;
                            yOff += pixelSize;
                            xOff = 0;
                        }
                    }


                    Texture2D sIMG = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/CRTshader").Value;

                    var eff = ModContent.Request<Effect>("TranscendenceMod/Miscannellous/Assets/Shaders/Effects/MovingNoiseTrans", AssetRequestMode.ImmediateLoad).Value;
                    eff.Parameters["uImageSize0"].SetValue(new Vector2(16 * pixelSize, 12 * pixelSize));
                    eff.Parameters["uImageSize1"].SetValue(sIMG.Size() * 4f);
                    eff.Parameters["uTime"].SetValue(0);
                    eff.Parameters["yChange"].SetValue(Main.GlobalTimeWrappedHourly / 0.2f);
                    eff.Parameters["useAlpha"].SetValue(true);
                    eff.Parameters["alpha"].SetValue(1f);
                    eff.Parameters["useExtraCol"].SetValue(false);
                    Main.instance.GraphicsDevice.Textures[1] = sIMG;

                    TranscendenceUtils.RestartSB(spriteBatch, BlendState.Additive, eff);

                    spriteBatch.Draw(TextureAssets.BlackTile.Value, gameRec, Color.White);

                    TranscendenceUtils.RestartSB(spriteBatch, BlendState.AlphaBlend, null);
                }


                TranscendenceUtils.RestartSB(spriteBatch, BlendState.AlphaBlend, null);



                if (game.BossEdition)
                {
                    spriteBatch.Draw(TextureAssets.BlackTile.Value, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.Red * (0.125f + (float)Math.Sin(Main.GlobalTimeWrappedHourly * 2f) * 0.075f));


                    if (p.dead)
                        spriteBatch.Draw(TextureAssets.BlackTile.Value, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.Black);


                    Texture2D nuc = ModContent.Request<Texture2D>("TranscendenceMod/NPCs/Boss/Nucleus/ProjectNucleus").Value;
                    Texture2D nuc2 = ModContent.Request<Texture2D>("TranscendenceMod/NPCs/Boss/Nucleus/ProjectNucleus_Glow_Red").Value;
                    Color glowCol = Color.White;
                    if (game.Time < 450)
                    {
                        nuc2 = ModContent.Request<Texture2D>("TranscendenceMod/NPCs/Boss/Nucleus/ProjectNucleus_Glow_Grey").Value;
                        glowCol = new Color(20, 20, 20);
                    }

                    spriteBatch.Draw(nuc, new Vector2(Main.screenWidth / 2f, Main.screenHeight / 2f - 670) + shake, null, Color.Lerp(Color.White, new Color(1f, 0.5f, 0.5f), (float)Math.Sin(Main.GlobalTimeWrappedHourly * 2f)), 0f, nuc.Size() * 0.5f, 14f, SpriteEffects.None, 0f);
                    spriteBatch.Draw(nuc2, new Vector2(Main.screenWidth / 2f, Main.screenHeight / 2f - 670) + shake, null, glowCol, 0f, nuc2.Size() * 0.5f, 14f, SpriteEffects.None, 0f);

                }

                DynamicSpriteFont font = FontAssets.MouseText.Value;
                if (OperatingSystem.IsWindows())
                    font = ModContent.Request<DynamicSpriteFont>(TranscendenceMod.ASSET_PATH + "/PixelatedFont", AssetRequestMode.ImmediateLoad).Value;

                int am = game.BossEdition ? 5 : 15;
                string room = game.room.ToString() + "/" + $"{am} ROOM";
                string time = (game.Time / 60).ToString() + " TIME";
                Color flashCol = !game.BossEdition ? Color.DarkRed : new Color(Main.masterColor, Main.masterColor / 4f, 0f);

                spriteBatch.Draw(TextureAssets.BlackTile.Value, new Rectangle((int)(Main.screenWidth / 2f - 120) - 2, y + 398, 244, game.room > 1 && game.BossEdition ? 104 : 64), flashCol);
                spriteBatch.Draw(TextureAssets.BlackTile.Value, new Rectangle((int)(Main.screenWidth / 2f - 120), y + 400, 240, game.room > 1 && game.BossEdition ? 100 : 60), Color.Black);

                if (game.room > 0)
                    ChatManager.DrawColorCodedStringWithShadow(spriteBatch, font, room, new Vector2(Main.screenWidth / 2f - font.MeasureString(room).X / 2f, y + 420) + shake, Color.Red, 0, Vector2.Zero, Vector2.One, -1, 1f);


                if (game.room > 1 && game.BossEdition)
                {
                    ChatManager.DrawColorCodedStringWithShadow(spriteBatch, font, time, new Vector2(Main.screenWidth / 2f - font.MeasureString(time).X / 2f, y + 450) + shake, Color.Red, 0, Vector2.Zero, Vector2.One, -1, 1f);

                    float boom = game.Time < 30 ? MathHelper.Lerp(1f, 0f, game.Time / 30f) : 0f;
                    spriteBatch.Draw(TextureAssets.BlackTile.Value, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White * boom);
                }

            }

            base.PostDrawInterface(spriteBatch);
        }
        public override void ModifyTransformMatrix(ref SpriteViewMatrix Transform)
        {
            base.ModifyTransformMatrix(ref Transform);

            if (Main.LocalPlayer != null && Main.LocalPlayer.active && Main.LocalPlayer.TryGetModPlayer(out TranscendencePlayer game) && game != null && game.NucleusConsumed > 0)
            {
                Transform.Zoom *= 1f + game.NucleusZoom * 8f;
            }

        }
    }
}