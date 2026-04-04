using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace TranscendenceMod.Items.Tools
{
	public class VineGun : ModItem
	{
		public override void SetStaticDefaults()
		{
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

		public override void SetDefaults()
		{
            Item.width = 15;
			Item.height = 7;
			Item.value = Item.sellPrice(silver: 25);
			Item.rare = ItemRarityID.Blue;
			Item.shoot = ProjectileID.VineRopeCoil;
            Item.useAmmo = ItemID.VineRopeCoil;
            Item.autoReuse = true;
            Item.UseSound = SoundID.Item11;
            Item.shootSpeed = 13f;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
        }
	}
}
