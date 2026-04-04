using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TranscendenceMod.Buffs;
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Items.Materials.MobDrops;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Miscannellous.Biomes;
using TranscendenceMod.Projectiles;
using TranscendenceMod.Projectiles.NPCs.Bosses.SpaceBoss;

namespace TranscendenceMod.NPCs.SpaceBiome
{
    public class VoidSlime : SpaceBiomeNPC
    {
        public int AttackTimer;
        public bool TouchedGround;
        public float Fade;
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 2;
            NPCID.Sets.TrailCacheLength[Type] = 20;
            NPCID.Sets.TrailingMode[Type] = 1;
            NPCID.Sets.ImmuneToRegularBuffs[Type] = true;
        }

        public override void SetDefaults()
        {
            NPC.lifeMax = 3750;
            NPC.defense = 50;
            NPC.damage = 60;
            NPC.knockBackResist = NPC.downedMoonlord ? 0f : 0.25f;

            NPC.width = 40;
            NPC.height = 34;
            NPC.noGravity = true;
            NPC.noTileCollide = false;
            NPC.lavaImmune = true;

            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;

            NPC.friendly = false;
            NPC.value = Item.buyPrice(silver: 25);
            SpawnModBiomes = new int[1] { ModContent.GetInstance<Heaven>().Type };
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<VoidFragment>(), 1, 3, 5));
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.GetModPlayer<TranscendencePlayer>().ZoneStar && NPC.CountNPCS(Type) < 2 && NPC.downedMoonlord)
                return 1.5f;
            if (spawnInfo.Player.ZoneOverworldHeight && NPC.CountNPCS(Type) < 3 && NPC.downedMoonlord)
                return 0.0675f;

            return 0f;
        }
        public override Color? GetAlpha(Color drawColor) => Color.White;
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D sprite = ModContent.Request<Texture2D>(Texture).Value;
            for (int i = 0; i < NPC.oldPos.Length; i++)
            {
                TranscendenceUtils.DrawEntity(NPC, Color.Lerp(Color.White, Color.Transparent, i / (float)NPC.oldPos.Length), NPC.scale, sprite, NPC.rotation, NPC.oldPos[i] + NPC.Size / 2f, NPC.frame, NPC.frame.Size() / 2f, SpriteEffects.None);
            }
            return base.PreDraw(spriteBatch, screenPos, drawColor);
        }
        public override void AI()
        {
            NPC.TargetClosest();
            Player player = Main.player[NPC.target];
            NPC.direction = player.Center.X > NPC.Center.X ? 1 : -1;

            if (NPC.ai[0] == 0)
                NPC.ai[0] = 1;

            if (Collision.SolidCollision(NPC.position, NPC.width, NPC.height) && !Collision.SolidCollision(NPC.position - new Vector2(0, 16), NPC.width, 8))
                NPC.position.Y -= 16f;

            bool solid = Collision.SolidCollision(NPC.Left, NPC.width, NPC.height);
            if (player.Distance(NPC.Center) < 500 && NPC.scale < 4f && !solid)
            {
                NPC.scale += 1f / 120f;
                NPC.position = NPC.Center;
                NPC.position -= NPC.Size * 0.500175f;
            }

            NPC.width = (int)(40 * NPC.scale);
            NPC.height = (int)(34 * NPC.scale);

            if (AttackTimer < 21 && solid)
            {
                if (!TouchedGround)
                {
                    SoundEngine.PlaySound(SoundID.Item167, NPC.Center);
                    for (int i = 0; i < 8; i++)
                    {
                        Vector2 pos = Vector2.Lerp(NPC.BottomLeft, NPC.BottomRight, i / 8f);

                        Gore gore = Gore.NewGoreDirect(NPC.GetSource_FromAI(), pos, new Vector2(Main.rand.Next(-15, 15), Main.rand.Next(5, 15)), Main.rand.Next(375, 378), Main.rand.NextFloat(1f, 1.75f) * NPC.scale / 3f);
                        gore.velocity.X = Main.rand.NextFloat(-6f, 6f);
                        gore.velocity.Y = Main.rand.NextFloat(-6f, 2f);
                    }

                    TouchedGround = true;
                }

                NPC.ai[2] = NPC.direction;
                NPC.velocity.X *= 0.9f;
                AttackTimer++;
            }

            float sizeMod = MathHelper.Lerp(1f, 1.5f, NPC.scale / 4f);
            NPC.velocity.Y += 1f * sizeMod;

            if (AttackTimer > 20)
            {
                AttackTimer++;
                TouchedGround = false;

                if (AttackTimer < 30)
                    NPC.velocity.Y = (NPC.ai[0] > 0 ? -20f : -12.5f) * sizeMod;
                else
                {
                    if ((player.Center.X > (NPC.Center.X - 50) && player.Center.X < (NPC.Center.X + 50) || solid) && AttackTimer > 32)
                        NPC.velocity.X *= 0.7f;

                    else NPC.velocity.X = (NPC.ai[0] > 0 ? 10f : 17.5f) * NPC.ai[2] * sizeMod;
                }

                if (AttackTimer > 60)
                {
                    NPC.ai[0] = -NPC.ai[0];
                    AttackTimer = 0;
                }
            }
        }
        public override void FindFrame(int frameHeight)
        {
            if (!Collision.SolidCollision(NPC.Left, NPC.width, NPC.height))
            {
                NPC.frame.Y = 34;
                return;
            }

            if (NPC.frame.Y != frameHeight)
            {
                if (++NPC.frameCounter > 5)
                {
                    NPC.frame.Y += frameHeight;
                    NPC.frameCounter = 0;
                }
            }
            else NPC.frame.Y = 0;
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(ModContent.BuffType<SpaceDebuff>(), 120);
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.TranscendenceMod.Messages.Bestiary.EmpyreanSlime")),
            });
        }
        public override bool? CanFallThroughPlatforms() => true;
    }
}

