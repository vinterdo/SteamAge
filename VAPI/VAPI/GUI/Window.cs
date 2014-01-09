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

namespace VAPI
{
    public class Window : GUIComponent
    {
        Rectangle Position;
        public Stack<GUIComponent> GUIComponents;
        public bool Visible = true;
        Texture2D BgTex;

        public Window(Rectangle Position, Color BgColor)
        {
            GUIComponents = new Stack<GUIComponent>();
            this.Position = Position;
            BgTex = new Texture2D(GeneralManager.GDevice, 1, 1);
            BgTex.SetData<Color>(new[] { BgColor });

        }

        public Window(Rectangle Position, string TextureName)
        {
            GUIComponents = new Stack<GUIComponent>();
            this.Position = Position;
            this.BgTex = GeneralManager.Textures[TextureName];
        }

        public override void Draw(SpriteBatch SpriteBatch)
        {
            if (Visible)
            {
                
                SpriteBatch.Draw(BgTex, Position, Color.White);
                

                foreach (GUIComponent G in GUIComponents)
                {
                    G.Draw(SpriteBatch);
                }
            }
        }

        public override bool HandleInput()
        {
            if (Visible)
            {
                if (Position.Contains(Helper.VectorToPoint(GeneralManager.MousePos)) && GeneralManager.IsLMBClicked())
                {
                    foreach (GUIComponent G in GUIComponents)
                    {
                        if (G.HandleInput()) break;
                    }
                    return true;
                }
            }
            return false;
        }

        public override bool CheckActive()
        {
            if (Visible)
            {
                return Position.Contains(Helper.VectorToPoint(GeneralManager.MousePos));
            }
            return false;
        }

        public override void Update(GameTime gameTime)
        {
            if (Visible)
            {
                foreach (GUIComponent G in GUIComponents)
                {
                    G.Update(gameTime);
                }
            }
        }

        public void AddGUI(GUIComponent GUI)
        {
            this.GUIComponents.Push(GUI);
        }
    }
}
