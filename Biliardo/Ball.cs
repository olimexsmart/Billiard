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
        public double Vx, Vy;
        public static int Radius = 16;
        public static double Friction = 0.00012d;

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

        public void UpdateFriction(long dT)
        {
            if (Vx > 0) Vx -= Friction * Vx * dT;
            else Vx += Friction * -Vx * dT;

            if (Vy > 0) Vy -= Friction * Vy * dT;
            else Vy += Friction * -Vy * dT;
        }

        public bool CheckCollision(Ball otherBall)
        {   //This simply checks if the distance of the two centers is shorter of a ball diameter
            return Math.Sqrt((Math.Pow(Position.X - otherBall.Position.X, 2d) + (Math.Pow(Position.Y - otherBall.Position.Y, 2d)))) <= Ball.Radius * 2;
        }

        public bool SameDirection(Ball otherBall)
        {   //Vectorial proposition that chacks if two balls are going in directions opposite to each other
            return ((Vx - otherBall.Vx) * (Position.X - otherBall.Position.X) + (Vy - otherBall.Vy) * (Position.Y - otherBall.Position.Y)) < 0;
        }


    }
}
