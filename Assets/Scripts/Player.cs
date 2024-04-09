using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour 
{
    [SerializeField] private float hSpeed=7;                   // velocidad horizontal
    [SerializeField] private float vSpeed=7;                   // velocidad de salto

    [SerializeField] private AudioClip audioJump=null;         // sonido al saltar
    [SerializeField] private AudioClip audioRun=null;          // sonido al correr
    [SerializeField] private AudioClip audioMuere=null;        // sonido al morir

    private GameController gc;
    private Rigidbody2D rb2D;
    private AudioSource audioSource;

    private bool vivo = true;           // para saber si estoy vivo
    private bool enSuelo = false;       // para saber si puedo saltar
    private Vector2 velocidadSalto;     // velocidad de salto
    private Vector3 myPosition;         // mi posicion inicial para reiniciar
    private float limIzq;               // limite izquierdo de desplazamiento
    private float limDer;               // limite derecho de desplazamiento

    void Start()
    {
        gc = GameController.GetInstance();

        rb2D = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();

        velocidadSalto = new Vector2(0, vSpeed);
        myPosition = transform.position;

        limIzq = gc.GetLimiteIzquierdo();
        limDer = gc.GetLimiteDerecho();
    }

    void FixedUpdate()
    {
        if (!vivo) return;

        if(Input.GetButtonDown("Jump") && enSuelo)
        {
            enSuelo = false;
            rb2D.velocity = velocidadSalto;
            audioSource.clip = audioJump;
            audioSource.Play();
        }
    }

    void Update()
    {
        if (!vivo) return;

        float moveH = Input.GetAxis("Horizontal");
        if (moveH != 0)
        {
            Bounds bounds = GetComponent<SpriteRenderer>().bounds;
            float dx = moveH * hSpeed * Time.deltaTime;
            float x = transform.position.x + dx;

            if (x - bounds.size.x > limIzq && x + bounds.size.x < limDer)
            {
                transform.Translate(dx, 0, 0);
                if (enSuelo && !audioSource.isPlaying)
                {
                    audioSource.clip = audioRun;
                    audioSource.Play();
                }
            }
        }
        else if (enSuelo && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (!vivo) return;

        GameObject obj = col.gameObject;
        if (obj.tag == "Suelo")
        {
            enSuelo = true;
        }
        else if( obj.tag == "Muerte" )
        {
            vivo = false;
            audioSource.Stop();

            if (gc.RestaVidas() > 0 )
            {
                audioSource.clip = audioMuere;
                audioSource.Play();
                StartCoroutine(SiguienteVida());
            }
        }
    }
            
    public IEnumerator SiguienteVida()
    {
        while (audioSource.isPlaying)
        {
            yield return new WaitForSeconds( 1.0f );
        }

        transform.position = myPosition;
        vivo = true;
    }

}

