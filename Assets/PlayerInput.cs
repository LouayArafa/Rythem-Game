using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerInput : MonoBehaviour
{
    public static event Action press;

    public void Press()
    {
        press?.Invoke();
    }
}
