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

namespace SteamAge
{
    public class ItemSlot : GUIComponent
    {
        public ItemStack ItemStack;
        public Rectangle Position; // can be null, beware!
        public bool Visible = false; // if (Visible) -> Positions is not null
        public GameWorld World;

        public ItemSlot(GameWorld World):base()
        {
            this.World = World;
        }

        public override bool CheckActive()
        {
            if (Visible)
            {
                if (Position.Contains(Helper.VectorToPoint(GeneralManager.MousePos)))
                    return true;
            }
            return false;
        }

        public override void Draw(SpriteBatch SpriteBatch, GameTime GameTime)
        {
            if (Visible)
            {
                if (CheckActive())
                {
                    SpriteBatch.Draw(GeneralManager.Textures["Textures/GUI/ItemSlot"], Position, Color.White);

                }
                else
                {
                    SpriteBatch.Draw(GeneralManager.Textures["Textures/GUI/ItemSlot"], Position, Color.Gray);
                }

                if (ItemStack != null)
                {
                    ItemStack.DrawStack(SpriteBatch, Position);
                }
            }
        }
        public override bool HandleInput()
        {
            if (Visible)
            {
                if (CheckActive() && GeneralManager.IsLMBClickedEdge())
                {
                    ItemStack HoldingStack = World.GetCurrentPlayer().HoldingStack;
                    if (ItemStack == null)
                    {
                        if (HoldingStack != null)
                        {
                            ItemStack = World.GetCurrentPlayer().HoldingStack;
                            World.GetCurrentPlayer().HoldingStack = null;
                        }
                        // else {if both null do nothing}
                    }
                    else // if there is something in slot
                    {
                        if (HoldingStack == null)
                        {
                            World.GetCurrentPlayer().HoldingStack = this.ItemStack;
                            this.ItemStack = null;
                        }
                        else
                        {
                            if (HoldingStack.Item == ItemStack.Item) 
                            {
                                if (HoldingStack.Count + ItemStack.Count <= ItemStack.MAX_STACK)
                                {
                                    ItemStack.Count = HoldingStack.Count + ItemStack.Count;
                                    World.GetCurrentPlayer().HoldingStack = null;
                                }
                                else
                                {
                                    World.GetCurrentPlayer().HoldingStack.Count = HoldingStack.Count + ItemStack.Count - ItemStack.MAX_STACK;
                                    ItemStack.Count = ItemStack.MAX_STACK;
                                }
                            }
                            else // Switch
                            {
                                ItemStack TmpStack = HoldingStack;
                                World.GetCurrentPlayer().HoldingStack = ItemStack;
                                ItemStack = TmpStack;
                            }
                        }
                    }

                    return true;
                }

                if ( this.IsActive) // dzicz totalna
                {
                    if (World.GetCurrentPlayer().HoldingStack != null)
                    {
                        if (ItemStack == null)
                        {
                            ItemStack = new ItemStack(World.GetCurrentPlayer().HoldingStack.Item, (int)Math.Ceiling(World.GetCurrentPlayer().HoldingStack.Count / 2f));
                            World.GetCurrentPlayer().HoldingStack.Count = (int)Math.Floor(ItemStack.Count / 2f);
                            //ItemStack.Count = (int)Math.Ceiling(ItemStack.Count / 2f);
                        }
                    }
                    return true;
                }
            }
            return false;
        }

        public override void Update(GameTime gameTime)
        {
            
        }
    }
}
