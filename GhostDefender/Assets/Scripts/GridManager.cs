using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    private Dictionary<String, Tilemap> _tilemaps;

    public static GridManager Instance;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            _tilemaps = new Dictionary<string, Tilemap>();


            Tilemap[] maps = FindObjectsOfType<Tilemap>();
            for (int i = 0; i < maps.Length; i++)
            {
                _tilemaps.Add(maps[i].gameObject.name, maps[i]);
            }
            
            return;
        }

        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    public bool HasTile(String[] tileMapNames, Vector3 worldPos)
    {
        for (int i = 0; i < tileMapNames.Length; i++)
        {
            String tileMapName = tileMapNames[i];
            if (!_tilemaps.ContainsKey(tileMapName))
            {
                Debug.LogError("THERE IS NOT GRID WITH THE NAME " + tileMapName, this);
                continue;
            }

            Tilemap tilemap = _tilemaps[tileMapName];
            Vector3Int pos = tilemap.layoutGrid.WorldToCell(worldPos);
            if (tilemap.HasTile(pos))
            {
                return true;
            }
        }

        return false;
    }
    
    public Tilemap GetTileMap(String name)
    {
        return _tilemaps[name];
    }
}
