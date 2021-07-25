using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kadai : MonoBehaviour
{
    private GameObject ca;

    // Start is called before the first frame update
    void Start()
    {
        this.ca = GameObject.Find("Main Camera");
    }

    // Update is called once per frame
    void Update()
    {
         if (this.transform.position.z < ca.transform.position.z)
        {
            Destroy(gameObject);
            Debug.Log(gameObject);
            
        }

    }

    
}
