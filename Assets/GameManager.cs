using PathFinding;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; } = null;

    [SerializeField] private LevelGenerator levelGenerator = null;


    [SerializeField] private TextAsset[] levelsFiles = null;

    private LevelInfo Level { get; set; }
    private PlayerController player = null;
    private IEnumerable<EnemyController> enemies;

    public bool LevelStarted { get; private set; }
    private bool CloseUpCamera { get; set; }
    private int currentLevel = 0;
    private float levellTime = 0;
    private float totalTime = 0;
    private GameObject levelHolder = null;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }


    private void Update()
    {
        if (!LevelStarted) return;

        if (player == null)
        {
            player = FindObjectOfType<PlayerController>();
            if(player != null)
                Camera.main.GetComponent<CameraController>().SetTarget(player.transform);
        }


        enemies = FindObjectsOfType<EnemyController>().ToList();
        foreach (var e in enemies)
        {
            if (!e.HasTarget())
            {
                var pF = PositionToMapPoint(e.transform.position);
                var pT = PositionToMapPoint(player.transform.position);
                //print($"x:{pF.x},y:{pF.y}");
                //print($"x:{pT.x},y:{pT.y}");
                Point nextPoint = Level.GetNextPoint(pF, pT);
                //print($"x:{nextPoint.x},y:{nextPoint.y}");
                //print(Level.TilesMap[nextPoint.x, nextPoint.y]);
                //print(Level.TilesMap[nextPoint.x, nextPoint.y]);
                Vector2 nextPos = MapPointToPosition(nextPoint);
                //print(nextPos);
                e.SetTargetPos(nextPos);
            }
        }

        levellTime += Time.deltaTime;
        GameUiManager.Instance.UpdateLevelTime(levellTime);
    }

    

    public void StartGame()
    {
        StartNextLevel();
    }

    public void StartNextLevel()
    {
        if(levelHolder != null)
        {
            Destroy(levelHolder);
            Destroy(player.gameObject);
            foreach (var e in enemies)
            {
                Destroy(e.gameObject);
            }
        }

        totalTime += levellTime;
        levellTime = 0;
        currentLevel++;

        GameUiManager.Instance.UpdateLevelText(currentLevel);

        TextAsset levelFile = levelsFiles[currentLevel - 1];
        string[,] levelTileMap = GetLevelTileMapFromCsvFile(levelFile);

        Level = new LevelInfo(levelTileMap);
        levelHolder = levelGenerator.GenerateLevel(Level);

        Camera.main.GetComponent<CameraController>().SetUp(Level.Width, Level.Height, CloseUpCamera);
        LevelStarted = true;
    }

    private string[,] GetLevelTileMapFromCsvFile(TextAsset levelFile)
    {
        string[,] tileMap;
        string csv = levelFile.text.Trim();

        string[] lines = csv.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
        string[] tileMapCsvLines = lines.Skip(1).ToArray();
        string closeUpCamera = lines.Take(1).First();
        CloseUpCamera = bool.Parse(closeUpCamera);
        int colsCount = tileMapCsvLines[0].Split(';').Length;

        tileMap = new string[tileMapCsvLines.Length, colsCount];
        for (int lineIndx = 0; lineIndx < tileMapCsvLines.Length; lineIndx++)
        {
            string line = tileMapCsvLines[lineIndx];
            string[] cols = line.Split(';');
            for (int colIndx = 0; colIndx < cols.Length; colIndx++)
            {
                tileMap[lineIndx, colIndx] = cols[colIndx];
            }
        }

        return tileMap;
    }

    internal void GameOver()
    {
        LevelStarted = false;
        totalTime += levellTime;
        GameUiManager.Instance.ShowFinishedPanel("You Lost", currentLevel, totalTime);
    }

    internal void LevelCompleted()
    {
        LevelStarted = false;
        bool isLastLevel = currentLevel == levelsFiles.Length;
        if (isLastLevel)
        {
            totalTime += levellTime;
            GameUiManager.Instance.ShowFinishedPanel("You Won", currentLevel, totalTime);
        }
        else
        {
            GameUiManager.Instance.ShowContinuePanel(levellTime);
        }

    }

    private Point PositionToMapPoint(Vector3 worldPos)
    {
        return new Point(Mathf.FloorToInt(worldPos.x), Mathf.FloorToInt(worldPos.y));
    }

    private Vector2 MapPointToPosition(Point point)
    {
        return Vector2.right * point.x + Vector2.up * point.y;
    }
}
