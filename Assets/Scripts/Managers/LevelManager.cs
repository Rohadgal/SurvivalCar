using System;
using UnityEngine;
using TMPro;
using Unity.Mathematics;

public class LevelManager : MonoBehaviour {
    public static LevelManager s_instance;
    private GameManager m_gameManager;


    public float gameTimeInSeconds = 600f; // time to win 
    public float timer = 0f;
    public TMP_Text timerC;
    public TMP_Text timerToWin;
    public LevelState LevelState { get; private set; }
    
    private void Awake() {
        if (FindObjectOfType<LevelManager>() != null &&
            FindObjectOfType<LevelManager>().gameObject != gameObject) {
            Destroy(gameObject);
            return;
        }
        s_instance = this;
        setLevelState(LevelState.Playing);
    }

    private void Start() {
        m_gameManager = FindObjectOfType<GameManager>();
        timerToWin.text = Mathf.FloorToInt(gameTimeInSeconds).ToString();
    }

    private void Update() {
        surviveTime();
    }

    private void surviveTime() {
        timer += Time.deltaTime;
        timerC.text = Mathf.FloorToInt(timer).ToString();
        if (timer >= gameTimeInSeconds) {
            m_gameManager.changeGameSate(GameState.Win);
        }
    }

    private void OnEnable() {
        ExperienceManager.OnLevelUp += setSelectingPowerUpState;
    }

    private void OnDisable() {
        ExperienceManager.OnLevelUp += setSelectingPowerUpState;
    }

    /// <summary>
    /// Gets the current state of the level.
    /// </summary>
    /// <returns>The current state of the level.</returns>
    public LevelState getLevelState() {
        return LevelState;
    }

    /// <summary>
    /// Sets the state of the level and performs necessary actions based on the state change.
    /// </summary>
    /// <param name="t_levelState">The new state of the level.</param>
    public void setLevelState(LevelState t_levelState) {
        if (LevelState == t_levelState) {
            return;
        }
        LevelState = t_levelState;
        switch (LevelState) {
            case LevelState.None:
                break;
            case LevelState.LoadingLevel:
                break;
            case LevelState.Menu:
                break;
            case LevelState.Playing:
                break;
            case LevelState.Credits:
                m_gameManager.gameOver();
                break;
            case LevelState.SelectingPowerUp:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void setSelectingPowerUpState() {
        Time.timeScale = 0;
        setLevelState(LevelState.SelectingPowerUp);
    }

}

/// <summary>
/// Enumeration representing the possible states of the game level.
/// </summary>
public enum LevelState {
    None,
    LoadingLevel,
    Menu,
    Playing,
    Credits,
    SelectingPowerUp,
}