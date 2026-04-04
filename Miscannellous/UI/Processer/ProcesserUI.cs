using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
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
using TranscendenceMod.Items.Consumables.Placeables.SpaceBiome;
using TranscendenceMod.Items.Farming;
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Items.Modifiers;
using TranscendenceMod.Items.Modifiers.Upgrades;
using TranscendenceMod.Items.Weapons.Melee;
using TranscendenceMod.Miscannellous.UI.Processer;
using TranscendenceMod.Miscannellous.UI.Processer.RecipeButtons;

namespace TranscendenceMod.Miscannellous.UI.Processer
{
    public class ProcesserUI : UIState
    {
        public ResultSlot rslot;
        public UIText text;
        public UIText recText;
        public static Item[] itemsInside = new Item[16];
        public static int[] PreviewItems = new int[16];

        public override void OnInitialize()
        {
            UIPanel modPanel = new UIPanel();

            modPanel.Width.Set(425f, 0f);
            modPanel.Height.Set(425f, 0f);

            modPanel.BackgroundColor = new Color(0.05f, 0.15f, 0.3f, 0.85f);
            modPanel.OnUpdate += Panel_OnUpdate;

            Append(modPanel);


            UIPanel recipeBook = new UIPanel();

            recipeBook.Width.Set(125f, 0f);
            recipeBook.Height.Set(275f, 0f);

            recipeBook.BackgroundColor = new Color(0.05f, 0.15f, 0.3f, 0.85f);
            recipeBook.OnUpdate += RecipeBook_OnUpdate;

            Append(recipeBook);



            CopperOre cop = new CopperOre();
            recipeBook.Append(cop);

            TinOre tin = new TinOre();
            recipeBook.Append(tin);

            IronOre iro = new IronOre();
            recipeBook.Append(iro);

            LeadOre lea = new LeadOre();
            recipeBook.Append(lea);

            SilverOre sil = new SilverOre();
            recipeBook.Append(sil);

            TungstenOre tun = new TungstenOre();
            recipeBook.Append(tun);

            GoldOre god = new GoldOre();
            recipeBook.Append(god);

            PlatOre pla = new PlatOre();
            recipeBook.Append(pla);

            CarbonOreRecipe car = new CarbonOreRecipe();
            recipeBook.Append(car);

            SunburntAlloys sba = new SunburntAlloys();
            recipeBook.Append(sba);

            LunarBar lun = new LunarBar();
            recipeBook.Append(lun);

            GalaxyAlloys gay = new GalaxyAlloys();
            recipeBook.Append(gay);

            StarcraftAlloy sta = new StarcraftAlloy();
            recipeBook.Append(sta);



            UIPanel titlePanel = new UIPanel();

            titlePanel.Width.Set(425f, 0f);
            titlePanel.Height.Set(30f, 0f);

            titlePanel.Left.Set(0f, 0f);
            titlePanel.Top.Set(0f, 0f);

            titlePanel.BackgroundColor = new Color(0.21f, 0.11f, 0.38f, 0.85f);
            titlePanel.OnUpdate += TitlePanel_OnUpdate;

            modPanel.Append(titlePanel);

            text = new UIText("Process");
            text.HAlign = 0.5f;
            text.VAlign = 0.5f;
            titlePanel.Append(text);

            UIPanel recTitle = new UIPanel();

            recTitle.Width.Set(0, 1f);
            recTitle.Height.Set(30f, 0f);

            recTitle.Left.Set(0f, 0f);
            recTitle.Top.Set(0f, 0f);

            recTitle.BackgroundColor = new Color(0.21f, 0.11f, 0.38f, 0.85f);
            recTitle.OnUpdate += RecTitle_OnUpdate;

            recipeBook.Append(recTitle);

            recText = new UIText("Recipes");
            recText.HAlign = 0.5f;
            recText.VAlign = 0.5f;
            recTitle.Append(recText);



            UIImageButton modButtonExit = new UIImageButton(ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/UI/ModifierUI/ModifierUIClose"));

            modButtonExit.Width.Set(18f, 0f);
            modButtonExit.Height.Set(18f, 0f);
            modButtonExit.SetHoverImage(ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/UI/ModifierUI/ModifierUIClose_Hover"));

            modButtonExit.Left.Set(0f, 0.9f);
            modButtonExit.Top.Set(0f, 0.1f);

            modButtonExit.OnLeftClick += Button_OnLeftClick;

            modPanel.Append(modButtonExit);

            UIImageButton modButtonCombine = new UIImageButton(ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/UI/Processer/ProcessButton"));

            modButtonCombine.Width.Set(124f, 0f);
            modButtonCombine.Height.Set(24f, 0f);

            modButtonCombine.Left.Set(-62f, 0.5f);
            modButtonCombine.Top.Set(0f, 0.8f);

            modButtonCombine.OnLeftClick += Process;

            modPanel.Append(modButtonCombine);

            int am = 0;
            int row = 0;
            for (int i = 0; i < 16; i++)
            {
                if (++am > 4)
                {
                    row++;
                    am = 1;
                }

                ProcessionSlot slot = new ProcessionSlot();
                slot.xMod2 += 42 * am;
                slot.yMod2 += 42 * row;
                slot.HAlign = 0.5f;
                slot.VAlign = 0.5f;
                slot.index = i;
                modPanel.Append(slot);
            }

            rslot = new ResultSlot();
            rslot.HAlign = 0.5f;
            rslot.VAlign = 0.5f;
            rslot.index = 20;
            modPanel.Append(rslot);
        }

        private void RecTitle_OnUpdate(UIElement affectedElement)
        {
            recText.SetText(Language.GetTextValue("Mods.TranscendenceMod.Messages.RecipeBookTitle"));
        }

        private void RecipeBook_OnUpdate(UIElement affectedElement)
        {
            affectedElement.Top.Set(-425f / 2f, 0.5f);
            affectedElement.Left.Set(425f / 2f + 10f, 0.5f);
            affectedElement.Width.Set(215f, 0f);

            if (affectedElement.IsMouseHovering)
            {
                Main.LocalPlayer.mouseInterface = true;
            }
        }

        public void Recipe(int result, int[] recipe)
        {
            int am = recipe[16];
            int c = 0;

            for (int i = 0; i < 16; i++)
            {
                if (itemsInside[i].type == recipe[i])
                    c++;
                else c--;
            }


            if (c >= 16)
            {
                if (!rslot.Item2.IsAir)
                {
                    if (rslot.Item2.type == result)
                        rslot.Item2.stack += am;
                    else return;
                }
                else rslot.Item2 = new Item(result, am);

                for (int i = 0; i < 16; i++)
                {
                    if (itemsInside[i].stack > 1)
                        itemsInside[i].stack -= 1;
                    else itemsInside[i].TurnToAir();
                }

                if (!Main.dedServ)
                    SoundEngine.PlaySound(SoundID.AchievementComplete);
            }
        }

        private void Process(UIMouseEvent evt, UIElement listeningElement)
        {
            Recipe(ItemID.CopperOre, ProcessRecipes.CopperOre());
            Recipe(ItemID.TinOre, ProcessRecipes.TinOre());
            Recipe(ItemID.LunarBar, ProcessRecipes.Luminite());
            Recipe(ModContent.ItemType<CarbonOre>(), ProcessRecipes.CarbonOre());
            Recipe(ModContent.ItemType<SunburntAlloy>(), ProcessRecipes.SunburntAlloy());
            Recipe(ModContent.ItemType<GalaxyAlloy>(), ProcessRecipes.GalaxyAlloy());
            Recipe(ModContent.ItemType<StarcraftedAlloy>(), ProcessRecipes.StarcraftedAlloy());

            if (!Main.dedServ)
                SoundEngine.PlaySound(SoundID.Research);
        }

        private void TitlePanel_OnUpdate(UIElement affectedElement)
        {
            text.SetText(Language.GetTextValue("Mods.TranscendenceMod.Messages.Process"));
        }

        private void Panel_OnUpdate(UIElement affectedElement)
        {
            if (!ProcesserUIDrawing.Visible)
                return;

            affectedElement.Top.Set(-425f / 2f, 0.5f);
            affectedElement.Left.Set(-425f / 2f, 0.5f);

            if (affectedElement.IsMouseHovering)
            {
                Main.LocalPlayer.mouseInterface = true;
            }
        }

        private void Button_OnLeftClick(UIMouseEvent evt, UIElement listeningElement)
        {
            if (!ProcesserUIDrawing.Visible)
                return;

            if (Main.netMode != NetmodeID.Server)
                SoundEngine.PlaySound(SoundID.MenuClose);

            ProcesserUIDrawing.Visible = false;
        }
    }
    public class ResultSlot : BaseProcessionSlot
    {
        public override int xMod => 84 + 21;

        public override int yMod => 200;

        public override bool Locked => true;

        public override string icon => "TranscendenceMod/Miscannellous/UI/ModifierUI/ModifiableItemIcon";
    }
    public class ProcessionSlot : BaseProcessionSlot
    {
        public int xMod2; 
        public int yMod2;
        public override int xMod => xMod2;

        public override int yMod => yMod2;

        public override bool Locked => false;

        public override string icon => "TranscendenceMod/Miscannellous/UI/ModifierUI/ModifierIngredientIcon";
    }
    public abstract class BaseProcessionSlot : UIElement
    {
        public Item Item2;
        public Item Item2Temp;
        public int CD;
        public abstract int xMod { get; }
        public abstract int yMod { get; }
        public int x;
        public int y;
        public bool Hovering;
        public int index;
        public abstract bool Locked { get; }
        public virtual string icon => "TranscendenceMod/Miscannellous/UI/ModifierUI/ModifierIngredientIcon";
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            x = (int)GetDimensions().X - 94;
            y = (int)GetDimensions().Y - 112;

            Hovering = Main.MouseWorld.Distance(new Vector2(Main.screenPosition.X + x + xMod - 16, Main.screenPosition.Y + y + yMod - 16)) < 16;

            Texture2D bg = TextureAssets.InventoryBack.Value;

            int sizeBoost = Hovering ? 4 : 0;
            Rectangle rec = new Rectangle(x + xMod - 32 - sizeBoost / 2, y + yMod - 32 - sizeBoost / 2, 42 + sizeBoost, 42 + sizeBoost);
            Rectangle recEmpty = new Rectangle(x + xMod - 26, y + yMod - 26, 30, 30);

            spriteBatch.Draw(bg, rec, Hovering ? Color.White : new Color(150, 150, 150));

            if (Item2 == null)
                return;

            Vector2 pos = new Vector2(x + xMod - 11, y + yMod - 10);
            Item fakeItem = null;
            bool showFake = index > -1 && index < 16 && ProcesserUI.PreviewItems[index] > 0;
            if (showFake)
                fakeItem = new Item(ProcesserUI.PreviewItems[index]);

            if (Item2.IsAir)
            {
                if (showFake)
                {
                    var eff = ModContent.Request<Effect>("TranscendenceMod/Miscannellous/Assets/Shaders/Effects/SeraphOutlineShader", AssetRequestMode.ImmediateLoad).Value;

                    eff.Parameters["uOpacity"].SetValue(0.75f);
                    eff.Parameters["uSaturation"].SetValue(0.75f);

                    eff.Parameters["uRotation"].SetValue(0f);
                    eff.Parameters["uTime"].SetValue(0.1f);
                    eff.Parameters["uDirection"].SetValue(0.2f);

                    spriteBatch.End();
                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, eff, Main.UIScaleMatrix);


                    ItemSlot.DrawItemIcon(fakeItem, ItemSlot.Context.InventoryItem, spriteBatch, pos, 1f, bg.Width * 0.7f, Color.Black);


                    spriteBatch.End();
                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.UIScaleMatrix);
                }
                else spriteBatch.Draw(ModContent.Request<Texture2D>(icon).Value, recEmpty, null, Color.White * 0.175f);
            }

            if (Hovering)
            {
                if (!Item2.IsAir)
                {
                    Main.HoverItem = Item2;
                    Main.hoverItemName = Item2.Name;
                    Main.mouseText = true;
                }
                else
                {
                    if (Locked)
                    {
                        Main.hoverItemName = Language.GetTextValue("Mods.TranscendenceMod.Messages.Tooltips.ModifierResult");
                        Main.mouseText = true;
                    }
                    if (showFake)
                    {
                        Main.HoverItem = fakeItem;
                        Main.hoverItemName = fakeItem.Name;
                        Main.mouseText = true;
                    }
                }
            }
            
            if (Item2 != null && !Item2.IsAir)
            {
                ItemSlot.DrawItemIcon(Item2, ItemSlot.Context.InventoryItem, spriteBatch, pos, 1f, bg.Width * 0.7f, Color.White);

                if (Item2.stack > 1)
                {
                    string stack = Item2.stack.ToString();
                    ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.MouseText.Value,
                        stack, new Vector2(x - 26 + xMod, y + yMod - 5), Color.White, 0, Vector2.Zero, Vector2.One * 0.75f);
                }
            }
        }

