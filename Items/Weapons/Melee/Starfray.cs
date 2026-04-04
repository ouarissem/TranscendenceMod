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
    public class Starfray : ModItem
    {
        int projectile = ModContent.ProjectileType<Starfray_Proj>();

        public override void SetStaticDefaults() => CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        public override void SetDefaults()
        {
            Item.damage = 150;
            Item.DamageType = DamageClass.Melee;
            Item.width = 40;
            Item.height = 40;

            Item.useTime = 8;
            Item.useAnimation = 8;
            Item.useStyle = ItemUseStyleID.Swing;

            Item.knockBack = 1f;
            Item.crit = 10;

            Item.value = Item.sellPrice(gold: 15);
            Item.rare = ModContent.RarityType<Brown>();
            Item.UseSound = SoundID.Item1;
            Item.useTurn = true;
        }
        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {

            Vector2 pos = new Vector2(Main.MouseWorld.X, player.Center.Y) - new Vector2(0, 1000);
            Projectile.NewProjectile(player.GetSource_FromThis(), pos, new Vector2(pos.DirectionTo(target.Center).X * 5f + Main.rand.NextFloat(-0.5f, 0.5f), 5f), projectile,
                Item.damage / 3, Item.knockBack / 2f, player.whoAmI);
            target.AddBuff(BuffID.Daybreak, 180);

        }
    }
}