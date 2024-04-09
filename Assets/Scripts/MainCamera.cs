using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    private GameObject player;
    private float limIzq;
    private float limDer;
    private Vector3 offset;

    void Start()
    {
        GameController gc = GameController.GetInstance();

        player = gc.GetPlayer();
        offset = transform.position - player.transform.position;

        limIzq = gc.GetLimiteIzquierdo();
        limDer = gc.GetLimiteDerecho();
    }

    void LateUpdate()
    {
        float verticalHeightSeen = Camera.main.orthographicSize * 2.0f;
        float verticalWidthSeen = verticalHeightSeen * Camera.main.aspect;
        float dx = verticalWidthSeen / 2.0f;

        Vector3 newPos = player.transform.position + offset;
        newPos.y = transform.position.y;
        if (newPos.x - dx > limIzq && newPos.x + dx < limDer)
            transform.position = newPos;
    }
}
