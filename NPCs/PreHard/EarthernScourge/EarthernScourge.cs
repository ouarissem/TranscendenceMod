using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TranscendenceMod.Buffs;
using TranscendenceMod.Items.Materials.MobDrops;
using TranscendenceMod.Items.Modifiers.Upgrades;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Miscannellous.Biomes;
using TranscendenceMod.Miscannellous.GlobalStuff;

namespace TranscendenceMod.NPCs.PreHard.EarthernScourge
{
    public class EarthernScourge_Head : HeadSegment
    {
        public override int MaxSegments => 10;
        public int Segments2;
        public float rot;
        public int Timer;
        public float speed;

        public override int BodySegmentType => ModContent.NPCType<EarthernScourge_Body>();

        public override int TailSegmentType => ModContent.NPCType<EarthernScourge_Tail>();

        public override void OnSpawn(IEntitySource source)
        {
            //SpawnSegments(NPC);
            NPC.GetGlobalNPC<TranscendenceNPC>().Worm = Main.rand.Next(0, int.MaxValue - 1);
        }

        public override void SetStaticDefaults()
        {
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Shimmer] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Poisoned] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Venom] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire3] = true;
        }

        public override void SetDefaults()
        {
            NPC.lifeMax = Main.hardMode ? 500 : 80;
            NPC.damage = Main.hardMode ? 65 : 45;
            NPC.defense = 5;
            NPC.knockBackResist = 0;

            NPC.width = 30;
            NPC.height = 52;

            NPC.aiStyle = -1;
            NPC.noGravity = true;
            NPC.noTileCollide = true;

            NPC.HitSound = SoundID.Tink;
            NPC.DeathSound = SoundID.NPCDeath8;

            NPC.friendly = false;
            NPC.value = Item.sellPrice(silver: 5);
            SpawnModBiomes = new int[1] { ModContent.GetInstance<VolcanicBiome>().Type };
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemID.StoneBlock, 1, 5, 12));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<VolcanicRemains>(), 1, 2, 3));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<LegendaryHilt>(), 50));
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.GetModPlayer<TranscendencePlayer>().ZoneVolcano && (!NPC.AnyNPCs(Type) || NPC.downedBoss3))
                return 0.25f;
            else return 0;
        }
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
        }
        public override void AI()
        {
            NPC.TargetClosest();
            Player player = Main.player[NPC.target];

            speed += 0.125f;
            if (speed > 60)
                speed = 0;

            float speed2 = (float)Math.Sin(speed) * 70;

            NPC.velocity = Vector2.Lerp(NPC.velocity, NPC.DirectionTo(player.Center) * (25 + (speed2 * 0.75f)), 0.007585f);
            NPC.rotation = NPC.velocity.ToRotation() + MathHelper.PiOver2;
            NPC npc = NPC;
            rot += 0.033f;

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
                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.TranscendenceMod.Messages.Bestiary.EarthernScourge")),
            });
            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new()
            {
                CustomTexturePath = "TranscendenceMod/Miscannellous/Assets/Bestiary/EarthernScourgeBestiary",
                PortraitScale = 0.66f,
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
        }
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            TranscendenceUtils.DrawEntity(NPC, Color.White, NPC.scale, Texture + "_Glow", NPC.rotation, NPC.Center + new Vector2(0, 4), null);
        }
        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            scale = 1.5f;
            return (int)NPC.ai[1] == (int)(NPC.ai[2] / 2);
        }
    }
    public class EarthernScourge_Body : BodySegment
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
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Poisoned] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Venom] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire3] = true;
        }
        public override void SetDefaults()
        {
            NPC.lifeMax = 5;
            NPC.damage = Main.hardMode ? 50 : NPC.downedBoss3 ? 35 : 30; 
            NPC.knockBackResist = 0;
            NPC.takenDamageMultiplier = 0.33f;

            NPC.width = 30;
            NPC.height = 30;

            NPC.aiStyle = -1;
            NPC.noGravity = true;
            NPC.noTileCollide = true;

            NPC.HitSound = SoundID.Tink;
            NPC.DeathSound = SoundID.NPCDeath8;

            NPC.friendly = false;
            NPC.value = Item.buyPrice(silver: 25);
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            TranscendenceUtils.DrawEntity(NPC, drawColor, NPC.scale, Texture, NPC.rotation, NPC.Center, null);
            TranscendenceUtils.DrawEntity(NPC, Color.White, NPC.scale, Texture + "_Glow", NPC.rotation, NPC.Center, null);

            return false;
        }
        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position) => false;
    }
    public class EarthernScourge_Tail : TailSegment
    {
        public override float DistanceBetweenSegments => 0.75f;
        public override void SetStaticDefaults()
        {
            TranscendenceUtils.BeGoneBestiary(NPC);

            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Shimmer] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Poisoned] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Venom] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire3] = true;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            TranscendenceUtils.DrawEntity(NPC, Color.White, NPC.scale, Texture + "_Glow", NPC.rotation, NPC.Center + new Vector2(2), null);
        }
        public override void AI()
        {
            BodyAI(NPC);
        }
        public override void SetDefaults()
        {
            NPC.lifeMax = 5;
            NPC.damage = Main.hardMode ? 40 : NPC.downedBoss3 ? 25 : 30;
            NPC.knockBackResist = 0;

            NPC.width = 30;
            NPC.height = 48;

            NPC.aiStyle = -1;
            NPC.noGravity = true;
            NPC.noTileCollide = true;

            NPC.HitSound = SoundID.Tink;
            NPC.DeathSound = SoundID.NPCDeath8;

            NPC.friendly = false;
            NPC.value = Item.buyPrice(silver: 25);
        }
        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position) => false;
    }
}

