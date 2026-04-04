using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI.Chat;
using TranscendenceMod.Miscannellous.Rarities;

namespace TranscendenceMod.Items
{
    public class PortalBox : ModItem
    {
        public int CurSelection;
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.useTime = 20;
            Item.useAnimation = 20;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Mech;

            Item.width = 18;
            Item.height = 18;

            Item.value = Item.sellPrice(gold: 7, silver: 50);
            Item.rare = ItemRarityID.Green;

        }
        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            base.PostDrawInInventory(spriteBatch, position, frame, drawColor, itemColor, origin, scale);

            ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.MouseText.Value, (CurSelection + 1).ToString(), position + new Vector2(10, 0), Color.Magenta, 0f, Vector2.Zero, Vector2.One);
        }
        public override bool AltFunctionUse(Player player) => true;
        public override bool? UseItem(Player player)
        {
            if (player.ItemAnimationJustStarted)
            {

                if (player.altFunctionUse == 2)
                {
                    if (++CurSelection > 2)
                        CurSelection = 0;
                    DialogUI.SpawnDialog((CurSelection + 1).ToString(), player.Top - new Vector2(0, 42), 45, Color.HotPink);
                }
                else
                    player.GetModPlayer<TranscendencePlayer>().PortalBoxPositions[CurSelection] = player.Center;

            }
            return base.UseItem(player);
        }
    }
}