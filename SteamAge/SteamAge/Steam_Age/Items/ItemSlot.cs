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

        public delegate void StackModified();
        public StackModified OnStackModified = null;

        public SlotState State = SlotState.Normal;

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

        public override void Draw(SpriteBatch SpriteBatch)
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
                    return HandleLMBClick();
                }
            }
            return false;
        }

        private bool HandleLMBClick()
        {
            ItemStack HoldingStack = World.GetCurrentPlayer().HoldingStack;
            if (ItemStack == null && State == SlotState.Normal || State == SlotState.OutputLocked)
            {
                InsetHoldingToSlot(HoldingStack);
            }
            else if (ItemStack != null)// if there is something in slot
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
                        MergeStacks(HoldingStack);
                    }
                    else // Switch
                    {
                        SwitchStacks(HoldingStack);
                    }
                }
            }
            
            //Call OnStackModified delegate

            if (OnStackModified != null)
            {
                OnStackModified();
            }
            return true;
        }

        private void InsetHoldingToSlot(ItemStack HoldingStack)
        {
            if (HoldingStack != null)
            {
                ItemStack = World.GetCurrentPlayer().HoldingStack;
                World.GetCurrentPlayer().HoldingStack = null;
            }
        }

        private void MergeStacks(ItemStack HoldingStack)
        {
            switch(State)
            {
                case SlotState.Normal:
                case SlotState.OutputLocked:
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
                    break;
                case SlotState.InputLocked:
                    if (HoldingStack.Count + ItemStack.Count <= ItemStack.MAX_STACK)
                    {
                        World.GetCurrentPlayer().HoldingStack.Count = HoldingStack.Count + ItemStack.Count;
                        ItemStack = null;
                    }
                    else
                    {
                        ItemStack.Count = HoldingStack.Count + ItemStack.Count - ItemStack.MAX_STACK;
                        World.GetCurrentPlayer().HoldingStack.Count = ItemStack.MAX_STACK;
                    }
                    break;
                case SlotState.IOLocked:
                    //nothing
                    break;
            }
        }

        private void SwitchStacks(ItemStack HoldingStack)
        {
            ItemStack TmpStack = HoldingStack;
            World.GetCurrentPlayer().HoldingStack = ItemStack;
            ItemStack = TmpStack;
        }

        public override void Update(GameTime gameTime)
        {
            
        }

        public enum SlotState
        {
            Normal,
            InputLocked,
            OutputLocked,
            IOLocked
        }
    }
}
