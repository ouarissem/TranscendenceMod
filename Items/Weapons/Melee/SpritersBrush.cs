using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Projectiles.Weapons.Melee;

namespace TranscendenceMod.Items.Weapons.Melee
{
    public class SpritersBrush : ModItem
    {
        public int Combo = 0;
        public int ComboDeathTimer = 0;
        int projectile = ModContent.ProjectileType<SpritersBrushProj>();
        public override void SetStaticDefaults() => CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        public override void SetDefaults()
        {
            Item.damage = 65;
            Item.DamageType = DamageClass.Melee;

            Item.width = 24;
            Item.height = 24;

            Item.useTime = 17;
            Item.useAnimation = 17;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 3;

            Item.shoot = projectile;
            Item.shootSpeed = 4;

            Item.value = Item.sellPrice(gold: 5);
            Item.rare = ItemRarityID.Lime;

            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.channel = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<SculptureRupture>())
            .AddIngredient(ItemID.Paintbrush)
            .AddIngredient(ItemID.BlackInk)
            .AddIngredient(ItemID.ChlorophyteBar, 12)
            .AddTile(TileID.DyeVat)
            .Register();
        }
        public override void UpdateInventory(Player player)
        {
            if (player.GetModPlayer<TranscendencePlayer>().BrushCooloff == 0) ComboDeathTimer++;
            if (ComboDeathTimer > 20)
            {
                Combo = 0;
                ComboDeathTimer = 0;
            }
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.ownedProjectileCounts[projectile] == 0)
            {
                ComboDeathTimer = 0;

                Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, Combo);
                Combo++;

