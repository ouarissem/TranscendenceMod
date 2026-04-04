using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Items.Materials.MobDrops;
using TranscendenceMod.Miscannellous.Rarities;

namespace TranscendenceMod.Items.Armor.Hats
{
    [AutoloadEquip(EquipType.Head)]
    public class BrainOfAnnihilation : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.rare = ModContent.RarityType<Brown>();
            Item.defense = 20;

            Item.value = Item.sellPrice(gold: 10);
        }
        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<TranscendencePlayer>().OcramHelmet = true;

            player.GetDamage(DamageClass.Generic) *= 1.2f;
            player.GetCritChance(DamageClass.Generic) *= 1.2f;
            player.manaRegen += 48;

            player.statLifeMax2 += 50;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<GalaxyAlloy>(), 8);
            recipe.AddIngredient(ItemID.RottenChunk, 20);
            recipe.AddIngredient(ItemID.SoulofLight, 8);
            recipe.AddIngredient(ItemID.SoulofNight, 8);
            recipe.AddIngredient(ItemID.SoulofFlight, 8);
            recipe.AddIngredient(ItemID.SoulofSight, 8);
            recipe.AddIngredient(ItemID.SoulofMight, 8);
            recipe.AddIngredient(ItemID.SoulofFright, 8);
            recipe.AddIngredient(ModContent.ItemType<SoulOfKnight>(), 12);
            recipe.AddTile(TileID.DemonAltar);
            recipe.Register();

            Recipe recipe2 = CreateRecipe();
            recipe2.AddIngredient(ModContent.ItemType<GalaxyAlloy>(), 8);
            recipe2.AddIngredient(ItemID.Vertebrae, 20);
            recipe2.AddIngredient(ItemID.SoulofLight, 8);
            recipe2.AddIngredient(ItemID.SoulofNight, 8);
            recipe2.AddIngredient(ItemID.SoulofFlight, 8);
            recipe2.AddIngredient(ItemID.SoulofSight, 8);
            recipe2.AddIngredient(ItemID.SoulofMight, 8);
            recipe2.AddIngredient(ItemID.SoulofFright, 8);
            recipe2.AddIngredient(ModContent.ItemType<SoulOfKnight>(), 8);
            recipe2.AddTile(TileID.DemonAltar);
            recipe2.Register();
        }
    }
}
