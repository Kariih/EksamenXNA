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
    public enum GemType
    {
        None,
        Blue,
        Green,
        Orange
    }

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
            bordComponent = new BordComponent(this);
            textComponent = new TextComponent(this);


            this.Components.Add(bordComponent);
            this.Components.Add(textComponent);
        }

        protected override void Initialize()
        {

            spriteBatch = new SpriteBatch(GraphicsDevice);
            ScoreService Scores = new ScoreService();
            Services.AddService(typeof(ScoreService), Scores);
            base.Initialize();

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
