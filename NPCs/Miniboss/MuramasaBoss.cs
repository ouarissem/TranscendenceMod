using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.UI.ModBrowser;
using TranscendenceMod.Items.Accessories.Offensive;
using TranscendenceMod.Items.Accessories.Other;
using TranscendenceMod.Items.Consumables;
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Items.Materials.MobDrops;
using TranscendenceMod.Items.Weapons.Magic;
using TranscendenceMod.Items.Weapons.Melee;
using TranscendenceMod.Items.Weapons.Ranged;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Miscannellous.GlobalStuff;
using TranscendenceMod.Projectiles.NPCs.Bosses.Muramasa;
using static TranscendenceMod.TranscendenceWorld;

namespace TranscendenceMod.NPCs.Miniboss
{
    [AutoloadBossHead]
    public class MuramasaBoss : ModNPC
    {
        public int Timer;
        public int ProjectileCD;
        public int Duration;
        public int ContactDMG = 50;
        public bool FoundPos;
        public Vector2 DashPos;
        Player player;
        public override void SetStaticDefaults()
        {
            NPCID.Sets.TrailCacheLength[Type] = 25;
            NPCID.Sets.TrailingMode[Type] = 1; 
        }
        public override void SetDefaults()
        {
            /*Stats*/
            NPC.lifeMax = Downed.Contains(Bosses.Atmospheron) ? 75000 : 1275;
            NPC.defense = Downed.Contains(Bosses.Atmospheron) ? 120 : 25;
            NPC.damage = Downed.Contains(Bosses.Atmospheron) ? 80 : 40;
            NPC.value = Downed.Contains(Bosses.Atmospheron) ? Item.sellPrice(gold: 17, silver: 50) : Item.sellPrice(gold: 2, silver: 50);
            NPC.width = Downed.Contains(Bosses.Atmospheron) ? 74 : 64;
            NPC.height = Downed.Contains(Bosses.Atmospheron) ? 74 : 64;
            NPC.aiStyle = 0;
            NPC.rarity = 3;

            /*Colision*/
            NPC.noGravity = true;
            NPC.noTileCollide = true;

            /*Audio*/
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath6;

            NPC.friendly = false;
            NPC.knockBackResist = 0f;
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.OneFromOptions(1,
                ModContent.ItemType<SewerHoop>(),
                ModContent.ItemType<CobaltWaterGun>(),
                ModContent.ItemType<WaterKunai>(),
                ModContent.ItemType<RingOfBravery>(),
                ModContent.ItemType<LeatherGlove>()));

