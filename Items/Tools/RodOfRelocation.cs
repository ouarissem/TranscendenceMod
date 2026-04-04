using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Creative;
using TranscendenceMod.Items.Consumables;
using TranscendenceMod.Projectiles.Equipment.Tools;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Items.Tools
{
    public class RodOfRelocation : ModItem
	{
		public override void SetStaticDefaults()
		{
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
		public override void SetDefaults()
		{
            Item.width = 20;
			Item.height = 43;
			Item.value = Item.sellPrice(gold: 10);
			Item.rare = ItemRarityID.Cyan;

            Item.UseSound = SoundID.Item8;
            Item.useTime = 20;
            Item.useAnimation = 20;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<RodOfRelocationWarp>();
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) => position = player.Center;
        public override bool CanUseItem(Player player) => !player.GetModPlayer<TranscendencePlayer>().WandOfReturnCD && player.ownedProjectileCounts[Item.shoot] == 0 && !Collision.SolidCollision(player.position, player.width, player.height) && !TranscendenceUtils.BossAlive();
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.PortalGun)
                .AddIngredient(ItemID.Sapphire, 15)
                .AddIngredient(ModContent.ItemType<Starfruit>(), 5)
                .AddTile(TileID.DemonAltar)
                .Register();

        }
    }
}
