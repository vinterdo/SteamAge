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
        public List<TreePartLog> TreeLogs;
        public List<TreeBranch> TreeBranches;
        public List<Joint> Joints;
        public Vector2 Position;

        public Tree(GameWorld World, Vector2 Position)
            : base(World)
        {
            TreeLogs = new List<TreePartLog>();
            TreeBranches = new List<TreeBranch>();
            Joints = new List<Joint>();
            this.Position = Position;
        }

        public void Generate(int Size)
        {
            GenerateTrunk(Size, 64);
            GenerateBranches();
            CreateRoots();
            //SetStatic();
        }

        private void GenerateTrunk(int Size, int Width)
        {
            int CurrentHeight = 0;

            while (CurrentHeight <= Size)
            {
                TreePartLog T = new TreePartLog(new Vector2(Width, Width), World, GeneralManager.Textures["Textures/DynamicBodies/TreeLog"]);
                T.Position = Position - new Vector2(0, CurrentHeight);
                if (TreeLogs.Count >= 1)
                {
                    ConnectTreeParts(TreeLogs[TreeLogs.Count - 1], T);
                }
                TreeLogs.Add(T);
                
                CurrentHeight += Width;
            }
        }


        private void GenerateBranches()
        {

        }

        public void Destroy()
        {
        }

        public void SetStatic()
        {
            foreach (TreePartLog TL in TreeLogs)
            {
                TL.Fixture.Body.BodyType = BodyType.Static;
            }
        }

        public void SetDynamic()
        {
            foreach (TreePartLog TL in TreeLogs)
            {
                TL.Fixture.Body.BodyType = BodyType.Dynamic;
            }
        }

        public override void Draw(SpriteBatch SpriteBatch, Vector2 CameraPos)
        {
            foreach (TreePartLog TP in TreeLogs)
            {
                TP.Draw(SpriteBatch, CameraPos);
            }
            base.Draw(SpriteBatch, CameraPos);
        }

        public void ConnectTreeParts(TreePartLog TP1, TreePartLog TP2)
        {
            //JointFactory.CreateSliderJoint(this.World.PhysicalWorld, TP1.Fixture.Body, TP2.Fixture.Body, Vector2.Zero, Vector2.Zero, 24, 24);
            RevoluteJoint RJ1 = JointFactory.CreateRevoluteJoint(this.World.PhysicalWorld, TP1.Fixture.Body, TP2.Fixture.Body, new Vector2(0, 0));
            //RevoluteJoint RJ2 = JointFactory.CreateRevoluteJoint(this.World.PhysicalWorld, TP1.Fixture.Body, TP2.Fixture.Body, new Vector2(12, 12));
            RJ1.MotorEnabled = true;
            RJ1.LowerLimit = -MathHelper.TwoPi * 0.01f;
            RJ1.UpperLimit = MathHelper.TwoPi * 0.01f;
            RJ1.LimitEnabled = true;
            RJ1.CollideConnected = true;

            
            //WeldJoint WJ1 = JointFactory.CreateWeldJoint(TP1.Fixture.Body, TP2.Fixture.Body, Vector2.Zero);
            /*DistanceJoint DJ1 = JointFactory.CreateDistanceJoint(this.World.PhysicalWorld, TP1.Fixture.Body, TP2.Fixture.Body, new Vector2(-12, 8), new Vector2(-12, -8));
            DJ1.Length = 8;

            DistanceJoint DJ2 = JointFactory.CreateDistanceJoint(this.World.PhysicalWorld, TP1.Fixture.Body, TP2.Fixture.Body, new Vector2(12, 8), new Vector2(12, -8));
            DJ2.Length = 8;*/

        }

        public void CreateRoots()
        {
            int ScanRange = 3;
            Fixture[,] BlocksNearby = new Fixture[ScanRange,ScanRange];
            for (int y = 0; y < ScanRange; y++)
            {
                for (int x = 0; x < ScanRange; x++)
                {
                    BlocksNearby[x, y] = World.GetBlockFixture((int)Position.X / 32 + x, (int)Position.Y + y);
                    //JointFactory.CreateRevoluteJoint(
                }
            }
        }
    }
}
