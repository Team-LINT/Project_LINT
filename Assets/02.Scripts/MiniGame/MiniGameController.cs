using TMPro;
using UnityEngine;

public class MiniGameController : MonoBehaviour
{
    private TextMeshProUGUI timerText;
    private GameObject resultPhanel;
    public TextMeshProUGUI resultText;

    private float currentTime;
    private const float miniGameTime = 30.0f;

    protected bool isGameSuccess = false;
    protected bool isGameOver = false;

    private void Awake()
    {
        timerText = transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        resultPhanel = transform.GetChild(1).gameObject;
        resultText = resultPhanel.GetComponentInChildren<TextMeshProUGUI>(true);
        Init();
    }

    private void Update()
    {
        if (isGameOver)
            return;

        if(currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            timerText.text = ((int)currentTime).ToString();
        }
        else
        {
            isGameSuccess = false;
            MiniGameEnd();
        }
    }

    // �ʱ�ȭ
    public void Init()
    {
        isGameOver = false;
        isGameSuccess = false;
        currentTime = miniGameTime;
    }

    // �̴ϰ��� ����
    public virtual void MiniGameStart()
    {
        isGameOver = true;
        currentTime = miniGameTime;
        timerText.text = ((int)currentTime).ToString();
        transform.gameObject.SetActive(true);
    }

    // �̴ϰ��� ����
    public virtual void MiniGameEnd()
    {
        isGameOver = false;
        resultPhanel.SetActive(true);
        resultText.text = (isGameSuccess) ? "����!" : "����!";
    }

    // ���� ���
    public virtual void MiniGameReward()
    {

    }
}