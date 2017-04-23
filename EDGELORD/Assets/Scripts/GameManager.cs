using System.Collections;
using System.Collections.Generic;
using EDGELORD.TreeBuilder;
using Players;
using UnityEngine;
using TMPro;

namespace EDGELORD.Manager {
    public class GameManager : Singleton<GameManager> {

        private ScoreDisplay player1ScoreDisplay;
        private ScoreDisplay player2ScoreDisplay;
        private TimerDisplay timerDisplay;

        private bool gameInProgress;
        private float timeLeft;

        private IEnumerator timerCoroutine;
        private IEnumerator TEST_scoreUpdateCoroutine;

        private Vector3[] startingPlayerPositions;

        public MusicPlayer musicPlayer;
        //private SfxPlayer sfxPlayer;
        public Canvas ui;
        public GameObject[] players;

        public int gameLengthInSeconds = 60;
        public bool DEBUG_Disable_Music = false;

        protected GameManager () {}

        public TreeRoot Player1TreeRoot;
        public TreeRoot Player2TreeRoot;

        void Start () {
            gameInProgress = false;
            musicPlayer = (MusicPlayer)FindObjectOfType<MusicPlayer>();
            //sfxPlayer = (SfxPlayer)FindObjectOfType(typeof(SfxPlayer));
            ui = (Canvas)FindObjectOfType<Canvas>();
            timerDisplay = ui.GetComponentInChildren<TimerDisplay>();

            Component[] playerScoreDisplays = ui.GetComponentsInChildren<ScoreDisplay>();
            player1ScoreDisplay = (ScoreDisplay)playerScoreDisplays[0];
            player2ScoreDisplay = (ScoreDisplay)playerScoreDisplays[1];

            timerCoroutine = startTimer();
            //TEST_scoreUpdateCoroutine = TEST_updateScore();

            startingPlayerPositions = new Vector3[] { players[0].transform.position, players[1].transform.position };

            InitObjects();

            if (!DEBUG_Disable_Music) {
                musicPlayer.StartMusic();
            }

            var roots = FindObjectsOfType<TreeRoot>();
            if(roots.Length >= 2)
            {
                if (roots[0].OwningPlayer == PlayerID.Player_1)
                {
                    Player1TreeRoot = roots[0];
                    Player2TreeRoot = roots[1];
                }
                else
                {
                    Player1TreeRoot = roots[1];
                    Player2TreeRoot = roots[0];
                }
                if (Player1TreeRoot)
                {
                    Player1TreeRoot.OnUpdateArea += area =>
                    {
                        player1ScoreDisplay.UpdateScore(area);
                    };
                }
                if (Player2TreeRoot)
                {
                    Player2TreeRoot.OnUpdateArea += area =>
                    {
                        player2ScoreDisplay.UpdateScore(area);
                    };
                }
                
            }

        }

        void Update () {
            // wait until both players have readied

            // start the game
            if (!gameInProgress) {
                StartGame();
                gameInProgress = true;
                return;
            }          

            if (timeLeft <= 0) {
                StopGame();
            }
            // when the game is over determine the winner

            // wait for player input to quit or restart game
        }

        void InitObjects () {
            timeLeft = gameLengthInSeconds;
            timerDisplay.UpdateTime(gameLengthInSeconds);

            player1ScoreDisplay.ResetScore();
            player2ScoreDisplay.ResetScore();

            for (int i = 0; i < players.Length; ++i) {
                // disable player input and reset position
                players[i].GetComponent<PlayerInputManager>().inputsEnabled = false;
                players[i].transform.position = startingPlayerPositions[i];
            }
        }

        void StartGame () {
            foreach (var player in players) {
                // enable player input
                player.GetComponent<PlayerInputManager>().inputsEnabled = true;
            }
            StartCoroutine(timerCoroutine);
            StartCoroutine(TEST_scoreUpdateCoroutine);
        }

        void StopGame () {
            StopCoroutine(timerCoroutine);
            timerDisplay.ResetTime();
            gameInProgress = false;

            foreach (var player in players) {
                // disable player input
                player.GetComponent<PlayerInputManager>().inputsEnabled = false;
            }
            ResetGame();
        }

        void ResetGame () {
            InitObjects();
        }

        public void UpdateScores () {
            // TODO: calculate scores for each player here
            //if (Player1TreeRoot)
            //{
            //    player1ScoreDisplay.UpdateScore(Player1TreeRoot.TotalArea);
            //}
            //if (Player2TreeRoot)
            //{
            //    player2ScoreDisplay.UpdateScore(Player2TreeRoot.TotalArea);
            //}

            //player1ScoreDisplay.UpdateScore(Random.value);
            //player2ScoreDisplay.UpdateScore(Random.value);
        }

        private IEnumerator startTimer() {
            timerDisplay.ResetTime();
            while (timeLeft > 0) {
                timeLeft -= Time.deltaTime;
                timerDisplay.UpdateTime(timeLeft);
                yield return null;
            }
            timerDisplay.UpdateTime(0);
        }

        private IEnumerator TEST_updateScore() {
            while (true) {
                yield return new WaitForSeconds(1);
                UpdateScores();
            }
        }
    }
}