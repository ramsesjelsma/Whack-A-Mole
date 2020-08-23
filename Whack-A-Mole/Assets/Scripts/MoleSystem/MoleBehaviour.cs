// Copyright Ramses Jelsma, 2020

using HoleSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoleSystem
{
    /// <summary>
    /// Base class for any mole behaviour.
    /// Turn any necessary method into protected to make it available to possible children.
    /// </summary>
    [CreateAssetMenu(menuName = "ScriptableObjects/Mole Behaviour")]
    public class MoleBehaviour : ScriptableObject
    {
        [System.Serializable]
        public struct FloatRange
        {
            public float min, max;

            public FloatRange(float min, float max) { this.min = min; this.max = max; }
        }

        public GameObject molePrefab;
        public FloatRange activeDurationRange;
        public FloatRange cooldownDurationRange;
        public float popupAnimDuration;
        public float downAnimDuration;
        public int hp;
        public int scoreOnKill;

        private int currentHP;
        private Renderer[] renderers;
        private List<Material> mats = new List<Material>();
        private List<Color> startCol = new List<Color>();
        protected Coroutine currentAnimation; // Manages the current animation, so it can cancel when it's hit
        protected Mole moleParent;
        protected HoleManager holeManager;
        protected Hole activeHole;

        /// <summary>
        /// Sets up the mole for use, has to be called for the cycle of animations to start (pop up, go down, delay, etc).
        /// </summary>
        public void Setup(Mole i_mole, HoleManager i_holeManager)
        {
            moleParent = i_mole;
            SwitchVisible(false);
            holeManager = i_holeManager;
            currentHP = hp;
            Renderer[] renderers = moleParent.GetComponentsInChildren<Renderer>();
            for (int i = 0; i < renderers.Length; i++)
            {
                mats.Add(renderers[i].material);
                startCol.Add(renderers[i].material.color);
            }
            currentAnimation = moleParent.StartCoroutine(DelayAndGoUp());
        }

        #region MoleHitting
        public bool HitMole(int i_damage)
        {
            currentHP -= i_damage;
            moleParent.StartCoroutine(DamageVisuals());
            if (currentHP <= 0)
            {
                moleParent.GetComponentInChildren<Collider>().enabled = false;
                moleParent.StopCoroutine(currentAnimation);
                moleParent.StartCoroutine(MoleRetreatOnHitAnimation());
                return true;
            }
            else
            {
                return false;
            }
        }

        private IEnumerator DamageVisuals()
        {
            float val = 0;

            while (val < 1)
            {
                for (int i = 0; i < mats.Count; i++)
                {
                    mats[i].color = Color.Lerp(startCol[i], Color.red, val);
                }

                val += Time.deltaTime / 0.25f;
                yield return new WaitForEndOfFrame();
            }
            val = 0;
            while (val < 1)
            {
                for (int i = 0; i < mats.Count; i++)
                {
                    mats[i].color = Color.Lerp(Color.red, startCol[i], val);
                }

                val += Time.deltaTime / 0.25f;
                yield return new WaitForEndOfFrame();
            }


            for (int i = 0; i < mats.Count; i++)
            {
                mats[i].color = startCol[i];
            }
        }

        private IEnumerator MoleRetreatOnHitAnimation()
        {
            yield return currentAnimation = moleParent.StartCoroutine(GoDownAnimation());
            currentAnimation = moleParent.StartCoroutine(DelayAndGoUp());
        }

        #endregion

        #region StandardMoleCycle

        protected IEnumerator DelayAndGoUp()
        {
            currentHP = hp;
            float waitingTime = UnityEngine.Random.Range(cooldownDurationRange.min, cooldownDurationRange.max);
            yield return new WaitForSeconds(waitingTime);
            currentAnimation = currentAnimation = moleParent.StartCoroutine(PopUpAnimation((bool aHoleWasFound) =>
            {
                if (aHoleWasFound)
                {
                    currentAnimation = moleParent.StartCoroutine(DelayAndGoDown()); // Go down after Success!
                }
                else
                {
                    currentAnimation = moleParent.StartCoroutine(DelayAndGoUp()); // Reset if can't start popupanimation
                }
            }));
        }

        protected virtual IEnumerator DelayAndGoDown()
        {
            float activeTime = UnityEngine.Random.Range(activeDurationRange.min, activeDurationRange.max);
            yield return new WaitForSeconds(activeTime);
            yield return currentAnimation = moleParent.StartCoroutine(GoDownAnimation());
            currentAnimation = moleParent.StartCoroutine(DelayAndGoUp());
        }

        #endregion

        #region Animations
        /// <summary>
        /// Called by DelayAndGoUp to see if it can actually go up
        /// </summary>
        private IEnumerator PopUpAnimation(Action<bool> succeeded)
        {
            activeHole = holeManager.RequestAvailableHole();
            if (activeHole == null)
            {
                succeeded(false);
            }
            else
            {
                moleParent.transform.position = activeHole.transform.position - new Vector3(0, moleParent.transform.localScale.y, 0);
                SwitchVisible(true);

                yield return currentAnimation = moleParent.StartCoroutine(AnimateToTarget(moleParent.transform.position + new Vector3(0, moleParent.transform.localScale.y, 0), popupAnimDuration));

                succeeded(true);
            }

        }

        protected IEnumerator GoDownAnimation()
        {
            yield return currentAnimation = moleParent.StartCoroutine(AnimateToTarget(moleParent.transform.position - new Vector3(0, moleParent.transform.localScale.y, 0), downAnimDuration));
            holeManager.LeaveHole(activeHole);
            activeHole = null;
            SwitchVisible(false);
        }
        #endregion

        #region Util
        private IEnumerator AnimateToTarget(Vector3 i_targetPos, float i_duration)
        {
            Vector3 startPos = moleParent.transform.position;
            float i = 0;

            while (i < 1)
            {
                moleParent.transform.position = Vector3.Lerp(startPos, i_targetPos, i);

                i += Time.deltaTime / i_duration;
                yield return new WaitForEndOfFrame();
            }

            moleParent.transform.position = i_targetPos;
        }

        private void SwitchVisible(bool i_visible)
        {
            Renderer[] renderers = moleParent.GetComponentsInChildren<Renderer>();
            Collider[] colliders = moleParent.GetComponentsInChildren<Collider>();

            for (int i = 0; i < renderers.Length; i++)
            {
                renderers[i].enabled = i_visible;
            }
            for (int i = 0; i < colliders.Length; i++)
            {
                colliders[i].enabled = i_visible;
            }
        }
        #endregion
    }
}