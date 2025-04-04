using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class MenuController : MonoBehaviour
{
    [Header("Panel de Inicio")]
    [SerializeField] private GameObject panelInicio;

    [Header("Panel Jugadores")]
    [SerializeField] private GameObject panelJugadores;
    [SerializeField] private TMP_InputField inputNombre1;
    [SerializeField] private TMP_InputField inputNombre2;
    [SerializeField] private Button botonConfirmar;

    [Header("Botones de Elementos Jugador 1")]
    [SerializeField] private Button botonFuego1;
    [SerializeField] private Button botonAgua1;
    [SerializeField] private Button botonPlanta1;
    [SerializeField] private Button botonNormal1;

    [Header("Botones de Elementos Jugador 2")]
    [SerializeField] private Button botonFuego2;
    [SerializeField] private Button botonAgua2;
    [SerializeField] private Button botonPlanta2;
    [SerializeField] private Button botonNormal2;

    private string nombreJugador1, nombreJugador2;
    private List<string> elementosJugador1 = new List<string>();
    private List<string> elementosJugador2 = new List<string>();

    void Start()
    {
        panelInicio.SetActive(true);
        panelJugadores.SetActive(false);
        botonConfirmar.interactable = false;

        inputNombre1.onValueChanged.AddListener(delegate { ValidarConfirmacion(); });
        inputNombre2.onValueChanged.AddListener(delegate { ValidarConfirmacion(); });
    }

    public void IniciarJuego()
    {
        panelInicio.SetActive(false);
        panelJugadores.SetActive(true);
        ResetearBotones();
    }

    public void SeleccionarFuego1() => SeleccionarElemento(1, botonFuego1, "Fuego");
    public void SeleccionarAgua1() => SeleccionarElemento(1, botonAgua1, "Agua");
    public void SeleccionarPlanta1() => SeleccionarElemento(1, botonPlanta1, "Planta");
    public void SeleccionarNormal1() => SeleccionarElemento(1, botonNormal1, "Normal");

    public void SeleccionarFuego2() => SeleccionarElemento(2, botonFuego2, "Fuego");
    public void SeleccionarAgua2() => SeleccionarElemento(2, botonAgua2, "Agua");
    public void SeleccionarPlanta2() => SeleccionarElemento(2, botonPlanta2, "Planta");
    public void SeleccionarNormal2() => SeleccionarElemento(2, botonNormal2, "Normal");

    private void SeleccionarElemento(int jugador, Button boton, string elemento)
    {
        List<string> elementos = (jugador == 1) ? elementosJugador1 : elementosJugador2;

        if (elementos.Contains(elemento))
        {
            elementos.Remove(elemento);
            SetAlpha(boton, 0.5f);
        }
        else
        {
            elementos.Add(elemento);
            SetAlpha(boton, 1.0f);
        }

        ValidarConfirmacion();
    }

    private void ValidarConfirmacion()
    {
        bool nombresValidos = !string.IsNullOrEmpty(inputNombre1.text) && !string.IsNullOrEmpty(inputNombre2.text);
        bool elementosValidos = elementosJugador1.Count > 0 && elementosJugador2.Count > 0;
        botonConfirmar.interactable = nombresValidos && elementosValidos;
    }

    public void ConfirmarJugadores()
    {
        nombreJugador1 = inputNombre1.text;
        nombreJugador2 = inputNombre2.text;

        PlayerPrefs.SetString("NombreJugador1", nombreJugador1);
        PlayerPrefs.SetString("ElementoJugador1", string.Join(",", elementosJugador1));

        PlayerPrefs.SetString("NombreJugador2", nombreJugador2);
        PlayerPrefs.SetString("ElementoJugador2", string.Join(",", elementosJugador2));
        PlayerPrefs.Save();

        UnityEngine.SceneManagement.SceneManager.LoadScene("EscenaJuego");
    }

    private void ResetearBotones()
    {
        ResetearBotonesJugador(botonFuego1, botonAgua1, botonPlanta1, botonNormal1, elementosJugador1);
        ResetearBotonesJugador(botonFuego2, botonAgua2, botonPlanta2, botonNormal2, elementosJugador2);
    }

    private void ResetearBotonesJugador(Button fuego, Button agua, Button planta, Button normal, List<string> elementos)
    {
        SetAlpha(fuego, 0.5f);
        SetAlpha(agua, 0.5f);
        SetAlpha(planta, 0.5f);
        SetAlpha(normal, 0.5f);
        elementos.Clear();
    }

    private void SetAlpha(Button boton, float alpha)
    {
        Color color = boton.image.color;
        color.a = alpha;
        boton.image.color = color;
    }
}
