using UnityEngine;

public class ClockHandsController : MonoBehaviour
{
    public Transform hourHand;    // Ссылка на часовую стрелку
    public Transform minuteHand;  // Ссылка на минутную стрелку
    public float rotationSpeed = 5000f;

    private Transform currentHand; // Какая стрелка сейчас захвачена
    private Vector3 previousMousePosition;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Определяем, какую стрелку мы кликнули
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
            // Вычисляем разницу в позиции мыши
            Vector3 delta = Input.mousePosition - previousMousePosition;
            float rotationAmount = delta.x * rotationSpeed;

            // Вращаем только захваченную стрелку
            currentHand.Rotate(0, 0, -rotationAmount, Space.World);

        }
    }
}



