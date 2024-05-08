using Cinemachine;
using UnityEngine;
using static UnityEngine.GridBrushBase;

public class Cameratoration : MonoBehaviour
{
    [SerializeField] private CinemachineFreeLook freeLookCam;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private AICarController controller;
    [SerializeField] private Transform city;

    void Update()
    {
        Transform car = controller.transform;

        Vector3 directionToCity = (city.position - car.position).normalized;
        float angleToCity = Vector3.SignedAngle(car.forward, directionToCity, Vector3.up);

        Debug.Log(angleToCity);

        float minAngle = Mathf.Clamp(angleToCity - 30f, 0f, 180f);
        float maxAngle = Mathf.Clamp(angleToCity + 30f, 0f, 180f);

        float minValue = minAngle / 1f;
        float maxValue = maxAngle / 1f;

        freeLookCam.m_XAxis.m_MinValue = minValue;
        freeLookCam.m_XAxis.m_MaxValue = maxValue;

        freeLookCam.m_XAxis.Value += rotationSpeed * Time.deltaTime;
    }
}
