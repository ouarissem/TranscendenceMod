using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Projectiles.Equipment.Tools;
using TranscendenceMod.Tiles.TilesheetHell.Nature.Farming;

namespace TranscendenceMod.Items.Farming
{
    public class SkullMasterSickle : BaseHoe
    {
        public override ushort soil => (ushort)ModContent.TileType<Soil>();

        public override int range => 5;

        public override void SetDefaults()
        {
            base.SetDefaults();

            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = true;
            Item.rare = ItemRarityID.Yellow;

            Item.width = 22;
            Item.height = 22;

            Item.useAnimation = 14;
            Item.useTime = 14;

            Item.value = Item.sellPrice(gold: 15);
        }

        public override bool AltFunctionUse(Player player) => true;

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2)
            {
                Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<SkullMasterProj>(), 0, 0, player.whoAmI);
                SoundEngine.PlaySound(SoundID.Item71, position);
            }
            else base.Shoot(player, source, position, velocity, type, damage, knockback);

            return false;
        }
    }
}
