// Copyright Ramses Jelsma, 2020

using LevelManagement;
using MoleSystem;
using UnityEngine;

namespace InputSystem
{
    /// <summary>
    /// The MoleHitManager takes inputs and lets you hit holes. Can be easily expanded on.
    /// </summary>
    public class MoleHitManager : EventMonoBehaviourBase
    {
        public static event ScoreManager.ScoreAddHandler OnMoleHit;
        public static event ScoreManager.ScoreAddHandler OnMoleMiss;
        [Header("Tweakables")]
        public int moleMissScore = -50;
        public int damage = 1;
        [Range(0.01f, 0.1f)]
        public float tapRadius = 0.1f;
        [Header("References")]
        public GameObject particleSystemOnTap;

        private void Awake()
        {
            InputHandler.TouchStart += TouchStart;
            // Could add other events to i.e. do different things on long press. TouchMove / TouchEnd.
            LevelManager.LevelEndsEvent += OnLevelEnd;
        }

        // DEBUG CODE
        private void Update()
        {
#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                GameObject ps = Instantiate(particleSystemOnTap);
                ps.transform.position = ray.origin + new Vector3(0, 0, 0.05f);

                if (Physics.SphereCast(ray, tapRadius, out RaycastHit hit))
                {
                    Mole target = hit.transform.GetComponentInParent<Mole>();
                    if (target != null)
                    {
                        if (target.behaviour.HitMole(damage))
                        {
                            OnMoleHit?.Invoke(target.behaviour.scoreOnKill);
                        }
                    }
                    else
                    {
                        OnMoleMiss?.Invoke(moleMissScore);
                    }
                }
                else
                {
                    OnMoleMiss?.Invoke(moleMissScore);
                }
            }
#endif
        }

        private void TouchStart(Touch touch, Vector2 pos)
        {
            Ray ray = Camera.main.ScreenPointToRay(touch.position);

            GameObject ps = Instantiate(particleSystemOnTap);
            ps.transform.position = ray.origin + new Vector3(0, 0, 0.05f);

            if (Physics.SphereCast(ray, tapRadius, out RaycastHit hit))
            {
                Mole target = hit.transform.GetComponentInParent<Mole>();
                if (target != null)
                {
                    target.behaviour.HitMole(damage);
                    OnMoleHit?.Invoke(target.behaviour.scoreOnKill);
                }
                else
                {
                    OnMoleMiss?.Invoke(moleMissScore);
                }
            }
            else
            {
                OnMoleMiss?.Invoke(moleMissScore);
            }
        }

        public override void OnLevelEnd()
        {
            foreach (ScoreManager.ScoreAddHandler d in OnMoleHit.GetInvocationList())
            {
                OnMoleHit -= (ScoreManager.ScoreAddHandler)d;
            }
            foreach (ScoreManager.ScoreAddHandler d in OnMoleMiss.GetInvocationList())
            {
                OnMoleMiss -= (ScoreManager.ScoreAddHandler)d;
            }

            this.enabled = false;
        }
    }
}