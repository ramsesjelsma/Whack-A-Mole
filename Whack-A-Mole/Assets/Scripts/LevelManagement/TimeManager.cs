// Copyright Ramses Jelsma, 2020

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace LevelManagement
{
    /// <summary>
    /// Manages the remaining time for the level
    /// </summary>
    public class TimeManager : MonoBehaviour
    {
        [Header("References")]
        public LevelManager levelManager;
        public Text timeText;

        private float levelTime;

        private void Awake()
        {
            LevelManager.LevelStartsEvent += LevelStartEvent;
        }

        private void LevelStartEvent(LevelStats i_stats)
        {
            levelTime = i_stats.levelDuration;
            StartCoroutine(ReduceTime());
        }

        private IEnumerator ReduceTime()
        {
            while (levelTime > 0)
            {
                levelTime -= Time.deltaTime;
                timeText.text = Math.Truncate(levelTime).ToString();
                yield return new WaitForEndOfFrame();
            }

            levelManager.OnLevelEnd();
        }
    }
}