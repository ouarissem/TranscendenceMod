using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Items.Materials.MobDrops;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Miscannellous.Rarities;
using TranscendenceMod.Projectiles.Weapons;

namespace TranscendenceMod.Items.Weapons
{
    public class StarMegaLauncher : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.DamageType = ModContent.GetInstance<MagicRangedDamageClass>();
            Item.damage = 100;
            Item.mana = 10;
            Item.useAmmo = AmmoID.Bullet;
            Item.knockBack = 2;

            Item.width = 42;
            Item.height = 20;

            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.useAmmo = ModContent.ItemType<Starlite>();

            Item.value = Item.sellPrice(gold: 25);
            Item.rare = ModContent.RarityType<ModdedPurple>();
            Item.shoot = ProjectileID.PurificationPowder;
            Item.shootSpeed = 20f;
            Item.UseSound = SoundID.Item158;
        }
        public override bool CanUseItem(Player player)
        {
            if (player.statMana > Item.mana && player.HasAmmo(Item))
            {
                return base.CanUseItem(player);
            }
            else return false;
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            TranscendenceUtils.DrawItemGlowmask(Item, rotation, scale, Texture);
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            type = ModContent.ProjectileType<StarMegaLauncherStar>();
            base.ModifyShootStats(player, ref position, ref velocity, ref type, ref damage, ref knockback);
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-8f, -4f);
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<StarLauncher>())
            .AddIngredient(ModContent.ItemType<StarGun>())
            .AddIngredient(ItemID.FragmentSolar, 10)
            .AddIngredient(ItemID.FragmentVortex, 10)
            .AddIngredient(ItemID.FragmentNebula, 10)
            .AddIngredient(ItemID.FragmentStardust, 10)
            .AddTile(TileID.LunarCraftingStation)
            .Register();
        }
    }
}