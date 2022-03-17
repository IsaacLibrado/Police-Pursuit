using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Clase que controla la regeneracion de suelos
/// </summary>
/// Version 1.0
/// Fecha de creación 13/03/22
/// Creador Isaac Librado
public class Suelo : MonoBehaviour {

	//todos los suelos disponibles
	public ProcBasico[] suelos;

	/// <summary>
	/// Metodo que genera los suelos
	/// </summary>
	/// Version 1.0
	/// Fecha de creación 13/03/22
	/// Creador Isaac Librado
	public void Generar()
    {
		//por cada suelo se destruye y vuelve a crear
		foreach(ProcBasico suelo in suelos)
        {
			suelo.Destruir();
			suelo.Generar();
        }
    }
}
