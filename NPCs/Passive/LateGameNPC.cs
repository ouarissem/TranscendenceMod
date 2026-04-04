using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Personalities;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Utilities;
using TranscendenceMod.Items;
using TranscendenceMod.Items.Consumables;
using TranscendenceMod.Items.Consumables.FoodAndDrinks;
using TranscendenceMod.Items.Consumables.LootBags;
using TranscendenceMod.Items.Consumables.Placeables.Decorations;
using TranscendenceMod.Items.Consumables.Placeables.SpaceBiome;
using TranscendenceMod.Items.Consumables.SuperBomb;
using TranscendenceMod.Items.Farming;
using TranscendenceMod.Items.Farming.Seeds;
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Items.Materials.Fish;
using TranscendenceMod.Items.Materials.LargeRecipes;
using TranscendenceMod.Items.Materials.MobDrops;
using TranscendenceMod.Items.Modifiers;
using TranscendenceMod.Items.Weapons;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Miscannellous.Biomes;
using TranscendenceMod.Projectiles.NPCs;
using TranscendenceMod.Projectiles.NPCs.Bosses.SpaceBoss;
using static TranscendenceMod.TranscendenceWorld;

namespace TranscendenceMod.NPCs.Passive
{
    public class LateGameNPC : ModNPC
    {
        public static int CurrentItem;
        public static int WantedStack;
        public static int CurrentButton1;
        public int Choice;
        public int Asked;

        string ShopText = Language.GetTextValue("LegacyInterface.28");
        string Quest = Language.GetTextValue("LegacyInterface.64");
        string TimeManip = Language.GetTextValue("Mods.TranscendenceMod.NPCs.LateGameNPC.TimeManip");
        string Portal = Language.GetTextValue("Mods.TranscendenceMod.NPCs.LateGameNPC.Portal");

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 23;
            NPCID.Sets.DangerDetectRange[Type] = 400;

