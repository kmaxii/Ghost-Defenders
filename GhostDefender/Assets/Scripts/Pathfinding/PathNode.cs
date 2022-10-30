using UnityEngine;

namespace Pathfinding
{
    public class PathNode
    {

        public Vector3Int TilePosition;
        public float G;
        private readonly float _h;
        private float _goalY;

        public PathNode Previous;

        public float f
        {
            get
            {
                float distanceToRightY = _goalY - TilePosition.y;
                distanceToRightY = distanceToRightY >= 0 ? distanceToRightY : distanceToRightY * -1;
                distanceToRightY *= 1000;
                return _h + G + distanceToRightY;
            }
        }

        public PathNode(Vector3Int tilePosition, float g, float h, PathNode previous, float goalY)
        {
            this.TilePosition = tilePosition;
            this.G = g;
            this._h = h;
            Previous = previous;
            _goalY = goalY;
        }
    }
}