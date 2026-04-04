using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;

namespace TranscendenceMod.Miscannellous.UI
{
    public class VampireNecklaceBar : UIElement
    {
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            Texture2D sprite = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/UI/VampireNecklaceBar").Value;
            Texture2D sprite2 = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/UI/VampireNecklaceBar_Inner").Value;
            Player player = Main.LocalPlayer;

            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.AlphaBlend, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            if (player != null && player.active && player.TryGetModPlayer(out TranscendencePlayer modplayer) && modplayer.Vampire)
            {
                int x = (int)(player.Center.X - Main.screenPosition.X);
                int y = (int)(player.Center.Y - 100 - Main.screenPosition.Y);

                int width = (int)MathHelper.Lerp(0f, 32f, modplayer.VampireBlood / (float)modplayer.CrimsonNecklaceMaxBlood);
                int width2 = (int)MathHelper.Lerp(0f, 32f, modplayer.VampireMinTimer / (float)modplayer.VampireMinTime);
                Rectangle rec = new Rectangle(x - (sprite.Width / 2), y + 59, sprite.Width, sprite.Height);
                Rectangle rec2 = new Rectangle(x - 16, y + 65, width, 6);
                Rectangle rec3 = new Rectangle(x - 16, y + 65, width2, 6);

                Color col = modplayer.VampireBlood >= modplayer.CrimsonNecklaceMaxBlood ? (modplayer.VampireMinTimer >= modplayer.VampireMinTime ? Color.Lime : Color.Gray) : Color.Red;

                spriteBatch.Draw(sprite2, rec, Color.White);
                spriteBatch.Draw(TextureAssets.BlackTile.Value, rec2, col);
                if (modplayer.VampireMinTimer < modplayer.VampireMinTime && modplayer.VampireBlood >= modplayer.CrimsonNecklaceMaxBlood)
                    spriteBatch.Draw(TextureAssets.BlackTile.Value, rec3, Color.White);

                spriteBatch.Draw(sprite, rec, Color.White);

                if (modplayer.VampireBlood > 0 && modplayer.VampireBlood < modplayer.CrimsonNecklaceMaxBlood)
                {
                    for (int i = 0; i < 6; i++)
                    {
                        spriteBatch.Draw(TextureAssets.BlackTile.Value, new Vector2(x - 16 + width, y + 68) + Main.rand.NextVector2Circular(3f, 6f),
                            null, col, 0f, TextureAssets.BlackTile.Value.Size() * 0.5f, 0.125f, SpriteEffects.None, 0f);
                    }
                }

                bool Hover = Main.MouseWorld.Between(new Vector2(x - (sprite.Width / 2), y + 59) + Main.screenPosition, new Vector2(x - (sprite.Width / 2), y + 59) + Main.screenPosition + new Vector2(sprite.Width, sprite.Height));
                if (Hover)
                {
                    Main.hoverItemName = modplayer.VampireBlood + " / " + modplayer.CrimsonNecklaceMaxBlood;
                }
            }

            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.AlphaBlend);
        }
    }

    public class VampireNecklaceBarState : UIState
    {
        public VampireNecklaceBar neck;
        public override void OnInitialize()
        {
            neck = new VampireNecklaceBar();
            Append(neck);
        }
    }
}
namespace TranscendenceMod.Miscannellous.UI
{
    [Autoload(Side = ModSide.Client)]
    public class VampireNecklaceBarDraw : ModSystem
    {
        internal VampireNecklaceBarState necks;
        private UserInterface necks2;

        public override void Load()
        {
            necks = new VampireNecklaceBarState();
            necks.Activate();

            necks2 = new UserInterface();
            necks2.SetState(necks);
        }
        public override void UpdateUI(GameTime gameTime)
        {
            necks2?.Update(gameTime);
        }
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (mouseTextIndex != -1)
            {
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer("TranscendenceMod: Vampire Necklace UI", delegate
                {
                    necks2.Draw(Main.spriteBatch, new GameTime());
                    return true;
                }, InterfaceScaleType.UI));
            }
        }
    }
}