        public void TakeItem(Item it)
        {
            it.SetDefaults(Item2.type);
            it.prefix = Item2.prefix;
            it.stack = Item2.stack;

            it.GetGlobalItem<ModifiersItem>().ModifiersUnlocked = Item2.GetGlobalItem<ModifiersItem>().ModifiersUnlocked;

            if (!Main.dedServ)
                SoundEngine.PlaySound(SoundID.Grab);

            if (Item2.GetGlobalItem<ModifiersItem>().Modifier > 0)
                it.GetGlobalItem<ModifiersItem>().Modifier = Item2.GetGlobalItem<ModifiersItem>().Modifier;

            if (Item2.ModItem is ModifierContainer cont && it.ModItem is ModifierContainer cont2)
            {
                cont2.ItemsInside = cont.ItemsInside;
                cont2.CurSelection = cont.CurSelection;
            }

            Item2.TurnToAir();
            CD = 0;
        }

        public override void Update(GameTime gameTime)
        {
            Item item = Main.mouseItem;


            x = (int)GetDimensions().X;
            y = (int)GetDimensions().Y - 20;

            if (Item2 == null)
            {
                Item2 = new Item();
            }

            if (!ProcesserUIDrawing.Visible)
                return;

            if (!Main.mouseLeft) CD++;

            if (index < 16)
            {
                ProcesserUI.itemsInside[index] = Item2;
                if (Item2.IsAir)
                    ProcesserUI.itemsInside[index] = new Item(0);
            }


            if (Hovering)
            {
                if (!Item2.IsAir && !item.IsAir && item.type == Item2.type && Main.mouseLeft && CD > 10)
                {
                    item.stack += Item2.stack;
                    Item2.TurnToAir();
                    CD = 0;

                    if (!Main.dedServ)
                        SoundEngine.PlaySound(SoundID.Grab);
                }

                if (!Item2.IsAir && item.IsAir && CD > 15 && Main.mouseLeft)
                    TakeItem(item);

                Main.LocalPlayer.mouseInterface = true;

                if (Item2 != item && item != null && !item.IsAir && CD > 20 && (Main.mouseLeft && Item2.IsAir || Main.mouseRight && (Item2.type == item.type || Item2.IsAir)) && !Locked)
                {
                    SoundEngine.PlaySound(SoundID.Grab);

                    int stack = 0;
                    if (!Item2.IsAir)
                        stack = Item2.stack;

                    Item2 = item.Clone();
                    Item2.stack = stack;
                    Item2Temp = Item2;
                    
                    if (Main.mouseLeft)
                    {
                        Item2.stack = item.stack;
                        item.TurnToAir();
                    }
                    if (Main.mouseRight)
                    {
                        item.stack--;
                        Item2.stack = Item2.stack + 1;
                    }

                    CD = 0;
                }
            }
        }
    }
}
namespace TranscendenceMod.Miscannellous.UI.Processer
{
    [Autoload(Side = ModSide.Client)]
    public class ProcesserUIDrawing : ModSystem
    {
        internal ProcesserUI mauis;
        private UserInterface mauis2;
        public static bool Visible;

        public override void Load()
        {
            mauis = new ProcesserUI();
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
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer("TranscendenceMod: Material Processer UI", delegate
                {
                    if (Visible) mauis2.Draw(Main.spriteBatch, new GameTime());
                    return true;
                }, InterfaceScaleType.UI));
            }
        }
    }
}

