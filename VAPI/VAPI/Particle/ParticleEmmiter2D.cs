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
    public class ParticleEmmiter2D
    {
        public ParticleWorld2D World;

        public ParticleState2D InitialState;
        public ParticleState2D OffsetState;

        public ParticleState2D InitialChangeState;
        public ParticleState2D OffsetChangeState;

        public float GenerationChance;

        public Vector2 Position;

        bool Enabled = true;


        public void Enable()
        {
            Enabled = true;
        }

        public void Disable()
        {
            Enabled = false;
        }

        public void Switch()
        {
            Enabled = !Enabled;
        }

        public ParticleEmmiter2D(ParticleWorld2D World, Vector2 Position)
        {
            this.World = World;
            this.Position = Position;
        }

        public void Update(GameTime GameTime)
        {
            if (Enabled)
            {
                for (float Dafuq = GenerationChance; Dafuq > 0; Dafuq--)
                {
                    if (Dafuq >= 1)
                    {
                        CreateParticle();
                    }
                    else
                    {
                        if (Helper.GetRandom() % 10000 <= Dafuq * 10000)
                        {
                            CreateParticle();
                        }
                    }
                }
            }
        }

        public void CreateParticle()
        {
            Particle2D Part = new Particle2D();
            Part.CurrentState = this.InitialState;
            Part.ChangeState = this.InitialChangeState;

            Part.CurrentState.Angle += Helper.GetRandomTo(OffsetState.Angle);
            Part.CurrentState.Color = new Color(Helper.GetRandomTo(OffsetState.Color.R) + InitialState.Color.R, Helper.GetRandomTo(OffsetState.Color.G) + InitialState.Color.G, Helper.GetRandomTo(OffsetState.Color.B) + InitialState.Color.B);
            Part.CurrentState.Layer += Helper.GetRandomTo(OffsetState.Layer);
            Part.CurrentState.LifeTime -= Helper.GetRandomTo(OffsetState.LifeTime);
            Part.CurrentState.Opacity += Helper.GetRandomTo(OffsetState.Opacity);
            Part.CurrentState.Position += Helper.GetRandomTo(OffsetState.Position);
            Part.CurrentState.Scale += Helper.GetRandomTo(OffsetState.Scale);

            Part.ChangeState.Angle += Helper.GetRandomTo(OffsetChangeState.Angle);
            Part.ChangeState.Color = new Color(Helper.GetRandomTo(OffsetChangeState.Color.R) + InitialState.Color.R, Helper.GetRandomTo(OffsetChangeState.Color.G) + InitialState.Color.G, Helper.GetRandomTo(OffsetChangeState.Color.B) + InitialState.Color.B);
            Part.ChangeState.Layer += Helper.GetRandomTo(OffsetChangeState.Layer);
            Part.ChangeState.LifeTime -= Helper.GetRandomTo(OffsetChangeState.LifeTime);
            Part.ChangeState.Opacity += Helper.GetRandomTo(OffsetChangeState.Opacity);
            Part.ChangeState.Position += Helper.GetRandomTo(OffsetChangeState.Position);
            Part.ChangeState.Scale += Helper.GetRandomTo(OffsetChangeState.Scale);



            World.Particles.Add(Part);
        }
    }
}
