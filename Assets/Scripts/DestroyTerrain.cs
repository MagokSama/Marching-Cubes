using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTerrain : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void __OnCollisionStay(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            Debug.DrawRay(contact.point, contact.normal, Color.red);
            Chunk c = collision.gameObject.GetComponent<Chunk>();
            if (c != null)
            {
                c.SetDencity(contact.point, 2f, -5);
               
            }
        }

    }
}
