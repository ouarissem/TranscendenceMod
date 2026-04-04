using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using ReLogic.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.UI.Chat;
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Items.Modifiers;
using TranscendenceMod.Items.Modifiers.Upgrades;
using TranscendenceMod.Items.Weapons.Melee;
using TranscendenceMod.Miscanellous.UI.Achievements.Tasks;
using TranscendenceMod.Miscannellous.UI.Achievements;

namespace TranscendenceMod.Miscannellous.UI.Achievements
{
    public class AchievementsUI : UIState
    {
        // Code is absolute dogshit, but oh well
        public override void OnInitialize()
        {
            Texture2D sprite = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/UI/Achievements/AchBookBG", AssetRequestMode.ImmediateLoad).Value;

            UIElement modPanel = new UIElement();
            modPanel.Width.Set(500f, 0f);
            modPanel.Height.Set(375f, 0f);

            modPanel.Top.Set(-187.5f, 0.5f);
            modPanel.Left.Set(-250f, 0.5f);

            Append(modPanel);

            UIImage img = new UIImage(sprite);

            img.Color = Color.White;
            img.Left = new StyleDimension(-250f, 0.5f);
            img.Top = new StyleDimension(-147.5f, 0.5f);

            img.OnUpdate += Panel_OnUpdate;

            modPanel.Append(img);

            UIElement parry = new Parry();
            modPanel.Append(parry);

            UIElement snowman = new Snowman();
            modPanel.Append(snowman);

            UIElement modifier = new Modifier();
            modPanel.Append(modifier);

            UIElement modifierbag = new ModBag();
            modPanel.Append(modifierbag);

            UIElement cosmicNPC = new CosmicNPC();
            modPanel.Append(cosmicNPC);

            UIElement timedial = new TimeDial();
            modPanel.Append(timedial);

            UIElement hardmetal = new Hardmetal();
            modPanel.Append(hardmetal);

            UIElement processer = new Procession();
            modPanel.Append(processer);

            UIElement volcanic = new Volcanic();
            modPanel.Append(volcanic);

            UIElement chalice = new Chalice();
            modPanel.Append(chalice);

            UIElement Sans = new sans();
            modPanel.Append(Sans);

            UIElement muramasa = new Muramasa();
            modPanel.Append(muramasa);

            UIElement wall = new Wall();
            modPanel.Append(wall);

            UIElement eol = new Empress();
            modPanel.Append(eol);

            UIElement moon = new Moonlord();
            modPanel.Append(moon);

            UIElement serpent = new FrostSerpent();
            modPanel.Append(serpent);

            UIElement atmospheron = new Atmospheron();
            modPanel.Append(atmospheron);

            UIElement poseidon = new Poseidon();
            modPanel.Append(poseidon);

            UIElement nuc1 = new Nucleus1();
            modPanel.Append(nuc1);

            UIElement nuc2 = new Nucleus2();
            modPanel.Append(nuc2);

            UIElement artifact = new Artifact();
            modPanel.Append(artifact);

            UIElement seraph = new Seraph();
            modPanel.Append(seraph);

            UIElement starforge = new SeraphForge();
            modPanel.Append(starforge);

            UIElement eolchal = new EmpressChallenge();
            modPanel.Append(eolchal);

            UIElement nucchal = new NucleusChallenge();
            modPanel.Append(nucchal);

            UIElement ttchal = new TwentyTwo();
            modPanel.Append(ttchal);

            UIElement dchal = new DragonChallenge();
            modPanel.Append(dchal);

            UIElement begin = new Begin();
            modPanel.Append(begin);

        }

        private void Panel_OnUpdate(UIElement affectedElement)
        {
            if (!QuestBookUIDrawing.Visible)
                return;

            if (!Main.playerInventory)
                QuestBookUIDrawing.Visible = false;

            if (affectedElement.IsMouseHovering)
                Main.LocalPlayer.mouseInterface = true;

        }
    }
}
namespace TranscendenceMod.Miscannellous.UI.Achievements
{
    [Autoload(Side = ModSide.Client)]
    public class QuestBookUIDrawing : ModSystem
    {
        internal AchievementsUI mauis;
        private UserInterface mauis2;
        public static bool Visible;

        public override void Load()
        {
            mauis = new AchievementsUI();
            mauis.Activate();

            mauis2 = new UserInterface();
            mauis2.SetState(mauis);
        }
        public override void UpdateUI(GameTime gameTime)
        {
            if (Visible)
            {
                mauis?.Update(gameTime);
                mauis2?.Update(gameTime);
            }
        }
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (mouseTextIndex != -1)
            {
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer("TranscendenceMod: Quest Book UI", delegate
                {
                    if (Visible) mauis2.Draw(Main.spriteBatch, new GameTime());
                    return true;
                }, InterfaceScaleType.UI));
            }
        }
    }
}

