using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Buffs;
using TranscendenceMod.Dusts;
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Items.Materials.MobDrops;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Miscannellous.Biomes;
using TranscendenceMod.Projectiles.NPCs.Bosses.SpaceBoss;

namespace TranscendenceMod.NPCs.SpaceBiome
{
    public class NebulaMatter : SpaceBiomeNPC
    {
        public int AttackDelay;
        public int dashCount;
        public Vector2 desiredPos;
        public int dashProj = ModContent.ProjectileType<DashBallProj2>();
        Player player;
        public override string Texture => "TranscendenceMod/Miscannellous/Assets/GlowBloomNoBG";

        public override void SetStaticDefaults()
        {
            NPCID.Sets.ImmuneToRegularBuffs[Type] = true;
        }

        public override void SetDefaults()
        {
            NPC.lifeMax = NPC.downedMoonlord ? 1250 : 90;
            NPC.damage = NPC.downedMoonlord ? 115 : 45;
            NPC.knockBackResist = 0f;

            NPC.width = 64;
            NPC.height = 64;
            NPC.noGravity = true;
            NPC.noTileCollide = true;

            NPC.HitSound = SoundID.NPCHit52;
            NPC.DeathSound = SoundID.NPCDeath52;

            NPC.friendly = false;
            NPC.value = Item.buyPrice(silver: NPC.downedMoonlord ? 30 : 4);
            SpawnModBiomes = new int[1] { ModContent.GetInstance<Heaven>().Type };
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PulverizedPlanet>(), 2, 1, 3));
            npcLoot.Add(ItemDropRule.ByCondition(new MoonlordDropRule(), ItemID.FragmentNebula, 1, 1, 3));
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.GetModPlayer<TranscendencePlayer>().ZoneStar)
                return 0.75f;
            else return 0;
        }
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.damage = (int)(NPC.damage * 0.525f);
        }
        public override void AI()
        {
            NPC.TargetClosest();
            player = Main.player[NPC.target];
            int dashCD = 50;
            AttackDelay++;

            if (NPC.velocity.Length() > 1) Dust.NewDust(NPC.Center, NPC.width, NPC.height, ModContent.DustType<ArenaDust>(), -NPC.velocity.X,
                -NPC.velocity.Y, 0, Color.Magenta, Main.rand.NextFloat(0.65f, 1.1f));

            if (player == null || player.dead || !player.active)
            {
                NPC.velocity = new Vector2(0, -10);
                NPC.EncourageDespawn(10);
                return;
            }

            if (AttackDelay < dashCD)
                desiredPos = NPC.DirectionTo(player.Center) * (NPC.downedMoonlord ? 55f : 35f);


            if (AttackDelay > dashCD)
            {
                if (AttackDelay > (dashCD + 5)) NPC.velocity *= 0.7f;
                else NPC.velocity = desiredPos;

                if (AttackDelay > (dashCD + 17)) AttackDelay = 0;
                if (AttackDelay == (dashCD + 2))
                {
                    if (++dashCount > 2)
                    {
                        if (NPC.downedMoonlord)
                        {
                            TranscendenceUtils.ProjectileRing(NPC, 8, NPC.GetSource_FromAI(), NPC.Center, dashProj, 70,
                                0, 1.25f, 0, NPC.whoAmI, 1.25f, -1, 0);
                        }
                        else
                        {
                            TranscendenceUtils.ProjectileRing(NPC, 5, NPC.GetSource_FromAI(), NPC.Center, dashProj, 70,
                                0, 0.75f, 0, NPC.whoAmI, 0.35f, -1, MathHelper.PiOver2);
                        }
                    }
                    if (dashCount == 2)
                    {
                        for (int i = 0; i < 55; i++)
                        {
                            Vector2 vel = new Vector2(0, NPC.downedMoonlord ? 15f : 7.5f).RotatedBy(MathHelper.TwoPi * i / 55);
                            vel = new Vector2(vel.X, vel.Y / 2f).RotatedBy(desiredPos.ToRotation() - MathHelper.PiOver2);
                            Dust.NewDustPerfect(NPC.Center, ModContent.DustType<ArenaDust>(), vel, 0, Color.Magenta, 1.5f);
                        }
                    }
                    if (dashCount > 2)
                        dashCount = 0;
                }
            }

        }
        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            if (Main.rand.NextBool(4))
                target.AddBuff(ModContent.BuffType<SpaceDebuff>(), 180);
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                new FlavorTextBestiaryInfoElement("A sentient nebula, one of the Seraph's first creations."),
            });
            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new()
            {
                CustomTexturePath = $"Terraria/Images/Item_3457",
                PortraitScale = 1.25f,
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
        }
        public override bool PreDraw(SpriteBatch sb, Vector2 screenPos, Color drawColor)
        {
            //Request the Effect
            var eff = ModContent.Request<Effect>("TranscendenceMod/Miscannellous/Assets/Shaders/Effects/MovingNoiseTrans", AssetRequestMode.ImmediateLoad).Value;
            //Apply Shader Texture
            Texture2D shaderImage = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/SeraphForcefieldShader").Value;
            Main.instance.GraphicsDevice.Textures[1] = shaderImage;

            eff.Parameters["uImageSize0"].SetValue(shaderImage.Size());
            eff.Parameters["uImageSize1"].SetValue(shaderImage.Size() * 3f);
            eff.Parameters["uTime"].SetValue(0);
            eff.Parameters["yChange"].SetValue(Main.GlobalTimeWrappedHourly / 2f);
            eff.Parameters["useAlpha"].SetValue(false);
            eff.Parameters["useExtraCol"].SetValue(true);
            eff.Parameters["extraCol"].SetValue(new Vector3(Color.Magenta.R / 255f, 0f, Color.Magenta.B / 255f));

            sb.End();
            sb.Begin(default, BlendState.Additive, Main.DefaultSamplerState, default, default, eff, Main.GameViewMatrix.TransformationMatrix);

            for (int i = 0; i < 2; i++)
                TranscendenceUtils.DrawEntity(NPC, Color.White, 1f, Texture, 0, NPC.Center, null);

            sb.End();
            sb.Begin(default, BlendState.Additive, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            if (player != null)
                drawCoolLine(NPC.Center, desiredPos.ToRotation() + MathHelper.PiOver2);


            TranscendenceUtils.RestartSB(sb, BlendState.AlphaBlend, null);

            void drawCoolLine(Vector2 pos, float dir)
            {
                Texture2D sprite = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/BloomLine").Value;
                Vector2 origin = new Vector2(sprite.Width * 0.5f, sprite.Height * 0.5f);

                for (int i = 0; i <  5; i++)
                    Main.EntitySpriteDraw(sprite, pos - Main.screenPosition + new Vector2(0, NPC.gfxOffY), null, Color.HotPink, dir, origin, 1f, SpriteEffects.None);
            }

            return false;
        }
    }
}

