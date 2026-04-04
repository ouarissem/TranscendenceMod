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
using TranscendenceMod.Dusts;
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Items.Materials.MobDrops;
using TranscendenceMod.Miscannellous.Biomes;
using TranscendenceMod.Projectiles.NPCs;

namespace TranscendenceMod.NPCs.SpaceBiome
{
    public class CosmicGazer : SpaceBiomeNPC
    {
        public int AttackDelay;

        public override void SetStaticDefaults()
        {
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
            NPC.lifeMax = 1255;
            NPC.defense = 25;
            NPC.damage = 85;
            NPC.knockBackResist = 0;

            NPC.width = 15;
            NPC.height = 40;

            NPC.aiStyle = -1;
            NPC.noGravity = true;
            NPC.noTileCollide = true;

            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath22;

            NPC.friendly = false;
            NPC.value = Item.buyPrice(silver: 75);
            SpawnModBiomes = new int[1] { ModContent.GetInstance<Heaven>().Type };
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PulverizedPlanet>(), 1, 3, 7));
            npcLoot.Add(ItemDropRule.Common(ItemID.FallenStar, 1, 2, 6));

            npcLoot.Add(ItemDropRule.Common(ItemID.FragmentSolar, 1, 2, 3));
            npcLoot.Add(ItemDropRule.Common(ItemID.FragmentNebula, 1, 1, 4));
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.GetModPlayer<TranscendencePlayer>().ZoneStar && !NPC.AnyNPCs(Type) && NPC.downedMoonlord)
                return 0.6f;
            else return 0;
        }
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.damage = (int)(NPC.damage * 0.55f);
        }
        public override void AI()
        {
            NPC.TargetClosest();
            Player player = Main.player[NPC.target];

            if (NPC.Distance(player.Center) > 275 && AttackDelay > 89 || NPC.Distance(player.Center) > 500)
                NPC.velocity = Vector2.Lerp(NPC.velocity, NPC.DirectionTo(player.Center) * 15f, 0.05f);

            if (NPC.Distance(player.Center) < 500)
            {
                AttackDelay++;
            }
            else
                return;

            if (AttackDelay < 61)
            {
                if (AttackDelay % 15 == 0)
                {
                    Vector2 target = player.Center - player.velocity * 6f;

                    for (int i = 0; i < 6; i++)
                    {
                        for (int j = 0; j < 5; j++)
                        {
                            Vector2 vec = Vector2.One.RotatedBy(MathHelper.TwoPi * j / 5f + MathHelper.ToRadians(i * 24f)) * (float)(25f + i * 2.5f);
                            float rot = NPC.DirectionTo(target).ToRotation() - MathHelper.PiOver2;

                            Vector2 pos = NPC.Center + new Vector2(vec.X, vec.Y / 2.5f).RotatedBy(rot) + Vector2.One.RotatedBy(rot + MathHelper.PiOver4) * 25f;
                            Dust d = Dust.NewDustPerfect(pos, ModContent.DustType<SpaceCrystalDust>(), Vector2.Zero, 0, Color.White, 2f);
                            d.noGravity = true;
                        }
                    }

                    SoundEngine.PlaySound(SoundID.Item158, NPC.Center);
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.DirectionTo(target), ModContent.ProjectileType<StarsighterLaser>(), 85, 0f, -1, 0, NPC.whoAmI);
                    NPC.velocity = NPC.DirectionTo(player.Center) * -4f;
                }
            }
            if (AttackDelay > 75)
            {
                NPC.velocity *= 0.95f;
                if (AttackDelay > 150)
                    AttackDelay = 0;
            }

        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.TranscendenceMod.Messages.Bestiary.Stargazer")),
            });
        }
        public override Color? GetAlpha(Color drawColor) => Color.White;
        public override bool? CanFallThroughPlatforms() => true;
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            return base.PreDraw(spriteBatch, screenPos, drawColor);
        }
    }
}

