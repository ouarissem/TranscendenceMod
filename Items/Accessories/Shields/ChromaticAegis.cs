using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TranscendenceMod.Items.Accessories.Shields
{
    public class ChromaticAegis : BaseShield
    {
        public override int Cooldown => 120;

        public override int DefenseAmount => 6;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Pink;
            Item.width = 32;
            Item.height = 24;
            Item.value = Item.sellPrice(gold: 20);
        }

        public override void UpdateEquip(Player player)
        {
            base.UpdateEquip(player);
            player.GetModPlayer<TranscendencePlayer>().EolAegis = true;
        }
        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Texture2D sprite = ModContent.Request<Texture2D>(Texture + "_Glow").Value;
            for (int i = 0; i < 4; i++)
            {
                float pi = MathHelper.TwoPi * i / 4;
                float rot = pi += MathHelper.ToRadians(TranscendenceWorld.Timer) * 4;

                Vector2 pos = position + Vector2.One.RotatedBy(rot) * 2;
                Main.EntitySpriteDraw(sprite, pos, null, Main.DiscoColor * 0.25f, 0, sprite.Size() * 0.5f, scale, SpriteEffects.None);
            }
        }
    }
}
