using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EDGELORD.Manager {
    public class GameManager : Singleton<GameManager> {
        private MusicPlayer musicPlayer;
        private SfxPlayer sfxPlayer;
        private Canvas ui;

        public bool DEBUG_Disable_Music = false;

        /* game field and other objects go here */

        protected GameManager () {}

        void Awake () {
            musicPlayer = (MusicPlayer)FindObjectOfType(typeof(MusicPlayer));
            sfxPlayer = (SfxPlayer)FindObjectOfType(typeof(SfxPlayer));
            ui = (Canvas)FindObjectOfType(typeof(Canvas));
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

            // run countdown
            Countdown countdown = ui.GetComponentInChildren<Countdown>();
            countdown.StartCountdown();

            // allow player control
        }
    }
}