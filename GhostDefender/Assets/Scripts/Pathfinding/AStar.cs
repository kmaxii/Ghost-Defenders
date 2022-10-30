using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Pathfinding
{
    public class AStar
    {
        private List<PathNode> _open;
        private readonly Dictionary<Vector3Int, PathNode> _openDictionary;
        private readonly Dictionary<Vector3Int, PathNode> _closed;

        private readonly Vector3Int _start;
        private readonly Vector3Int _goal;

        private readonly Tilemap _objects;

        public AStar(Vector3Int start, Vector3Int goal, Tilemap tilemap)
        {
            _start = start;
            _goal = goal;
            _objects = tilemap;
            _open = new List<PathNode>();
            _closed = new Dictionary<Vector3Int, PathNode>();
            _openDictionary = new Dictionary<Vector3Int, PathNode>();
        }

        public List<Vector3> GetPath()
        {
            List<Vector3> bestPath = new List<Vector3>();

            Vector3Int previous = _goal;
            while (true)
            {
                PathNode node = _closed[previous];
                Vector3 worldPos = _objects.CellToWorld(node.TilePosition);
                worldPos.y++;
                worldPos.x += 0.5f;
                bestPath.Add(worldPos);

                if (node.Previous == null)
                {
                    break;
                }
                previous = node.Previous.TilePosition;
            }

            bestPath.Reverse();
            return bestPath;
        }


        public void RunAlgorithm()
        {
            AddStartNode();

            while (_open.Count > 0)
            {
                PathNode bestNode = GetTileWithLowestH();
                MoveToClosed(bestNode);

                if (bestNode.TilePosition == _goal)
                    break;

                CheckNeighbours(bestNode);
            }
        }


        private void MoveToClosed(PathNode node)
        {
            _open.Remove(node);
            _openDictionary.Remove(node.TilePosition);
            _closed.Add(node.TilePosition, node);

        }
        private void CheckNeighbours(PathNode node)
        {
            foreach (var neighbour in GetNeighbours(node.TilePosition))
            {
                //This node neighbour node has already been checked
                if (_closed.ContainsKey(neighbour))
                {
                    PathNode neighborNod = _closed[neighbour];

                    //If this way to the node is faster then the current recorded one
                    if (node.G + 1 < neighborNod.G)
                    {
                        neighborNod.G = node.G + 1;
                        neighborNod.Previous = node;
                        
                        MoveFromClosedToOpen(neighborNod);
                    }

                    continue;
                }

                if (!_openDictionary.ContainsKey(neighbour))
                {
                    PathNode toAdd = new PathNode(neighbour, node.G + 1, GetHeuristic(neighbour), node, _goal.y);
                    _open.Add(toAdd);
                    _openDictionary.Add(neighbour, toAdd);
                    continue;
                }

                PathNode neighbourNode = _openDictionary[neighbour];

                //If this way to the node is faster then the current recorded one
                if (node.G + 1 < neighbourNode.G)
                {
                    neighbourNode.G = node.G + 1;
                    neighbourNode.Previous = node;
                }
            }
        }

        private void MoveFromClosedToOpen(PathNode node)
        {
            _closed.Remove(node.TilePosition);
            _open.Add(node);
            _openDictionary.Add(node.TilePosition, node);
        }

        private List<Vector3Int> GetNeighbours(Vector3Int position)
        {
            List<Vector3Int> neighbours = new List<Vector3Int>();

            Vector3Int pos = new Vector3Int(position.x + 1, position.y, position.z);
            if (IsValidTilePos(pos))
                neighbours.Add(pos);
            pos = new Vector3Int(position.x - 1, position.y, position.z);
            if (IsValidTilePos(pos))
                neighbours.Add(pos);
            pos = new Vector3Int(position.x, position.y + 1, position.z);
            if (IsValidTilePos(pos))
                neighbours.Add(pos);
            pos = new Vector3Int(position.x, position.y - 1, position.z);
            if (IsValidTilePos(pos))
                neighbours.Add(pos);

            return neighbours;
        }

        private bool IsValidTilePos(Vector3Int position)
        {
            return !_objects.HasTile(position);
        }


        private void AddStartNode()
        {
            PathNode startNode = new PathNode(_start, 0, GetHeuristic(_start), null, _goal.y);

            _open.Add(startNode);
            _openDictionary.Add(_start, startNode);
        }

        private void OrderListByH()
        {
            _open = _open.OrderBy(o => o.f).ToList();
        }

        private PathNode GetTileWithLowestH()
        {
            OrderListByH();
            PathNode bestNode = _open[0];
            return bestNode;
        }

        private float GetHeuristic(Vector3Int position)
        {
            Vector3Int distance = _goal - position;
            return distance.x * distance.x + distance.y * distance.y;
        }
    }
}