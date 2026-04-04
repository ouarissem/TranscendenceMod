using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Buffs;
using TranscendenceMod.Dusts;
using TranscendenceMod.Items.Consumables.Placeables.SpaceBiome;
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Items.Materials.MobDrops;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Miscannellous.Rarities;
using TranscendenceMod.Tiles.BigTiles;

namespace TranscendenceMod.Items.Weapons.Melee
{
    public class StellarSpear : ModItem
    {
        int projectile = ModContent.ProjectileType<StellarSpearProj>();

        public override void SetStaticDefaults()
        {
            ItemID.Sets.Spears[Type] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.damage = 185;
            Item.crit = 15;
            Item.DamageType = DamageClass.Melee;

            Item.width = 18;
            Item.height = 18;

            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.reuseDelay = 18;
            Item.useStyle = ItemUseStyleID.Shoot;

            Item.knockBack = 3f;
            Item.shoot = projectile;
            Item.shootSpeed = 12;

            Item.value = Item.sellPrice(gold: 5);
            Item.rare = ModContent.RarityType<ModdedPurple>();

            Item.UseSound = SoundID.Item1;
            Item.noUseGraphic = true;
            Item.noMelee = true;
        }
        public override bool CanShoot(Player player)
        {
            if (player.ownedProjectileCounts[projectile] > 0)
                return false;
            else return true;
        }
        public override void ModifyShootStats(Player player, ref Vector2 position,
            ref Vector2 velocity, ref int type, ref int damage, ref float knockback) => type = ProjectileID.None;
        public override bool CanUseItem(Player player)
        {
            if (player.ownedProjectileCounts[projectile] > 0)
                return false;
            else return base.CanUseItem(player);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.ownedProjectileCounts[projectile] < 1)
            {
                float sizeMult = player.GetAdjustedItemScale(Item);
                Projectile.NewProjectile(source, position, velocity, projectile, damage, knockback, player.whoAmI, 0, sizeMult);
            }
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
             .AddIngredient(ModContent.ItemType<GalaxyAlloy>(), 3)
             .AddIngredient(ModContent.ItemType<AetherRootItem>(), 6)
             .AddIngredient(ModContent.ItemType<CrystalItem>(), 8)
             .AddTile(ModContent.TileType<ShimmerAltar>())
             .Register();
        }
    }
    public class StellarSpearProj : ModProjectile
    {
        public float vel = 0;
        public float RotSpeed = 0.5f;
        public float Timer;
        public int dir;
        public float thickness;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.NoMeleeSpeedVelocityScaling[Type] = true;
        }
        public override void SetDefaults()
        {
            Projectile.width = 124;
            Projectile.height = 136;
            Projectile.DamageType = DamageClass.MeleeNoSpeed;

            Projectile.ownerHitCheck = true;
            Projectile.netImportant = true;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;

            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 42;
            Projectile.extraUpdates = 1;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            /*for (int i = 1; i < 4; i++)
            {
                Dust.NewDust(Projectile.Center + Projectile.velocity * Projectile.ai[2] * i * 0.75f, 1, 1, ModContent.DustType<ArenaDust>(),
                    Main.rand.NextFloat(), Main.rand.NextFloat(), 0, Color.BlueViolet, 1.5f);

                Dust.NewDust(Projectile.Center + Projectile.velocity * Projectile.ai[2] * i, 1, 1, ModContent.DustType<NovaDust>(),
                    Main.rand.NextFloat(), Main.rand.NextFloat(), 0, Color.BlueViolet, 2);
            }*/

            target.AddBuff(ModContent.BuffType<SpaceDebuff>(), 60);
        }
        public override void OnSpawn(IEntitySource source)
        {
            Player player = Main.player[Projectile.owner];
            dir = player.direction;
            Timer = 1.25f;
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.hide = true;
            player.heldProj = Projectile.whoAmI;
            player.direction = dir;

            if (player.HeldItem.type != ModContent.ItemType<StellarSpear>() || player.dead)
                Projectile.Kill();

            Projectile.Center = player.Center + (Projectile.velocity * vel * Projectile.scale);
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
            Projectile.velocity = Projectile.velocity.RotatedBy(Timer / 270);
            Projectile.scale = Projectile.ai[1];

            int d = Dust.NewDust(Projectile.Center + Projectile.velocity * 12 * (1 + Projectile.ai[2] * 0.0155f), 1, 1, ModContent.DustType<ExtraTerrestrialDust>(),
                0, 0, 0, Color.White, 1.5f);
            Main.dust[d].noGravity = true;
            Main.dust[d].velocity = Vector2.Zero;

            if (Projectile.ai[2] > 2 && Projectile.ai[2] < 12)
            {
                vel += MathHelper.Lerp(0.05f, 0.7f, Projectile.ai[2] / 12f);
                Timer += Projectile.ai[2] > 6 ? -RotSpeed * 2f : RotSpeed;
            }

            if (Projectile.ai[2] > 14 && Projectile.ai[2] < 20)
                thickness += 0.15f;

            if (Projectile.ai[2] > 24)
            {
                vel -= 0.25f;
                thickness -= 0.1f;
            }
            Projectile.ai[2] += 0.75f;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float reference = float.NaN;
            if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center,
                Projectile.Center + Projectile.velocity * (4 + Projectile.ai[2] * Projectile.scale), 12, ref reference))
            {
                return true;
            }
            else return false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;
            //TranscendenceUtils.RestartSB(spriteBatch, BlendState.Additive, null);

            Texture2D sprite = ModContent.Request<Texture2D>($"{Texture}").Value;
            Texture2D sprite2 = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/TrailDarkBG").Value;

            for (int i = 1; i < 5; i++)
            {
                Vector2 pos = (Projectile.Center + Projectile.velocity * 2.5f * Projectile.scale * (1 + (vel * 0.25f)) * (1 + (i * 0.35f))) - Main.screenPosition + new Vector2(0f, 8f);
                spriteBatch.Draw(sprite2, new Rectangle((int)(pos.X), (int)(pos.Y), (int)((int)(75 + (vel * 5) * Projectile.scale) * thickness), (int)(300 * Projectile.scale)), null, new Color(5f / 255f, 7f / 255f, 45f / 255f, 0f) * (i * 2),
                    Projectile.rotation + MathHelper.ToRadians(45), sprite2.Size() * 0.5f, SpriteEffects.None, 0);

                spriteBatch.Draw(sprite2, new Rectangle((int)(pos.X), (int)(pos.Y), (int)((int)(25 + (vel * 5) * Projectile.scale) * thickness), (int)(200 * Projectile.scale)), null, new Color(1f, 25f / 255f, 7f / 255f, 0f) * (1 - (i * 0.1f)),
                    Projectile.rotation + MathHelper.ToRadians(45), sprite2.Size() * 0.5f, SpriteEffects.None, 0);

            }

            //TranscendenceUtils.RestartSB(spriteBatch, BlendState.AlphaBlend, null);

            bool boolean = Main.player[Projectile.owner].direction == 1;

            Main.EntitySpriteDraw(sprite, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation + (boolean ? 0 : MathHelper.PiOver2), sprite.Size() * 0.5f,
              Projectile.scale, boolean ? SpriteEffects.None : SpriteEffects.FlipHorizontally);

            return false;
        }
    }
}