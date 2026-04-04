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
using Terraria.ModLoader.Utilities;
using TranscendenceMod.Dusts;
using TranscendenceMod.Items.Materials.MobDrops;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Projectiles;
using TranscendenceMod.Projectiles.NPCs;
using TranscendenceMod.Projectiles.NPCs.Guardians;
using static TranscendenceMod.TranscendenceWorld;

namespace TranscendenceMod.NPCs.Miniboss
{
    public class FlameGuardian : ModNPC
    {
        public override void SetStaticDefaults() { }

        public override void SetDefaults()
        {
            NPC.lifeMax = 1050;
            NPC.defense = 15;
            NPC.damage = 30;
            NPC.knockBackResist = 0f;

            NPC.width = 42;
            NPC.height = 42;
            NPC.noGravity = true;
            NPC.noTileCollide = true;

            NPC.HitSound = SoundID.Tink;
            NPC.DeathSound = SoundID.NPCDeath8;
            NPC.Opacity = 0f;

            NPC.friendly = false;
            Music = MusicID.OtherworldlyWoF;
        }
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * (Main.masterMode ? 0.55f : 0.5875f));
            NPC.damage = (int)(NPC.damage * (Main.masterMode ? 0.525f : 0.625f));
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D sprite = ModContent.Request<Texture2D>(Texture).Value;
            Texture2D sprite2 = ModContent.Request<Texture2D>(Texture + "_Glow").Value;

            Rectangle rec = NPC.frame;
            Vector2 pos = NPC.Center - screenPos;
            Vector2 origin = rec.Size() * 0.5f;

            SpriteBatch sb = Main.spriteBatch;

            spriteBatch.Draw(sprite, pos, rec, drawColor * NPC.Opacity, NPC.rotation, origin, NPC.scale,
                NPC.direction == 1 ? SpriteEffects.FlipVertically : SpriteEffects.None, 0);

            spriteBatch.Draw(sprite2, pos, rec, Color.White * NPC.Opacity, NPC.rotation, origin, NPC.scale,
                NPC.direction == 1 ? SpriteEffects.FlipVertically : SpriteEffects.None, 0);
            return false;
        }
        public override bool PreKill()
        {
            if (!Downed.Contains(Bosses.FlameGuardian))
                Downed.Add(Bosses.FlameGuardian);
            NPC.boss = false;

            return base.PreKill();
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
                        int gore = Mod.Find<ModGore>($"FGGore_{i}").Type;
                        Gore.NewGore(NPC.GetSource_Death(), NPC.Center, Main.rand.NextVector2Circular(3f, 3f), gore);
                    }
                }
            }
        }
        public override void AI()
        {
            if (!NPC.boss)
                NPC.boss = true;

            NPC.TargetClosest();
            Player player = Main.player[NPC.target];
            NPC.direction = player.Center.X > NPC.Center.X ? 1 : -1;

            NPC.rotation = NPC.DirectionTo(player.Center).ToRotation() + MathHelper.Pi;
            NPC.dontTakeDamage = NPC.ai[2] == 0f || player.Distance(NPC.Center) > 375;

            switch (NPC.ai[2])
            {
                case 0: Intro(); break;
                case 1: Minions(); break;
                case 2: Projectiles(); break;
                case 3: NPC.ai[2] = 1; goto case 1;
            }

            if (++NPC.ai[0] > NPC.ai[1])
            {
                NPC.ai[2]++;
                NPC.ai[0] = 0f;
                NPC.Opacity = 1f;
            }
            NPC.ai[3]++;

            void Intro()
            {
                NPC.ai[1] = 180f;
                player.GetModPlayer<TranscendencePlayer>().cameraModifier = true;
                player.GetModPlayer<TranscendencePlayer>().cameraPos = Vector2.Lerp(player.Center, NPC.Center, NPC.ai[0] < 45 ? NPC.ai[0] / 45f : 1f);

                if (Main.rand.NextBool(5))
                {
                    Vector2 pos = NPC.Center + Vector2.One.RotatedByRandom(MathHelper.TwoPi) * 64f;
                    Vector2 vel = pos.DirectionTo(NPC.Center) * 8f;

                    Dust d = Dust.NewDustPerfect(pos, DustID.Torch, vel, 0, default, 2f);
                    d.noGravity = true;
                }

                if (NPC.ai[0] > 30)
                {
                    NPC.Opacity = MathHelper.Lerp(NPC.Opacity, 1f, 1f / 60f);
                }

                if ((int)NPC.ai[0] == 150)
                {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<Shockwave>(), 2500, 50, -1, 1f, 0.5f, 0);
                    SoundEngine.PlaySound(SoundID.Roar, NPC.Center);
                }

            }

            void Minions()
            {
                NPC.ai[1] = 430f;

                if (NPC.ai[3] >= 10f && NPC.ai[0] >= 60f && NPC.ai[0] < 340f)
                {
                    int p = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, new Vector2(Main.rand.NextFloat(4f, 8f) * Main.rand.NextFloatDirection(), 0.25f), ModContent.ProjectileType<FlameSummon>(), 25, 0f, -1, 0, NPC.whoAmI);

                    // Skeleton spawns
                    if (((int)NPC.ai[0] == 250 || (int) NPC.ai[0] == 320) && NPC.CountNPCS(NPCID.SmallSkeleton) < 4)
                        Main.projectile[p].ai[0] = 1f;

                    NPC.ai[3] = 0f;
                }
            }

            void Projectiles()
            {
                NPC.ai[1] = 51f;

                if (NPC.ai[0] <= 30)
                {
                    Vector2 pos = NPC.Center + Vector2.One.RotatedBy(NPC.DirectionTo(player.Center).ToRotation() - MathHelper.PiOver4) * 12f;
                    Vector2 vel = pos.DirectionTo(player.Center).RotatedByRandom(0.3f) * 12f;

                    Dust d = Dust.NewDustPerfect(pos, ModContent.DustType<Ember2>(), vel, 0, default, 1f);
                    d.noGravity = true;

                    return;
                }

                if (NPC.ai[3] % 10 == 0)
                {
                    SoundEngine.PlaySound(SoundID.Item158, NPC.Center);
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.DirectionTo(player.Center), ModContent.ProjectileType<FlameLaser>(), 35, 0f, -1, 0, NPC.whoAmI);

                    float rot = NPC.DirectionTo(player.Center).ToRotation() - MathHelper.PiOver2;
                    for (int j = 0; j < 16; j++)
                    {
                        Vector2 vec = Vector2.One.RotatedBy(MathHelper.TwoPi * j / 16f) * 25f;

                        Vector2 pos = NPC.Center + new Vector2(vec.X, vec.Y / 2.5f).RotatedBy(rot) + Vector2.One.RotatedBy(rot + MathHelper.PiOver4) * 37.5f;
                        Dust d = Dust.NewDustPerfect(pos, DustID.Torch, Vector2.Zero, 0, Color.White, 3f);
                        d.noGravity = true;
                    }
                }
            }
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns,
                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.TranscendenceMod.Messages.Bestiary.StormEel")),
            });
        }
    }
}

