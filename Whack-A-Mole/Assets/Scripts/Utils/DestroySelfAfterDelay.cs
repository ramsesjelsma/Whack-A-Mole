// Copyright Ramses Jelsma, 2020

using System.Collections;
using UnityEngine;

public class DestroySelfAfterDelay : MonoBehaviour
{
    [Header("Tweakables")]
    public float delay;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroySelf(delay));
    }

    private IEnumerator DestroySelf(float i_delay)
    {
        yield return new WaitForSeconds(i_delay);
        Destroy(this.gameObject);
    }
}
