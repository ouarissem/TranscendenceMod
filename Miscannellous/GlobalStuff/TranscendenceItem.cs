using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI.Chat;
using TranscendenceMod.Items.Accessories.Offensive.EoL;
using TranscendenceMod.Items.Accessories.Other;
using TranscendenceMod.Items.Accessories.Shields;
using TranscendenceMod.Items.Consumables.Placeables;
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Items.Modifiers;
using TranscendenceMod.Items.Weapons.Magic;
using TranscendenceMod.Miscannellous.Rarities;
using TranscendenceMod.Projectiles.Equipment;
using TranscendenceMod.Projectiles.Equipment.PowerTablet;

namespace TranscendenceMod.Miscannellous.GlobalStuff
{
    public class TranscendenceItem : GlobalItem
    {
        public override bool InstancePerEntity => true;

        public int ShieldParryCD;

        /* Cosmic Rarity */
        public bool SeraphDifficultyItem;
        public Vector2[] starPositions = new Vector2[6];
        public int[] starsRandomX = new int[6];
        public int[] starsRandomY = new int[6];
        public float[] starsSize = new float[6];
        public static int[] starResetTimer = new int[6];
        public float[] starsRot = new float[6];
        public int[] starsDir = new int[6];

        public bool IsAGloveThrowableItem(Item item)
        {
            return item.axe > 0 || item.pick > 0 || item.type == ItemID.Umbrella || item.type == ItemID.TragicUmbrella;
        }

        public override void PostDrawTooltipLine(Item item, DrawableTooltipLine line)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;

            //Request effect
            var eff = ModContent.Request<Effect>("TranscendenceMod/Miscannellous/Assets/Shaders/Effects/MovingNoiseTrans", AssetRequestMode.ImmediateLoad).Value;

            //Set the shader noise image
            Texture2D shaderImage = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/SeraphFontShader").Value;
            Main.instance.GraphicsDevice.Textures[1] = shaderImage;

            //Changes what sprite the stars use, couldn't really find any other good texture and too lazy to make my own
            Texture2D star = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/StarEffect").Value;

            //Make the texture actually scroll with Main.GlobalTimeWrappedHourly
            eff.Parameters["uImageSize0"].SetValue(new Vector2(1500));
            eff.Parameters["uImageSize1"].SetValue(shaderImage.Size() * 3.17f);
            eff.Parameters["uTime"].SetValue(Main.GlobalTimeWrappedHourly * 0.75f);
            eff.Parameters["yChange"].SetValue(0.25f);
            eff.Parameters["useAlpha"].SetValue(false);
            eff.Parameters["useExtraCol"].SetValue(false);

            if ((SeraphDifficultyItem || item.rare == ModContent.RarityType<CosmicRarity>()) && line.Name == "ItemName")
            {
                Color col = TranscendenceWorld.CosmicPurple;
                Color color = new Color(col.R, col.G, col.B, 0f);
                Color color3 = Color.White;

                //Draw tooltips for expert seraph loot
                if (item.expert)
                {
                    col = Main.DiscoColor;
                    color3 = Color.Black;
                    Main.instance.GraphicsDevice.Textures[1] = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/ExpertModeShader").Value;

                    eff.Parameters["uImageSize1"].SetValue(shaderImage.Size() * 2f);
                    eff.Parameters["uTime"].SetValue(Main.GlobalTimeWrappedHourly * -1.5f);
                }

                int width = (int)FontAssets.MouseText.Value.MeasureString(line.Text).X;
                int height = (int)FontAssets.MouseText.Value.MeasureString(line.Text).Y;

                ChatManager.DrawColorCodedStringShadow(spriteBatch, FontAssets.MouseText.Value, line.Text, new Vector2(line.X, line.Y), Color.Black, 0, Vector2.Zero, Vector2.One, -1, 3);
                spriteBatch.End();
                //Restart spritebatch with the shader active
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, spriteBatch.GraphicsDevice.SamplerStates[0],
                    spriteBatch.GraphicsDevice.DepthStencilState, spriteBatch.GraphicsDevice.RasterizerState, eff, Main.UIScaleMatrix);
                
