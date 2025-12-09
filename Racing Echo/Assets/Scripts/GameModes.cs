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
    [SerializeField] GameObject[] checkpoints;
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
            for (int i = 0; i < checkpoints.Length; i++)
            {
                Destroy(checkpoints[i]);
            }
            Destroy(timerPanel);
            Destroy(finish);
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
        if (other.CompareTag("Finish"))
        {
            laps++;
        }
    }
}
