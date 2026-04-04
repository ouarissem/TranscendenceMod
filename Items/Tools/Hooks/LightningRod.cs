using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Miscannellous.GlobalStuff;
using TranscendenceMod.Miscannellous.Rarities;

namespace TranscendenceMod.Items.Tools.Hooks
{
    public class LightningRod : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.DiamondHook);
            Item.width = 22;
            Item.height = 22;

            Item.damage = 230;
            Item.DamageType = DamageClass.Generic;

            Item.shoot = ModContent.ProjectileType<LightningRodProj>();
            Item.shootSpeed = 26f;

            Item.useStyle = ItemUseStyleID.None;
            Item.value = Item.sellPrice(gold: 4);
            Item.rare = ModContent.RarityType<Brown>();
        }
    }
    public class LightningRodProj : ModProjectile
    {
        public int Draw;
        Vector2 ownerPos;
        Vector2 pos;
        Vector2 angleToOwner;
        float extenderRot;
        float distance;
        public int hookAmount;
        public Player player;
        public float velRot;
        public int dmg;
        public int FixFreeze;

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.LunarHookSolar);
            Projectile.friendly = false;
        }
        public override void AI()
        {
            player = Main.player[Projectile.owner];

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];

                if (npc != null && npc.active && !npc.friendly && !npc.dontTakeDamage && npc.GetGlobalNPC<TranscendenceNPC>().HitCD == 0)
                {
                    Rectangle rect = npc.getRect();
                    int num11 = 8;
                    rect.X -= num11;
                    rect.Y -= num11;
                    rect.Width += num11 * 2;
                    rect.Height += num11 * 2;

                    if (Projectile.Colliding(Projectile.getRect(), rect))
                    {
                        npc.SimpleStrikeNPC(dmg, 0, false, 4, DamageClass.Generic, true, Main.player[Projectile.owner].luck);
                        npc.GetGlobalNPC<TranscendenceNPC>().HitCD = 10;
                    }
                }
            }
        }
        public override void OnSpawn(IEntitySource source)
        {
            velRot = Projectile.velocity.ToRotation() - MathHelper.PiOver2;
            dmg = Projectile.damage;
        }
        public override bool? GrappleCanLatchOnTo(Player player, int x, int y)
        {
            return base.GrappleCanLatchOnTo(player, x, y);
        }
        public override void GrapplePullSpeed(Player player, ref float speed) {
            speed = 25f;
        }
        public override void GrappleRetreatSpeed(Player player, ref float speed) {
            speed = 25f;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float reference = float.NaN;
            if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Main.player[Projectile.owner].Center, Projectile.Center, 1, ref reference))
                return true;
            else return false;
        }
        public override float GrappleRange() => 555f;
        public override bool? CanUseGrapple(Player player) => player.ownedProjectileCounts[Type] < 3;
        public override void NumGrappleHooks(Player player, ref int numHooks) {
            numHooks = 2;
        }
        public override void PostDraw(Color lightColor)
        {
            float rot = player.DirectionTo(Projectile.Center).ToRotation() + MathHelper.PiOver4;
            TranscendenceUtils.DrawEntity(Projectile, lightColor * 3.5f, 1f, Texture + "_Handle",
                rot, player.Center + Vector2.One.RotatedBy(rot - MathHelper.Pi + MathHelper.PiOver2) * 20, null);

            if (Draw % 5 == 0)
                FixFreeze = 0;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            TranscendenceUtils.DrawEntity(Projectile, lightColor * 3.5f, 1f, Texture, velRot, Projectile.Center, null);
            return false;
        }
        public override bool PreDrawExtras()
        {
            Asset<Texture2D> sprite = ModContent.Request<Texture2D>(Texture + "_Chain");
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
                chainSegmentLength = 14;

            //Hopefully fix infinite loops
            if (++Draw % 1 == 0)
            {
                while (chainSegmentLength >= 0 && distance > 30 && !float.IsNaN(distance) && Projectile.Distance(ownerPos) > 40)
                {
                    if (FixFreeze++ > 100)
                        break;

                    angleToOwner /= (distance > 0 ? distance : 1);
                    angleToOwner *= sprite.Height();

                    pos += angleToOwner;
                    angleToOwner = ownerPos - pos;
                    distance = angleToOwner.Length();

                    Lighting.AddLight(pos, 0f, 0.1f, 0.5f);
                    Main.EntitySpriteDraw(sprite.Value, pos - Main.screenPosition, sprite.Value.Bounds, Color.White, extenderRot, sprite.Size() * 0.5f, 1, SpriteEffects.None, 0);
                }
            }
            return false;
        }
    }
}