            NPCID.Sets.ActsLikeTownNPC[Type] = true;
            NPCID.Sets.NoTownNPCHappiness[Type] = true;
            NPCID.Sets.AttackType[Type] = 0;
            NPCID.Sets.AttackTime[Type] = 30;
            NPCID.Sets.AttackAverageChance[Type] = 10;
            AnimationType = NPCID.Wizard;
        }
        public override void SetDefaults()
        {
            /*Stats*/
            NPC.lifeMax = 250;
            NPC.defense = 15;
            NPC.damage = 35;
            NPC.width = 34;
            NPC.height = 44;
            NPC.aiStyle = 7;

            /*Animation*/
            AnimationType = NPCID.Wizard;

            /*Colision*/
            NPC.noGravity = false;
            NPC.noTileCollide = false;

            /*Audio*/
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;

            NPC.friendly = true;
            NPC.lavaImmune = true;
            TownNPCStayingHomeless = true;
            SpawnModBiomes = new int[1] { ModContent.GetInstance<Heaven>().Type };
        }
        public override void SaveData(TagCompound tag)
        {
            tag["Name"] = NPC.GivenName;
        }
        public override void LoadData(TagCompound tag)
        {
            NPC.GivenName = tag.GetString("Name");
        }
        public override bool CheckActive() => false;
        public override bool NeedSaving() => true;
        public override bool CanChat() => true;
        public override void AI()
        {
            NPC.spriteDirection = NPC.direction;

            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile proj = Main.projectile[i];
                if (proj != null && proj.active && proj.type == ModContent.ProjectileType<CelestialSeraphSummoner>() && NPC.Center.Between(proj.Center - new Vector2(450, 0), proj.Center + new Vector2(450, 300)))
                {
                    if (!NPC.Center.Between(proj.Center - new Vector2(50, 0), proj.Center + new Vector2(50, 300)))
                    {
                        NPC.velocity.X = NPC.DirectionTo(proj.Center).X * 2;
                        NPC.spriteDirection = (int)(NPC.velocity.X / NPC.velocity.X);
                    }
                    else NPC.velocity.X *= 0.9f;
                }
            }
        }
        public void ShuffleItem()
        {
            int maxChoice = 8;

            if (Main.hardMode)
                maxChoice = 15;
            if (NPC.downedMoonlord)
                maxChoice = 13;


            //Randomize Items
            Choice = Main.rand.Next(1, maxChoice);
            int RAND = Main.rand.Next(0, maxChoice);
            while (RAND == Choice) RAND = Main.rand.Next(0, maxChoice);
            Choice = RAND;


            //Set Items
            /*Pre-HM*/
            switch (Choice)
            {
                case 0: CurrentItem = ModContent.ItemType<FlowerEssence>(); WantedStack = 1; break;
                case 1: CurrentItem = ItemID.AntlionMandible; WantedStack = 6; break;
                case 2: CurrentItem = ItemID.SharkFin; WantedStack = 2; break;
                case 3: CurrentItem = ItemID.Feather; WantedStack = 12; break;
                case 4: CurrentItem = ItemID.Geode; WantedStack = 1; break;
                case 5: CurrentItem = ItemID.FlinxFur; WantedStack = 3; break;
                case 6: CurrentItem = ItemID.JungleSpores; WantedStack = 6; break;
                case 7: CurrentItem = ItemID.Vine; WantedStack = 4; break;
            }
            /*Hardmode*/
            if (Main.hardMode)
            {
                switch (Choice)
                {
                    case 0: CurrentItem = ModContent.ItemType<SunburntAlloy>(); WantedStack = 5; break;
                    case 1: CurrentItem = ItemID.HellstoneBar; WantedStack = 8; break;
                    case 2: CurrentItem = ModContent.ItemType<FlowerEssence>(); WantedStack = 2; break;
                    case 3: CurrentItem = ItemID.LifeFruit; WantedStack = 1; break;
                    case 4: CurrentItem = ItemID.DarkShard; WantedStack = 2; break;
                    case 5: CurrentItem = ItemID.LightShard; WantedStack = 2; break;
                    case 6: CurrentItem = ItemID.SoulofFlight; WantedStack = 24; break;
                    case 7: CurrentItem = ItemID.SpiderFang; WantedStack = 18; break;
                    case 8: CurrentItem = ItemID.UnicornHorn; WantedStack = 3; break;
                    case 9: CurrentItem = ItemID.PixieDust; WantedStack = 12; break;
                    case 10: CurrentItem = ModContent.ItemType<Tomato>(); WantedStack = 12; break;
                    case 11: CurrentItem = ModContent.ItemType<CocoaBean>(); WantedStack = 12; break;
                }
            }
            /*Post Moonlord*/
            if (NPC.downedMoonlord)
            {
                switch (Choice)
                {
                    case 0: CurrentItem = ModContent.ItemType<StewThatMakesHerGoBlindFor1day>(); WantedStack = 1; break;
                    case 1: CurrentItem = ModContent.ItemType<Lightning>(); WantedStack = 6; break;
                    case 2: CurrentItem = ModContent.ItemType<FlowerEssence>(); WantedStack = 3; break;
                    case 3: CurrentItem = ModContent.ItemType<LivingOrganicMatter>(); WantedStack = 2; break;
                    case 4: CurrentItem = ItemID.SolarTablet; WantedStack = 1; break;
                    case 5: CurrentItem = ItemID.PumpkinMoonMedallion; WantedStack = 1; break;
                    case 6: CurrentItem = ItemID.NaughtyPresent; WantedStack = 1; break;
                    case 7: CurrentItem = ModContent.ItemType<Starfruit>(); WantedStack = 4; break;
                    case 8: CurrentItem = ModContent.ItemType<RedSpiderLily>(); WantedStack = 32; break;
                    case 9: CurrentItem = ModContent.ItemType<GalaxyAlloy>(); WantedStack = 2; break;
                    case 10: CurrentItem = ItemID.PixieDust; WantedStack = 18; break;
                    case 11: CurrentItem = ItemID.CrystalShard; WantedStack = 18; break;
                    case 12: CurrentItem = ItemID.DefenderMedal; WantedStack = 20; break;
                }
            }
        }
        public override void TownNPCAttackStrength(ref int damage, ref float knockback)
        {
            damage = 30;
            knockback = 7f;
        }

        public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
        {
            cooldown = 5;
            randExtraCooldown = 1;
        }
        public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
        {
            projType = ModContent.ProjectileType<StarcraftedBlade>();
            attackDelay = 1;
        }
        public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
        {
            multiplier = 5;
            randomOffset = 0;
        }
        public override bool CanGoToStatue(bool toKingStatue) => true;
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.NotScalingWithLuck(ModContent.ItemType<PulverizedPlanet>(), 4, 1, 3));
        }
        public override List<string> SetNPCNameList()
        {
            return new List<string>()
            {
                "Gamma",
                "Stara",
                "Cosma",
                "Luneis",
                "Lulicia",
                "Celistelle",
                "Ceilisia"
            };
        }
        public override bool CanTownNPCSpawn(int numTownNPCs) => false;
        public override void SetChatButtons(ref string button, ref string button2)
        {
            if (CurrentButton1 == 0)
                button = ShopText;

            if (CurrentButton1 == 1)
                button = Quest;

            if (CurrentButton1 == 2)
                button = TimeManip;

            if (CurrentButton1 == 3)
                button = Portal + " 1";

            if (CurrentButton1 == 4)
                button = Portal + " 2";

            if (CurrentButton1 == 5)
                button = Portal + " 3";

            button2 = Language.GetTextValue("Mods.TranscendenceMod.NPCs.LateGameNPC.Cycle");
        }
        public override void OnChatButtonClicked(bool firstButton, ref string shop)
        {
            Player player = Main.LocalPlayer;

            Main.npcChatText = GetChat();

            if (firstButton)
            {
                player.TryGetModPlayer(out TranscendencePlayer mp);
                if (CurrentButton1 == 0) shop = ShopText;
                if (CurrentButton1 == 1) QuestBehaviour();
                if (CurrentButton1 == 2) TimeDialFunction();
                if (CurrentButton1 == 3 && mp.PortalBoxPositions[0] != Vector2.Zero) player.Teleport(mp.PortalBoxPositions[0], TeleportationStyleID.PotionOfReturn);
                if (CurrentButton1 == 4 && mp.PortalBoxPositions[1] != Vector2.Zero) player.Teleport(mp.PortalBoxPositions[1], TeleportationStyleID.PotionOfReturn);
                if (CurrentButton1 == 5 && mp.PortalBoxPositions[2] != Vector2.Zero) player.Teleport(mp.PortalBoxPositions[2], TeleportationStyleID.PotionOfReturn);
            }
            else
            {
                bool HasPortalBox = player.HasItem(ModContent.ItemType<PortalBox>());
                int max = 2;
                if (HasPortalBox)
                    max += 3;

                if (++CurrentButton1 > max)
                    CurrentButton1 = 0;

                //Quests
                if (CurrentButton1 == 1)
                {
                    string ask = Main.rand.NextBool(2) ?
                        Language.GetTextValue($"Mods.TranscendenceMod.NPCs.LateGameNPC.Dialogue.QuestAsk1", ItemID.Search.GetName(CurrentItem), WantedStack) :
                        Language.GetTextValue($"Mods.TranscendenceMod.NPCs.LateGameNPC.Dialogue.QuestAsk2", ItemID.Search.GetName(CurrentItem), WantedStack);

                    Main.npcChatText = ask;
                }
            }
            void QuestBehaviour()
            {
                if (player.HasItem(CurrentItem))
                {
                    string chat = Language.GetTextValue($"Mods.TranscendenceMod.NPCs.LateGameNPC.Dialogue.Quest1", ItemID.Search.GetName(CurrentItem), WantedStack);
                    Main.npcChatText = chat;

                    /*START Rewards*/
                    int type = ModContent.ItemType<ExtraTerrestrialReward>();
                    if (Main.hardMode)
                        type = ModContent.ItemType<CrystalReward>();
                    if (NPC.downedMoonlord)
                        type = ModContent.ItemType<HeavenlyReward>();

                    Item.NewItem(player.GetSource_GiftOrReward(), player.getRect(),
                        type, Main.rand.Next(1, 2));
                    /*Rewards END*/

                    player.GetModPlayer<TranscendencePlayer>().CosmicNPCQuests++;
                    player.ConsumeItem(CurrentItem);
                    ShuffleItem();

                    Asked = 0;
                }
                else
                {
                    string chat1 = Language.GetTextValue($"Mods.TranscendenceMod.NPCs.LateGameNPC.Dialogue.QuestAsk1", ItemID.Search.GetName(CurrentItem), WantedStack);
                    string chat2 = Language.GetTextValue($"Mods.TranscendenceMod.NPCs.LateGameNPC.Dialogue.QuestAsk2", ItemID.Search.GetName(CurrentItem), WantedStack);
                    Main.npcChatText = Main.rand.NextBool(2) ? chat2 : chat1;
                }
            }
            void TimeDialFunction()
            {
                if (player.HasItem(ModContent.ItemType<Timedial>()) || TranscendenceWorld.ObtainedTimeDial)
                {
                    if (Main.dayTime) Main.fastForwardTimeToDusk = true;
                    else Main.fastForwardTimeToDawn = true;
                    if (!TranscendenceWorld.ObtainedTimeDial) player.ConsumeItem(ModContent.ItemType<Timedial>());
                    Main.npcChatText = Language.GetTextValue($"Mods.TranscendenceMod.NPCs.LateGameNPC.Dialogue.TimedialUnlock");
                    TranscendenceWorld.ObtainedTimeDial = true;
                }
                else
                {
                    Main.npcChatText = Language.GetTextValue($"Mods.TranscendenceMod.NPCs.LateGameNPC.Dialogue.TimedialReq", ModContent.ItemType<Timedial>());
                }
            }
        }
        public override void AddShops()
        {
            var npcshop = new NPCShop(Type, ShopText);
            {
                /*Always available*/

                TranscendenceUtils.sell(npcshop, ItemID.FallenStar, Item.buyPrice(silver: 50));
                TranscendenceUtils.sell(npcshop, ModContent.ItemType<EnchantedHopper>(), Item.buyPrice(silver: 65));
                TranscendenceUtils.sell(npcshop, ItemID.EnchantedNightcrawler, Item.buyPrice(silver: 85));
                TranscendenceUtils.sell(npcshop, ModContent.ItemType<ExtraTerrestrialBrew>());
                TranscendenceUtils.sell(npcshop, ModContent.ItemType<StarcraftedStew>());
                TranscendenceUtils.sell(npcshop, ModContent.ItemType<CosmicGrassSeed>());
                TranscendenceUtils.sell(npcshop, ModContent.ItemType<StarGun>());


                /* Locked Stuff */

                //Angel-Horn Scythe
                TranscendenceUtils.sell(npcshop, ModContent.ItemType<AngelHornScythe>(), Condition.Hardmode);

                //Cultist Painting
                TranscendenceUtils.sell(npcshop, ModContent.ItemType<AncientCultist>(), Condition.DownedCultist);

                //Lunar Fragments
                TranscendenceUtils.sell(npcshop, ItemID.FragmentSolar, Item.buyPrice(gold: 10), Condition.DownedMoonLord);
                TranscendenceUtils.sell(npcshop, ItemID.FragmentVortex, Item.buyPrice(gold: 10), Condition.DownedMoonLord);
                TranscendenceUtils.sell(npcshop, ItemID.FragmentNebula, Item.buyPrice(gold: 10), Condition.DownedMoonLord);
                TranscendenceUtils.sell(npcshop, ItemID.FragmentStardust, Item.buyPrice(gold: 10), Condition.DownedMoonLord);
            }
            npcshop.Register();
        }
        public override string GetChat()
        {
            if (CurrentItem < 1)
                ShuffleItem();

            WeightedRandom<string> dialog = new();
            dialog.Add(Language.GetTextValue($"Mods.TranscendenceMod.NPCs.LateGameNPC.Dialogue.Generic1"));
            dialog.Add(Language.GetTextValue($"Mods.TranscendenceMod.NPCs.LateGameNPC.Dialogue.Generic2"));
            dialog.Add(Language.GetTextValue($"Mods.TranscendenceMod.NPCs.LateGameNPC.Dialogue.Generic3"));
            dialog.Add(Language.GetTextValue($"Mods.TranscendenceMod.NPCs.LateGameNPC.Dialogue.Generic4"));

            if (!TranscendenceWorld.ObtainedTimeDial)
            {
                int TavernKeep = NPC.FindFirstNPC(NPCID.DD2Bartender);

                if (TavernKeep >= 0)
                    dialog.Add(Language.GetTextValue("Mods.TranscendenceMod.NPCs.LateGameNPC.Dialogue.TimedialHintEtheria", Main.npc[TavernKeep].GivenName));
                else
                    dialog.Add(Language.GetTextValue("Mods.TranscendenceMod.NPCs.LateGameNPC.Dialogue.TimedialHintEtheriaNoBar"));

                if (Main.hardMode)
                    dialog.Add(Language.GetTextValue("Mods.TranscendenceMod.NPCs.LateGameNPC.Dialogue.TimedialHintEarth"));
                dialog.Add(Language.GetTextValue("Mods.TranscendenceMod.NPCs.LateGameNPC.Dialogue.TimedialHintConstant"));
            }

            if (NPC.downedAncientCultist)
            {
                if (Main.rand.NextBool(4))
                    dialog.Add(Language.GetTextValue($"Mods.TranscendenceMod.NPCs.LateGameNPC.Dialogue.GenericPostCultist1"));
                if (NPC.TowerActiveSolar || NPC.TowerActiveVortex || NPC.TowerActiveNebula || NPC.TowerActiveStardust)
                    dialog.Add(Language.GetTextValue($"Mods.TranscendenceMod.NPCs.LateGameNPC.Dialogue.GenericPostCultist2"));
            }
            if (NPC.downedMoonlord)
            {
                dialog.Add(Language.GetTextValue($"Mods.TranscendenceMod.NPCs.LateGameNPC.Dialogue.GenericDivineEra1"));
                dialog.Add(Language.GetTextValue($"Mods.TranscendenceMod.NPCs.LateGameNPC.Dialogue.GenericDivineEra2"));

                if (!Downed.Contains(Bosses.Atmospheron))
                    dialog.Add(Language.GetTextValue($"Mods.TranscendenceMod.NPCs.LateGameNPC.Dialogue.GenericAtmospheron"));

                if (!Downed.Contains(Bosses.CelestialSeraph))
                {
                    dialog.Add(Language.GetTextValue($"Mods.TranscendenceMod.NPCs.LateGameNPC.Dialogue.GenericSeraph1"));
                    dialog.Add(Language.GetTextValue($"Mods.TranscendenceMod.NPCs.LateGameNPC.Dialogue.GenericSeraph2"));
                    dialog.Add(Language.GetTextValue($"Mods.TranscendenceMod.NPCs.LateGameNPC.Dialogue.GenericSeraph3"));
                }
                else
                {
                    dialog.Add(Language.GetTextValue($"Mods.TranscendenceMod.NPCs.LateGameNPC.Dialogue.GenericPostSeraph1"));
                }
            }

            return dialog;
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            ContentSamples.NpcBestiaryRarityStars[NPC.type] = 5;
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                new FlavorTextBestiaryInfoElement("Placeholder")
            });
        }
        public class StarBiomeShop : IShoppingBiome, ILoadable
        {
            public string NameKey => "StarBiome";
            public bool IsInBiome(Player player) => player.GetModPlayer<TranscendencePlayer>().ZoneStar;

            public void Load(Mod mod) { }
            public void Unload() { }
        }
    }
}