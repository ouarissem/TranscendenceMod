using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Miscannellous.UI.ModifierUI;
using TranscendenceMod.NPCs.Passive;

namespace TranscendenceMod.Items.Tools
{
    public class TinkererPhone : ModItem
    {
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.value = Item.sellPrice(gold: 10);
            Item.rare = ItemRarityID.LightRed;

            Item.UseSound = SoundID.Item4;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useTime = 20;
            Item.useAnimation = 20;
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            TranscendenceUtils.DrawItemGlowmask(Item, rotation, scale, Texture);
        }
        public override bool? UseItem(Player player)
        {
            if (player.ItemAnimationJustStarted && !ModifierApplierUIDrawing.Visible)
            {
                if (!NPC.AnyNPCs(ModContent.NPCType<Tinkerer>()))
                {
                    DialogUI.SpawnDialog(Language.GetTextValue("Mods.TranscendenceMod.Messages.BlacksmithPhoneError"), player.Center - new Vector2(0, 42), 90, Color.Gray);
                    return base.UseItem(player);
                }

                NPC npc = new NPC();
                npc.Center = player.Center;

                player.GetModPlayer<TranscendencePlayer>().ModifierUINPCPos = npc;
                player.GetModPlayer<TranscendencePlayer>().TinkererHappiness = 2f;
                player.GetModPlayer<TranscendencePlayer>().ModifierUIOnPhone = true;

                ModifierApplierUIDrawing.Visible = true;

                Main.CloseNPCChatOrSign();
            }
            return base.UseItem(player);
        }
    }
}
