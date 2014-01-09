using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using VAPI;
using VAPI.Particle;
using System.IO;
using System.Xml;
using System.Reflection;

namespace SteamAge
{
    public class SteamAge : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public static Profile CurrentProfile;
        public static GameSettings CurrentSettings;
        public static List<BaseMod> Mods;
        //GameScreen TestGameScreen;

        public SteamAge()
        {
            CurrentSettings = new GameSettings();
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.IsFullScreen = false;
            Mods = new List<BaseMod>();
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            LoadSettings();
            

            GeneralManager.Initalize(Content, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight, this);

            LoadTextures();
            RegisterBlocksAndItems();
            LoadMods();

            GeneralManager.CurrentScreen = new ProfileSelectionScreen(this, GeneralManager.ScreenX, GeneralManager.ScreenY);
            
            base.Initialize();
        }

        private static void RegisterBlocksAndItems()
        {
            Block.RegisterBlock(new Blocks.BlockAir());
            Block.RegisterBlock(new Blocks.BlockDirt());

            Item.RegisterItem(new Items.CoalItem());

            Block.RegisterBlock(new TileEntities.TorchBlock());
            Block.RegisterBlock(new TileEntities.CraftingTableBlock());
            Block.RegisterBlock(new TileEntities.TreeBlock());
        }


        private void LoadSettings()
        {
            VAPI.GameSettings TestSettings = new VAPI.GameSettings();
            try
            {
                TestSettings.Load(@"settings.xml");
            }
            catch (FileNotFoundException)
            {
                TestSettings.Save(@"settings.xml");
            }
            ApplySettings(TestSettings);
        }

        private static void LoadTextures()
        {
            GeneralManager.LoadTex("Textures/Logo");
            GeneralManager.LoadTex("Textures/SkyGradient");
            GeneralManager.LoadTex("Textures/Sun");


            GeneralManager.LoadTex("Textures/GUI/Button1");
            GeneralManager.LoadTex("Textures/GUI/Wheel1");
            GeneralManager.LoadTex("Textures/GUI/Barometer");
            GeneralManager.LoadTex("Textures/GUI/BarometerTop");
            GeneralManager.LoadTex("Textures/GUI/Pointer");
            GeneralManager.LoadTex("Textures/GUI/PaperBg");
            GeneralManager.LoadTex("Textures/GUI/BlackGradient");
            GeneralManager.LoadTex("Textures/GUI/SelectProfile");
            GeneralManager.LoadTex("Textures/GUI/PipeRightBot");
            GeneralManager.LoadTex("Textures/GUI/Frame");
            GeneralManager.LoadTex("Textures/GUI/ButtonSelect");
            GeneralManager.LoadTex("Textures/GUI/WoodSign");
            GeneralManager.LoadTex("Textures/GUI/MainMenuShipForeground");
            GeneralManager.LoadTex("Textures/GUI/MainMenuShipBackground1");
            GeneralManager.LoadTex("Textures/GUI/MainMenuShipBackground2");
            GeneralManager.LoadTex("Textures/GUI/MainMenuShipBackground3");
            GeneralManager.LoadTex("Textures/GUI/EqBar");
            GeneralManager.LoadTex("Textures/GUI/ItemSlot");
            GeneralManager.LoadTex("Textures/GUI/CloseButton");

            GeneralManager.LoadTex("Textures/Hud/ArmorBack");
            GeneralManager.LoadTex("Textures/Hud/Chat");
            GeneralManager.LoadTex("Textures/Hud/CraftingBack");
            GeneralManager.LoadTex("Textures/Hud/Debug");
            GeneralManager.LoadTex("Textures/Hud/EqBack");
            GeneralManager.LoadTex("Textures/Hud/ExternalEqBack");
            GeneralManager.LoadTex("Textures/Hud/HotbarBack");
            GeneralManager.LoadTex("Textures/Hud/HpBar");
            GeneralManager.LoadTex("Textures/Hud/ManaBar");
            GeneralManager.LoadTex("Textures/Hud/MinimapBack");
            GeneralManager.LoadTex("Textures/Hud/Tooltip");

            GeneralManager.LoadTex("Textures/Particles/SteamParticle");

            GeneralManager.LoadTex("Textures/DynamicBodies/Barrel1");
            GeneralManager.LoadTex("Textures/DynamicBodies/TreeLog");


            GeneralManager.LoadTex("Textures/Backgrounds/BackgroundMountain");
            GeneralManager.LoadTex("Textures/Backgrounds/BackgroundForest");
            GeneralManager.LoadTex("Textures/Backgrounds/BackgroundDirt");


            GeneralManager.LoadTex("Textures/Items/CoalItem");

            GeneralManager.LoadFont("Fonts/SteamWreck");

            //GeneralManager.LoadEffect("Effects/Bloom");
            GeneralManager.LoadEffect("Effects/KryptonEffect");
        }

        private static void LoadMods()
        {
            DirectoryInfo ModFolder = new DirectoryInfo(Directory.GetCurrentDirectory() + @"\Mods\");
            if (!ModFolder.Exists)
            {
                ModFolder.Create();
            }

            foreach (FileInfo F in ModFolder.GetFiles())
            {
                if (F.Name.StartsWith("mod_") && F.Extension == ".dll")
                {
                    Assembly a = Assembly.LoadFrom(F.FullName);

                    Type[] ModType = a.GetExportedTypes();// = a.GetType("ModMain");
                    BaseMod Main;
                    foreach (Type T in ModType)
                    {
                        if (T.Name == "ModMain")
                        {
                            Main = (Activator.CreateInstance(T) as BaseMod);
                            Mods.Add(Main);
                        }
                    }
                }
            }
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

        }

        protected override void UnloadContent()
        {
            spriteBatch.Dispose();
        }

        protected override void Update(GameTime gameTime)
        {
            GeneralManager.Update();
            GeneralManager.CurrentScreen.HandleInput();
            GeneralManager.CurrentScreen.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            
            GeneralManager.CurrentScreen.BeginDraw(spriteBatch, gameTime);
            GeneralManager.CurrentScreen.Draw(spriteBatch, gameTime);
            GeneralManager.CurrentScreen.EndDraw(spriteBatch, gameTime);

            base.Draw(gameTime);
        }


        public void ApplySettings(VAPI.GameSettings Settings)
        {
            SteamAge.CurrentSettings = Settings;
            this.graphics.IsFullScreen = Settings.FullScreen;
            this.graphics.PreferredBackBufferWidth = Settings.ResX;
            this.graphics.PreferredBackBufferHeight = Settings.ResY;

            GeneralManager.ScreenX = Settings.ResX;
            GeneralManager.ScreenX = Settings.ResY;

            this.graphics.ApplyChanges();
            
        }

    }
}
