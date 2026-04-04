using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Items.Accessories.Expert;
using TranscendenceMod.Items.Consumables.Boss;
using TranscendenceMod.Items.Consumables.Placeables;
using TranscendenceMod.Items.Materials.LargeRecipes;
using TranscendenceMod.Items.Materials.MobDrops;
using TranscendenceMod.Items.Weapons.Magic;
using TranscendenceMod.Items.Weapons.Melee;
using TranscendenceMod.Miscanellous.UI.Achievements.Tasks;
using TranscendenceMod.Miscannellous.Rarities;
using static TranscendenceMod.TranscendenceWorld;

namespace TranscendenceMod.Items
{
    public class DownedBossSetter : ModItem
    {
        public int CurSelection = 0;
        public override void SetDefaults()
        {
            Item.useTime = 10;
            Item.useAnimation = 10;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Mech;

            Item.width = 18;
            Item.height = 18;

            Item.rare = ModContent.RarityType<Brown>();
            CurSelection = 0;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            base.ModifyTooltips(tooltips);

            var tt = new TooltipLine(Mod, "DownedSelection", "Selected: " + text);
            tt.OverrideColor = Color.Gold;

            tooltips.Add(tt);
        }

        public override bool AltFunctionUse(Player player) => true;

        private string text = "";
        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            base.PostDrawInInventory(spriteBatch, position, frame, drawColor, itemColor, origin, scale);

            Texture2D sprite = TextureAssets.BlackTile.Value;
            int item = 0;
            switch (CurSelection)
            {
                case 0: item = ModContent.ItemType<VolcanicRemains>(); break;
                case 1: item = ModContent.ItemType<FrozenMaw>(); break;
                case 2: item = ModContent.ItemType<WindDragonsClaw>(); break;
                case 3: item = ModContent.ItemType<ElectricalComponent>(); break;
                case 4: item = ModContent.ItemType<CosmicArtifact>(); break;
            }
            sprite = TextureAssets.Item[item].Value;
            spriteBatch.Draw(sprite, position, null, Color.White);
        }
        public override bool? UseItem(Player player)
        {
            if (player.ItemAnimationJustStarted)
            {

                if (player.altFunctionUse == 2)
                {
                    CurSelection++;

                    switch (CurSelection)
                    {
                        case 0: text = "Flame Guardian"; break;
                        case 1: text = "Frost Serpent"; break;
                        case 2: text = "Atmospheron"; break;
                        case 3: text = "Project Nucleus"; break;
                        case 4: text = "Celestial Seraph"; break;
                        case 5: CurSelection = 0; goto case 0;
                    }

                    Main.NewText("Selected: " + text);
                }
                else
                {
                    switch (CurSelection)
                    {
                        case 0:
                            SetClearStatus(Bosses.FlameGuardian); break;
                        case 1:
                            SetClearStatus(Bosses.FrostSerpent); break;
                        case 2:
                            SetClearStatus(Bosses.Atmospheron); break;
                        case 3:
                            SetClearStatus(Bosses.ProjectNucleus); break;
                        case 4:
                            SetClearStatus(Bosses.CelestialSeraph); break;
                    }

                    void SetClearStatus(Bosses boss)
                    {
                        if (Downed.Contains(boss))
                            Downed.Remove(boss);
                        else Downed.Add(boss);

                        bool downed = Downed.Contains(boss);
                        Main.NewText("Flags for " + text + " set to " + downed.ToString() + "!");
                    }
                }

            }
            return base.UseItem(player);
        }
    }
}