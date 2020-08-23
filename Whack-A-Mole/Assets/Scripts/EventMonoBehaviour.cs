// Copyright Ramses Jelsma, 2020

using UnityEngine;

/// <summary>
/// Base class for monobehaviors with static events, the idea is that you always manage these static variables on level end.
/// </summary>

    // My initial idea for this class was to use reflection to locate all static events and unsubscrube everything from them, unfortunately this proved
    // to be too complex to create given the time limit. Though I normally don't support the use of reflection, this seemed to be like a rare
    // opportunity to use it correctly. :)
public abstract class EventMonoBehaviourBase : MonoBehaviour
{
    /// <summary>
    /// Every monobehavior with a static event needs to manage it, when scene gets reloaded
    /// </summary>
    public abstract void OnLevelEnd();
}
