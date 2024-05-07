
#region Libraries
using UnityEngine;
using FMODUnity;
using static UnityEngine.ParticleSystem;
using System.Collections.Generic;
using UnityEngine.UIElements;
using System.Collections;
using System;
using static Scriptable;
using Unity.VisualScripting;
using UnityEngine.UI;
#endregion


public class FMODTimeReader : MonoBehaviour
{
#region ---VARIABLES---
   
    
    #region Randoms
    //Event of player input
    public static event Action Pulse;
    //Scriptable for songs
    [SerializeField] private Scriptable Song;
    //animator
    [SerializeField] private Animator animator;
    //Indexs
    public int x = 0, y = 0;
    [Space(15)]
    #endregion


    #region FMOD Settings
    [Header("FMOD Settings")]
    public FMODUnity.EventReference fmodEvent;
    private FMOD.Studio.EventInstance eventInstance;
    private FMOD.Studio.PLAYBACK_STATE playbackState;
    [Space(15)]
    #endregion


    #region     playback_State
    [Header("playback State")]
    [SerializeField] private int timelinePosition = 0;
    [Space]
    public bool Idle;
    public bool WaitingForInput;
    public bool PressTime;
    public bool BeatTime = false;
    bool beatTime_flag = false;
    bool pressTime_flag = false;
    bool idle_flag = false;
    [Space(15)]
    #endregion


    #region Song Stats
    [Header("Song Stats")]
    [SerializeField] private float BPM;
    [SerializeField] private int length_ms;
    //[SerializeField] private float frequency;
    //[SerializeField] private float cycle  ;
    //[SerializeField] private float positionInBeat;
    [SerializeField] private float beatLength;
    [SerializeField] private float BeatRange;
    #endregion

     
    
    #region class LIST
    //List of Class
    [System.Serializable]
    public class BeatInfo
    {
        public float ExactTime;
        public float startTime;
        public float endTime;
        public float startPressTime;
        public float endPressTime;
    }
    [Space(15)]
    public List<BeatInfo> beatList = new List<BeatInfo>();
    [Space(15)]
    #endregion


    #region Visual Feedback
    [Header("Visual feedback")]
    [SerializeField] private float visualsOffset = 500;
    [SerializeField] private GameObject visualCue;
    [SerializeField] private Transform visualCueCenter;
    [SerializeField] private Transform visualCuePos;
    [SerializeField] private float visualCueDistance;
    [Space(15)]
    #endregion

    
    
    #region calculation
    //calculation
    [SerializeField] private float perfectRange;
    [SerializeField] private float goodRange;
    [SerializeField] private float poorRange;
    #endregion

#endregion
    
    
    
  
    
    private void Start()
    {
        // Load FMOD event and Start it
        eventInstance = FMODUnity.RuntimeManager.CreateInstance(fmodEvent);
        eventInstance.start();
        
        //Getting the used beats
        for (int i = 0; i < Song.beats.Count; i++)
        {
            if (Song.beats[i].active)
            {
                BeatInfo newBeat = new BeatInfo
                {
                    ExactTime = (Song.beats[i].ExactTime),
                    startTime = (Song.beats[i].startTime),
                    endTime = (Song.beats[i].endTime),
                    startPressTime = (Song.beats[i].startPressTime),
                    endPressTime = (Song.beats[i].endPressTime),
                };
                beatList.Add(newBeat);
            }
        }
    
    }





    private void Update()
    {
        //Update timelinePos every Frame
        eventInstance.getTimelinePosition(out timelinePosition);

        ///if it's beat time
        ///pulse then test for next beat in the list
        if (timelinePosition <= beatList[x].endTime && timelinePosition >= beatList[x].startTime && !BeatTime)
        {
            Debug.Log(timelinePosition);
            x = Increment(x);
            BeatTime = true;
        }

        /// if it's input time check
        /// input or missed in time then next beat
        /// if input in wrong time do smth
        if (timelinePosition <= beatList[y].endPressTime && timelinePosition >= beatList[y].startPressTime && !WaitingForInput)
        {        
                WaitingForInput = true;
        }
    }
    
    private void LateUpdate()
    {
        if(BeatTime)
        {
            animator.SetTrigger("Pulse");
            BeatTime = false;

        }

        //if player missed the beat
        if (timelinePosition > beatList[y].endPressTime && WaitingForInput)
        {
           Debug.Log("Missed");
           y=  Increment(y);
           WaitingForInput = false;
        }
    }

   
    
    
    
    
    #region CUSTOM METHODES
    
    //Incrementing a variable and loop it around number of the beats in the song
    private int Increment(int w)
    {
        return w = (w + 1) % beatList.Count;
    }


    #region Observer Pattren
    private void OnEnable()
    {
        PlayerInput.press += Action;
    }
    private void OnDisable()
    {
        PlayerInput.press -= Action;

    }
    private void Action()
    {
        if (WaitingForInput)
        {
            Debug.Log("Perfect Hit " + timelinePosition);
            WaitingForInput = false;
            y = Increment(y);
        }
        else
        {
            Debug.Log("Nope");
        }
    }
    #endregion



    #endregion




}



#region TESTING WORK
/* void LateUpdate()
    {
        //eventInstance.getPlaybackState(out playbackState);
        if (true)//playbackState == FMOD.Studio.PLAYBACK_STATE.PLAYING)
        {
            eventInstance.getTimelinePosition(out timelinePosition);

            if (timelinePosition < beatList[x] + 100 && timelinePosition > beatList[x])
            {
                //print(timelinePosition);
                if(true)//Input.GetMouseButton(0))
                {
                   // print(timelinePosition);

                    animator.SetTrigger("Pulse");
                    Pulse?.Invoke() ;
                    x++;
                    if (x == beatList.Count)
                    {
                        x = 0;
                    }
                }
                
            }
          

        }

    }

    public void Press()
    {
        int PressTime = timelinePosition;
        int w = x;
        print(PressTime);
        print(beatList[w]);   
        print(Mathf.Abs(beatList[w] - PressTime));
        if (Mathf.Abs( PressTime - beatList[w])< perfectRange)
        {
            print ("Perfect");
        }
    }

}









/* void Start()
 {
     // Load FMOD event
     eventInstance = FMODUnity.RuntimeManager.CreateInstance(fmodEvent);

     // Start the event
     eventInstance.start();
     //maths
     beatLength = (60 / BPM);
     cycle = frequency * beatLength;



 }

 void OnDestroy()
 {
     // Release FMOD resources when the script is destroyed
     eventInstance.release();
     RuntimeManager.StudioSystem.release();
 }

 void Update()
 {
     eventInstance.getPlaybackState(out playbackState);

     if (playbackState == FMOD.Studio.PLAYBACK_STATE.PLAYING)
     {
         int timelinePosition;
         eventInstance.getTimelinePosition(out timelinePosition);

     }
 }
 */
#endregion