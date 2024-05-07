
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSong", menuName = "Song")]
public class Scriptable : ScriptableObject
{
    [TextArea(5,10)]
    public string description;
    
    [System.Serializable]
    public class Beat
    {
        public float ExactTime;
        public float startTime;
        public float endTime;
        public float startPressTime;
        public float endPressTime;
        public bool active;
    }

    public List<Beat> beats = new List<Beat>();

}
