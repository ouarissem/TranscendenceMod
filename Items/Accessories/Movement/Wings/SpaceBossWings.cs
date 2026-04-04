using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Items.Consumables.Placeables.SpaceBiome;
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Miscannellous.Rarities;
using TranscendenceMod.Tiles.BigTiles.Furniture;

namespace TranscendenceMod.Items.Accessories.Movement.Wings
{
    [AutoloadEquip(EquipType.Wings)]
    public class SpaceBossWings : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.rare = ModContent.RarityType<CosmicRarity>();
            Item.width = 29;
            Item.height = 24;
            Item.accessory = true;
            Item.value = Item.sellPrice(gold: 35);
            ArmorIDs.Wing.Sets.Stats[Item.wingSlot] = new WingStats(300, 13, 0.5f, true, 10);
        }
        public override void UpdateEquip(Player player)
        {
            base.UpdateEquip(player);
            player.GetModPlayer<TranscendencePlayer>().CosmicWings = true;
        }
        public override void HorizontalWingSpeeds(Player player, ref float speed, ref float acceleration)
        {
            speed = 11;
            acceleration = 0.65f;

            if (player.controlDown && player.controlJump && player.wingTime > 0)
            {
                acceleration = 1.25f;

                player.position.Y -= player.velocity.Y;

                if (player.velocity.Y > 0.1f)
                    player.velocity.Y = 0.1f;

                else if (player.velocity.Y < -0.1f)
                    player.velocity.Y = -0.1f;
            }
        }
        public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising, ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
        {
            ascentWhenFalling = 0.9f;
            ascentWhenRising = 0.185f;
            maxCanAscendMultiplier = 1;
            maxAscentMultiplier = 3;
            constantAscend = 0.135f;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<StarcraftedAlloy>())
            .AddIngredient(ModContent.ItemType<AetherRootItem>(), 18)
            .AddIngredient(ItemID.SoulofFlight, 20)
            .AddTile(ModContent.TileType<StarcraftedForge>())
            .Register();
        }
    }
}
