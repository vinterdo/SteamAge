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
    public struct ParticleState2D
    {
        public Vector2 Position;
        public float Scale;
        public float Opacity;
        public Color Color;
        public float LifeTime;
        public Texture2D Tex;
        public float Angle;
        public float Layer;
        public Vector2 Center;

    }
}
