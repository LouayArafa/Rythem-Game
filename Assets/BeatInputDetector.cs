using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class BeatInputDetector : MonoBehaviour
{
    //[SerializeField] private float beatTolerance = 100f;
    [SerializeField] private float perfectTolerance = 50f;
    [SerializeField] private EventReference fmodEvent;
    [SerializeField] private Scriptable song; // Reference to the Scriptable Object containing beat information
    public Animator animator;
    private EventInstance eventInstance; // FMOD Event Instance
    private int currentBeatIndex = 0; // Index of the current beat being checked
    private bool beatStarted = false; // Flag to track if beat detection has started
    private bool eventStart = false;

    private void Start()
    {
        // Initialize FMOD event instance
        eventInstance = RuntimeManager.CreateInstance(fmodEvent);
    }

    private void Update()
    {
        // Check if beat detection has started
        if (!beatStarted && eventInstance.isValid())
        {
            beatStarted = true;

            InvokeRepeating("CheckPlayerInput", 1f, 0.005f);
        }
    }

    private void CheckPlayerInput()
    {
        // Start the song
        if (!eventStart)
        {
            eventStart = true;
            eventInstance.start();
        }

        // Get the current time of the FMOD music event in milliseconds
        eventInstance.getTimelinePosition(out int timelinePosition);
        float currentTime = timelinePosition;

        // Get the expected time for the current beat
        float expectedBeatTime = song.beats[currentBeatIndex].ExactTime;

        // Get the time for the current beat
        float EndBeatTime = song.beats[currentBeatIndex].endTime;
        float StartBeatTime = song.beats[currentBeatIndex].startTime;
        float ExactBeatTime = song.beats[currentBeatIndex].ExactTime;
        float NextBeatEndTime = song.beats[(currentBeatIndex + 1) % song.beats.Count].endTime;
        float NextBeatStartTime = song.beats[(currentBeatIndex + 1) % song.beats.Count].startTime;

        // Calculate the difference between current Time and expected beat time
        float timeDifference = Mathf.Abs(currentTime - expectedBeatTime);


        if (Input.GetKeyDown(KeyCode.Space))
        {

        
        #region On Beat
        // Check for missed beats
       
        if (currentBeatIndex == 0 
            && (song.Length_ms - perfectTolerance < currentTime && currentTime > NextBeatEndTime)
                || (currentTime > ExactBeatTime + perfectTolerance && currentTime < NextBeatEndTime))
        // in the last beat in the list: if current time is still didn't loop and we enter the perfect tolerance intervale
        // OR if current time looped and we are in the interva of perfect beat
        {

                if (song.beats[currentBeatIndex].active)
                {
                    Debug.Log("0 - Perfect Beat! Current Time: " + currentTime + ", Expected Beat Time: " + expectedBeatTime);

                    animator.SetTrigger("Pulse");



                    // Increment the beat index to check the next beat
                    currentBeatIndex = (currentBeatIndex + 1) % song.beats.Count;
                }
            
        }
        else if (currentTime > ExactBeatTime - perfectTolerance && ExactBeatTime + perfectTolerance > currentTime)
        {
          

                if (song.beats[currentBeatIndex].active)
                {
                    Debug.Log("Index: " + currentBeatIndex + "- Perfect Beat! Current Time: " + currentTime + ", Expected Beat Time: " + expectedBeatTime);

                    animator.SetTrigger("Pulse");
                    // Increment the beat index to check the next beat

                    currentBeatIndex = (currentBeatIndex + 1) % song.beats.Count;

                }
              
            
        }
        else
            Debug.Log("Index: " + currentBeatIndex + "- OFF BEAT! Current Time: " + currentTime + ", Expected Beat Time: " + expectedBeatTime);
            #endregion

        }
        #region Missed Beats
        // Check for missed beats
        if (currentBeatIndex == 0)
        {


            if (currentTime > EndBeatTime && currentTime < NextBeatStartTime)// we added the second condition to ensure correct calculation when the beats loops
            {

                if (song.beats[currentBeatIndex].active)
                {
                    Debug.Log(0 + "- Missed Beat! Current Time: " + currentTime + ", Expected Beat Time: " + expectedBeatTime);

                }

                // Increment the beat index to check the next beat
                currentBeatIndex = (currentBeatIndex + 1) % song.beats.Count;

            }
        }
        else
        {
            if (currentTime > EndBeatTime && currentTime< NextBeatStartTime)
            {



                if (song.beats[currentBeatIndex].active)
                {
                    Debug.Log("Index: " + currentBeatIndex + "- Missed Beat! Current Time: " + currentTime + ", Expected Beat Time: " + expectedBeatTime);

                }
                // Increment the beat index to check the next beat

                currentBeatIndex = (currentBeatIndex + 1) % song.beats.Count;

            }
        }
        #endregion
    }


}




/*
    if (Input.GetKeyDown(KeyCode.Space))
    {
        // Check if the player input is within tolerance of the beat
        if (timeDifference <= beatTolerance)
        {
            if (timeDifference <= perfectTolerance)
            {
                Debug.Log("Perfect Timing!");
            }
            else
            {
                Debug.Log("Good Timing!");
            }

            currentBeatIndex = (currentBeatIndex + 1) % song.beats.Count;
        }
        else
        {
            Debug.Log("Off Beat...");
        }
    }*/
