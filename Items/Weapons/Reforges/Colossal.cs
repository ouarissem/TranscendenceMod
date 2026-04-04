using Terraria;
using Terraria.ModLoader;

namespace TranscendenceMod.Items.Weapons.Reforges
{
    public class Colossal : ModPrefix
    {
        public override PrefixCategory Category => PrefixCategory.Melee;
        public override float RollChance(Item item)
        {
            return 0.2f;
        }
        public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult, ref float shootSpeedMult, ref float manaMult, ref int critBonus)
        {
            useTimeMult *= 1f + 0.25f;
            knockbackMult *= 1f + 0.4f;
            scaleMult *= 1f + 0.4f;
        }
        public override void ModifyValue(ref float valueMult)
        {
            valueMult += 1f * 0.12f;
        }
    }
}