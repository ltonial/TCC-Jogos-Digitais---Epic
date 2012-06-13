using UnityEngine;
using System.Collections;

public class LiftMovement : MonoBehaviour
{
    void Start()
    {
        animation.wrapMode = WrapMode.PingPong;
        animation.Play("LiftUpDown");
    }
}
