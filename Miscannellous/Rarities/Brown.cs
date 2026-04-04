using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace TranscendenceMod.Miscannellous.Rarities
{
    public class Brown : ModRarity
    {
        public override int GetPrefixedRarity(int offset, float valueMult)
        {
            if (offset < 0)
                return ModContent.RarityType<ModdedPurple>();
            if (offset > 0)
                return ModContent.RarityType<SteelGray>();
            return Type;
        }
        public override Color RarityColor => new Color(145, 92, 79);
    }
}