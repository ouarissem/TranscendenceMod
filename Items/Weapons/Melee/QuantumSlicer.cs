using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Miscannellous.Rarities;
using TranscendenceMod.Projectiles.Weapons.Melee;

namespace TranscendenceMod.Items.Weapons.Melee
{
    public class QuantumSlicer : ModItem
    {
        public int boomerang = ModContent.ProjectileType<QuantumSlicerProj>();
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Melee;
            Item.damage = 255;

            Item.knockBack = 2.25f;
            Item.width = 18;
            Item.height = 18;

            Item.shoot = boomerang;
            Item.shootSpeed = 10;

            Item.noMelee = true;
            Item.noUseGraphic = true;

            Item.useTime = 120;
            Item.useAnimation = 120;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item1;

            Item.value = Item.sellPrice(gold: 17, silver: 50);
            Item.rare = ModContent.RarityType<ModdedPurple>();
            Item.crit = 5;
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            TranscendenceUtils.DrawItemGlowmask(Item, rotation, scale, Texture, new Vector2(0, -2));
        }
        public override bool? UseItem(Player player)
        {
            if (player.altFunctionUse != 2 && player.ownedProjectileCounts[boomerang] > 0)
                return false;
            else return true;
        }
        public override bool CanShoot(Player player)
        {
            return player.altFunctionUse != 2 && player.ownedProjectileCounts[boomerang] < 1;
        }

        public override bool AltFunctionUse(Player player) => player.ownedProjectileCounts[boomerang] == 1;
    }
}