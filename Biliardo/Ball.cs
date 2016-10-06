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
    public class Ball : BilliardComponents
    {        
        public Point Position;
        public float Vx, Vy;
        public static int Radius = 20;

        private Ball() { }

        public Ball(SpriteBatch spriteBatch, Texture2D texture, int posX, int posY) : base(spriteBatch, texture)
        {            
            Position = new Point(posX, posY);
            Vx = 0;
            Vy = 0;            
        }

        public override void Draw()
        {
            where.X = Position.X + Radius;
            where.Y = Position.Y - Radius;
            where.Height = Radius * 2;
            where.Width = Radius * 2;
            base.Draw();
        }
    }
}
