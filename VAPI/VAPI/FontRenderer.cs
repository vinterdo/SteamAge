using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VAPI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace VAPI
{
    public class FontRenderer
    {
        public FontRenderer(FontFile fontFile, Texture2D fontTexture)
        {
            _fontFile = fontFile;
            _texture = fontTexture;
            _characterMap = new Dictionary<char, FontChar>();

            foreach (var fontCharacter in _fontFile.Chars)
            {
                char c = (char)fontCharacter.ID;
                _characterMap.Add(c, fontCharacter);
            }
        }

        private Dictionary<char, FontChar> _characterMap;
        private FontFile _fontFile;
        private Texture2D _texture;

        public void DrawText(SpriteBatch spriteBatch, int x, int y, string text)
        {
            DrawText(spriteBatch, x, y, text, Color.White);
        }

        public void DrawText(SpriteBatch spriteBatch, int x, int y, string text, Color Color)
        {
            int dx = x;
            int dy = y;
            foreach (char c in text)
            {
                FontChar fc;
                if (_characterMap.TryGetValue(c, out fc))
                {
                    var sourceRectangle = new Rectangle(fc.X, fc.Y, fc.Width, fc.Height);
                    var position = new Vector2(dx + fc.XOffset, dy + fc.YOffset);

                    spriteBatch.Draw(_texture, position, sourceRectangle, Color);
                    dx += fc.XAdvance;
                }
            }
        }

        public void DrawText(SpriteBatch spriteBatch, Rectangle Position, string text, Color Color)
        {
            float Scale = (float)(this.MeasureString(text))/(float)(Position.Width);
            int CurrentLenght = Position.X;

            foreach (char c in text)
            {
                FontChar fc;
                if (_characterMap.TryGetValue(c, out fc))
                {
                    var sourceRectangle = new Rectangle(fc.X, fc.Y, fc.Width, fc.Height);
                    Rectangle DestRect = new Rectangle(CurrentLenght, Position.Y, (int)( fc.Width / Scale), Position.Height);

                    spriteBatch.Draw(_texture, DestRect, sourceRectangle, Color);
                    CurrentLenght += (int)(fc.Width / Scale);
                }
            }
        }

        public int MeasureString(string Text)
        {
            int Sum = 0;
            foreach (char C in Text.ToCharArray())
            {
                Sum += _characterMap[C].Width;
            }
            return Sum;
        }
    }
}

