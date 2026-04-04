using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Buffs.Items.Potions;

namespace TranscendenceMod.Items.Consumables.FoodAndDrinks
{
    public class CosmicJelly : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            ItemID.Sets.FoodParticleColors[Type] = new Color[2]
            {
                new Color(36, 89, 174),
                new Color(255, 103, 211)
            };
            ItemID.Sets.IsFood[Type] = true;
            Main.RegisterItemAnimation(Type, new DrawAnimationVertical(int.MaxValue, 3));
        }
        public override void SetDefaults()
        {
            Item.DefaultToFood(24, 16, ModContent.BuffType<JellyBuff>(), 4 * 60 * 60);
        }
    }
}
