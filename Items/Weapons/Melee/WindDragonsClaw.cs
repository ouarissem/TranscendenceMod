using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Buffs.Items.Weapons;
using TranscendenceMod.Dusts;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Miscannellous.Rarities;
using TranscendenceMod.Projectiles;
using TranscendenceMod.Projectiles.Weapons.Melee;

namespace TranscendenceMod.Items.Weapons.Melee
{
    public class WindDragonsClaw : ModItem
    {
        public int boomerang = ModContent.ProjectileType<WindDragonClawmerang>();

        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Melee;
            Item.damage = 161;

            Item.knockBack = 1.8f;
            Item.width = 16;
            Item.height = 28;

            Item.shoot = boomerang;
            Item.shootSpeed = 6;

            Item.useTime = 16;
            Item.useAnimation = 16;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item1;

            Item.value = Item.sellPrice(gold: 15);
            Item.rare = ModContent.RarityType<Brown>();
            Item.crit = 15;
        }
        public override bool CanShoot(Player player) => player.ownedProjectileCounts[boomerang] <= 16;
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            Projectile.NewProjectile(player.GetSource_ItemUse(Item), position, velocity, type, (int)(damage * 0.75f), knockback, player.whoAmI, 0, 1);
            Projectile.NewProjectile(player.GetSource_ItemUse(Item), position, velocity, type, (int)(damage * 0.75f), knockback, player.whoAmI, 0, -1);
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            Vector2 pos = new Vector2(hitbox.X, hitbox.Y);

            Dust.NewDustPerfect(pos, ModContent.DustType<ArenaDust>(),
                new Vector2(10 * player.direction, 6), 0, Color.CornflowerBlue * 0.4f, 1);
        }

        public override bool? CanMeleeAttackCollideWithNPC(Rectangle meleeAttackHitbox, Player player, NPC target)
        {
            if (target.Hitbox.Distance(new Vector2(meleeAttackHitbox.X, meleeAttackHitbox.Y)) < (84 * Item.scale))
            {
                return true;
            }

            else return false;
        }
    }
}