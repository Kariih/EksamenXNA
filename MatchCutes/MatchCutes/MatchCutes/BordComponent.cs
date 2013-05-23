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

    public class BordComponent : Microsoft.Xna.Framework.DrawableGameComponent
    {

        private int fieldX = 7;
        private int fieldY = 12;

        public GemType[,] Playfield = new GemType[7, 12];
        private Random _randomizer = new Random();

        private Texture2D _blueGem;
        private Texture2D _greenGem;
        private Texture2D _orangeGem;
        private Rectangle _gemSource = new Rectangle(0, 56, 100, 114);

        private bool _blocksAreFalling;

        private List<Point> _blueHits = new List<Point>();
        private List<Point> _greenHits = new List<Point>();
        private List<Point> _orangeHits = new List<Point>();

        Color _fadedColor = new Color(160, 160, 160, 160);
        private Point _selected = new Point(-1, -1);
        private MouseState _curMouseState;
        private MouseState _prevMouseState;

        public BordComponent(Game game)
            : base(game)
        {
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        public override void Initialize()
        {
            base.Initialize();
            _curMouseState = Mouse.GetState();
            _prevMouseState = Mouse.GetState();
            spawnRow();
            moveFallingGems();
            spawnRow();
            moveFallingGems();
            spawnRow();
            moveFallingGems();
            spawnRow();
            moveFallingGems();
            detectClusters();
            Score = 0;
            _remainingMoves = 0;
        }

        private void spawnRow()
        {
            for (int i = 0; i < fieldX; i++)
            {
                GemType gem = ((GemType)_randomizer.Next(1, 4));
                while (i > 1 && (Playfield[i - 1, 0] == gem && Playfield[i - 2, 0] == gem))
                    gem = ((GemType)_randomizer.Next(1, 4));
                Playfield[i, 0] = gem;
            }
            _remainingMoves = _allowedMovesBeforeSpawn;
            _selected.X = _selected.Y = -1;
            _blocksAreFalling = true;
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            _greenGem = Content.Load<Texture2D>("Gem Green");
            _blueGem = Content.Load<Texture2D>("Gem Blue");
            _orangeGem = Content.Load<Texture2D>("Gem Orange");
            _font = Content.Load<SpriteFont>("GameFont");
        }

        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here

            base.Update(gameTime);
        }
    }
}
