using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Clase que define la generacion random del suelo
/// </summary>
/// Version 1.0
/// Fecha de creación 13/03/22
/// Creador Isaac Librado
public class ProcBasico : MonoBehaviour {

	//gameobjects de los objetos posibles para crear
	public GameObject[] objetosPosibles;

	//el objeto creado
	private GameObject creado;

	// Use this for initialization
	void Start() {
		Generar();
	}

	/// <summary>
	/// Metodo para la generacion de los suelos
	/// </summary>
	/// Version 1.0
	/// Fecha de creación 13/03/22
	/// Creador Isaac Librado
	public void Generar()
	{
		//Se instancia el objeto tomando una posicion random del array de objetos y un giro random
		creado = Instantiate(
			objetosPosibles[Random.Range(0, objetosPosibles.Length)],
			transform.position,
			Quaternion.Euler(Vector3.up*(Random.Range(0,2)*180))
			);
	}

	/// <summary>
	/// Metodo para la destruccion del jugador
	/// </summary>
	/// Version 1.0
	/// Fecha de creación 13/03/22
	/// Creador Isaac Librado
	public void Destruir()
    {
		Destroy(creado);
    }
}
