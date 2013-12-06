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
    class ProfileCreationScreen : GameScreen
    {
        TextBox NameBox;

        public ProfileCreationScreen(Game Game, int x, int y)
            : base(Game, x, y)
        {
            NameBox = new TextBox(GeneralManager.GetPartialRect(0.4f,0.45f,0.2f,0.1f), "Textures/GUI/Frame", "Fonts/SteamWreck");
            AddGUI(NameBox);
            Button CreateButton = new Button(GeneralManager.GetPartialRect(0.4f, 0.6f, 0.2f, 0.1f), "Create", GeneralManager.Textures["Textures/GUI/Frame"], Color.Gray, Color.White, "Fonts/SteamWreck");
            AddGUI(CreateButton);
        }

        public void CreateProfile()
        {
            Profile P = new Profile();
            P.PlayerName = NameBox.Text;
            P.SaveProfile();
        }
    }
}
