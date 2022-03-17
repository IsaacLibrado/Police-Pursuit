using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Clase que define al movimiento del sol para simular el ciclo de día y noche
/// </summary>
/// Version 1.0
/// Fecha de creación 13/03/22
/// Creador Isaac Librado
public class CicloDiaNoche : MonoBehaviour {

	//escala de rotación por frame
	public int escalaRotacion = 10;
	
	void Update () {
		transform.Rotate(escalaRotacion * Time.deltaTime,0,0);
	}
}
