// Copyright Ramses Jelsma, 2020

using System.Collections;
using UnityEngine;

namespace MoleSystem
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Mole Behaviour Wizard")]
    public class MoleBehaviourWizard : MoleBehaviour
    {
        protected override IEnumerator DelayAndGoDown()
        {
            float activeTime = UnityEngine.Random.Range(activeDurationRange.min, activeDurationRange.max);
            yield return new WaitForSeconds(activeTime / 2);

            // Warp
            holeManager.LeaveHole(activeHole);
            activeHole = holeManager.RequestAvailableHole();
            moleParent.transform.position = activeHole.transform.position;

            yield return new WaitForSeconds(activeTime / 2);
            yield return currentAnimation = moleParent.StartCoroutine(GoDownAnimation());
            currentAnimation = moleParent.StartCoroutine(DelayAndGoUp());
        }
    }
}