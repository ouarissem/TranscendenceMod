using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace TranscendenceMod.Items.Tools
{
    public class JungleShovel : ModItem
    {
        public int Timer;
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 18;
            Item.DamageType = DamageClass.Melee;
            Item.width = 35;
            Item.height = 35;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 4;
            Item.value = Item.sellPrice(gold: 1);
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.useTurn = true;
        }
        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            if (++Timer == 10)
            {
                int range = 2 * 16;
                for (int x = -range; x < range; x++)
                {
                    for (int y = -range; y < range; y++)
                    {
                        Vector2 pos = Main.MouseWorld + new Vector2(x, y);
                        pos /= 16;
                        Tile tile = Main.tile[(int)pos.X, (int)pos.Y];
                        if ((tile.TileType == TileID.LivingMahoganyLeaves || tile.TileType == TileID.Silt || tile.TileType == TileID.HoneyBlock || tile.TileType == TileID.CrispyHoneyBlock || tile.TileType == TileID.Hive || tile.TileType == TileID.Mud || tile.TileType == TileID.CorruptJungleGrass || tile.TileType == TileID.CrimsonJungleGrass || tile.TileType == TileID.JungleGrass)
                            && player.Distance(Main.MouseWorld) < (8 * 16))
                        {
                            player.PickTile((int)pos.X, (int)pos.Y, 15);
                        }
                    }
                }
            }
            if (player.ItemAnimationEndingOrEnded)
                Timer = 0;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.BeeWax, 15)
            .AddIngredient(ItemID.JungleSpores, 12)
            .AddIngredient(ItemID.RichMahogany, 10)
            .AddTile(TileID.Anvils)
            .Register();
        }
    }
}
