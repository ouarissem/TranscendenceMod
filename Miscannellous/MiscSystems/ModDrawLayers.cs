using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using System;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using TranscendenceMod.Buffs;
using TranscendenceMod.Items.Accessories.Movement.Wings;
using TranscendenceMod.Items.Accessories.Movement;
using TranscendenceMod.Items.Weapons.Melee;
using TranscendenceMod.Items.Weapons.Ranged;
using TranscendenceMod.Items.Tools.Compasses;
using TranscendenceMod.NPCs.Boss.Seraph;

namespace TranscendenceMod
{
    public class CompassNeedle : PlayerDrawLayer
    {
        public override Position GetDefaultPosition() => PlayerDrawLayers.AfterLastVanillaLayer;
        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
        {
            Player player = drawInfo.drawPlayer;
            return player.HeldItem.ModItem is Compass;
        }
        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            Texture2D sprite = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/CompassNeedle").Value;

            Player player = drawInfo.drawPlayer;
            if (player.HeldItem.ModItem is not Compass compass)
                return;

            if (compass.Pos == Vector2.Zero)
                return;

            float rot = player.DirectionTo(compass.Pos).ToRotation() - MathHelper.PiOver4;
            if (player.GetModPlayer<AngelsGatewayPlayer>().ZoneAngelGateway > 0 || NPC.AnyNPCs(ModContent.NPCType<CelestialSeraph>()))
                rot = Main.rand.NextFloat(MathHelper.TwoPi);

