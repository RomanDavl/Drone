using UnityEngine;

public class BrakingZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        AICarController controller = other.GetComponent<AICarController>();
        if (controller)
        {
            controller.IsInsideBraking = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        AICarController controller = other.GetComponent<AICarController>();
        if (controller)
        {
            controller.IsInsideBraking = false;
        }
    }
}
