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
    float raceTimer = 6f;
    int rt;
    [SerializeField] GameObject raceTimerPanel;
    [SerializeField] TMP_Text raceTimerText;
    float deleteTimer = 0f;
    bool start = false;
    [SerializeField] GameObject[] trafficLight;
    void Start()
    {
        if (Settings.gameMode == "Гонка")
        {
            GetComponent<CarControllerSample>().enabled = false;
            for (int i = 0; i < cars.Length; i++)
            {
                cars[i].GetComponent<AI>().enabled = false;
            }
            Destroy(timerPanel);
        }
        if (Settings.gameMode == "На время")
        {
            for (int i = 0; i < cars.Length; i++)
            {
                Destroy(cars[i]);
            }
            Destroy(raceTimerPanel);
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
            Destroy(raceTimerPanel);
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
        if ((Settings.gameMode == "Гонка") && (!start))
        {
            if (raceTimer >= 1)
            {
                raceTimer -= Time.deltaTime;
                rt = (int) raceTimer;
                raceTimerText.text = rt.ToString();
            }
            else
            {
                for (int i = 0; i < trafficLight.Length; i++)
                {
                    trafficLight[i].GetComponent<MeshRenderer>().material.color = Color.green;
                }
                raceTimerText.text = "Старт!";
                GetComponent<CarControllerSample>().enabled = true;
                for (int i = 0; i < cars.Length; i++)
                {
                    cars[i].GetComponent<AI>().enabled = true;
                }
                deleteTimer += Time.deltaTime;
                if (deleteTimer >= 3)
                {
                    Destroy(raceTimerPanel);
                    start = true;
                }
            }
        }
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
