using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class punto_de_control : MonoBehaviour
{
    GameObject elControlador;
    int posicion;
    [SerializeField] GameObject flecha;

    public GameObject ElControlador
    {
        get { return elControlador; }
        set { elControlador = value; }
    }
    public int Posicion
    {
        get { return posicion; }
        set { posicion = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "jugador")
        {
            if (elControlador.GetComponent<El_controlador>().Posicion == posicion)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
