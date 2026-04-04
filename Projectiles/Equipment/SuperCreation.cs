using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TranscendenceMod.Items.Consumables.SuperBomb;
using TranscendenceMod.Miscannellous.GlobalStuff;

namespace TranscendenceMod.Projectiles.Equipment
{
    public class SuperCreation : ModProjectile
    {
        public float Rad;
        public float MaxRad;
        public float Alpha = 1;
        public Item block;
        public Item block2;
        public int Tries;
        public override string Texture => "TranscendenceMod/Miscannellous/Assets/InvisSprite";
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 1500;
        }
        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;

            Projectile.tileCollide = false;
            Projectile.aiStyle = -1;

            Projectile.friendly = true;
            Projectile.hostile = true;

            Projectile.timeLeft = 750;
            Projectile.extraUpdates = 52;
            Projectile.penetrate = -1;
        }
        public override void OnSpawn(IEntitySource source)
        {
            MaxRad = Projectile.timeLeft;
            SoundEngine.PlaySound(SoundID.Item14 with { MaxInstances = 0 }, Projectile.Center);

            int rad = 14 * 16;
            Player player = Main.player[Projectile.owner];

            Vector2 center = Projectile.Center + new Vector2(Projectile.width * 0.25f, Projectile.height * 0.15f);

            for (int u = 0; u < player.inventory.Length; u++)
            {
                block = player.inventory[u];
                if (block != null && block.type != ItemID.CopperCoin && block.type != ItemID.SilverCoin && block.type != ItemID.GoldCoin && block.type != ItemID.PlatinumCoin && block.favorited && (block.createTile != -1 && Projectile.ai[1] == 0 || block.createWall != -1 && Projectile.ai[1] == 1) && block.consumable && block.stack > 0 && block2 == null)
                {
                    block2 = block;
                }
            }
            for (int i = -rad; i < rad; i++)
            {
                for (int j = -rad; j < rad; j++)
                {
                    Vector2 pos = center + new Vector2(i, j);
                    Tile tile = Framing.GetTileSafely((int)pos.X / 16, (int)pos.Y / 16);

                    if (block2 != null && Projectile.owner == Main.myPlayer && player != null && tile != null && (i * i + j * j) <= rad * rad)
                    {
                        if (player.HasItem(block2.type) && (Projectile.ai[1] == 0 && !tile.HasTile || Projectile.ai[1] == 1 && tile.WallType == 0) && Tries < 7750)
                        {
                            if (Projectile.ai[1] == 0) WorldGen.PlaceTile((int)(pos.X / 16), (int)(pos.Y / 16), block2.createTile);
                            else WorldGen.PlaceWall((int)(pos.X / 16), (int)(pos.Y / 16), block2.createWall);
                            player.ConsumeItem(block2.type);
                            Tries++;
                        }
                    }
                }
            }


            if (block2 == null)
            {
                int c = CombatText.NewText(Projectile.getRect(), Color.Red, Language.GetTextValue("Mods.TranscendenceMod.Messages.CreationBombFail"));
                Item.NewItem(Projectile.GetSource_FromAI(), Projectile.getRect(), Projectile.ai[2] == 1 ? ModContent.ItemType<SuperBomb_Brick_Sticky>() : ModContent.ItemType<SuperBomb_Brick>());
                Main.combatText[c].lifeTime = 120;
                return;
            }
            else
            {
                int c = CombatText.NewText(Projectile.getRect(), Color.White, Language.GetTextValue("Mods.TranscendenceMod.Messages.CreationBombSuccess", block2.Name));
                Main.combatText[c].lifeTime = 120;
            }
        }
        public override void AI()
        {
            Projectile.position = Projectile.Center;
            

            Projectile.width++; 
            Projectile.height++;
            Projectile.position.Y += 0.0625f;

            Projectile.position -= Projectile.Size * 0.5f;
            Alpha = MathHelper.Lerp(0, 2, (float)Projectile.timeLeft / (float)MaxRad);
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (targetHitbox.Distance(Projectile.Center + new Vector2(Projectile.width * 0.25f, Projectile.height * 0.15f)) < (Projectile.width * 0.33f) && Projectile.timeLeft > 100 && Projectile.timeLeft < 500)
                return true;
            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;
            
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            string spritepath = "TranscendenceMod/Miscannellous/Assets/Perlin2";
            Texture2D sprite = ModContent.Request<Texture2D>(spritepath).Value;
            Color col = Color.Lerp(Color.Gold, Color.OrangeRed, (float)Math.Sin(Projectile.width / 32f));

            Vector2 scale = new Vector2(1.5f, 1f);
            Vector2 drawPosition = Projectile.Center - Main.screenPosition + Projectile.Size * scale * 0.66f;
            Rectangle drawArea = new Rectangle(0, 0, Projectile.width, Projectile.height);

            DrawData drawData = new DrawData(sprite, drawPosition, drawArea, col * Alpha, 0, Projectile.Size, scale, SpriteEffects.None);

            GameShaders.Misc["ForceField"].UseColor(col * Alpha);
            GameShaders.Misc["ForceField"].Apply(drawData);
            drawData.Draw(spriteBatch);

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);
            return false;
        }
    }
}