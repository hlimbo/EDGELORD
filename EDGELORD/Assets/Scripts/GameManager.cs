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
        private WinnerDisplay winnerDisplay;

        private GameObject[] winnerMessages;

        private bool gameInProgress;
        private bool playersReady;
        private float timeLeft;

        private IEnumerator timerCoroutine;
        private IEnumerator TEST_scoreUpdateCoroutine;

        private Vector3[] startingPlayerPositions;

        public MusicPlayer musicPlayer;
        //private SfxPlayer sfxPlayer;
        public Canvas ui;
        public GameObject[] players;

        public int gameLengthInSeconds = 60;
        public int countdownLengthInSeconds = 3;
        public bool DEBUG_Disable_Music = false;

        protected GameManager () {}

        public TreeRoot Player1TreeRoot;
        public TreeRoot Player2TreeRoot;

        void Start () {
            gameInProgress = false;
            playersReady = false;

            musicPlayer = (MusicPlayer)FindObjectOfType<MusicPlayer>();
            //sfxPlayer = (SfxPlayer)FindObjectOfType(typeof(SfxPlayer));
            ui = (Canvas)FindObjectOfType<Canvas>();
            timerDisplay = ui.GetComponentInChildren<TimerDisplay>();
            winnerDisplay = ui.GetComponent<WinnerDisplay>();

            Component[] playerScoreDisplays = ui.GetComponentsInChildren<ScoreDisplay>();
            player1ScoreDisplay = (ScoreDisplay)playerScoreDisplays[0];
            player2ScoreDisplay = (ScoreDisplay)playerScoreDisplays[1];

            timerCoroutine = startTimer();

            startingPlayerPositions = new Vector3[] { players[0].transform.position, players[1].transform.position };

            InitObjects();

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

            // TODO: temporary
            playersReady = true;

            if (!DEBUG_Disable_Music) {
                musicPlayer.StartMusic();
            }
        }

        void Update () {
            // wait until both players have readied

            // start the game
            if (!gameInProgress && playersReady) {
                StartGame();
                gameInProgress = true;
                return;
            }          

            if (timeLeft <= 0 && gameInProgress) {
                StopGame();
                showWinner();
                playersReady = false;

                // if (true) { // TODO: get player input to restart the game here
                //     ResetGame();
                // }
            }
        }

        void InitObjects () {
            timeLeft = gameLengthInSeconds;
            timerDisplay.UpdateTime(gameLengthInSeconds);
            winnerDisplay.HideWindow();

            player1ScoreDisplay.ResetScore();
            player2ScoreDisplay.ResetScore();

            disablePlayerInput(true);
        }

        void StartGame () {
            enablePlayerInput();
            StartCoroutine(startGameWithCountdown());
        }

        private IEnumerator startGameWithCountdown () {
            int downCounter = countdownLengthInSeconds;
            while (downCounter > 0) {
                Debug.Log(System.Convert.ToString(downCounter)); // TODO: show message onscreen here
                --downCounter;
                yield return new WaitForSeconds(1);
            }
            // TODO: hide message here
            StartCoroutine(timerCoroutine);
        }

        void StopGame () {
            StopCoroutine(timerCoroutine);
            timerDisplay.ResetTime();
            gameInProgress = false;

            Debug.Log("game stopped");

            disablePlayerInput();
        }

        void ResetGame () {
            InitObjects();
        }

        private void showWinner () {
            string winner;
            if (Player1TreeRoot.TotalArea > Player2TreeRoot.TotalArea) {
                winner = "PLAYER1";
            } else if (Player1TreeRoot.TotalArea < Player2TreeRoot.TotalArea) {
                winner = "PLAYER2";
            } else {
                winner = "TIE";
            }

            winnerDisplay.ShowMessage(winner);
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

        private void enablePlayerInput() {
            foreach (var player in players) {
                player.GetComponent<PlayerInputManager>().inputsEnabled = true;
            }
        }

        private void disablePlayerInput(bool resetPosition = false) {
            for (int i = 0; i < players.Length; ++i) {
                players[i].GetComponent<PlayerInputManager>().inputsEnabled = false;
                if (resetPosition) {
                    players[i].transform.position = startingPlayerPositions[i];
                }
            }
        }
    }
}