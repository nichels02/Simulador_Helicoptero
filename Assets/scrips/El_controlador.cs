using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class El_controlador : MonoBehaviour
{
    [SerializeField] GameObject ElPuntoDeControl;
    [SerializeField] List<Vector3> UbicacionesDePuntos;
    int posicion = 0;

    public int Posicion
    {
        get { return posicion; }
        set { posicion = value; }
    }

    private void Awake()
    {

        for(int i=0; i < UbicacionesDePuntos.Capacity; i++)
        {

            GameObject punto_Temp = Instantiate(ElPuntoDeControl, UbicacionesDePuntos[i], Quaternion.identity);

        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    void cambiarElPunto()
    {

    }
}
