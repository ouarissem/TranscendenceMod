using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Projectiles.Weapons.Melee;

namespace TranscendenceMod.Items.Weapons.Melee
{
    public class SculptureRupture : ModItem
    {
        int projectile = ModContent.ProjectileType<Sculpture>();
        int Timer;

        public override void SetStaticDefaults() => CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        public override void SetDefaults()
        {
            Item.damage = 35;
            Item.DamageType = DamageClass.Melee;

            Item.width = 32;
            Item.height = 32;

            Item.useTime = 22;
            Item.useAnimation = 22;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 4;

            Item.shoot = ProjectileID.PurificationPowder;
            Item.shootSpeed = 10;

            Item.value = Item.sellPrice(gold: 1);
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Tink;
            Item.useTurn = true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Timer++;
            for (int i = 0; i < 12; i++)
            {
                Vector2 pos2 = Vector2.One.RotatedBy(MathHelper.TwoPi * i / 12f) * 100f;
                Vector2 pos = player.Center + new Vector2(pos2.X, pos2.Y / 2f).RotatedBy(velocity.ToRotation() - MathHelper.PiOver2);
                if (Timer > 4)
                {
                    Projectile.NewProjectile(source, pos, velocity, projectile, damage, knockback, player.whoAmI, 0, i, TranscendenceWorld.Timer);
                }
                else
                {
                    Dust d = Dust.NewDustPerfect(pos, DustID.Clay, Vector2.Zero, 0, default, 2);
                    d.noGravity = true;
                }
            }
            if (Timer > 4)
                Timer = 0;
            return false;
        }
        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Texture2D sprite = ModContent.Request<Texture2D>($"Terraria/Images/UI/Achievement_InnerPanelBottom").Value;
            Texture2D sprite2 = ModContent.Request<Texture2D>($"Terraria/Images/UI/GolfSwingBarFill").Value;
            spriteBatch.Draw(sprite, new Rectangle((int)position.X - 34, (int)position.Y - 23, 16, 55), Color.White);
            spriteBatch.Draw(sprite2, new Rectangle((int)position.X - 31, (int)position.Y - 20, 10, (int)MathHelper.Lerp(0, 48f, Timer / 4f)), new Rectangle(Timer * 8, 4, 6, 6), Color.Orange);
        }
        public override void AddRecipes()
        {
            CreateRecipe()
             .AddIngredient(ItemID.ClayBlock, 20)
             .AddIngredient(ItemID.RedBrick, 15)
             .AddIngredient(ItemID.MudBlock, 10)
             .AddIngredient(ItemID.HellstoneBar, 5)
             .AddTile(TileID.Hellforge)
             .Register();
        }
    }
}