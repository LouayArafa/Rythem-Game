using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatDetector : MonoBehaviour
{
    [SerializeField] private bool START_DETECTING_BEAT = false;
    [SerializeField] private bool START_DETECTING_INPUT = false;

    [SerializeField] private float BPM;
    [SerializeField] private int length_ms;
    [SerializeField] private float beatLength;
    [SerializeField] private int numberOfParts;
    [SerializeField] private Scriptable Song;
    [SerializeField] private float BeatRange;
    [SerializeField] private float pressRange;

    [SerializeField] private List<int> playerInput;

    void Start()
    {
        //maths
        beatLength = (60 / BPM) * 1000;
        if(START_DETECTING_BEAT)
        {
            SplitAudioIntoParts();
        }
    }

    private void SplitAudioIntoParts()
    {
        numberOfParts = Mathf.CeilToInt(length_ms / beatLength);

        for (int i = 0; i < numberOfParts; i++)
        {
            
           
            Song.beats[i].startTime = (i * beatLength);
            Song.beats[i].startPressTime = (i * beatLength);

            
            Song.beats[i].ExactTime = i * beatLength;


            Song.beats[i].endTime = (i * beatLength) + BeatRange;
            Song.beats[i].endPressTime = (i * beatLength) + pressRange;
        }
    }
    private void Update()
    {
        if (START_DETECTING_INPUT) 
        { 
        
        }

    }
}