            var drawPos = drawInfo.Center + Vector2.One.RotatedBy(rot) * 125f - Main.screenPosition;
            drawInfo.DrawDataCache.Add(new DrawData(sprite, drawPos, null, Color.White * 0.75f, rot + MathHelper.PiOver2 * 1.5f - player.fullRotation, sprite.Size() * 0.5f, 1f, SpriteEffects.None));
        }
    }

    public class CosmicSet : PlayerDrawLayer
    {
        public float rot;
        public override Position GetDefaultPosition() => PlayerDrawLayers.AfterLastVanillaLayer;
        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
        {
            Player player = drawInfo.drawPlayer;
            return player.GetModPlayer<TranscendencePlayer>().CosmicSetWear && player.GetModPlayer<TranscendencePlayer>().ArmorKeybind && !player.HasBuff(BuffID.ChaosState);
        }
        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            Texture2D sprite = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/Icons/CosmicTp").Value;

            Player player = drawInfo.drawPlayer;
            TranscendencePlayer modplayer = player.GetModPlayer<TranscendencePlayer>();
            int am = modplayer.CosmicTPpositions.Length;

            for (int i = 0; i < am; i++)
            {
                float tpDistance = player.Distance(Main.MouseWorld);
                if (tpDistance < 126)
                    tpDistance = 125;

                if (tpDistance > 499)
                    tpDistance = 500;

                Vector2 vec = Vector2.One.RotatedBy(MathHelper.TwoPi * i / 8f + MathHelper.PiOver4 - player.fullRotation) * tpDistance * 0.7f;

                var drawPos = player.Center + vec - Main.screenPosition;

                bool hasTiles = Collision.SolidCollision((player.Center + vec) - (player.Size / 2), player.width, player.height);

                Color col = Color.White;
                if (hasTiles)
                    col = Color.Red * 0.33f;

                float sizeMult = 1f;

                if (Main.MouseWorld.Distance(player.Center + vec) < 50 && !hasTiles)
                {
                    sizeMult = 1.5f;
                    for (int j = 0; j < 4; j++)
                    {
                        drawInfo.DrawDataCache.Add(new DrawData(sprite, drawPos + Vector2.One.RotatedBy(MathHelper.TwoPi * j / 4f + TranscendenceWorld.UniversalRotation * 2f) * 6f, null, Color.Aqua * 0.33f, -player.fullRotation, sprite.Size() * 0.5f, sizeMult, SpriteEffects.None));
                    }
                }

                drawInfo.DrawDataCache.Add(new DrawData(sprite, drawPos, null, col * 0.75f, -player.fullRotation, sprite.Size() * 0.5f, sizeMult, SpriteEffects.None));
            }
        }
    }

    //Void Necklace Ability
    public class VoidNecklaceBlackhole : PlayerDrawLayer
    {
        public float rot;
        public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.NeckAcc);
        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
        {
            Player player = drawInfo.drawPlayer;
            return player.GetModPlayer<TranscendencePlayer>().VoidNecklaceAcc && player.GetModPlayer<TranscendencePlayer>().VoidNecklaceAlpha > 0f;
        }
        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            Texture2D sprite = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/BlackHole/BlackHoleAura").Value;
            Texture2D sprite2 = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/BlackHole/BlackHoleCenter").Value;

            Player player = drawInfo.drawPlayer;
            var drawPos = drawInfo.Center - Main.screenPosition;
            drawPos += new Vector2(3 * player.direction, 3);

            float alpha = player.GetModPlayer<TranscendencePlayer>().VoidNecklaceAlpha;
            rot += 0.05f;

            drawInfo.DrawDataCache.Add(new DrawData(sprite, drawPos, null, Color.Lerp(new Color(1f, 0.125f, 0.66f, 0f), new Color(0.75f, 0.1f, 0.75f, 0f), (float)Math.Sin(Main.GlobalTimeWrappedHourly * 10f)) * 0.35f * alpha, rot, sprite.Size() * 0.5f,
                alpha * 0.35f, SpriteEffects.None));

            drawInfo.DrawDataCache.Add(new DrawData(sprite2, drawPos, null, Color.Black, -rot, sprite2.Size() * 0.5f,
                alpha * 0.035f, SpriteEffects.None));
        }
    }

    //Cosmos-Shard Launcher
    public class CosmicRocketLauncherLayer : PlayerDrawLayer
    {
        public float rot;
        public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.Torso);
        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
        {
            Player player = drawInfo.drawPlayer;
            return player.HeldItem.type == ModContent.ItemType<CosmosShardLauncher>()
                && !player.dead && player.active && player.Distance(Main.MouseWorld) > 100;
        }
        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            string spriteString = "TranscendenceMod/Items/Weapons/Ranged/CosmosShardLauncher";
            Player player = drawInfo.drawPlayer;

            if (player.GetModPlayer<TranscendencePlayer>().CosmoShardTimer != 0 || !player.HasAmmo(new Item(ItemID.RocketLauncher)))
                spriteString = "TranscendenceMod/Items/Weapons/Ranged/CosmosShardLauncherNoRocket";

            Texture2D sprite = ModContent.Request<Texture2D>(spriteString).Value;
            bool left = Main.MouseWorld.X < player.Center.X;

            Vector2 vel = new Vector2(5 * player.direction, 2);
            if (!player.HasBuff(ModContent.BuffType<SeraphTimeStop>())) vel = player.DirectionTo(Main.MouseWorld) * 6.5f;
            Vector2 drawPos = player.Center + vel * 4 - new Vector2(player.direction == 1 ? 5 : -5, 20) - Main.screenPosition;

            drawInfo.DrawDataCache.Add(new DrawData(sprite, drawPos, null,
                Color.White,
                vel.ToRotation(),
                sprite.Size() * 0.5f, 1, left ? SpriteEffects.FlipVertically : SpriteEffects.None));
        }
    }

    //Raintide Muramasa
    public class RaintideMuramsaHoldout : PlayerDrawLayer
    {
        public override Position GetDefaultPosition() => new BeforeParent(PlayerDrawLayers.ArmOverItem);
        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
        {
            Player player = drawInfo.drawPlayer;
            return player.HeldItem.type == ModContent.ItemType<UpgradedMuramasa>() && player.GetModPlayer<TranscendencePlayer>().MuramasaTime < 1 && !player.dead && player.active;
        }
        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            Texture2D Texture = ModContent.Request<Texture2D>("TranscendenceMod/Items/Weapons/Melee/UpgradedMuramasa").Value;
            Player player = drawInfo.drawPlayer;

            Vector2 drawPos = player.Center + new Vector2(30 * -player.direction, player.gfxOffY) - Main.screenPosition;
            drawPos = new Vector2((int)drawPos.X, (int)drawPos.Y);

            drawInfo.DrawDataCache.Add(new DrawData(Texture, drawPos - new Vector2(0, 2), null, Color.White, MathHelper.PiOver4 / -2 * player.direction, Texture.Size() * 0.5f, 1f, player.direction > 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None));
        }
    }

    //Evasion Stone Rock Barrier
    public class EvasionStoneDraw : PlayerDrawLayer
    {
        public override Position GetDefaultPosition() => new BeforeParent(PlayerDrawLayers.Wings);
        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
        {
            Player player = drawInfo.drawPlayer;
            return player.GetModPlayer<TranscendencePlayer>().EvasionStoneEquipped && player.GetModPlayer<TranscendencePlayer>().EvasionStoneExists;
        }
        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            Texture2D StoneTexture = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/EvasionStoneDrawlayer").Value;
            Player player = drawInfo.drawPlayer;

            Vector2 drawPos = player.Center + new Vector2(0, player.gfxOffY) - Main.screenPosition;
            drawPos = new Vector2((int)drawPos.X, (int)drawPos.Y);

            drawInfo.DrawDataCache.Add(new DrawData(StoneTexture, drawPos, null, Color.White, 0, StoneTexture.Size() * 0.5f, 1f, SpriteEffects.None));
        }
    }

    //Evasion Stone Graze Ring
    public class EvasionStoneGrazeDraw : PlayerDrawLayer
    {
        public override Position GetDefaultPosition() => PlayerDrawLayers.AfterLastVanillaLayer;
        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
        {
            Player player = drawInfo.drawPlayer;
            return player.GetModPlayer<TranscendencePlayer>().EvasionStoneEquipped && !player.GetModPlayer<TranscendencePlayer>().EvasionStoneExists;
        }
        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            Texture2D StoneTexture = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/EvasionStoneGrazeDrawlayer").Value;
            Player player = drawInfo.drawPlayer;

            Vector2 drawPos = (player.mount.Active ? player.MountedCenter : player.Center) - Main.screenPosition;
            drawPos = new Vector2((int)drawPos.X, (int)drawPos.Y);

            DrawData drawData = new DrawData(StoneTexture, drawPos, null, Color.White * 0.5f, 0, StoneTexture.Size() * 0.5f, 1, SpriteEffects.None);
            drawData.shader = player.cBody;
            drawInfo.DrawDataCache.Add(drawData);
        }
    }

    // Tiara Lacewing Transformation
    public class LacewingTransformation : PlayerDrawLayer
    {
        public override Position GetDefaultPosition() => PlayerDrawLayers.AfterLastVanillaLayer;
        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
        {
            Player player = drawInfo.drawPlayer;
            return player.GetModPlayer<TranscendencePlayer>().LacewingTrans && !player.dead;
        }
        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            Texture2D Fly = TextureAssets.Npc[NPCID.EmpressButterfly].Value;
            Player player = drawInfo.drawPlayer;

            Vector2 drawPos = player.Center + new Vector2(0, player.gfxOffY) - Main.screenPosition;
            drawPos = new Vector2((int)drawPos.X, (int)drawPos.Y);

            Rectangle rec = new Rectangle(0, player.GetModPlayer<TranscendencePlayer>().LacewingFrame, 24, 24);
            SpriteEffects se = player.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            bool fish = player.GetModPlayer<TranscendencePlayer>().FishNeck && player.wet;
            float sin = (float)(3f + Math.Sin(TranscendenceWorld.UniversalRotation * 4f) * 2f);
            for (int i = 0; i < 8; i++)
            {
                Color col = Main.hslToRgb(i / 8f, 1f, 0.5f);
                if (Main.dayTime)
                    col = Color.Lerp(Color.Gold, Color.OrangeRed, i / 8f);
                if (fish)
                    col = Color.DeepSkyBlue;
                if (player.GetModPlayer<TranscendencePlayer>().Focus < 25f)
                    col = Color.Gray;

                col.A = 0;
                DrawData drawData2 = new DrawData(Fly, drawPos + Vector2.One.RotatedBy(MathHelper.TwoPi * i / 8f + TranscendenceWorld.UniversalRotation * 2f) * sin, rec, col * 0.5f, player.velocity.X * 0.075f, rec.Size() * 0.5f, 1f, se);
                drawInfo.DrawDataCache.Add(drawData2);
            }

            Color col2 = (player.GetModPlayer<TranscendencePlayer>().Focus < 25f ? Color.Gray : fish ? Color.DeepSkyBlue : Main.dayTime ? Color.Gold : Color.White) * 0.75f;
            Lighting.AddLight(player.Center, col2.R / 255f, col2.G / 255f, col2.B / 255f);

            DrawData drawData = new DrawData(Fly, drawPos, rec, new Color(1f, 1f, 1f, 0f), player.velocity.X * 0.075f, rec.Size() * 0.5f, 1f, se);
            drawInfo.DrawDataCache.Add(drawData);
        }
    }

    //Fish Pendant Transformation
    public class FishTransform : PlayerDrawLayer
    {
        public override Position GetDefaultPosition() => PlayerDrawLayers.AfterLastVanillaLayer;
        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
        {
            Player player = drawInfo.drawPlayer;
            return player.GetModPlayer<TranscendencePlayer>().FishTrans > 0 && !player.dead;
        }
        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            Texture2D Fish = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/FishTrans").Value;
            Player player = drawInfo.drawPlayer;

            Vector2 drawPos = player.Center + new Vector2(0, player.gfxOffY) - Main.screenPosition;
            drawPos = new Vector2((int)drawPos.X, (int)drawPos.Y);

            Rectangle rec = new Rectangle(0, player.GetModPlayer<TranscendencePlayer>().FishFrame, 28, 16);
            SpriteEffects se = player.direction == -1 ? SpriteEffects.FlipVertically : SpriteEffects.None;

            DrawData drawData = new DrawData(Fish, drawPos, rec, drawInfo.colorArmorBody, -MathHelper.PiOver2, rec.Size() * 0.5f, 1f, se);
            drawData.shader = player.cBody;
            drawInfo.DrawDataCache.Add(drawData);
        }
    }

    //Golem Bulwark Transformation
    public class GolemBulwark : PlayerDrawLayer
    {
        public override Position GetDefaultPosition() => PlayerDrawLayers.AfterLastVanillaLayer;
        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
        {
            Player player = drawInfo.drawPlayer;
            return player.GetModPlayer<TranscendencePlayer>().InsideGolem && player.GetModPlayer<TranscendencePlayer>().LihzardianBulwarkEquipped && !player.dead;
        }
        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            Texture2D Body = TextureAssets.Npc[NPCID.Golem].Value;
            Texture2D BodyG = TextureAssets.Golem[3].Value;
            Texture2D Fist = TextureAssets.Npc[NPCID.GolemFistLeft].Value;
            Texture2D Head = TextureAssets.Npc[NPCID.GolemHeadFree].Value;
            Texture2D HeadG = TextureAssets.Golem[1].Value;
            Texture2D HeadG2 = TextureAssets.Extra[107].Value;
            Player player = drawInfo.drawPlayer;

            Vector2 drawPos = player.Bottom - new Vector2(0, 48) + new Vector2(0, player.gfxOffY) - Main.screenPosition;
            Vector2 eyePos = drawPos - new Vector2(5, 100);

            //Body
            drawInfo.DrawDataCache.Add(new DrawData(Body, drawPos - new Vector2(2, 30), new Rectangle(0, 0, 186, 172), Lighting.GetColor((int)player.Center.X / 16, (int)player.Center.Y / 16, Color.White), 0, new Rectangle(0, 0, 186, 172).Size() * 0.5f, 1f, SpriteEffects.None));
            drawInfo.DrawDataCache.Add(new DrawData(BodyG, drawPos - new Vector2(2, 30), new Rectangle(0, 0, 186, 172), Color.White, 0, new Rectangle(0, 0, 186, 172).Size() * 0.5f, 1f, SpriteEffects.None));

            //Head + Eyes
            drawInfo.DrawDataCache.Add(new DrawData(Head, drawPos - new Vector2(4, 105), new Rectangle(0, 0, 112, 106), Lighting.GetColor((int)player.Center.X / 16, (int)player.Center.Y / 16, Color.White), 0, new Rectangle(0, 0, 112, 106).Size() * 0.5f, 1f, SpriteEffects.None));
            drawInfo.DrawDataCache.Add(new DrawData(HeadG, eyePos + Vector2.One.RotatedBy(player.DirectionTo(Main.MouseWorld).ToRotation() - MathHelper.PiOver4) * 2f, new Rectangle(0, 0, 38, 10), Color.White, 0, new Rectangle(0, 0, 38, 10).Size() * 0.5f, 1f, SpriteEffects.None));
            drawInfo.DrawDataCache.Add(new DrawData(HeadG2, drawPos - new Vector2(4, 90), new Rectangle(0, 0, 112, 138), Color.White, 0, new Rectangle(0, 0, 112, 138).Size() * 0.5f, 1f, SpriteEffects.None));

            //Fists
            drawInfo.DrawDataCache.Add(new DrawData(Fist, drawPos - new Vector2(80, 25), null, Lighting.GetColor((int)player.Center.X / 16, (int)player.Center.Y / 16, Color.White), 0, Fist.Size() * 0.5f, 1f, SpriteEffects.None));
            drawInfo.DrawDataCache.Add(new DrawData(Fist, drawPos - new Vector2(-80, 25), null, Lighting.GetColor((int)player.Center.X / 16, (int)player.Center.Y / 16, Color.White), 0, Fist.Size() * 0.5f, 1f, SpriteEffects.FlipHorizontally));

        }
    }


    //Wing Glowmasks
    public class GlowWings : PlayerDrawLayer
    {
        public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.Wings);
        public bool Space(Player player) => player.wings == EquipLoader.GetEquipSlot(Mod, nameof(SpaceBossWings), EquipType.Wings);
        public bool Meteor(Player player) => player.wings == EquipLoader.GetEquipSlot(Mod, nameof(MeteorJetpack), EquipType.Wings);
        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
        {
            Player player = drawInfo.drawPlayer;
            return (player.wings == EquipLoader.GetEquipSlot(Mod, nameof(CorruptedWanderingKit), EquipType.Wings) ||
                Space(player) || Meteor(player)) && !player.dead && player.active;
        }
        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            Player player = drawInfo.drawPlayer;

            string spritePath = Meteor(player) ? "TranscendenceMod/Items/Accessories/Movement/Wings/MeteorJetpack_Wings_Glow" :
                Space(player) ? "TranscendenceMod/Items/Accessories/Movement/Wings/SpaceBossWings_Wings" :
                "TranscendenceMod/Items/Accessories/Movement/CorruptedWanderingKit_Wings";
            Texture2D Texture = ModContent.Request<Texture2D>(spritePath).Value;

            Vector2 directions = player.Directions;
            Vector2 vector9 = new Vector2(0f, 8f);
            Vector2 vector = drawInfo.Position - Main.screenPosition + new Vector2(player.width / 2, player.height - player.bodyFrame.Height / 2) + vector9;
            int num = -13;
            int num8 = -9;
            Vector2 vector10 = vector + new Vector2(num8, num) * directions;

            DrawData drawData = new DrawData(Texture, vector10.Floor(), new Rectangle(0, player.wingFrame * 62, 86, 62), Color.White, 0, new Vector2(TextureAssets.Wings[player.wings].Width() / 2, TextureAssets.Wings[player.wings].Height() / 14), 1f, player.direction < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
            drawData.shader = player.cWings;

            drawInfo.DrawDataCache.Add(drawData);
        }
    }

    //Ret Laser Lens
    public class NucleusLensLayer : PlayerDrawLayer
    {
        public int mountOffset = 0;
        public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.Head);
        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
        {
            Player player = drawInfo.drawPlayer;
            return player.GetModPlayer<TranscendencePlayer>().NucleusLensSocial;
        }
        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            Texture2D Texture = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/MechanicalLens").Value;
            Player player = drawInfo.drawPlayer;

            Vector2 pos = player.Center + new Vector2(3.75f * player.direction, -MathHelper.Lerp(28, 6, player.GetModPlayer<TranscendencePlayer>().ScaleMult) + player.gfxOffY);

            Vector2 drawPos = pos - Main.screenPosition;
            drawPos = new Vector2((int)drawPos.X, (int)drawPos.Y);

            if (player.mount.Active) mountOffset = player.mount.HeightBoost;
            else mountOffset = 0;

            drawPos -= new Vector2(0, 3 + mountOffset);

            drawInfo.DrawDataCache.Add(new DrawData(Texture, drawPos, null, new Color(1f, 1f, 1f, 0f), 0, Texture.Size() * 0.5f, 1f, player.direction < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None));
        }
    }


    //Parry Shield Drawing
    public class StardustShield : PlayerDrawLayer
    {
        public override Position GetDefaultPosition() => PlayerDrawLayers.AfterLastVanillaLayer;
        public bool cultist(Player player)
        {
            return player.GetModPlayer<TranscendencePlayer>().CultistForcefield && player.GetModPlayer<TranscendencePlayer>().ParryTimer > 0;
        }
        public bool cosmic(Player player)
        {
            return player.GetModPlayer<TranscendencePlayer>().StardustShield && player.GetModPlayer<TranscendencePlayer>().ParryTimer > 0;
        }
        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
        {
            Player player = drawInfo.drawPlayer;
            return cosmic(player) || cultist(player);
        }
        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            Player player = drawInfo.drawPlayer;
            SpriteBatch spriteBatch = Main.spriteBatch;
            Vector2 drawPos = (player.mount.Active ? player.MountedCenter : player.Center) - Main.screenPosition;
            drawPos = new Vector2((int)drawPos.X, (int)drawPos.Y);

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            string spritepath = $"Terraria/Images/Misc/Perlin";
            Texture2D sprite = ModContent.Request<Texture2D>(spritepath).Value;

            Vector2 shieldPos = drawPos + new Vector2(55, 60) + new Vector2(126, 128);
            Rectangle rec = new Rectangle(0, 0, 150, 125);

            Color col = cultist(player) ? Color.White : Color.Blue;

            DrawData drawData = new DrawData(sprite, shieldPos, rec, col, 0, sprite.Size() * 0.5f, 1f, SpriteEffects.None);
            GameShaders.Misc["ForceField"].UseColor(col);
            GameShaders.Misc["ForceField"].Apply(drawData);
            drawData.Draw(spriteBatch);

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);
        }
    }
    public class CultistForcefield : PlayerDrawLayer
    {
        public int ForcefieldSize;
        public override Position GetDefaultPosition() => PlayerDrawLayers.AfterLastVanillaLayer;
        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
        {
            Player player = drawInfo.drawPlayer;
            return player.GetModPlayer<TranscendencePlayer>().CultistForcefield && player.GetModPlayer<TranscendencePlayer>().ParryTimer > 0;
        }
        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            Player player = drawInfo.drawPlayer;
            SpriteBatch spriteBatch = Main.spriteBatch;
            Vector2 drawPos = (player.mount.Active ? player.MountedCenter : player.Center) - Main.screenPosition;
            drawPos = new Vector2((int)drawPos.X, (int)drawPos.Y);

            ForcefieldSize += 2;
            if (ForcefieldSize > 15)
                ForcefieldSize = -200;

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            string spritepath = $"Terraria/Images/Misc/Perlin";
            Texture2D sprite = ModContent.Request<Texture2D>(spritepath).Value;

            Vector2 shieldPos = drawPos + new Vector2(55 - (ForcefieldSize / 4), 60 - (ForcefieldSize / 4)) + new Vector2(126, 128);
            Rectangle rec = new Rectangle(-30 + ForcefieldSize, -30 + (ForcefieldSize / 2), 150 + (ForcefieldSize / 2), 125 + (ForcefieldSize / 2));

            Color col = Color.Lerp(Main.hslToRgb(ForcefieldSize / 215f, 1f, 0.5f), Color.Transparent, ForcefieldSize / 15f);
            DrawData drawData = new DrawData(sprite, shieldPos, rec, col, 0, sprite.Size() * 0.5f, 1f, SpriteEffects.None);

            GameShaders.Misc["ForceField"].UseColor(col);
            GameShaders.Misc["ForceField"].Apply(drawData);
            drawData.Draw(spriteBatch);

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);

        }
    }
}

