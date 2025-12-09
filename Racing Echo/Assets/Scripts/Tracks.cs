using UnityEngine;

public class Tracks : MonoBehaviour
{
    [SerializeField] GameObject[] objectsToDelete1;
    [SerializeField] GameObject[] objectsToDelete2;
    [SerializeField] GameObject[] objectsToDelete3;
    void Start()
    {
        if (Settings.raceTrack == 1)
        {
            for (int i = 0; i < objectsToDelete1.Length; i++)
            {
                Destroy(objectsToDelete1[i]);
            }
        }
        if (Settings.raceTrack == 2)
        {
            for (int i = 0; i < objectsToDelete2.Length; i++)
            {
                Destroy(objectsToDelete2[i]);
            }
        }
        if (Settings.raceTrack == 3)
        {
            for (int i = 0; i < objectsToDelete3.Length; i++)
            {
                Destroy(objectsToDelete3[i]);
            }
        }
    }
}
