using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Projectiles.Equipment;

namespace TranscendenceMod.Items.Consumables.SuperBomb
{
    public class SuperBomb_Brick_Sticky : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 5;
        }
        public override void SetDefaults()
        {
            Item.width = 14;
            Item.height = 14;

            Item.shoot = ModContent.ProjectileType<SuperBombProj_Brick_Sticky>();
            Item.shootSpeed = 11;
            Item.rare = ItemRarityID.LightRed;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 22;
            Item.useAnimation = 22;
            Item.noUseGraphic = true;
            Item.value = Item.sellPrice(gold: 3);

            Item.consumable = true;
            Item.maxStack = 9999;
            Item.autoReuse = true;
        }
        public override bool AltFunctionUse(Player player) => true;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, 0, player.altFunctionUse == 2 ? 1 : 0);
            return false;
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            TranscendenceUtils.DrawItemGlowmask(Item, rotation, scale, "TranscendenceMod/Items/Consumables/SuperBomb/SuperBomb");
        }
        public override void AddRecipes()
        {
            Recipe myth = CreateRecipe();
            myth.AddIngredient(ItemID.DirtStickyBomb, 3);
            myth.AddIngredient(ItemID.RedBrick, 8);
            myth.AddIngredient(ItemID.SandBlock, 12);
            myth.AddIngredient(ItemID.MythrilBar, 4);
            myth.AddTile(TileID.MythrilAnvil);
            myth.Register();

            Recipe orichal = CreateRecipe();
            orichal.AddIngredient(ItemID.DirtStickyBomb, 3);
            orichal.AddIngredient(ItemID.RedBrick, 8);
            orichal.AddIngredient(ItemID.SandBlock, 12);
            orichal.AddIngredient(ItemID.OrichalcumBar, 4);
            orichal.AddTile(TileID.MythrilAnvil);
            orichal.Register();
        }
    }
}
