using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissapearAfterStart : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(Dissapear());
    }

    IEnumerator Dissapear()
    {
        yield return new WaitForSeconds(10f);
        Destroy(this.gameObject);
    }
}
