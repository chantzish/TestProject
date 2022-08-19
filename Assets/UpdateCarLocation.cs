using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateCarLocation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        App.SetInitialCarLocation(transform.position[0], transform.position[2]);
    }

    // Update is called once per frame
    void Update()
    {
        //if(transform.position[2]>=539.8 && transform.position[2]<=540.2)
        //Debug.Log($"({transform.position[0]},{transform.position[2]})");
        App.MoveCar(transform.position[0], transform.position[2]);
        App.ReportSpeed(GetComponent<Rigidbody>().velocity.magnitude * 3.6f);
        //Debug.Log($"{((Rigidbody)GetComponent("Rigidbody")).velocity.magnitude * 3.6f}");
    }
}
