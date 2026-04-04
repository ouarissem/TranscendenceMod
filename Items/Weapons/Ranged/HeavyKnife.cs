using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Projectiles.Weapons.Ranged;

namespace TranscendenceMod.Items.Weapons.Ranged
{
    public class HeavyKnife : ModItem
    {
        public int Combo = 1;
        public int ComboDeathTimer = 0;
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.damage = 7;
            Item.knockBack = 0.75f;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 15;
            Item.useAnimation = 15;

            Item.width = 10;
            Item.height = 16;

            Item.value = Item.sellPrice(silver: 75);
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item1;

            Item.noMelee = false;
            Item.noUseGraphic = true;

            Item.shoot = ModContent.ProjectileType<HeavyKnifeProj>();
            Item.shootSpeed = 12f;

            Item.DamageType = DamageClass.Ranged;
            Item.autoReuse = true;
        }
        public override void UpdateInventory(Player player)
        {
            ComboDeathTimer++;
            if (ComboDeathTimer > 60)
            {
                Combo = 1;
                ComboDeathTimer = 0;
            }
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            //Main.NewText(Combo);
            if (Combo > 2)
            {
                ComboDeathTimer = 0;

                TranscendenceUtils.ProjectileShotgun(velocity, position, source, type, damage, knockback, 1, 1, 15, -1, 0, 0, 0, 0);
                Combo = 0;
            }
            Combo++;

            return base.Shoot(player, source, position, velocity, type, damage, knockback);
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddRecipeGroup(RecipeGroupID.IronBar, 12);
            recipe.AddIngredient(ItemID.Diamond, 3);
            recipe.AddIngredient(ItemID.Wood, 8);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}