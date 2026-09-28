using UnityEngine;
using System.Collections;

public class GameBootstrapper : MonoBehaviour
{
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        StartCoroutine(InitializeGame());
    }

    private IEnumerator InitializeGame()
    {
        Debug.Log("Bootstrapper: Инициализация базовых систем...");

        // Здесь в будущем будут инициализироваться SaveSystem, AudioManager и т.д.
        // Имитируем загрузку систем длительностью в 1 секунду

        yield return new WaitForSeconds(1f);
        Debug.Log("Bootstrapper: Инициализация завершена. Переходим на тестовую арену.");

        // Пока мы тестируем механики, логичнее грузить TestArena. 
        // Позже поменяем это на "MainMenu".

        SceneLoader.Instance.LoadScene("TestArena");
    }

}

// using System.Collections - добавляет Coroutines, type IEnumerator, yield return new WaitForSeconds(3f)

// Bootstrapper - паттерн проектирования, его главные задачи:
// 1. Инициализировать глобальные системы (подключить базы данных, загрузить сохранения, настроить аудио-менеджер).
// 2. Настроить конфигурации и связать важные компоненты между собой (для этого и понадобился SceneLoader).
// 3. После полной готовности систем — дать команду SceneLoader запустить первую игровую сцену (например, Главное меню).


// Название класса GameBootstrapper совпадает с именем файла GameBootstrapper.cs, Unity прикрепит его к игровому обьекту. 
// : - символ наследование, забирает все возможности, свойства и методы класса MonoBehaviour
// базовый класс MonoBehaviour - C#-код превращается в «игровой компонент Unity» в инспекторе


// DontDestroyOnLoad сохранит главного менеджера игры GameBootstrapper в памяти для управления всей игрой в фоне
// StartCoroutine инициализируем игру постепенно, чтобы не тормозило на старте
