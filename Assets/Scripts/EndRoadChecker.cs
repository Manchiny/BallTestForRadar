using System;
using UnityEngine;

public class EndRoadChecker : MonoBehaviour
{
    private const string BALL_TAG = "Ball";
    public Action OnRoadEnded;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == BALL_TAG)
        {
            OnRoadEnded?.Invoke();
        }
    }
}
