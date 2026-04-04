using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Items.Materials.MobDrops;
using TranscendenceMod.Miscannellous.Biomes;
using TranscendenceMod.Projectiles.NPCs;

namespace TranscendenceMod.NPCs.SpaceBiome
{
    public class LimboEyes : SpaceBiomeNPC
    {
        public int AttackDelay;
        public float ChaseSpeed = 10;
        public Vector2 desiredPos;
        Player player;
        public override void SetStaticDefaults()
        {
            NPCID.Sets.ImmuneToRegularBuffs[Type] = true;
        }

        public override void SetDefaults()
        {
            NPC.lifeMax = 1755;
            NPC.defense = 20;
            NPC.damage = 85;
            NPC.knockBackResist = 0f;

            NPC.width = 74;
            NPC.height = 58;
            NPC.noGravity = true;
            NPC.noTileCollide = true;

            NPC.HitSound = SoundID.NPCHit19;
            NPC.DeathSound = SoundID.NPCDeath22;

            NPC.friendly = false;
            NPC.value = Item.buyPrice(silver: 50);
            SpawnModBiomes = new int[1] { ModContent.GetInstance<Heaven>().Type };
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<VoidFragment>(), 1, 3, 5));
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.GetModPlayer<TranscendencePlayer>().ZoneStar && NPC.CountNPCS(Type) < 2 && NPC.downedMoonlord)
                return 1f;
            else return 0;
        }
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)    
        {
            NPC.damage = (int)(NPC.damage * 0.525f);
        }
        public override void AI()
        {
            NPC.TargetClosest();
            player = Main.player[NPC.target];

            if (NPC.Distance(player.Top) > 50)
                AttackDelay++;
            else
            {
                NPC.velocity = Vector2.Zero;
                NPC.Center = player.Top;
                player.AddBuff(BuffID.Obstructed, 2);

                return;
            }

            if (AttackDelay > 60)
            {
                NPC.velocity *= 0.9f;
                if (AttackDelay % 20 == 0)
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.DirectionTo(player.Center) * 7.5f, ModContent.ProjectileType<TwistedTendril>(), 80, 0);
            }
            else NPC.velocity = Vector2.Lerp(NPC.velocity, NPC.DirectionTo(player.Top) * 18f, 1f / 20f);

            if (AttackDelay > 150)
            {
                NPC.velocity.X = 0;
                AttackDelay = 0;
            }
        }

        public int P3FlyTimer;
        public int P3FlyTimer2;

        public void Phase3Flight()
        {
            if (P3FlyTimer > 0)
                P3FlyTimer--;
            if (P3FlyTimer2 > 0)
                P3FlyTimer2--;

            if (NPC.Distance(player.Center) > 575 || P3FlyTimer > 0)
            {
                Vector2 vel = NPC.DirectionTo(player.Center);

                NPC.velocity.X = MathHelper.Lerp(NPC.velocity.X, vel.X * 45f, 1f / 20f);
                NPC.velocity.Y = MathHelper.Lerp(NPC.velocity.Y, vel.Y * 15f, 1f / 10f);

                if (NPC.Distance(player.Center) > 500)
                    P3FlyTimer = 25;

            }
            else if (NPC.Distance(player.Center) < 500 || P3FlyTimer2 > 0)
            {
                Vector2 vel = -NPC.DirectionTo(player.Center);

                NPC.velocity.X = MathHelper.Lerp(NPC.velocity.X, vel.X * 45f, 1f / 20f);
                NPC.velocity.Y = MathHelper.Lerp(NPC.velocity.Y, vel.Y * 15f, 1f / 10f);


                if (NPC.Distance(player.Center) < 500)
                    P3FlyTimer2 = 25;
            }
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.TranscendenceMod.Messages.Bestiary.LimboEyes")),
            });
        }
    }
}

