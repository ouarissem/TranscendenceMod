using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.NPCs.PreHard
{
    public class StormEel : ModNPC
    {
        public int AttackDelay;
        public int ChargeTimer = 0;
        public bool Charged;
        public float ChargeAlpha;
        public override void SetStaticDefaults() { }

        public override void SetDefaults()
        {
            NPC.lifeMax = 145;
            NPC.defense = 5;
            NPC.damage = 15;
            NPC.knockBackResist = 0.25f;

            NPC.width = 18;
            NPC.height = 18;
            NPC.noGravity = true;
            NPC.noTileCollide = false;

            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;

            NPC.friendly = false;
            NPC.value = Item.buyPrice(0, 0, 10);
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.ByCondition(Condition.DownedMoonLord.ToDropCondition(ShowItemDropInUI.WhenConditionSatisfied),
                ModContent.ItemType<Lightning>(), 3, 1, 2));
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return SpawnCondition.OverworldDayRain.Chance * 0.7f;
        }
        public override Color? GetAlpha(Color drawColor) => Color.White;
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D sprite = ModContent.Request<Texture2D>(Texture).Value;
            Rectangle rec = NPC.frame;
            Vector2 pos = NPC.Center - screenPos;
            Vector2 origin = rec.Size() * 0.5f;

            SpriteBatch sb = Main.spriteBatch;
            if (Charged)
            {
                TranscendenceUtils.DrawEntity(NPC, new Color(0f, 0.5f, 0.75f, 0f) * ChargeAlpha, 3f, "TranscendenceMod/Miscannellous/Assets/GlowBloom", 0, NPC.Center, null);
            }

            spriteBatch.Draw(sprite, pos, rec, Color.White, NPC.rotation, origin, NPC.scale,
                NPC.direction == 1 ? SpriteEffects.FlipVertically : SpriteEffects.None, 0);
            return false;
        }
        public override void AI()
        {
            NPC.TargetClosest();
            Player player = Main.player[NPC.target];
            NPC.direction = player.Center.X > NPC.Center.X ? 1 : -1;

            if (Charged && ChargeAlpha < 1 && ChargeTimer > 90) ChargeAlpha += 0.025f;
            if (!Charged) ChargeAlpha = 0;
            if (ChargeTimer > 0) ChargeTimer--;
            if (ChargeTimer < 90)
            {
                ChargeAlpha -= 1f / 30f;
                if (ChargeTimer < 60)
                    Charged = false;
            }
            if (player.Distance(NPC.Center) < 300 && ChargeTimer == 0)
            {
                ChargeTimer = 240;
                Charged = true;
            }

            if (++AttackDelay > 60)
            {
                NPC.velocity = NPC.DirectionTo(player.Center) * 15;
                AttackDelay = 0;
            }

            NPC.velocity *= 0.95f;
            NPC.rotation = NPC.DirectionTo(player.Center).ToRotation() + MathHelper.Pi;

            if (player.Distance(NPC.Center) < 150 && Charged && ChargeAlpha > 0.75f && !player.dead && player.active)
            {
                player.Hurt(PlayerDeathReason.ByNPC(NPC.whoAmI), NPC.damage, NPC.direction * 3);
                player.velocity = player.DirectionTo(NPC.Center) * -30f;
                player.AddBuff(BuffID.Electrified, 60);
                for (int i = 0; i < 30; i++)
                {
                    SoundEngine.PlaySound(SoundID.Item93 with { MaxInstances = 0 }, player.Center);
                }

                ChargeTimer = 240;
                Charged = false;
            }
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(BuffID.Electrified, 180);
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns,
                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.TranscendenceMod.Messages.Bestiary.StormEel")),
            });
        }
        public override bool? CanFallThroughPlatforms() => true;
    }
}

