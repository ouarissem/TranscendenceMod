using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Buffs.Items;
using TranscendenceMod.Items.Materials.MobDrops;
using TranscendenceMod.Miscannellous.GlobalStuff;
using TranscendenceMod.Miscannellous.Rarities;
using TranscendenceMod.Tiles.BigTiles;

namespace TranscendenceMod.Items.Accessories.Vanity
{
    public class SuspiciousLookingEmergencyButton : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            SetupDrawing();
        }

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;

            Item.rare = ModContent.RarityType<CosmicRarity>();
            Item.accessory = true;
            Item.vanity = true;
        }
        public override void UpdateVanity(Player player)
        {
            player.GetModPlayer<TranscendencePlayer>().Sussy = true;
            player.AddBuff(ModContent.BuffType<AmogusTrans>(), 1);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.AddBuff(ModContent.BuffType<AmogusTrans>(), 1);
            player.GetModPlayer<TranscendencePlayer>().Sussy = true;
        }
        public override void Load()
        {
            if (Main.netMode != NetmodeID.Server)
            {
                EquipLoader.AddEquipTexture(Mod, $"{Texture}_Head", EquipType.Head, this);
                EquipLoader.AddEquipTexture(Mod, $"{Texture}_Body", EquipType.Body, this);
                EquipLoader.AddEquipTexture(Mod, $"{Texture}_Legs", EquipType.Legs, this);
            }
        }
        private void SetupDrawing()
        {
            if (Main.netMode != NetmodeID.Server)
            {
                int head = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Head);
                int body = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Body);
                int legs = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Legs);

                ArmorIDs.Body.Sets.HidesTopSkin[body] = true;
                ArmorIDs.Body.Sets.HidesBottomSkin[body] = true;
                ArmorIDs.Legs.Sets.HidesBottomSkin[legs] = true;
                ArmorIDs.Legs.Sets.HidesTopSkin[legs] = true;
                ArmorIDs.Head.Sets.DrawHead[head] = false;
            }
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddRecipeGroup(nameof(ItemID.SilverBar), 14)
            .AddIngredient(ItemID.ThrowingKnife, 99)
            .AddIngredient(ItemID.Wire, 99)
            .AddIngredient(ModContent.ItemType<PulverizedPlanet>(), 12)
            .AddTile(ModContent.TileType<ShimmerAltar>())
            .Register();
        }
    }
}
