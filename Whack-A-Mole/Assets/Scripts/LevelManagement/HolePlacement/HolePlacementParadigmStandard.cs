// Copyright Ramses Jelsma, 2020

using HoleSystem;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LevelManagement
{
    /// <summary>
    /// Standard way of placing holes.
    /// </summary>
    [CreateAssetMenu(menuName = "ScriptableObjects/Hole Placement Standard")]
    public class HolePlacementParadigmStandard : HolePlacementParadigm
    {
        public int maxRowWidth;
        [Range(0, 1)]
        public float marginBetweenHoles;

        public override List<Hole> Place(int i_holeAmount, GameObject i_holeToPlace, GameObject i_surfaceToPlaceOn)
        {
            List<Hole> allHoles = new List<Hole>();

            // Set up variables first
            double rowAmount = (double)i_holeAmount / maxRowWidth;
            int rowAmountCeiling = (int)Math.Ceiling(rowAmount);
            int holesToPlace = i_holeAmount;

            // Set start placing position to top of surface
            Vector3 startingPositionBase = i_surfaceToPlaceOn.transform.position
                                        + new Vector3(0, (i_surfaceToPlaceOn.transform.localScale.y / 2), 0);

            // Offset in depth based on amount of rows
            startingPositionBase += new Vector3(0, 0, CalculateOffset(rowAmountCeiling, startingPositionBase, i_holeToPlace));

            // And place
            for (int i = 0; i < rowAmount; i++)
            {
                Vector3 startingPosition = startingPositionBase;

                // Starting position goes up by the hole's depth + margin per row
                float sizeOfHolesInDepth = i_holeToPlace.transform.localScale.z * i;
                float sizeOfMarginsInDepth = (i_holeToPlace.transform.localScale.z * marginBetweenHoles) * i;
                startingPosition -= new Vector3(0, 0, sizeOfHolesInDepth + sizeOfMarginsInDepth);

                List<Hole> holesToAdd;

                // If this is NOT leftovers:
                if (holesToPlace >= maxRowWidth)
                {
                    startingPosition -= new Vector3(CalculateOffset(maxRowWidth, startingPosition, i_holeToPlace), 0, 0);
                    holesToAdd = PlaceHoles(maxRowWidth, startingPosition, i_holeToPlace, i_surfaceToPlaceOn.transform);
                    holesToPlace -= maxRowWidth;
                }
                // If this IS leftovers:
                else
                {
                    startingPosition -= new Vector3(CalculateOffset(holesToPlace, startingPosition, i_holeToPlace), 0, 0);
                    holesToAdd = PlaceHoles(holesToPlace, startingPosition, i_holeToPlace, i_surfaceToPlaceOn.transform);
                }

                allHoles.AddRange(holesToAdd);
            }

            return allHoles;
        }

        private float CalculateOffset(int i_widthOfRow, Vector3 i_startingPosition, GameObject i_holeToPlace)
        {
            float amountOfMargins = i_widthOfRow - 1;
            float amountOfMarginsOnSide = amountOfMargins / 2;  // Used to calculate for offset
            float amountOfHoles = i_widthOfRow;
            float amountOfHolesOnSide = amountOfHoles / 2;      // Used to calculate for offset
            float marginWidth = (i_holeToPlace.transform.localScale.x * marginBetweenHoles); // Used to calculate for offset

            float sizeOfMarginsOnSide = amountOfMarginsOnSide * marginWidth;
            float sizeOfHolesOnSide = amountOfHolesOnSide * i_holeToPlace.transform.localScale.x;
            // Offset
            return sizeOfMarginsOnSide + sizeOfHolesOnSide;
        }

        private List<Hole> PlaceHoles(int i_amountToPlace, Vector3 i_startingPosition, GameObject i_holeToPlace, Transform i_parent)
        {
            List<Hole> placedHoles = new List<Hole>();
            // Place all holes in the row
            for (int j = 0; j < i_amountToPlace; j++)
            {
                GameObject hole = Instantiate(i_holeToPlace);
                hole.name = "Hole #" + j.ToString();
                hole.transform.position = i_startingPosition;
                hole.transform.SetParent(i_parent);

                placedHoles.Add(hole.AddComponent<Hole>());

                // Move over based on hole/margin width
                float widthOfOneHole = i_holeToPlace.transform.localScale.x;
                float widthOfMargin = i_holeToPlace.transform.localScale.x * marginBetweenHoles;
                i_startingPosition += new Vector3(widthOfOneHole + widthOfMargin, 0, 0);
            }
            return placedHoles;
        }
    }
}