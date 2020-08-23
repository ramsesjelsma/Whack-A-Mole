// Copyright Ramses Jelsma, 2020

using UnityEngine;
using UnityEngine.UI;
using LevelManagement;
using System.Collections;

namespace UI
{
    /// <summary>
    /// Manages all of the UI in the game, and as such, requires many references.
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        [Header("References")]
        public ScoreManager scoreManager;

        [Header("In Game UI")]
        public Text ingameScoreText;

        [Header("End Game UI")]
        public Canvas inGameUI;
        public Canvas endOfGameUI;
        public Button previousLevel;
        public Button retryLevel;
        public Button nextLevel;
        [Space(5)]
        public Text levelText;
        public Text endgameScoreText;
        public Text molesHitText;
        public Text accuracyText;
        public Text winText;
        [Space(5)]
        public GameObject highscoreText;

        [Header("Tweakables")]
        [Header("In Game UI")]
        public float scoreAnimDuration = 0.2f;
        public AnimationCurve animCurve;
        [Header("End Game UI")]
        public string onWinText;
        public string onLoseText;

        private LevelManager levelManager; // gained through object.find, because it's static

        // For in game score text:
        private Vector2 standardTextSize;
        private Color standardTextColor;
        private Coroutine activeAnimation;

        private void Awake()
        {
            LevelManager.LevelStartsEvent += OnLevelStarts;
            LevelManager.LevelEndsEvent += OnLevelEnds;

            standardTextSize = ingameScoreText.GetComponent<RectTransform>().localScale;
            standardTextColor = ingameScoreText.color;
            ingameScoreText.text = scoreManager.LevelScore.score.ToString() + "/" + scoreManager.LevelScore.minimumScore.ToString();
        }


        #region in-game UI and end-game UI management
        private void OnLevelStarts(LevelStats i_stats)
        {
            inGameUI.enabled = true;
            endOfGameUI.enabled = false;

            levelManager = FindObjectOfType<LevelManager>();

            // I always add functions to button in code for easier debugging
            previousLevel.onClick.AddListener(levelManager.PreviousLevel);
            retryLevel.onClick.AddListener(levelManager.RestartLevel);
            nextLevel.onClick.AddListener(levelManager.NextLevel);
        }

        private void OnLevelEnds()
        {
            inGameUI.enabled = false;
            endOfGameUI.enabled = true;

            scoreManager.GetScoreInformation(out bool win, out bool highscore, out ScoreManager.LevelScoring levelScore);
            highscoreText.SetActive(highscore);
            SetButtonsActive(win);
            SetUITexts(win, levelScore);
        }

        private void SetUITexts(bool i_win, ScoreManager.LevelScoring i_levelScore)
        {
            levelText.text = (levelManager.currentLevel + 1).ToString();
            endgameScoreText.text = i_levelScore.score + "/" + i_levelScore.minimumScore;
            molesHitText.text = i_levelScore.molesHit.ToString();
            accuracyText.text = (((float)i_levelScore.molesHit / ((float)i_levelScore.molesHit + (float)i_levelScore.molesMissed)) * 100f).ToString() + "%" ;

            if (i_win)
            {
                winText.text = onWinText;
            }
            else
            {
                winText.text = onLoseText;
            }
        }

        private void SetButtonsActive(bool i_win)
        {
            if (levelManager.levelStats.Count == levelManager.currentLevel + 1 || i_win == false)
            {
                nextLevel.interactable = false;
            }
            else
            {
                nextLevel.interactable = true;
            }

            if (levelManager.currentLevel == 0)
            {
                previousLevel.interactable = false;
            }
            else
            {
                previousLevel.interactable = true;
            }
        }
        #endregion

        #region scoreLogic
        public void SetScoreText(bool positive)
        {
            if (activeAnimation != null) StopCoroutine(activeAnimation);
            activeAnimation = StartCoroutine(AnimateScoreText(positive));
        }

        private IEnumerator AnimateScoreText(bool positive)
        {
            ingameScoreText.color = positive == true ? Color.green : Color.red;
            ingameScoreText.text = scoreManager.LevelScore.score.ToString() + "/" + scoreManager.LevelScore.minimumScore.ToString();

            float i = 0f;
            float multiplier = positive == true ? 1.25f : 0.75f;
            RectTransform target = ingameScoreText.GetComponent<RectTransform>();

            Vector2 targetScale = new Vector2(target.localScale.x * multiplier, target.localScale.y * multiplier);
            while (i < 1)
            {
                target.localScale = Vector2.Lerp(standardTextSize, targetScale, animCurve.Evaluate(i));

                i += Time.deltaTime / scoreAnimDuration;
                yield return new WaitForEndOfFrame();
            }
            i = 0f;
            while (i < 1)
            {
                target.localScale = Vector2.Lerp(targetScale, standardTextSize, animCurve.Evaluate(i));

                i += Time.deltaTime / scoreAnimDuration;
                yield return new WaitForEndOfFrame();
            }

            target.localScale = standardTextSize;
            ingameScoreText.color = standardTextColor;
        }
        #endregion
    }
}