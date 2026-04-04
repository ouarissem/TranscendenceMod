using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TranscendenceMod.Buffs;
using TranscendenceMod.Miscannellous.Biomes;
using TranscendenceMod.Projectiles.NPCs.Bosses.SpaceBoss;

namespace TranscendenceMod.NPCs.SpaceBiome
{
    public class Scorpio : SpaceBiomeNPC
    {
        public int AttackDelay;
        public float ChaseSpeed = 10;
        public Vector2 desiredPos;
        Player player;
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 3;
            NPCID.Sets.ImmuneToRegularBuffs[Type] = true;
        }

        public override void SetDefaults()
        {
            NPC.lifeMax = 1550;
            NPC.damage = 85;
            NPC.knockBackResist = 0f;

            NPC.width = 56;
            NPC.height = 56;
            NPC.noGravity = true;
            NPC.noTileCollide = true;

            NPC.HitSound = SoundID.Item9;
            NPC.DeathSound = SoundID.Item4;

            NPC.friendly = false;
            NPC.value = Item.buyPrice(silver: 45);
            SpawnModBiomes = new int[1] { ModContent.GetInstance<Heaven>().Type };
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.ByCondition(new MoonlordDropRule(), ItemID.FragmentStardust, 1, 2, 4));
            npcLoot.Add(ItemDropRule.Common(ItemID.FallenStar, 3, 1, 4));
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (NPC.downedMoonlord && spawnInfo.Player.GetModPlayer<TranscendencePlayer>().ZoneStar)
                return 0.8f;
            else return 0;
        }
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)    
        {
            NPC.damage = (int)(NPC.damage * 0.6f);
        }
        public override void AI()
        {
            NPC.TargetClosest();
            player = Main.player[NPC.target];

            NPC.rotation = NPC.velocity.ToRotation() + MathHelper.PiOver2;

            AttackDelay++;

            if (AttackDelay < 70)
            {
                Vector2 pos = player.Center + Vector2.One.RotatedBy(MathHelper.ToRadians(++NPC.ai[2])) * 150;
                if (NPC.Distance(pos) > 100)
                    NPC.velocity = NPC.DirectionTo(pos) * ChaseSpeed;
            }
            else
            {
                NPC.velocity *= 0.9f;
                if (AttackDelay > 105)
                {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.DirectionTo(player.Center) * 5, ModContent.ProjectileType<CelestialStar>(), 80, 2, -1, 0, 0, 0.5f);
                    AttackDelay = 0;
                }
            }

        }
        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            if (Main.rand.NextBool(2))
                target.AddBuff(ModContent.BuffType<SpaceDebuff>(), 240);
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.TranscendenceMod.Messages.Bestiary.Scorpio")),
            });
        }
        public override Color? GetAlpha(Color drawColor) => Color.White;
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
    }
}

