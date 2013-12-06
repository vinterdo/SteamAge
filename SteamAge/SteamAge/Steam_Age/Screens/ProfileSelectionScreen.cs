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
//using Box2D.XNA;
using FarseerPhysics;
using FarseerPhysics.Collision;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using FarseerPhysics.Controllers;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;


namespace SteamAge
{
    class ProfileSelectionScreen : GameScreen
    {
        int Selected = 1;
        public List<Button> Buttons;

        public ProfileSelectionScreen(Game Game, int x, int y)
            : base(Game, x, y)
        {
            float i = 0.1f;
            Buttons = new List<Button>();
            foreach (string P in Profile.AvalibleProfiles())
            {
                Button TmpButton = new Button(GeneralManager.GetPartialRect(0.3f, i, 0.3f, 0.14f), "", GeneralManager.Textures["Textures/GUI/Frame"], Color.Gray, Color.White, "Fonts/SteamWreck");
                AddGUI(TmpButton);
                Buttons.Add(TmpButton);
                Label TmpLabel = new Label();
                TmpLabel.Font = GeneralManager.Fonts["Fonts/SteamWreck"];
                TmpLabel.Name = P;
                TmpLabel.Position =GeneralManager.GetPartialRect(0.38f, i + 0.04f, 0.12f, 0.06f);
                AddGUI(TmpLabel);
                //SpriteBatch.Draw(Generanew Button(GeneralManager.GetPartialRect(0.3f, i, 0.3f, 0.14f)lManager.Textures["Textures/GUI/Frame"], GeneralManager.GetPartialRect(0.3f, i, 0.3f, 0.14f), Color.White);
                i += 0.15f;
            }


            Button ChooseButton = new Button(GeneralManager.GetPartialRect(0.15f, 0.85f, 0.3f, 0.14f), "", GeneralManager.Textures["Textures/GUI/Frame"], Color.Gray, Color.White, "Fonts/SteamWreck");
            ChooseButton.Action += Choose;
            AddGUI(ChooseButton);
            Label TmpLabel2 = new Label();
            TmpLabel2.Font = GeneralManager.Fonts["Fonts/SteamWreck"];
            TmpLabel2.Name = "Choose";
            TmpLabel2.Position = GeneralManager.GetPartialRect(0.23f,0.89f, 0.12f, 0.06f);
            AddGUI(TmpLabel2);


            Button NewButton = new Button(GeneralManager.GetPartialRect(0.45f, 0.85f, 0.3f, 0.14f), "", GeneralManager.Textures["Textures/GUI/Frame"], Color.Gray, Color.White, "Fonts/SteamWreck");
            NewButton.Action += ToNewProfile;
            AddGUI(NewButton);
            Label TmpLabel3 = new Label();
            TmpLabel3.Font = GeneralManager.Fonts["Fonts/SteamWreck"];
            TmpLabel3.Name = "New  Profile";
            TmpLabel3.Position = GeneralManager.GetPartialRect(0.53f, 0.89f, 0.12f, 0.06f);
            AddGUI(TmpLabel3);
        }

        public override void Draw(SpriteBatch SpriteBatch, GameTime GameTime)
        {
            SpriteBatch.Draw(GeneralManager.Textures["Textures/GUI/PaperBg"], GeneralManager.GetPartialRect(0, 0, 1, 1), Color.Gray);
            SpriteBatch.Draw(GeneralManager.Textures["Textures/GUI/SelectProfile"], GeneralManager.GetPartialRect(0.05f, 0.05f, 0.2f, 0.3f), Color.White);
            SpriteBatch.Draw(GeneralManager.Textures["Textures/GUI/PipeRightBot"], GeneralManager.GetPartialRect(0.65f, 0.55f, 0.35f, 0.45f), Color.White);
            
            base.Draw(SpriteBatch, GameTime);


            SpriteBatch.Draw(GeneralManager.Textures["Textures/GUI/ButtonSelect"], GeneralManager.GetPartialRect(0.29f, Selected * 0.15f - 0.05f, 0.32f, 0.12f), Color.White);

        }

        public override void Update(GameTime GameTime)
        {
            int i =1;
            foreach (Button B in Buttons)
            {
                if (B.IsActive && GeneralManager.IsLMBClickedEdge())
                {
                    Selected = i;
                }
                i++;
            }
            base.Update(GameTime);
        }

        public void Choose()
        {
            SteamAge.CurrentProfile = Profile.LoadProfile(Profile.AvalibleProfiles()[Selected - 1]);
            this.SwitchTo(new MainMenuScreen(Parent, GeneralManager.ScreenX, GeneralManager.ScreenY, null));
        }

        public void ToNewProfile()
        {
            this.SwitchTo(new ProfileCreationScreen(Parent, GeneralManager.ScreenX, GeneralManager.ScreenY));
            Profile Test = new Profile();
            Test.PlayerName = "test prof";
            Test.SaveProfile();
        }
    }
}
