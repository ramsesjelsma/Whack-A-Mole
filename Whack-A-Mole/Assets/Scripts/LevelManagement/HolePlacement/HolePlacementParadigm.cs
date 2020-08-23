// Copyright Ramses Jelsma, 2020

using HoleSystem;
using System.Collections.Generic;
using UnityEngine;

namespace LevelManagement
{
    public abstract class HolePlacementParadigm : ScriptableObject
    {
        public abstract List<Hole> Place(int holeAmount, GameObject holeToPlace, GameObject surfaceToPlaceOn);
    }
}
