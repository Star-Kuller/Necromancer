using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    // Метод для загрузки сцены с игрой
    public void StartGame()
    {
        // Замените "GameScene" на имя вашей игровой сцены
        SceneManager.LoadScene("GameScene");
    }

    // Метод для выхода из игры
    public void ExitGame()
    {
        // Проверяем, запущена ли игра в редакторе или как билд
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false; // Останавливаем игру в редакторе
        #else
            Application.Quit(); // Закрываем приложение в билде
        #endif
    }
}