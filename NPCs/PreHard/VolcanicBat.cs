using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using TranscendenceMod.Items.Materials.MobDrops;
using TranscendenceMod.Items.Modifiers.Upgrades;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Miscannellous.Biomes;

namespace TranscendenceMod.NPCs.PreHard
{
    public class VolcanicBat : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 4;
            NPCID.Sets.TrailCacheLength[Type] = 20;
            NPCID.Sets.TrailingMode[Type] = 1;
        }


        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.GiantBat);

            AnimationType = NPCID.GiantBat;
            NPC.width = 38;
            NPC.height = 38;

            NPC.lifeMax = Main.hardMode ? 280 : 60;
            NPC.defense = 5;
            NPC.damage = Main.hardMode ? 85 : 30;
            NPC.knockBackResist = 0.5f;

            NPC.HitSound = SoundID.Tink;
            NPC.DeathSound = SoundID.NPCDeath8;

            NPC.value = Item.buyPrice(0, 0, silver: 10);
            SpawnModBiomes = new int[1] { ModContent.GetInstance<VolcanicBiome>().Type };

        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            SpriteEffects se = NPC.direction == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            TranscendenceUtils.DrawTrailNPC(NPC, Color.White, 1f, Texture, false, true, 1.5f, new Vector2(0, 4), se);
            return base.PreDraw(spriteBatch, screenPos, drawColor);
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemID.StoneBlock, 1, 3, 5));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<VolcanicRemains>(), 2, 1, 3));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<LegendaryHilt>(), 50));
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.GetModPlayer<TranscendencePlayer>().ZoneVolcano)
                return 0.5f;
            else return 0;
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.TranscendenceMod.Messages.Bestiary.VolcanicBat")),
            });
        }
    }
}

