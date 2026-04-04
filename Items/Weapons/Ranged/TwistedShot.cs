using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Items.Materials.MobDrops;
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Items.Weapons.Melee;
using TranscendenceMod.Miscannellous.Rarities;
using TranscendenceMod.Projectiles.Weapons.Ranged;

namespace TranscendenceMod.Items.Weapons.Ranged
{
    public class TwistedShot : ModItem
    {
        public float rotation = -MathHelper.PiOver4;
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Ranged;
            Item.damage = 130;
            Item.knockBack = 0.5f;
            Item.crit = 15;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.shootSpeed = 3f;

            Item.useTime = 14;
            Item.useAnimation = 14;
            Item.autoReuse = true;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAmmo = AmmoID.Arrow;
            Item.UseSound = SoundID.Item5;

            Item.width = 25;
            Item.height = 25;

            Item.noMelee = true;

            Item.value = Item.sellPrice(gold: 15);
            Item.rare = ModContent.RarityType<ModdedPurple>();

        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity * 2.25f, ModContent.ProjectileType<GodlessTendril>(), damage, knockback, player.whoAmI, 1f, 0f, Main.rand.NextFloat(1f, 2f));
            Projectile.NewProjectile(source, position, velocity * 2.25f, ModContent.ProjectileType<GodlessTendril>(), damage, knockback, player.whoAmI, -1f, 0f, Main.rand.NextFloat(1f, 2f));

            return false;
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
        }

        public override void AddRecipes()
        {
            CreateRecipe()
             .AddIngredient(ModContent.ItemType<GalaxyAlloy>(), 3)
             .AddIngredient(ModContent.ItemType<VoidFragment>(), 8)
             .AddTile(TileID.LunarCraftingStation)
             .Register();
        }
    }
}