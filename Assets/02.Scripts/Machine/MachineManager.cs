using Unity.VisualScripting;
using UnityEditor.VersionControl;
using UnityEngine;
using Utils.EnumTypes;

public class MachineManager : MonoBehaviour
{
    private static MachineManager instance;
    public static MachineManager Instance {  get { return instance; } }

    private RaycastHit2D hit;
    public BasketController basket;
    public MachineController machine;

    public int clickCount = 0;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance);
        else
            instance = this;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 _pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            hit = Physics2D.Raycast(_pos, Vector2.zero, 0.0f);

            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("Basket"))
                    OnBasketSelect();
                else if (hit.collider.CompareTag("Machine"))
                    OnMachineSelect();
            }
        }
    }

    // ��� ����
    public void OnMachineSelect()
    {
        MachineController _machine = hit.collider?.GetComponent<MachineController>();

        if(basket == null)
        {
            if (_machine.machineState == MachineState.Complete)
            {
                // ���� �Ϸ� �� ��迡�� ������ ������
                machine = _machine;
                machine.OnSelect(true, (int)machine.machineType - 1);
                basket = machine.currentBasket;

                if(_machine.machineType == MachineType.IroningBoard)
                {
                    basket.OnComplete();
                    machine.OnSelect(false, (int)machine.machineType - 1);
                    machine.Init();
                    machine = null;
                    basket = null;
                }
            }
        }
        else if(basket != null)
        {
            if(_machine.machineState == MachineState.Idle && basket.laundryState == _machine.laundryState)
            {
                // ��� �ִ� ��迡 ���� �ֱ�
                if (machine != null)
                {
                    _machine.currentBasket = basket;
                    _machine.SetTime(basket.laundryCount);
                    _machine.machineState = MachineState.Working;

                    machine.OnSelect(false, (int)machine.machineType - 1);
                    machine.Init();
                    machine = null;

                    basket.OnNextStep();
                    basket.gameObject.SetActive(false);
                    basket = null;
                }
                else
                {
                    _machine.currentBasket = basket;
                    _machine.SetTime(basket.laundryCount);
                    _machine.machineState = MachineState.Working;

                    machine.OnSelect(false, (int)machine.machineType - 1);
                    machine = null;

                    basket.OnNextStep();
                    basket.gameObject.SetActive(false);
                    basket = null;
                }
            }
            else
            {
                if (machine != null)
                {
                    machine.OnSelect(false, (int)machine.machineType - 1);
                    machine = null;
                }
                else
                {
                    machine.OnSelect(false, (int)machine.machineType - 1);
                    basket = null;
                }
            }
        }
    }

    // ���� �ٱ��� ����
    public void OnBasketSelect()
    {
        if (machine != null)
        {
            machine.GetComponent<BasketController>().OnSelect(false);
            machine = null;
        }

        if (basket == null)
        {
            basket = hit.collider?.GetComponent<BasketController>();
            basket.OnSelect(true);
        }
        else
        {
            basket.OnSelect(false);
            basket = hit.collider?.GetComponent<BasketController>();
            basket.OnSelect(true);
        }
    }

    // ������Ʈ ���� ����
    //public void OnColorHandler(GameObject _object, Color _color)
    //{
    //    _object.GetComponent<SpriteRenderer>().color = _color;
    //}
}