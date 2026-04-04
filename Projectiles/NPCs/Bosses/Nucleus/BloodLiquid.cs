using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Buffs;
using TranscendenceMod.Miscanellous.MiscSystems;
using TranscendenceMod.Miscannellous.GlobalStuff;
using TranscendenceMod.NPCs.Boss.Nucleus;

namespace TranscendenceMod.Projectiles.NPCs.Bosses.Nucleus
{
    public class BloodLiquid : ModProjectile
    {
        public float Fade;
        public override string Texture => "TranscendenceMod/Miscannellous/Assets/Pixel";
        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.extraUpdates = 2;

            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 60;
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 4000;
        }
        public float Sinewave;
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            overWiresUI.Add(index);
        }
        public bool InsideLiquid(Entity entity)
        {
            for (int i = -125; i < 250; i++)
            {
                float y = (float)Math.Sin(i * 0.035f * 2f + TranscendenceWorld.UniversalRotation * 1.5f) * 200f;
                float y2 = Projectile.Center.Y - 620 + (int)Sinewave + y;
                float x = Projectile.Center.X + 1000 + (i * 20f);

                //Dust.QuickDust(new Vector2(x, y2), Color.White);

                if (entity.Center.Between(new Vector2(x - 50, y2), new Vector2(x + 50, y2 + 750)))
                    return true;
            }
            return false;
        }
        public override void AI()
        {
            Projectile.timeLeft = 60;
            Projectile.hide = true;

            Sinewave = (float)Math.Sin(TranscendenceWorld.UniversalRotation * 2f) * 50;

            NPC npc = Main.npc[(int)Projectile.ai[1]];

            if (InsideLiquid(Main.LocalPlayer) && Main.LocalPlayer.GetModPlayer<TranscendencePlayer>().NucleusConsumed == 0
                && !Main.LocalPlayer.GetModPlayer<NucleusGame>().Active && npc.ai[1] != 99)
                Main.LocalPlayer.AddBuff(ModContent.BuffType<MagmaBlood>(), 2);

            int height = 1000;

            if (npc == null || !npc.active || npc.life < (npc.lifeMax * 0.1f) || npc.type != ModContent.NPCType<ProjectNucleus>())
            {
                if (Projectile.Center.Y < Projectile.GetGlobalProjectile<TranscendenceProjectiles>().startPos.Y)
                    Projectile.position.Y += 2f;
                else Projectile.Kill();
            }
            else
            {
                if (Projectile.Center.Y > (Projectile.GetGlobalProjectile<TranscendenceProjectiles>().startPos.Y - height))
                    Projectile.position.Y -= 1f;
            }

        }
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch sb = Main.spriteBatch;

            //Request the Effect
            var eff = ModContent.Request<Effect>("TranscendenceMod/Miscannellous/Assets/Shaders/Effects/NucleusLiquidShader", AssetRequestMode.ImmediateLoad).Value;
            //Apply Shader Texture
            Texture2D shaderImage = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/Blood").Value;
            Main.instance.GraphicsDevice.Textures[1] = shaderImage;

            eff.Parameters["uImageSize0"].SetValue(new Vector2(5000, 2500));
            eff.Parameters["uImageSize1"].SetValue(shaderImage.Size());
            eff.Parameters["uTime"].SetValue(TranscendenceWorld.UniversalRotation * 1.5f);

            sb.End();
            sb.Begin(default, BlendState.Additive, Main.DefaultSamplerState, default, default, eff, Main.GameViewMatrix.TransformationMatrix);

            int x = (int)Projectile.Center.X - (int)Main.screenPosition.X;
            int y = (int)Projectile.Center.Y - (int)Main.screenPosition.Y;
            sb.Draw(TextureAssets.BlackTile.Value, new Rectangle(x - 2500, y - 1250 + (int)Sinewave, 5000, 2500), Color.White);

            sb.End();
            sb.Begin(default, BlendState.AlphaBlend, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);
            return false;
        }
    }
}