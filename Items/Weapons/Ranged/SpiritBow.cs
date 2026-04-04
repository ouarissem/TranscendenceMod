using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Dusts;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Projectiles.Weapons.Ranged;

namespace TranscendenceMod.Items.Weapons.Ranged
{
    public class SpiritBow : ModItem
    {
        public Vector2 pos;
        public Vector2 pos2;
        public float rot;
        public bool Powered;

        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 48;
            Item.knockBack = 3f;
            Item.DamageType = DamageClass.Ranged;

            Item.width = 30;
            Item.height = 42;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 6;
            Item.useAnimation = 16;

            Item.value = Item.sellPrice(gold: 10);
            Item.rare = ItemRarityID.Cyan;

            Item.UseSound = SoundID.Item102;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.consumeAmmoOnFirstShotOnly = true;

            Item.crit = 15;
            Item.useAmmo = AmmoID.Arrow;
            Item.shoot = ProjectileID.WoodenArrowFriendly;
            Item.shootSpeed = 25f;
        }
        public override bool? UseItem(Player player)
        {
            rot += 0.33f;

            Vector2 pos2 = Vector2.One.RotatedBy(rot) * 50f;
            Vector2 pos3 = Vector2.One.RotatedBy(player.DirectionTo(Main.MouseWorld).ToRotation() - MathHelper.PiOver4) * 50f;
            Vector2 pos = player.Center + pos3 + new Vector2(pos2.X, pos2.Y / 3f).RotatedBy(player.DirectionTo(Main.MouseWorld).ToRotation() - MathHelper.PiOver2);
            Vector2 pos12 = player.Center + pos3 + new Vector2(-pos2.X, -pos2.Y / 3f).RotatedBy(player.DirectionTo(Main.MouseWorld).ToRotation() - MathHelper.PiOver2);

            int d12 = Dust.NewDust(pos, 1, 1, ModContent.DustType<ArenaDust>(), 0, 0, 0, Color.DeepSkyBlue, 2.25f);
            Main.dust[d12].velocity = Vector2.Zero;

            int d22 = Dust.NewDust(pos12, 1, 1, ModContent.DustType<ArenaDust>(), 0, 0, 0, Color.DeepSkyBlue, 2.25f);
            Main.dust[d22].velocity = Vector2.Zero;


            int d = Dust.NewDust(pos, 1, 1, ModContent.DustType<ArenaDust>(), 0, 0, 0, default, 1.5f);
            Main.dust[d].velocity = Vector2.Zero;

            int d2 = Dust.NewDust(pos12, 1, 1, ModContent.DustType<ArenaDust>(), 0, 0, 0, default, 1.5f);
            Main.dust[d2].velocity = Vector2.Zero;

            return base.UseItem(player);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 pos2 = Vector2.One.RotatedBy(rot) * 50f;
            Vector2 pos3 = Vector2.One.RotatedBy(player.DirectionTo(Main.MouseWorld).ToRotation() - MathHelper.PiOver4) * 50f;
            Vector2 pos = player.Center + pos3 + new Vector2(pos2.X, pos2.Y / 3f).RotatedBy(player.DirectionTo(Main.MouseWorld).ToRotation() - MathHelper.PiOver2);
            Vector2 pos12 = player.Center + pos3 + new Vector2(-pos2.X, -pos2.Y / 3f).RotatedBy(player.DirectionTo(Main.MouseWorld).ToRotation() - MathHelper.PiOver2);

            if (pos != Vector2.Zero && velocity != Vector2.Zero)
            {
                TranscendenceUtils.DustRing(pos, 10, DustID.Phantasmal, 2, Color.White, 1);
                Projectile.NewProjectile(source, pos, velocity, ModContent.ProjectileType<DivineSoul>(), damage, knockback, player.whoAmI);

                TranscendenceUtils.DustRing(pos12, 10, DustID.Phantasmal, 2, Color.White, 1);
                Projectile.NewProjectile(source, pos12, velocity, ModContent.ProjectileType<DivineSoul>(), damage, knockback, player.whoAmI);
            }
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.PulseBow, 1)
            .AddIngredient(ItemID.SpectreBar, 17)
            .AddIngredient(ItemID.FragmentVortex, 10)
            .AddTile(TileID.LunarCraftingStation)
            .Register();
        }
    }
}