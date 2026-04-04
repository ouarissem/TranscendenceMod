using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace TranscendenceMod.Items.Farming
{
    public class Carrot : BasePlantFood
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            ItemID.Sets.FoodParticleColors[Type] = new Color[3]
            {
                new Color(150, 63, 36),
                new Color(255, 153, 0),
                new Color(255, 213, 27)
            };
        }
        public override void SetDefaults()
        {
            Item.DefaultToFood(15, 25, BuffID.WellFed2, 4 * 60 * 60);
            Item.value = Item.sellPrice(silver: 25);
        }
    }
}
