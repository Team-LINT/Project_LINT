namespace Utils.EnumTypes
{
    // �մ� ����
    public enum CustomerState
    {
        Idle,  // �⺻
        Move,  // �̵�
        WaitLine,  // ����ٷ� �̵�
        Wait,  // ��ٸ�
        Happy, // ���
        Angry, // ȭ��
        Leave  // ����
    }

    // ��� Ÿ��
    public enum MachineType
    {
        WashingMachine, // ��Ź��
        Dryer,          // ������
        IroningBoard    // �ٸ���
    }
}