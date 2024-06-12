using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour

{
    public Transform target; // Das Zielobjekt
    public float changeInterval = 5f; // Zeitintervall für Zieländerung
    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        timer = changeInterval;
        ChangeTargetPosition();
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            ChangeTargetPosition();
            timer = changeInterval;
        }
    }
     void ChangeTargetPosition()
    {
        float x = Random.Range(-10f, 10f);
        float z = Random.Range(-10f, 10f);
        target.position = new Vector3(x, target.position.y, z);
    }
}
