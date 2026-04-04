using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using TranscendenceMod.Items.Materials.MobDrops;
using TranscendenceMod.Projectiles.NPCs;

namespace TranscendenceMod.NPCs.PostML
{
    public class StormSquid : ModNPC
    {
        public int AttackDelay;
        public override void SetStaticDefaults() => Main.npcFrameCount[Type] = 4;

        public override void SetDefaults()
        {
            NPC.lifeMax = 4000;
            NPC.defense = 5;
            NPC.damage = 115;
            NPC.knockBackResist = 0;

            NPC.width = 15;
            NPC.height = 40;
            NPC.noGravity = true;
            NPC.noTileCollide = false;

            NPC.HitSound = SoundID.NPCHit25;
            NPC.DeathSound = SoundID.NPCDeath28;
            NPC.alpha = 55;

            NPC.friendly = false;
            NPC.value = Item.buyPrice(silver: 25);
            AnimationType = NPCID.FlyingSnake;
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Lightning>(), 2, 1, 2));
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (Main.IsItStorming && NPC.downedMoonlord && NPC.CountNPCS(Type) < 3) return SpawnCondition.OverworldDay.Chance * 0.4f;
            else return 0;
        }
        public override void AI()
        {
            NPC.TargetClosest();
            Player player = Main.player[NPC.target];

            bool NearDeath = NPC.life < (NPC.lifeMax * 0.33f);
            float ChaseSpeed = NearDeath ? 15 : 12;
            int projCD = NearDeath ? 60 : 120;

            NPC.velocity = Vector2.Lerp(NPC.velocity.RotatedByRandom(-0.2f), NPC.DirectionTo(player.Center + Main.rand.NextVector2Circular(125f, 125f)).RotatedByRandom(0.2f) * ChaseSpeed, 0.0125f);
            if (++AttackDelay > projCD)
            {
                SoundEngine.PlaySound(SoundID.Thunder, NPC.Center);
                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, new Vector2(0, 20),
                    ModContent.ProjectileType<SquidLightningBolt>(), 90, 0);

                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.DirectionTo(player.Center) * 5,
                    ModContent.ProjectileType<SquidLightningBolt>(), 80, 0);
                AttackDelay = 0;
            }
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.Rain,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.WindyDay,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
                new FlavorTextBestiaryInfoElement("That strange tingling sensation in the water may be the unwelcome surging of electrical death from a brainless jellyfish. Swim with care."),
            });
        }
        public override bool? CanFallThroughPlatforms() => true;
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D sprite = ModContent.Request<Texture2D>($"{Texture}_Glow").Value;
            Rectangle rec = NPC.frame;
            Vector2 pos = NPC.Center - screenPos;
            Vector2 origin = rec.Size() * 0.5f;

            spriteBatch.Draw(sprite, pos, rec, Color.White, NPC.rotation, origin, NPC.scale, SpriteEffects.None, 0);

            return base.PreDraw(spriteBatch, screenPos, drawColor);
        }
    }
}

