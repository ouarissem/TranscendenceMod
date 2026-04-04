using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Dusts;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Projectiles;
using TranscendenceMod.Projectiles.NPCs.Bosses.Nucleus;

namespace TranscendenceMod.NPCs.Boss.Nucleus
{
    public class HealHeart : ModNPC
    {
        public override void SetStaticDefaults() { TranscendenceUtils.BeGoneBestiary(NPC); }

        public override void SetDefaults()
        {
            Main.npcFrameCount[Type] = 4;

            NPC.lifeMax = 7250;
            NPC.defense = 35;
            NPC.knockBackResist = 0.5f;

            NPC.noGravity = true;
            NPC.aiStyle = -1;
            NPC.scale = 2f;

            NPC.width = 20;
            NPC.height = 24;

            NPC.HitSound = SoundID.NPCHit9;
            NPC.DeathSound = SoundID.NPCDeath12 with { MaxInstances = 0 };

            NPC.value = Item.buyPrice(silver: 50);
        }
        public NPC npc;
        public override void OnSpawn(IEntitySource source)
        {
            base.OnSpawn(source);

            npc = Main.npc[(int)NPC.ai[1]];
        }
        public override void AI()
        {
            if (npc == null || !npc.active)
                NPC.active = false;

            NPC.velocity *= 0.975f;

            bool InsideLiquid = false;

            for (int p = 0; p < Main.maxProjectiles; p++)
            {
                Projectile projectile = Main.projectile[p];

                if (projectile != null && projectile.active && projectile.ModProjectile is BloodLiquid liquid && liquid.InsideLiquid(NPC))
                    InsideLiquid = true;
            }

            if (NPC.ai[3] > 30 && NPC.ai[3] % 45 == 0)
            {
                if (NPC.Distance(npc.Center) < 250)
                    NPC.velocity = NPC.DirectionTo(npc.Center) * -8f;
                if (NPC.Distance(npc.Center) < 425)
                    NPC.velocity = Main.rand.NextVector2CircularEdge(15f, 15f);
                else NPC.velocity = NPC.DirectionTo(npc.Center) * 8f;

                if (NPC.Center.Y > (npc.Center.Y + 500) || NPC.Center.Y < (npc.Center.Y - 500))
                    NPC.position.Y = npc.position.Y;
            }

            if (npc.life < (npc.lifeMax * 0.1f))
                NPC.active = false;

            if (++NPC.ai[3] >= 1200)
            {
                int am = (int)(npc.lifeMax / 10f);

                if (npc.life > (npc.lifeMax - am))
                    npc.life = npc.lifeMax;
                else npc.life += am;

                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<Shockwave>(), 1000, 20f, -1, 0f, 1f, 0f);
                SoundEngine.PlaySound(NPC.DeathSound, NPC.Center);
                for (int i = 0; i < 24; i++)
                {
                    Dust.NewDustPerfect(NPC.Center, ModContent.DustType<BetterBlood>(), Main.rand.NextVector2Circular(14f, 8f), 0, default, 1.5f);
                }

                npc.HealEffect(am);
                NPC.active = false;
            }
        }
        private Texture2D sprite;
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            var eff = ModContent.Request<Effect>("TranscendenceMod/Miscannellous/Assets/Shaders/Effects/SeraphOutlineShader", AssetRequestMode.ImmediateLoad).Value;

            eff.Parameters["uOpacity"].SetValue(1f);
            eff.Parameters["uSaturation"].SetValue(NPC.ai[3] >= 1020 ? 0.5f + (float)Math.Sin(Main.GlobalTimeWrappedHourly * 16f) * 0.5f : 0f);

            eff.Parameters["uRotation"].SetValue(0f);
            eff.Parameters["uTime"].SetValue(0.75f);
            eff.Parameters["uDirection"].SetValue(0f);

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, eff, Main.GameViewMatrix.TransformationMatrix);

            if (NPC.active && npc != null && npc.active)
            {
                sprite = ModContent.Request<Texture2D>($"{Texture}_Line").Value;


                spriteBatch.Draw(sprite, new Rectangle(
                    (int)(NPC.Center.X - Main.screenPosition.X), (int)(NPC.Center.Y - Main.screenPosition.Y), 24,
                    (int)(NPC.Distance(npc.Center) * 2)), null,
                    Color.White, NPC.DirectionTo(npc.Center).ToRotation() + MathHelper.PiOver2, sprite.Size() * 0.5f, SpriteEffects.None, 0);
            }

            TranscendenceUtils.DrawEntity(NPC, Color.White, NPC.scale, ModContent.Request<Texture2D>(Texture).Value, NPC.rotation, NPC.Center, NPC.frame, NPC.frame.Size() * 0.5f, SpriteEffects.None);
            
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            return false;
        }
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * (Main.masterMode ? 0.525f : 0.55f));
        }
        public override void FindFrame(int frameHeight)
        {
            if (NPC.frame.Y < (frameHeight * 4))
            {
                if (++NPC.frameCounter > 5)
                {
                    NPC.frame.Y += frameHeight;
                    NPC.frameCounter = 0;
                }
            }
            else NPC.frame.Y = 0;
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemID.Heart, 1, 2, 4));
        }
    }
}

