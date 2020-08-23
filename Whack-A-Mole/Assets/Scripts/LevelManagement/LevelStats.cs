// Copyright Ramses Jelsma, 2020

using MoleSystem;
using System.Collections.Generic;
using UnityEngine;


namespace LevelManagement {
    [CreateAssetMenu(menuName = "ScriptableObjects/Level Stats")]
    public class LevelStats : ScriptableObject
    {
        public string levelName;
        [Header("Hole Stats")]
        public int holeAmount;
        public HolePlacementParadigm holePlacementParadigm;
        [Space(10)]
        [Header("Mole Stats")]
        public List<MoleBehaviour> moles = new List<MoleBehaviour>();
        public int maxActiveMoles;
        [Space(10)]
        [Header("Other")]
        public GameObject cabinetPrefab;
        public int levelDuration;
        public int scoreNeededToWin;

        [HideInInspector]
        public GameObject cabinetInstance;
    }
}