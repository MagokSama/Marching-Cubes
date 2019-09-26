using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class CharController : MonoBehaviour
{
    Rigidbody RB;
    public float Speed = 5;
    void Start()
    {
        RB = GetComponent<Rigidbody>();
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
            transform.position += transform.forward * Speed * Time.deltaTime;
        if (Input.GetKey(KeyCode.S))
            transform.position += -transform.forward * Speed * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
            transform.Rotate(-Vector3.up * Speed * 7 * Time.deltaTime);

        if (Input.GetKey(KeyCode.D))
            transform.Rotate(Vector3.up * Speed * 7 * Time.deltaTime);

    }

}

