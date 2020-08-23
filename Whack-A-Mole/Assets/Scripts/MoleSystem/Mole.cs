// Copyright Ramses Jelsma, 2020

using HoleSystem;
using LevelManagement;
using UnityEngine;

namespace MoleSystem
{
    public class Mole : MonoBehaviour
    {
        public MoleBehaviour behaviour;

        public void StartMoleCycle(HoleManager holeManager)
        {
            behaviour.Setup(this, holeManager);
            LevelManager.LevelEndsEvent += OnEndLevel;
        }

        private void OnEndLevel()
        {
            StopAllCoroutines();
        }
    }
}