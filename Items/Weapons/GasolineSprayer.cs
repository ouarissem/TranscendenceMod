using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Projectiles.Weapons;

namespace TranscendenceMod.Items.Weapons
{
    public class GasolineSprayer : ModItem
    {
        public override void SetDefaults()
        {
            Item.staff[Type] = true;
            Item.damage = 2;
            Item.DamageType = DamageClass.Default;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 4;
            Item.useAnimation = 4;

            Item.UseSound = SoundID.Item96;
            Item.autoReuse = true;

            Item.width = 18;
            Item.height = 22;

            Item.value = Item.sellPrice(gold: 5);
            Item.rare = ItemRarityID.Green;

            Item.shoot = ModContent.ProjectileType<GasolineProj>();
            Item.shootSpeed = 10f;
        }
    }
}
