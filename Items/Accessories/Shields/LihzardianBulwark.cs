using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI.Chat;
using static AssGen.Assets;

namespace TranscendenceMod.Items.Accessories.Shields
{
    public class LihzardianBulwark : BaseShield
    {
        public override int Cooldown => 90;

        public override int DefenseAmount => 12;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Yellow;
            Item.width = 26;
            Item.height = 18;
            Item.value = Item.sellPrice(gold: 12, silver: 50);
        }

        public override void UpdateEquip(Player player)
        {
            base.UpdateEquip(player);
            player.GetModPlayer<TranscendencePlayer>().LihzardianBulwarkEquipped = true;
        }

        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            base.PostDrawInInventory(spriteBatch, position, frame, drawColor, itemColor, origin, scale);
            Main.LocalPlayer.TryGetModPlayer(out TranscendencePlayer modPlayer);

            //Draw golem CD
            float golemTimer = (float)Math.Round(Main.LocalPlayer.GetModPlayer<TranscendencePlayer>().GolemCD / 60f, 1);
            if (modPlayer.GolemCD > 0)
                ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.ItemStack.Value, golemTimer.ToString() + "s", position, Color.OrangeRed, 0f, Vector2.Zero, Vector2.One);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<JungleShield1>())
            .AddIngredient(ItemID.LunarTabletFragment, 32)
            .AddIngredient(ItemID.RichMahogany, 20)
            .AddIngredient(ItemID.BeetleHusk, 4)
            .AddTile(TileID.MythrilAnvil)
            .Register();
        }
    }
}
