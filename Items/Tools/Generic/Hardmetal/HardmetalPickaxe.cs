using Terraria;
using Terraria.GameContent.Creative;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Items.Tools.Generic.Hardmetal
{
    public class HardmetalPickaxe : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 18;
            Item.DamageType = DamageClass.Melee;

            Item.width = 32;
            Item.height = 32;

            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Swing;

            Item.knockBack = 5;
            Item.value = Item.sellPrice(silver: 37, copper: 50);
            Item.rare = ItemRarityID.Green;

            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;

            Item.pick = 59;
            Item.useTurn = true;
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (target.life < (target.lifeMax * 0.33f))
            {
                int coin = Main.rand.NextBool(5) ? ItemID.SilverCoin : ItemID.CopperCoin;
                Item.NewItem(player.GetSource_OnHit(Item), target.getRect(), coin, Main.rand.Next(1, 5));
                TranscendenceUtils.ParticleOrchestra(ParticleOrchestraType.SilverBulletSparkle, player.itemLocation, player.whoAmI);
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
             .AddIngredient(ModContent.ItemType<HardmetalBar>(), 15)
             .AddRecipeGroup(nameof(ItemID.SilverBar), 10)
             .AddTile(TileID.Anvils)
             .Register();
        }
    }
}
