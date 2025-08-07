using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get { return instance; }}

    private int money = 0;
    private int reputation = 50;

    private float gamePlayTime = 600.0f;
    private float currentTime;

    private bool isGameOver = true;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance);
        else
            instance = this;

        DontDestroyOnLoad(gameObject);
        Init();
    }

    private void Update()
    {
        if (isGameOver)
            return;

        if(currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            UIManager.Instance.TimerHandler(currentTime);
        }
        else
        {
            GameOver();
        }
    }

    // �ʱ�ȭ
    private void Init()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        Screen.SetResolution(1920, 1080, true);
        Application.targetFrameRate = 65;

        currentTime = gamePlayTime;
    }

    // ��ȭ ����
    public void MoneyHandler(int _money)
    {
        if (money + _money <= 0)
            money = 0;
        else
            money += _money;

        UIManager.Instance.ChangeMoneyText(money);
    }

    // ���� ����
    public void ReputationHandler(int _reputation)
    {
        if (reputation + _reputation <= 0)
            reputation = 0;
        else if (reputation + _reputation >= 100)
            reputation = 100;
        else
            reputation += _reputation;

        UIManager.Instance.ChangeReputationBar(reputation);
    }

    // ���� ����
    public void GameStart()
    {
        SceneManager.LoadScene(1);
    }

    // ���� ����
    public void GameOver()
    {
        currentTime = 0;
        isGameOver = true;
    }

    // ���� ������
    public void GameExit()
    {
        Application.Quit();
    }

    public void OnSceneLoaded(Scene _scene, LoadSceneMode _mode)
    {
        if(_scene.name == "MainScene")
        {
            UIManager.Instance.Init();
            isGameOver = false;
        }
    }
}