using UnityEngine;

public class ClockHandsController : MonoBehaviour
{
    public Transform hourHand;    // ������ �� ������� �������
    public Transform minuteHand;  // ������ �� �������� �������
    public float rotationSpeed = 5000f;

    private Transform currentHand; // ����� ������� ������ ���������
    private Vector3 previousMousePosition;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // ����������, ����� ������� �� ��������
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform == hourHand || hit.transform == minuteHand)
                {
                    Debug.Log("Hand");
                    currentHand = hit.transform;
                    previousMousePosition = Input.mousePosition;
                }
            }
        }

        if (Input.GetMouseButton(0) && currentHand != null)
        {
            // ��������� ������� � ������� ����
            Vector3 delta = Input.mousePosition - previousMousePosition;
            float rotationAmount = delta.x * rotationSpeed;

            // ������� ������ ����������� �������
            currentHand.Rotate(0, 0, -rotationAmount, Space.World);

        }
    }
}



