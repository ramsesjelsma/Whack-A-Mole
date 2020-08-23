// Copyright Ramses Jelsma, 2020

using HoleSystem;
using System.Collections.Generic;
using UnityEngine;

namespace LevelManagement
{
    /// <summary>
    /// Spiral placement.
    /// </summary>
    [CreateAssetMenu(menuName = "ScriptableObjects/Hole Placement Spiral")]
    public class HolePlacementParadigmSpiral: HolePlacementParadigm
    {
        [Range(0.5f, 3f)]
        public float marginBetweenHoles;

        public float angleBetweenHoles = 1;
        public float sizeOfCircle = 1f;

        private float startRadius = 0.1f;
        private float startAngle = 1f;

        public override List<Hole> Place(int i_holeAmount, GameObject i_holeToPlace, GameObject i_surfaceToPlaceOn)
        {
            List<Hole> allHoles = new List<Hole>();
            // Set start placing position to top of surface
            Vector3 startingPositionBase = i_surfaceToPlaceOn.transform.position
                                        + new Vector3(0, (i_surfaceToPlaceOn.transform.localScale.y / 2), 0);

            GameObject placer = new GameObject();
            placer.transform.position = startingPositionBase;

            float tempAngle = startAngle;
            float tempRadius = startRadius;
            for (int i = 0; i < i_holeAmount; i++)
            {
                tempAngle += marginBetweenHoles * angleBetweenHoles;
                tempRadius += marginBetweenHoles * sizeOfCircle;

                float x = tempRadius * Mathf.Cos(Mathf.Deg2Rad * tempAngle);
                float z = tempRadius * Mathf.Sin(Mathf.Deg2Rad * tempAngle);

                placer.transform.position += new Vector3(x, 0, z);

                GameObject hole = Instantiate(i_holeToPlace);
                hole.name = "Hole #" + i.ToString();
                hole.transform.position = placer.transform.position;
                hole.transform.SetParent(i_surfaceToPlaceOn.transform);

                allHoles.Add(hole.AddComponent<Hole>());
            }

            Destroy(placer);
            return allHoles;
        }

    }
}