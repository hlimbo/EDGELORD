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

        public bool DEBUG_Disable_Music = false;

        /* game field and other objects go here */

        protected GameManager () {}

        void Awake () {
            musicPlayer = (MusicPlayer)FindObjectOfType(typeof(MusicPlayer));
            sfxPlayer = (SfxPlayer)FindObjectOfType(typeof(SfxPlayer));
            ui = (Canvas)FindObjectOfType(typeof(Canvas));

            Component[] playerScoreDisplays = ui.GetComponentsInChildren<ScoreDisplay>();
            player1ScoreDisplay = (ScoreDisplay)playerScoreDisplays[0];
            player2ScoreDisplay = (ScoreDisplay)playerScoreDisplays[1];

            InitObjects();
            StartGame();
        }

        void InitObjects () {
            // TODO: instantiate and setup playing field and player objects here
        }

        void StartGame () {
            if (!DEBUG_Disable_Music) {
                musicPlayer.StartMusic();
            }

            // // run countdown
            // Countdown countdown = ui.GetComponentInChildren<Countdown>();
            // if (countdown != null) {
            //     countdown.StartCountdown();
            // } else {
            //     Debug.Log("countdown is null");
            // }

            // allow player control

            // DEBUG
            StartCoroutine(TEST_updateScore());
        }

        public void UpdateScores () {
            // TODO: calculate scores for each player here
            player1ScoreDisplay.UpdateScore(Random.value);
            player2ScoreDisplay.UpdateScore(Random.value);
        }

        private IEnumerator TEST_updateScore() {
            while (true) {
                yield return new WaitForSeconds(1);
                UpdateScores();
            }
        }
    }
}