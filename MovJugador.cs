using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Clase que define los controles del jugador
/// </summary>
/// Version 1.0
/// Fecha de creación 13/03/22
/// Creador Isaac Librado
public class MovJugador : MonoBehaviour
{
    //atributos de movimiento
    public float velocidad = 15;
    public float velAngular = 60;

    //Gameobjects para la generacion de terreno y objetos
    public Suelo suelo;
    public Suelo obj;
    public ControlObjetos ctrlObjetos;

    //gameobject del objeto recogible
    public GameObject objeto;

    //Gameobject del policia que persigue al jugador
    public GameObject policia;

    //gameobjects de efectos de particula
    public GameObject turbo;
    public GameObject dano;
    public GameObject explosion;
    public GameObject escudo;


    //atributos para control de vidas y turbo
    public int vidas;
    public float turboT;

    //control de estados
    private int estado;
    enum estadosJug { normal, muerto, correr }

    //UI
    public GameObject final;
    public GameObject hud;

    void Start()
    {
        vidas = 5;
        turboT = 1f;
    }

    /// <summary>
    /// Metodo para activarse al inicio y cambiar los fps meta
    /// </summary>
    /// Version 1.0
    /// Fecha de creación 13/03/22
    /// Creador Isaac Librado
    void Awake()
    {
        QualitySettings.vSyncCount = 0;  // VSync must be disabled
        Application.targetFrameRate = 45;
    }

    // Update is called once per frame
    void Update()
    {
        //Si el estado no es estar muerto, podemos hacer el movimiento del jugador
        if (estado != (int)estadosJug.muerto)
        {
            //aumentamos la velocidad cada frame
            velocidad += 0.1f * Time.deltaTime;

            //rotamos al jugador a la izquierda o a la derecha 
            if (Input.GetKey(KeyCode.A) && transform.rotation.y > 0.9f)
            {
                transform.Rotate(new Vector3(0, -velAngular * Time.deltaTime, 0));
            }
            if (Input.GetKey(KeyCode.D) && transform.rotation.y > 0.9f)
            {
                transform.Rotate(new Vector3(0, velAngular * Time.deltaTime, 0));
            }

            //Hacemos la reaparición del jugador para hacer parecer que el camino es infinito
            if (transform.position.z < -105)
            {
                //Movemos al jugador y al policia a la posicion inicial
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 110f);
                policia.transform.position = new Vector3(policia.transform.position.x, policia.transform.position.y, policia.transform.position.z + 110f);

                //Volvemos a generar los suelos y los objetos
                suelo.Generar();
                obj.Generar();
                ctrlObjetos.Generar();
            }

            //Si el jugador se pasa de los limites lo vuelve a reaparecer
            if (transform.position.x < -10 || transform.position.x > 10)
            {
                transform.position = new Vector3(0, transform.position.y, transform.position.z);
                transform.LookAt(new Vector3(transform.position.x, transform.position.y, -250));
            }

            //Si se presiona espacio y hay un objeto recogido lo activamos y lo ponemos detrás del jugador
            if (Input.GetKey(KeyCode.Space) && objeto != null)
            {
                objeto.SetActive(true);
                objeto.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 0.8f);
                objeto = null;
            }

            //si se acaban las vidas muere el jugador
            if (vidas <= 0)
            {
                velocidad = 0f;
                estado = (int)estadosJug.muerto;
                explosion.SetActive(true);
            }

            //Dependiendo si nos movemos normal o corremos
            if (estado == (int)estadosJug.normal)
            {
                //desactivamos el escudo
                escudo.SetActive(false);

                //si el jugador presiona la tecla para correr y hay suficiente energia para correr se cambia de estado
                if (Input.GetButton("Fire3") && turboT >= 1f)
                {
                    //activamos las particulas del turbo
                    turbo.SetActive(false);
                    turbo.SetActive(true);

                    estado = (int)estadosJug.correr;
                    velocidad *= 2;
                }
            }
            else if (estado == (int)estadosJug.correr)
            {
                //activamos el escudo
                escudo.SetActive(true);

                //disminuimos la energia del turbo cada frame
                turboT -= 1 * Time.deltaTime;

                //si se acaba volvemos al estado normal
                if (turboT <= 0f)
                {
                    velocidad /= 2;
                    estado = (int)estadosJug.normal;
                }
            }

            //nos movemos hacia adelante siempre
            transform.Translate(new Vector3(0, 0, velocidad * Time.deltaTime));

        }
        else
        {
            //si esta muerto mostramos el HUD del final
            hud.SetActive(false);
            final.SetActive(true);
        }
    }

    /// <summary>
    /// Metodo para detectar la colision con otros objetos
    /// </summary>
    /// <param name="other">El collider del otro objeto</param>
    /// Version 1.0
    /// Fecha de creación 13/03/22
    /// Creador Isaac Librado
    private void OnTriggerStay(Collider other)
    {
        //el jugador tiene invencibilidad al correr
        if (estado != (int)estadosJug.correr)
        {
            //Si es un objeto recogible lo guardamos en la variable y lo desactivamos
            if (other.gameObject.tag == "Objeto" && objeto == null)
            {
                objeto = other.gameObject;
                objeto.SetActive(false);
            }

            //si el objeto es un obstaculo
            if (other.gameObject.tag == "Obstaculo")
            {
                //lo desactivamos
                other.gameObject.SetActive(false);

                //mostramos las particulas
                dano.SetActive(false);
                dano.SetActive(true);

                //reducimos las vidas
                if (vidas > 0f)
                    vidas--;

                //cambiamos las velocidades a sus valores normales
                velocidad = 10f;
                policia.GetComponent<Policia>().velocidad = 10.5f;

                //si se pierden las vidas, se muere el jugador
                if (vidas <= 0)
                {
                    velocidad = 0f;
                    estado = (int)estadosJug.muerto;
                    explosion.SetActive(true);
                }

                //si el policia está lejos se pone en una posicion más cercana
                if (policia.transform.position.z > transform.position.z + 5f)
                    policia.transform.position = new Vector3(policia.transform.position.x, policia.transform.position.y, transform.position.z + 5f);
            }

            //si es un turbo, aumentamos la potencia del turbo
            if (other.gameObject.tag == "Turbo")
            {
                other.gameObject.SetActive(false);
                turboT += 1f;
            }

            //Si es reparacion, aumentamos 5 vidas
            if (other.gameObject.tag == "Reparacion")
            {
                other.gameObject.SetActive(false);
                vidas += 5;
            }
        }
    }
}
