using TMPro;
using UnityEngine;

public class MiniGameController : MonoBehaviour
{
    protected TextMeshProUGUI timerText;
    public GameObject resultPhanel;
    public TextMeshProUGUI resultText;

    public float currentTime;
    protected const float miniGameTime = 30.0f;

    protected bool isGameSuccess = false;
    protected bool isGameOver = false;

    // �ʱ�ȭ
    public virtual void Init()
    {
        isGameOver = false;
        isGameSuccess = false;
        currentTime = miniGameTime;

        timerText = transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        resultPhanel = transform.GetChild(1).gameObject;
        resultText = resultPhanel.GetComponentInChildren<TextMeshProUGUI>(true);
    }

    // �̴ϰ��� ����
    public virtual void MiniGameStart()
    {
        Debug.Log("::: MiniGame Start :::");
        Init();
        isGameOver = false;
        currentTime = miniGameTime;
        timerText.text = ((int)currentTime).ToString();
        transform.gameObject.SetActive(true);
    }

    // �̴ϰ��� ����
    public virtual void MiniGameEnd()
    {
        isGameOver = true;
        resultPhanel.SetActive(true);
        resultText.text = (isGameSuccess) ? "����!" : "����!";
    }

    // ���� ���
    public virtual void MiniGameReward()
    {

    }
}