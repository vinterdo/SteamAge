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
using SteamAge.Generators;

namespace SteamAge
{
    public class WorldGenerator
    {
        GameWorld _world;
        public List<Generator> Generators;

        public WorldGenerator(GameWorld World)
        {
            _world = World;
            Generators = new List<Generator>();
        }

        public void RegisterGenerator(Generator G)
        {
            Generators.Add(G);
        }

        public void Generate(Vector2 Pos)
        {
            if (!_world.Chunks.ContainsKey(Pos))
            {
                Chunk C = new Chunk(_world, Pos);

                _world.Chunks.Add(Pos, C);

                foreach (Generator G in Generators)
                {
                    G.Generate(C);
                }

                if (_world.Chunks.ContainsKey(Pos - new Vector2(0, 1)))
                {
                    for (int i = 0; i < GameWorld.ChunkSize; i++)
                    {
                        _world.Chunks[Pos - new Vector2(0, 1)].CheckEdges(i, 15);
                        _world.Chunks[Pos - new Vector2(0, 1)].CheckEdgesBackground(i, 15);
                    }

                    _world.Chunks[Pos - new Vector2(0, 1)].CalcLights();
                }

                if (_world.Chunks.ContainsKey(Pos - new Vector2(1, 0)))
                {
                    for (int i = 0; i < GameWorld.ChunkSize; i++)
                    {
                        _world.Chunks[Pos - new Vector2(1, 0)].CheckEdges(15, i);
                        _world.Chunks[Pos - new Vector2(1, 0)].CheckEdgesBackground(15, i);
                    }
                    _world.Chunks[Pos - new Vector2(1, 0)].CalcLights();
                }

                if (_world.Chunks.ContainsKey(Pos - new Vector2(0, -1)))
                {
                    for (int i = 0; i < GameWorld.ChunkSize; i++)
                    {
                        _world.Chunks[Pos - new Vector2(0, -1)].CheckEdges(0, i);
                        _world.Chunks[Pos - new Vector2(0, -1)].CheckEdgesBackground(0, i);
                    }
                    _world.Chunks[Pos - new Vector2(0, -1)].CalcLights();
                }

                if (_world.Chunks.ContainsKey(Pos - new Vector2(-1, 0)))
                {
                    for (int i = 0; i < GameWorld.ChunkSize; i++)
                    {
                        _world.Chunks[Pos - new Vector2(-1, 0)].CheckEdges(i, 0);
                        _world.Chunks[Pos - new Vector2(-1, 0)].CheckEdgesBackground(i, 0);
                    }
                    _world.Chunks[Pos - new Vector2(-1, 0)].CalcLights();
                }
            }
        }

        
    }
}
