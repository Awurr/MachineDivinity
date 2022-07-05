using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public List<GameObject> Spawns = new List<GameObject>();
    public Transform UpperBound, LowerBound;
    public Transform Center;
    public Transform[] StaticSpawns;
    public bool Entered, Active;
    public Animator[] Doors;
    private bool Completed = false;
    public bool IgnoreIncrement;
    public GameObject RoomBoss;
    public Transform Train;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>())
        {
            FindObjectOfType<GameManager>().ChangeRoom(this);

            if (collision.GetComponent<Rigidbody2D>().velocity.y > 0)
            {
                collision.GetComponent<Player>().StartBoing();
            }

            EnterRoom();
        }
    }

    void Update()
    {
        // spawn enemies in each spawn location until it runs out
        // when all enemies in a group are killed, spawn a new group
        if (Train == null)
        {
            if (Active)
            {
                int RandomInt = Random.Range(0, StaticSpawns.Length);

                if (Spawns.Count == 0)
                {
                    if (!Completed && CheckComplete())
                    {
                        FindObjectOfType<GameManager>().ClearRoom();
                    }
                    return;
                }

                if (StaticSpawns.Length > 0)
                {
                    GameObject SpawnLocation = StaticSpawns[RandomInt].gameObject;
                    if (!SpawnLocation.GetComponent<SpawnLocation>().Full)
                    {
                        GameObject Instance = Instantiate(Spawns[0], SpawnLocation.transform.position, Quaternion.identity);
                        SpawnLocation.GetComponent<SpawnLocation>().Full = Instance;
                        Spawns.Remove(Spawns[0]);
                    }
                }
            }
        }
        else
        {
            if (!Completed)
            {
                CheckComplete();
            }
        }
    }

    public void EnterRoom()
    {
        if (Entered)
            return;

        StartCoroutine(CloseDoors());
        Entered = true;
        Active = true;

        if (RoomBoss != null)
        {
            StartCoroutine(SpawnBoss(5));
        }

        Spawns = FindObjectOfType<GameManager>().GenerateEnemies();
        FindObjectOfType<GameManager>().VisitedRooms++;
        FindObjectOfType<GameManager>().UpdateProgress();
    }

    public bool CheckComplete()
    {
        if (Train == null)
        {
            Completed = true;
            foreach (Transform SpawnLocation in StaticSpawns)
            {
                if (SpawnLocation.GetComponent<SpawnLocation>().Full)
                {
                    Completed = false;
                }
            }
        }
        else
        {
            if (RoomBoss != null)
            {
                Completed = false;
            }
            else
            {
                Completed = true;
            }
        }

        if (Completed)
        {
            foreach (Animator Anim in Doors)
            {
                Anim.Play("Open");
            }

            if (Train != null)
            {
                FindObjectOfType<TrainGenerator>().CallTrain();
            }

            return true;
        }

        return false;
    }

    IEnumerator CloseDoors()
    {
        yield return new WaitForSeconds(0.5f);

        foreach (Animator Anim in Doors)
        {
            Anim.Play("Close");
        }
    }

    IEnumerator SpawnBoss(float Delay)
    {
        yield return new WaitForSeconds(Delay);
        RoomBoss.SetActive(true);
    }
}