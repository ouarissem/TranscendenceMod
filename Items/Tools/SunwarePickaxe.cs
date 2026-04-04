using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Projectiles.Equipment.Tools;

namespace TranscendenceMod.Items.Tools
{
    public class SunwarePickaxe : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 35;
            Item.DamageType = DamageClass.Melee;
            Item.width = 35;
            Item.height = 35;
            Item.useTime = 13;
            Item.useAnimation = 13;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 7;
            Item.value = Item.sellPrice(gold: 5);
            Item.rare = ItemRarityID.Pink;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.pick = 190;
            Item.useTurn = true;
        }

        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            int proj = ModContent.ProjectileType<SunwarePickaxeGuidingLight>();
            if (player.ItemAnimationJustStarted && player.ownedProjectileCounts[proj] == 0)
            {
                Vector2 pos = Main.MouseWorld;
                pos /= 16;
                Tile tile = Main.tile[(int)pos.X, (int)pos.Y];
                if (tile.HasTile && (player.Distance(Main.MouseWorld) < ((5 + Item.tileBoost) * 16) || Main.SmartCursorShowing && player.Distance(new Vector2(Main.SmartCursorX * 16, Main.SmartCursorY * 16)) < ((5 + Item.tileBoost) * 16)))
                {
                    Projectile.NewProjectile(player.GetSource_FromAI(), Main.SmartCursorShowing ? new Vector2(Main.SmartCursorX * 16, Main.SmartCursorY * 16) : new Vector2(pos.X, pos.Y).ToWorldCoordinates(), Vector2.Zero, proj, 0, 0, player.whoAmI);
                }
            }
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<SunburntAlloy>(), 10);
            recipe.AddIngredient(ItemID.SunplateBlock, 12);
            recipe.AddIngredient(ItemID.Feather, 12);
            recipe.AddTile(TileID.SkyMill);
            recipe.Register();
        }
    }
}
