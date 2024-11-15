using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameData gameData;

    public int nextSpawnPoint;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.G))
        {
            gameData.PlayerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
            SaveData(GameManager.instance.gameData.Ranura);
        }
        if (Input.GetKey(KeyCode.RightControl) && Input.GetKeyDown(KeyCode.D))
        {
            PlayerPrefs.DeleteAll();
        }

    }

    public void SaveData(int ranura)
    {
        gameData.CurrentScene = SceneManager.GetActiveScene().buildIndex;
        string data = JsonUtility.ToJson(gameData);
        PlayerPrefs.SetString("gameData"+ranura.ToString(), data);

    }

    public void LoadData(string partidaName)
    {
        if (PlayerPrefs.HasKey(partidaName) == true)
        {
            string data = PlayerPrefs.GetString(partidaName);
            gameData = JsonUtility.FromJson<GameData>(data);
        }
        else
        {
            gameData = new GameData();
            gameData.Life = 100;
            gameData.MaxLife = 100;
            gameData.PlayerPos = new Vector3(-5.12f, 0.258f, 0);
            gameData.MaxMana = 100;
            gameData.Mana = 100;
        }
        
    }
}
