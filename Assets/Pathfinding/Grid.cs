using System.Collections.Generic;

namespace PathFinding
{
    /// <summary>
    /// A 2D grid of nodes we use to find path.
    /// The grid mark which tiles are walkable and which are not.
    /// </summary>
    public class Grid
    {
        // nodes in grid
        public Node[,] nodes;

        // grid size
        int gridSizeX, gridSizeY;

        /// <summary>
        /// Create a new grid without tile prices, eg with just walkable / unwalkable tiles.
        /// </summary>
        /// <param name="walkable_tiles">A 2d array representing which tiles are walkable and which are not.</param>
        public Grid(bool[,] walkable_tiles)
        {
            // create nodes
            CreateNodes(walkable_tiles.GetLength(0), walkable_tiles.GetLength(1));

            // init nodes
            for (int x = 0; x < gridSizeX; x++)
            {
                for (int y = 0; y < gridSizeY; y++)
                {
                    nodes[x, y] = new Node(walkable_tiles[x, y] ? 1.0f : 0.0f, x, y);
                }
            }
        }

        /// <summary>
        /// Create the nodes grid and set size.
        /// </summary>
        /// <param name="width">Nodes grid width.</param>
        /// <param name="height">Nodes grid height.</param>
        private void CreateNodes(int width, int height)
        {
            gridSizeX = width;
            gridSizeY = height;
            nodes = new Node[gridSizeX, gridSizeY];
        }

        /// <summary>
        /// Get all the neighbors of a given tile in the grid.
        /// </summary>
        /// <param name="node">Node to get neighbors for.</param>
        /// <returns>List of node neighbors.</returns>
        public System.Collections.IEnumerable GetNeighbours(Node node, Pathfinding.DistanceType distanceType)
        {
            int x = 0, y = 0;
            switch (distanceType)
            {
                case Pathfinding.DistanceType.Manhattan:
                    y = 0;
                    for (x = -1; x <= 1; ++x)
                    {
                        var neighbor = AddNodeNeighbour(x, y, node);
                        if (neighbor != null)
                            yield return neighbor;
                    }

                    x = 0;
                    for (y = -1; y <= 1; ++y)
                    {
                        var neighbor = AddNodeNeighbour(x, y, node);
                        if (neighbor != null)
                            yield return neighbor;
                    }
                    break;
            }
        }

        /// <summary>
        /// Adds the node neighbour.
        /// </summary>
        /// <returns><c>true</c>, if node neighbour was added, <c>false</c> otherwise.</returns>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="node">Node.</param>
        /// <param name="neighbours">Neighbours.</param>
        Node AddNodeNeighbour(int x, int y, Node node)
        {
            if (x == 0 && y == 0)
            {
                return null;
            }

            int checkX = node.gridX + x;
            int checkY = node.gridY + y;

            if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
            {
                return nodes[checkX, checkY];
            }

            return null;
        }
    }
}