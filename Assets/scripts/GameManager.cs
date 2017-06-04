using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public GameObject TileTemplate;

    // Use this for initialization
    void Start()
    {
        for (int x = -10; x < 10; x++)
        {
            for (int y = -10; y < 10; y++)
            {
                GameObject newTile = (GameObject)Instantiate(TileTemplate, new Vector3(x, y, .5f), Quaternion.identity);
                newTile.transform.SetParent(transform.Find("Board Tiles"));
            }
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
    void StartBattle()
    {

    }
}
