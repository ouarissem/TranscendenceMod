using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using TranscendenceMod.Items.Modifiers;
using TranscendenceMod.Miscannellous.UI.ModifierUI;
using TranscendenceMod.Projectiles.Modifiers;

namespace TranscendenceMod.Miscannellous
{
    public class ModifiersItem : GlobalItem
    {
        /*Modifiers*/
        public override bool InstancePerEntity => true;
        public ModifierIDs Modifier;
        public bool ModifiersUnlocked;
        public int ModifierCD;
        public int ModifierCD2;

        public bool BlacksmithGiantHandleAllowed;

        public override GlobalItem Clone(Item from, Item to)
        {
            return from.TryGetGlobalItem<ModifiersItem>(out ModifiersItem it) ? it : base.Clone(from, to);
        }

        public override void SaveData(Item item, TagCompound tag)
        {
            int mod = (int)Modifier;
            tag["Modifier"] = mod;
            if (ModifiersUnlocked) tag["ModifiersUnlocked"] = ModifiersUnlocked;
        }
        public override void LoadData(Item item, TagCompound tag)
        {
            int mod = tag.GetInt("Modifier");
            Modifier = (ModifierIDs)mod;
            ModifiersUnlocked = tag.ContainsKey("ModifiersUnlocked");
        }
        public override void UpdateInventory(Item item, Player player)
        {
            if (Modifier != ModifierIDs.None)
            {
                player.GetModPlayer<TranscendencePlayer>().HasModifiersInventory = true;
            }
        }
        public override void Load()
        {
            base.Load();

            On_Item.AffixName += On_Item_AffixName;
        }

        private string On_Item_AffixName(On_Item.orig_AffixName orig, Item self)
        {
            try
            {
                if (self == null || self.IsAir)
                    return orig(self);

                self.TryGetGlobalItem(out ModifiersItem gi);

                if (gi != null)
                {
                    int mod = (int)gi.Modifier;

                    if (mod > 0 && mod < 900 && self.ModItem is not BaseModifier)
                        return Language.GetTextValue($"Mods.TranscendenceMod.Messages.Tooltips.Modifiers.Names.{mod}") + " " + orig(self);
                }
            }
            catch { }

            return orig(self);
        }

