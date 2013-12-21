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
using SteamAge;
using SteamAge.TileEntities;

namespace BasicMachines
{
    public class FurnaceTE : TileEntity, IMultiBlockTE
    {
        MultiBlockDef MultiBlock;
        public FurnaceTE(GameWorld World)
            : base(World, "Furnace")
        {
            this.Entity = new FurnaceEntity(World);
            this.TileBlock = Block.GetBlock(1003); 
            this.Name = "Textures/TileEntities/Furnace";

            MultiBlock = new MultiBlockDef(new Vector2(2, 2), Vector2.One);
            MultiBlock.SetBlock(0, 0, Block.GetBlock(1003));
            MultiBlock.SetBlock(0, 1, Block.GetBlock(1003));
            MultiBlock.SetBlock(1, 0, Block.GetBlock(1003));
            MultiBlock.SetBlock(1, 1, Block.GetBlock(1003));
        }

        public MultiBlockDef GetMultiBlockDef()
        {
            return MultiBlock;
        }

        public bool CanBePlaced(Vector2 BlockPos)
        {
            for (int y = 0; y < MultiBlock.Size.Y; y++)
            {
                for (int x = 0; x < MultiBlock.Size.X; x++)
                {
                    if (World.GetBlock(new Vector2(x, y) + Position) != Block.GetBlock(0))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public Vector2 GetCenterCoord()
        {
            return Position;
        }

        public override void Kill()
        {
            for (int y = 0; y < MultiBlock.Size.Y; y++)
            {
                for (int x = 0; x < MultiBlock.Size.X; x++)
                {
                    World.SetTileEntity(new Vector2(x, y) + Position, null);
                    World.SetBlock(new Vector2(x, y) + Position, Block.GetBlock(0));
                }
            }

            base.Kill();
        }
    }

    public class FurnaceEntity : Entity
    {
        public FurnaceEntity(GameWorld World)
            : base(World)
        {
        }

    }

    public class FurnaceBlock : Block, IEntityBlock
    {
        public FurnaceBlock()
            : base()
        {
            IsSolid = true;
            Family = "Furnace";
            Tex = "Textures/TileEntities/Furnace";
            Id = 1003;
            Textures[0] = new KeyValuePair<Block.BlockState, string>((Block.BlockState)0, "Textures/TileEntities/Furnace");

            Drop.AddDrop(1, 0, this);
        }
        
        public TileEntity GetNewTE(GameWorld World, Vector2 BlockPos)
        {
            return new FurnaceTE(World);
        }
    }

}
