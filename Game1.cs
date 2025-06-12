using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Finalv2
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        MouseState _mouseState;
        KeyboardState _keyboardState;

        Vector2 windowSize = new (1280, 720);

        //random number generator
        Random rnd = new Random();

        #region drinking game
        //drink variables

        //combo varible 
        int comboOne;
        int comboTwo;
        int comboThree;

        //combo input
        int inputOne;
        int inputTwo;
        int inputThree;


        //bools
        bool drinkingGameComplete = false;

        bool inputOneComplete = false;
        bool inputTwoComplete = false;
        bool inputThreeComplete = false;





        #endregion

        #region arm wrestling
        //import texture
        Texture2D backgroundTexture, tempArm;
        //game variables
        float armRotation = 0.1f; //so the cool animation play and get it shaky
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
        float aiPushForceTimer = 0f;
        const float aiPushInterval = 1f; //AI push force every whatever seconds

        //visuals player stuff
        int angerLevel = 1;
        //1 = scared
        //2 = weak
        //3 = normal
        //4 = angry
        //5 = very angry
        #endregion

        #region shooting game
        //shootingDuel variables

        //bools
        bool shootingDuelComplete = false;

        //movement

        //textures
        Texture2D gunTexture, bgTextureShoot;


        #endregion









        #region boxing game

        //textures
        Texture2D ringBg;
        Texture2D fistTexture;
        Texture2D crosshairTexture;

        //variables
        Vector2 fistPosition;
        bool isBlocking;
        bool punchLeft;
        bool punchRight;
        float actionCooldown = 0.3f;
        float actionTimer = 0f;

        float scaleZoom = 0;
        //Vector2 offset;
        Vector2 offset = new Vector2(0, 0);

        float drawScale;
        float drawnWidth;
        float drawnHeight;

        #endregion





















        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _graphics.PreferredBackBufferHeight = (int)windowSize.Y;
            _graphics.PreferredBackBufferWidth = (int)windowSize.X;
            _graphics.ApplyChanges();

            //arm wrestling
            aiPushForce = rnd.Next(1, 4) * 0.01f; //base stat when game starts







            fistPosition = new Vector2(0, 300);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);


            //armwrestling
            backgroundTexture = Content.Load<Texture2D>("Arm/Background");
            tempArm = Content.Load<Texture2D>("Arm/Arm");

            //drinking game
            // will add later

            //shooting
            //gunTexture = Content.Load<Texture2D>("ShootingGame/Gun");
            //bgTextureShoot = Content.Load<Texture2D>("ShootingGame/Background");

            //boxing
            ringBg = Content.Load<Texture2D>("Boxing/boxingbg");
            fistTexture = Content.Load<Texture2D>("Boxing/fist");
            crosshairTexture = Content.Load<Texture2D>("Boxing/crosshair");



            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //keyboard, mouse, time
            KeyboardState keyboardstate = Keyboard.GetState();
            MouseState mousestate = Mouse.GetState();

            float gametime = (float)gameTime.ElapsedGameTime.TotalSeconds;



            if (actionTimer > 0) actionTimer -= gametime;
            else
            {
                //cooldown
                punchLeft = punchRight = false;
            }

            //blocking
            isBlocking = keyboardstate.IsKeyDown(Keys.Space);

            //punching
            if (actionTimer <= 0)
            {
                if (mousestate.LeftButton == ButtonState.Pressed)
                {
                    punchLeft = true;
                    actionTimer = actionCooldown;
                }
                else if (mousestate.RightButton == ButtonState.Pressed)
                {
                    punchRight = true;
                    actionTimer = actionCooldown;
                }
            }


            //scale
            if (keyboardstate.IsKeyDown(Keys.W)) scaleZoom += .01f;
            else if (keyboardstate.IsKeyDown(Keys.S)) scaleZoom -= .01f;

            //fake movement
            if (keyboardstate.IsKeyDown(Keys.A)) offset.X += 10;
            else if (keyboardstate.IsKeyDown(Keys.D)) offset.X -= 10;

            scaleZoom = Math.Clamp(scaleZoom, 1.2f, 2f);

            //turn based on mouse pos
            offset += (windowSize/2 - new Vector2(mousestate.X, mousestate.Y))/10;
            Mouse.SetPosition((int)windowSize.X/2, (int)windowSize.Y/2);

            //find screensize width and height
            drawScale = windowSize.Y / (float)ringBg.Height * scaleZoom;
            drawnWidth = ringBg.Width * drawScale;
            drawnHeight = ringBg.Height * drawScale;

            //give extra so dont cut off
            Vector2 maxOffset = new Vector2((drawnWidth - windowSize.X) / 2f, (drawnHeight - windowSize.Y) / 2f);
            offset = Vector2.Clamp(offset,-maxOffset, maxOffset);








            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();






            //draw bg
            _spriteBatch.Draw(ringBg, windowSize / 2 + offset, null, Color.White, 0, new(ringBg.Width/2, ringBg.Height/2), windowSize.Y/ringBg.Height * scaleZoom, SpriteEffects.None, 0f);




            //fist
            Color fistColor = Color.White;
            if (isBlocking) fistColor = Color.CornflowerBlue;
            else if (punchLeft || punchRight) fistColor = Color.Red;


            _spriteBatch.Draw(fistTexture, fistPosition, fistColor);

            //crosshair 
            _spriteBatch.Draw(crosshairTexture, new Vector2(windowSize.X / 2, windowSize.Y / 2), null, Color.White, 0f, new Vector2(crosshairTexture.Width / 2, crosshairTexture.Height / 2), 0.2f, SpriteEffects.None, 0f);






            _spriteBatch.End();


            base.Draw(gameTime);
        }


















        //arm wrestling minigame logic
        private void armWrestlingLogic()
        {
            //every second add +/- to the ai base stats
            if (aiPushForceTimer >= aiPushInterval)
            {
                aiPushForceADD = rnd.Next(-1, 2) * 0.001f;
                aiPushForce = Math.Clamp(aiPushForce + aiPushForceADD, 0.01f, 0.05f);
                aiPushForceTimer = 0f;
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

            //taunt the ai to break it, make it weaker by pressing e
            if (keyboardState.IsKeyDown(Keys.E) && prevKeyboardState.IsKeyUp(Keys.E))
            {
                aiPushForce = Math.Clamp(aiPushForce - 0.001f, 0.01f, 0.05f);

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
                    armRotation += ((returnSpeed) / 2);
                }
            }
        }

        //arm wrestling minigame draw
        private void armWrestlingDraw()
        {
            _spriteBatch.Draw(backgroundTexture, new Rectangle(0, 0, 1280, 720), Color.White);

            //draw arm and spin/roate it based on the armRotation variable
            _spriteBatch.Draw(tempArm, new Vector2(700, 730), null, Color.White, armRotation, new Vector2(455, 505), 1.0f, SpriteEffects.None, 0f);
        }








        //drinking game
        private void drinkingGameLogic()
        {

            //keyboard press frame
            int keyPressedThisFrame = 0;

            //run once
            if (comboOne == 0)
            {
                comboOne = rnd.Next(1, 5);
                comboTwo = rnd.Next(1, 5);
                comboThree = rnd.Next(1, 5);

                //reset 
                inputOne = 0;
                inputTwo = 0;
                inputThree = 0;

                inputOneComplete = false;
                inputTwoComplete = false;
                inputThreeComplete = false;
                drinkingGameComplete = false;
            }

            //check for a single key press this frame
            if (keyboardState.IsKeyDown(Keys.Up) && prevKeyboardState.IsKeyUp(Keys.Up)) keyPressedThisFrame = 1;
            else if (keyboardState.IsKeyDown(Keys.Down) && prevKeyboardState.IsKeyUp(Keys.Down)) keyPressedThisFrame = 2;
            else if (keyboardState.IsKeyDown(Keys.Left) && prevKeyboardState.IsKeyUp(Keys.Left)) keyPressedThisFrame = 3;
            else if (keyboardState.IsKeyDown(Keys.Right) && prevKeyboardState.IsKeyUp(Keys.Right)) keyPressedThisFrame = 4;

            //check input if the current combo isn't complete and a key was pressed
            if (!drinkingGameComplete && keyPressedThisFrame != 0)
            {
                if (!inputOneComplete)
                {
                    if (keyPressedThisFrame == comboOne)
                    {
                        inputOne = keyPressedThisFrame;
                        inputOneComplete = true;
                    }
                    //else if wrong first key, player just has to try again...idk what to put yet
                }
                else if (!inputTwoComplete) //first key done wait for the second
                {
                    if (keyPressedThisFrame == comboTwo)
                    {
                        inputTwo = keyPressedThisFrame;
                        inputTwoComplete = true;
                    }
                    else //mistake on the second key
                    {
                        //reset progress for THIS combo
                        inputOneComplete = false;
                        inputOne = 0;

                    }
                }
                else if (!inputThreeComplete) //waiting for the third
                {
                    if (keyPressedThisFrame == comboThree)
                    {
                        inputThree = keyPressedThisFrame;
                        inputThreeComplete = true;
                        drinkingGameComplete = true;
                    }
                    else //mistake on the third key
                    {
                        //reset progress for THIS combo
                        inputOneComplete = false;
                        inputTwoComplete = false;
                        inputOne = 0;
                        inputTwo = 0;

                    }
                }
            }

            //display game status on the window title
            string displayMessage;
            string targetComboStr = $"{KeyToArrow(comboOne)} {KeyToArrow(comboTwo)} {KeyToArrow(comboThree)}";
            string playerAttemptStr = "";


            //just spacing for looks
            if (inputOneComplete) playerAttemptStr += KeyToArrow(inputOne); else playerAttemptStr += "_";
            playerAttemptStr += " ";
            if (inputTwoComplete) playerAttemptStr += KeyToArrow(inputTwo); else playerAttemptStr += "_";
            playerAttemptStr += " ";
            if (inputThreeComplete) playerAttemptStr += KeyToArrow(inputThree); else playerAttemptStr += "_";


            //title for now will remove later
            if (drinkingGameComplete)
            {
                displayMessage = $"COMBO! {playerAttemptStr} - Press 'N' for next.";
                if (keyboardState.IsKeyDown(Keys.N) && prevKeyboardState.IsKeyUp(Keys.N)) //new combo generation and reset everything
                {
                    comboOne = 0;
                }
            }
            else
            {
                displayMessage = $"Target: {targetComboStr} | You: {playerAttemptStr}";
            }
            Window.Title = displayMessage;
        }

        //arrow key conversion for UI for now will switch later
        public string KeyToArrow(int keyVal)
        {
            //converts number to arrow string for display
            switch (keyVal)
            {
                case 0: return "_"; //placeholder for not-yet-entered keys
                case 1: return "↑";
                case 2: return "↓";
                case 3: return "←";
                case 4: return "→";
                default: return "?"; //shouldn't happen
            }
        }













        //shooting game
        private void shootingLogic()
        {
        }

        //shooting game draw
        private void shootingDraw()
        {
        }











        //boxing logic
        private void boxingLogic()
        {

        }


        //boxing draw
        private void boxingDraw()
        {

        }
    }
}