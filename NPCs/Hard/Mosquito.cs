using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using TranscendenceMod.Items.Materials.MobDrops;

namespace TranscendenceMod.NPCs.Hard
{
    public class Mosquito : ModNPC
    {
        public int AttackDelay;
        public Vector2 Center;
        public override void SetStaticDefaults() => Main.npcFrameCount[Type] = 3;

        public override void SetDefaults()
        {
            NPC.lifeMax = 190;
            NPC.damage = 45;
            NPC.knockBackResist = 0f;

            NPC.width = 62;
            NPC.height = 42;
            NPC.noGravity = true;
            NPC.noTileCollide = false;

            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;

            NPC.friendly = false;
            NPC.value = Item.buyPrice(silver: 5);
        }
        public override void OnSpawn(IEntitySource source)
        {
            Center = NPC.Center;
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemID.Stinger, 2, 1, 2));
            npcLoot.Add(ItemDropRule.NormalvsExpert(ItemID.AdhesiveBandage, 100, 50));
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (!Main.hardMode)
                return 0f;
            return SpawnCondition.SurfaceJungle.Chance * 0.9f;
        }
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.damage = (int)(NPC.damage * 0.65f);
        }
        public override void AI()
        {
            NPC.TargetClosest();
            NPC.spriteDirection = NPC.direction;
            Player player = Main.player[NPC.target];

            switch (++AttackDelay)
            {
                case 0: break;
                case 120: Dash(); break;
                case 140:
                    {
                        NPC.velocity = Vector2.Zero;
                        AttackDelay = 0;
                        Center = NPC.Center;
                        goto case 0;
                    }
            }
            if (AttackDelay < 120 && AttackDelay > 30)
                NPC.velocity = NPC.DirectionTo(player.Center) * 5;

            if (AttackDelay < 90)
            {
                NPC.velocity = NPC.DirectionTo(Center + Vector2.One.RotatedByRandom(1) * Main.rand.NextFloat(-250, 250)) * 10f;
            }

            void Dash()
            {
                NPC.velocity = NPC.DirectionTo(player.Center) * 35;
                SoundEngine.PlaySound(SoundID.Item97, NPC.Center);
            }
        }
        public override void FindFrame(int frameHeight)
        {
            if (NPC.frame.Y != (frameHeight * 2))
            {
                if (++NPC.frameCounter > 4)
                {
                    NPC.frame.Y += frameHeight;
                    NPC.frameCounter = 0;
                }
            }
            else NPC.frame.Y = 0;
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            if (Main.rand.NextBool(5))
                target.AddBuff(BuffID.Rabies, 900);

            if (NPC.life > (NPC.lifeMax - (hurtInfo.Damage * 5)))
                return;

            target.AddBuff(BuffID.Bleeding, 180);
            NPC.life += hurtInfo.Damage * 7;
            NPC.HealEffect(hurtInfo.Damage);
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Jungle,
                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.TranscendenceMod.Messages.Bestiary.Mosquito")),
            });
        }
        public override bool? CanFallThroughPlatforms() => true;
    }
}

