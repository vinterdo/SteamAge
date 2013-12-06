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
    public class WorldScreen: GameScreen
    {
        GameWorld World;
        FarseerPhysics.DebugViews.DebugViewXNA Debug;
        public Window EqWindow;

        public WorldScreen(Game Game, int SizeX, int SizeY)
            : base(Game, SizeX, SizeY)
        {
            foreach (BaseMod Mod in SteamAge.Mods)
            {
                Mod.Initalize();
            }

            EqWindow = new Window(GeneralManager.GetPartialRect(0.29f, 0.29f, 0.42f, 0.42f), new Color(1, 1, 1, 0.7f));

            World = new GameWorld(this);
            Debug = new FarseerPhysics.DebugViews.DebugViewXNA(World.PhysicalWorld);
            Debug.AppendFlags(FarseerPhysics.DebugViewFlags.Shape);
            Debug.AppendFlags(FarseerPhysics.DebugViewFlags.AABB);
            Debug.AppendFlags(FarseerPhysics.DebugViewFlags.PerformanceGraph);
            Debug.AppendFlags(FarseerPhysics.DebugViewFlags.Joint);
            Debug.AppendFlags(FarseerPhysics.DebugViewFlags.ContactPoints);
            Debug.DefaultShapeColor = Color.White;
            Debug.SleepingShapeColor = Color.LightGray;
            Debug.LoadContent(Parent.GraphicsDevice, Parent.Content);

            this.AddGUI(EqWindow);
        }

        public override void Draw(SpriteBatch SpriteBatch, GameTime GameTime)
        {
            World.Draw(SpriteBatch, GameTime);
            base.Draw(SpriteBatch, GameTime);

            //SpriteBatch.Draw(GeneralManager.Textures["Textures/GUI/EqBar"], GeneralManager.GetPartialRect(0.9f, 0.9f, 0.6f, 0.1f), Color.White);

            SpriteBatch.Draw(GeneralManager.Textures["Textures/GUI/ItemSlot"], GeneralManager.GetPartialRect(0.295f + World.GetCurrentPlayer().SelectedStack * 0.04f, 0.945f, 0.047f, 0.047f), Color.White);
            DrawHoldingStack(SpriteBatch);
        }

        public override void Update(GameTime GameTime)
        {
            World.Update(GameTime);
            base.Update(GameTime);
        }

        public override bool HandleInput()
        {
            if(base.HandleInput()) return true;

            if (World.HandleInput()) return true;
            if (GeneralManager.CheckKeyEdge(Keys.Q))
            {
                SteamAge.CurrentSettings.DebugMode = !SteamAge.CurrentSettings.DebugMode;
                return true;
            }
            if (GeneralManager.CheckKeyEdge(Keys.E))
            {
                this.EqWindow.Visible = !this.EqWindow.Visible;
                return true;
            }
            if (GeneralManager.CheckKeyEdge(Keys.D1))
            {
                World.GetCurrentPlayer().SelectedStack = 0;
                return true;
            }
            if (GeneralManager.CheckKeyEdge(Keys.D2))
            {
                World.GetCurrentPlayer().SelectedStack = 1;
                return true;
            }
            if (GeneralManager.CheckKeyEdge(Keys.D3))
            {
                World.GetCurrentPlayer().SelectedStack = 2;
                return true;
            }
            if (GeneralManager.CheckKeyEdge(Keys.D4))
            {
                World.GetCurrentPlayer().SelectedStack = 3;
                return true;
            }
            if (GeneralManager.CheckKeyEdge(Keys.D5))
            {
                World.GetCurrentPlayer().SelectedStack = 4;
                return true;
            }
            if (GeneralManager.CheckKeyEdge(Keys.D6))
            {
                World.GetCurrentPlayer().SelectedStack = 5;
                return true;
            }
            if (GeneralManager.CheckKeyEdge(Keys.D7))
            {
                World.GetCurrentPlayer().SelectedStack = 6;
                return true;
            }
            if (GeneralManager.CheckKeyEdge(Keys.D8))
            {
                World.GetCurrentPlayer().SelectedStack = 7;
                return true;
            }
            if (GeneralManager.CheckKeyEdge(Keys.D9))
            {
                World.GetCurrentPlayer().SelectedStack = 8;
                return true;
            }
            if (GeneralManager.CheckKeyEdge(Keys.D0))
            {
                World.GetCurrentPlayer().SelectedStack = 9;
                return true;
            }


            return false;
        }

        public override void EndDraw(SpriteBatch SpriteBatch, GameTime GameTime)
        {
            base.EndDraw(SpriteBatch, GameTime);


            if (SteamAge.CurrentSettings.DebugMode)
            {
                Matrix ViewMatrix = Matrix.CreateOrthographic(GeneralManager.ScreenX, -GeneralManager.ScreenY, 0, 100) * Matrix.CreateTranslation(new Vector3(-1, 1, 0)) * Matrix.CreateTranslation(new Vector3(-2 * World.CameraPos.X / GeneralManager.ScreenX, 2 * World.CameraPos.Y / GeneralManager.ScreenY, 0));

                Debug.RenderDebugData(ref ViewMatrix);
            }

        }

        public void DrawHoldingStack(SpriteBatch SpriteBatch)
        {
            Player CurrentPlayer = World.GetCurrentPlayer();
            if (CurrentPlayer.HoldingStack != null)
            {
                CurrentPlayer.HoldingStack.DrawStack(SpriteBatch, new Rectangle((int)GeneralManager.MousePos.X, (int)GeneralManager.MousePos.Y, 48, 32));
                //SpriteBatch.Draw(GeneralManager.Textures[CurrentPlayer.HoldingStack.Item.Tex], new Rectangle((int)GeneralManager.MousePos.X, (int)GeneralManager.MousePos.Y, 24, 24), Color.White);
            }
        }

    }
}
