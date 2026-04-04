using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Items.Weapons.Melee
{
    public class LegendarySword : ModItem
    {
        public int proj = ModContent.ProjectileType<LegendarySword_Proj>();

        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Melee;
            Item.damage = 72;

            Item.knockBack = 5.75f;
            Item.width = 22;
            Item.height = 22;

            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.GetGlobalItem<ModifiersItem>().BlacksmithGiantHandleAllowed = true;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noUseGraphic = true;
            Item.noMelee = true;

            Item.value = Item.sellPrice(gold: 2, silver: 75);
            Item.rare = ItemRarityID.Blue;

            Item.shoot = proj;
            Item.shootSpeed = 3;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            player.GetModPlayer<TranscendencePlayer>().ShieldID = Type;
            player.GetModPlayer<TranscendencePlayer>().LegendarySwordTimer = 10;

            float sizeMult = player.GetAdjustedItemScale(Item);

            int p = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            Main.projectile[p].scale = sizeMult;
            Main.projectile[p].extraUpdates += (int)(player.GetAttackSpeed(DamageClass.Melee) * 3f);

            return false;
        }
        public override bool CanUseItem(Player player) => player.ownedProjectileCounts[proj] == 0;
        public override bool MeleePrefix() => true;
    }
    public class LegendarySword_Proj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 4;
            ProjectileID.Sets.NoMeleeSpeedVelocityScaling[Type] = true; 
        }
        public override void SetDefaults()
        {
            Projectile.width = 64;
            Projectile.height = 64;
            Projectile.DamageType = DamageClass.Melee;

            Projectile.ownerHitCheck = true;
            Projectile.netImportant = true;

            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 20;

            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;

            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.noEnchantmentVisuals = true;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
        }
        public override bool PreDraw(ref Color lightColor)
        {
            TranscendenceUtils.DrawProjAnimated(Projectile, lightColor * (Projectile.ai[1] / 15f), Projectile.scale, $"{Texture}", Projectile.rotation, Projectile.Center, false, false, false);
            return false;
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            player.heldProj = Projectile.whoAmI;

            TranscendenceUtils.AnimateProj(Projectile, 5);
            if (Projectile.Distance(Main.MouseWorld) > 50) Projectile.velocity = Projectile.DirectionTo(Main.MouseWorld);

            Projectile.Center = player.Center + (Projectile.velocity * 20f);
            Projectile.rotation = Projectile.velocity.ToRotation();

            if (++Projectile.ai[1] == 10)
                SoundEngine.PlaySound(SoundID.Item1, Projectile.Center);

            //Dust.NewDustPerfect(Projectile.Center + Projectile.velocity * 20, ModContent.DustType<ArenaDust>(), Projectile.velocity * 12, 0, Color.White, 0.75f);
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float reference = float.NaN;
            if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Main.player[Projectile.owner].Center,
                Projectile.Center + Projectile.velocity * 55, 64, ref reference))
            {
                return true;
            }
            else return false;
        }
    }
}