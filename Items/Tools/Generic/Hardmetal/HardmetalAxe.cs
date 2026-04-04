using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Items.Tools.Generic.Hardmetal
{
    public class HardmetalAxe : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 22;
            Item.DamageType = DamageClass.Melee;

            Item.width = 32;
            Item.height = 32;

            Item.useTime = 17;
            Item.useAnimation = 17;
            Item.useStyle = ItemUseStyleID.Swing;

            Item.knockBack = 2;
            Item.value = Item.sellPrice(silver: 37, copper: 50);
            Item.rare = ItemRarityID.Green;

            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;

            Item.axe = 15;
            Item.useTurn = true;
        }
        public override void ModifyHitNPC(Player player, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (target.life < (target.lifeMax * 0.25f))
            {
                modifiers.SetCrit();
                TranscendenceUtils.ParticleOrchestra(ParticleOrchestraType.SilverBulletSparkle, player.itemLocation, player.whoAmI);
                SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode, target.Center);
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
