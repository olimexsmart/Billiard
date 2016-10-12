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
    public class Ball : BilliardComponents, ICloneable
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
            return Math.Sqrt((Math.Pow(Position.X - otherBall.Position.X, 2d) + (Math.Pow(Position.Y - otherBall.Position.Y, 2d)))) < Ball.Radius * 2;
        }

        public bool SameDirection(Ball otherBall)
        {   //Vectorial proposition that chacks if two balls are going in directions opposite to each other
            return ((Vx - otherBall.Vx) * (Position.X - otherBall.Position.X) + (Vy - otherBall.Vy) * (Position.Y - otherBall.Position.Y)) < 0;
        }

        /*
            It directly updates the object data, so the collision on the other ball needs to
            be computed on a copy of the object
        */
        public void Collision(Ball otherBall)
        {
            double relPosX = Position.X - otherBall.Position.X;
            double relPosY = Position.Y - otherBall.Position.Y;
            double relVX = Vx - otherBall.Vx;
            double relVY = Vy - otherBall.Vy;
            double dotProduct = relVX * relPosX + relVY * relPosY;

            dotProduct /= relPosX * relPosX + relPosY * relPosY;
            relPosX *= dotProduct;
            relPosY *= dotProduct;

            Vx -= relPosX;
            Vy -= relPosY;
        }

        public void UpdatePosition(long dT)
        {
            Position.X += (int)Math.Round(Vx * dT);
            Position.Y += (int)Math.Round(Vy * dT);
        }

        //Useful to provide rapidly the original object when a collision occours
        public object Clone()
        {
            Ball newBall = new Ball(SpriteBatch, Texture, Position.X, Position.Y);

            newBall.Vx = Vx;
            newBall.Vy = Vy;

            return newBall;
        }
    }
}
