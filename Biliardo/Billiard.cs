using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
          
            table = new Background(spriteBatch, Content.Load<Texture2D>("pool_table_v1"), 500, 1000);
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

            // TODO: Add your update logic here

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
            {
                b.Draw();
            }

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
            int X = (table.Widht / 4) * 3;
            int Y = table.Height / 2;
            int Yorig = table.Height / 2;            
            int index = 1;

            for (int i = 0; i < 5; i++)
            {
                X += (int) Math.Sqrt((double) (Ball.Radius * 4 * Ball.Radius) - (Ball.Radius * Ball.Radius));

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
    }
}
