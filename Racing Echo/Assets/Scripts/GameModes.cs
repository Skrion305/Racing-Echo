using UnityEngine;
using TMPro;

public class GameModes : MonoBehaviour
{
    [SerializeField] GameObject[] cars;
    [SerializeField] GameObject timerPanel;
    [SerializeField] TMP_Text timerText;
    public int minutes = 0;
    public int seconds = 0;
    public float timer = 0f;
    public int laps = 0;
    public int racingPosition;
    [SerializeField] GameObject finish;
    [SerializeField] GameObject firstTrackCheckpoints;
    [SerializeField] GameObject secondTrackCheckpoints;
    [SerializeField] GameObject thirdTrackCheckpoints;
    Transform[] waypoints;
    [SerializeField] Transform[] checkpoints1;
    [SerializeField] Transform[] checkpoints2;
    [SerializeField] Transform[] checkpoints3;
    int currentPoint = 0;
    void Start()
    {
        if (Settings.gameMode == "Гонка")
        {
            Destroy(timerPanel);
        }
        if (Settings.gameMode == "На время")
        {
            for (int i = 0; i < cars.Length; i++)
            {
                Destroy(cars[i]);
            }
        }
        if (Settings.gameMode == "Свободный заезд")
        {
            for (int i = 0; i < cars.Length; i++)
            {
                Destroy(cars[i]);
            }
            Destroy(firstTrackCheckpoints);
            Destroy(secondTrackCheckpoints);
            Destroy(thirdTrackCheckpoints);
            Destroy(timerPanel);
            Destroy(finish);
        }
        if (Settings.gameMode != "Свободный заезд")
        {
            if (Settings.raceTrack == 1)
            {
                waypoints = checkpoints1;
                Destroy(secondTrackCheckpoints);
                Destroy(thirdTrackCheckpoints);
            }
            if (Settings.raceTrack == 2)
            {
                waypoints = checkpoints2;
                Destroy(firstTrackCheckpoints);
                Destroy(thirdTrackCheckpoints);
            }
            if (Settings.raceTrack == 3)
            {
                waypoints = checkpoints3;
                Destroy(firstTrackCheckpoints);
                Destroy(secondTrackCheckpoints);
            }
        }
    }
    void Update()
    {
        if (Settings.gameMode == "На время")
        {
            timer += Time.deltaTime;
            seconds = (int) timer % 60;
            minutes = (int) timer / 60;
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (((other.CompareTag("Checkpoint")) || (other.CompareTag("Finish"))) && (other.gameObject == waypoints[currentPoint].gameObject))
        {
            currentPoint++;
            if (currentPoint >= waypoints.Length)
            {
                currentPoint = 0;
                laps++;
            }
        }
    }
}
