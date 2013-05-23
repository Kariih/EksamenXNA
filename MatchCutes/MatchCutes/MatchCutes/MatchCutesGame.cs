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

    public class MatchCutesGame : Microsoft.Xna.Framework.Game
    {

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        BordComponent bordComponent;
        TextComponent textComponent;


        public MatchCutesGame()
        {
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 1024;
            graphics.PreferredBackBufferWidth = 1024;
            graphics.ApplyChanges();

        }

        protected override void Initialize()
        {
            InputComponent input = new InputComponent(this);
            Components.Add(input);
            Services.AddService(typeof(InputComponent), input);

            bordComponent = new BordComponent(this);
            textComponent = new TextComponent(this);

            this.Components.Add(bordComponent);
            this.Components.Add(textComponent);

            spriteBatch = new SpriteBatch(GraphicsDevice);
            ScoreService Scores = new ScoreService();
            Services.AddService(typeof(ScoreService), Scores);

            base.Initialize();

        }
        public void restart()
        {
            Components.Remove(bordComponent);
            bordComponent = new BordComponent(this);
            Components.Add(bordComponent);
        
        }
        protected override void Update(GameTime gameTime)
        {
            if(Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Beige);
            base.Draw(gameTime);
        }
    }
}
