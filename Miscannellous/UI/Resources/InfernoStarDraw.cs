using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace TranscendenceMod.Miscannellous.UI.Resources
{
    public class InfernoStarDraw : ModResourceOverlay
    {
        private Dictionary<string, Asset<Texture2D>> vanillaAssetCache = new();
        private Asset<Texture2D> heart, fancyBG, bar, barBG;
        string fancy = "Images/UI/PlayerResourceSets/FancyClassic/";
        string bars = "Images/UI/PlayerResourceSets/HorizontalBars/";
        public override void PostDrawResource(ResourceOverlayDrawContext context)
        {
            Asset<Texture2D> asset = context.texture;
            
            return;

            if (asset == TextureAssets.Mana)
                DrawHeart(context);

            else if (CompareAssets(asset, fancy + "Star_Fill"))
                DrawHeart(context);

            else if (CompareAssets(asset, bars + "MP_Fill"))
                DrawBars(context);

            else if (CompareAssets(asset, bars + "MP_Panel_Middle"))
                DrawBarsBG(context);
        }

        private void DrawHeart(ResourceOverlayDrawContext context)
        {
            context.texture = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/UI/Resources/InfernoStar");
            context.Draw();
        }

        private void DrawBars(ResourceOverlayDrawContext context)
        {
            context.texture = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/UI/Resources/InfernoBar");
            context.Draw();
        }

        private void DrawBarsBG(ResourceOverlayDrawContext context)
        {
            context.texture = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/UI/Resources/InfernoBarBG");
            context.position += new Vector2(0, 6);
            context.scale.Y = 0.5f;
            context.Draw();
        }

        private bool CompareAssets(Asset<Texture2D> vanillaAsset, string v)
        {
            if (!vanillaAssetCache.TryGetValue(v, out var asset))
                asset = vanillaAssetCache[v] = Main.Assets.Request<Texture2D>(v);
            return vanillaAsset == asset;
        }
    }
}
