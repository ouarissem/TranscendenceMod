using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Projectiles.Equipment;

namespace TranscendenceMod.Items.Consumables.SuperBomb
{
    public class SuperBomb : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 5;
        }
        public override void SetDefaults()
        {
            Item.width = 14;
            Item.height = 14;

            Item.shoot = ModContent.ProjectileType<SuperBombProj>();
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
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            TranscendenceUtils.DrawItemGlowmask(Item, rotation, scale, Texture);
        }
        public override void AddRecipes()
        {
            Recipe myth = CreateRecipe();
            myth.AddIngredient(ItemID.Bomb, 6);
            myth.AddIngredient(ItemID.ExplosivePowder, 8);
            myth.AddIngredient(ModContent.ItemType<MiningPowder>());
            myth.AddIngredient(ItemID.MythrilBar, 4);
            myth.AddTile(TileID.MythrilAnvil);
            myth.Register();

            Recipe orichal = CreateRecipe();
            orichal.AddIngredient(ItemID.Bomb, 6);
            orichal.AddIngredient(ItemID.ExplosivePowder, 8);
            orichal.AddIngredient(ModContent.ItemType<MiningPowder>());
            orichal.AddIngredient(ItemID.OrichalcumBar, 4);
            orichal.AddTile(TileID.MythrilAnvil);
            orichal.Register();
        }
    }
}
