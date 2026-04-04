using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace TranscendenceMod.Items.Weapons.Magic
{
    public class DevWeapon : ModItem
    {
        public int rot;
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Magic;
            Item.knockBack = 4;

            Item.width = 18;
            Item.height = 24;

            Item.useTime = 1;
            Item.useAnimation = 1;
            Item.reuseDelay = 3;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.autoReuse = true;
            Item.useTurn = true;

            Item.value = Item.sellPrice(platinum: int.MaxValue);
            Item.rare = -12;
            Item.shoot = ProjectileID.PhantasmalDeathray;
            Item.shootSpeed = 1;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-4f, 0f);
        }
        public override bool? UseItem(Player player)
        {
            for (int n = 0; n < Main.maxNPCs; n++)
            {
                NPC npc = Main.npc[n];
                if (npc.Distance(Main.MouseWorld) < 220 && !npc.dontTakeDamage)
                {
                    if (Main.mouseRight)
                    {
                        npc.Center = Main.MouseWorld;
                        npc.velocity = Vector2.Zero;
                    }
                    npc.StrikeNPC(new NPC.HitInfo() { Damage = npc.lifeMax / 20 }, false, false);
                }
            }
            for (int i = 0; i < 8; i++)
            {
                int d = Dust.NewDust(Main.MouseWorld + Vector2.One.RotatedBy(MathHelper.ToRadians(++rot * 3) + i) * 40, 1, 1,
                    DustID.Torch, 0, 0, 0, default, 3);
                Main.dust[d].noGravity = true;
            }
            return base.UseItem(player);
        }
    }
}