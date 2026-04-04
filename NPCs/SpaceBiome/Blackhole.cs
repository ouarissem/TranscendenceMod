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
using TranscendenceMod.Items.Modifiers;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Miscannellous.Biomes;
using TranscendenceMod.Projectiles.NPCs.Bosses.SpaceBoss;

namespace TranscendenceMod.NPCs.SpaceBiome
{
    public class Blackhole : SpaceBiomeNPC
    {
        public int AttackDelay;
        public bool Activated;
        public int Proj = ModContent.ProjectileType<DashBallProj>();
        public int SuckTimer;
        public int SuckCD;
        Player player;
        public override string Texture => "TranscendenceMod/Miscannellous/Assets/InvisSprite";
        public override void SetStaticDefaults()
        {
            NPCID.Sets.ImmuneToRegularBuffs[Type] = true;
        }

        public override void SetDefaults()
        {
            NPC.lifeMax = 2250;
            NPC.damage = 115;
            NPC.knockBackResist = 0.25f;

            NPC.width = 64;
            NPC.height = 64;
            NPC.noGravity = true;
            NPC.noTileCollide = true;

            NPC.HitSound = SoundID.NPCHit52;
            NPC.DeathSound = SoundID.NPCDeath52;

            NPC.friendly = false;
            NPC.value = Item.buyPrice(silver: 75);
            SpawnModBiomes = SpawnModBiomes = new int[1] { ModContent.GetInstance<Heaven>().Type };
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<VoidFragment>(), 1, 5, 8));
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (NPC.downedMoonlord && spawnInfo.Player.GetModPlayer<TranscendencePlayer>().ZoneStar)
                return 0.4f;
            else return 0;
        }
        public override void AI()
        {
            NPC.TargetClosest();
            AttackDelay++;

            NPC.velocity *= 0.9f;

            for (int i = 0; i < Main.maxItems; i++)
            {
                if (i < Main.maxNPCs)
                {
                    NPC n = Main.npc[i];
                    if (n != null && n.active && n.type != Type && n.lifeMax < 50000 && n.Distance(NPC.Center) < 500)
                    {
                        n.velocity += n.DirectionTo(NPC.Center) * 0.66f;
                        if (n.Distance(NPC.Center) < 75)
                            n.StrikeInstantKill();
                    }
                }

                if (i < Main.maxItems)
                {
                    Item it = Main.item[i];
                    if (it != null && it.active && it.Distance(NPC.Center) < 500)
                    {
                        it.velocity += it.DirectionTo(NPC.Center);
                    }
                }

                if (i < Main.maxPlayers)
                {
                    Player player = Main.player[i];
                    if (player != null && player.active)
                    {
                        if (player.Distance(NPC.Center) < 500 && player.Distance(NPC.Center) > 25)
                        {
                            SoundEngine.PlaySound(SoundID.Item117 with { MaxInstances = 0, Volume = 0.15f, Pitch = -1f }, NPC.Center);
                            player.velocity += player.DirectionTo(NPC.Center) * (3.66f - (player.Distance(NPC.Center) * 0.0025f)) * 0.5f;
                        }

                        if (player.Distance(NPC.Center) < (50 * NPC.scale))
                        {
                            player.velocity += player.DirectionTo(NPC.Center) * 2.5f;
                            player.AddBuff(ModContent.BuffType<BlackHoleDebuff>(), 5);
                        }
                    }
                }
            }

        }
        public override bool CanHitPlayer(Player target, ref int cooldownSlot) => false;
        public override bool PreKill()
        {
            TranscendenceUtils.DustRing(NPC.Center, 30, ModContent.DustType<VoidDust>(), Color.White, 2f, 3f, 4f);
            return true;
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.TranscendenceMod.Messages.Bestiary.Blackhole")),
            });
            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new()
            {
                CustomTexturePath = "TranscendenceMod/Miscannellous/Assets/Bestiary/Blackhole",
                PortraitScale = 0.5f,
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            BlackholeDrawer.DrawBlackhole(NPC, 1.5f * NPC.scale, spriteBatch);
            return false;
        }
    }
}

