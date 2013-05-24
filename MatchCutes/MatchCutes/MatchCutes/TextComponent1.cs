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

        private Rectangle _restartButton = new Rectangle(740, 850, 200, 50);
        private Rectangle _quitButton = new Rectangle(740, 900, 200, 50);

        InputComponent _input;

        public TextComponent(Game game)
            : base(game)
        {

        }

        public override void Initialize()
        {
            _input = (InputComponent)Game.Services.GetService(typeof(InputComponent));
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
            if(_input.mouseClick())
            {
                if(_restartButton.Contains(_input.MousePosition()))
                {
                    ((MatchCutesGame)Game).restart();
                }
                else if (_quitButton.Contains(_input.MousePosition()))
                {
                    Game.Exit();
                }
            }


            base.Update(gameTime);
        }
        public override void  Draw(GameTime gameTime)
        {
 	         base.Draw(gameTime); 

            spriteBatch.Begin();
            if (_ScoreServ.gameOver)
            {
                base.GraphicsDevice.Clear(Color.Black);
                spriteBatch.DrawString(_font, "GAAAAAAAAAME OVEEEEEER", new Vector2(300, 500), Color.Pink);
            }

            spriteBatch.DrawString(_font, "Your score: " + _ScoreServ.Score, new Vector2(740, 40), Color.DeepPink);

            //flyttet restart button lengre ned fordi jeg syns det så bedre ut
            spriteBatch.DrawString(_font, "RESTART GAME", new Vector2(740, 850), (_restartButton.Contains(_input.MousePosition())) ? Color.White : Color.DeepPink);
            spriteBatch.DrawString(_font, "QUIT GAME", new Vector2(740, 900), (_quitButton.Contains(_input.MousePosition())) ? Color.White : Color.DeepPink);


            spriteBatch.End();
        }
    }
}
