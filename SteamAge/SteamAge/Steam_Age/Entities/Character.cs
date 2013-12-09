using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
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
using FarseerPhysics.Dynamics.Joints;
using Spine;

namespace SteamAge
{
    public class Character
    {
        public Vector2 Position;
        public Vector2 Speed;
        public static int CollisionX = 37;
        public static int CollisionY = 62;

        Vector2 gravity = new Vector2(0f, 0.125f);

        Rectangle collisionRect = new Rectangle(0, 0, CollisionX, CollisionY);

        Texture2D blankTex;

        SkeletonRenderer skeletonRenderer;
        Skeleton skeleton;
        Animation walkAnimation;
        Animation jumpAnimation;
        Animation crawlAnimation;
        Animation fallAnimation;
        Animation grabAnimation;
        Animation climbAnimation;
        float animTime;

        int faceDir = 1;

        bool walking = false;
        bool jumping = false;
        bool crouching = false;
        bool falling = false;
        bool grabbed = false;
        bool climbing = false;

        bool justUngrabbed = false;

        Vector2 grabbedPosition;

        GameWorld World;

        public Fixture CatchedFixture;
        public Joint CatchingJoint;

        public Character(Vector2 spawnPosition, GameWorld World)
        {
            Position = spawnPosition;
            this.World = World;
        }

        public void LoadContent(ContentManager content, GraphicsDevice graphicsDevice)
        {
            blankTex = content.Load<Texture2D>("Textures/Blank");

            skeletonRenderer = new SkeletonRenderer(graphicsDevice);
            
            Atlas atlas = new Atlas(graphicsDevice, System.IO.Path.Combine(content.RootDirectory, "spineboy.atlas"));
            SkeletonJson json = new SkeletonJson(atlas);
            json.Scale = 0.5f;
            skeleton = new Skeleton(json.readSkeletonData("spineboy", File.ReadAllText(System.IO.Path.Combine(content.RootDirectory, "spineboy.json"))));
            skeleton.SetSkin("default");
            skeleton.SetSlotsToBindPose();
            walkAnimation = skeleton.Data.FindAnimation("walk");
            jumpAnimation = skeleton.Data.FindAnimation("jump");
            crawlAnimation = skeleton.Data.FindAnimation("crawl");
            fallAnimation = skeleton.Data.FindAnimation("fall");
            grabAnimation = skeleton.Data.FindAnimation("grab");
            climbAnimation = skeleton.Data.FindAnimation("climb");

            skeleton.RootBone.X = Position.X;
            skeleton.RootBone.Y = Position.Y;
            skeleton.UpdateWorldTransform();

            /*Body = new Body(World.PhysicalWorld);
            Body.BodyType = BodyType.Dynamic;
            Body.SetTransform(this.Position, 0f);
            Vertices V = new Vertices();
            V.Add(new Vector2(0,0));
            V.Add(new Vector2(50,0));
            V.Add(new Vector2(50,50));
            V.Add(new Vector2(0,50));
            Fixture = Body.CreateFixture(new PolygonShape(V, 1f));*/
        }

        public void Update(GameTime gameTime)
        {
            if (!walking && !jumping && !crouching && !grabbed)
            {
                skeleton.SetToBindPose();
                
            }

            if (walking && !jumping && !grabbed)
            {
                animTime += gameTime.ElapsedGameTime.Milliseconds / 1000f;
                if (!crouching)
                {
                    walkAnimation.Mix(skeleton, animTime, true, 0.3f);
                }
                else
                {
                    crawlAnimation.Mix(skeleton, animTime, true, 0.5f);
                }
            }

            if (jumping)
            {
                animTime += gameTime.ElapsedGameTime.Milliseconds / 1000f;
                jumpAnimation.Mix(skeleton, animTime, false, 0.5f);
            }

            if (crouching && !jumping)
            {
                collisionRect.Width = (int)(CollisionX * 1.125f);
                collisionRect.Height = (int)(CollisionY * 0.4f);

                if (!walking)
                {
                    animTime = 0;
                    crawlAnimation.Mix(skeleton, animTime, false, 0.5f);
                }
            }
            else
            {
                collisionRect.Width = CollisionX;
                collisionRect.Height = CollisionY;
            }

            if (falling)
            {
                Speed += gravity;

                if (Speed.Y > 1)
                {
                    animTime += gameTime.ElapsedGameTime.Milliseconds / 1000f;
                    fallAnimation.Mix(skeleton, animTime, true, 0.75f);
                }
            }

            if (grabbed)
            {
                Position = Vector2.Lerp(Position, grabbedPosition, 0.1f);
                animTime += gameTime.ElapsedGameTime.Milliseconds / 1000f;
                grabAnimation.Mix(skeleton, animTime, true, 0.3f);
            }

            if (climbing)
            {
                //Position = Vector2.Lerp(Position, grabbedPosition, 0.1f);
                animTime += gameTime.ElapsedGameTime.Milliseconds / 500f;
                climbAnimation.Apply(skeleton, animTime, false);

                Position = Vector2.Lerp(Position, grabbedPosition - new Vector2(20*(-faceDir), CollisionY * 2.5f), (0.07f/grabAnimation.Duration) * animTime);

                if ((Position - (grabbedPosition - new Vector2(20 * (-faceDir), CollisionY * 2.5f))).Length() < 5f)
                    climbing = false;
            }

            Position += Speed;
            collisionRect.Location = new Point((int)Position.X - (collisionRect.Width / 2), (int)Position.Y - (collisionRect.Height));
            CheckCollision();

            
            skeleton.RootBone.X = Position.X;
            skeleton.RootBone.Y = Position.Y;
    

            if (faceDir == -1) skeleton.FlipX = true; else skeleton.FlipX = false;

            skeleton.UpdateWorldTransform();

            walking = false;
            Speed.X = 0f;
            Position = new Vector2((int)Position.X, (int)Position.Y);
        }