        public override bool PreDrawInWorld(Item item, SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            if (Modifier != ModifierIDs.None && item.ModItem is not BaseModifier)
            {
                //Request effect
                var eff = ModContent.Request<Effect>("TranscendenceMod/Miscannellous/Assets/Shaders/Effects/ModifierShader", AssetRequestMode.ImmediateLoad).Value;

                //Set uImage1 to be Orion Nebula
                Texture2D shaderImage = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/ModifierEffect").Value;
                Main.instance.GraphicsDevice.Textures[1] = shaderImage;

                //Make the texture actually scroll with Main.GlobalTimeWrappedHourly
                eff.Parameters["uImageSize0"].SetValue(new Vector2(500));
                eff.Parameters["uImageSize1"].SetValue(shaderImage.Size());
                eff.Parameters["uTime"].SetValue(-Main.GlobalTimeWrappedHourly * 0.5f);
                eff.Parameters["uSaturation"].SetValue(Main.GlobalTimeWrappedHourly * 0.5f);
                Texture2D sprite = TextureAssets.Item[item.type].Value;

                Main.EntitySpriteDraw(sprite, item.Center - Main.screenPosition, sprite.Frame(), Color.Gray * 0.25f, rotation, sprite.Size() * 0.5f, scale, SpriteEffects.None);

                spriteBatch.End();
                spriteBatch.Begin(default, BlendState.NonPremultiplied, Main.DefaultSamplerState, default, default, eff, Main.GameViewMatrix.TransformationMatrix);

                Main.EntitySpriteDraw(sprite, item.Center - Main.screenPosition, sprite.Frame(), Color.White, rotation, sprite.Size() * 0.5f, scale, SpriteEffects.None);

                spriteBatch.End();
                spriteBatch.Begin(default, default, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);
                return false;
            }
            return base.PreDrawInWorld(item, spriteBatch, lightColor, alphaColor, ref rotation, ref scale, whoAmI);
        }
        private bool bag(Item item)
        {
            Main.LocalPlayer.TryGetModPlayer(out TranscendencePlayer mp);

            if (ModifierContainerUIDrawing.Visible && item.ModItem is BaseModifier &&
                mp.ModifierContainerItem.ModItem is ModifierContainer cont)
            {
                for (int i = 0; i < cont.ItemsInside.Length; i++)
                {
                    if (cont.ItemsInside[i] != null && !cont.ItemsInside[i].IsAir && cont.ItemsInside[i].type == item.type && cont.ItemsInside[i] != item)
                        return true;
                }
            }
            return false;
        }
        public override bool PreDrawInInventory(Item item, SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Main.LocalPlayer.TryGetModPlayer(out TranscendencePlayer mp);


            if (ModifierContainerUIDrawing.Visible && item.ModItem is BaseModifier mod && !bag(item) && mp.ModifierContainerItem.ModItem is ModifierContainer cont && (int)mod.ModifierType < 1000)
            {
                bool insideUI = false;
                for (int i = 0; i < cont.ItemsInside.Length; i++)
                {
                    if (cont.ItemsInside[i] != null && !cont.ItemsInside[i].IsAir && cont.ItemsInside[i] == item)
                        insideUI = true;
                }

                if (!insideUI && TranscendenceUtils.InsideInventory(item))
                {
                    TranscendenceUtils.DrawEntity(item, Color.White, 1.25f + (float)(Math.Sin(TranscendenceWorld.UniversalRotation * 3f) * 0.5f), "bloom", 0f, position + Main.screenPosition, null);
                    TranscendenceUtils.DrawEntity(item, Color.White, 0.75f, "bloom", 0f, position + Main.screenPosition, null);
                }
            }

            if (Modifier != ModifierIDs.None && item.ModItem is not BaseModifier)
            {
                //Request effect
                var eff = ModContent.Request<Effect>("TranscendenceMod/Miscannellous/Assets/Shaders/Effects/ModifierShader", AssetRequestMode.ImmediateLoad).Value;

                //Set uImage1 to be Orion Nebula
                Texture2D shaderImage = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/ModifierEffect").Value;
                Main.instance.GraphicsDevice.Textures[1] = shaderImage;

            
                eff.Parameters["uImageSize0"].SetValue(new Vector2(500));
                eff.Parameters["uImageSize1"].SetValue(shaderImage.Size());
                //Make the texture actually scroll with Main.GlobalTimeWrappedHourly
                eff.Parameters["uTime"].SetValue(-Main.GlobalTimeWrappedHourly * 0.5f);
                eff.Parameters["uSaturation"].SetValue(Main.GlobalTimeWrappedHourly * 0.5f);
                Texture2D sprite = TextureAssets.Item[item.type].Value;

                Main.EntitySpriteDraw(sprite, position, frame, Color.Gray * 0.25f, 0, frame.Size() * 0.5f, scale, SpriteEffects.None);

                spriteBatch.End();
                spriteBatch.Begin(default, BlendState.NonPremultiplied, Main.DefaultSamplerState, default, default, eff, Main.UIScaleMatrix);

                Main.EntitySpriteDraw(sprite, position, frame, Color.White * 2f, 0, frame.Size() * 0.5f, scale, SpriteEffects.None);

                spriteBatch.End();
                spriteBatch.Begin(default, default, default, default, default, null, Main.UIScaleMatrix);
                return false;
            }
            return base.PreDrawInInventory(item, spriteBatch, position, frame, drawColor, itemColor, origin, scale);
        }
        public override void PostDrawInInventory(Item item, SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            if (item.ModItem is BaseModifier || Modifier == ModifierIDs.None)
                return;

            string texString = $"TranscendenceMod/Miscannellous/Assets/Icons/{Modifier}";
            Texture2D sprite = ModContent.Request<Texture2D>(texString).Value;
            spriteBatch.Draw(sprite, position + new Vector2(-16, 0), null, Color.White);
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (Modifier != 0 && item.ModItem is not BaseModifier)
            {
                var modifiername = Language.GetTextValue($"Mods.TranscendenceMod.Messages.Tooltips.Modifiers.{Modifier}");

                var modifier = new TooltipLine(Mod, "Modifier Name", modifiername);
                tooltips.Insert(tooltips.Count, modifier);
            }

            var modifierUnlocked = new TooltipLine(Mod, "Modifier Name", Language.GetTextValue("Mods.TranscendenceMod.Messages.Tooltips.Modifiers.Unlocked"));
            if (ModifiersUnlocked)
                tooltips.Insert(tooltips.Count, modifierUnlocked);
        }
        public override void HoldItem(Item item, Player player)
        {
            if (Modifier == ModifierIDs.Hooked)
                player.fishingSkill += 10;

            if (Modifier == ModifierIDs.CrateMagnet)
                player.GetModPlayer<TranscendencePlayer>().UsingCrateMagnet = true;

            if (Modifier == ModifierIDs.EnchantedPearl)
                player.GetModPlayer<TranscendencePlayer>().PearlMod = true;
        }
        public override void UpdateEquip(Item item, Player player)
        {
            if (Modifier == ModifierIDs.Silky)
                player.GetModPlayer<TranscendencePlayer>().SilkyEgg++;

            if (Modifier == ModifierIDs.Jolly)
                player.GetModPlayer<TranscendencePlayer>().Jolly++;

            if (Modifier == ModifierIDs.Draconic)
            {
                player.GetModPlayer<TranscendencePlayer>().DragonScales++;
                player.statDefense -= 5;
            }

            if (Modifier == ModifierIDs.DangerDetecting)
                player.dangerSense = true;

            if (Modifier == ModifierIDs.GiantSlayer)
                player.GetModPlayer<TranscendencePlayer>().GiantSlayer++;

            if (Modifier == ModifierIDs.Spazzy)
            {
                if (++ModifierCD > 1)
                {
                    int dmg = NPC.downedMoonlord ? 275 : NPC.downedPlantBoss ? 175 : 80;
                    float yvel = Main.MouseWorld.Y > (player.Center.Y + 300) ? 4 : Main.MouseWorld.Y > (player.Center.Y + 150) ? 1 :
                        Main.MouseWorld.Y < (player.Center.Y - 300) ? -4 : Main.MouseWorld.Y < (player.Center.Y - 150) ? -1 : 0;
                    Projectile.NewProjectile(player.GetSource_FromAI(), player.Center - new Vector2(player.direction * 60, 5 + (yvel * 15)), new Vector2(player.direction * 5,
                        yvel),
                        ModContent.ProjectileType<SpazzingEyeFire>(), dmg, 0, player.whoAmI);
                    ModifierCD = 0;
                }
            }
            if (Modifier == ModifierIDs.Luminous)
            {
                player.GetModPlayer<TranscendencePlayer>().CritDamage += 0.05f;
                player.GetModPlayer<TranscendencePlayer>().Luminosity += 0.2f;
                player.gravity *= 1.075f;
            }
            if (Modifier == ModifierIDs.CultistScroll)
            {
                player.GetModPlayer<TranscendencePlayer>().CultScrollsEquipped += 1;
            }
        }
        public override void UpdateAccessory(Item Item, Player player, bool hideVisual)
        {
        }
    }
}
