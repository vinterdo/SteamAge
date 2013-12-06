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

namespace SteamAge
{
    class MainMenuScreen : GameScreen
    {
        public float Angle= 0;
        bool SteamGenerating = false;


        public MainMenuScreen(Game Game, int SizeX, int SizeY, Effect Effect):base(Game, SizeX, SizeY, Effect)
        {
            MenuComponent Menu = new MenuComponent(new string[] { " New Game ", " Load Game " ,"Multiplayer", "Change Profile", "Options","    Exit    " }, GeneralManager.GetPartialRect(0.12f, 0.3f, 0.16f, 0.6f), "Textures/GUI/Button1", Color.DarkGray, Color.White, "Fonts/SteamWreck", new Vector2(10, 20));
            Menu.Buttons[0].Action = this.NewGame;
            Menu.Buttons[1].Action = this.LoadGame;
            Menu.Buttons[2].Action = this.Multiplayer;
            Menu.Buttons[3].Action = this.ChooseProfile;
            Menu.Buttons[4].Action = this.Settings;
            Menu.Buttons[5].Action = Parent.Exit;

            AddGUI(Menu);

            ParticleWorlds.Add(new VAPI.Particle.ParticleWorld2D(this));
            ParticleWorlds[0].Emmiters.Add(new SteamEmmiter(ParticleWorlds[0], GeneralManager.GetPartialVector(0.82f, 0.35f)));
            ParticleWorlds[0].Emmiters[0].Disable();

            Label PlayerName = new Label();
            PlayerName.Font = GeneralManager.Fonts["Fonts/SteamWreck"];
            PlayerName.Name = SteamAge.CurrentProfile.PlayerName;
            PlayerName.Position = GeneralManager.GetPartialRect(0.54f, 0.89f, 0.11f, 0.06f);
            AddGUI(PlayerName);
        }

        public override void Draw(SpriteBatch SpriteBatch, GameTime GameTime)
        {
            SpriteBatch.Draw(GeneralManager.Textures["Textures/GUI/MainMenuShipBackground1"], GeneralManager.GetPartialRect(0, 0, 1, 1), Color.White);
            SpriteBatch.Draw(GeneralManager.Textures["Textures/GUI/MainMenuShipBackground2"], GeneralManager.GetPartialRect(-0.1f + GeneralManager.MousePos.X / GeneralManager.ScreenX / 10, -0.1f + GeneralManager.MousePos.Y / GeneralManager.ScreenY / 10, 1.2f, 1.2f), Color.White);
            SpriteBatch.Draw(GeneralManager.Textures["Textures/GUI/MainMenuShipBackground3"], GeneralManager.GetPartialRect(-0.1f + GeneralManager.MousePos.X / GeneralManager.ScreenX / 15, -0.1f + GeneralManager.MousePos.Y / GeneralManager.ScreenY / 15, 1.2f, 1.2f), Color.White);
            
            SpriteBatch.Draw(GeneralManager.Textures["Textures/GUI/MainMenuShipForeground"], GeneralManager.GetPartialRect(0, 0, 1, 1), Color.White);

            SpriteBatch.Draw(GeneralManager.Textures["Textures/GUI/Wheel1"], new Rectangle((int)(GeneralManager.ScreenX * -0.1), (int)(GeneralManager.ScreenY * 0.5), (int)(GeneralManager.ScreenX * 0.8), (int)(GeneralManager.ScreenX * 0.8)), null, Color.White, Angle, Helper.GetCenter(GeneralManager.Textures["Textures/GUI/Wheel1"]) / 2, SpriteEffects.None, 0.5f);

            SpriteBatch.Draw(GeneralManager.Textures["Textures/GUI/BlackGradient"], GeneralManager.GetPartialRect(0.07f, 0, 0.25f, 1), Color.DarkGray);

            SpriteBatch.Draw(GeneralManager.Textures["Textures/GUI/WoodSign"], GeneralManager.GetPartialRect(0.5f, 0.85f, 0.19f, 0.14f), Color.White);
            
            base.Draw(SpriteBatch, GameTime);

            SpriteBatch.Draw(GeneralManager.Textures["Textures/GUI/Barometer"], GeneralManager.GetPartialRect(0.7f, 0.4f, 0.27f, 0.6f), null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0.5f);
            SpriteBatch.Draw(GeneralManager.Textures["Textures/GUI/Pointer"], GeneralManager.GetPartialRect(0.84f, 0.76f, 0.083f, 0.083f), null, Color.White, (float)Math.Sin(Angle), Helper.GetCenter(GeneralManager.Textures["Textures/GUI/Pointer"]) - new Vector2(38, 38), SpriteEffects.None, 0.5f);

            SpriteBatch.Draw(GeneralManager.Textures["Textures/GUI/BarometerTop"], GeneralManager.GetPartialRect(0.7f, 0.4f, 0.27f, 0.6f), null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0.5f);


            SpriteBatch.Draw(GeneralManager.Textures["Textures/Logo"], GeneralManager.GetPartialRect(0.2f, 0.0f, 0.6f, 0.2f), null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0.5f);

        }

        public override void Update(GameTime GameTime)
        {
            Angle += 0.003f;

            if (!SteamGenerating && Math.Sin(Angle) > 0.90f)
            {
                SteamGenerating = true;
                ParticleWorlds[0].Emmiters[0].Enable();
            }
            else if (SteamGenerating && Math.Sin(Angle) < - 0.4f)
            {
                SteamGenerating = false;
                ParticleWorlds[0].Emmiters[0].Disable();
            }

            ParticleWorlds[0].Emmiters[0].GenerationChance = (float)Math.Sin(Angle) / 2;//+0.5f;

            base.Update(GameTime);
        }

        void NewGame()
        {
            this.SwitchTo(new WorldScreen(Parent, GeneralManager.ScreenX, GeneralManager.ScreenY));
        }

        void ChooseProfile()
        {
            this.SwitchTo(new ProfileSelectionScreen(Parent, GeneralManager.ScreenX, GeneralManager.ScreenY));
        }

        void Settings()
        {
            this.SwitchTo(new SettingsScreen(Parent, GeneralManager.ScreenX, GeneralManager.ScreenY));
        }

        void Multiplayer()
        {
        }

        void LoadGame()
        {
        }

    }
}
