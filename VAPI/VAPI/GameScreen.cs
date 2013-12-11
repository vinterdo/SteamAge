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
using VAPI.Particle;

namespace VAPI
{
    public class GameScreen
    {

        protected RenderTarget2D RenderTarget;
        protected Game Parent;
        protected Effect Effect;

        protected float GoingOut = 0;
        protected float GoingIn = 5;

        public LinkedList<GUIComponent> GUIComponents;
        public List<ParticleWorld2D> ParticleWorlds;

        public GameScreen(Game Game, int SizeX, int SizeY)
        {
            GUIComponents = new LinkedList<GUIComponent>();
            ParticleWorlds = new List<ParticleWorld2D>();
            this.Parent = Game;
            RenderTarget = new RenderTarget2D(Parent.GraphicsDevice, SizeX, SizeY );//new RenderTarget2D(Parent.GraphicsDevice, SizeX, SizeY, 1, SurfaceFormat.Color);
        }

        public GameScreen(Game Game, int SizeX, int SizeY, Effect Effect)
        {
            GUIComponents = new LinkedList<GUIComponent>();
            ParticleWorlds = new List<ParticleWorld2D>();
            this.Parent = Game;
            RenderTarget = new RenderTarget2D(Parent.GraphicsDevice, SizeX, SizeY);//, 1, SurfaceFormat.Color);
            this.Effect = Effect;
        }


        public virtual bool HandleInput()
        {
            foreach (GUIComponent G in GUIComponents)
            {
                if (G.HandleInput())
                {
                    return true;
                }
            }
            return false;
            /*
            if (GeneralManager.CheckKeyEdge(Keys.Down))
            {
                int i = 0;
                foreach (GUIComponent G in GUIComponents)
                {
                    if (G.IsActive)
                    {
                        G.IsActive = false;
                        if (i + 1 == GUIComponents.Count)
                        {
                            
                        }
                    }
                    i++;
                }
            }*/
        }

        public virtual void Update(GameTime GameTime)
        {
            foreach (GUIComponent G in GUIComponents)
            {
                G.Update(GameTime);
                G.IsActive = false;
            }

            foreach (GUIComponent G in GUIComponents)
            {
                if (G.CheckActive())
                {
                    break;
                }
            }

            foreach (ParticleWorld2D PW in ParticleWorlds)
            {
                PW.Update(GameTime);
            }
        }

        public virtual void Draw(SpriteBatch SpriteBatch, GameTime GameTime)
        {

            int i = GUIComponents.Count -1;
            List<GUIComponent> GC = GUIComponents.ToList<GUIComponent>();

            foreach (GUIComponent G in GUIComponents)
            {
                GC[i].Draw(SpriteBatch);
                
                i--;
            }

            foreach (ParticleWorld2D PW in ParticleWorlds)
            {
                PW.Draw(SpriteBatch, GameTime);
            }
            
        }

        public void AddGUI(GUIComponent GUI)
        {
            this.GUIComponents.AddFirst(GUI);
        }

        public void RemoveGUI(GUIComponent GUI)
        {
            GUIComponents.Remove(GUI);
        }

        public void BeginDraw(SpriteBatch SpriteBatch, GameTime GameTime)
        {

            Parent.GraphicsDevice.SetRenderTarget(RenderTarget);
            Parent.GraphicsDevice.Clear(Color.Black);

            SpriteBatch.Begin();//SpriteBlendMode.AlphaBlend);
            //SpriteBatch.Begin();
        }


        public virtual void EndDraw(SpriteBatch SpriteBatch, GameTime GameTime)
        {

            SpriteBatch.End();
            Parent.GraphicsDevice.SetRenderTarget(null);

            //Color[] TexData = new Color[RenderTarget.Width * RenderTarget.Height];
            //RenderTexture = new Texture2D(Parent.GraphicsDevice, RenderTarget.Width, RenderTarget.Height);
            //RenderTarget.GetData<Color>(TexData); ;// GetTexture();
            //RenderTexture.SetData<Color>(TexData);
            Parent.GraphicsDevice.Clear(Color.Black);

            SpriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            if (Effect != null)
            {
                //Effect.Begin();
                foreach (EffectPass EP in Effect.CurrentTechnique.Passes)
                {
                    EP.Apply();// Begin();
                }
            }

            SpriteBatch.Draw(RenderTarget, new Rectangle(0,0, GeneralManager.ScreenX, GeneralManager.ScreenY), Color.White);

            SpriteBatch.End();

            if (Effect != null)
            {

                foreach (EffectPass EP in Effect.CurrentTechnique.Passes)
                {
                    //EP.End();
                }
                //Effect.End();
            }
        }

        public void SwitchTo(GameScreen Screen)
        {
            GeneralManager.CurrentScreen = Screen;
        }
        
    }
}
