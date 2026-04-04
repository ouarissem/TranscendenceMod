using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Items.Weapons.Melee
{
    public class BaseballBat : ModItem
    {
        int projectile = ModContent.ProjectileType<BaseballBatProj>();
        public override void SetStaticDefaults() => CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        public override void SetDefaults()
        {
            Item.damage = 17;
            Item.DamageType = DamageClass.Melee;

            Item.width = 24;
            Item.height = 24;

            Item.useTime = 17;
            Item.useAnimation = 17;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 10f;

            Item.shoot = projectile;

            Item.value = Item.sellPrice(silver: 50);
            Item.rare = ItemRarityID.Blue;

            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.channel = true;
        }
        public override bool CanShoot(Player player)
        {
            return player.ownedProjectileCounts[projectile] == 0;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddRecipeGroup(RecipeGroupID.Wood, 40)
                .AddTile(TileID.HeavyWorkBench)
                .Register();
        }
    }
    internal class BaseballBatProj : ModProjectile
    {
        public override string Texture => "TranscendenceMod/Items/Weapons/Melee/BaseballBat";
        public float Timer;
        public float ChargeTimer;
        public float RotSpeed = 3;
        public bool boolean;
        public int dir;
        public bool Released;

        public override void SetDefaults()
        {
            Projectile.width = 42;
            Projectile.height = 42;
            Projectile.DamageType = DamageClass.MeleeNoSpeed;

            Projectile.ownerHitCheck = true;
            Projectile.netImportant = true;

            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 5;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Vector2 pos = Vector2.SmoothStep(Projectile.Center, target.Center, 0.5f);
            SoundEngine.PlaySound(SoundID.Item178, pos);
            TranscendenceUtils.ParticleOrchestra(Terraria.GameContent.Drawing.ParticleOrchestraType.Keybrand, pos, -1);
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.Knockback *= (ChargeTimer / 2);
        }
        public override void OnSpawn(IEntitySource source)
        {
            SoundEngine.PlaySound(SoundID.Item1, Projectile.Center);
            dir = Main.player[Projectile.owner].direction;
            Timer = 1.8f * dir;
            Projectile.velocity = new Vector2(Main.player[Projectile.owner].direction * 3, -5);
        }
        public override bool? CanDamage() => Released;
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float reference = float.NaN;
            if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center,
                Projectile.Center + Projectile.velocity * 5, 8, ref reference) && Released)
            {
                return true;
            }
            else return false;
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            player.heldProj = Projectile.whoAmI;
            player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, (player.MountedCenter - Projectile.Center).ToRotation() + MathHelper.PiOver2);

            if (player.HeldItem.type != ModContent.ItemType<BaseballBat>() || player.dead)
                Projectile.Kill();

            Projectile.Center = player.Center + (Projectile.velocity * 2);
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
            player.ChangeDir(dir);

            if (player.channel && !Released)
            {
                Projectile.timeLeft = 15;
                if (ChargeTimer < 5)
                {
                    ChargeTimer += 0.075f;
                    Projectile.velocity = Projectile.velocity.RotatedBy(ChargeTimer / -130 * dir);
                }
                return;
            }

            Released = true;
            Projectile.velocity = Projectile.velocity.RotatedBy(Timer / 25);

            Timer += RotSpeed * 0.25f * dir;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D sprite = ModContent.Request<Texture2D>($"{Texture}").Value;
            Vector2 origin = new Vector2(sprite.Width * 0.5f, Projectile.height * 0.5f);

            float rot = Projectile.rotation + (boolean ? 0 : MathHelper.PiOver2);
            SpriteEffects spriteEffects = boolean ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            Main.EntitySpriteDraw(sprite, Projectile.Center + Projectile.velocity * 2.85f - Main.screenPosition, null, lightColor, rot, origin,
                Projectile.scale, spriteEffects);
            return false;
        }
    }
}