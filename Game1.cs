using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
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

        Vector2 windowSize = new(1280, 720);

        //screens
        enum Screen
        {
            Menu,
            drinkingGame,
            armWrestling,
            boxing,

            winscreen,
            losescreen

        };

        Texture2D menuTexture;
        SoundEffect menuVoice;
        SoundEffectInstance menuVoiceInstance;
        bool menuVoicePlayed = false;

        //sound played

        SoundEffect armWrestlingVoice;
        SoundEffect boxingVoice;
        SoundEffect drinkingGameVoice;
        SoundEffect winscreenVoice;
        SoundEffect losescreenVoice;
        SoundEffect againScreenVoice;

        bool armWrestlingVoicePlayed = false;
        bool boxingVoicePlayed = false;
        bool drinkingGameVoicePlayed = false;
        bool winscreenVoicePlayed = false;
        bool losescreenVoicePlayed = false;
        bool againScreenVoicePlayed = false;

        SoundEffectInstance armWrestlingVoiceInstance, boxingVoiceInstance, drinkingGameVoiceInstance, winscreenVoiceInstance, losescreenVoiceInstance, againScreenVoiceInstance;





        Screen currentScreen;

        //random number generator
        Random rnd = new Random();

        #region drinking game

        //textures
        Texture2D bgDrinkTexture, bottleTexture;

        //variables
        float bottleRotation = 0f;
        int pointingWin = 0;







        #endregion








        #region arm wrestling
        //import texture
        Texture2D backgroundTexture, tempArm, backgroundTableTexture, enemyArmW;
        //game variables
        float armRotation = 0.1f; //so the cool animation play and get it shaky
        float aiPushForce = 2f; //ai strength

        KeyboardState keyboardState, prevKeyboardState;

        const float minRotation = -0.39f; //left
        const float maxRotation = 1.5f;  //right
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


        //smoothening
        float targetRotation = 0.4f;
        const float rotationLerpSpeed = 5f;

        //ai help mechanics
        bool eUsed = false;
        bool eActive = false;
        float eTimer = 0f;
        const float eDuration = 2f;
        const float eAmount = 0.2f;
        //set to 2 sec for now


        #endregion
        #region shooting game
        //shootingDuel variables

        //bools
        bool shootingDuelComplete = false;

        //movement

        //textures
        Texture2D gunTexture, bgTextureShoot, stanceDuelTexture;


        #endregion
        #region boxing game

        //textures
        Texture2D ringBg;
        Texture2D crosshairTexture;

        Texture2D fistGuardTexture;
        Texture2D fistPunchTexture;
        Texture2D fistBlockTexture;


        //variables
        Vector2 fistPosition;
        bool isBlocking;
        bool punchLeft;
        float actionCooldown = 0.3f;
        float actionTimer = 0f;

        float scaleZoom = 0;
        //Vector2 offset;
        Vector2 offset = new Vector2(0, 0);

        float drawScale;
        float drawnWidth;
        float drawnHeight;

        //enemy
        Texture2D enemyGuardTexture;
        Texture2D enemyPunchTexture;
        Texture2D enemyBlockTexture;
        Texture2D enemyHurtTexture;

        Vector2 enemyPosition;
        float enemyActionTimer = 0f;
        const float enemyActionCooldown = 1.5f;

        int enemyHealth = 3;
        int playerHealth = 3;

        bool enemyPunch, enemyBlock, enemyHurt;
        float enemyYOffset = 190f;
        float enemyXOffset = 0f;

        bool enemyAIActive = false;

        const float enemyXStep = 100f;
        float enemyScaleOffset = 1f;
        const float enemyScaleStep = 0.05f;
        const float minEnemyScaleOff = 0.8f;
        const float maxEnemyScaleOff = 1.2f;


        //combat
        const float punchRangeScale = 1.6f;
        const float punchWindupTime = 0.5f;
        const float punchHoldTime = 0.5f;
        float punchTimer = 0f;
        bool enemyWindingUp = false;
        bool enemyPunching = false;
        float hurtTimer = 0f;
        const float hurtTime = 0.3f;
        const int maxEnemyHealth = 10;


        //for guard cooldown before/after punch
        const float postPunchGuardTime = 1f;
        float guardCooldownTimer = 0f;

        //health and stamina
        const int maxPlayerStamina = 2;
        int playerStamina = maxPlayerStamina;
        Texture2D heartTexture;
        Texture2D gloveTexture;
        const float iconScale = 0.15f;
        float staminaRechargeTimer = 0f;
        const float staminaRechargeDelay = 1f;
        float mouseHitCooldown = 0f;
        const float mouseHitCooldownDuration = 0.5f;





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


            //boxing
            fistPosition = new Vector2(720 / 2, 350);

            //combat - boxing
            enemyPosition = new Vector2(windowSize.X / 2 + 200, windowSize.Y / 2);
            enemyActionTimer = enemyActionCooldown;
            enemyHealth = maxEnemyHealth;

            //current screen
            currentScreen = 0;


            base.Initialize();
        }





















        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);


            //armwrestling
            backgroundTexture = Content.Load<Texture2D>("Arm/Background");
            tempArm = Content.Load<Texture2D>("Arm/Arm");
            backgroundTableTexture = Content.Load<Texture2D>("Arm/BackgroundTable");
            enemyArmW = Content.Load<Texture2D>("Arm/enemyArmW");

            //drinking game
            bgDrinkTexture= Content.Load<Texture2D>("Drinking/bg");
            bottleTexture = Content.Load<Texture2D>("Drinking/bottle"); 







            //boxing
            ringBg = Content.Load<Texture2D>("Boxing/boxingbg");
            crosshairTexture = Content.Load<Texture2D>("Boxing/crosshair");

            fistGuardTexture = Content.Load<Texture2D>("Boxing/guard");
            fistPunchTexture = Content.Load<Texture2D>("Boxing/punch");
            fistBlockTexture = Content.Load<Texture2D>("Boxing/block");

            enemyGuardTexture = Content.Load<Texture2D>("Boxing/eguard");
            enemyPunchTexture = Content.Load<Texture2D>("Boxing/epunch");
            enemyBlockTexture = Content.Load<Texture2D>("Boxing/eblock");
            enemyHurtTexture = Content.Load<Texture2D>("Boxing/ehurt");

            heartTexture = Content.Load<Texture2D>("Boxing/heart");
            gloveTexture = Content.Load<Texture2D>("Boxing/stam");




            //screens and sounds
            menuTexture = Content.Load<Texture2D>("screen/menu");
            menuVoice = Content.Load<SoundEffect>("sound/intro");
            menuVoiceInstance = menuVoice.CreateInstance();

            armWrestlingVoice = Content.Load<SoundEffect>("sound/armwrestle");
            boxingVoice = Content.Load<SoundEffect>("sound/boxing");


            drinkingGameVoice = Content.Load<SoundEffect>("sound/drinking");


            winscreenVoice = Content.Load<SoundEffect>("sound/win");
            losescreenVoice = Content.Load<SoundEffect>("sound/lose");
            againScreenVoice = Content.Load<SoundEffect>("sound/again");

            armWrestlingVoiceInstance = armWrestlingVoice.CreateInstance();
            boxingVoiceInstance = boxingVoice.CreateInstance();


            drinkingGameVoiceInstance = drinkingGameVoice.CreateInstance();
 


            winscreenVoiceInstance = winscreenVoice.CreateInstance();
            losescreenVoiceInstance = losescreenVoice.CreateInstance();
            againScreenVoiceInstance = againScreenVoice.CreateInstance();





            // TODO: use this.Content to load your game content here
        }

























        protected override void Update(GameTime gameTime)
        {
            float gametime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            prevKeyboardState = keyboardState;
            keyboardState = Keyboard.GetState();

            if (currentScreen == 0)
            {
                //if esc is pressed
                if (keyboardState.IsKeyDown(Keys.Escape) && prevKeyboardState.IsKeyUp(Keys.Escape))
                {
                    Exit();
                }

            }
            else if (currentScreen != 0)
            {
                if (keyboardState.IsKeyDown(Keys.Escape) && prevKeyboardState.IsKeyUp(Keys.Escape))
                {
                    currentScreen = 0;
                    againScreenVoiceInstance.Play();

                }

            }


            //play once
            if (!menuVoicePlayed && currentScreen == 0)
            {
                menuVoiceInstance.Play();
                menuVoicePlayed = true;
            }
            if (currentScreen != Screen.Menu && menuVoiceInstance.State == SoundState.Playing)
            {
                menuVoiceInstance.Stop();

            }

            //stop playing again sound if not in menu
            if (currentScreen != Screen.Menu && againScreenVoiceInstance.State == SoundState.Playing)
            {
                againScreenVoiceInstance.Stop();
            }

            //stop all other sound
            if (currentScreen == Screen.Menu)
            {
                armWrestlingVoiceInstance.Stop();
                boxingVoiceInstance.Stop();
                drinkingGameVoiceInstance.Stop();
                winscreenVoiceInstance.Stop();
                losescreenVoiceInstance.Stop();
            }





            //if current screen is menu
            if (currentScreen == 0)
            {

                //if 1 is pressed
                if (keyboardState.IsKeyDown(Keys.D1) && prevKeyboardState.IsKeyUp(Keys.D1))
                {
                    currentScreen = Screen.drinkingGame;

                }

                //if 2 is pressed
                if (keyboardState.IsKeyDown(Keys.D2) && prevKeyboardState.IsKeyUp(Keys.D2))
                {
                    currentScreen = Screen.armWrestling;

                }

                //if 3 is pressed
                if (keyboardState.IsKeyDown(Keys.D3) && prevKeyboardState.IsKeyUp(Keys.D3))
                {
                    currentScreen = Screen.boxing;

                }

            }

            //if current screen is arm wrestling
            if (currentScreen == Screen.armWrestling)
            {
                //play once
                if (!armWrestlingVoicePlayed)
                {
                    armWrestlingVoiceInstance.Play();
                    armWrestlingVoicePlayed = true;
                }
                //stop play if input is detected
                if (keyboardState.IsKeyDown(Keys.Space) && prevKeyboardState.IsKeyUp(Keys.Space))
                {
                    if (armWrestlingVoiceInstance.State == SoundState.Playing)
                        armWrestlingVoiceInstance.Stop();
                }
                //replay if missed
                if (keyboardState.IsKeyDown(Keys.H) && prevKeyboardState.IsKeyUp(Keys.H))
                {
                    armWrestlingVoicePlayed = false;
                }

                armWrestlingLogic(gameTime);
            }

            //if current screen is drinking game
            if (currentScreen == Screen.drinkingGame)
            {
                //play once
                if (!drinkingGameVoicePlayed)
                {
                    drinkingGameVoiceInstance.Play();
                    drinkingGameVoicePlayed = true;
                }
                //stop play if input is detected
                if (keyboardState.IsKeyDown(Keys.Space) && prevKeyboardState.IsKeyUp(Keys.Space))
                {
                    if (drinkingGameVoiceInstance.State == SoundState.Playing)
                        drinkingGameVoiceInstance.Stop();
                }
                //replay if missed
                if (keyboardState.IsKeyDown(Keys.H) && prevKeyboardState.IsKeyUp(Keys.H))
                {
                    drinkingGameVoicePlayed = false;
                }

                drinkingGameLogic(gameTime);
            }

            //if current screen is boxing
            if (currentScreen == Screen.boxing)
            {
                //play once
                if (!boxingVoicePlayed)
                {
                    boxingVoiceInstance.Play();
                    boxingVoicePlayed = true;
                }
                //stop play if input is detected
                if (keyboardState.IsKeyDown(Keys.Space) && prevKeyboardState.IsKeyUp(Keys.Space))
                {
                    if (boxingVoiceInstance.State == SoundState.Playing)
                        boxingVoiceInstance.Stop();
                }
                //replay if missed
                if (keyboardState.IsKeyDown(Keys.H) && prevKeyboardState.IsKeyUp(Keys.H))
                {
                    boxingVoicePlayed = false;
                }

                boxingLogic(gameTime);
            }



            base.Update(gameTime);
        }














        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            _spriteBatch.Begin();

            if (currentScreen == 0)
            {
                _spriteBatch.Draw(menuTexture, new Rectangle(0, 0, 1280, 720), Color.White);
            }

            //if current screen is arm wrestling
            if (currentScreen == Screen.armWrestling)
            {
                armWrestlingDraw();
            }

            //if current screen is drinking game
            if (currentScreen == Screen.drinkingGame)
            {
                drinkingGameDraw();
            }

            //if current screen is boxing
            if (currentScreen == Screen.boxing)
            {
                boxingDraw();
            }


            _spriteBatch.End();


            base.Draw(gameTime);
        }










        //drinking game
        private void drinkingGameLogic(GameTime gameTime)
        {
            //3 point at user
            //6 point at enemy



            //if space is pressed spin bottle
            if (keyboardState.IsKeyDown(Keys.Space) && prevKeyboardState.IsKeyUp(Keys.Space))
            {
                bottleRotation += 6f;
            }

            //if bottle roation not 0 wait
            if (bottleRotation > 0)
            {
                bottleRotation -= 0.03f;
            }

            //windowtitle
            Window.Title = bottleRotation.ToString();


        }


        //drinking game draw
        private void drinkingGameDraw()
        {
            //draw bg
            _spriteBatch.Draw(bgDrinkTexture, new Rectangle(0, 0, 1280, 720), Color.White);

            //draw bottle in the center
            _spriteBatch.Draw(bottleTexture, new Vector2(1280 / 2, 720 / 2), null, Color.White, bottleRotation, new Vector2(130, 290), 1.0f, SpriteEffects.None, 0f);
        }










       




























        #region arm wrestling

        //arm wrestling minigame logic
        private void armWrestlingLogic(GameTime gameTime)
        {
            float gametime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            aiPushForceTimer += gametime;

            //ai settings
            const float aiPushInterval = 0.2f;

            if (eActive)
            {
                eTimer -= gametime;
                if (eTimer <= 0f)
                {
                    eActive = false;
                }
                else
                {
                    aiPushForce = 0.6f;
                }
            }

            //every second add +/- to the ai base stats
            if (aiPushForceTimer >= aiPushInterval)
            {
                aiPushForceADD = rnd.Next(-1, 2) * 1.2f;
                aiPushForce = Math.Clamp(aiPushForce + aiPushForceADD, 0.6f, 0.9f);
                aiPushForceTimer = 0f;
            }

            //reset - rematch
            if (keyboardState.IsKeyDown(Keys.R) && prevKeyboardState.IsKeyUp(Keys.R))
            {
                armRotation = targetRotation = 0.4f;
                isFrozen = false;
                aiActivated = false;
                gameWinStatus = 0;
                //set new match
                aiPushForce = rnd.Next(2, 7) * 0.01f;
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
                targetRotation -= moveStep;
                aiActivated = true;
            }
            //start only when the player is ready
            if (aiActivated)
            {
                targetRotation += aiPushForce * gametime;
            }

            //prevent arm from exceeding min/max rotation
            if (targetRotation <= minRotation)
            {
                isFrozen = true;
                gameWinStatus = 1;
            }
            else if (targetRotation >= maxRotation)
            {
                isFrozen = true;
                gameWinStatus = 2;
            }



            //taunt the ai to break it, make it weaker by pressing e
            if (!eUsed && keyboardState.IsKeyDown(Keys.E) && prevKeyboardState.IsKeyUp(Keys.E))
            {
                //2 sec start
                eUsed = true;
                eActive = true;
                eTimer = eDuration;

                //set to weaker ai
                aiPushForce = 0.6f;
            }

            //smooth rotation
            armRotation = MathHelper.Lerp(armRotation, targetRotation, rotationLerpSpeed * gametime);
            targetRotation = MathHelper.Clamp(targetRotation, minRotation, maxRotation);

        }

        //arm wrestling minigame draw
        private void armWrestlingDraw()
        {
            //draw bg
            _spriteBatch.Draw(backgroundTexture, new Rectangle(0, 0, 1280, 720), Color.White);

            //enemy arm
            float enemyArmRotationOffset = -0.1f; //slight offset
            float enemyArmRotation = MathHelper.Clamp(armRotation * 0.4f + enemyArmRotationOffset, minRotation, maxRotation); //weaker effect

            float enemyXOffset = 0f;
            if (armRotation > 0.4f)
            {
                float shiftAmount = 450f;
                float t = (armRotation - 0.4f) / (maxRotation - 0.4f);
                enemyXOffset = MathHelper.Clamp(t * shiftAmount, 0f, shiftAmount);
            }

            _spriteBatch.Draw(enemyArmW, new Vector2(650 + enemyXOffset, 580), null, Color.White, enemyArmRotation, new Vector2(270, 480), 1.0f, SpriteEffects.FlipHorizontally, 0f);

            // draw table on top of background/enemy arm
            _spriteBatch.Draw(backgroundTableTexture, new Rectangle(0, 0, 1280, 720), Color.White);

            //draw arm and spin/roate it based on the armRotation variable
            _spriteBatch.Draw(tempArm, new Vector2(700, 750), null, Color.White, armRotation, new Vector2(300, 490), 1.0f, SpriteEffects.None, 0f);
        }
