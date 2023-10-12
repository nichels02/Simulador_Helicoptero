using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public Transform player;
    public Camera camera;
    public Transform UIPlayer;
    public GameObject Minimap;
    public GameObject prefabEnemyMove;
    public GameObject prefabEnemyStatic;
    // Start is called before the first frame update
    void Start()
    {

        GameObject[] listEnemyStatic = GameObject.FindGameObjectsWithTag("EnemyStatic");
        foreach (var item in listEnemyStatic)
        {
            GameObject canvas = Instantiate(prefabEnemyStatic, item.transform);
            canvas.transform.localPosition = Vector3.up * 50;
        }
        GameObject[] listEnemyMove = GameObject.FindGameObjectsWithTag("EnemyMove");
        foreach (var item in listEnemyMove)
        {
            GameObject canvas = Instantiate(prefabEnemyMove, item.transform);
            canvas.transform.localPosition = Vector3.up * 50;
        }

    }
    private void Update()
    {
        if (InputsHelicopter.instance.Minimap && !Minimap.activeSelf)
            Minimap.SetActive(true);
        else
        if (!InputsHelicopter.instance.Minimap && Minimap.activeSelf)
            Minimap.SetActive(false);
    }
    // Update is called once per frame
    void LateUpdate()
    {
        camera.fieldOfView = Mathf.Clamp(camera.fieldOfView, 20, 130);
        if(player!=null)
            UIPlayer.position = new Vector3(player.position.x, player.position.y + 50, player.position.z);

    }
    
}
