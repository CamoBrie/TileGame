using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TileGame.GameObjects;
using TileGame.Input;

namespace TileGame.Controllers
{
    abstract class Controller
    {
        /// <summary>
        /// The view managed by this controller.
        /// </summary>
        internal GameObject view;

        /// <summary>
        /// The constructor of BaseController.
        /// This constructor sets the view that the controller is controlling and then initializes this view and all the events.
        /// </summary>
        /// <param name="view">The view managed by this controller.</param>
        internal Controller(GameObject view)
        {
            this.view = view;
            this.InitializeViewAndEvents();
        }

        /// <summary>
        /// Abstract method that initializes all the event handlers.
        /// This method sets initializes all events of its own view, but also possibly of their children!
        /// E.g: the screen controller can set the events of the screen itself, but also the events on the buttons on the screen.
        /// This is because buttons are so small, that they do not need their own controller.
        /// </summary>
        protected abstract void InitializeViewAndEvents();

        /// <summary>
        /// Update method for the game.
        /// It always first collects the user input, and then gives it to the view.
        /// Note that several controllers could be asking for input, and propagating this input at the same time. This is why the handleInput function in GameObject checks if it hasn't already seen this input.
        /// </summary>
        /// <param name="time">The current time in the game. </param>
        internal virtual void Update(GameTime time)
        {
            this.HandleInput();
            this.view.Update(time);
            
        }

        /// <summary>
        /// The method which draws the view onto a spritebatch.
        /// </summary>
        /// <param name="batch">The spritebatch in which the view is drawn.</param>
        internal virtual void Draw(SpriteBatch batch)
        {
            this.view.Draw(batch);
        }

        /// <summary>
        /// Responds to all touches and keyinputs pertaining to this game state.
        /// </summary>
        internal virtual void HandleInput()
        {
            this.view.HandleInput();
        }
    }
}
