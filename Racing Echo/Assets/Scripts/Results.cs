using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Results : MonoBehaviour
{
    int racingPosition = 1;
    [SerializeField] GameModes player;
    [SerializeField] GameObject completing;
    [SerializeField] GameObject timerPanel;
    [SerializeField] TMP_Text result;
    int recordMinutes;
    int recordSeconds;
    bool end = false;
    float timer = 0f;
    void Update()
    {
        if (end)
        {
            timer += Time.deltaTime;
            if (timer >= 5)
            {
                SceneManager.LoadScene("Menu");
            }
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (Settings.gameMode == "Гонка")
        {
            if ((other.CompareTag("Car")) && (other.GetComponent<AI>().currentLap > 3))
            {
                other.GetComponent<AI>().racingPosition = racingPosition;
                racingPosition++;
            }
            if ((other.CompareTag("Player")) && (player.laps > 2))
            {
                other.GetComponent<GameModes>().racingPosition = racingPosition;
                end = true;
            }
        }
        if ((Settings.gameMode == "На время") && (other.CompareTag("Player")) && (player.laps > 2))
        {
            if (player.timer < Settings.record)
            {
                Settings.record = player.timer;
            }
            recordMinutes = (int) Settings.record / 60;
            recordSeconds = (int) Settings.record % 60;
            result.text = "Результат: " + string.Format("{0:00}:{1:00}", player.minutes, player.seconds) + "\n" + "Рекорд: " + string.Format("{0:00}:{1:00}", recordMinutes, recordSeconds);
            timerPanel.SetActive(false);
            completing.SetActive(true);
            end = true;
        }
    }
}
