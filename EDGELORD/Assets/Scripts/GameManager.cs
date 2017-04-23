using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace EDGELORD.Manager {
    public class GameManager : Singleton<GameManager> {
        private MusicPlayer musicPlayer;
        private SfxPlayer sfxPlayer;
        private Canvas ui;

        private ScoreDisplay player1ScoreDisplay;
        private ScoreDisplay player2ScoreDisplay;
        private TimerDisplay timerDisplay;

        private bool isGameOver;
        private float timeLeft;

        public int gameLengthInSeconds = 60;
        public bool DEBUG_Disable_Music = false;

        /* game field and other objects go here */

        protected GameManager () {}

        void Awake () {
            isGameOver = true;
            musicPlayer = (MusicPlayer)FindObjectOfType(typeof(MusicPlayer));
            sfxPlayer = (SfxPlayer)FindObjectOfType(typeof(SfxPlayer));
            ui = (Canvas)FindObjectOfType(typeof(Canvas));

            Component[] playerScoreDisplays = ui.GetComponentsInChildren<ScoreDisplay>();
            player1ScoreDisplay = (ScoreDisplay)playerScoreDisplays[0];
            player2ScoreDisplay = (ScoreDisplay)playerScoreDisplays[1];
            timerDisplay = ui.GetComponentInChildren<TimerDisplay>();
            timeLeft = gameLengthInSeconds;
            timerDisplay.UpdateTime(gameLengthInSeconds);

            InitObjects();
            StartGame();
        }

        void InitObjects () {
            // TODO: instantiate and setup playing field and player objects here
        }

        void StartGame () {
            if (!isGameOver) {
                return;
            }

            if (!DEBUG_Disable_Music) {
                musicPlayer.StartMusic();
            }

            isGameOver = false;
            StartCoroutine(startTimer());
            StartCoroutine(TEST_updateScore());
        }

        void StopGame () {

        }

        public void UpdateScores () {
            // TODO: calculate scores for each player here
            player1ScoreDisplay.UpdateScore(Random.value);
            player2ScoreDisplay.UpdateScore(Random.value);
        }

        private IEnumerator startTimer() {
            while (timeLeft > 0) {
                timeLeft -= Time.deltaTime;
                timerDisplay.UpdateTime(timeLeft);
                yield return null;
            }
            timerDisplay.UpdateTime(0);
            StopGame();
        }

        private IEnumerator TEST_updateScore() {
            while (true) {
                yield return new WaitForSeconds(1);
                UpdateScores();
            }
        }
    }
}