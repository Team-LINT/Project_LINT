using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;
using Utils.EnumTypes;
using UnityEngine.EventSystems;

public class CustomerBehaviour : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private Camera uiCam;
    private Canvas canvas;
    private GraphicRaycaster raycaster;
    private EventSystem eventSystem;
    private GameObject speechBubble;

    public CustomerState state = CustomerState.Idle;
    private Vector2 dir;
    private float moveSpeed = 1.0f;
    public int lineIndex = 0;

    public int customerUID = 0;
    private int laundryCount = 0;
    private const int minLaundryCount = 2;
    private const int maxLaundryCount = 4;

    private float currentTime = 0.0f;
    private float waitingTime = 30.0f;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        canvas = transform.GetChild(0).GetComponent<Canvas>();
        uiCam = Camera.main;
        canvas.worldCamera = uiCam;
        raycaster = canvas.GetComponent<GraphicRaycaster>();
        eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        speechBubble = canvas.transform.GetChild(0).gameObject;

        Init();
    }

    public void Init()
    {
        agent.speed = moveSpeed;
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        speechBubble.SetActive(false);
    }

    private void Update()
    {
        OnDirection();
        StateHandler();
    }

    private void StateHandler()
    {
        switch (state)
        {
            case CustomerState.Idle:
                break;
            case CustomerState.Move:
                break;
            case CustomerState.WaitLine:
                break;
            case CustomerState.Wait:
                OnWaiting();
                break;
            case CustomerState.Leave:
                break;
        }
    }

    // ������ ����
    public void SetDestination(Transform _target)
    {
        agent.SetDestination(_target.position);
    }

    // ��ٱ�� ��
    public void OnWaiting()
    {
        if (OnRayCheck()?.name == "Bubble")
            CustomerManager.Instance.OnOrder(lineIndex);

        if (currentTime < waitingTime)
        {
            if (lineIndex == 0)
                speechBubble.SetActive(true);
            else
                speechBubble.SetActive(false);

            spriteRenderer.sortingOrder = lineIndex + 2;
            animator.SetInteger("Dir", 1);
            currentTime += Time.deltaTime;
        }
        else
        {
            currentTime = 0.0f;
            state = CustomerState.Leave;
            CustomerManager.Instance.DequeueCustomer(lineIndex);
        }
    }

    // ������ ���� Ȯ��
    public bool HasArriveDestination()
    {
        // ��� ����� �����ٸ�
        if (!agent.pathPending)
        {
            // ���� �Ÿ��� ª�ٸ�
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                // �������� �ʴ� ���¶��
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0.0f)
                    return true;
            }
        }
        return false;
    }

    // ���� Ȯ��
    public void OnDirection()
    {
        //if (!agent.hasPath)
        //    return;

        dir = new Vector2(agent.velocity.x, agent.velocity.y).normalized;

        if (dir.sqrMagnitude > 0.01f)
        {
            if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
            {
                if (dir.x > 0)
                {
                    // ������ �̵�
                    transform.localScale = new Vector3(1, 1, 1);
                    animator.SetInteger("Dir", 4);
                }
                else
                {
                    // ���� �̵�
                    transform.localScale = new Vector3(-1, 1, 1);
                    animator.SetInteger("Dir", 4);
                }
            }
            else
            {
                if (dir.y > 0)
                {
                    // ���� �̵�
                    animator.SetInteger("Dir", 3);
                }
                else
                {
                    // �Ʒ��� �̵�
                    animator.SetInteger("Dir", 2);
                }
            }
        }
        else
        {
            animator.SetInteger("Dir", 0);
        }
    }

    // Ray �浹 Ȯ��
    public GameObject OnRayCheck()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PointerEventData pointerData = new PointerEventData(eventSystem);
            pointerData.position = Input.mousePosition;

            List<RaycastResult> results = new List<RaycastResult>();
            raycaster.Raycast(pointerData, results);

            if (results.Count > 0)
                return results[0].gameObject;
            else
                return null;
        }
        else
            return null;
    }

    private void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.transform.CompareTag("Door") && state == CustomerState.Leave)
        {
            Destroy(gameObject, 0.25f);
        }
        else if (coll.transform.CompareTag("Line"))
        {
            if(state == CustomerState.Move)
            {
                state = CustomerState.Wait;
                laundryCount = Random.Range(minLaundryCount, maxLaundryCount);
            }
            else if(state == CustomerState.WaitLine)
            {
                state = CustomerState.Wait;
            }
        }
    }
}