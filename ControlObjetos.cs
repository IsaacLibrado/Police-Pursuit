using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Clase que controla la regeneración de objetos recogibles
/// </summary>
/// Version 1.0
/// Fecha de creación 13/03/22
/// Creador Isaac Librado
public class ControlObjetos : MonoBehaviour {

	//Todos los objetos que se pueden recoger
	public GameObject[] objetosRecogibles;

	//script del jugador
	public MovJugador jugador;

	//ubicación en z
	public float ubiz;

	// Update is called once per frame
	void Update () {

		//si el jugador no tiene objetos
		if(jugador.objeto!=null)
        {
			//activamos todos los objetos
			foreach (GameObject objRec in objetosRecogibles)
			{
				objRec.SetActive(false);
			}
		}
	}

	/// <summary>
	/// Metodo para generar los objetos
	/// </summary>
	/// Version 1.0
	/// Fecha de creación 13/03/22
	/// Creador Isaac Librado
	public void Generar()
    {
		float ubix = 4f;

		//por cada objeto activamos y colocamos los objetos
		foreach (GameObject objRec in objetosRecogibles)
        {
			ubix -= 2f;
			objRec.SetActive(true);
			objRec.transform.position=new Vector3(ubix, 0.5f, ubiz);
        }
    }

}
