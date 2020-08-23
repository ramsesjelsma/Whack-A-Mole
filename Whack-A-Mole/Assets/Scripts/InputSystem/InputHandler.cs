// Copyright Ramses Jelsma, 2020

using LevelManagement;
using System.Collections.Generic;
using UnityEngine;

namespace InputSystem
{
    /// <summary>
    /// Input handler takes all the inputs and then fires events. It doesn't decide what happens with the inputs.
    /// </summary>
    public class InputHandler : EventMonoBehaviourBase
    {
        public delegate void TouchStartHandler(Touch touch, Vector2 pos);
        public static event TouchStartHandler TouchStart;

        public delegate void TouchMoveHandler(Touch touch, Vector2 newPos, Vector2 direction);
        public static event TouchMoveHandler TouchMove;

        public delegate void TouchEndHandler(Touch touch);
        public static event TouchEndHandler TouchEnd;


        private List<Touch> allTouches = new List<Touch>();
        private Dictionary<Touch, Vector2> startPosByTouch = new Dictionary<Touch, Vector2>();

        private void Start()
        {
            LevelManager.LevelEndsEvent += OnLevelEnd;
        }

        private void Update()
        {
            if (Input.touchCount > 0)
            {
                for (int i = 0; i < Input.touchCount; i++)
                {
                    Touch touch = Input.GetTouch(i);
                    if (allTouches.Contains(touch) == false)
                    {
                        allTouches.Add(touch);
                        startPosByTouch.Add(touch, touch.position);
                    }

                    FireTouchEvent(touch, startPosByTouch[touch]);
                }
            }
        }

        private void FireTouchEvent (Touch i_touch, Vector2 i_startPos = default)
        {
            switch (i_touch.phase)
            {
                case TouchPhase.Began:
                    TouchStart?.Invoke(i_touch, i_touch.position);
                    break;

                case TouchPhase.Moved:
                    TouchMove?.Invoke(i_touch, i_touch.position, i_touch.position - i_startPos);
                    break;

                case TouchPhase.Ended:
                    TouchEnd?.Invoke(i_touch);
                    startPosByTouch.Remove(i_touch);
                    allTouches.Remove(i_touch);
                    break;
            }
        }

        public override void OnLevelEnd()
        {
            if (TouchStart != null)
            {
                foreach (TouchStartHandler d in TouchStart.GetInvocationList())
                {
                    TouchStart -= (TouchStartHandler)d;
                }
            }
            if (TouchMove != null)
            {
                foreach (TouchMoveHandler d in TouchMove.GetInvocationList())
                {
                    TouchMove -= (TouchMoveHandler)d;
                }
            }
            if (TouchEnd != null)
            {
                foreach (TouchEndHandler d in TouchEnd.GetInvocationList())
                {
                    TouchEnd -= (TouchEndHandler)d;
                }
            }
        }
    }
}