        public bool DestroyBlock(Vector2 Vect, Player Player)
        {
            if (!World.RaycastAny(Position + new Vector2(CollisionX / 2, CollisionY / 2), Vect /32))
            {
                World.GetBlock(Vect).Drop.PollDrop(Player);
                World.SetBlock(Vect, Block.GetBlock(0));
                return true;
            }
            return false;
        }


        public bool PlaceBlock(Block B, Vector2 Vect)
        {
            if (!World.RaycastAny(Position + new Vector2(CollisionX / 2, CollisionY / 2), Vect / 32))
            {
                if (World.GetBlock(Vect) == Block.GetBlock(0))
                {
                    if (B is IEntityBlock)
                    {
                        TileEntity TE =  (B as IEntityBlock).GetNewTE(this.World, Vect * 32 - Vector2.One * 16);
                        TE.Initalize();
                        World.SetBlock(Vect, TE);
                        return true;
                    }
                    else
                    {
                        World.SetBlock(Vect, B);
                        return true;
                    }
                }
            }
            return false;
        }

        public bool DestroyBackgroundBlock(Vector2 Vect)
        {
            if (!World.RaycastAny(Position + new Vector2(CollisionX/2, CollisionY/2),Vect /32)&& !World.GetBlock(Vect).IsSolid)
            {
                World.SetBackgroundBlock(Vect, Block.GetBlock(0));
                return true;
            }
            return false;
        }

        public void Draw(SpriteBatch spriteBatch, Matrix CameraMatrix)
        {

            spriteBatch.End();
            spriteBatch.Begin();

            skeletonRenderer.Begin(CameraMatrix);
            skeletonRenderer.Draw(skeleton);
            skeletonRenderer.End();
            //spriteBatch.GraphicsDevice.Reset();

            // Draw collision box
            //spriteBatch.Draw(GeneralManager.Textures["Textures/GUI/Frame"], new Rectangle(collisionRect.X , collisionRect.Y, collisionRect.Width, collisionRect.Height), Color.White * 0.3f);

            spriteBatch.End();
            spriteBatch.Begin();
        }


        public void MoveLeftRight(float dir)
        {
            if (grabbed || climbing) return;
            if (dir > 0) faceDir = 1; else faceDir = -1;

            Speed.X = dir * 2f;
            walking = true;
        }

        public void Jump()
        {
            if (grabbed && (Position - grabbedPosition).Length()<5f)
            {
                climbing = true;
                grabbed = false;
                animTime = 0;
                return;
            }

            if (!jumping && !crouching && !falling && !climbing && !grabbed)
            {
                jumping = true;
                animTime = 0;
                Speed.Y = -4f;
            }
        }

        public void Crouch()
        {
            if (grabbed)
            {
                grabbed = false;
                falling = true;
                justUngrabbed = true;
                Position.X += (-faceDir * 40f);
            }
            else
                if(!falling && !climbing && !justUngrabbed) crouching = true;
        }

