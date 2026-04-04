using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Items.Materials.MobDrops;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Miscannellous.Rarities;
using TranscendenceMod.Projectiles.Weapons.Melee;

namespace TranscendenceMod.Items.Weapons.Melee
{
    public class SunBlade : ModItem
    {
        int projectile = ModContent.ProjectileType<SunBladeProj>();
        int Timer;

        public override void SetStaticDefaults() => CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        public override void SetDefaults()
        {
            Item.damage = 125;
            Item.DamageType = DamageClass.Melee;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 6;
            Item.useAnimation = 12;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 3;
            Item.shoot = projectile;
            Item.crit = 10;
            Item.shootSpeed = 20f;
            Item.value = Item.sellPrice(gold: 25);
            Item.rare = ModContent.RarityType<Brown>();
            Item.UseSound = SoundID.Item1;
            Item.useTurn = true;
        }
        public override bool CanShoot(Player player)
        {
            if (++Timer > 6)
            {
                Timer = 0;
                return true;
            }
            else return false;
        }
        public override void UpdateInventory(Player player)
        {
            Timer++;
        }
        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Daybreak, 180);
            TranscendenceUtils.DustRing(target.Center, 10, DustID.SparksMech, 4, Color.White, 2);
            Projectile.NewProjectile(player.GetSource_FromAI(), target.Center, Vector2.Zero, ProjectileID.SolarWhipSwordExplosion, Item.damage * 5, 4, player.whoAmI, 0, 3);
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.DD2SquireBetsySword)
            .AddIngredient(ItemID.SolarEruption)
            .AddIngredient(ItemID.FragmentSolar, 15)
            .AddIngredient(ModContent.ItemType<PoseidonsTide>(), 4)
            .AddIngredient(ModContent.ItemType<SoulOfKnight>(), 4)
            .AddTile(TileID.LunarCraftingStation)
            .Register();
        }
    }
}