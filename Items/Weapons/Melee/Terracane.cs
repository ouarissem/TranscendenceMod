using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Projectiles.Weapons.Melee;

namespace TranscendenceMod.Items.Weapons.Melee
{
    public class Terracane : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 150;
            Item.knockBack = 4f;
            Item.crit = 5;
            Item.DamageType = DamageClass.Melee;

            Item.width = 22;
            Item.height = 22;

            Item.useTime = 14;
            Item.useAnimation = 14;
            Item.useStyle = ItemUseStyleID.Swing;

            Item.value = Item.sellPrice(gold: 20);
            Item.rare = ItemRarityID.Yellow;
            Item.UseSound = SoundID.Item60;

            Item.shoot = ModContent.ProjectileType<Terradime>();
            Item.shootSpeed = 8f;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int i = 0; i < 7; i++)
            {
                Projectile.NewProjectile(source, position, velocity.RotatedByRandom(0.275f) * Main.rand.NextFloat(0.725f, 1.75f), type, damage / 2, knockback, player.whoAmI);
            }
            return false;
        }
        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Midas, 300);
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.TaxCollectorsStickOfDoom)
            .AddIngredient(ItemID.PearlwoodSword)
            .AddIngredient(ItemID.BrokenHeroSword)
            .AddTile(TileID.MythrilAnvil)
            .Register();
        }
    }
}