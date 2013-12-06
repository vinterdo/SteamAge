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
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Factories;
using System.Diagnostics;
using VAPI.FluidSim;
using Krypton;
using Krypton.Lights;
using SteamAge.Generators;

namespace SteamAge.Entities
{
    public class Tree : Entity
    {
        public List<TreePart> TreeParts;
        public List<Joint> Joints;
        public Vector2 Position;

        public Tree(GameWorld World, Vector2 Position)
            : base(World)
        {
            TreeParts = new List<TreePart>();
            Joints = new List<Joint>();
            this.Position = Position;
        }

        public void Generate(int Size)
        {
            GenerateTrunk(Size);
            GenerateBranches();
        }

        private void GenerateTrunk(int Size)
        {
            int CurrentHeight = 0;

            while (CurrentHeight <= Size)
            {
                TreePart T = new TreePart(new Vector2(24, 24), World, GeneralManager.Textures["Textures/DynamicBodies/TreeLog"]);
                T.Position = Position - new Vector2(0, CurrentHeight);
                if (TreeParts.Count >= 1)
                {
                    ConnectTreeParts(TreeParts[TreeParts.Count - 1], T);
                }
                TreeParts.Add(T);
                
                CurrentHeight += 24;
            }
        }


        private void GenerateBranches()
        {

        }

        public void Destroy()
        {
        }

        public override void Draw(SpriteBatch SpriteBatch, Vector2 CameraPos)
        {
            foreach (TreePart TP in TreeParts)
            {
                TP.Draw(SpriteBatch, CameraPos);
            }
            base.Draw(SpriteBatch, CameraPos);
        }

        public void ConnectTreeParts(TreePart TP1, TreePart TP2)
        {
            JointFactory.CreateSliderJoint(this.World.PhysicalWorld, TP1.Fixture.Body, TP2.Fixture.Body, Vector2.Zero, Vector2.Zero, 24, 24);
        }
    }
}
