using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public bool DebugEnabled;
    public enum Character
    {
        Human
    }
    public enum DeathType
    {
        Default,
        SmallProj,
        LargeProj,
        Worms,
        Bleed
    }
    Character PlayedCharacter;
    DeathType LatestDeath;

    [Header("Game Info")]
    public float Difficulty = 1;
    public float DifficultyScale = 1;
    public int Kills = 0;
    [HideInInspector] public Room CurrentRoom;
    public EnemyPool CurrentEnemyPool;
    public RoomPool CurrentRoomPool;
    public int RoomsToGenerate, VisitedRooms;
    public List<GameObject> GeneratedRooms;
    public GameObject Background;
    public Material[] BackgroundMaterials;

    [Header("Chances")]
    public float ItemChance;
    public float UnusualChance;
    public float EsotericChance;
    public float CommandChance;
    public float RevelationChance;

    [Header("Spawnables")]
    public GameObject TypicalSpawn;
    public GameObject UnusualSpawn;
    public GameObject EsotericSpawn;
    public GameObject CommandSpawn;
    public GameObject RevelationSpawn;

    [Header("UI - Health")]
    public Transform HealthChunkHolder;
    public GameObject HealthChunk;
    List<GameObject> HealthChunks = new List<GameObject>();
    public Sprite HealthFullSprite, HealthEmptySprite;
    public float HealthConst;
    public Animator HeartAnimator;
    float HeartSpeed;

    [Header("UI - Progress")]
    public Transform ProgressChunkHolder;
    public GameObject ProgressChunk;
    List<GameObject> ProgressChunks = new List<GameObject>();
    public Sprite ProgressFullSprite, ProgressEmptySprite;
    public float ProgressConst;
    public Text DifficultyText;
    public Text KillsText;

    [Header("UI - Items")]
    public Image[] Knobs;
    public int[] KnobStates;
    public Sprite[] KnobSprites;
    public Transform PickupText;
    public Color[] ItemColors;
    public Sprite[] ItemTextTypes;
    public enum ItemTypes
    {
        Typical,
        Unusual,
        Esoteric,
        Command,
        Revelation
    }

    void Start()
    {
        StartGame(Character.Human);
    }

    private void Update()
    {
        UpdateKills();
        if (DebugEnabled)
            DebugMode();
        HeartAnimator.SetFloat("Speed", HeartSpeed);
    }

    public void StartGame(Character Char)
    {
        PlayedCharacter = Char;
        CheckVars();
        GenerateLevel();
        UpdateProgress();
        UpdateKills();
    }

    public void EndGame(DeathType Death)
    {
        LatestDeath = Death;
    }

    public void ChangeRoom(Room RoomChange)
    {
        CurrentRoom = RoomChange;
        FindObjectOfType<CameraFollow>().SetCameraLimits(RoomChange);

        Background.transform.position = RoomChange.transform.position;
    }

    public void UpdateHealth()
    {
        int MaxHealth = (int)FindObjectOfType<Player>().Profile.GetStat(PlayerProfile.StatKey.MaxHealth);
        int CurrentHealth = FindObjectOfType<Player>().Health;
        if (CurrentHealth > 0)
        {
            HeartSpeed = ((MaxHealth - CurrentHealth) / 2) + 1;
        }
        else
        {
            DOTween.To(() => HeartSpeed, x => HeartSpeed = x, 0f, 5);
        }
        UpdateBar(MaxHealth, CurrentHealth, HealthChunkHolder, HealthChunk, HealthChunks, HealthFullSprite, HealthEmptySprite, HealthConst);
    }

    public void UpdateProgress()
    {
        UpdateBar(RoomsToGenerate, VisitedRooms, ProgressChunkHolder, ProgressChunk, ProgressChunks, ProgressFullSprite, ProgressEmptySprite, ProgressConst);
    }

    void UpdateDifficulty()
    {
        DifficultyText.text = " " + Difficulty;
    }

    public void MoveBGToPlayer()
    {
        Background.transform.position = FindObjectOfType<Player>().transform.position;
    }

    public void SetBGMaterial(int Index)
    {
        Background.GetComponent<SpriteRenderer>().material = BackgroundMaterials[Index];
    }

    void CheckVars()
    {
        if (ItemChance > 100)
        {
            throw new System.Exception("Complete reward chance sum > 100!");
        }
        if (UnusualChance + EsotericChance + CommandChance + RevelationChance > 100)
        {
            throw new System.Exception("Item chance sum > 100!");
        }
    }

    public void UpdateBar(int Max, int Current, Transform ChunkHolder, GameObject ChunkPrefab, List<GameObject> Chunks, Sprite FullSprite, Sprite EmptySprite, float Const)
    {
        // Clear previous chunks
        foreach (GameObject Object in Chunks)
        {
            Destroy(Object);
        }
        Chunks.Clear();

        // Create and set location of new chunks
        // Apply sprite based on health
        float ChunkHeight = ((Screen.height / 1080) * Const) / Max; // 153 is a constant that only works in 1080p
        for (int i = 0; i < Max; i++)
        {
            GameObject Instance = Instantiate(ChunkPrefab, ChunkHolder);
            Chunks.Add(Instance);

            RectTransform InstanceRect = Instance.GetComponent<RectTransform>();
            InstanceRect.offsetMin = new Vector2(InstanceRect.offsetMin.x, ChunkHeight * i * 2f);
            InstanceRect.offsetMax = new Vector2(InstanceRect.offsetMax.x, (-ChunkHeight * (Max - i + 1f) * 2f) + (ChunkHeight * 4f));

            if (i + 1 > Current)
            {
                Instance.GetComponent<Image>().sprite = EmptySprite;
            }
            else
            {
                Instance.GetComponent<Image>().sprite = FullSprite;
            }
        }
    }

    public List<GameObject> GenerateEnemies()
    {
        List<GameObject> Spawners = new List<GameObject>();
        float RemainingTokens = Difficulty;

        while(RemainingTokens > 0)
        {
            int RandomInt = Random.Range(0, CurrentEnemyPool.Spawners.Count);

            if (CurrentEnemyPool.Spawners[RandomInt].GetComponent<Spawn>().DifficultyCost <= RemainingTokens)
            {
                Spawners.Add(CurrentEnemyPool.Spawners[RandomInt]);
                RemainingTokens -= CurrentEnemyPool.Spawners[RandomInt].GetComponent<Spawn>().DifficultyCost;
            }
        }

        return Spawners;
    }
    
    public void GenerateLevel()
    {
        // clear existing rooms
        foreach (GameObject Obj in GeneratedRooms)
        {
            Destroy(Obj);
        }
        GeneratedRooms.Clear();

        // (temporary) reset player pos
        if (!DebugEnabled) 
        FindObjectOfType<Player>().transform.position = new Vector3(0, 1, 0);

        // generate rooms randomly in sequence
        for (int i = 0; i < RoomsToGenerate; i++)
        {
            GameObject Instance;

            if (i == 0)
            {
                Instance = Instantiate(CurrentRoomPool.StartingRoom, Vector3.zero, Quaternion.identity);
            }
            else if (i == RoomsToGenerate - 1)
            {
                // pick random boss room
                Instance = Instantiate(CurrentRoomPool.BossRooms[0], GeneratedRooms[i - 1].transform.Find("RoomTop").transform.position, Quaternion.identity);
            }
            else
            {
                // pick random room from pool
                int RandomInt = Random.Range(0, CurrentRoomPool.Rooms.Count);
                Instance = Instantiate(CurrentRoomPool.Rooms[RandomInt], GeneratedRooms[i - 1].transform.Find("RoomTop").transform.position, Quaternion.identity);
            }

            GeneratedRooms.Add(Instance);
        }
    }

    public void UpdateKills()
    {
        if (Kills <= 0)
        {
            KillsText.text = "N";
            return;
        }

        // initialize dictionaries
        Dictionary<int, string> Numerals = new Dictionary<int, string>();
        Numerals.Add(1, "I");
        Numerals.Add(5, "V");
        Numerals.Add(10, "X");
        Numerals.Add(50, "L");
        Numerals.Add(100, "C");
        Numerals.Add(500, "D");
        Numerals.Add(1000, "M");

        // create string
        string[] Characters = new string[Kills.ToString().Length];
        for (int i = 0; i < Characters.Length; i++)
        {
            string Str = "";
            int Num = System.Convert.ToInt32(Kills.ToString().Substring(i, 1));

            while (Num > 0)
            {
                if (Num >= 5)
                {
                    Num -= 5;
                    Str += "B";
                }
                else
                {
                    Num--;
                    Str += "A";
                }
            }
            switch (Str)
            {
                case "BAAAA":
                    Str = "AC";
                    break;
                case "AAAA":
                    Str = "AB";
                    break;
            }

            int Power = 1;
            for (int j = 0; j < Characters.Length - (i + 1); j++)
            {
                Power *= 10;
            }
            for (int j = 0; j < Str.Length; j++)
            {
                string Add = "";
                switch (Str.Substring(j, 1))
                {
                    case "A":
                        Numerals.TryGetValue(1 * Power, out Add);
                        break;
                    case "B":
                        Numerals.TryGetValue(5 * Power, out Add);
                        break;
                    case "C":
                        Numerals.TryGetValue(10 * Power, out Add);
                        break;
                }
                Characters[i] += Add;
            }
        }

        string Output = "";
        for (int i = 0; i < Characters.Length; i++)
        {
            Output += Characters[i];
        }

        KillsText.text = Output;
    }

    public void ClearRoom()
    {
        Difficulty += DifficultyScale;
        UpdateDifficulty();

        // chose room reward
        List<float> Chances = new List<float>();
        Chances.Add(ItemChance);
        switch (RollChance(Chances, 0))
        {
            case 1:
                // spawn item
                Chances.Clear();
                Chances.Add(UnusualChance);
                Chances.Add(EsotericChance);
                Chances.Add(RevelationChance);
                Chances.Add(CommandChance);
                GameObject ToSpawn = null;
                switch (RollChance(Chances, 0))
                {
                    case 0:
                        ToSpawn = TypicalSpawn;
                        break;
                    case 1:
                        ToSpawn = UnusualSpawn;
                        break;
                    case 2:
                        ToSpawn = EsotericSpawn;
                        break;
                    case 3:
                        //ToSpawn = RevelationSpawn;
                        break;
                    case 4:
                        //ToSpawn = CommandSpawn;
                        break;
                }
                Instantiate(ToSpawn, CurrentRoom.Center.position, CurrentRoom.Center.rotation);
                break;
        }

        UpdateProgress();
    }

    public void ItemUpdate(Item NewItem)
    {
        StartCoroutine(ShowItemText(NewItem));
        StartCoroutine(TwistKnobs());
    }

    IEnumerator ShowItemText(Item NewItem)
    {
        Transform T = PickupText;
        switch (NewItem.Type)
        {
            case ItemTypes.Typical:
                T.parent.GetComponent<Image>().color = ItemColors[0];
                T.parent.Find("Type").GetComponent<Image>().sprite = ItemTextTypes[0];
                break;
            case ItemTypes.Unusual:
                T.parent.GetComponent<Image>().color = ItemColors[1];
                T.parent.Find("Type").GetComponent<Image>().sprite = ItemTextTypes[1];
                break;
            case ItemTypes.Esoteric:
                T.parent.GetComponent<Image>().color = ItemColors[2];
                T.parent.Find("Type").GetComponent<Image>().sprite = ItemTextTypes[2];
                break;
        }
        T.GetComponent<PickupTitle>().SetTitle(NewItem.Titles);
        T.parent.Find("Type").GetComponent<Image>().color = NewItem.DescriptionColor;
        T.parent.parent.GetComponent<Text>().color = NewItem.DescriptionColor;
        T.parent.parent.GetComponent<Text>().text = NewItem.Description;

        T.parent.parent.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.05f);
        T.parent.parent.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.05f);
        T.parent.parent.gameObject.SetActive(true);
        yield return new WaitForSeconds(5.5f);
        T.parent.parent.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.05f);
        T.parent.parent.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.05f);
        T.parent.parent.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.05f);
        T.parent.parent.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.05f);
        T.parent.parent.gameObject.SetActive(false);
    }

    IEnumerator TwistKnobs()
    {
        for (int i = 0; i < Knobs.Length; i++)
        {
            {
                yield return new WaitForSeconds(Random.Range(0f, 1f));

                int TurnDirection = Random.Range(0, 2);

                if (TurnDirection == 0 && KnobStates[i] == 0)
                {
                    KnobStates[i] = 7;
                }
                else if (TurnDirection == 1 && KnobStates[i] == 7)
                {
                    KnobStates[i] = 0;
                }
                else
                {
                    if (TurnDirection == 0)
                    {
                        KnobStates[i]--;
                    }
                    else if (TurnDirection == 1)
                    {
                        KnobStates[i]++;
                    }
                }

                Knobs[i].sprite = KnobSprites[KnobStates[i]];
            }
        }
    }

    /// <summary>
    /// Randomly returns true or false based on <c>Percent</c> and <c>Luck</c>.
    /// </summary>
    /// <param name="Percent">Percent chance (out of 100.0) that will return true</param>
    /// <param name="Luck">Rolls multiple times with more luck</param>
    /// <returns>Returns <c>true</c> if roll was successful</returns>
    public static bool RollChance(float Percent, int Luck)
    {
        for (int i = 0; i < Luck + 1; i++)
        {
            float Rand = Random.Range(0f, 100f);
            if (Rand <= Percent)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Randomly returns the index of a value in <c>Percent</c> based on <c>Luck</c>
    /// 0 may be picked if the sum of all values in <c>Percent</c> does not add to 100.0
    /// </summary>
    /// <param name="Percent">List of percent chances that may be picked</param>
    /// <param name="Luck">Rolls multiple times with more luck</param>
    /// <returns>Returns index of <c>Percent</c> that was chosen</returns>
    public static int RollChance(List<float> Percent, int Luck)
    {
        int HighestPick = 0;
        List<float> Chances = new List<float>();
        Chances.Add(0f);
        for (int i = 0; i < Percent.Count; i++)
        {
            float Total = 0;
            for (int j = 0; j <= i; j++)
            {
                Total += Percent[j];
            }
            Chances.Add(Total);
        }

        for (int i = 0; i < Luck + 1; i++)
        {
            float Rand = Random.Range(0f, 100f);
            int HighPick = 0;
            for (int j = 0; j < Chances.Count; j++)
            {
                if (Chances[j] > Rand)
                {
                    HighPick = j;
                    break;
                }
            }

            if (HighPick > HighestPick)
            {
                HighestPick = HighPick;
            }
        }
        return HighestPick;
    }

    void DebugMode()
    {
        
        if (Input.GetKey(KeyCode.LeftControl))
        {
            // ctrl + p : kill all enemies
            if (Input.GetKeyDown(KeyCode.K))
            {
                foreach (Damageable T in FindObjectsOfType<Damageable>())
                {
                    T.Health = 0;
                    T.TakeDamage(1, transform);
                }
            }
            // ctrl + h : full heal
            if (Input.GetKeyDown(KeyCode.H))
            {
                FindObjectOfType<Player>().Health = (int)FindObjectOfType<Player>().Profile.GetStat(PlayerProfile.StatKey.MaxHealth);
                UpdateHealth();
            }
        }
    }
}
