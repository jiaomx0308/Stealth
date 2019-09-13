using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeTest : MonoBehaviour
{

    public float Speed = 1;

    private Collider collider;
    // Start is called before the first frame update
    void Start()
    {
        collider = this.GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Translate(Speed * Time.deltaTime, 0 ,0);
    }

    private void OnTriggerEnter(Collider other)
    {
        print($"cube OntriggerENter : {other.name}");
    }

    private void OnCollisionEnter(Collision collision)
    {
        print($"cube OnCollisionEnter : {collider.gameObject.name}");
    }
}
