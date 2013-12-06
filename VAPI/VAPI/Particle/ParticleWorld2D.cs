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

namespace VAPI.Particle
{
    public class ParticleWorld2D 
    {
        public List<ParticleEmmiter2D> Emmiters;
        public List<Particle2D> Particles;

        GameScreen GameScreen;

        public ParticleWorld2D(GameScreen GameScreen)
        {
            this.GameScreen = GameScreen;
            this.Emmiters = new List<ParticleEmmiter2D>();
            this.Particles = new List<Particle2D>();
        }

        public void Update(GameTime GameTime)
        {
            foreach (ParticleEmmiter2D E in Emmiters)
            {
                E.Update(GameTime);
            }

            for(int i =0; i < Particles.Count; i++)
            {
                Particles[i].Update(GameTime);
                if (Particles[i].CurrentState.LifeTime <= 0)
                {
                    Particles.Remove(Particles[i]);
                }
            }
        }

        public void AddEmmiter(ParticleEmmiter2D Emmiter)
        {
            Emmiters.Add(Emmiter);
        }

        public void Draw(SpriteBatch SpriteBatch, GameTime GameTime)
        {
            foreach (Particle2D P in Particles)
            {
                P.Draw(SpriteBatch);
            }
        }

    }
}
