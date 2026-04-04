using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using ReLogic.Content;
using Terraria.Graphics.Shaders;
using Terraria.GameContent.Creative;
using TranscendenceMod.Miscannellous.Rarities;
using Terraria.ID;
using Terraria.DataStructures;

namespace TranscendenceMod.Items.Cosmetics
{
    public class EarthHairdyeData : HairShaderData
    {
        public Asset<Texture2D> earth;
        public EarthHairdyeData(Ref<Effect> shader, string passName) : base(shader, passName)
        {
            earth = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/EarthMap");
        }
        public override void Apply(Player player, DrawData? drawData = null)
        {
            if (drawData.HasValue)
            {
                UseColor(new Color(0, 0, 255));
                UseImage(earth);
                Shader.Parameters["uTime"].SetValue(Main.GlobalTimeWrappedHourly * 0.66f * player.direction);
            }

            base.Apply(player, drawData);
        }
    }
    public class EarthHairdye : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;

            if (!Main.dedServ)
            {
                Effect eff = ModContent.Request<Effect>("TranscendenceMod/Miscannellous/Assets/Shaders/Effects/EarthHairShader", AssetRequestMode.ImmediateLoad).Value;
                GameShaders.Hair.BindShader(Item.type, new EarthHairdyeData(new Ref<Effect>(eff), "EarthHairShaderT2"));
            }
        }
        public override void SetDefaults()
        {
            Item.width = 14;
            Item.height = 24;

            Item.maxStack = 9999;
            Item.UseSound = SoundID.Item3;

            Item.consumable = true;
            Item.useTime = 20;
            Item.useAnimation = 20;

            Item.useStyle = ItemUseStyleID.DrinkLiquid;

            Item.value = Item.sellPrice(gold: 25);
            Item.rare = ModContent.RarityType<CosmicRarity>();
        }
        public override Color? GetAlpha(Color lightColor) => Color.White;
    }
}

