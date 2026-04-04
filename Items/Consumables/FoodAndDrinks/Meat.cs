using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace TranscendenceMod.Items.Consumables.FoodAndDrinks
{
    public class Meat : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            ItemID.Sets.FoodParticleColors[Type] = new Color[2]
            {
                new Color(121, 26, 33),
                new Color(187, 90, 71)
            };
            ItemID.Sets.IsFood[Type] = true;
            Main.RegisterItemAnimation(Type, new DrawAnimationVertical(int.MaxValue, 3));
        }
        public override void SetDefaults()
        {
            Item.DefaultToFood(24, 16, BuffID.WellFed2, 8 * 60 * 60);
        }
    }
}
