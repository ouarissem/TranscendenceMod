using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Items.Weapons.Melee
{
    public class DoubleEdgedSword : ModItem
    {
        public int State = 1;
        public int proj;
        public int red = ModContent.ProjectileType<DoubleSwordRedProj>();
        public int blue = ModContent.ProjectileType<DoubleSwordBlueProj>();

        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.damage = 55;
            Item.DamageType = DamageClass.MeleeNoSpeed;

            Item.width = 24;
            Item.height = 24;

            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Rapier;

            Item.knockBack = 2;
            Item.shoot = red;
            Item.shootSpeed = 6;

            Item.value = Item.sellPrice(gold : 2, silver: 50);
            Item.rare = ItemRarityID.Green;

            Item.GetGlobalItem<ModifiersItem>().BlacksmithGiantHandleAllowed = true;

            Item.UseSound = SoundID.Item1;
            Item.noUseGraphic = true;
            Item.noMelee = true;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            var statetooltip = new TooltipLine(Mod, "State", $"Current State:" + (State == 1 ? $"[c/ff002a: Red]" :
                $"[c/0373fc: Blue]"));
            tooltips.Add(statetooltip);
        }
        public override bool CanShoot(Player player)
        {
            if (player.ownedProjectileCounts[red] > 0 && player.ownedProjectileCounts[blue] > 0 || player.altFunctionUse == 2)
                return false;
            else return true;
        }
        public override void ModifyShootStats(Player player, ref Vector2 position,
            ref Vector2 velocity, ref int type, ref int damage, ref float knockback) => type = ProjectileID.None;
        public override bool? UseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                State = -State;
                SoundEngine.PlaySound(SoundID.MaxMana, player.Center);
            }
            proj = State == 1 ? red : blue;
            return true;
        }
        public override bool CanUseItem(Player player)
        {
            if (player.ownedProjectileCounts[red] > 0 && player.ownedProjectileCounts[blue] > 0)
                return false;
            else return base.CanUseItem(player);
        }
        public override bool AltFunctionUse(Player player) => true;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.ownedProjectileCounts[red] < 1 && player.ownedProjectileCounts[blue] < 1)
            {
                Projectile.NewProjectile(source, position, velocity.RotatedBy(MathHelper.PiOver2), proj, damage, knockback, -1, 0, 0, 12);
            }
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
             .AddIngredient(ItemID.Bone, 17)
             .AddIngredient(ItemID.Diamond, 12)
             .AddIngredient(ItemID.Ruby, 12)
             .AddIngredient(ItemID.Wood, 10)
             .AddTile(TileID.Anvils)
             .Register();
        }
    }
    internal class DoubleSwordRedProj : ModProjectile
    {
        public float Timer;
        public float SparkleTimer;
        public float RotSpeed = 0.75f;
        public bool boolean;

        public override void SetDefaults()
        {
            Projectile.width = 42;
            Projectile.height = 42;
            Projectile.DamageType = DamageClass.MeleeNoSpeed;

            Projectile.ownerHitCheck = true;
            Projectile.netImportant = true;

            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 19;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 14;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];

            for (int i = 0; i < 7; i++)
            {
                int dust = Dust.NewDust(Projectile.Center + Projectile.velocity * 3.5f, 8, 8, DustID.Adamantite, Main.rand.NextVector2CircularEdge(15, 15).X,
                    Main.rand.NextVector2Circular(15, 15).X, 0, default, 1.25f);
                Main.dust[dust].noGravity = true;
            }
            target.AddBuff(BuffID.OnFire3, 180);
            target.AddBuff(BuffID.Ichor, 180);

        }
        public override void OnSpawn(IEntitySource source) => Timer = -5f;
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float reference = float.NaN;
            if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center,
                Projectile.Center + Projectile.velocity * 6f * Projectile.scale, 16, ref reference))
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

            if (player.HeldItem.type != ModContent.ItemType<DoubleEdgedSword>() || player.dead)
                Projectile.Kill();

            if (Projectile.timeLeft > 9)
            {
                Projectile.scale += MathHelper.Lerp(0.01f, 0.75f, 1f / 9f);
            }
            else Projectile.scale -= 0.1f;
            Projectile.Center = player.Center + (Projectile.velocity * 2f * (Projectile.scale * 3f));
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;

            Projectile.velocity = Projectile.velocity.RotatedBy(Timer / 60f);

            Timer -= RotSpeed;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D sprite = ModContent.Request<Texture2D>($"{Texture}").Value;

            Vector2 origin = new Vector2(sprite.Width * 0.5f, Projectile.height * 0.5f);

            float rot = Projectile.rotation + (boolean ? 0 : MathHelper.PiOver2);
            SpriteEffects spriteEffects = boolean ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            Main.EntitySpriteDraw(sprite, Projectile.Center + Projectile.velocity * 2.85f - Main.screenPosition, null, Color.White, rot, origin,
                Projectile.scale, spriteEffects);
            return false;
        }
    }
    internal class DoubleSwordBlueProj : DoubleSwordRedProj
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            for (int i = 0; i < 7; i++)
            {
                int dust = Dust.NewDust(Projectile.Center + Projectile.velocity * 3.5f, 8, 8, DustID.Ice, Main.rand.NextVector2CircularEdge(15, 15).X,
                    Main.rand.NextVector2Circular(15, 15).X, 0, default, 1.25f);
                Main.dust[dust].noGravity = true;
            }
            target.AddBuff(BuffID.Frostburn2, 180);
            target.AddBuff(BuffID.Oiled, 180);

        }
        public override void OnSpawn(IEntitySource source) => Timer = -5f;
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            base.Colliding(projHitbox, targetHitbox);
            return base.Colliding(projHitbox, targetHitbox);
        }
        public override void AI()
        {
            base.AI();
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D sprite = ModContent.Request<Texture2D>($"{Texture}").Value;

            Vector2 origin = new Vector2(sprite.Width * 0.5f, Projectile.height * 0.5f);

            float rot = Projectile.rotation + (boolean ? 0 : MathHelper.PiOver2);
            SpriteEffects spriteEffects = boolean ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            Main.EntitySpriteDraw(sprite, Projectile.Center + Projectile.velocity * 2.85f - Main.screenPosition, null, Color.White, rot, origin,
                Projectile.scale, spriteEffects);
            return false;
        }
    }
}