                if (Combo > 2)
                    Combo = 0;
            }

            return false;
        }
        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[projectile] == 0;
        }
    }
    internal class SpritersBrushProj : ModProjectile
    {
        public override string Texture => "TranscendenceMod/Items/Weapons/Melee/SpritersBrush";
        public float Timer = 0;
        public float RotSpeed = 3;
        public bool boolean;
        public int dir;

        public Vector2 startVel;
        public NPC npc;

        Vector2 ownerPos;
        Vector2 pos;
        Vector2 angleToOwner;
        float extenderRot;
        float distance;

        public override void SetDefaults()
        {
            Projectile.width = 46;
            Projectile.height = 46;
            Projectile.DamageType = DamageClass.MeleeNoSpeed;

            Projectile.ownerHitCheck = true;
            Projectile.netImportant = true;

            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;

            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.timeLeft = 120;

            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 12;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Vector2 pos = Vector2.SmoothStep(Projectile.Center, target.Center, 0.5f);
            SoundEngine.PlaySound(SoundID.Item111, pos);
            TranscendenceUtils.ParticleOrchestra(Terraria.GameContent.Drawing.ParticleOrchestraType.Keybrand, pos, -1);

            if (Projectile.ai[0] == 1)
            {
                startVel = Projectile.Center - target.Center;
                npc = target;
            }
        }
        public override void OnSpawn(IEntitySource source)
        {
            SoundEngine.PlaySound(SoundID.Item1, Projectile.Center);
            dir = Main.player[Projectile.owner].direction;

            if (Projectile.ai[0] == 0)
                Projectile.damage *= 3;
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            player.heldProj = Projectile.whoAmI;
            Projectile.ai[2] += 0.2f;
            player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, (player.MountedCenter - Projectile.Center).ToRotation() + MathHelper.PiOver2);

            if (player.HeldItem.type != ModContent.ItemType<SpritersBrush>() || player.dead)
                Projectile.Kill();

            player.ChangeDir(dir);
            player.GetModPlayer<TranscendencePlayer>().BrushCooloff = 5;

            switch (Projectile.ai[0])
            {
                case 0:
                    {
                        if (Projectile.timeLeft > 119)
                        {
                            Timer = 6f * dir;
                            Projectile.velocity = new Vector2(Main.player[Projectile.owner].direction * 3, -5);
                        }

                        Projectile.Center = player.Center + (Projectile.velocity * 2 * Projectile.ai[2]);
                        Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;

                        Projectile.velocity = Projectile.velocity.RotatedBy(Timer / 22);
                        Timer += RotSpeed * 0.001f * dir;
                        break;
                    }
                case 1:
                    {
                        if (Projectile.timeLeft > 90)
                        {
                            Projectile.Center = player.Center;
                            Projectile.velocity = player.DirectionTo(Main.MouseWorld);
                            return;
                        }

                        if (Projectile.timeLeft == 85)
                            SoundEngine.PlaySound(SoundID.Item42, Projectile.Center);

                        Projectile.Center = player.Center + (Projectile.velocity * Timer * 2);
                        Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;

                        if (Projectile.timeLeft < 20)
                        {
                            Timer -= Projectile.Distance(player.Center) > 500 ? 35f : 15f;

                            if (Projectile.Distance(player.Center) < 25)
                                Projectile.Kill();
                        }
                        else
                        {
                            if (npc == null)
                                Timer += 8.5f;
                            else Projectile.Center = npc.Center + startVel;
                        }
                        break;
                    }
                case 2:
                    {
                        Projectile.localAI[2] += 0.05f;
                        if (Projectile.timeLeft > 44)
                        {
                            Timer = 3f * dir;
                            Projectile.velocity = new Vector2(Main.player[Projectile.owner].direction * 3, -5);
                            Projectile.timeLeft = 20;
                        }

                        Projectile.Center = player.Center + (Projectile.velocity * 2);
                        Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;

                        Projectile.velocity = Projectile.velocity.RotatedBy(Timer / 22);
                        Timer -= RotSpeed * 0.02f * dir;

                        Projectile.NewProjectile(player.GetSource_FromAI(), Projectile.Center + Projectile.velocity * 4, Projectile.velocity * 1.25f, ModContent.ProjectileType<SpritersPaint>(), (int)(Projectile.damage * 0.15f), Projectile.knockBack * 0.4f, player.whoAmI, 0, Projectile.localAI[2]);

                        break;
                    }
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D sprite2 = ModContent.Request<Texture2D>($"{Texture}").Value;
            Vector2 origin = new Vector2(sprite2.Width * 0.5f, Projectile.height * 0.5f);
            Player player = Main.player[Projectile.owner];

            if (Projectile.ai[0] == 1 && Projectile.timeLeft > 90)
            {
                SpriteBatch sb = Main.spriteBatch;
                Texture2D sprite = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/BloomLine").Value;

                sb.End();
                sb.Begin(default, BlendState.Additive, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);

                sb.Draw(sprite, new Rectangle(
                    (int)(player.Center.X - Main.screenPosition.X),
                    (int)(player.Center.Y - Main.screenPosition.Y),
                    25,
                    500), null,
                    Color.Brown, player.DirectionTo(Main.MouseWorld).ToRotation() + MathHelper.PiOver2,
                    sprite.Size() * 0.5f,
                    SpriteEffects.None,
                    0);

                sb.Draw(sprite, new Rectangle(
                     (int)(player.Center.X - Main.screenPosition.X),
                     (int)(player.Center.Y - Main.screenPosition.Y),
                     5,
                     500), null,
                     Color.White, player.DirectionTo(Main.MouseWorld).ToRotation() + MathHelper.PiOver2,
                     sprite.Size() * 0.5f,
                     SpriteEffects.None,
                     0);

                sb.End();
                sb.Begin(default, BlendState.AlphaBlend, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);
                return false;
            }

            if (Projectile.ai[0] < 2)
            {
                Asset<Texture2D> sprite = TextureAssets.Chain16;
                ProjectileID.Sets.TrailCacheLength[Type] = 20;
                ProjectileID.Sets.TrailingMode[Type] = 3;

                ownerPos = player.MountedCenter;
                pos = Projectile.Center;
                angleToOwner = ownerPos - pos;
                extenderRot = angleToOwner.ToRotation() - MathHelper.PiOver2;
                distance = angleToOwner.Length();

                Rectangle? chainSourceRectangle = null;
                float chainHeightAdjustment = 0f;

                float chainSegmentLength = (chainSourceRectangle.HasValue ? chainSourceRectangle.Value.Height : sprite.Height()) + chainHeightAdjustment;
                if (chainSegmentLength == 0)
                    chainSegmentLength = 10;

                while (chainSegmentLength >= 0 && distance > 20 && !float.IsNaN(distance) && Projectile.Distance(player.Center) > 30)
                {
                    angleToOwner /= distance;
                    angleToOwner *= sprite.Height();

                    pos += angleToOwner;
                    angleToOwner = ownerPos - pos;
                    distance = angleToOwner.Length();

                    Main.EntitySpriteDraw(sprite.Value, pos - Main.screenPosition, sprite.Value.Bounds, Lighting.GetColor((int)pos.X / 16, (int)(pos.Y / 16f)), extenderRot, sprite.Size() * 0.5f, 1, SpriteEffects.None, 0);
                }
            }


            float rot = Projectile.rotation + (boolean ? 0 : MathHelper.PiOver2);
            SpriteEffects spriteEffects = boolean ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            Main.EntitySpriteDraw(sprite2, Projectile.Center + Projectile.velocity * 2.85f - Main.screenPosition, null, lightColor, rot, origin,
                Projectile.scale, spriteEffects);
            return false;
        }
    }
}