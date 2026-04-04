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
using TranscendenceMod.Miscannellous.Biomes;
using TranscendenceMod.Projectiles.NPCs.Bosses.SpaceBoss;

namespace TranscendenceMod.NPCs.SpaceBiome
{
    public class Starsighter : SpaceBiomeNPC
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
            NPC.lifeMax = NPC.downedMoonlord ? 1005 : 65;
            NPC.defense = 10;
            NPC.damage = 85;
            NPC.knockBackResist = 0;

            NPC.width = 15;
            NPC.height = 40;

            NPC.aiStyle = -1;
            NPC.noGravity = true;
            NPC.noTileCollide = true;

            NPC.HitSound = SoundID.NPCHit18;
            NPC.DeathSound = SoundID.NPCDeath22;

            NPC.friendly = false;
            NPC.value = Item.buyPrice(silver: NPC.downedMoonlord ? 30 : 5);
            SpawnModBiomes = new int[1] { ModContent.GetInstance<Heaven>().Type };
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PulverizedPlanet>(), 1, 3, 7));

            npcLoot.Add(ItemDropRule.ByCondition(new MoonlordDropRule(), ItemID.FragmentSolar, 1, 2, 3));
            npcLoot.Add(ItemDropRule.ByCondition(new MoonlordDropRule(), ItemID.FragmentNebula, 1, 1, 4));
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.GetModPlayer<TranscendencePlayer>().ZoneStar && !NPC.AnyNPCs(Type))
                return 0.6f;
            else return 0;
        }
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.damage = (int)(NPC.damage * 0.525f);
        }
        public override void AI()
        {
            NPC.TargetClosest();
            Player player = Main.player[NPC.target];

            NPC.rotation = NPC.velocity.X * 0.1f;

            float ChaseSpeed = 8;
            int projCD = 150;

            if (NPC.Distance(player.Center - new Vector2(0, 150)) > 375)
            {
                NPC.velocity = NPC.DirectionTo(player.Center - new Vector2(0, 150)) * ChaseSpeed;
                return;
            }
            else NPC.velocity *= 0.95f;

            if (++AttackDelay < 100 && AttackDelay % 20 == 0)
            {
                SoundEngine.PlaySound(SoundID.Item158, NPC.Center);

                for (int i = 0; i < 4; i++)
                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center - new Vector2(0, 12 * NPC.scale), NPC.DirectionTo(player.Center).RotatedByRandom(0.1f) * Main.rand.NextFloat(4f, 7f), ModContent.ProjectileType<StellarFireball>(), NPC.downedMoonlord ? 70 : 25, 0, -1, 0f, NPC.whoAmI, 0f);
            }
            if (AttackDelay > 120)
                AttackDelay = 0;
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.TranscendenceMod.Messages.Bestiary.Starsighter")),
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

