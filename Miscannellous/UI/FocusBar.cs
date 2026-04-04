using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Terraria;
using Terraria.GameContent;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.UI.Chat;
using TranscendenceMod.Miscanellous.MiscSystems;
using TranscendenceMod.Miscannellous.UI.Achievements;


namespace TranscendenceMod.Miscannellous.UI
{
    public class FocusBar : UIElement
    {
        public float a;
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            Texture2D sprite = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/UI/FocusBar").Value;
            Texture2D sprite2 = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/UI/FocusBar_Jewel").Value;

            Player player = Main.LocalPlayer;
            Vector2 pos = player.Center - Main.screenPosition;

            if (player != null && player.GetModPlayer<NucleusGame>().Active)
            {
                Main.mouseText = false;
                Main.hoverItemName = "";
                Main.isMouseLeftConsumedByUI = true;
                Main.LocalPlayer.mouseInterface = true;
            }


            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.AlphaBlend, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);


            if (player != null && player.active && player.TryGetModPlayer(out TranscendencePlayer modPlayer) && !QuestBookUIDrawing.Visible)
            {
                float focus = modPlayer.Focus;
                float maxFocus = modPlayer.MaxFocus;
                float cost = modPlayer.ParryFocusCost;


                int x = (int)pos.X;
                int y = (int)(pos.Y + (55 + player.gfxOffY) * Main.UIScale);


                int width = (int)MathHelper.Lerp(0, 52, focus / maxFocus);
                int width2 = (int)MathHelper.Lerp(0, 52, cost / maxFocus);


                x -= sprite.Width / 2;
                Rectangle rec = new Rectangle(x, y, sprite.Width, sprite.Height);
                Rectangle rec2 = new Rectangle(x + 16, y + 8, width, 8);
                Rectangle rec3 = new Rectangle(x + 16, y + 8, width2, 8);

                spriteBatch.Draw(sprite, rec, Color.White);
                spriteBatch.Draw(sprite2, rec, focus < modPlayer.ParryFocusCost ? Color.Red : Color.Lime);


                if (focus < cost)
                    spriteBatch.Draw(TextureAssets.BlackTile.Value, rec3, Color.Red);

                spriteBatch.Draw(TextureAssets.BlackTile.Value, rec2, Color.White);

                if (focus > cost)
                spriteBatch.Draw(TextureAssets.BlackTile.Value, rec3, Color.Lime * 0.5f);


                bool Hover = Main.MouseWorld.Between(new Vector2(x, y) + Main.screenPosition, new Vector2(x, y) + Main.screenPosition + new Vector2(sprite.Width, sprite.Height * 2));
                if (Hover)
                    a = MathHelper.Lerp(a, 1f, 1f / 20f);
                else a = MathHelper.Lerp(a, 0.25f, 1f / 10f);

                bool showParticles = focus > 0 && focus < maxFocus && modPlayer.FocusRegenDelay <= 0f && player.velocity.Length() < 10f;
                if (showParticles)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        spriteBatch.Draw(TextureAssets.BlackTile.Value, new Vector2(x + 16 + width, y + 12) + Main.rand.NextVector2Circular(4f, 8f), null, Color.White, 0f, TextureAssets.BlackTile.Value.Size() * 0.5f, 0.175f, SpriteEffects.None, 0f);
                    }
                }

                string t = (Hover ? Language.GetTextValue("Mods.TranscendenceMod.Messages.FocusUI") : "") + $" ({Math.Round(modPlayer.Focus, 0)}/{modPlayer.MaxFocus})";
                ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.MouseText.Value, t, new Vector2(pos.X - (FontAssets.MouseText.Value.MeasureString(t).X * 0.5f), y + 20), Color.White * a, 0f, Vector2.Zero, Vector2.One);
            }

            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.AlphaBlend);

        }
    }

    public class FocusState : UIState
    {
        public FocusBar focus;

        public override void OnInitialize()
        {
            focus = new FocusBar();
            Append(focus);
        }
    }
}
namespace TranscendenceMod.Miscannellous.UI
{
    [Autoload(Side = ModSide.Client)]
    public class ParryBarDraw : ModSystem
    {
        internal FocusState parryBar;
        private UserInterface parryBar2;

        public override void Load()
        {
            parryBar = new FocusState();
            parryBar.Activate();

            parryBar2 = new UserInterface();
            parryBar2.SetState(parryBar);
        }
        public override void UpdateUI(GameTime gameTime)
        {
            parryBar2?.Update(gameTime);
        }
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (mouseTextIndex != -1)
            {
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer("TranscendenceMod: Focus Gauge", delegate
                {
                    parryBar2.Draw(Main.spriteBatch, new GameTime());
                    return true;
                }, InterfaceScaleType.UI));
            }
        }
    }
}
