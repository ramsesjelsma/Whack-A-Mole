// Copyright Ramses Jelsma, 2020

using InputSystem;
using UI;
using UnityEngine;

namespace LevelManagement
{ 
    /// <summary>
    /// Keeps track of the player's score in the level.
    /// </summary>
    public class ScoreManager : MonoBehaviour
    {
        [System.Serializable]
        public class LevelScoring
        {
            public int score;
            public int minimumScore;
            public int molesHit;
            public int molesMissed;
        }
        public LevelScoring LevelScore { get => levelScore; }
        public delegate void ScoreAddHandler(int i_scoreToAdd);

        [Header("References")]
        public UIManager uiManager;

        private LevelScoring levelScore = new LevelScoring();
        private string levelName;
        private int highScore;
        

        private void Awake()
        {
            LevelManager.LevelStartsEvent += LevelStartEvent;
            LevelManager.LevelEndsEvent += LevelEndEvent;
            MoleHitManager.OnMoleHit += OnMoleHit;
            MoleHitManager.OnMoleMiss += OnMoleMiss;
        }

        public void GetScoreInformation(out bool o_win, out bool o_highscore, out LevelScoring o_levelScore)
        {
            o_win = LevelScore.score >= LevelScore.minimumScore;
            o_highscore = o_win && levelScore.score > highScore;
            o_levelScore = LevelScore;
        }

        private void OnMoleHit(int i_scoreToAdd)
        {
            levelScore.score += i_scoreToAdd;
            levelScore.molesHit++;

            uiManager.SetScoreText(true);
        }

        private void OnMoleMiss(int i_scoreToAdd)
        {
            levelScore.score += i_scoreToAdd;
            if (levelScore.score < 0)
            {
                levelScore.score = 0;
            }
            levelScore.molesMissed++;

            uiManager.SetScoreText(false) ;
        }

        private void LevelStartEvent(LevelStats i_stats)
        {
            levelScore.minimumScore = i_stats.scoreNeededToWin;
            levelName = i_stats.levelName;
            highScore = PlayerPrefs.GetInt(levelName, 0);
        }

        private void LevelEndEvent()
        {
            if (LevelScore.score >= LevelScore.minimumScore && levelScore.score > highScore)
            {
                PlayerPrefs.SetInt(levelName, levelScore.score);
                PlayerPrefs.Save();
            }
        }
    }
}