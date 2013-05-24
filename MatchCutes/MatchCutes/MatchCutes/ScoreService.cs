using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MatchCutes
{

    // Laget et service for å lett kunne bruke variabler mellom gameklassen, textComponent og bordComponent.
    class ScoreService
    {
        public int Score { get; set; }
        public bool gameOver { get; set; }
        public int clickCount {get; set;}
    }
}
