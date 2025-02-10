using System;
using System.ComponentModel.DataAnnotations.Schema;
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
        float armRotation = 0.1f; //so the cool animation play and get it shakey
        float aiPushForce = 2f; //ai strength

        KeyboardState keyboardState, prevKeyboardState;

        const float minRotation = -0.39f; //left
        const float maxRotation = 1.5f;  //right
        const float returnSpeed = 0.01f;  //speed at which the arm resets
        const float moveStep = 0.10f; //spacebar power

        bool aiActivated = false;
        bool isFrozen = false;
        int gameWinStatus = 0;    
        //0 = nothing
        //1 = win
        //2 = loss

        //better ai
        float aiPushForceADD = 0f;

        //random number generator
        Random rnd = new Random();

        //visuals player stuff
        int angerLevel = 1;
        //1 = scared
        //2 = weak
        //3 = normal
        //4 = angry
        //5 = very angry
       

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

            seconds = 0f;

            //aiPushForce = rnd.Next(1, 4) * 0.01f; //base stat when game starts

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

            seconds += (float)gameTime.ElapsedGameTime.TotalSeconds;

            prevKeyboardState = keyboardState;
            keyboardState = Keyboard.GetState();

            // TODO: Add your update logic here
            //make winodow title show the aipushforce varible
            //TEST PURPOSE
            Window.Title = $"AI Push: {aiPushForce:F4} | Anger Level: {angerLevel}";

            //every second add +/- to the ai base stats
            //NOT WORKING
            if (gameTime.TotalGameTime.TotalSeconds % 1 == 0)
{
                aiPushForceADD = rnd.Next(-3, 3) * 0.01f;
                aiPushForce += aiPushForceADD;
                //dont work
            }

            //anger level for visual
            if (aiPushForce <= 0.011f)
                angerLevel = 1; // Scared (Weak AI)
            else if (aiPushForce <= 0.017f)
                angerLevel = 2; 
            else if (aiPushForce <= 0.023f)
                angerLevel = 3; 
            else if (aiPushForce <= 0.029f)
                angerLevel = 4; 
            else
                angerLevel = 5; // Angry (Strong AI)


            //reset - rematch
            if (keyboardState.IsKeyDown(Keys.R) && prevKeyboardState.IsKeyUp(Keys.R))
            {
                armRotation = 0.4f;
                isFrozen = false;  
                aiActivated = false;
                gameWinStatus = 0;
                aiPushForce = rnd.Next(1, 4) * 0.01f; //ai base stat reset every rematch
            }
            //if winstat 2 then it freezes
            if (gameWinStatus == 2)
            {
                isFrozen = true;
            }

            if (isFrozen)
                return;

            //play need to press space
            if (keyboardState.IsKeyDown(Keys.Space) && prevKeyboardState.IsKeyUp(Keys.Space))
            {
                armRotation -= moveStep; 
                aiActivated = true;  
            }
            //start only when the player is ready
            if (aiActivated && armRotation < maxRotation)
            {
                armRotation += aiPushForce;
            }

            //every space press moves the arm to the left counterclockwise
            if (keyboardState.IsKeyDown(Keys.Space) && prevKeyboardState.IsKeyUp(Keys.Space))
            {
                armRotation -= moveStep;
            }

            //rotate left (counterclockwise)
            if (keyboardState.IsKeyDown(Keys.Left) && armRotation > minRotation)
            {
                armRotation -= 0.03f;
            }

            //rotate right (clockwise)
            if (keyboardState.IsKeyDown(Keys.Right) && armRotation < maxRotation)
            {
                armRotation += 0.03f;
            }

            //prevent arm from exceeding min/max rotation
            if (armRotation <= minRotation)
            {
                armRotation = minRotation;
                isFrozen = true;
                gameWinStatus = 1;
            }
            else if (armRotation >= maxRotation)
            {
                armRotation = maxRotation;
                gameWinStatus = 2;
            }

            if (aiActivated)
                return;
            //return to orginal pos
            if (!keyboardState.IsKeyDown(Keys.Left) && !keyboardState.IsKeyDown(Keys.Right) & !(keyboardState.IsKeyDown(Keys.Space) && prevKeyboardState.IsKeyUp(Keys.Space)))
            {
                if (armRotation > 0.4f)
                {
                    armRotation -= returnSpeed;
                }
                else if (armRotation < 0.4f)
                {
                    armRotation += ((returnSpeed)/2);
                }
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
            _spriteBatch.Draw(tempArm, new Vector2(700, 730), null, Color.White, armRotation, new Vector2(455, 505), 1.0f, SpriteEffects.None, 0f);

            _spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}
