// Copyright Ramses Jelsma, 2020

using HoleSystem;
using MoleSystem;
using UnityEngine;

namespace LevelManagement
{
    /// <summary>
    /// Helper class.
    /// </summary>
    public class MoleSpawner
    {
        public void SpawnMoles(HoleManager holeManager, LevelStats i_stats)
        {
            for (int i = 0; i < i_stats.moles.Count; i++)
            {
                GameObject moleObj = GameObject.Instantiate(i_stats.moles[i].molePrefab);
                moleObj.name = "Mole #" + i.ToString();
                moleObj.transform.SetParent(holeManager.transform);
                Mole mole = moleObj.AddComponent<Mole>();
                mole.behaviour = GameObject.Instantiate(i_stats.moles[i]);
                mole.StartMoleCycle(holeManager);
            }
        }
    }
}