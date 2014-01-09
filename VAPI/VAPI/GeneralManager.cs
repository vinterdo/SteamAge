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
using System.Resources;
using System.Reflection;
namespace VAPI
{
    public class GeneralManager
    {
        public static GameScreen CurrentScreen;
        public static Dictionary<string, GameScreen> GameScreens = new Dictionary<string, GameScreen>();
        static KeyboardState CurrentKeyboardState;
        static KeyboardState OldKeyboardState;
        public static MouseState MouseState;
        public static MouseState OldMouseState;
        public static ContentManager Content;
        public static Vector2 MousePos;

        public static Dictionary<string, Texture2D> Textures;
        public static Dictionary<string, FontRenderer> Fonts;
        public static Dictionary<string, Effect> Effects;

        public static Game Game;

        public static int ScreenX;
        public static int ScreenY;

        public static GraphicsDevice GDevice;
        


        public static void Initalize(ContentManager _Content, int _ScreenX, int _ScreenY, Game _Game)
        {
            Content = _Content;
            Textures = new Dictionary<string, Texture2D>();
            Fonts = new Dictionary<string, FontRenderer>();
            Effects = new Dictionary<string, Effect>();

            ScreenX = _ScreenX;
            ScreenY = _ScreenY;

            Game = _Game;
            GDevice = _Game.GraphicsDevice;

            Logger.Init();
        }

        public static void LoadTex(string Name)
        {
            Textures.Add(Name, Content.Load<Texture2D>(Name));
            Logger.Write("Loaded " + Name + " Texture");
        }

        public static void LoadFont(string Name)
        {
            //Fonts.Add(Name, Content.Load<SpriteFont>(Name));
            var fontFilePath = Path.Combine(Content.RootDirectory, Name + ".FNT");
            var fontFile = FontLoader.Load(fontFilePath);
            var fontTexture = Content.Load<Texture2D>(Name + "_0");

            FontRenderer FR = new FontRenderer(fontFile, fontTexture);
            Fonts.Add(Name, FR);


            Logger.Write("Loaded " + Name + " Font");
        }

        public static void LoadEffect(string Name)
        {
            //Effects.Add(Name, Content.Load<Effect>(Name + ".mgfxo"));

            Logger.Write("Loaded " + Name + " Effect");
            
        }

        public static bool CheckKeyEdge(Keys Key)
        {
            return CurrentKeyboardState.IsKeyDown(Key) && ! OldKeyboardState.IsKeyDown(Key);
        }

        public static bool CheckKey(Keys Key)
        {
            return CurrentKeyboardState.IsKeyDown(Key);
        }

        public static void Update()
        {
            UpdateKeyboard();
            UpdateMouse();
        }

        static void UpdateKeyboard()
        {
            OldKeyboardState = CurrentKeyboardState;
            CurrentKeyboardState = Keyboard.GetState();
        }

        static void UpdateMouse()
        {
            OldMouseState = MouseState;
            MouseState = Mouse.GetState();

            MousePos = new Vector2(MouseState.X, MouseState.Y);
        }

        public static bool IsLMBClicked()
        {
            return MouseState.LeftButton == ButtonState.Pressed;
        }

        public static bool IsRMBClicked()
        {
            return MouseState.RightButton == ButtonState.Pressed;
        }

        public static bool IsLMBClickedEdge()
        {
            return MouseState.LeftButton == ButtonState.Pressed && !(OldMouseState.LeftButton == ButtonState.Pressed);
        }

        public static bool IsRMBClickedEdge()
        {
            return MouseState.RightButton == ButtonState.Pressed && !(OldMouseState.RightButton == ButtonState.Pressed);
        }


        static public int GetPartialWidth(float Part)
        {
            return (int)(ScreenX * Part);
        }

        static public int GetPartialHeight(float Part)
        {
            return (int)(ScreenX * Part);
        }

        static public Vector2 GetPartialVector(float PartX, float PartY)
        {
            return new Vector2(ScreenX * PartX, ScreenY * PartY);
        }

        static public Rectangle GetPartialRect(float PartX, float PartY, float PartWidth, float PartHeight)
        {
            return new Rectangle((int)(ScreenX * PartX), (int)(ScreenY * PartY), (int)(ScreenX * PartWidth), (int)(ScreenY * PartHeight));
        }

        public static int HalfWidth
        {
            get
            {
                return (int)ScreenX / 2;
            }
        }

        public static int HalfHeight
        {
            get
            {
                return (int)ScreenY / 2;
            }
        }
    }
}
