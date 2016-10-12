using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace Biliardo
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Billiard : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Ball[] balls = new Ball[16];
        Background table;
        Stopwatch update;
        double friction;

        public Billiard()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            graphics.PreferredBackBufferWidth = 1000;  // set this value to the desired width of your window
            graphics.PreferredBackBufferHeight = 500;   // set this value to the desired height of your window
            graphics.ApplyChanges();
            this.TargetElapsedTime = TimeSpan.FromSeconds(1.0f / 100.0f);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            table = new Background(spriteBatch, Content.Load<Texture2D>("pool_table_v1"), 500, 1000, 60);
            Texture2D ballsImage = Content.Load<Texture2D>("biliardTrasp");

            //Loading up portions of the image into the objects
            int offset = 17;
            int space = 6;
            int diameter = 90;
            for (int i = 0; i < balls.Length; i++)
                balls[i] = new Ball(spriteBatch, Crop(ballsImage, new Rectangle(offset + ((space + diameter) * (i % 4)), offset + ((space + diameter) * (i / 4)), diameter, diameter)), 0, 0);

            //Putting the white ball in the first position of the aray
            Ball[] temp = new Ball[16];
            temp[0] = balls[15];
            for (int i = 0; i < balls.Length - 1; i++)
                temp[i + 1] = balls[i];
            balls = temp;

            InitialBallPosition();

            //Delta T between updates
            update = new Stopwatch();
            update.Start();

            balls[0].Vx = 0.85d;
            balls[0].Vy = 0.10d;
            friction = 0.00012d;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            long dT = update.ElapsedMilliseconds;
            foreach (Ball b in balls)            
                b.UpdateFriction(dT);
            
        
            //Collision detection
            for (int i = 0; i < balls.Length; i++)
            {
                //Fence collision first
                if ((balls[i].Position.X + Ball.Radius < table.Border & balls[i].Vx < 0) | (balls[i].Position.X + Ball.Radius > table.Widht - table.Border - 33 & balls[i].Vx > 0))
                    balls[i].Vx = -balls[i].Vx;
                if ((balls[i].Position.Y - Ball.Radius < table.Border & balls[i].Vy < 0) | (balls[i].Position.Y + Ball.Radius > table.Height - table.Border & balls[i].Vy > 0))
                    balls[i].Vy = -balls[i].Vy;

                //Once the collision is detected the update goes on both balls involved
                for (int k = i + 1; k < balls.Length; k++)
                {   //Checking if the distance between the two centers is smaller than two radius
                    bool contact = balls[i].CheckCollision(balls[k]);
                    bool sameDirection = balls[i].SameDirection(balls[k]);
                    if (contact && sameDirection)
                    {
                        double[] newVeli = Collision(balls[i].Vx, balls[i].Vy, balls[k].Vx, balls[k].Vy, balls[i].Position.X, balls[i].Position.Y, balls[k].Position.X, balls[k].Position.Y);
                        double[] newVelk = Collision(balls[k].Vx, balls[k].Vy, balls[i].Vx, balls[i].Vy, balls[k].Position.X, balls[k].Position.Y, balls[i].Position.X, balls[i].Position.Y);

                        balls[i].Vx = newVeli[0];
                        balls[i].Vy = newVeli[1];

                        balls[k].Vx = newVelk[0];
                        balls[k].Vy = newVelk[1];
                    }
                }
            }


            //Position update            
            foreach (Ball b in balls)
            {
                b.Position.X += (int) Math.Round(b.Vx * dT);
                b.Position.Y += (int) Math.Round(b.Vy * dT);
            }

            update.Restart();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            table.Draw();

            foreach (Ball b in balls)
                b.Draw();

            spriteBatch.End();

            base.Draw(gameTime);
        }

        private static Texture2D Crop(Texture2D source, Rectangle area)
        {
            if (source == null)
                return null;

            Texture2D cropped = new Texture2D(source.GraphicsDevice, area.Width, area.Height);
            Color[] data = new Color[source.Width * source.Height];
            Color[] cropData = new Color[cropped.Width * cropped.Height];

            source.GetData(data);

            int index = 0;
            for (int y = area.Y; y < area.Y + area.Height; y++)
            {
                for (int x = area.X; x < area.X + area.Width; x++)
                {
                    cropData[index] = data[x + (y * source.Width)];
                    index++;
                }
            }

            cropped.SetData(cropData);

            return cropped;
        }

        //Place balls in the intial triangle
        private void InitialBallPosition()
        {
            int X = ((table.Widht - 2 * table.Border) / 4) * 3 + table.Border;
            int Y = table.Height / 2;
            int Yorig = table.Height / 2;
            int index = 1;

            balls[0].Position.Y = Y;
            balls[0].Position.X = ((table.Widht - 2 * table.Border) / 4) + table.Border;

            for (int i = 0; i < 5; i++)
            {
                X += (int)Math.Sqrt((double)(Ball.Radius * 4 * Ball.Radius) - (Ball.Radius * Ball.Radius));

                for (int k = 0; k < i + 1; k++)
                {
                    balls[index].Position.X = X;
                    balls[index].Position.Y = Y;

                    index++;

                    Y -= Ball.Radius * 2;
                }

                Y = Yorig + Ball.Radius * (i + 1);
            }
        }

        private double[] Collision(double v1x, double v1y, double v2x, double v2y, int x1x, int x1y, int x2x, int x2y)
        {
            double relPosX = x1x - x2x;
            double relPosY = x1y - x2y;
            double relVX = v1x - v2x;
            double relVY = v1y - v2y;
            double dotProduct = relVX * relPosX + relVY * relPosY;

            dotProduct /= relPosX * relPosX + relPosY * relPosY;
            relPosX *= dotProduct;
            relPosY *= dotProduct;

            return new double[] { v1x - relPosX, v1y - relPosY };
        }
    }
}
