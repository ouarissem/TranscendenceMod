using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace TranscendenceMod.Items.Farming
{
    public class Tomato : BasePlantFood
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            ItemID.Sets.FoodParticleColors[Type] = new Color[2]
            {
                new Color(38, 108, 41),
                new Color(255, 0, 0)
            };
        }
        public override void SetDefaults()
        {
            Item.DefaultToFood(22, 22, BuffID.WellFed, (int)(2.5f * 60f * 60f));
            Item.value = Item.sellPrice(silver: 25);
        }
    }
}
