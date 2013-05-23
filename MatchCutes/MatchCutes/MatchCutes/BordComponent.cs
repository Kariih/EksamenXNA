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
        Orange,
        Star
    }

    public class BordComponent : Microsoft.Xna.Framework.DrawableGameComponent
    {

        public int Score { get; set; }

        private int fieldX = 7;
        private int fieldY = 12;
        private Rectangle gameFrame;
        private int gridSize = 80;

        public GemType[,] Playfield = new GemType[7, 12];
        private Random _randomizer = new Random();

        private Texture2D _blueGem;
        private Texture2D _greenGem;
        private Texture2D _orangeGem;
        private Texture2D _starGem;
        private Rectangle _gemSource = new Rectangle(0, 56, 100, 114);

        private bool _blocksAreFalling;

        private List<Point> _blueHits = new List<Point>();
        private List<Point> _greenHits = new List<Point>();
        private List<Point> _orangeHits = new List<Point>();
        private List<Point> _starHits = new List<Point>();

        Color _fadedColor = new Color(160, 160, 160, 160);
        private Point _selected = new Point(-1, -1);

        private double _dropDownTimer;
        private const double time_between_block_drops = 0.25f;
        private int _remainingMoves;
        private int _allowedMovesBeforeSpawn = 2;

        private InputComponent _input;

        private ScoreService _ScoreServ;

        SpriteBatch spriteBatch;

        public BordComponent(Game game)
            : base(game)
        {
            gameFrame = new Rectangle(0, 0, fieldX * gridSize, fieldY * gridSize);
        }

        public override void Initialize()
        {
            _input = (InputComponent)Game.Services.GetService(typeof(InputComponent));
            _ScoreServ = (ScoreService)Game.Services.GetService(typeof(ScoreService));
            _ScoreServ.Score = 0;
            _ScoreServ.gameOver = false;

            base.Initialize();

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
                if (Playfield[i, 0] != GemType.None)
                {
                    _ScoreServ.gameOver = true;
                }
                GemType gem = ((GemType)_randomizer.Next(1, 4));
                while (i > 1 && (Playfield[i - 1, 0] == gem && Playfield[i - 2, 0] == gem))
                    gem = ((GemType)_randomizer.Next(1, 4));
               
                
                Playfield[i, 0] =_randomizer.Next(20) == 0 ? GemType.Star : gem;


            }
            _remainingMoves = _allowedMovesBeforeSpawn;
            _selected.X = _selected.Y = -1;
            _blocksAreFalling = true;
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            _greenGem = Game.Content.Load<Texture2D>("Gem Green");
            _blueGem = Game.Content.Load<Texture2D>("Gem Blue");
            _orangeGem = Game.Content.Load<Texture2D>("Gem Orange");
            _starGem = Game.Content.Load<Texture2D>("Star");
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {

            if (!_ScoreServ.gameOver)
            {
                _dropDownTimer += gameTime.ElapsedGameTime.TotalSeconds;

                if (_dropDownTimer >= time_between_block_drops)
                {
                    _dropDownTimer -= time_between_block_drops;
                    moveFallingGems();
                }

                if (_remainingMoves <= 0)
                {
                    spawnRow();
                }

                if (!_blocksAreFalling)
                {
                    if (_input.mouseClick())
                        if (gameFrame.Contains(_input.MousePosition()))
                        {
                            {
                                if (_selected.X == -1 && _selected.Y == -1)
                                {
                                    _selected.X = _input.MousePosition().X / 80;
                                    _selected.Y = _input.MousePosition().Y / 80;
                                    GemType selectedGem = Playfield[_selected.X, _selected.Y];
                                    if (selectedGem == GemType.None)
                                        _selected.X = _selected.Y = -1;
                                }
                                else
                                {
                                    Point newSelection = new Point(_input.MousePosition().X / 80, _input.MousePosition().Y / 80);
                                    GemType selectedGem = Playfield[newSelection.X, newSelection.Y];
                                    if (selectedGem == GemType.None || (_selected.X == newSelection.X) && _selected.Y == newSelection.Y)
                                    {
                                        _selected.X = _selected.Y = -1;
                                    }
                                    else
                                    {
                                        int distance = Math.Abs(_selected.X - newSelection.X) + Math.Abs(_selected.Y - newSelection.Y);
                                        if (distance == 1)
                                        {
                                            Playfield[newSelection.X, newSelection.Y] = Playfield[_selected.X, _selected.Y];
                                            Playfield[_selected.X, _selected.Y] = selectedGem;
                                            _selected.X = _selected.Y = -1;
                                            _remainingMoves--;
                                            _dropDownTimer = time_between_block_drops;
                                        }
                                    }
                                }
                            }
                        }

                    detectClusters();
                }
            }
            base.Update(gameTime);
        }
        private void detectClusters()
        {
            for (int y = 0; y < fieldY; y++)
                detectRowCluster(y);
            for (int x = 0; x < fieldX; x++)
                detectColumnCluster(x);

            _ScoreServ.Score += (_greenHits.Count * _greenHits.Count + _blueHits.Count * _blueHits.Count +
                      _orangeHits.Count * _orangeHits.Count) * 100;
            removeCountedClusters();

        }

        private void detectColumnCluster(int column)
        {
            for (int yStart = 0; yStart < fieldY - 3; yStart++)
            {
                GemType gem = Playfield[column, yStart];
                int hits = 0;
                for (int y = yStart; y < fieldY; y++)
                {
                    if (gem != Playfield[column, y] && Playfield[column, y] != GemType.Star)
                    {
                        if (hits < 4)
                            hits = 0;
                        break;
                    }
                    hits++;
                }
                if (hits >= 4)
                {
                    for (int i = 0; i < hits; i++)
                    {
                        addHit(new Point(column, yStart + i));
                    }
                }
            }
        }

        private void detectRowCluster(int row)
        {
            for (int xStart = 0; xStart < 7 - 3; xStart++)
            {
                GemType gem = Playfield[xStart, row];
                int hits = 0;
                for (int x = xStart; x < fieldX; x++)
                {
                    if (gem != Playfield[x, row] && Playfield[x, row] != GemType.Star)
                    {
                        if (hits < 4)
                            hits = 0;
                        break;
                    }
                    hits++;
                }
                if (hits >= 4)
                {
                    for (int i = 0; i < hits; i++)
                    {
                        addHit(new Point(xStart + i, row));
                    }
                }
            }
        }

        private void removeCountedClusters()
        {
            foreach (Point hit in _blueHits)
            {
                Playfield[hit.X, hit.Y] = GemType.None;
            }
            foreach (Point hit in _greenHits)
            {
                Playfield[hit.X, hit.Y] = GemType.None;
            }
            foreach (Point hit in _orangeHits)
            {
                Playfield[hit.X, hit.Y] = GemType.None;
            }
            _blueHits.Clear();
            _greenHits.Clear();
            _orangeHits.Clear();
        }

        private void addHit(Point position)
        {
            switch (Playfield[position.X, position.Y])
            {
                case GemType.Blue:
                    _blueHits.Add(position);
                    break;
                case GemType.Green:
                    _greenHits.Add(position);
                    break;
                case GemType.Orange:
                    _orangeHits.Add(position);
                    break;
                case GemType.Star:
                    _starHits.Add(position);
                    break;
            }
        }

        private void moveFallingGems()
        {
            bool foundGem = false;
            for (int x = 0; x < fieldX; x++)
            {
                for (int y = fieldY - 1; y > 0; y--)
                {
                    GemType gem = Playfield[x, y - 1];
                    if (gem != GemType.None && Playfield[x, y] == GemType.None)
                    {
                        foundGem = true;
                        Playfield[x, y] = gem;
                        Playfield[x, y - 1] = GemType.None;
                    }
                }
            }
            _blocksAreFalling = foundGem;

        }

        public override void Draw(GameTime gameTime)
        {

            spriteBatch.Begin();
            for (int x = 0; x < fieldX; x++)
            {
                for (int y = 0; y < fieldY; y++)
                {
                    Texture2D toDraw = null;
                    switch (Playfield[x, y])
                    {
                        case GemType.Blue:
                            toDraw = _blueGem;
                            break;
                        case GemType.Green:
                            toDraw = _greenGem;
                            break;
                        case GemType.Orange:
                            toDraw = _orangeGem;
                            break;
                        case GemType.Star:
                            toDraw = _starGem;
                            break;
                        case GemType.None:
                            continue;
                    }
                    if (_selected.X == x && _selected.Y == y)
                        spriteBatch.Draw(toDraw, new Rectangle(x * 80, y * 80, 72, 72), _gemSource, Color.White);
                    else
                    {
                        spriteBatch.Draw(toDraw, new Rectangle(x * 80, y * 80, 72, 72), _gemSource, _fadedColor);
                    }
                }
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
