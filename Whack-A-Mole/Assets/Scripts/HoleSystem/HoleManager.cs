// Copyright Ramses Jelsma, 2020

using System.Collections.Generic;
using UnityEngine;
using LevelManagement;

namespace HoleSystem
{
    /// <summary>
    /// HoleManager manages all the holes, 
    /// - currently places all the holes at the start of the game.
    /// - mainly a point for the Moles to go to when they need to enter/leave holes.
    /// </summary>
    public class HoleManager : MonoBehaviour
    {
        [Header("References")]
        public GameObject holePrefab;

        private List<Hole> allHoles = new List<Hole>();
        private int maxActiveMoles;
        private int currentActiveMoles = 0;

        private void Awake()
        {
            LevelManager.LevelStartsEvent += OnLevelLoaded;
        }

        private void OnLevelLoaded(LevelStats i_stats)
        {
            int holesAmount = i_stats.holeAmount;
            HolePlacementParadigm placementParadigm = i_stats.holePlacementParadigm;
            allHoles = placementParadigm.Place(holesAmount, holePrefab, i_stats.cabinetInstance); // Place holes

            maxActiveMoles = i_stats.maxActiveMoles;
        }

        /// <summary>
        /// Used by mole to request an available hole (if there is one)
        /// </summary>
        /// <returns>Available hole OR null if there is none</returns>
        public Hole RequestAvailableHole()
        {
            List<Hole> availableHoles = GetAvailableHoles();
            // Only go if currently active moles are LESS than maximum allowed
            // And if there are free holes at all (occupied holes are less than all holes)
            if (currentActiveMoles < maxActiveMoles && availableHoles.Count > 0)
            {
                System.Random random = new System.Random();
                int randomHoleNumber = random.Next(availableHoles.Count);
                Hole chosenHole = availableHoles[randomHoleNumber];

                chosenHole.occupied = true;
                currentActiveMoles++;
                return chosenHole;
            }
            else
            {
                return null;
            }
        }

        public void LeaveHole(Hole i_holeToLeave)
        {
            i_holeToLeave.occupied = false;
            currentActiveMoles--;
        }

        private List<Hole> GetAvailableHoles()
        {
            List<Hole> availableHoles = new List<Hole>();
            for (int i = 0; i < allHoles.Count; i++)
            {
                if (allHoles[i].occupied == false)
                {
                    availableHoles.Add(allHoles[i]);
                }
            }

            return availableHoles;
        }
    }
}