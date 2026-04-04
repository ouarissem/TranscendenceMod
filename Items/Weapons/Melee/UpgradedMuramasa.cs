using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Dusts;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Miscannellous.Rarities;
using TranscendenceMod.Projectiles.Weapons.Melee;

namespace TranscendenceMod.Items.Weapons.Melee
{
    public class UpgradedMuramasa : ModItem
    {
        int projectile = ModContent.ProjectileType<UpgradedMuramasaProj>();
        public int cycle = 1;
        public int timer;

        public float MuramasaColorFadeTimer;
        public float MuramasaColorFade = 0.02f;
        public Color MuramasaBlue;

        public int DashTimer;
        public int NoSword;

        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = true;
        }
        public override void SetDefaults()
        {
            Item.damage = 175;
            Item.crit = 15;
            Item.DamageType = DamageClass.Melee;

            Item.width = 22;
            Item.height = 28;

            Item.useTime = 16;
            Item.useAnimation = 16;
            Item.useStyle = ItemUseStyleID.Shoot;

            Item.knockBack = 2f;
            Item.shoot = projectile;
            Item.shootSpeed = 6f;

            Item.value = Item.sellPrice(gold: 50);
            Item.rare = ModContent.RarityType<Brown>();
            Item.GetGlobalItem<ModifiersItem>().BlacksmithGiantHandleAllowed = true;


            Item.UseSound = new SoundStyle("TranscendenceMod/Miscannellous/Assets/Sounds/Weapons/MuramasaSwing")
            {
                Volume = 1.85f,
                MaxInstances = 0
            };
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
            float sizeMult = player.GetAdjustedItemScale(Item);
            if (player.ownedProjectileCounts[projectile] < 1)
            {
                player.GetModPlayer<TranscendencePlayer>().MuramasaTime = 45;
                cycle = -cycle;
                int p = Projectile.NewProjectile(source, position, velocity, projectile, damage, knockback, -1, sizeMult, cycle, 12);
                Main.projectile[p].extraUpdates += (int)(player.GetAttackSpeed(DamageClass.Melee) * 3f);
                DashTimer = 0;
            }
            return false;
        }
         
        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            MuramasaColorFadeTimer += MuramasaColorFade;
            if (MuramasaColorFadeTimer > 1 || MuramasaColorFadeTimer < 0) MuramasaColorFade = -MuramasaColorFade;
            MuramasaBlue = Color.Lerp(Color.DeepSkyBlue, Color.DarkBlue, MuramasaColorFadeTimer);

            Texture2D sprite = ModContent.Request<Texture2D>($"{Texture}").Value;
            for (int i = 0; i < 4; i++)
            {
                float pi = MathHelper.TwoPi * i / 4;
                float rot = pi += MathHelper.ToRadians(++timer + 4);

                Vector2 pos = position + Vector2.One.RotatedBy(rot) * 3;
                Main.EntitySpriteDraw(sprite, pos, null, MuramasaBlue * 0.3f, 0, sprite.Size() * 0.5f, 0.5f, SpriteEffects.None);
            }
            return base.PreDrawInInventory(spriteBatch, position, frame, drawColor, itemColor, origin, scale);
        }
    }
    public class UpgradedMuramasaProj : ModProjectile
    {
        public float vel = 0;
        public float Timer;
        public float SparkleTimer;
        public float RotSpeed = 0.5f;
        public float ScaleAmount = 0.1f;
        public bool boolean;
        public float armRot;
        int projectile = ModContent.ProjectileType<MuramasaPlusShred>();

        public override string Texture => "TranscendenceMod/Items/Weapons/Melee/UpgradedMuramasa";
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 30;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.width = 74;
            Projectile.height = 74;
            Projectile.DamageType = DamageClass.Melee;

            Projectile.ownerHitCheck = true;
            Projectile.netImportant = true;

            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 38;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.ArmorPenetration = 25;

            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];

            SoundEngine.PlaySound(SoundID.NPCHit18, target.Center);

            int p = Projectile.NewProjectile(Projectile.GetSource_OnHit(target), target.Center + Vector2.One.RotatedByRandom(360) * 150, Projectile.DirectionTo(target.Center) * 4,
            projectile, Projectile.damage, 1, player.whoAmI, 0, target.whoAmI);
            Main.projectile[p].velocity = Main.projectile[p].DirectionTo(target.Center) * 10;
        }
        public override void OnSpawn(IEntitySource source)
        {
            Timer = -5f * Projectile.ai[1];
            RotSpeed *= Projectile.ai[1];
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float reference = float.NaN;
            if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center,
                Projectile.Center + Projectile.velocity * (Projectile.ai[2] * (Projectile.scale * 1.75f)) * 0.425f, 128, ref reference))
            {
                return true;
            }
            else return false;
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            player.heldProj = Projectile.whoAmI;

            // Hitbox test
            //Dust.QuickDustLine(Projectile.Center,
                //Projectile.Center + Projectile.velocity * (Projectile.ai[2] * (Projectile.scale * 1.75f)) * 0.425f, 20f, Color.Red);

            if (player.HeldItem.type != ModContent.ItemType<UpgradedMuramasa>() || player.dead)
                Projectile.Kill();

            Projectile.Center = player.Center + (Projectile.velocity * vel * 3f);
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;

            Projectile.velocity = Projectile.velocity.RotatedBy(Timer / 15f);
            ScaleAmount = Projectile.ai[0] / 7f;

            if (Projectile.timeLeft > 36)
            {
                boolean = Projectile.Center.X > player.Center.X;
            }

            Vector2 sparklePos = Projectile.Center + Projectile.velocity * Projectile.scale * 5.35f;
            Projectile.noEnchantmentVisuals = true;

            if (Projectile.timeLeft < 19 && Projectile.timeLeft > 9)
            {
                for (int i = 0; i < 4; i++)
                    Projectile.EmitEnchantmentVisualsAt(sparklePos - new Vector2(4), 8, 8);
                Dust.NewDustPerfect(sparklePos, ModContent.DustType<MuramasaDust>(), Vector2.Zero, 0, new Color(59, 110, 233), 1.25f);
            }

            else
            {
                Timer += RotSpeed;
                vel += 0.25f;
                if (Projectile.timeLeft < 33)
                {
                    Projectile.scale += ScaleAmount;
                }
            }

            if (Projectile.timeLeft < 19)
            {
                vel -= 0.25f;
                Timer -= RotSpeed;
                if (Projectile.scale > 0) Projectile.scale -= ScaleAmount * 1.2f;
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 35;
            ProjectileID.Sets.TrailingMode[Type] = 3;
            Texture2D sprite = ModContent.Request<Texture2D>($"{Texture}").Value;
            Texture2D sprite2 = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/Trail").Value;

            Vector2 origin = new Vector2(sprite.Width * 0.5f, Projectile.height * 0.5f);

            float rot = Projectile.rotation + (boolean ? 0 : MathHelper.PiOver2);
            SpriteEffects spriteEffects = boolean ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            Main.EntitySpriteDraw(sprite, Projectile.Center + Projectile.velocity * 3 * Projectile.scale - Main.screenPosition, null, Color.White, rot, origin,
                Projectile.scale, spriteEffects);
            return false;
        }
    }
}