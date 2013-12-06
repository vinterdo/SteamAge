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
using FarseerPhysics;
using FarseerPhysics.Collision;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using FarseerPhysics.Controllers;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;

namespace VAPI.FluidSim
{
    public class Particle
    {
        public Vector2 position;
        public Vector2 velocity;
        public bool alive;
        public int index;

        public float[] distances;
        public int[] neighbors;
        public int neighborCount;

        public Vector2[] collisionVertices;
        public Vector2[] collisionNormals;

        public int ci; // GridCoords
        public int cj;

        public float p;
        public float pnear;

        public const int MAX_FIXTURES_TO_TEST = 20;
        public Fixture[] fixturesToTest;
        public int numFixturesToTest;

        public Particle(Vector2 position, Vector2 velocity, bool alive)
        {
            this.position = position;
            this.velocity = velocity;
            this.alive = alive;

            distances = new float[FluidSimulation.MAX_NEIGHBORS];
            neighbors = new int[FluidSimulation.MAX_NEIGHBORS];
            fixturesToTest = new Fixture[MAX_FIXTURES_TO_TEST];

            collisionVertices = new Vector2[FarseerPhysics.Settings.MaxPolygonVertices];
            collisionNormals = new Vector2[FarseerPhysics.Settings.MaxPolygonVertices];
        }
    }
}
