using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Premio : MonoBehaviour
{
    private bool vivo = true;

    void OnTriggerEnter2D(Collider2D col)
    {
        GameObject obj = col.gameObject;
        if (vivo && obj.tag == "Player")
        {
            vivo = false;

            GameController gc = GameController.GetInstance();
            gc.SumaPuntos(1);

            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<AudioSource>().Play();
            Destroy(gameObject, 1.0f);
        }
    }
}
