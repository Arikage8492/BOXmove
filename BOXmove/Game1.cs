using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct2D1;
using System;

namespace BOXmove
{
    public class Game1 : Game
    {
        Texture2D playerTexture;
        Vector2 playerPosition = new Vector2(100, 100);
        Vector2 playerVelocity;
        float playerSpeed = 200f;
        bool onGround = false;

        Texture2D floorTexture;
        Rectangle floorRect;

        Texture2D wall_left_texture;
        Texture2D wall_right_texture;
        Rectangle wall_left_rect;
        Rectangle wall_right_rect;



        private GraphicsDeviceManager _graphics;
        private Microsoft.Xna.Framework.Graphics.SpriteBatch _spriteBatch;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new Microsoft.Xna.Framework.Graphics.SpriteBatch(GraphicsDevice);

            // make a simple player texture (a white square)

            playerTexture = new Texture2D(GraphicsDevice, 1, 1);
            playerTexture.SetData(new[] { Color.White });

            playerPosition = new Vector2(100, 100); // starting position

            // make a simple floor texture (a gray rectangle)
            floorTexture = new Texture2D(GraphicsDevice, 1, 1);
            floorTexture.SetData(new[] { Color.Gray });

            // make left wall texure
            wall_left_texture = new Texture2D(GraphicsDevice, 1, 1);
            wall_left_texture.SetData(new[] { Color.Brown });

            // make right wall texture
            wall_right_texture = new Texture2D(GraphicsDevice, 1, 1);
            wall_right_texture.SetData(new[] { Color.Brown });

            // create floor rectangle
            floorRect = new Rectangle(0, 450, 800, 50);  // X, Y, Width, Height

            // create left wall rectangle
            wall_left_rect = new Rectangle(0, 0, 50, 480);  // X, Y, Width, Height

            // create right wall rectangle
            wall_right_rect = new Rectangle(750, 0, 50, 480);  // X, Y, Width, Height


            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            KeyboardState ks = Keyboard.GetState();

            // Horizontal movement
            if (ks.IsKeyDown(Keys.A))
                playerVelocity.X = -playerSpeed;
            else if (ks.IsKeyDown(Keys.D))
                playerVelocity.X = playerSpeed;
         

            // Apply friction when no left/right input
            if (!ks.IsKeyDown(Keys.A) && !ks.IsKeyDown(Keys.D))
            {
                playerVelocity.X *= 0.88f;  // friction factor 0.85 default

                // prevent jittery tiny sliding
                if (Math.Abs(playerVelocity.X) < 1f)
                    playerVelocity.X = 0f;
            }

            // Jump
            if (ks.IsKeyDown(Keys.W) && onGround)
            {
                playerVelocity.Y = -500f;
                onGround = false;
            }

            // Gravity
            playerVelocity.Y += 20f;

            // Apply movement
            playerPosition += playerVelocity * dt;

            // Collision with floor
            Rectangle playerRect = new Rectangle(
                (int)playerPosition.X,
                (int)playerPosition.Y,
                50, 50
            );

            

            if (playerRect.Intersects(floorRect))
            {
                // place player on top of floor
                playerPosition.Y = floorRect.Y - 50;
                playerVelocity.Y = 0;
                onGround = true;
            }

            // --- WALL COLLISION --- //

            playerRect = new Rectangle(
                (int)playerPosition.X,
                (int)playerPosition.Y,
                20, 50
            );

            // Left wall
            if (playerRect.Intersects(wall_left_rect))
            {
                playerPosition.X = wall_left_rect.Right;   // push player to the right
                playerVelocity.X = 0;
            }

            // Right wall
            if (playerRect.Intersects(wall_right_rect))
            {
                playerPosition.X = wall_right_rect.Left - 20;  // push player to the left
                playerVelocity.X = 0;
            }


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            // Draw a 50×50 white square
            _spriteBatch.Draw(
                playerTexture,
                new Rectangle((int)playerPosition.X, (int)playerPosition.Y, 20, 50),
                Color.Red              // tint — this will make the square red
            );

            // Draw floor
            _spriteBatch.Draw(floorTexture, floorRect, Color.DarkGray);

            // Draw left wall
            _spriteBatch.Draw(wall_left_texture, wall_left_rect, Color.Brown);

            // Draw right wall
            _spriteBatch.Draw(wall_right_texture, wall_right_rect, Color.Brown);


            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
