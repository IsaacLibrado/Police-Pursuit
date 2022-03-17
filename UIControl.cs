using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Clase que controla el HUD del jugador con la información del juego
/// </summary>
/// Version 1.0
/// Fecha de creación 13/03/22
/// Creador Isaac Librado
public class UIControl : MonoBehaviour {

	//script del jugador con la información internamente
	public MovJugador scriptJugador;

	//objetos de UI
	public Text vidas;
	public Text velocidad;
	public RawImage objeto;
	public RawImage turbo;
	public Text km;
	public Text kmFinal;

	//valores para calcular la distancia
	private float velocidadVal;
	private float kmVal;

	// Use this for initialization
	void Start () {
		kmVal = 0;
	}

	// Update is called once per frame
	void Update() {

		//obtenemos los valores de velocidad y vidas del script del jugador
		velocidadVal = scriptJugador.velocidad;
		velocidad.text = string.Format("{0} km/h", Mathf.FloorToInt(velocidadVal));

		vidas.text = scriptJugador.vidas.ToString();

		//cambiamos el alfa de las imagenes dependiendo si el jugador puede realizar dichas acciones
		if (scriptJugador.objeto != null)
			objeto.GetComponent<RawImage>().color = new Color32(255, 255, 225, 255);
		else
			objeto.GetComponent<RawImage>().color = new Color32(255, 255, 225, 23);

		if (scriptJugador.turboT >= 1f)
			turbo.GetComponent<RawImage>().color = new Color32(255, 255, 225, 255);
		else
			turbo.GetComponent<RawImage>().color = new Color32(255, 255, 225, 23);

		//aumentamos la distancia dividiendo la velocidad entre 60
		kmVal += velocidadVal/60;

		//ponemos los valores de distancia en los UI correspondientes
		km.text = string.Format("{0} km", Mathf.FloorToInt(kmVal));
		kmFinal.text = string.Format("{0} km", Mathf.FloorToInt(kmVal));
	}
}
