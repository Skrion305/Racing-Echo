using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

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
    [SerializeField] GameObject[] cars;
    [SerializeField] List<GameObject> lastCars;
    string firstPosition;
    string secondPosition;
    string thirdPosition;
    string fourthPosition;
    string fifthPosition;
    string sixthPosition;
    string carName;
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
        if (GameData.gameMode == "Гонка")
        {
            if ((other.CompareTag("Car")) && (other.GetComponent<AI>().currentLap > 3))
            {
                other.GetComponent<AI>().racingPosition = racingPosition;
                lastCars.Remove(other.gameObject);
                racingPosition++;
            }
            if ((other.CompareTag("Player")) && (player.laps > 2))
            {
                other.GetComponent<GameModes>().racingPosition = racingPosition;
                if ((!GameData.achievement1) && (other.GetComponent<GameModes>().racingPosition == 1))
                {
                    GameData.achievement1 = true;
                }
                racingPosition++;
                lastCars.Sort((car1, car2) =>
                {
                    if (car1.GetComponent<AI>().currentLap != car2.GetComponent<AI>().currentLap)
                    {
                        return car2.GetComponent<AI>().currentLap.CompareTo(car1.GetComponent<AI>().currentLap);
                    }
                    if (car1.GetComponent<AI>().currentPoint != car2.GetComponent<AI>().currentPoint)
                    {
                        return car2.GetComponent<AI>().currentPoint.CompareTo(car1.GetComponent<AI>().currentPoint);
                    }
                    return car1.GetComponent<AI>().racingPosition.CompareTo(car2.GetComponent<AI>().racingPosition);
                });
                foreach (var car in lastCars)
                {
                    car.GetComponent<AI>().racingPosition = racingPosition;
                    racingPosition++;
                }
                for (int i = 0; i < cars.Length; i++)
                {
                    switch (i)
                    {
                        case 0:
                            carName = "Player";
                            break;
                        case 1:
                            carName = "Blue";
                            break;
                        case 2:
                            carName = "Red";
                            break;
                        case 3:
                            carName = "Green";
                            break;
                        case 4:
                            carName = "Yellow";
                            break;
                        case 5:
                            carName = "White";
                            break;
                    }
                    if (cars[i].CompareTag("Player"))
                    {
                        switch (cars[i].GetComponent<GameModes>().racingPosition)
                        {
                            case 1:
                                firstPosition = "1   " + carName;
                                break;
                            case 2:
                                secondPosition = "2   " + carName;
                                break;
                            case 3:
                                thirdPosition = "3   " + carName;
                                break;
                            case 4:
                                fourthPosition = "4   " + carName;
                                break;
                            case 5:
                                fifthPosition = "5   " + carName;
                                break;
                            case 6:
                                sixthPosition = "6   " + carName;
                                break;
                        }
                    }
                    else
                    {
                        switch (cars[i].GetComponent<AI>().racingPosition)
                        {
                            case 1:
                                firstPosition = "1   " + carName;
                                break;
                            case 2:
                                secondPosition = "2   " + carName;
                                break;
                            case 3:
                                thirdPosition = "3   " + carName;
                                break;
                            case 4:
                                fourthPosition = "4   " + carName;
                                break;
                            case 5:
                                fifthPosition = "5   " + carName;
                                break;
                            case 6:
                                sixthPosition = "6   " + carName;
                                break;
                        }
                    }
                }
                result.text = firstPosition + "\n" + secondPosition + "\n" + thirdPosition + "\n" + fourthPosition + "\n" + fifthPosition + "\n" + sixthPosition;
                completing.SetActive(true);
                end = true;
            }
        }
        if ((GameData.gameMode == "На время") && (other.CompareTag("Player")) && (player.laps > 2))
        {
            if (player.timer < GameData.record)
            {
                GameData.record = player.timer;
            }
            if ((!GameData.achievement2) && (GameData.record <= 180f))
            {
                GameData.achievement2 = true;
            }
            recordMinutes = (int) GameData.record / 60;
            recordSeconds = (int) GameData.record % 60;
            result.text = "Результат: " + string.Format("{0:00}:{1:00}", player.minutes, player.seconds) + "\nРекорд: " + string.Format("{0:00}:{1:00}", recordMinutes, recordSeconds);
            timerPanel.SetActive(false);
            completing.SetActive(true);
            end = true;
        }
    }
}
