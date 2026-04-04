using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TranscendenceMod.Items.Materials.MobDrops;
using TranscendenceMod.Items.Modifiers.Upgrades;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Miscannellous.Biomes;

namespace TranscendenceMod.NPCs.PreHard
{
    public class VolcanicSlime : ModNPC
    {
        public int AttackTimer;
        public bool TouchedGround;
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 2;
            NPCID.Sets.TrailCacheLength[Type] = 20;
            NPCID.Sets.TrailingMode[Type] = 1;
        }

        public override void SetDefaults()
        {
            NPC.lifeMax = Main.hardMode ? 220 : 55;
            NPC.defense = 10;
            NPC.damage = Main.hardMode ? 60 : 20;
            NPC.knockBackResist = 0.25f;

            NPC.width = 40;
            NPC.height = 32;
            NPC.noGravity = true;
            NPC.noTileCollide = false;
            NPC.lavaImmune = true;
            NPC.buffImmune[BuffID.OnFire] = true;
            NPC.buffImmune[BuffID.OnFire3] = true;

            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;

            NPC.friendly = false;
            NPC.value = Item.buyPrice(silver: 10);
            SpawnModBiomes = new int[1] { ModContent.GetInstance<VolcanicBiome>().Type };
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemID.Gel, 1, 2, 4));
            npcLoot.Add(ItemDropRule.Common(ItemID.AshBlock, 2, 1, 3));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<VolcanicRemains>(), 4, 1, 2));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<LegendaryHilt>(), 50));
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.GetModPlayer<TranscendencePlayer>().ZoneVolcano)
                return 1f;
            else return 0;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            TranscendenceUtils.DrawEntity(NPC, Color.White, NPC.scale, Texture + "_Glow", NPC.rotation, NPC.Center + new Vector2(0, 20), NPC.frame);
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            TranscendenceUtils.DrawTrailNPC(NPC, Color.White, 1f, Texture, false, true, 1f, new Vector2(0, 4));
            return base.PreDraw(spriteBatch, screenPos, drawColor);
        }
        public override void AI()
        {
            NPC.TargetClosest();
            Player player = Main.player[NPC.target];
            NPC.direction = player.Center.X > NPC.Center.X ? 1 : -1;

            NPC.velocity.Y += 0.2f;

            if (AttackTimer < 91 && Collision.SolidCollision(NPC.Left, NPC.width, NPC.height))
            {
                if (!TouchedGround)
                {
                    int p = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, new Vector2(0, -5), ProjectileID.GeyserTrap, (int)(NPC.damage * ((Main.masterMode || Main.expertMode) ? 0.5f : 1)), 2);
                    Main.projectile[p].friendly = false;
                    TouchedGround = true;
                }

                NPC.ai[2] = NPC.direction;
                NPC.velocity.X *= 0.9f;
                AttackTimer++;
            }

            if (AttackTimer > 90)
            {
                AttackTimer++;
                TouchedGround = false;

                NPC.velocity.X = 7.5f * NPC.ai[2];
                if (AttackTimer < 100)
                    NPC.velocity.Y = -7.5f;
                if (AttackTimer > 120)
                    AttackTimer = 0;
            }
        }
        public override void FindFrame(int frameHeight)
        {
            if (!Collision.SolidCollision(NPC.Left, NPC.width, NPC.height))
            {
                NPC.frame.Y = 32;
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
            target.AddBuff(BuffID.OnFire3, 60);
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.TranscendenceMod.Messages.Bestiary.VolcanicSlime")),
            });
        }
        public override bool? CanFallThroughPlatforms() => true;
    }
}

