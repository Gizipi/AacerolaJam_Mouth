using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterPathfinder : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public static List<Vector3> FindLocation(Vector2 destination, Vector2 startLocation)
    {

        //Debug.Log("find location");
        Dictionary<int, Costs> open = new Dictionary<int, Costs>();
        Dictionary<int, Costs> closed = new Dictionary<int, Costs>();

        int start;
        int goal;
        int current;

        int neighborIndex;
        Costs currentCost;
        (int x, int y) location;


        start = FindIndex(startLocation);
        goal = FindIndex(destination);

        if (goal < 0 || goal >= GlobalData.allNodes.Count || destination.x >= GlobalData.width || destination.x < 0)
            return new List<Vector3>();

        current = start;

        bool search = true;

        Costs neighborCost = new Costs();
        neighborCost.parentIndex = start;

        neighborCost.sCost = 0;
        neighborCost.gCost = Vector2.Distance(startLocation, destination);
        neighborCost.fCost = neighborCost.sCost + neighborCost.gCost + GlobalData.allNodes[neighborCost.parentIndex].data.cost;

        open.Add(start, neighborCost);

        int debugCount = 0;

        while (search)
        {
            float currentLowestCost = Mathf.Infinity;
            debugCount++;

            foreach (var cost in open)
            {
                float currentCheckedCost = (open[cost.Key].fCost);
                if (currentCheckedCost < currentLowestCost)
                {

                    currentLowestCost = currentCheckedCost;
                    current = cost.Key;
                }
            }

            location = FindLocation(current);

            if (current == goal)
            {
                search = false;
                return CollectPath(current, start, closed, open[current]);
            }

            currentCost = open[current];



            for (int x = -1; x < 2; x++)
            {
                for (int y = -1; y < 2; y++)
                {

                    (int x, int y) NeighborLocation = (location.x + x, location.y + y);
                    neighborIndex = CollectIndex(NeighborLocation);

                    if (neighborIndex < 0 ||
                        neighborIndex == current ||
                        neighborIndex >= GlobalData.allNodes.Count ||
                        NeighborLocation.x >= GlobalData.width ||
                        NeighborLocation.x < 0 ||
                        closed.ContainsKey(neighborIndex))
                        continue;

                    neighborCost.parentIndex = current;
                    neighborCost.sCost = currentCost.sCost + Mathf.Clamp((1 * Mathf.Abs(x)) + (1 * Mathf.Abs(y)), 1, 1.414213562373095f);
                    neighborCost.gCost = Vector2.Distance(new Vector2(NeighborLocation.x, NeighborLocation.y), destination);
                    neighborCost.fCost = neighborCost.gCost + neighborCost.sCost + GlobalData.allNodes[neighborIndex].data.cost;

                    if (open.ContainsKey(neighborIndex))
                    {
                        if (neighborCost.fCost <= open[neighborIndex].fCost)
                            open[neighborIndex] = neighborCost;
                    }
                    else
                        open.Add(neighborIndex, neighborCost);
                }
            }

            open.Remove(current);
            closed.Add(current, currentCost);
        }

        return new List<Vector3>();
    }

    public static int FindIndex(Vector2 location)
    {
        return (int)((Mathf.Round(location.y) * GlobalData.width) + Mathf.Round(location.x));
    }

    private static int CollectIndex((int x, int y) location) => (location.y * GlobalData.width) + location.x;

    private static List<Vector3> CollectPath(int current, int start, Dictionary<int, Costs> closed, Costs currentCost)
    {
        bool collectingPath;
        List<Vector3> path = new List<Vector3>();

        (int x, int y) location = FindLocation(current);

        collectingPath = true;
        path.Add(new Vector3(location.x, location.y));

        while (collectingPath)
        {
            if (closed.ContainsKey(current))
                current = closed[current].parentIndex;
            else
                closed.Add(current, currentCost);
            location = FindLocation(current);

            path.Insert(0, new Vector2(location.x, location.y));
            if (current == start)
            {
                collectingPath = false;
            }
        }

        return path;
    }

    public static (int, int) FindLocation(int index, int? width = null)
    {
        int actualWidth = width ?? GlobalData.width;
        return (index % actualWidth, index / actualWidth);
    }

}



public struct Costs
{
    public int parentIndex;
    public float sCost;
    public float gCost;
    public float fCost;
}


public static class GlobalData
{
    public static List<Node> allNodes = new List<Node>();
    public static int width = 0;
    public static int height = 0;
}
