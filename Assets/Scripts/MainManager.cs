using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text MaxScoreText;
    public GameObject GameOverText;
    
    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;
    
    public string playerName;
    public int maxPoints;
    
    // Start is called before the first frame update
    void Start()
    {
        // 加载数据
        LoadMaxPoints();
        playerName = MenuManager.instance.playerName;
        
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        if (m_Points > maxPoints) {
            maxPoints = m_Points;
            SaveMaxPoints();
        }
        m_GameOver = true;
        GameOverText.SetActive(true);
    }

    [System.Serializable]
    class SaveData
    {
        public string playerName;
        public int points;
    }

    public void SaveMaxPoints() {
        Debug.Log("Saving Max Points");
        SaveData data = new SaveData();
        data.playerName = playerName;
        data.points = maxPoints;
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/MaxPoints.json", json);
        MaxScoreText.text = $"Best Score: {playerName}: {maxPoints}";
    }

    public void LoadMaxPoints() {
        string path = Application.persistentDataPath + "/MaxPoints.json";
        string maxPlayerName;
        if (File.Exists(path)) {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            
            maxPlayerName = data.playerName;
            maxPoints = data.points;
        } else {
            maxPlayerName = "";
            maxPoints = 0;
        }
        MaxScoreText.text = $"Best Score: {maxPlayerName}: {maxPoints}";
    }

    public void ResetMaxPoints() {
        Debug.Log("Resetting Max Points");
        maxPoints = 0;
        SaveData data = new SaveData();
        data.playerName = "";
        data.points = maxPoints;
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/MaxPoints.json", json);
        MaxScoreText.text = "Best Score: : 0";
    }
}
