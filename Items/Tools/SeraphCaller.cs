using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous.Rarities;

namespace TranscendenceMod.Items.Tools
{
    public class SeraphCaller : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 14;
            Item.height = 18;
            Item.maxStack = 9999;

            Item.rare = ModContent.RarityType<CosmicRarity>();
            Item.value = Item.sellPrice(gold: 5);

            Item.UseSound = SoundID.Item35;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 20;
            Item.useAnimation = 20;
        }
        public override bool CanUseItem(Player player)
        {
            return false;
            //bool flatSurface = !Collision.SolidTiles(player.Center - new Vector2(750f), 1500, 500);
            //return !NPC.AnyNPCs(ModContent.NPCType<TalkingSeraph>()) && !Main.dayTime && player.ZoneOverworldHeight && flatSurface && !TranscendenceUtils.BossAlive();
        }
        public override bool? UseItem(Player player)
        {
            string topic = "0";
            int l = 0;
            string[] parameters = new string[4];

            switch (Main.rand.Next(1, 15))
            {
            }

            /*if (player.whoAmI == Main.myPlayer)
            {
                SeraphEncounters.RegisterEncounter(player, true, l, $"CasualTalk.Generic{topic}", TalkingSeraph.TopicType.CasualTalk, parameters);
            }*/
            return true;
        }
    }
}
