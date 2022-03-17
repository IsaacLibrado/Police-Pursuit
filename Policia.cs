using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script que controla al policia IA
/// </summary>
/// Version 1.0
/// Fecha de creación 13/03/22
/// Creador Isaac Librado
public class Policia : MonoBehaviour {

    //Atributos de movimiento
    public float velocidad = 5;
    public Transform jugador;
    private Vector3 Objetivo;

    //atritubos para la deteccion
    public float distanciaDeteccion = 10;
    private RaycastHit hitD;
    private RaycastHit hitI;
    private Vector3 ladoDerecho;
    private Vector3 ladoIzquierdo; 
    private Vector3 direccion;
    private Vector3 puntoI;
    private Vector3 normal;
    private RaycastHit hit;
    public float escala = 1f;

    //gameobjects de particulas para el juice del juego
    public GameObject particulas;
    public GameObject preparacion;
    public GameObject dano;

    //atributos para la maquina de estados finitos
    private int estado;
    enum estadosPolicia { normal, parar, chocar }


    // Update is called once per frame
    void Update()
    {
        //aumentamos la velocidad del policia cada frame
        velocidad += 0.1f * Time.deltaTime;

        //dependiendo el estado realiza una acción
        if (estado == (int)estadosPolicia.normal)
        {
            Normal();
        }
        else if (estado == (int)estadosPolicia.chocar)
        {
            Chocar();
        }
        else if (estado == (int)estadosPolicia.parar)
        {
            Parar();
        }
    }

    /// <summary>
    /// Metodo que se realiza normalmente
    /// </summary>
    /// Version 1.0
    /// Fecha de creación 13/03/22
    /// Creador Isaac Librado
    void Normal()
    {
        //indicamos que el objetivo está en la ubicación del jugador
        Objetivo = jugador.position;

        //creamos los vectores de bigotes
        ladoDerecho = transform.forward + (transform.right * 0.7f);
        ladoIzquierdo = transform.forward - (transform.right * 0.7f);

        //revisamos la colisión de los bigotes, mientras la detección no sea con el jugador cambiamos el objetivo del policia para evitar la colision
        if (Physics.Raycast(transform.position, ladoDerecho, out hitD, distanciaDeteccion))
        {
            if (hitD.collider.transform.tag != "Player")
                Objetivo -= transform.right * (distanciaDeteccion / hitD.distance);

        }

        if (Physics.Raycast(transform.position, ladoIzquierdo, out hitI, distanciaDeteccion))
        {
            if (hitI.collider.transform.tag != "Player")
                Objetivo += transform.right * (distanciaDeteccion / hitI.distance);

        }

        //obtenemos la dirección al jugador
        direccion = jugador.position - transform.position;

        //detectamos la colision
        if (Physics.Raycast(transform.position, direccion, out hit, distanciaDeteccion))
        {
            //mientras no sea el jugador la colision, cambiamos el objetivo para evitar colision
            if (hit.collider.transform.tag != "Player")
            {
                puntoI = hit.point;
                normal = hit.normal;

                escala = 12f / (puntoI - transform.position).magnitude;

                Objetivo = puntoI + (normal * escala);
                Objetivo.y = transform.position.y;
            }

        }

        //miramos al objetivo y nos movemos hacia ahí
        transform.LookAt(Objetivo);
        transform.Translate(Vector3.forward * velocidad * Time.deltaTime);

        //si la distancia es menor a 3.5f empezamos la preparación
        if (direccion.magnitude < 3.5f)
        {
            preparacion.SetActive(true);

            //obtenemos un valor random
            int valorran = Random.Range(0, 50);
         
            //si el valor random es 5 cambiamos de estado
            if (valorran == 5)
                estado = (int)estadosPolicia.chocar;
        }
        else
            preparacion.SetActive(false);

    }

    /// <summary>
    /// Metodo que realiza el movimiento de choque del policia
    /// </summary>
    /// Version 1.0
    /// Fecha de creación 13/03/22
    /// Creador Isaac Librado
    void Chocar()
    {
        //Cambiamos los efectos de particulas
        preparacion.SetActive(false);
        particulas.SetActive(true);

        //Miramos hacia adelante y nos movemos con velocidad multiplicada por 1.5
        transform.LookAt(new Vector3(transform.position.x, transform.position.y, -250));
        transform.Translate(Vector3.forward * velocidad*1.5f* Time.deltaTime);

        //si estamos 4f más adelante del jugador cambiamos de estado
        if(transform.position.z<jugador.position.z-4f)
            estado = (int)estadosPolicia.parar;
    }

    /// <summary>
    /// Metodo que realiza parar el movimiento del policia
    /// </summary>
    /// Version 1.0
    /// Fecha de creación 13/03/22
    /// Creador Isaac Librado
    void Parar()
    {
        //Cambiamos los efectos de particulas
        preparacion.SetActive(false);
        particulas.SetActive(false);

        //si la distancia es mayor a 6f cambiamos de estado a normal
        if (transform.position.z > jugador.position.z + 6f)
            estado = (int)estadosPolicia.normal;
    }

    /// <summary>
    /// Metodo que se activa cuando se detecta que un collider entra al collider del policia
    /// </summary>
    /// <param name="other">El collider del otro objeto</param>
    /// Version 1.0
    /// Fecha de creación 13/03/22
    /// Creador Isaac Librado
    private void OnTriggerStay(Collider other)
    {
        //si el objeto es el jugador activamos los efectos de choque y realizamos la disminucion de vidas
        if (other.gameObject.tag == "Player")
        {
            dano.SetActive(false);
            dano.SetActive(true);

            if (jugador.gameObject.GetComponent<MovJugador>().vidas > 0f)
            {
                jugador.gameObject.GetComponent<MovJugador>().velocidad = 10f;
                jugador.gameObject.GetComponent<MovJugador>().vidas--;
                velocidad = 10.5f;
            }

            //cambiamos el estado del policia
            estado = (int)estadosPolicia.parar;
        }
    }
}
