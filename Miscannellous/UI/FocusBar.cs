using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.UI.Chat;
using TranscendenceMod.Miscanellous.MiscSystems;
using TranscendenceMod.Miscannellous.UI.Achievements;

namespace TranscendenceMod.Miscannellous.UI
{
    public class FocusBarLayer : PlayerDrawLayer
    {
        public float a;

        public override Position GetDefaultPosition()
        {
            return new AfterParent(PlayerDrawLayers.Head);
        }

        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
        {
            Player player = drawInfo.drawPlayer;
            return player != null && player.active && !player.dead;
        }

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            Texture2D sprite = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/UI/FocusBar").Value;
            Texture2D sprite2 = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/UI/FocusBar_Jewel").Value;

            Player player = drawInfo.drawPlayer;
            Vector2 pos = player.Center - Main.screenPosition;

            // if (player != null && player.GetModPlayer<NucleusGame>().Active)
            // {
            //     Main.mouseText = false;
            //     Main.hoverItemName = "";
            //     Main.isMouseLeftConsumedByUI = true;
            //     Main.LocalPlayer.mouseInterface = true;
            // }

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

                drawInfo.DrawDataCache.Add(new DrawData(sprite, rec, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0));
                drawInfo.DrawDataCache.Add(new DrawData(sprite2, rec, null, focus < modPlayer.ParryFocusCost ? Color.Red : Color.Lime, 0f, Vector2.Zero, SpriteEffects.None, 0));

                if (focus < cost)
                    drawInfo.DrawDataCache.Add(new DrawData(TextureAssets.BlackTile.Value, rec3, null, Color.Red, 0f, Vector2.Zero, SpriteEffects.None, 0));

                drawInfo.DrawDataCache.Add(new DrawData(TextureAssets.BlackTile.Value, rec2, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0));

                if (focus > cost)
                    drawInfo.DrawDataCache.Add(new DrawData(TextureAssets.BlackTile.Value, rec3, null, Color.Lime * 0.5f, 0f, Vector2.Zero, SpriteEffects.None, 0));

                bool Hover = Main.MouseWorld.Between(new Vector2(x, y) + Main.screenPosition, new Vector2(x, y) + Main.screenPosition + new Vector2(sprite.Width, sprite.Height * 2));

                if (Hover)
                {
                    a = MathHelper.Lerp(a, 1f, 1f / 20f);
                    Main.LocalPlayer.mouseInterface = true;
                }
                else
                {
                    a = MathHelper.Lerp(a, 0.25f, 1f / 10f);
                }

                bool showParticles = focus > 0 && focus < maxFocus && modPlayer.FocusRegenDelay <= 0f && player.velocity.Length() < 10f;
                if (showParticles)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        Vector2 particlePos = new Vector2(x + 16 + width, y + 12) + Main.rand.NextVector2Circular(4f, 8f);
                        drawInfo.DrawDataCache.Add(new DrawData(TextureAssets.BlackTile.Value, particlePos, null, Color.White, 0f, TextureAssets.BlackTile.Value.Size() * 0.5f, 0.175f, SpriteEffects.None, 0));
                    }
                }

                string t = (Hover ? Language.GetTextValue("Mods.TranscendenceMod.Messages.FocusUI") : "") + $" ({Math.Round(modPlayer.Focus, 0)}/{modPlayer.MaxFocus})";

                FocusBarTooltip.TextToDraw = t;
                FocusBarTooltip.TextAlpha = a;
                FocusBarTooltip.TextPosition = new Vector2(pos.X - (FontAssets.MouseText.Value.MeasureString(t).X * 0.5f), y + 20);
                FocusBarTooltip.ShouldDraw = true;
            }
            else
            {
                FocusBarTooltip.ShouldDraw = false;
            }
        }
    }

    [Autoload(Side = ModSide.Client)]
    public class FocusBarTooltip : ModSystem
    {
        public static string TextToDraw = "";
        public static float TextAlpha = 0f;
        public static Vector2 TextPosition = Vector2.Zero;
        public static bool ShouldDraw = false;

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (mouseTextIndex != -1)
            {
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer("TranscendenceMod: Focus Tooltip", delegate
                {
                    if (ShouldDraw && !string.IsNullOrEmpty(TextToDraw))
                    {
                        ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, FontAssets.MouseText.Value, TextToDraw, TextPosition, Color.White * TextAlpha, 0f, Vector2.Zero, Vector2.One);
                    }
                    return true;
                }, InterfaceScaleType.UI));
            }
        }
    }
}
