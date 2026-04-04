using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous.Rarities;

namespace TranscendenceMod.Items.Cosmetics
{
    public class CosmicFogDyeData : ArmorShaderData
    {
        public Asset<Texture2D> fog;
        public CosmicFogDyeData(Ref<Effect> shader, string passName) : base(shader, passName)
        {
            fog = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/SerpentCrack");
        }
        public override void Apply(Entity entity, DrawData? drawData = null)
        {
            if (drawData != null)
            {
                UseColor(new Color(125, 0, 155));
                UseImage(fog);
                Shader.Parameters["uImageSize1"].SetValue(new Vector2(50));
                Shader.Parameters["uTime"].SetValue(Main.GlobalTimeWrappedHourly * 4f);
            }

            base.Apply(entity, drawData);
        }
    }
    public class CosmicFogDye : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
            if (!Main.dedServ)
            {
                Effect eff = ModContent.Request<Effect>("TranscendenceMod/Miscannellous/Assets/Shaders/Effects/CosmicFogDyeShader", AssetRequestMode.ImmediateLoad).Value;
                GameShaders.Armor.BindShader(Item.type, new CosmicFogDyeData(new Ref<Effect>(eff), "CosmicFogDyeShaderT2"));
            }
        }
        public override void SetDefaults()
        {
            Item.width = 19;
            Item.height = 24;
            Item.value = Item.sellPrice(gold: 25);
            Item.rare = ModContent.RarityType<CosmicRarity>();
        }
        public override Color? GetAlpha(Color lightColor) => Color.White;
    }
}
