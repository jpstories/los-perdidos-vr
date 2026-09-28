using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        Debug.Log($"Начинаем загрузку сцены: {sceneName}");

        // Позже сюда можно добавить включение UI экрана загрузки

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // Позже сюда можно добавить выключение UI экрана загрузки
        Debug.Log($"Сцена {sceneName} успешно загружена");
    }
}

// SceneLoader.cs
// Скрипт выполняет фоновую асинхронную плавную загрузку сцен (уровней)
// постоянно находится в оперативной памяти.
// Асинхронная загрузка не дает VR зависнуть.


// MonoBehaviour — базовый класс движка Unity.
// Наследование от него позволяет прикрепить этот
// скрипт к пустому объекту (GameObject) прямо в
// Unity Inspector, после чего скрипт начинает
// автоматически участвовать в жизненном цикле игры.


// Паттерн Singleton (Одиночка) (Many Client - One Instance)


// Awake - cистемный метод, срабатывает самым первым при инициализации объекта (__init__)


// Destroy - защита от дубликатов в новой сцене висящие в памяти.


// DontDestroyOnLoad - по умолчанию Unity агрессивно вычищает из оперативной
// памяти все старые объекты при загрузке новой сцены.
// эта команда делает менеджер бессмертным при переходах.


// LoadScene - Обычные методы в C# выполняются за один кадр
// Метод LoadScene здесь служит лишь удобной
// публичной оберткой для запуска
// StartCoroutine - функции, выполнение которой можно «размазать» во времени.


// SceneManager.LoadSceneAsync - дает движку команду начать чтение файлов сцены
// с диска в фоновом потоке, возвращая объект AsyncOperation для отслеживания статуса.


// IEnumerator и yield return - работает в точности как генераторы в Python:

// 1. Цикл while проверяет, завершилась ли загрузка.

// 2. Если нет, команда yield return null; буквально означает:
// "Поставь выполнение этого метода на паузу, отдай управление главному потоку Unity
// (чтобы движок успел отрендерить текущий кадр), и вернись сюда в следующем кадре".

// 3. Как только загрузка завершается (asyncLoad.isDone становится true),
// цикл прерывается, и код идет дальше.