            npcLoot.Add(ItemDropRule.Common(ItemID.GoldenKey, 1, 2, 4));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<SturdyPlate>(), 1, 1, 3));
            npcLoot.Add(ItemDropRule.ByCondition(new DragonDropRule(), ModContent.ItemType<PoseidonsTide>(), 1, 8, 12));
        }
        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return NPC.ai[1] != 5;
        }
        public override void AI()
        {
            player = Main.player[NPC.target];
            Player local = Main.LocalPlayer;

            if (NPC.life > (int)(NPC.lifeMax * 0.925f))
            {
                NPC.rotation = MathHelper.PiOver2 + MathHelper.PiOver4;
                NPC.chaseable = false;

                return;
            }
            else
            {
                NPC.chaseable = true;

                if (NPC.ai[2] == 5)
                    SoundEngine.PlaySound(ModSoundstyles.SeraphSwords_Draw, NPC.Center);

                NPC.ai[2]++;

                NPC.npcSlots = 12f;

                if (NPC.ai[2] < 180)
                {
                    NPC.rotation += 0.1f;
                    return;
                }
            }

            if (++Timer > Duration)
            {
                NPC.ai[1]++;
                NPC.velocity = Vector2.Zero;
                FoundPos = false;
                NPC.ai[0] = 0;
                NPC.ai[3] = 0;
                Timer = 0;
            }
            //Attack Patterns
            switch (NPC.ai[1])
            {
                case 1: Spin(); break;
                case 2: Slash(); break;
                case 3: ComeHere(); break;
                case 4: Slam(); break;
                case 5: SpeedSlice(); break;
                case 6: NPC.ai[1] = 1; goto case 1;
            }

            void Spin()
            {
                Duration = 130;
                if (Timer < 90)
                {
                    DashPos = NPC.DirectionTo(player.Center);
                    NPC.rotation += 0.25f;
                }
                else
                {
                    Dust p = Dust.NewDustPerfect(NPC.Center, DustID.DungeonWater, -NPC.velocity / 3, 0, default, 1.5f);
                    p.noGravity = true;
                    NPC.rotation = DashPos.ToRotation() + MathHelper.PiOver4;
                    NPC.velocity = DashPos * 25;
                }
            }
            
            void Slash()
            {
                Duration = 240;

                if (Timer < 90)
                {
                    if (Timer < 60)
                        DashPos = NPC.DirectionTo(player.Center + player.velocity * 10f);
                    NPC.rotation += 0.375f;
                }
                else
                {
                    Dust p = Dust.NewDustPerfect(NPC.Center, DustID.DungeonWater, -NPC.velocity / 3, 0, default, 1.5f);
                    p.noGravity = true;

                    NPC.rotation = DashPos.ToRotation() + MathHelper.PiOver4;

                    if (NPC.ai[3] != 1)
                    {
                        NPC.velocity = DashPos * 28f;

                        if (Collision.SolidCollision(NPC.Center - new Vector2(8), 16, 16) && Timer > 115)
                        {
                            SoundEngine.PlaySound(SoundID.DD2_MonkStaffGroundImpact, NPC.Center);
                            TranscendenceUtils.ProjectileRing(NPC, 12, NPC.GetSource_FromAI(), NPC.Center, ModContent.ProjectileType<MuramasaDeathLaser>(), Downed.Contains(Bosses.Atmospheron) ? 100 : 30, 2f, 1f, 1f, 0f, 0.75f, -1, Main.rand.NextFloat(MathHelper.TwoPi));

                            NPC.velocity = Vector2.Zero;
                            NPC.ai[3] = 1;
                        }
                    }
                }
            }

            void ComeHere()
            {
                Duration = 260;
                Vector2 pos = NPC.Center + Vector2.One.RotatedBy(MathHelper.ToRadians(Timer * 7.5f)) * (550 - Timer);
                Vector2 pos2 = NPC.Center + Vector2.One.RotatedBy(-MathHelper.ToRadians(Timer * 7.5f)) * (450 - (Timer * 0.75f));
                NPC.rotation += 0.5f;

                if (Timer < 60)
                {
                    Dust.NewDust(pos, 1, 1, DustID.DungeonWater);
                    Dust.NewDust(pos2, 1, 1, DustID.DungeonWater);
                }
                else
                {
                    if (Timer % 5 == 0)
                        SoundEngine.PlaySound(SoundID.Item71 with { MaxInstances = 0 }, Main.rand.NextBool(2) ? pos : pos2);

                    Projectile.NewProjectile(NPC.GetSource_FromAI(), pos, -NPC.DirectionTo(pos) * 12,
                        ModContent.ProjectileType<MuramasaShred>(), Downed.Contains(Bosses.Atmospheron) ? 100 : 30, 1, -1, 1, 1, 1);

                    Projectile.NewProjectile(NPC.GetSource_FromAI(), pos2, -NPC.DirectionTo(pos2) * 7,
                        ModContent.ProjectileType<MuramasaShred>(), Downed.Contains(Bosses.Atmospheron) ? 100 : 30, 1, -1, 1, 1, 1);
                }
            }


            void SpeedSlice()
            {
                Duration = 150;
                NPC.rotation += 0.5f;

                if (Timer > 45 && Timer < (Duration - 60) && Timer % (Downed.Contains(Bosses.Atmospheron) ? 6 : 10) == 0)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        Vector2 pos = player.Center + Vector2.One.RotatedByRandom(MathHelper.TwoPi) * 750f;

                        int p = Projectile.NewProjectile(NPC.GetSource_FromAI(), pos, pos.DirectionTo(player.Center).RotatedByRandom(MathHelper.PiOver2),
                            ModContent.ProjectileType<MuramasaDeathLaser>(), Downed.Contains(Bosses.Atmospheron) ? 90 : 25, 1, -1, 1, 0, 3);
                        Main.projectile[p].extraUpdates = 0;
                    }
                }
            }

            void Slam()
            {
                Duration = 350;

                if (Timer < 45)
                {
                    NPC.rotation += 0.5f;
                    DashPos = player.Center - new Vector2(0, 400);
                }
                if (Timer > 45 && Timer < 90)
                {
                    if (NPC.Distance(DashPos) < 75)
                    {
                        NPC.rotation = MathHelper.PiOver2 - (MathHelper.PiOver4 / 2f);
                        NPC.velocity = Vector2.Zero;
                    }
                    else
                    {
                        NPC.rotation = NPC.DirectionTo(DashPos).ToRotation() + MathHelper.PiOver4;
                        NPC.velocity = NPC.DirectionTo(DashPos) * 30f;
                    }
                }
                if (Timer > 90)
                {
                    NPC.velocity.X = 0;

                    if (NPC.ai[3] != 1)
                    {
                        NPC.velocity.Y = 25f;

                        if (Timer % 5 == 0)
                        {
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, new Vector2(2, 0), ModContent.ProjectileType<MuramasaSlash>(), Downed.Contains(Bosses.Atmospheron) ? 100 : 30, 2, -1, 0, 0, 2);
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, new Vector2(-2, 0), ModContent.ProjectileType<MuramasaSlash>(), Downed.Contains(Bosses.Atmospheron) ? 100 : 30, 2, -1, 0, 0, 2);
                        }


                        if (Collision.SolidCollision(NPC.Center - new Vector2(8), 16, 16) && NPC.position.Y > (player.position.Y - 20))
                        {
                            SoundEngine.PlaySound(SoundID.DD2_MonkStaffGroundImpact, NPC.Center);
                            NPC.velocity.Y = 0;
                            NPC.ai[3] = 1;
                        }
                    }
                    else
                    {
                        if ((++NPC.ai[0] < 60 || Downed.Contains(Bosses.Atmospheron)) && Timer % 10 == 0)
                        {
                            float rand = Main.rand.NextFloat(-25f, 25f);
                            int rand2 = Main.rand.Next(2, 9);
                            
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.DirectionTo(player.Center) * 4f, ModContent.ProjectileType<MuramasaSlash>(), Downed.Contains(Bosses.Atmospheron) ? 80 : 20, 2, -1, 0, 0, 2);

                            SoundEngine.PlaySound(SoundID.Item71 with { MaxInstances = 0}, NPC.Center);
                        }
                        if (NPC.ai[0] > 90)
                            NPC.ai[0] = 0;
                    }
                }
            }
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (NPC.AnyNPCs(Type))
                return 0f;

            if (spawnInfo.Player.ZoneDungeon)
                return Downed.Contains(Bosses.Muramasa) ? 0.0075f : 0.025f;

            return 0f;
        }
        public override bool? CanFallThroughPlatforms() => true;
        public override bool PreKill()
        {
            if (!Downed.Contains(Bosses.Muramasa))
                Downed.Add(Bosses.Muramasa);
            return base.PreKill();
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            string sprite = Texture;
            if (Downed.Contains(Bosses.Atmospheron))
                sprite = "TranscendenceMod/Items/Weapons/Melee/UpgradedMuramasa";

            if (NPC.ai[1] == 1 && Timer < 90 ||NPC.ai[1] == 2 && Timer < 60)
            {
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);

                Texture2D sprite2 = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/BloomLine").Value;
                spriteBatch.Draw(sprite2, new Rectangle(
                    (int)(NPC.Center.X - Main.screenPosition.X), (int)(NPC.Center.Y - Main.screenPosition.Y), 16,
                    1250), null,
                    Color.DeepSkyBlue * 0.75f, NPC.DirectionTo(player.Center).ToRotation() + MathHelper.PiOver2, sprite2.Size() * 0.5f, SpriteEffects.None, 0);

                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);
            }

            TranscendenceUtils.DrawTrailNPC(NPC, NPC.GetAlpha(drawColor), NPC.scale, sprite, false, true, 1, Vector2.Zero);

            TranscendenceUtils.DrawEntity(NPC, Color.White, NPC.scale, sprite, NPC.rotation, NPC.Center, null);
            return false;
        }
        public override void OnSpawn(IEntitySource source)
        {
            if (NPC.downedPlantBoss)
            {
                NPC.ai[0] = 1;
            }

        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheDungeon,
                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.TranscendenceMod.Messages.Bestiary.Muramasa1")),
            });
        }
    }
}