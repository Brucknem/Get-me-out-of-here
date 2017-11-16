﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateMap3D : CreateMap
{
    [Header("GameObjects")]
    public GameObject tile;

 
    // Use this for initialization
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void AdjustMap()
    {
        float tileDim = Mathf.Min(1.0f * mapHeight / GameData3D.Instance.currentHeight, 1.0f * mapWidth / GameData3D.Instance.currentWidth, 30);

        foreach (KeyValuePair<Vector2, GameObject> tile in tiles)
        {
            tile.Value.SetActive(false);
            
            tile.Value.transform.GetChild(0).gameObject.SetActive(false);
            tile.Value.transform.GetChild(1).gameObject.SetActive(false);
        }
        
        for (int x = 0; x < GameData3D.Instance.currentWidth; x++)
        {
            for (int y = 0; y < GameData3D.Instance.currentHeight; y++)
            {
                Vector2 pos = new Vector2(x, y);
                
                if (!tiles.ContainsKey(pos))
                {
                    tiles.Add(pos, Instantiate(tile));
                    tiles[pos].transform.SetParent(/*map.*/transform);
                    tiles[pos].GetComponent<Renderer>().material.color = GameData3D.Instance.CostToColor(GameData3D.Instance.grid[x, y]);
                }
                tiles[pos].SetActive(true);
                tiles[pos].transform.position = new Vector3(x, 0, y);

                if (GameData3D.Instance.goals.Contains(pos))
                {
                    tiles[pos].GetComponent<Renderer>().material.color = Color.blue;
                }
                else if (GameData3D.Instance.grid[x, y] == GameData3D.Instance.MaxCost)
                {
                    tiles[pos].GetComponent<Renderer>().material.color = Color.black;
                    tiles[pos].transform.GetChild(1).gameObject.SetActive(true);
                }
                else
                {
                    tiles[pos].GetComponent<Renderer>().material.color = GameData3D.Instance.CostToColor(GameData3D.Instance.grid[(int)pos.x, (int)pos.y]);
                    tiles[pos].transform.GetChild(2).transform.GetChild(0).GetComponent<InputField>().text = GameData3D.Instance.grid[(int)pos.x, (int)pos.y].ToString();
                }
            }
        }
    }
    
    public override void ShowCostTable()
    {
        for (int x = 0; x < GameData3D.Instance.currentWidth; x++)
        {
            for (int y = 0; y < GameData3D.Instance.currentHeight; y++)
            {
                Vector2 pos = new Vector2(x, y);
                tiles[pos].SetActive(true);
                if (GameData3D.Instance.value[x, y] == GameData3D.Instance.MaxCost)
                {
                    tiles[pos].GetComponent<Renderer>().material.color = Color.black;
                    tiles[pos].transform.GetChild(0).gameObject.SetActive(false);
                    tiles[pos].transform.GetChild(1).gameObject.SetActive(true);
                }
                else
                {
                    tiles[pos].transform.GetChild(0).gameObject.SetActive(true);
                    if (GameData3D.Instance.policy[x, y] == '*')
                    {
                        tiles[pos].GetComponent<Renderer>().material.color = Color.blue;
                        tiles[pos].transform.GetChild(0).transform.rotation = Quaternion.Euler(0, 0, -90);
                        tiles[pos].transform.GetChild(0).transform.localScale = new Vector3(2, .4f, .4f);
                        tiles[pos].transform.GetChild(0).transform.localPosition = new Vector3(0, 2.5f, 0);
                    }
                    else
                    {
                        tiles[pos].GetComponent<Renderer>().material.color = GameData3D.Instance.CostToColor(GameData3D.Instance.grid[x,y]);
                        int rotation = Array.IndexOf(GameData3D.Instance.deltaNames, GameData3D.Instance.policy[x, y]);
                        tiles[pos].transform.GetChild(0).transform.rotation = Quaternion.Euler(-90, 90 * rotation, 0);
                        tiles[pos].transform.GetChild(0).transform.localScale = new Vector3(.4f, .4f, 1);
                        tiles[pos].transform.GetChild(0).transform.localPosition = new Vector3(0, 1, 0);
                    }
                }
            }
        }
    }
}
