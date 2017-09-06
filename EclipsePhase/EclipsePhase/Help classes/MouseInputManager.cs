using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EclipsePhase
{
    static class MouseInputManager
    {
        //Stores the mouse state
        private static MouseState previousMouseState;
        private static MouseState currentMouseState;

        public static MouseState PreviousMouseState { get { return previousMouseState; } }
        public static MouseState CurrentMouseState { get { return currentMouseState; } }

        /// <summary>
        /// Updates the states so that they contain the right data.
        /// </summary>
        public static void Update()
        {
            previousMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();
        }

        /// <summary>
        /// Returns a small rectangle at the mouse's current location.
        /// </summary>
        /// <returns></returns>
        public static Rectangle GetMouseBounds()
        {
            return new Rectangle(currentMouseState.Position.X, currentMouseState.Position.Y, 10, 10);
        }

        /// <summary>
        /// Returns a small rectangle at the mouse's previous location.
        /// </summary>
        /// <returns></returns>
        public static Rectangle GetMouseBoundsAtPrevLocation()
        {
            return new Rectangle(previousMouseState.X, previousMouseState.Y, 1, 1);
        }

        /// <summary>
        /// Returns a bool true if the given mouse button is released after it was pressed.
        /// </summary>
        /// <param name="btn"></param>
        /// <returns></returns>
        public static bool GetHasMouseButtonBeenReleased(MouseButton btn)
        {
            switch (btn)
            {
                case MouseButton.Left:
                    if (currentMouseState.LeftButton == ButtonState.Released && previousMouseState.LeftButton == ButtonState.Pressed)
                        return true;
                    break;
                case MouseButton.Middle:
                    if (currentMouseState.MiddleButton == ButtonState.Released && previousMouseState.MiddleButton == ButtonState.Pressed)
                        return true;
                    break;
                case MouseButton.Right:
                    if (currentMouseState.RightButton == ButtonState.Released && previousMouseState.RightButton == ButtonState.Pressed)
                        return true;
                    break;
            }
            return false;
        }

        /// <summary>
        /// Returns true if the given mouse button is pressed.
        /// </summary>
        /// <param name="btn"></param>
        /// <returns></returns>
        public static bool GetIsMouseButtonPressed(MouseButton btn)
        {
            switch (btn)
            {
                case MouseButton.Left:
                    if (currentMouseState.LeftButton == ButtonState.Pressed)
                        return true;
                    break;
                case MouseButton.Middle:
                    if (currentMouseState.MiddleButton == ButtonState.Pressed)
                        return true;
                    break;
                case MouseButton.Right:
                    if (currentMouseState.RightButton == ButtonState.Pressed)
                        return true;
                    break;
            }
            return false;
        }

        /// <summary>
        /// Returns true if the given mouse button was pressed.
        /// </summary>
        /// <param name="btn"></param>
        /// <returns></returns>
        public static bool GetWasMouseButtonPressed(MouseButton btn)
        {
            switch (btn)
            {
                case MouseButton.Left:
                    if (previousMouseState.LeftButton == ButtonState.Pressed)
                        return true;
                    break;
                case MouseButton.Middle:
                    if (previousMouseState.MiddleButton == ButtonState.Pressed)
                        return true;
                    break;
                case MouseButton.Right:
                    if (previousMouseState.RightButton == ButtonState.Pressed)
                        return true;
                    break;
            }
            return false;
        }

        /// <summary>
        /// Gets the mouse's position as a Vector.
        /// </summary>
        /// <returns></returns>
        public static Vector2 GetMousePositionVec()
        {
            return new Vector2(currentMouseState.X, currentMouseState.Y);
        }

        /// <summary>
        /// Gets the mouse's position as a Point.
        /// </summary>
        /// <returns></returns>
        public static Point GetMousePositionPoint()
        {
            return currentMouseState.Position;
        }
    }
}
