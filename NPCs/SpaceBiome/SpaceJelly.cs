using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Buffs;
using TranscendenceMod.Items.Consumables.FoodAndDrinks;
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Items.Materials.MobDrops;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Miscannellous.Biomes;
using TranscendenceMod.NPCs.Boss.Seraph;
using TranscendenceMod.Projectiles.NPCs.Bosses.SpaceBoss;

namespace TranscendenceMod.NPCs.SpaceBiome
{
    public class SpaceJelly : SpaceBiomeNPC
    {
        public int AttackDelay;
        public float ChaseSpeed = 8;
        public int dashCount;
        public Vector2 desiredPos;
        public int dashProj = ModContent.ProjectileType<DashBallProj>();
        Player player;
        public override void SetStaticDefaults()
        {
            NPCID.Sets.TrailCacheLength[Type] = 20;
            NPCID.Sets.TrailingMode[Type] = 3;

            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Shimmer] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Poisoned] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Venom] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Daybreak] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire3] = true;
            NPC.buffImmune[ModContent.BuffType<SpaceDebuff>()] = true;
        }

        public override void SetDefaults()
        {
            Main.npcFrameCount[Type] = 7;
            NPC.lifeMax = NPC.downedMoonlord ? 1250 : 140;
            NPC.damage = NPC.downedMoonlord ? 85 : 60;
            NPC.knockBackResist = 0.125f;

            NPC.width = 32;
            NPC.height = 32;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.npcSlots = 0.5f;

            NPC.HitSound = SoundID.NPCHit25;
            NPC.DeathSound = SoundID.NPCDeath28;

            NPC.friendly = false;
            NPC.value = Item.buyPrice(silver: NPC.downedMoonlord ? 45 : 7);
            SpawnModBiomes = new int[1] { ModContent.GetInstance<Heaven>().Type };
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PulverizedPlanet>(), 4, 1, 3));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CosmicJelly>(), 5));
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.GetModPlayer<TranscendencePlayer>().ZoneStar && !NPC.AnyNPCs(ModContent.NPCType<CelestialSeraph>()))
                return Main.dayTime ? 1.33f : 0.75f;
            else return 0;
        }
        public override void FindFrame(int frameHeight)
        {
            if (NPC.frame.Y != (44 * 6))
            {
                if (++NPC.frameCounter > 5)
                {
                    NPC.frame.Y += 44;
                    NPC.frameCounter = 0;
                }
            }
            else NPC.frame.Y = 0;
        }
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.damage = (int)(NPC.damage * 0.5f);
        }
        public override void AI()
        {
            NPC.TargetClosest();
            player = Main.player[NPC.target];

            NPC.rotation = NPC.velocity.ToRotation() + MathHelper.PiOver2;

            if (NPC.downedMoonlord)
            {
                if (++AttackDelay > 120)
                {
                    ChaseSpeed = 28;
                    if (AttackDelay > 180)
                    {
                        ChaseSpeed = 50;
                        AttackDelay = 0;
                    }
                }
            }
            else ChaseSpeed = 18;

            float accuracy = AttackDelay > 120 ? 0.05f : 0.025f;
            if (NPC.Distance(player.Center) > 70)
                NPC.velocity = Vector2.Lerp(NPC.velocity, NPC.DirectionTo(player.Center).RotatedByRandom(3f) * ChaseSpeed, accuracy);

        }
        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            if (AttackDelay > 120)
                target.AddBuff(BuffID.Electrified, 180);
            if (Main.rand.NextBool(3))
                target.AddBuff(ModContent.BuffType<SpaceDebuff>(), 120);
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                new FlavorTextBestiaryInfoElement("The death of Moon Lord has influenced wild life around it, an example can be seen here as a repurposed body for the Lord of the Moon. Stretches itself to catch prey."),
            });
            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new()
            {
                CustomTexturePath = "TranscendenceMod/Miscannellous/Assets/Bestiary/SpaceJellyBestiary",
                PortraitScale = 0.7f,
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
        }
        public override Color? GetAlpha(Color drawColor) => Color.White;
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D sprite = ModContent.Request<Texture2D>($"{Texture}_Tentacle").Value;
            Texture2D sprite2 = ModContent.Request<Texture2D>($"{Texture}_TentacleEnd").Value;

            Vector2 origin = new Vector2(sprite.Width * 0.5f, NPC.height * 0.5f);
            Vector2 origin2 = new Vector2(sprite2.Width * 0.5f, NPC.height * 0.5f);

            float alpha = (0.66f + (float)(Math.Sin(TranscendenceWorld.UniversalRotation * 4f) * 0.33f));
            Color col = Color.White * alpha;

            TranscendenceUtils.DrawEntity(NPC, Color.White * 0.25f, 8f * alpha, "bloom", 0, NPC.Center - new Vector2(NPC.width, NPC.height) * 0.35f, null);
            TranscendenceUtils.DrawEntity(NPC, TranscendenceWorld.CosmicPurple, 3f * alpha, "bloom", 0, NPC.Center - new Vector2(NPC.width, NPC.height) * 0.35f, null);
             
            for (int i = 0; i < (NPC.oldPos.Length / 2.15f); i++)
            {
                Main.EntitySpriteDraw(sprite, NPC.oldPos[i * 2] - new Vector2(NPC.width, NPC.height) * 0.3f - Main.screenPosition + origin + new Vector2(0, NPC.gfxOffY),
                    null, col, NPC.oldRot[i * 2], origin, 1, SpriteEffects.None);
                Main.EntitySpriteDraw(sprite, NPC.oldPos[i * 2] + new Vector2(NPC.width, -NPC.height) * 0.3f - Main.screenPosition + origin + new Vector2(0, NPC.gfxOffY),
                    null, col, NPC.oldRot[i * 2], origin, 1, SpriteEffects.None);
            }

            //Right tentacle end
            Main.EntitySpriteDraw(sprite2, NPC.oldPos[19] - new Vector2(NPC.width, NPC.height) * 0.2f - Main.screenPosition + origin2 + new Vector2(0, NPC.gfxOffY),
                null, col, NPC.oldRot[19], origin, 1, SpriteEffects.None);

            //Left tentacle end
            Main.EntitySpriteDraw(sprite2, NPC.oldPos[19] + new Vector2(NPC.width, -NPC.height) * 0.2f - Main.screenPosition + origin2 + new Vector2(0, NPC.gfxOffY),
                null, col, NPC.oldRot[19], origin, 1, SpriteEffects.None);
            TranscendenceUtils.DrawEntity(NPC, col, NPC.scale * 1.4f, $"{Texture}", NPC.rotation, NPC.Center - new Vector2(NPC.width, NPC.height) * 0.35f, null);

            Rectangle frame = new Rectangle(0, NPC.frame.Y, 34, 44);
            if (NPC.downedMoonlord && ChaseSpeed > 40)
                TranscendenceUtils.DrawEntity(NPC, Color.White, 2f, TextureAssets.GlowMask[25].Value, NPC.rotation, NPC.position, frame, frame.Size() * 0.5f);
            return false;
        }
    }
}

