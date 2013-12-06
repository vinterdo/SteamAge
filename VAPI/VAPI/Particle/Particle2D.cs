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
    public class Particle2D
    {
        public ParticleState2D CurrentState;
        public ParticleState2D ChangeState;

        public Particle2D()
        {
            CurrentState = new ParticleState2D();
            ChangeState = new ParticleState2D();
        }

        public void Update(GameTime GameTime)
        {
            
            CurrentState.Angle += ChangeState.Angle;
            CurrentState.Color = new Color(CurrentState.Color.R + ChangeState.Color.R, CurrentState.Color.G + ChangeState.Color.G, CurrentState.Color.B + ChangeState.Color.B, CurrentState.Opacity);
            CurrentState.Layer += ChangeState.Layer;
            CurrentState.LifeTime -= ChangeState.LifeTime;
            CurrentState.Opacity += ChangeState.Opacity;
            CurrentState.Position += ChangeState.Position;
            CurrentState.Scale += ChangeState.Scale;

        }

        public void Draw(SpriteBatch SpriteBatch)
        {
            SpriteBatch.Draw(CurrentState.Tex, CurrentState.Position, null, CurrentState.Color, CurrentState.Angle, CurrentState.Center, CurrentState.Scale, SpriteEffects.None, CurrentState.Layer);
        }


    }
}
