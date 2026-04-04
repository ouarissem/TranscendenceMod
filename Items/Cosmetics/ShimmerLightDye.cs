using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace TranscendenceMod.Items.Cosmetics
{
    public class ShimmerLightDye : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
            if (!Main.dedServ)
            {
                GameShaders.Armor.BindShader(Item.type, new ArmorShaderData(Mod.Assets.Request<Effect>("Miscannellous/Assets/Shaders/Effects/CreamsShader"),
                    "Test2").UseColor(new Color(25, 55, 138)));
            }
        }
        public override void SetDefaults()
        {
            Item.width = 19;
            Item.height = 24;
            Item.value = Item.sellPrice(gold: 3, silver: 75);
            Item.rare = ItemRarityID.Cyan;
        }
    }
}
