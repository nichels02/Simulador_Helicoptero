using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class El_controlador : MonoBehaviour
{
    [SerializeField] GameObject ElPuntoDeControl;
    [SerializeField] List<Vector3> UbicacionesDePuntos;
    [SerializeField] List<GameObject> TodosLosPuntos;
    int posicion = 0;

    public int Posicion
    {
        get { return posicion; }
        set { posicion = value; }
    }

    private void Awake()
    {
        Vector3 rotacionCalculada1;
        for(int i=0; i < UbicacionesDePuntos.Count; i++)
        {

            GameObject punto_Temp = Instantiate(ElPuntoDeControl, UbicacionesDePuntos[i], Quaternion.identity);
            TodosLosPuntos.Add(punto_Temp);

            #region flecha
            if (i != TodosLosPuntos.Count)
            {
                rotacionCalculada1=UbicacionesDePuntos[i+1]-punto_Temp.transform.position;
                punto_Temp.GetComponent<punto_de_control>().Flecha.transform.rotation = Quaternion.LookRotation(rotacionCalculada1);
            }
            else
            {

            }
            #endregion

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



    public void cambiarElPunto()
    {
        posicion += 1;
        TodosLosPuntos[posicion].SetActive(true);


    }
}
