// Copyright Ramses Jelsma, 2020

using UnityEngine;

namespace LevelManagement
{
    /// <summary>
    /// Helper class.
    /// </summary>
    public class CabinetSpawner
    {
        /// <summary>
        /// Could pass a Transform to parent the Cabinet, but I like it in the scene root.
        /// </summary>
        public GameObject SpawnCabinet(LevelStats i_stats)
        {
            GameObject cabinet = GameObject.Instantiate(i_stats.cabinetPrefab);
            cabinet.name = "CabinetInstance";
            cabinet.transform.position = Vector3.zero;

            return cabinet;
        }
    }
}