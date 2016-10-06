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

        private Background() { }

        public Background(SpriteBatch spriteBatch, Texture2D texture, int height, int widht) : base(spriteBatch, texture)
        {         
            Height = height;
            Widht = widht;
        }

        public override void Draw()
        {
            where.Height = Height;
            where.Width = Widht;
            base.Draw();
        }
    }
}
