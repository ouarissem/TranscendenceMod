using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI.Chat;

namespace TranscendenceMod.Items.Accessories.Shields
{
    public class JungleShield2 : BaseShield
    {
        public override int Cooldown => 90;

        public override int DefenseAmount => 8;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Yellow;
            Item.width = 29;
            Item.height = 38;
            Item.value = Item.sellPrice(gold: 7, silver: 25);
        }

        public override void UpdateEquip(Player player)
        {
            base.UpdateEquip(player);
            player.GetModPlayer<TranscendencePlayer>().BeetleShield = true;
        }

        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            base.PostDrawInInventory(spriteBatch, position, frame, drawColor, itemColor, origin, scale);
            Main.LocalPlayer.TryGetModPlayer(out TranscendencePlayer modPlayer);

            if (modPlayer.ShellCrumble > 0)
                ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.ItemStack.Value, Main.LocalPlayer.GetModPlayer<TranscendencePlayer>().ShellCrumble.ToString(), position - new Vector2(-2, 12), Color.Lime, 0f, Vector2.Zero, Vector2.One * 0.85f);

            float num = (float)Math.Round((2700f - Main.LocalPlayer.GetModPlayer<TranscendencePlayer>().ShellCrumbleCD) / 60f, 1);
            if (modPlayer.ShellCrumbleCD > 0 && modPlayer.ShellCrumbleCD < 2700)
                ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.ItemStack.Value, num.ToString() + "s", position, Color.Gold, 0f, Vector2.Zero, Vector2.One);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<JungleShield1>())
            .AddIngredient(ItemID.BeetleHusk, 12)
            .AddIngredient(ItemID.JungleSpores, 8)
            .AddTile(TileID.MythrilAnvil)
            .Register();
        }
    }
}
