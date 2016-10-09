using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Biliardo
{
    public class Background : BilliardComponents
    {        
        public int Height { get; private set; }
        public int Widht { get; private set; }
        public int Border { get; private set; }

        private Background() { }

        public Background(SpriteBatch spriteBatch, Texture2D texture, int height, int widht, int border) : base(spriteBatch, texture)
        {         
            Height = height;
            Widht = widht;
            Border = border;
        }

        public override void Draw()
        {
            where.Height = Height;
            where.Width = Widht;
            base.Draw();
        }
    }
}
