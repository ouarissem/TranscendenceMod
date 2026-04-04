using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Projectiles.Weapons.Magic;

namespace TranscendenceMod.Items.Weapons.Magic
{
    public class Eclipse : ModItem
    {
        int proj = ModContent.ProjectileType<SunEclipse>();
        int proj2 = ModContent.ProjectileType<MoonEclipse>();
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Magic;
            Item.damage = 155;
            Item.mana = 15;
            Item.crit = 20;
            Item.knockBack = 4f;
            Item.channel = true;

            Item.width = 12;
            Item.height = 26;

            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;

            Item.autoReuse = true;
            Item.value = Item.sellPrice(gold: 5);
            Item.rare = ItemRarityID.Yellow;
            Item.shoot = proj2;
            Item.shootSpeed = 1;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            tooltips.FirstOrDefault(x => x.Name == "ItemName").OverrideColor = Color.Lerp(Color.Orange, Color.Gray, Main.cursorAlpha);
        }
        public override bool CanShoot(Player player)
        {
            return player.ownedProjectileCounts[proj] == 0 && player.ownedProjectileCounts[proj2] == 0;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, Main.MouseWorld, Vector2.Zero, proj, damage, knockback, player.whoAmI);
            Projectile.NewProjectile(source, Main.MouseWorld, Vector2.Zero, proj2, damage, knockback, player.whoAmI);
            SoundEngine.PlaySound(SoundID.Item60, player.Center);
            return false;
        }
    }
}