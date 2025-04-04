using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class MenuController : MonoBehaviour
{
    [Header("Panel de Inicio")]
    [SerializeField] private GameObject panelInicio;

    [Header("Panel Jugador 1")]
    [SerializeField] private GameObject panelJugador1;
    [SerializeField] private TMP_InputField inputNombre1;
    [SerializeField] private Button botonConfirmar1;

    [SerializeField] private Button botonFuego1;
    [SerializeField] private Button botonAgua1;
    [SerializeField] private Button botonPlanta1;
    [SerializeField] private Button botonNormal1;

    [Header("Panel Jugador 2")]
    [SerializeField] private GameObject panelJugador2;
    [SerializeField] private TMP_InputField inputNombre2;
    [SerializeField] private Button botonConfirmar2;

    [SerializeField] private Button botonFuego2;
    [SerializeField] private Button botonAgua2;
    [SerializeField] private Button botonPlanta2;
    [SerializeField] private Button botonNormal2;

    private string nombreJugador1, nombreJugador2;
    private List<string> elementosJugador1 = new List<string>();
    private List<string> elementosJugador2 = new List<string>();

    void Start()
    {
        // Solo el panel de inicio estará activo
        panelInicio.SetActive(true);
        panelJugador1.SetActive(false);
        panelJugador2.SetActive(false);
    }

    // Método para iniciar el registro de los jugadores
    public void IniciarJuego()
    {
        panelInicio.SetActive(false);
        panelJugador1.SetActive(true);
        ResetearBotones(1);
        botonConfirmar1.interactable = false;
    }

    // Métodos para seleccionar elementos
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

        if (jugador == 1)
            botonConfirmar1.interactable = elementosJugador1.Count > 0;
        else
            botonConfirmar2.interactable = elementosJugador2.Count > 0;
    }

    public void ConfirmarJugador1()
    {
        nombreJugador1 = inputNombre1.text;

        panelJugador1.SetActive(false);
        panelJugador2.SetActive(true);

        ResetearBotones(2);
        botonConfirmar2.interactable = false;
    }

    public void ConfirmarJugador2()
    {
        nombreJugador2 = inputNombre2.text;

        // Guardar datos en PlayerPrefs
        PlayerPrefs.SetString("NombreJugador1", nombreJugador1);
        PlayerPrefs.SetString("ElementoJugador1", string.Join(",", elementosJugador1));

        PlayerPrefs.SetString("NombreJugador2", nombreJugador2);
        PlayerPrefs.SetString("ElementoJugador2", string.Join(",", elementosJugador2));
        PlayerPrefs.Save();

        // Cargar la siguiente escena
        UnityEngine.SceneManagement.SceneManager.LoadScene("EscenaJuego");
    }

    private void ResetearBotones(int jugador)
    {
        if (jugador == 1)
        {
            SetAlpha(botonFuego1, 0.5f);
            SetAlpha(botonAgua1, 0.5f);
            SetAlpha(botonPlanta1, 0.5f);
            SetAlpha(botonNormal1, 0.5f);
            elementosJugador1.Clear();
        }
        else
        {
            SetAlpha(botonFuego2, 0.5f);
            SetAlpha(botonAgua2, 0.5f);
            SetAlpha(botonPlanta2, 0.5f);
            SetAlpha(botonNormal2, 0.5f);
            elementosJugador2.Clear();
        }
    }

    private void SetAlpha(Button boton, float alpha)
    {
        Color color = boton.image.color;
        color.a = alpha;
        boton.image.color = color;
    }
}
