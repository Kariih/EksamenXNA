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

    public class TextComponent : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private SpriteFont _font;
        private ScoreService _ScoreServ;
        private SpriteBatch spriteBatch;

        public TextComponent(Game game)
            : base(game)
        {

        }

        public override void Initialize()
        {
            _ScoreServ = (ScoreService)Game.Services.GetService(typeof(ScoreService));

            base.Initialize();
        }
        protected override void LoadContent()
        {

            _font = Game.Content.Load<SpriteFont>("GameFont");
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {

            base.Update(gameTime);
        }
        public override void  Draw(GameTime gameTime)
        {
 	         base.Draw(gameTime); 

            spriteBatch.Begin();
            spriteBatch.DrawString(_font, "Your score: " + _ScoreServ.Score, new Vector2(740, 40), Color.DeepPink);

            spriteBatch.DrawString(_font, "RESTART GAME", new Vector2(740, 600), Color.DeepPink);
            spriteBatch.DrawString(_font, "QUIT GAME", new Vector2(740, 900), Color.DeepPink);

            spriteBatch.End();
        }
    }
}