#endregion


        #region boxing
        //boxing logic
        private void boxingLogic(GameTime gameTime)
        {
            float gametime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            //keyboard, mouse, time
            KeyboardState keyboardstate = Keyboard.GetState();
            MouseState mousestate = Mouse.GetState();
            MouseState prevMouseState;
            prevMouseState = _mouseState;
            _mouseState = Mouse.GetState();
            var keyboardState = Keyboard.GetState();

            bool justPressed = _mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released;

            if (hurtTimer > 0f)
            {
                hurtTimer -= gametime;
                if (hurtTimer <= 0f)
                    enemyHurt = false;
            }
            if (mouseHitCooldown > 0f)
                mouseHitCooldown -= (float)gameTime.ElapsedGameTime.TotalSeconds;


            //blocking
            isBlocking = keyboardstate.IsKeyDown(Keys.Space) || mousestate.RightButton == ButtonState.Pressed;

            //punching
            if (actionTimer <= 0 && mousestate.LeftButton == ButtonState.Pressed && playerStamina > 0)
            {
                punchLeft = true;
                actionTimer = actionCooldown;
                //spend stamina
                playerStamina--;
                staminaRechargeTimer = 0f;
            }

            //recharge when blocking
            if (isBlocking && playerStamina < maxPlayerStamina)
            {
                staminaRechargeTimer += gametime;
                if (staminaRechargeTimer >= staminaRechargeDelay)
                {
                    playerStamina++;
                    staminaRechargeTimer = 0f;
                }
            }
            else if (!isBlocking)
            {
                staminaRechargeTimer = 0f;
            }

            //countering
            if (!enemyWindingUp && !enemyPunching && !enemyHurt)
            {

                //crosshair need to be on enemy
                var enemyCenter = windowSize / 2 + offset + new Vector2(enemyXOffset * drawScale, enemyYOffset * drawScale);
                var tex = enemyPunchTexture;
                var bounds = new Rectangle((int)(enemyCenter.X - tex.Width / 2 * drawScale), (int)(enemyCenter.Y - tex.Height / 2 * drawScale), (int)(tex.Width * drawScale), (int)(tex.Height * drawScale));

                if (justPressed && bounds.Contains(_mouseState.X, _mouseState.Y) && playerStamina > 0 && rnd.NextDouble() < 0.7 && scaleZoom >= punchRangeScale)
                {
                    enemyHurt = true;
                    hurtTimer = hurtTime;
                    enemyHealth = Math.Max(0, enemyHealth - 1);
                    guardCooldownTimer = postPunchGuardTime;
                    playerStamina--;
                    staminaRechargeTimer = 0f;
                    mouseHitCooldown = mouseHitCooldownDuration;
                }


                else enemyHurt = false;
            }

            bool inRange = scaleZoom >= punchRangeScale;

            //if scale is greater than 1.5, then enemy AI is active
            if (scaleZoom >= 1.5f)
            {
                enemyAIActive = true;
            }

            //if enemy AI is active
            if (enemyAIActive)
            {

                if (enemyActionTimer <= 0f)
                {
                    //fake left and right
                    if (rnd.Next(0, 2) == 0)
                    {
                        enemyXOffset += enemyXStep;
                    }
                    else
                    {
                        enemyXOffset -= enemyXStep;
                    }

                    //fake forward and back
                    if (rnd.Next(0, 2) == 0)
                    {
                        enemyScaleOffset += enemyScaleStep;
                    }
                    else
                    {
                        enemyScaleOffset -= enemyScaleStep;
                    }
                    enemyScaleOffset = MathHelper.Clamp(enemyScaleOffset, minEnemyScaleOff, maxEnemyScaleOff);

                    //enemy clamp
                    float maxX = (drawnWidth - windowSize.X) / 2f / drawScale;
                    if (enemyXOffset > maxX) enemyXOffset = maxX;
                    if (enemyXOffset < -maxX) enemyXOffset = -maxX;

                    //reset timer
                    enemyActionTimer = enemyActionCooldown;

                }
                else
                {
                    enemyActionTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                }



                //ai combat logic


                bool enemyInRange = scaleZoom >= punchRangeScale;

                if (guardCooldownTimer > 0f)
                {
                    guardCooldownTimer -= gametime;
                    enemyBlock = true;
                    enemyWindingUp = enemyPunching = false;
                }
                else if (!inRange)
                {
                    //block if out of range
                    enemyBlock = true;
                    enemyWindingUp = enemyPunching = false;
                }
                else
                {
                    //a bit of warmup for user
                    if (!enemyWindingUp && !enemyPunching)
                    {
                        enemyBlock = false;
                        enemyWindingUp = true;
                        punchTimer = punchWindupTime;
                    }
                    else if (enemyWindingUp)
                    {
                        punchTimer -= gametime;
                        if (punchTimer <= 0f)
                        {
                            enemyWindingUp = false;
                            enemyPunching = true;
                            punchTimer = punchHoldTime;

                            //apply player damage
                            if (!isBlocking) playerHealth--;
                        }
                    }

                    else if (enemyPunching)
                    {
                        punchTimer -= gametime;

                        if (punchTimer <= 0f)
                        {
                            enemyPunching = false;
                            //after punch go into cooldown
                            guardCooldownTimer = postPunchGuardTime;
                        }
                    }
                }

                // reset punch when cooldown expires
                if (actionTimer > 0) actionTimer -= gametime;
                else punchLeft = false;

            }


            // reset punch
            if (actionTimer > 0) actionTimer -= gametime;
            else punchLeft = false;


            //scale
            if (keyboardstate.IsKeyDown(Keys.W)) scaleZoom += .01f;
            else if (keyboardstate.IsKeyDown(Keys.S)) scaleZoom -= .01f;

            //fake movement
            if (keyboardstate.IsKeyDown(Keys.A)) offset.X += 10;
            else if (keyboardstate.IsKeyDown(Keys.D)) offset.X -= 10;

            //clamp
            scaleZoom = Math.Clamp(scaleZoom, 1.4f, 2.4f);

            //turn based on mouse pos
            offset += (windowSize / 2 - new Vector2(mousestate.X, mousestate.Y)) / 10;
            Mouse.SetPosition((int)windowSize.X / 2, (int)windowSize.Y / 2);

            //find screensize width and height
            drawScale = windowSize.Y / (float)ringBg.Height * scaleZoom;
            drawnWidth = ringBg.Width * drawScale;
            drawnHeight = ringBg.Height * drawScale;

            //give extra so dont cut off
            Vector2 maxOffset = new Vector2((drawnWidth - windowSize.X) / 2f, (drawnHeight - windowSize.Y) / 2f);
            offset = Vector2.Clamp(offset, -maxOffset, maxOffset);

        }


        //boxing draw
        private void boxingDraw()
        {

            //draw bg
            _spriteBatch.Draw(ringBg, windowSize / 2 + offset, null, Color.White, 0, new(ringBg.Width / 2, ringBg.Height / 2), windowSize.Y / ringBg.Height * scaleZoom, SpriteEffects.None, 0f);



            //enemy
            Texture2D enemyToDraw;

            if (enemyWindingUp)
                enemyToDraw = enemyGuardTexture;
            else if (enemyPunching)
                enemyToDraw = enemyPunchTexture;
            else if (enemyHurt)
                enemyToDraw = enemyHurtTexture;
            else
                enemyToDraw = enemyBlockTexture;



            //scaling
            float baseScale = windowSize.Y / (float)ringBg.Height * scaleZoom;
            float enemyDrawScale = baseScale * enemyScaleOffset;

            Vector2 enemyDrawPos = windowSize / 2 + offset + new Vector2(enemyXOffset * drawScale, enemyYOffset * drawScale);
            float redIntensity = 1f - (float)enemyHealth / maxEnemyHealth;
            Color tint = Color.Lerp(Color.White, Color.Red, redIntensity);


            _spriteBatch.Draw(enemyToDraw, enemyDrawPos, null, tint, 0f, new Vector2(enemyToDraw.Width / 2f, enemyToDraw.Height / 2f), enemyDrawScale, SpriteEffects.None, 0f);



            //fist
            Texture2D fistToDraw;
            if (punchLeft)
                fistToDraw = fistPunchTexture;
            else if (isBlocking)
                fistToDraw = fistBlockTexture;
            else
                fistToDraw = fistGuardTexture;



            _spriteBatch.Draw(fistToDraw, fistPosition, Color.White);


            //crosshair 
            _spriteBatch.Draw(crosshairTexture, new Vector2(windowSize.X / 2, windowSize.Y / 2), null, Color.White, 0f, new Vector2(crosshairTexture.Width / 2, crosshairTexture.Height / 2), 0.2f, SpriteEffects.None, 0f);

            const int iconSpacing = 4;

            // draw hearts
            for (int i = 0; i < playerHealth; i++)
            {
                var pos = new Vector2(20 + i * ((heartTexture.Width * iconScale) + iconSpacing), 20
                );
                _spriteBatch.Draw(heartTexture, pos, null, Color.White, 0f, Vector2.Zero, iconScale, SpriteEffects.None, 0f
                );
            }

            //draw gloves(stamina)
            for (int i = 0; i < playerStamina; i++)
            {
                var pos = new Vector2(20 + i * ((gloveTexture.Width * iconScale) + iconSpacing), 20 + heartTexture.Height * iconScale + iconSpacing
                );
                _spriteBatch.Draw(gloveTexture, pos, null, Color.White, 0f, Vector2.Zero, iconScale, SpriteEffects.None, 0f);
            }





        }
        #endregion

    }
}