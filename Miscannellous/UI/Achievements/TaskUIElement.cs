using Microsoft.Build.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.UI;
using Terraria.ModLoader.UI.ModBrowser;
using Terraria.UI;
using Terraria.UI.Chat;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Miscanellous.UI.Achievements
{
    public abstract class TaskUIElement : UIElement
    {
        public abstract Texture2D Icon { get; }
        public abstract bool Unlocked { get; }

        public abstract TaskIDs type { get; }

        public abstract float x { get; }
        public abstract float y { get; }
        public abstract string col { get; }

        public abstract int reward { get; }
        public abstract int amount { get; }

        public abstract CategoryIDs category { get; }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            Vector2 pos = new Vector2(x * Main.UIScale + Main.screenWidth / 2f, y * Main.UIScale + Main.screenHeight / 2f);
            bool Hovering = Main.MouseWorld.Distance(Main.screenPosition + pos) < 15;
            Main.LocalPlayer.TryGetModPlayer(out TranscendencePlayer play);

            // Draw Panel
            Texture2D sprite = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/UI/Achievements/TaskUIElement").Value;
            Texture2D New = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/UI/Achievements/NEW").Value;

            spriteBatch.Draw(sprite, pos + new Vector2(4, 8), null, Unlocked ? Color.Black * 0.375f : Color.Red * 0.5f, 0f, sprite.Size() * 0.5f, 1f * Main.UIScale, SpriteEffects.None, 0f);
            spriteBatch.Draw(sprite, pos, null, Color.White, 0f, sprite.Size() * 0.5f, 1f * Main.UIScale, SpriteEffects.None, 0f);

            // Draw Icon
            spriteBatch.Draw(Icon, pos, null, Unlocked ? Color.White : new Color(50, 50, 50), 0f, Icon.Size() * 0.5f, (Hovering ? 1.375f : 1f) * Main.UIScale, SpriteEffects.None, 0f);


            // NEW Marker Icon
            if (play.NewAchievements.Contains(type))
            {
                float size = (1f + (float)Math.Sin(Main.GlobalTimeWrappedHourly * 4f) * 0.1f) * Main.UIScale;
                spriteBatch.Draw(New, pos, null, Color.White, 0f, New.Size() * 0.5f, size, SpriteEffects.None, 0f);
            }


            if (type == TaskIDs.Begin)
            {
                string begin = Language.GetTextValue($"Mods.TranscendenceMod.Achievement.Markers.Start");

                ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.MouseText.Value,
                    begin, pos - new Vector2(FontAssets.MouseText.Value.MeasureString(begin).X * 0.5f, 35f * Main.UIScale), Color.White, 0, Vector2.Zero, Vector2.One);
            }
            if (type == TaskIDs.StarForge)
            {
                string end = Language.GetTextValue($"Mods.TranscendenceMod.Achievement.Markers.End");

                ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.MouseText.Value,
                    end, pos - new Vector2(FontAssets.MouseText.Value.MeasureString(end).X * 0.5f, -10f * Main.UIScale), Color.White, 0, Vector2.Zero, Vector2.One);
            }

            // Hover Text
            if (Hovering)
            {
                string typeString = type.ToString();
                string catString = category.ToString();

                UICommon.TooltipMouseText($"[C/{col}:{Language.GetTextValue($"Mods.TranscendenceMod.Achievement.Steps.{typeString}.DisplayName")}]" + "\n" +
                    Language.GetTextValue($"Mods.TranscendenceMod.Achievement.Steps.{typeString}.Tooltip") + "\n" +
                    Language.GetTextValue($"Mods.TranscendenceMod.Achievement.Rewards", amount, reward) + "\n" +
                    Language.GetTextValue($"Mods.TranscendenceMod.Achievement.Categories.{catString}"));

                if (Main.mouseRight && play.NewAchievements.Contains(type))
                {
                    SoundEngine.PlaySound(SoundID.Research);
                    Item.NewItem(Main.LocalPlayer.GetSource_FromAI(), Main.LocalPlayer.getRect(), reward, amount);
                    play.NewAchievements.Remove(type);
                }
            }
        }
    }

    public class DescriptionTooltip : ModItem
    {
        public override string Texture => "TranscendenceMod/Miscannellous/Assets/InvisSprite";

        public virtual string Description { get; set; }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            tooltips.Clear();

            var tt = new TooltipLine(Mod, "QuestDescription", Description);
            tooltips.Add(tt);
        }
    }
}

