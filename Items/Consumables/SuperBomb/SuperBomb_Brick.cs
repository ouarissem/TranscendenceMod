using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Items.Accessories.Expert;
using TranscendenceMod.Items.Consumables.Boss;
using TranscendenceMod.Items.Materials.LargeRecipes;
using TranscendenceMod.Items.Weapons.Melee;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Projectiles.Equipment;

namespace TranscendenceMod.Items.Consumables.SuperBomb
{
    public class SuperBomb_Brick : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 5;
        }
        public override void SetDefaults()
        {
            Item.width = 14;
            Item.height = 14;

            Item.shoot = ModContent.ProjectileType<SuperBombProj_Brick>();
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
        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            base.PostDrawInInventory(spriteBatch, position, frame, drawColor, itemColor, origin, scale);

            Item block;
            int block2 = -1;

            for (int u = 0; u < Main.LocalPlayer.inventory.Length; u++)
            {
                block = Main.LocalPlayer.inventory[u];
                if (block != null && block.type != ItemID.CopperCoin && block.type != ItemID.SilverCoin && block.type != ItemID.GoldCoin && block.type != ItemID.PlatinumCoin && block.favorited && block.consumable && (block.createTile != -1 || block.createWall != -1) && block.stack > 0 && block2 == -1)
                {
                    block2 = block.type;
                }
            }

            if (block2 != -1)
            {
                Texture2D sprite = TextureAssets.Item[block2].Value;

                for (int i = 0; i < 4; i++)
                {
                    spriteBatch.Draw(sprite, position + new Vector2(4, 10) + Vector2.One.RotatedBy(MathHelper.TwoPi * i / 4f), null, Color.Black);
                }
                spriteBatch.Draw(sprite, position + new Vector2(4, 10), null, Color.White);
            }
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            TranscendenceUtils.DrawItemGlowmask(Item, rotation, scale, "TranscendenceMod/Items/Consumables/SuperBomb/SuperBomb");
        }
        public override void AddRecipes()
        {
            Recipe myth = CreateRecipe();
            myth.AddIngredient(ItemID.DirtBomb, 3);
            myth.AddIngredient(ItemID.RedBrick, 8);
            myth.AddIngredient(ItemID.SandBlock, 12);
            myth.AddIngredient(ItemID.MythrilBar, 4);
            myth.AddTile(TileID.MythrilAnvil);
            myth.Register();

            Recipe orichal = CreateRecipe();
            orichal.AddIngredient(ItemID.DirtBomb, 3);
            orichal.AddIngredient(ItemID.RedBrick, 8);
            orichal.AddIngredient(ItemID.SandBlock, 12);
            orichal.AddIngredient(ItemID.OrichalcumBar, 4);
            orichal.AddTile(TileID.MythrilAnvil);
            orichal.Register();
        }
    }
}
