// Copyright Ramses Jelsma, 2020

using HoleSystem;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LevelManagement
{
    /// <summary>
    /// Manages the level lifecycle, as such is persistent.
    /// </summary>
    public class LevelManager : EventMonoBehaviourBase
    {
        public static LevelManager instance; // There can only be one LevelManager
        [Header("References")]
        public List<LevelStats> levelStats = new List<LevelStats>();

        [HideInInspector]
        public int currentLevel = 0;

        public delegate void LevelStartedHandler(LevelStats i_stats);
        public static event LevelStartedHandler LevelStartsEvent;
        public delegate void LevelEndedHandler();
        public static event LevelEndedHandler LevelEndsEvent;

        /// <summary>
        /// Enable singleton pattern
        /// </summary>
        private void Awake()
        {
            if (instance != null && instance != this)
            {
                DestroyImmediate(this.gameObject);
            }
            else
            {
                instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
        }

        private void Start()
        {
            SceneManager.sceneLoaded += StartLevelOnSceneLoad;
            StartLevel(currentLevel);
        }

        private void StartLevelOnSceneLoad(Scene arg0, LoadSceneMode arg1)
        {
            StartLevel(currentLevel);
        }

        private void StartLevel(int i_index)
        {
            CabinetSpawner cabinetSpawner = new CabinetSpawner();
            GameObject cabinet = cabinetSpawner.SpawnCabinet(levelStats[i_index]);
            levelStats[i_index].cabinetInstance = cabinet;
            MoleSpawner spawner = new MoleSpawner();
            spawner.SpawnMoles(FindObjectOfType<HoleManager>(), levelStats[i_index]);
            LevelStartsEvent?.Invoke(levelStats[i_index]);
        }

        /// <summary>
        /// For static events, always unsubscribe at level end
        /// </summary>
        public override void OnLevelEnd()
        {
            foreach (LevelStartedHandler d in LevelStartsEvent.GetInvocationList())
            {
                LevelStartsEvent -= (LevelStartedHandler)d;
            }

            LevelEndsEvent?.Invoke();


            foreach (LevelEndedHandler d in LevelEndsEvent.GetInvocationList())
            {
                LevelEndsEvent -= (LevelEndedHandler)d;
            }
        }

        public void RestartLevel()
        {
            SceneManager.LoadScene(0);
        }

        public void NextLevel()
        {
            currentLevel++;
            SceneManager.LoadScene(0);
        }

        public void PreviousLevel()
        {
            currentLevel--;
            SceneManager.LoadScene(0);
        }

    }
}
