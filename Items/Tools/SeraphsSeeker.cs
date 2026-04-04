using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using TranscendenceMod.Miscannellous.Rarities;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Projectiles.Equipment.Tools;
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Tiles.BigTiles;

namespace TranscendenceMod.Items.Tools
{
    public class SeraphsSeeker : ModItem
	{
        public int bobber = ModContent.ProjectileType<SeraphBobber>();
		public override void SetStaticDefaults()
		{
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            ItemID.Sets.CanFishInLava[Item.type] = true;
        }

		public override void SetDefaults()
		{
            Item.width = 42;
			Item.height = 56;

			Item.value = Item.sellPrice(gold: 20);
            Item.rare = ModContent.RarityType<CosmicRarity>();
            Item.fishingPole = 65;

            Item.shoot = bobber;
            Item.shootSpeed = 13f;

            Item.UseSound = SoundID.Item1;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useAnimation = 8;
            Item.useTime = 8;
        }
        public override Color? GetAlpha(Color lightColor) => Color.White;

        public override void ModifyFishingLine(Projectile bobber, ref Vector2 lineOriginOffset, ref Color lineColor)
        {
            lineOriginOffset = new Vector2(39, -43);
            lineColor = TranscendenceWorld.CosmicPurple;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.GoldenFishingRod)
            .AddIngredient(ModContent.ItemType<StarcraftedAlloy>())
            .AddTile(ModContent.TileType<ShimmerAltar>())
            .Register();
        }
    }
}