                spriteBatch.Draw(ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/GlowBloomNoBG").Value, new Rectangle((int)(line.X - (width * 0.9375f)), line.Y - 12, width * 3, 45), color);
                spriteBatch.Draw(ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/GlowBloomNoBG").Value, new Rectangle((int)(line.X - (width * 0.9375f)), line.Y - 12, width * 3, 45), color);

                //Draw the shadered text
                ChatManager.DrawColorCodedStringShadow(spriteBatch, FontAssets.MouseText.Value, line.Text, new Vector2(line.X, line.Y), Color.White, 0, Vector2.Zero, Vector2.One, -1, 2);

                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, spriteBatch.GraphicsDevice.SamplerStates[0],
                    spriteBatch.GraphicsDevice.DepthStencilState, spriteBatch.GraphicsDevice.RasterizerState, null, Main.UIScaleMatrix);

                for (int i = 0; i < 6; i++)
                {
                    int size = (int)(star.Width * starsSize[i] + (float)Math.Sin(TranscendenceWorld.UniversalRotation * 2f) * 4);
                    if (++starResetTimer[i] > 40)
                    {
                        starsRandomX[i] = Main.rand.Next((int)(width * -0.4f), (int)(width * 1.4f));
                        starsRandomY[i] = Main.rand.Next(-75, 75);
                        starsSize[i] = Main.rand.NextFloat(0.15f, 0.3f);
                        starResetTimer[i] = Main.rand.Next(-4, 2);
                        starsRot[i] = MathHelper.ToRadians(Main.rand.Next(-4, 4));
                        starsDir[i] = Main.rand.NextBool(2) ? 1 : 2;
                    }

                    starsRandomX[i] = (int)new Vector2(starsRandomX[i], starsRandomY[i]).RotatedBy(starsRot[i] * starsDir[i] / -5f).X;
                    starsRandomY[i] = (int)new Vector2(starsRandomX[i], starsRandomY[i]).RotatedBy(starsRot[i] * starsDir[i] / -5f).Y;

                    if (starResetTimer[i] < 20 && starResetTimer[i] > 0)
                        starsSize[i] += 0.033f;
                    if (starResetTimer[i] > 20)
                        starsSize[i] = MathHelper.Lerp(starsSize[i], 0f, 1f / 15f);

                    starPositions[i] = new Vector2(line.X + starsRandomX[i], line.Y + (height / 2) + starsRandomY[i]);
                    starPositions[i] = Vector2.Lerp(starPositions[i], new Vector2(line.X + (width * 0.66f), line.Y + (height / 2)), starResetTimer[i] / 80f);

                    spriteBatch.Draw(star, new Rectangle((int)(starPositions[i].X), (int)(starPositions[i].Y), size * 3, size * 3),
                        null, (item.master ? Color.Gold * 0.5f : Color.White * 0.4f), starsRot[i], star.Size() * 0.5f, SpriteEffects.None, 0);
                    spriteBatch.Draw(star, new Rectangle((int)starPositions[i].X, (int)starPositions[i].Y, size, size),
                        null, Color.White, starsRot[i], star.Size() * 0.5f, SpriteEffects.None, 0);
                }

                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState,
                    spriteBatch.GraphicsDevice.DepthStencilState, spriteBatch.GraphicsDevice.RasterizerState, null, Main.UIScaleMatrix);

                ChatManager.DrawColorCodedStringShadow(spriteBatch, FontAssets.MouseText.Value, line.Text, new Vector2(line.X, line.Y), col, 0, Vector2.Zero, Vector2.One);


