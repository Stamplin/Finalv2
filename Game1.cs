using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Finalv2
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        //import texture
        Texture2D backgroundTexture, tempArm;
        //game variables
        float armRotation = 0.4f;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _graphics.PreferredBackBufferHeight = 720;
            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.ApplyChanges();


            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            backgroundTexture = Content.Load<Texture2D>("Background");
            tempArm = Content.Load<Texture2D>("Arm");

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            //limt arm movement so it wont go past a position
            if (armRotation > 1f)
            {
                armRotation = 0.5f;
            }
            else if (armRotation < 1f)
            {
                armRotation = 0.5f;
            }




            //intitial arm rotation position
            if (Keyboard.GetState().IsKeyDown(Keys.R))
            {
                armRotation = 0.4f;
            }

            //when left arrow is pressed arm spin counterclockwise
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                armRotation -= 0.1f;
            }
            //when right arrow is pressed arm spin clockwise
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                armRotation += 0.1f;
            }

            //make the arm slowly return to centre at armRotation = 0.4f;
            if (armRotation > 0.4f)
            {
                armRotation -= 0.02f;
            }
            else if (armRotation < 0.4f)
            {
                armRotation += 0.02f;
            }



            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            _spriteBatch.Draw(backgroundTexture, new Rectangle(0, 0, 1280, 720), Color.White);

            //draw arm and spin/roate it based on the armRotation variable
            _spriteBatch.Draw(tempArm, new Vector2(700, 530), null, Color.White, armRotation, new Vector2(tempArm.Width / 2, tempArm.Height / 2), 1.0f, SpriteEffects.None, 0f);

            _spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}
