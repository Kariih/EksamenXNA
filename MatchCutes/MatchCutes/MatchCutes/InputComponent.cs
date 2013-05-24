using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace MatchCutes
{
    //Laget en egen inputComponent for å holde input for mus for seg selv.
    public class InputComponent : Microsoft.Xna.Framework.GameComponent
    {
        public MouseState currentMouseState { get; protected set; }
        public MouseState OldMouseState {get; protected set;}


        public InputComponent(Game game)
            : base(game)
        {

        }

        public override void Initialize()
        {
            currentMouseState = Mouse.GetState();
            OldMouseState = currentMouseState;
            base.Initialize();
        }


        public override void Update(GameTime gameTime)
        {
            OldMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();
            base.Update(gameTime);
        }
        public bool mouseClick() 
        {
            return currentMouseState.LeftButton == ButtonState.Pressed && OldMouseState.LeftButton == ButtonState.Released;
        
        }
        public Point MousePosition()
        {
            return new Point(currentMouseState.X, currentMouseState.Y);
        
        }
    
    }
}