                ChatManager.DrawColorCodedString(spriteBatch, FontAssets.MouseText.Value, line.Text, new Vector2(line.X, line.Y), color3, 0, Vector2.Zero, Vector2.One);
            }
        }

        public override bool AltFunctionUse(Item item, Player player)
        {
            if (player.GetModPlayer<TranscendencePlayer>().ThrowingGlove && IsAGloveThrowableItem(item) && player.GetModPlayer<TranscendencePlayer>().ThrowingGloveCD == 0)
                return true;
            return base.AltFunctionUse(item, player);
        }

        public override void SetDefaults(Item item)
        {
            if (item.type == ItemID.Zenith)
            {
                item.damage = 180;
                item.rare = ModContent.RarityType<CosmicRarity>();
            }

            if (item.type == ItemID.LongRainbowTrailWings)
                item.expert = false;

            if (item.type == ItemID.Daybloom || item.type == ItemID.Shiverthorn || item.type == ItemID.Fireblossom || item.type == ItemID.Waterleaf || item.type == ItemID.Deathweed || item.type == ItemID.Moonglow)
                ItemID.Sets.ShimmerTransformToItem[item.type] = ModContent.ItemType<ShimmerBlossom>();

            //Why not
            if (item.maxStack == Item.CommonMaxStack)
                item.maxStack = 99999;

            if (item.type == ItemID.VineRopeCoil)
            {
                item.ammo = ItemID.VineRopeCoil;
                item.shoot = ProjectileID.VineRopeCoil;
                item.shootSpeed = 5;
            }
            if (item.type == ItemID.RopeCoil)
            {
                item.ammo = ItemID.VineRopeCoil;
                item.shoot = ProjectileID.RopeCoil;
                item.shootSpeed = 3;
            }
            if (item.type == ItemID.SilkRopeCoil)
            {
                item.ammo = ItemID.VineRopeCoil;
                item.shoot = ProjectileID.SilkRopeCoil;
                item.shootSpeed = 10;
            }
            if (item.type == ItemID.WebRopeCoil)
            {
                item.ammo = ItemID.VineRopeCoil;
                item.shoot = ProjectileID.WebRopeCoil;
                item.shootSpeed = 7;
            }
        }
        public override void ModifyItemLoot(Item item, ItemLoot itemLoot)
        {

            if (item.type == ItemID.FairyQueenBossBag)
            {
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<EasternTalismans>(), 3));
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<ChromaticAegis>(), 4));
            }

            if (item.type == ItemID.GolemBossBag)
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<HealthyJewel>(), 4));

            if (item.type == ItemID.BossBagBetsy)
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<DragonScale>(), 1, 1, 2));

            if (item.type == ItemID.FishronBossBag)
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<FishronPerception>(), 3));

            if (item.type == ItemID.MoonLordBossBag)
            {
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<SlayerOfGiants>(), 1, 2, 3));
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<LunarShield>(), 2));
            }

        }
        public override bool CanUseItem(Item item, Player player)
        {
            if (item.healMana > 0 && player.HasBuff(BuffID.ManaSickness))
                return false;

            if (player.GetModPlayer<TranscendencePlayer>().ZoneSpaceTemple && (item.type == ItemID.Wrench || item.type == ItemID.BlueWrench
                || item.type == ItemID.GreenWrench || item.type == ItemID.YellowWrench || item.type == ItemID.MulticolorWrench || item.type == ItemID.ActuationRod))
                return false;

            return !player.GetModPlayer<TranscendencePlayer>().CannotUseItems;
        }


        //Celestial Starboard is kill
        public override void HorizontalWingSpeeds(Item item, Player player, ref float speed, ref float acceleration)
        {
            if (item.type == ItemID.LongRainbowTrailWings)
                speed = 10;

            if ((item.type == ItemID.BetsyWings || item.type == ItemID.WingsNebula || item.type == ItemID.WingsVortex || item.type == ItemID.LongRainbowTrailWings)
                && player.controlDownHold && player.controlJump && TranscendenceUtils.BossAlive())
                speed = 9;
        }
        public override void VerticalWingSpeeds(Item item, Player player, ref float ascentWhenFalling, ref float ascentWhenRising, ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
        {
            if (item.type == ItemID.LongRainbowTrailWings)
            {
                ascentWhenRising = 0.215f;
                maxAscentMultiplier = 2.15f;
            }

        }

        public override bool? UseItem(Item item, Player player)
        {
            if (player.ItemAnimationJustStarted)
            {
                player.TryGetModPlayer(out TranscendencePlayer mp);

                int rocket = mp.RocketAcc;
                int rocketcd = mp.RocketCD;

                if (rocket != 0 && rocketcd == 0)
                {
                    Projectile.NewProjectile(player.GetSource_ItemUse(item), player.Center, player.DirectionTo(Main.MouseWorld) * 16f,
                        ModContent.ProjectileType<FireworkProjectile>(), 25, 2f, player.whoAmI);
                    mp.RocketCD = 180;
                }

                if (mp.EmpoweringTabletEquipped)
                {
                    int p = ProjectileID.None;

                    if (item.DamageType == DamageClass.Melee || item.DamageType == DamageClass.MeleeNoSpeed)
                        p = ModContent.ProjectileType<MoltenSlash>();
                    if (item.DamageType == DamageClass.Ranged)
                        p = ModContent.ProjectileType<MoltenBullet>();
                    if (item.DamageType == DamageClass.Magic)
                        p = ModContent.ProjectileType<MoltenTrail>();
                    if (item.DamageType == DamageClass.SummonMeleeSpeed)
                        p = ModContent.ProjectileType<MoltenWhip>();

                    Projectile.NewProjectile(player.GetSource_ItemUse(item), player.Center, player.DirectionTo(Main.MouseWorld) * 12f, p, item.damage / 3, item.knockBack, player.whoAmI);
                }
            }

            if (player.GetModPlayer<TranscendencePlayer>().ThrowingGlove && player.GetModPlayer<TranscendencePlayer>().ThrowingGloveCD == 0 && IsAGloveThrowableItem(item) && player.altFunctionUse == 2)
            {
                Projectile.NewProjectile(player.GetSource_FromAI(), player.Center, player.DirectionTo(Main.MouseWorld) * 20, ModContent.ProjectileType<GloveThrowItem>(), (int)(item.damage * 3.5f), item.knockBack, player.whoAmI, item.pick, item.axe);
                player.GetModPlayer<TranscendencePlayer>().ThrowingGloveCD = 180;
            }

            return base.UseItem(item, player);
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            TooltipLine equip = tooltips.Find(y => y.Name == "Defense");

            int plateTimer = Main.LocalPlayer.GetModPlayer<TranscendencePlayer>().SturdyPlateTimer;
            string plateTimer2 = plateTimer < 6000 ? (plateTimer / 60).ToString() + "s" : Math.Round(plateTimer / 60f / 60f).ToString() + "min";
            if (equip != null && plateTimer > 0 && (item.headSlot > 0 || item.bodySlot > 0 || item.legSlot > 0))
                equip.Text += $"[C/5c915d: (+4 / {plateTimer2})]";

            int parryIndex = equip is null ? 1 : tooltips.IndexOf(equip) + 1;

            if (ShieldParryCD != 0)
            {
                ModKeybind mkb = TranscendenceWorld.Guard;
                if (!Main.dedServ && mkb != null)
                {
                    List<string> keys = mkb.GetAssignedKeys();

                    if (keys.Count > 0)
                    {
                        StringBuilder sb = new StringBuilder(10);
                        sb.Append(keys[0]);

                        var generic = new TooltipLine(Mod, "ShieldGeneric", Language.GetTextValue("Mods.TranscendenceMod.Messages.Tooltips.Shields", Main.LocalPlayer.GetModPlayer<TranscendencePlayer>().ParryFocusCost));
                        var cd = new TooltipLine(Mod, "ShieldParryCD", Language.GetTextValue("Mods.TranscendenceMod.Messages.ParryCD", ShieldParryCD / 60f));

                        tooltips.Insert(parryIndex, generic);
                        tooltips.Insert(parryIndex + 1, cd);

                        TooltipLine line = tooltips.FirstOrDefault(x => x.Mod == "TranscendenceMod" && x.Text.Contains("(Unbound Key)"));
                        if (line != null)
                            line.Text = line.Text.Replace("(Unbound Key)", sb.ToString());
                    }
                }
            }

            if (item.type == ItemID.BeetleShell)
            {
                var samenametooltip = new TooltipLine(Mod, "SameNameTooltip", Language.GetTextValue("Mods.TranscendenceMod.Messages.Tooltips.BeetleShellTooltip"));
                tooltips.Add(samenametooltip);
            }
        }
        public override void NetSend(Item item, BinaryWriter writer)
        {
        }
        public override void NetReceive(Item item, BinaryReader reader)
        {
        }
        public override void PostDrawInInventory(Item item, SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Main.LocalPlayer.TryGetModPlayer(out TranscendencePlayer modPlayer);

            if (modPlayer != null)
            {
                //Draw golem CD
                float golemTimer = (float)Math.Round(Main.LocalPlayer.GetModPlayer<TranscendencePlayer>().GolemCD / 60f, 1);
                if (item.type == ModContent.ItemType<LihzardianBulwark>() && modPlayer.GolemCD > 0)
                    ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.ItemStack.Value, golemTimer.ToString() + "s", position, Color.OrangeRed, 0f, Vector2.Zero, Vector2.One);
            }
        }
        public override void UpdateEquip(Item item, Player player)
        {
            if ((item.headSlot > 0 || item.bodySlot > 0 || item.legSlot > 0) && player.GetModPlayer<TranscendencePlayer>().SturdyPlateTimer > 0)
            {
                player.statDefense += 4;
            }
        }
        public override void UpdateAccessory(Item Item, Player player, bool hideVisual)
        {
            if (ShieldParryCD != 0)
            {
                player.GetModPlayer<TranscendencePlayer>().ParryCD = ShieldParryCD;
            }
        }
    }
}
