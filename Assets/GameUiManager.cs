using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUiManager : MonoBehaviour
{
    public static GameUiManager Instance { get; private set; }

    [SerializeField] private GameObject mainPanel = null;
    [SerializeField] private GameObject continuePanel = null;
    [SerializeField] private Text continuePanelStatsText = null;
    [SerializeField] private GameObject finishedPanel = null;
    [SerializeField] private Text finishedPanelMessageText = null;
    [SerializeField] private Text finishedPanelStatsTexxt = null;

    [SerializeField] private Text levelTimeText = null;
    [SerializeField] private Text levelText = null;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        levelText.gameObject.SetActive(false);
        levelTimeText.gameObject.SetActive(false);
        mainPanel.SetActive(true);
        continuePanel.SetActive(false);
        finishedPanel.SetActive(false);
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    public void UpdateLevelTime(float time)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(time);

        levelTimeText.text = timeSpan.ToString(@"m\:ss\:ff");
    }

    public void UpdateLevelText(int level)
    {
        levelText.text = $"Level: {level}";
    }

    public void ShowContinuePanel(float levelTime)
    {
        continuePanelStatsText.text = $"Level time: {TimeSpan.FromSeconds(levelTime):m\\:ss\\:ff}";
        finishedPanel.SetActive(false);
        continuePanel.SetActive(true);
    }

    public void ShowFinishedPanel(string message, int level, float time)
    {
        levelText.gameObject.SetActive(false);
        levelTimeText.gameObject.SetActive(false);
        finishedPanelMessageText.text = message;
        finishedPanelStatsTexxt.text = $"Max Level: {level}{Environment.NewLine}Total time: {TimeSpan.FromSeconds(time):m\\:ss\\:ff}";
        continuePanel.SetActive(false);
        finishedPanel.SetActive(true);
    }

    public void StartGame_ButtonClick()
    {
        mainPanel.SetActive(false);
        GameManager.Instance.StartGame();
        levelTimeText.gameObject.SetActive(true);
        levelText.gameObject.SetActive(true);
    }

    public void Continue_ButtonClick()
    {
        continuePanel.SetActive(false);
        GameManager.Instance.StartNextLevel();
        levelText.gameObject.SetActive(true);
        levelTimeText.gameObject.SetActive(true);
    }

    public void PlayAgain_ButtonClick()
    {
        SceneManager.LoadScene(0);
    }
}
