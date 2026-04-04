using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Projectiles.Weapons.Melee;

namespace TranscendenceMod.Items.Weapons.Melee
{
    public class GalaxySaber : ModItem
    {
        public int proj = ModContent.ProjectileType<GalaxySaberProj>();
        public int proj2 = ModContent.ProjectileType<GalaxySaberShred>();
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 70;
            Item.DamageType = DamageClass.Melee;
            Item.width = 24;
            Item.height = 35;
            Item.useTime = 13;
            Item.useAnimation = 13;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 3f;
            Item.value = Item.sellPrice(gold: 5);
            Item.rare = ItemRarityID.LightPurple;
            Item.UseSound = SoundID.Item15 with { MaxInstances = 0 };
            Item.crit = 20;
            Item.shoot = proj;
            Item.shootSpeed = 15;
            Item.useTurn = true;
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            TranscendenceUtils.DrawItemGlowmask(Item, rotation, scale, Texture);
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override bool CanShoot(Player player)
        {
            return player.altFunctionUse == 2 && player.ownedProjectileCounts[proj] == 0;
        }
        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!target.CanBeChasedBy())
                return;

            Projectile.NewProjectile(player.GetSource_OnHit(target), target.Center + Vector2.One.RotatedByRandom(360) * 190,
                Main.rand.NextVector2CircularEdge(1, 1), proj2, (int)(Item.damage * 0.7f), Item.knockBack * 0.3f, player.whoAmI, 0, Main.rand.Next(1, 4));
        }
        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[proj] == 0;
        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            Lighting.AddLight(new Vector2(hitbox.X, hitbox.Y), 0.6f, 0.2f, 0.4f);
        }
    }
}