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
    public class EmpyreanSlime : SpaceBiomeNPC
    {
        public bool TouchedGround;
        public float Fade;
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 3;
            NPCID.Sets.TrailCacheLength[Type] = 20;
            NPCID.Sets.TrailingMode[Type] = 1;
            NPCID.Sets.ImmuneToRegularBuffs[Type] = true;
        }

        public override void SetDefaults()
        {
            NPC.lifeMax = NPC.downedMoonlord ? 2505 : 380;
            NPC.damage = NPC.downedMoonlord ? 80 : 40;
            NPC.knockBackResist = NPC.downedMoonlord ? 0f : 0.25f;

            NPC.width = 48;
            NPC.height = 40;
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
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PulverizedPlanet>(), 2, 2, 4));
            npcLoot.Add(ItemDropRule.ByCondition(new MoonlordDropRule(), ItemID.FragmentNebula, 2, 2, 3));
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.GetModPlayer<TranscendencePlayer>().ZoneStar && Main.hardMode)
                return 2f;
            else return 0;
        }
        public override Color? GetAlpha(Color drawColor) => Color.White;
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D sprite = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/ExpandingTelegraph").Value;
            Vector2 pos = NPC.Center - Main.screenPosition;

            TranscendenceUtils.DrawTrailNPC(NPC, Color.Magenta, NPC.scale, Texture + "_Glow", false, true, 1.5f, new Vector2(0, 4));

            return base.PreDraw(spriteBatch, screenPos, drawColor);
        }
        public override void AI()
        {
            NPC.TargetClosest();
            Player player = Main.player[NPC.target];
            NPC.direction = player.Center.X > NPC.Center.X ? 1 : -1;

            NPC.rotation = NPC.velocity.X * 0.075f;
            NPC.velocity.Y += 0.25f;
            bool downedML = NPC.downedMoonlord;
            bool OnGround = Collision.SolidCollision(NPC.Left, NPC.width, NPC.height);

            if (NPC.ai[0] < 51 && OnGround)
            {
                NPC.ai[2] = NPC.direction;
                NPC.velocity.X *= 0.8f;
                NPC.ai[0]++;
            }

            if (NPC.ai[1] > 0)
                NPC.ai[1]--;

            if (NPC.ai[0] > 50)
            {
                TouchedGround = false;
                
                NPC.ai[0]++;
                if (NPC.ai[1] == 0)
                    NPC.velocity.X = (downedML ? 12.5f : 7.5f) * NPC.ai[2];
                if (NPC.ai[0] < 70)
                    NPC.velocity.Y = -7.5f;

                if (Main.rand.NextBool(4))
                {
                    int p = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + Main.rand.NextVector2Circular(25f, 50f), Vector2.Zero,
                        ModContent.ProjectileType<CosmicSphere>(), 80, 0f);
                    Main.projectile[p].timeLeft = NPC.downedMoonlord ? 90 : 30;
                }
            }
            if (NPC.ai[0] > 100)
                NPC.ai[0] = 0;
        }
        public override void FindFrame(int frameHeight)
        {
            if (!Collision.SolidCollision(NPC.Left, NPC.width, NPC.height))
            {
                NPC.frame.Y = 40;
                return;
            }

            if (NPC.frame.Y < 80)
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

