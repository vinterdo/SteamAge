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

namespace SteamAge
{
    class SteamEmmiter : ParticleEmmiter2D
    {
        public SteamEmmiter(ParticleWorld2D World, Vector2 Position)
            : base(World, Position)
        {
            this.GenerationChance = 1.5f;
            this.InitialState = new ParticleState2D();
            this.OffsetState = new ParticleState2D();
            this.InitialChangeState = new ParticleState2D();
            this.OffsetChangeState = new ParticleState2D();

            InitialState.Angle = 0f;
            InitialState.Center = Vector2.One * 25;
            InitialState.Color = Color.White;
            InitialState.Layer = 0.0f;
            InitialState.LifeTime = 10f;
            InitialState.Opacity = 0.4f;
            InitialState.Position = this.Position;
            InitialState.Scale = 0.5f;
            InitialState.Tex = GeneralManager.Textures["Textures/Particles/SteamParticle"];

            OffsetState.Angle = 6f;
            //OffsetState.Center = Vector2.One * 5;
            OffsetState.Color = Color.Red;
            OffsetState.Layer = 0.5f;
            OffsetState.LifeTime = 2f;
            OffsetState.Opacity = 0.1f;
            OffsetState.Position = Vector2.One * 50f;
            OffsetState.Scale = 0.5f;

            //Change

            InitialChangeState.Angle = 0f;
            InitialChangeState.Color = Color.Black;
            InitialChangeState.Layer = 0.0f;
            InitialChangeState.LifeTime = 0.1f;
            InitialChangeState.Opacity = -0.005f;
            InitialChangeState.Position = new Vector2(-0.6f, -2);
            InitialChangeState.Scale = 0.005f;

            OffsetChangeState.Angle = 0.01f;
            OffsetChangeState.Color = Color.Black;
            OffsetChangeState.Layer = 0.0001f;
            OffsetChangeState.LifeTime = 0.05f;
            OffsetChangeState.Opacity = -0.002f;
            OffsetChangeState.Position = Vector2.One;
            OffsetChangeState.Scale = 0.002f;
        }
    }
}
