using System.Collections;
using System.Collections.Generic;
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

        public MusicPlayer musicPlayer;
        //private SfxPlayer sfxPlayer;
        public Canvas ui;
        public GameObject[] players;

        public int gameLengthInSeconds = 60;
        public bool DEBUG_Disable_Music = false;

        protected GameManager () {}

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
            TEST_scoreUpdateCoroutine = TEST_updateScore();

            InitObjects();

            if (!DEBUG_Disable_Music) {
                musicPlayer.StartMusic();
            }

        }

        void Update () {
            if (!gameInProgress) {
                StartGame();
                gameInProgress = true;
                return;
            }           
        }

        void InitObjects () {
            timeLeft = gameLengthInSeconds;
            timerDisplay.UpdateTime(gameLengthInSeconds);

            player1ScoreDisplay.ResetScore();
            player2ScoreDisplay.ResetScore();

            foreach (var player in players) {
                // disable player input
            }
        }

        void StartGame () {
            foreach (var player in players) {
                // enable player input
            }
            StartCoroutine(timerCoroutine);
            StartCoroutine(TEST_scoreUpdateCoroutine);
        }

        void StopGame () {
            StopCoroutine(timerCoroutine);
            timerDisplay.ResetTime();
            gameInProgress = false;
        }

        public void UpdateScores () {
            // TODO: calculate scores for each player here
            player1ScoreDisplay.UpdateScore(Random.value);
            player2ScoreDisplay.UpdateScore(Random.value);
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