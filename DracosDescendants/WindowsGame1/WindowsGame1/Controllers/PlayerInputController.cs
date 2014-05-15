#region File Description
//-----------------------------------------------------------------------------
// RocketInputController.cs
//
// Device-independent input manager.
//
// This class implements all of the input for the rocket lander game.
// It primarily controls the rocket thrust.
//
// Author: Walker M. White
// Based on original PhysicsDemo Lab by Don Holden, 2007
// MonoGame version, 2/14/2014
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
#endregion

/// <summary>
/// Subnamespace for controllers.
/// </summary>
namespace DracosD.Controllers
{

    /// <summary>
    /// Input control to move the rocket
    /// </summary>
    public class PlayerInputController
    {

        #region Fields
        // Fields to manage ship movement
        private float horizontal;		// Horizontal movement
        private float vertical;         // Vertical movement
        protected bool resetPressed; //the reset button
        protected bool moreGravPressed;
        protected bool lessGravPressed;
        protected bool moreRestPressed;
        protected bool lessRestPressed;
        protected bool moreSpeedPressed;
        protected bool lessSpeedPressed;
        protected bool moreDampenPressed;
        protected bool lessDampenPressed;
        protected bool moreDampenThresholdPressed;
        protected bool lessDampenThresholdPressed;

        protected bool keyPressed;

        protected bool breathing;

        protected bool pressedStart;
        protected bool up;
        protected bool upWasDown = false;
        protected bool down;
        protected bool downWasDown = false;
        protected bool right;
        protected bool rightWasDown = false;
        protected bool left;
        protected bool leftWasDown = false; 

        protected bool startWasDown = false;

        #endregion

        #region Properties (READ-ONLY)

        public bool Breathing
        {
            get { return breathing; }
        }
        /// <summary>
        /// The amount of sideways movement. -1 = left, 1 = right, 0 = still
        /// </summary>
        public float Horizontal
        {
            get { return horizontal; }
        }

        /// <summary>
        /// The amount of vertical movement. -1 = up, 1 = down, 0 = still
        /// </summary>
        public float Vertical
        {
            get { return vertical; }
        }

        /// <summary>
        /// Whether the player pressed to lower effect of gravity.
        /// </summary>
        public bool gravUp
        {
            get { return moreGravPressed; }
        }

        /// <summary>
        /// Whether the player pressed to lower effect of gravity.
        /// </summary>
        public bool gravDown
        {
            get { return lessGravPressed; }
        }

        /// <summary>
        /// Whether the player pressed to raise collision restitution.
        /// </summary>
        public bool restUp
        {
            get { return moreRestPressed; }
        }

        /// <summary>
        /// Whether the player pressed to lower collision restitution.
        /// </summary>
        public bool restDown
        {
            get { return lessRestPressed; }
        }

        /// <summary>
        /// Whether the player pressed to move to raise max speed.
        /// </summary>
        public bool speedUp
        {
            get { return moreSpeedPressed; }
        }

        /// <summary>
        /// Whether the player pressed to move to lower max speed.
        /// </summary>
        public bool speedDown
        {
            get { return lessSpeedPressed; }
        }

        /// <summary>
        /// Whether the player pressed to raise speed dampening.
        /// </summary>
        public bool dampUp
        {
            get { return moreDampenPressed; }
        }

        /// <summary>
        /// Whether the player pressed to lower dampening.
        /// </summary>
        public bool dampDown
        {
            get { return lessDampenPressed; }
        }

        /// <summary>
        /// Whether the player pressed to raise speed dampening.
        /// </summary>
        public bool dampThreshUp
        {
            get { return moreDampenThresholdPressed; }
        }

        /// <summary>
        /// Whether the player pressed to lower dampening.
        /// </summary>
        public bool dampThreshDown
        {
            get { return lessDampenThresholdPressed; }
        }

        public bool keyDown
        {
            get { return keyPressed; }
        }

        public bool reset
        {

            get { return resetPressed; }
        }

        public bool start
        {

            get { return pressedStart; }
        }

        public bool Up
        {

            get { return up; }
        }

        public bool Down
        {

            get { return down; }
        }

        public bool Right
        {

            get { return right; }
        }

        public bool Left
        {

            get { return left; }
        }

        #endregion

        #region Methods
        /// <summary>
        /// Creates a new input controller.
        /// </summary>
        public PlayerInputController() { }

        /// <summary>
        /// Reads the input for the player and converts the result into game logic.
        /// </summary>
        public void ReadInput()
        {
            // Check to see if a GamePad is connected
            GamePadState gamePad = GamePad.GetState(PlayerIndex.One);
            if (gamePad.IsConnected)
            {
                ReadGamepadInput();
            }
            else
            {
                ReadKeyboardInput();
            }
        }

