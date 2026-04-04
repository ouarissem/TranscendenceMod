using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Items.Materials.MobDrops;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Items.Accessories.Shields
{
    public class MoltenShield : BaseShield
    {
        public override int Cooldown => 120;

        public override int DefenseAmount => 3;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Orange;
            Item.width = 26;
            Item.height = 22;
            Item.value = Item.sellPrice(gold: 1, silver: 75);
        }
        public override void UpdateEquip(Player player)
        {
            base.UpdateEquip(player);
            player.GetModPlayer<TranscendencePlayer>().MoltenShieldEquipped = true;
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            TranscendenceUtils.DrawItemGlowmask(Item, rotation, scale, Texture);
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.HellstoneBar, 14)
            .AddIngredient(ModContent.ItemType<VolcanicRemains>(), 6)
            .AddTile(TileID.Hellforge)
            .Register();
        }
    }
}