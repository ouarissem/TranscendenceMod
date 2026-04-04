using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace TranscendenceMod.Items.Farming
{
    public class Potato : BasePlantFood
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            ItemID.Sets.FoodParticleColors[Type] = new Color[2]
            {
                new Color(142, 99, 55),
                new Color(255, 224, 126)
            };
        }
        public override void SetDefaults()
        {
            Item.DefaultToFood(22, 22, BuffID.WellFed, 2 * 60 * 60);
            Item.value = Item.sellPrice(silver: 10);
        }
    }
}
