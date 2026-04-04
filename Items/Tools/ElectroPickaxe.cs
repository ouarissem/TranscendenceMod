using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Items.Materials.MobDrops;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Miscannellous.Rarities;

namespace TranscendenceMod.Items.Tools
{
    public class ElectroPickaxe : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        int projectile = ModContent.ProjectileType<ElectroPickaxe_Proj>();
        public override void SetDefaults()
        {
            Item.damage = 150;
            Item.DamageType = DamageClass.Melee;
            Item.knockBack = 2;

            Item.width = 37;
            Item.height = 37;

            Item.useTime = 8;
            Item.useAnimation = 8;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noUseGraphic = true;
            Item.channel = true;
            Item.noMelee = true;

            Item.value = Item.sellPrice(gold: 10);
            Item.rare = ModContent.RarityType<ModdedPurple>();
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shoot = projectile;
            Item.pick = 225;
            Item.useTurn = true;
            Item.tileBoost = 4;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float sizeMult = player.GetAdjustedItemScale(Item);
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, 0, 0, sizeMult);
            return false;
        }
        public override bool CanShoot(Player player)
        {
            return base.CanShoot(player);
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            TranscendenceUtils.DrawItemGlowmask(Item, rotation, scale, Texture);
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.LaserDrill)
            .AddIngredient(ModContent.ItemType<GalaxyAlloy>(), 2)
            .AddIngredient(ModContent.ItemType<Lightning>(), 8)
            .AddTile(TileID.MythrilAnvil)
            .Register();
        }
    }
    public class ElectroPickaxe_Proj : ModProjectile
    {
        public override string Texture => "TranscendenceMod/Items/Tools/ElectroPickaxe";
        public float Timer;
        public float ChargeTimer;
        public float RotSpeed = 3;
        public bool boolean;
        public int dir;
        public bool Released;

        public override void SetDefaults()
        {
            Projectile.width = 98;
            Projectile.height = 98;
            Projectile.DamageType = DamageClass.MeleeNoSpeed;

            Projectile.ownerHitCheck = true;
            Projectile.netImportant = true;

            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 10;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.hide = true;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Vector2 pos = Vector2.SmoothStep(Projectile.Center, target.Center, 0.5f);
            TranscendenceUtils.ParticleOrchestra(Terraria.GameContent.Drawing.ParticleOrchestraType.StardustPunch, pos, -1);
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
        }
        public override void OnSpawn(IEntitySource source)
        {
            Timer = -4.2456745f * dir;
            Player player = Main.player[Projectile.owner];
            dir = player.direction;
            Projectile.velocity = new Vector2(dir * 4, -9);
        }
        public override bool? CanDamage() => true;
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float reference = float.NaN;
            if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center,
                Projectile.Center + Projectile.velocity * 5, 8, ref reference))
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

            if (player.HeldItem.type != ModContent.ItemType<ElectroPickaxe>() || player.dead)
                Projectile.Kill();

            Projectile.Center = player.Center + (Projectile.velocity * 6) + new Vector2(12 * Projectile.scale * dir, 32);
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
            Projectile.scale = Projectile.ai[2];

            player.direction = dir;
            player.SetDummyItemTime(2);

            Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(Timer));

            Timer += RotSpeed * 0.775f * dir;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D sprite = ModContent.Request<Texture2D>(Texture).Value;
            Texture2D sprite2 = ModContent.Request<Texture2D>(Texture + "_Glow").Value;
            Vector2 origin = new Vector2(sprite.Width * 0.5f, Projectile.height * 0.5f);

            float rot = Projectile.rotation + (dir > 0 ? 0 : MathHelper.PiOver2);
            SpriteEffects spriteEffects = dir > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            Main.EntitySpriteDraw(sprite, Projectile.Center + Projectile.velocity * 2.85f - Main.screenPosition, null, lightColor, rot, origin,
                Projectile.scale, spriteEffects);

            Main.EntitySpriteDraw(sprite2, Projectile.Center + Projectile.velocity * 2.85f - Main.screenPosition, null, Color.White, rot, origin,
                Projectile.scale, spriteEffects);

            return false;
        }
    }
}