        /// <summary>
        /// Reads input from an X-Box controller connected to this computer.
        /// </summary>
        private void ReadGamepadInput()
        {
            GamePadState gamePad = GamePad.GetState(PlayerIndex.One);

            // Increase animation frame, but only if trying to move
            horizontal = gamePad.ThumbSticks.Left.X;
            vertical = -gamePad.ThumbSticks.Left.Y;
            breathing = (gamePad.Buttons.A == ButtonState.Pressed);
            //pressedStart = (gamePad.Buttons.Start == ButtonState.Pressed);

            if (startWasDown && (gamePad.Buttons.Start == ButtonState.Pressed))
            {
                pressedStart = false;
                keyPressed = true;
            }
            else if ((gamePad.Buttons.Start == ButtonState.Pressed))
            {
                startWasDown = true;
                pressedStart = true;
            }
            else
            {
                pressedStart = false;
                startWasDown = false;
            }
            
            // NEED TO ADD LOGIC FOR CONTROLLING MENUS
            if (vertical < 0)
            {
                if (!upWasDown)
                {
                    up = true;
                    upWasDown = true;
                }
                else up = false;
            }
            else
            {
                upWasDown = false;
                up = false;
            }
            if (vertical > 0)
            {
                if (!downWasDown)
                {
                    down = true;
                    downWasDown = true;
                }
                else down = false;
            }
            else
            {
                downWasDown = false;
                down = false;
            }

            //Left/right 
            if (horizontal < 0)
            {
                if (!leftWasDown)
                {
                    left = true;
                    leftWasDown = true;
                }
                else left = false;
            }
            else
            {
                leftWasDown = false;
                left = false;
            }
            if (horizontal > 0)
            {
                if (!rightWasDown)
                {
                    right = true;
                    rightWasDown = true;
                }
                else right = false;
            }
            else
            {
                rightWasDown = false;
                right = false;
            }
            //resetPressed = (gamePad.Buttons.Back == ButtonState.Pressed);
        }

        /// <summary>
        /// Reads input from the keyboard.
        /// </summary>
        private void ReadKeyboardInput()
        {
            KeyboardState keyboard = Keyboard.GetState();
            keyPressed = false;
            horizontal = 0.0f;
            if (keyboard.IsKeyDown(Keys.Right))
            {
                horizontal += 1.0f;
                keyPressed = true;
                if (!rightWasDown)
                {
                    right = true;
                    rightWasDown = true;
                }
                //else if (keyboard.IsKeyUp(Keys.Up)) upWasDown = false;
                else right = false;
            }
            else if (keyboard.IsKeyUp(Keys.Right)) rightWasDown = false;

            if (keyboard.IsKeyDown(Keys.Left))
            {
                horizontal -= 1.0f;
                keyPressed = true;
                if (!leftWasDown)
                {
                    left = true;
                    leftWasDown = true;
                }
                //else if (keyboard.IsKeyUp(Keys.Up)) upWasDown = false;
                else left = false;
            }
            else if (keyboard.IsKeyUp(Keys.Left)) leftWasDown = false;

            vertical = 0.0f;
            if (keyboard.IsKeyDown(Keys.Up))
            {
                vertical -= 1.0f;
                keyPressed = true;
                if (!upWasDown)
                {
                    up = true;
                    upWasDown = true;
                }
                //else if (keyboard.IsKeyUp(Keys.Up)) upWasDown = false;
                else up = false;
            }
            else if (keyboard.IsKeyUp(Keys.Up)) upWasDown = false;

            if (keyboard.IsKeyDown(Keys.Down))
            {
                vertical += 1.0f;
                keyPressed = true;
                if (!downWasDown)
                {
                    down = true;
                    downWasDown = true;
                }
                //else if (keyboard.IsKeyUp(Keys.Down)) downWasDown = false;
                else down = false;
            }
            else if (keyboard.IsKeyUp(Keys.Down)) downWasDown = false;

            breathing = false;
            if (keyboard.IsKeyDown(Keys.Space))
            {
                breathing = true;
            }
            pressedStart = false;
            if (startWasDown && keyboard.IsKeyUp(Keys.Enter))
            {
                pressedStart = true;
                keyPressed = true;
            }
            if (keyboard.IsKeyDown(Keys.Enter))
            {
                startWasDown = true;
            }
            else
            {
                startWasDown = false;
            }


            /* Q/A control restitution
             * W/S control gravity
             * E/D control speed
             * Y/H control strength of dampening factor
             * T/G control the threshold when dampening takes effect */
            moreGravPressed = false;
            lessGravPressed = false;
            if (keyboard.IsKeyDown(Keys.W))
            {
                moreGravPressed=true;
            }
            if (keyboard.IsKeyDown(Keys.S))
            {
                lessGravPressed=true;
            }

            moreRestPressed = false;
            lessRestPressed = false;
            if (keyboard.IsKeyDown(Keys.Q))
            {
                moreRestPressed = true;
            }
            if (keyboard.IsKeyDown(Keys.A))
            {
                lessRestPressed = true;
            }

            moreSpeedPressed = false;
            lessSpeedPressed = false;
            if (keyboard.IsKeyDown(Keys.E))
            {
                moreSpeedPressed = true;
            }
            if (keyboard.IsKeyDown(Keys.D))
            {
                lessSpeedPressed = true;
            }

            moreDampenPressed = false;
            lessDampenPressed = false;
            if (keyboard.IsKeyDown(Keys.Y))
            {
                moreDampenPressed = true;
            }
            if (keyboard.IsKeyDown(Keys.H))
            {
                lessDampenPressed = true;
            }

            moreDampenThresholdPressed = false;
            lessDampenThresholdPressed = false;
            if (keyboard.IsKeyDown(Keys.T))
            {
                moreDampenThresholdPressed = true;
            }
            if (keyboard.IsKeyDown(Keys.G))
            {
                lessDampenThresholdPressed = true;
            }
        }
        #endregion

    }
}
