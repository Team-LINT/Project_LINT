using UnityEngine;

public class BasketController : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Sprite[] basketSprites;
    public Sprite[] basketSelectSprites;

    public int customerUID = 0;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = basketSprites[Random.Range(0, basketSprites.Length)];
    }

    // �����ٱ��� ����
    public void OnSelect()
    {
        
    }
}
