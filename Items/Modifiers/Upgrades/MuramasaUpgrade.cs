using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Items.Materials.MobDrops;
using TranscendenceMod.Items.Weapons.Melee;
using TranscendenceMod.Miscannellous.Rarities;
using TranscendenceMod.Tiles.BigTiles;

namespace TranscendenceMod.Items.Modifiers.Upgrades
{
    public class MuramasaUpgrade : BaseModifier
    {
        public override bool CanBeApplied(Item item) => item.type == ItemID.Muramasa;
        public override int RequiredItem => ItemID.Ectoplasm;
        public override int RequiredAmount => 24;
        public override ModifierIDs ModifierType => ModifierIDs.MuramasaUpgrade;
        public override int CraftingResultItem => ModContent.ItemType<UpgradedMuramasa>();


        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.width = 16;
            Item.height = 22;

            Item.value = Item.sellPrice(gold: 20);
            Item.rare = ModContent.RarityType<Brown>();
        }
        public override Color? GetAlpha(Color lightColor) => Color.White;
        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Texture2D sprite = ModContent.Request<Texture2D>($"{Texture}").Value;
            for (int i = 0; i < 4; i++)
            {
                float pi = MathHelper.TwoPi * i / 4;
                float rot = pi + Main.GlobalTimeWrappedHourly;

                Vector2 pos = position + Vector2.One.RotatedBy(rot) * 4;
                spriteBatch.Draw(sprite, pos - new Vector2(Item.width * 0.9f, Item.height * 0.85f), null, Color.Blue * 0.2f, 0, Vector2.Zero, 0.75f, SpriteEffects.None, 0);
            }
            return base.PreDrawInInventory(spriteBatch, position, frame, drawColor, itemColor, origin, scale);
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<PoseidonsTide>(), 16)
                .AddIngredient(ModContent.ItemType<SunburntAlloy>(), 12)
                .AddIngredient(ItemID.GoldBar, 12)
                .AddTile(ModContent.TileType<Oceation>())
                .Register();
        }
    }
}
