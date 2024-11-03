using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smoke : MonoBehaviour
{

    [SerializeField] float speed;

    float timer = 0;

    private void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);

        timer += Time.deltaTime;

        if (timer > 10)
            Destroy(gameObject);
    }
}
