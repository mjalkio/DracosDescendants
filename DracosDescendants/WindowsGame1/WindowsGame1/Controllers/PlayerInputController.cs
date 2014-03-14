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
/// Subnamespace for the rocket game.
/// </summary>
namespace DracosD.Rocket
{

    /// <summary>
    /// Input control to move the rocket
    /// </summary>
    public class RocketInputController
    {

        #region Fields
        // Fields to manage ship movement
        private float horizontal;		// Horizontal movement
        private float vertical;         // Vertical movement
        #endregion

        #region Properties (READ-ONLY)
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
        #endregion

        #region Methods
        /// <summary>
        /// Creates a new input controller.
        /// </summary>
        public RocketInputController() { }

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
            vertical = gamePad.ThumbSticks.Left.Y;
        }

        /// <summary>
        /// Reads input from the keyboard.
        /// </summary>
        private void ReadKeyboardInput()
        {
            KeyboardState keyboard = Keyboard.GetState();

            horizontal = 0.0f;
            if (keyboard.IsKeyDown(Keys.Right))
            {
                horizontal += 1.0f;
            }
            if (keyboard.IsKeyDown(Keys.Left))
            {
                horizontal -= 1.0f;
            }

            vertical = 0.0f;
            if (keyboard.IsKeyDown(Keys.Up))
            {
                vertical -= 1.0f;
            }
            if (keyboard.IsKeyDown(Keys.Down))
            {
                vertical += 1.0f;
            }
        }
        #endregion

    }
}
