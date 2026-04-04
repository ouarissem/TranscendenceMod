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
    public class AngelicHairdyeData : HairShaderData
    {
        public Asset<Texture2D> img;
        public AngelicHairdyeData(Ref<Effect> shader, string passName) : base(shader, passName)
        {
            img = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/SerpentCrack");
        }
        public override void Apply(Player player, DrawData? drawData = null)
        {
            if (drawData.HasValue)
            {
                UseColor(new Color(255, 225, 0));
                UseImage(ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/SeraphForcefieldShader"));
                Shader.Parameters["uTime"].SetValue(Main.GlobalTimeWrappedHourly * 2f);
            }

            base.Apply(player, drawData);
        }
    }
    public class AngelicHairdye : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;

            if (!Main.dedServ)
            {
                Effect eff = ModContent.Request<Effect>("TranscendenceMod/Miscannellous/Assets/Shaders/Effects/AngelHairSHader", AssetRequestMode.ImmediateLoad).Value;
                GameShaders.Hair.BindShader(Item.type, new AngelicHairdyeData(new Ref<Effect>(eff), "AngelHairShaderT2"));
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

