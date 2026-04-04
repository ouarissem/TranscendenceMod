using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TranscendenceMod.Buffs;
using TranscendenceMod.Items.Materials.MobDrops;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Miscannellous.Biomes;
using TranscendenceMod.Miscannellous.GlobalStuff;

namespace TranscendenceMod.NPCs.PreHard.BloodWorm
{
    public class BloodWorm_Head : HeadSegment
    {
        public override int MaxSegments => 20;
        public int Segments2;

        public override int BodySegmentType => ModContent.NPCType<BloodWorm_Body>();

        public override int TailSegmentType => ModContent.NPCType<BloodWorm_Tail>();

        public override void OnSpawn(IEntitySource source)
        {
            NPC.GetGlobalNPC<TranscendenceNPC>().Worm = Main.rand.Next(0, int.MaxValue - 1);
        }

        public override void SetStaticDefaults()
        {
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Shimmer] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;
        }

        public override void SetDefaults()
        {
            NPC.lifeMax = Main.hardMode ? 175 : 20;
            NPC.damage = Main.hardMode ? 40 : 15;
            NPC.defense = 5;
            NPC.knockBackResist = 0;

            NPC.width = 18;
            NPC.height = 18;

            NPC.aiStyle = -1;
            NPC.noGravity = true;
            NPC.noTileCollide = true;

            NPC.HitSound = SoundID.NPCHit19;
            NPC.DeathSound = SoundID.NPCDeath22;

            NPC.friendly = false;
            NPC.value = Item.sellPrice(silver: 5);
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemID.Vertebrae, 2));
            npcLoot.Add(ItemDropRule.Common(ItemID.WormTooth, 1, 5, 12));
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.ZoneCrimson && NPC.CountNPCS(Type) < 3)
                return 0.185f;
            else return 0;
        }
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
        }
        public override void AI()
        {
            NPC.TargetClosest();
            Player player = Main.player[NPC.target];

            if (NPC.Distance(player.Center) > 1000)
                NPC.velocity = NPC.DirectionTo(player.Center) * 5f;

            if (Collision.SolidCollision(NPC.position, NPC.width, NPC.height))
            {
                if (++NPC.ai[1] % 25 == 0)
                    SoundEngine.PlaySound(SoundID.WormDig with { MaxInstances = 0 }, NPC.Center);

                if (NPC.ai[2] < 40)
                    NPC.ai[2]++;

                if (NPC.ai[2] > 20)
                    NPC.velocity = Vector2.Lerp(NPC.velocity, NPC.DirectionTo(player.Center) * 10f, 1f / 10f);

                return;
            }
            else
                NPC.ai[2]--;

            if (NPC.ai[2] < 10)
                NPC.velocity.Y += 0.2f;



            NPC.rotation = NPC.velocity.ToRotation() + MathHelper.PiOver2;
            NPC npc = NPC;

            if (Segments2 < MaxSegments)
            {
                int n = NPC.NewNPC(npc.GetSource_FromAI(), (int)npc.Center.X, (int)npc.Center.Y, BodySegmentType, npc.whoAmI);
                Main.npc[n].ai[0] = npc.whoAmI;
                Main.npc[n].ai[1] = Segments2;
                Main.npc[n].ai[2] = MaxSegments;
                Main.npc[n].lifeMax = npc.lifeMax;
                Main.npc[n].realLife = npc.whoAmI;

                if (Segments2 == MaxSegments - 1)
                {
                    int n2 = NPC.NewNPC(npc.GetSource_FromAI(), (int)npc.Center.X, (int)npc.Center.Y, TailSegmentType, npc.whoAmI);
                    Main.npc[n2].ai[0] = npc.whoAmI;
                    Main.npc[n2].ai[1] = MaxSegments;
                    Main.npc[n2].lifeMax = npc.lifeMax;
                    Main.npc[n2].realLife = npc.whoAmI;
                }
                Segments2++;
            }
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCrimson,
                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.TranscendenceMod.Messages.Bestiary.EarthernScourge")),
            });
            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new()
            {
                CustomTexturePath = "TranscendenceMod/Miscannellous/Assets/Bestiary/EarthernScourgeBestiary",
                PortraitScale = 0.2f,
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
        }
        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position) => true;
    }
    public class BloodWorm_Body : BodySegment
    {
        public override float DistanceBetweenSegments => 1f;

        public override void AI()
        {
            BodyAI(NPC);
        }
        public override void SetStaticDefaults()
        {
            TranscendenceUtils.BeGoneBestiary(NPC);

            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Shimmer] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;
        }
        public override void SetDefaults()
        {
            NPC.lifeMax = 5;
            NPC.damage = Main.hardMode ? 40 : 15;
            NPC.knockBackResist = 0;
            NPC.takenDamageMultiplier = 0.33f;

            NPC.width = 18;
            NPC.height = 16;

            NPC.aiStyle = -1;
            NPC.noGravity = true;
            NPC.noTileCollide = true;

            NPC.HitSound = SoundID.NPCHit19;
            NPC.DeathSound = SoundID.NPCDeath22;

            NPC.friendly = false;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            //TranscendenceUtils.DrawEntity(NPC, drawColor, NPC.scale, Texture, NPC.rotation, NPC.Center, null);
            return true;
        }
        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position) => false;
    }
    public class BloodWorm_Tail : TailSegment
    {
        public override float DistanceBetweenSegments => 0.75f;
        public override void SetStaticDefaults()
        {
            TranscendenceUtils.BeGoneBestiary(NPC);

            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Shimmer] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;
        }
        public override void AI()
        {
            BodyAI(NPC);
        }
        public override void SetDefaults()
        {
            NPC.lifeMax = 5;
            NPC.damage = Main.hardMode ? 40 : 15;
            NPC.knockBackResist = 0;

            NPC.width = 18;
            NPC.height = 16;

            NPC.aiStyle = -1;
            NPC.noGravity = true;
            NPC.noTileCollide = true;

            NPC.HitSound = SoundID.NPCHit19;
            NPC.DeathSound = SoundID.NPCDeath22;

            NPC.friendly = false;
        }
        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position) => false;
    }
}

