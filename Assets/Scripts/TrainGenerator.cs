using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TrainGenerator : MonoBehaviour
{
    public bool TrainActive;
    public GameObject TrainExterior;
    public GameObject TrainInterior;
    public GameObject ExteriorPrefab;
    public GameObject InteriorPrefab;
    public GameObject EndPrefab;
    float BounceTimestamp;
    List<GameObject> ExteriorCars = new List<GameObject>();
    List<GameObject> InteriorCars = new List<GameObject>();
    public Vector2 ExteriorStart, ExteriorDistance, InteriorStart, InteriorDistance;

    public void CallTrain()
    {
        TrainExterior = FindObjectOfType<GameManager>().CurrentRoom.GetComponent<Room>().Train.gameObject;
        GenerateTrain();
        TrainExterior.transform.DOMoveX(0, 4.5f);
        TrainActive = true;
    }

    void GenerateTrain()
    {
        GameObject Instance;
        int Cars = Random.Range(2, 4);

        // generate exterior
        for (int i = 0; i < Cars; i++)
        {
            Instance = Instantiate(ExteriorPrefab, TrainExterior.transform);
            if (i == 0)
            {
                Instance.transform.localPosition = ExteriorStart;
            }
            else
            {
                float x = (ExteriorDistance * i).x;
                Instance.transform.localPosition = ExteriorStart + new Vector2(x, 0f);
            }
            ExteriorCars.Add(Instance);
        }

        // generate interior
        for (int i = 0; i < Cars; i++)
        {
            Instance = Instantiate(InteriorPrefab, TrainInterior.transform);
            if (i == 0)
            {
                Instance.transform.localPosition = InteriorStart;
            }
            else
            {
                float x = (InteriorDistance * i).x;
                Instance.transform.localPosition = InteriorStart + new Vector2(x, 0f);
            }
            InteriorCars.Add(Instance);
        }
        Instance = Instantiate(EndPrefab, InteriorCars[InteriorCars.Count - 1].transform);
        Instance.transform.localPosition = new Vector2(1.16f, 0f);
    }

    void Update()
    {
        if (TrainActive)
        {
            if (Time.time >= BounceTimestamp)
            {
                BounceTimestamp = Time.time + Random.Range(5f, 12f);

                int Rand = Random.Range(0, InteriorCars.Count);
                Debug.Log("Rand = " + Rand + " || Max = " + InteriorCars.Count);
                StartCoroutine(BounceCar(InteriorCars[Rand].transform));
            }
        }
    }
    
    IEnumerator BounceCar(Transform Car)
    {
        Car.DOLocalMoveY(0.03f, 0.05f);
        yield return new WaitForSeconds(0.05f);
        Car.DOLocalMoveY(0, 0.1f);
    }
}
