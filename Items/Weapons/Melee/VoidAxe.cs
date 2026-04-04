using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Items.Consumables.Placeables.SpaceBiome;
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Miscannellous.Rarities;
using TranscendenceMod.Tiles.BigTiles;

namespace TranscendenceMod.Items.Weapons.Melee
{
    public class VoidAxe : ModItem
    {
        int projectile = ModContent.ProjectileType<VoidAxeProj>();
        public override void SetStaticDefaults() => CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        public override void SetDefaults()
        {
            Item.damage = 225;
            Item.DamageType = DamageClass.Melee;

            Item.width = 42;
            Item.height = 34;
            Item.crit = 30;

            Item.useTime = 45;
            Item.useAnimation = 45;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 4;

            Item.shoot = projectile;

            Item.value = Item.sellPrice(gold: 25);
            Item.rare = ModContent.RarityType<ModdedPurple>();

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
            .AddIngredient(ModContent.ItemType<SpikedBaseballBat>())
            .AddIngredient(ModContent.ItemType<CrystalItem>(), 25)
            .AddIngredient(ModContent.ItemType<GalaxyAlloy>(), 3)
            .AddTile(ModContent.TileType<ShimmerAltar>())
            .Register();
        }
    }
    internal class VoidAxeProj : ModProjectile
    {
        public override string Texture => "TranscendenceMod/Items/Weapons/Melee/VoidAxe";
        public float Timer;
        public float ChargeTimer;
        public float RotSpeed = 3;
        public bool boolean;
        public int dir;
        public bool Released;
        public bool HasHitNPC;

        public override void SetDefaults()
        {
            Projectile.width = 68;
            Projectile.height = 54;
            Projectile.DamageType = DamageClass.MeleeNoSpeed;

            Projectile.ownerHitCheck = true;
            Projectile.netImportant = true;

            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 5;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;

            Projectile.noEnchantmentVisuals = true;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Vector2 pos = Vector2.SmoothStep(Projectile.Center, target.Center, 0.5f);
            SoundEngine.PlaySound(SoundID.Item62, pos);
            TranscendenceUtils.ParticleOrchestra(Terraria.GameContent.Drawing.ParticleOrchestraType.Keybrand, pos, -1);
            HasHitNPC = true;
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.FinalDamage *= (0.5f + (ChargeTimer * 1.75f));
            if (ChargeTimer > 9.95) 
            {
                Main.player[Projectile.owner].SetImmuneTimeForAllTypes(60);
            }
        }
        public override void OnSpawn(IEntitySource source)
        {
            SoundEngine.PlaySound(SoundID.Item1, Projectile.Center);
            dir = Main.player[Projectile.owner].direction;
            Timer = 1.8f * dir;
            Projectile.velocity = new Vector2(Main.player[Projectile.owner].direction * 3, -5);
        }
        public override bool? CanDamage() => Released && !HasHitNPC;
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float reference = float.NaN;
            if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center,
                Projectile.Center + Projectile.velocity * 10 * Projectile.scale, 16, ref reference) && Released)
            {
                return !HasHitNPC;
            }
            else return false;
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            player.heldProj = Projectile.whoAmI;
            player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, (player.MountedCenter - Projectile.Center).ToRotation() + MathHelper.PiOver2);

            if (player.HeldItem.type != ModContent.ItemType<VoidAxe>() || player.dead)
                Projectile.Kill();

            Projectile.Center = player.Center + (Projectile.velocity * 2 * Projectile.scale * 2f);
            if (!Released)
                Projectile.Center += new Vector2(player.direction * 10f * (Projectile.scale - 1f), 0f);

            player.ChangeDir(dir);
            player.itemTime = 2;
            player.itemAnimation = 2;
            player.reuseDelay = 10;

            if (player.channel && !Released)
            {
                Projectile.timeLeft = (int)(15 + (ChargeTimer / 3f));
                if (ChargeTimer < 10f)
                {
                    ChargeTimer += 0.1f;
                    if (ChargeTimer > 9.95f) SoundEngine.PlaySound(SoundID.MaxMana, Projectile.Center);
                }
                if (ChargeTimer < 5f)
                {
                    Projectile.velocity = Projectile.velocity.RotatedBy(ChargeTimer / -130 * dir);
                }


                Projectile.scale = (1f + (ChargeTimer / 4f));

                return;
            }

            if (Projectile.timeLeft < 5)
            {
                Projectile.velocity *= 0.9f;
                return;
            }

            Released = true;
            Projectile.velocity = Projectile.velocity.RotatedBy(Timer / 25);

            for (int i = -32; i < 32; i++)
            {
                for (int j = -32; j < 32; j++)
                {
                    Vector2 pos = Projectile.Center + new Vector2(i, j);
                    Tile tile = Framing.GetTileSafely((int)pos.X / 16, (int)pos.Y / 16);

                    if (Projectile.owner == Main.myPlayer && tile.HasTile && Main.player[Projectile.owner] != null && tile != null && Main.tileAxe[tile.TileType] && ++Projectile.ai[1] % 8 == 0)
                    {
                        Main.player[Projectile.owner].PickTile((int)(pos.X / 16), (int)(pos.Y / 16), 180);
                        HasHitNPC = true;
                        Projectile.ai[2]++;
                    }
                }
            }

            Timer += RotSpeed * 0.25f * dir;
        }
        public override void OnKill(int timeLeft)
        {
            Player player = Main.player[Projectile.owner];

            if (ChargeTimer < 2 || HasHitNPC)
                return;

           // Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, new Vector2(dir * (ChargeTimer * 2.5f), ChargeTimer * -2.5f),
               // ModContent.ProjectileType<VoidAxeThrown>(), (int)(Projectile.damage / 10 * (0.1f + (ChargeTimer / 4))), 2, player.whoAmI);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D sprite = ModContent.Request<Texture2D>($"{Texture}").Value;

            SpriteEffects spriteEffects = dir == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            Vector2 pos = Projectile.Center + Projectile.velocity * 4f * Projectile.scale + new Vector2(Released ? dir * Projectile.scale * 2f : 0f, Released ? Projectile.scale * 10f : 0f);
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4 + (dir == -1 ? MathHelper.PiOver2 : 0f);

            TranscendenceUtils.VeryBasicProjOutline(Projectile, Texture, 4f, 1f, 0f, 1f, ChargeTimer / 40f, false, new Rectangle(0, 0, sprite.Width, sprite.Height), spriteEffects, pos);
            TranscendenceUtils.DrawEntity(Projectile, Color.White, Projectile.scale, Texture, Projectile.rotation, pos, null, spriteEffects);

            return false;
        }
    }
}