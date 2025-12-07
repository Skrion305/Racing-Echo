using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    [SerializeField] GameObject[] cars;
    [SerializeField] GameObject[] checkpoints;
    [SerializeField] GameObject panel;
    [SerializeField] TMP_Text text;
    int minutes = 0;
    int seconds = 0;
    float timer = 0f;
    void Start()
    {
        if ((Settings.gameMode == "Гонка"))
        {
            Destroy(panel);
        }
        if ((Settings.gameMode == "На время"))
        {
            for (int i = 0; i < cars.Length; i++)
            {
                Destroy(cars[i]);
            }
            for (int i = 0; i < checkpoints.Length; i++)
            {
                Destroy(checkpoints[i]);
            }
        }
        if ((Settings.gameMode == "Свободный заезд"))
        {
            for (int i = 0; i < cars.Length; i++)
            {
                Destroy(cars[i]);
            }
            for (int i = 0; i < checkpoints.Length; i++)
            {
                Destroy(checkpoints[i]);
            }
            Destroy(panel);
        }
    }
    void Update()
    {
        if (Settings.gameMode == "На время")
        {
            timer += Time.deltaTime;
            seconds = (int) timer % 60;
            minutes = (int) timer / 60;
            text.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }
}