        void CheckCollision()
        {
            Rectangle? collRect;

            // Check for ledge grabs
            if ((jumping || falling) && !justUngrabbed)
            {
                if (Speed.X<0 && World.CheckTileCollision(new Vector2(collisionRect.Left, collisionRect.Top)))
                    if (!World.CheckTileCollision(new Vector2(collisionRect.Left, collisionRect.Top - 32)) &&
                       !World.CheckTileCollision(new Vector2(collisionRect.Left + 32, collisionRect.Top - 32)) &&
                       !World.CheckTileCollision(new Vector2(collisionRect.Left + 32, collisionRect.Top)))
                    {
                        grabbed = true;
                        jumping = false;
                        falling = false;
                        crouching = false;
                        Speed.Y = 0;
                        Speed.X = 0;
                        grabbedPosition = new Vector2((int)(collisionRect.Left / GameWorld.TileWidth) * GameWorld.TileWidth, (int)(collisionRect.Top / GameWorld.TileHeight) * GameWorld.TileWidth) + new Vector2(25, 85);
                        faceDir = -1;
                    }

                if (Speed.X>0 && World.CheckTileCollision(new Vector2(collisionRect.Right, collisionRect.Top)))
                    if (!World.CheckTileCollision(new Vector2(collisionRect.Right, collisionRect.Top - 32)) &&
                       !World.CheckTileCollision(new Vector2(collisionRect.Right - 32, collisionRect.Top - 32)) &&
                       !World.CheckTileCollision(new Vector2(collisionRect.Right - 32, collisionRect.Top)))
                    {
                        grabbed = true;
                        jumping = false;
                        falling = false;
                        crouching = false;
                        Speed.Y = 0;
                        Speed.X = 0;
                        grabbedPosition = new Vector2((int)(collisionRect.Right / GameWorld.TileWidth) * GameWorld.TileWidth, (int)(collisionRect.Top / GameWorld.TileHeight) * GameWorld.TileWidth) + new Vector2(0, 85);
                        faceDir = 1;
                    }
            }

            if (grabbed || climbing) return;


            
                collRect = CheckCollisionBottom();
                if (collRect.HasValue)
                {
                    if (falling)
                    {
                        Speed.Y = 0f;
                        Position.Y -= collRect.Value.Height;
                        collisionRect.Offset(0, -collRect.Value.Height);
                        jumping = false;
                        falling = false;
                        justUngrabbed = false;
                    }
                    
                }
                else
                    falling = true;

                if (Speed.Y < 0f)
                {
                    collRect = CheckCollisionTop();
                    if (collRect.HasValue)
                    {
                        Speed.Y = 0f;
                        Position.Y += collRect.Value.Height;
                        collisionRect.Offset(justUngrabbed ? (collRect.Value.Width * (-faceDir)) : 0, collRect.Value.Height);
                        falling = true;
                        jumping = false;
                    }
                }
            
            
            

            if (Speed.X > 0f)
            {
                collRect = CheckCollisionRight();
                if (collRect.HasValue)
                {
                    Speed.X = 0f;
                    Position.X -= (collRect.Value.Width);
                    collisionRect.Offset(-collRect.Value.Width, 0);
                }
            }
            if (Speed.X < 0f)
            {
                collRect = CheckCollisionLeft();
                if (collRect.HasValue)
                {
                    Speed.X = 0f;
                    Position.X += collRect.Value.Width;
                    collisionRect.Offset(collRect.Value.Width, 0);
                }
            }


            bool collided = false;
            for (int y = -1; y > -15; y--)
            {
                collisionRect.Offset(0, -1);
                collRect = CheckCollisionTop();
                if (collRect.HasValue) collided = true;
            }
            if (!collided) crouching = false;

        }
        
        Rectangle? CheckCollisionTop()
        {
            for (float x = collisionRect.Left+5; x < collisionRect.Right-5; x += 1)
            {
                Vector2 checkPos = new Vector2(x, collisionRect.Top);
                Rectangle? collRect = World.CheckTileCollisionIntersect(checkPos, collisionRect);
                if (collRect.HasValue) return collRect;
            }

            return null;
        }
        Rectangle? CheckCollisionBottom()
        {
            for (float x = collisionRect.Left+5; x < collisionRect.Right-5; x += 1)
            {
                Vector2 checkPos = new Vector2(x, collisionRect.Bottom);
                Rectangle? collRect = World.CheckTileCollisionIntersect(checkPos, collisionRect);
                if (collRect.HasValue) return collRect;
            }

            return null;
        }
        Rectangle? CheckCollisionRight()
        {
            for (float y = collisionRect.Top; y < collisionRect.Bottom; y += 1)
            {
                Vector2 checkPos = new Vector2(collisionRect.Right, y);
                Rectangle? collRect = World.CheckTileCollisionIntersect(checkPos, collisionRect);
                if (collRect.HasValue) return collRect;
            }

            return null;
        }
        Rectangle? CheckCollisionLeft()
        {
            for (float y = collisionRect.Top; y < collisionRect.Bottom; y += 1)
            {
                Vector2 checkPos = new Vector2(collisionRect.Left, y);
                Rectangle? collRect = World.CheckTileCollisionIntersect(checkPos, collisionRect);
                if (collRect.HasValue) return collRect;
            }

            return null;
        }

    }
}
