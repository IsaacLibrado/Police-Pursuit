using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Clase que define el movimiento de la camara en el juego
/// </summary>
/// Version 1.0
/// Fecha de creación 13/03/22
/// Creador Isaac Librado
public class Camara : MonoBehaviour {

	//transform con la ubicacion del jugador
	public Transform jugador;
	
	// Update is called once per frame
	void Update () {
		//la camara sigue al jugador en z
		transform.position = new Vector3(transform.position.x, transform.position.y, jugador.position.z);
	}
}
