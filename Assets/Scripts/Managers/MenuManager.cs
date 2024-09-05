using System;
using UnityEngine;

public class MenuManager : MonoBehaviour {
    private GameManager m_gameManager;
    
    private void Start() {
        m_gameManager = GameManager.s_instance;
    }

    public void goToMenu() {
        m_gameManager.changeGameSate(GameState.MainMenu);
    }

    public void playGame() {
        m_gameManager.changeGameSate(GameState.Playing);
    }

    public void goToCredits() {
        m_gameManager.changeGameSate(GameState.Credits);
    }

    public void gameOver() {
        throw new NotImplementedException();
    }
    public void restartLevel() {
        m_gameManager.changeGameSate(GameState.RestartLevel);
    }
    public void quitGame() {
        m_gameManager.changeGameSate(GameState.QuitGame);
    }
}