using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathFinding;
using PathFindingGrid = PathFinding.Grid;
using System;
using System.Linq;

public class LevelInfo
{
    public int Width { get; }
    public int Height { get; }
    public string[,] TilesMap { get; }


    bool[,] pathFindingInfo;
    PathFindingGrid pathFindingGrid;

    public LevelInfo(string[,] tilesMap)
    {
        this.TilesMap = tilesMap;
        
        Width = tilesMap.GetLength(0);
        Height = tilesMap.GetLength(1);

        pathFindingInfo = new bool[Width, Height];
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                pathFindingInfo[x, y] = TilesMap[x, y] == TilesCodes.WALL ? false : true;
                //Debug.LogWarning(pathFindingInfo[row, col]);
            }
            //Debug.LogWarning("New Line");
        }

        pathFindingGrid = new PathFindingGrid(pathFindingInfo);
    }

    public Point GetNextPoint(Point from, Point to)
    {
        return Pathfinding.FindPath(pathFindingGrid, from, to, Pathfinding.DistanceType.Manhattan).FirstOrDefault();
    }
}
