using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class NodesList
{
    public List<Node> nodes;
}

public class MonsterPathfinder : MonoBehaviour
{

    [SerializeField]private List<NodesList> _roomNodes = new List<NodesList>();

    public List<Node> FindLocation(Room destination, Node startLocation)
    {

        //Debug.Log("find location");
        Dictionary<Node, Costs> open = new Dictionary<Node, Costs>();
        Dictionary<Node, Costs> closed = new Dictionary<Node, Costs>();

        int start;
        Node goal;
        Node current;

        int rand = Random.Range(0, _roomNodes[(int)destination].nodes.Count);

        current = startLocation;
        goal = _roomNodes[(int)destination].nodes[rand];

        int neighborIndex;
        Costs currentCost;
        Vector3 location;


        bool search = true;

        Costs neighborCost = new Costs();
        neighborCost.parent = startLocation;

        neighborCost.sCost = 0;
        neighborCost.gCost = Vector2.Distance(startLocation.transform.position, goal.transform.position);
        neighborCost.fCost = neighborCost.sCost + neighborCost.gCost + neighborCost.parent.data.cost;

        open.Add(startLocation, neighborCost);

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

            location = current.transform.position;

            if (current == goal)
            {
                search = false;
                return CollectPath(current, startLocation, closed, open[current]);
            }

            currentCost = open[current];

            foreach(var neighbor in current.neighbors())
            {

                if (closed.ContainsKey(neighbor))
                {
                    continue;
                }

                neighborCost.parent = current;
                neighborCost.sCost = currentCost.sCost + 1;
                neighborCost.gCost = Vector2.Distance(neighbor.transform.position, goal.transform.position);
                neighborCost.fCost = neighborCost.gCost + neighborCost.sCost + neighbor.data.cost;

                if (open.ContainsKey(neighbor))
                {
                    if (neighborCost.fCost <= open[neighbor].fCost)
                        open[neighbor] = neighborCost;
                }
                else
                    open.Add(neighbor, neighborCost);
            }

            open.Remove(current);
            closed.Add(current, currentCost);
        }

        return new List<Node>();
    }

    private static List<Node> CollectPath(Node current, Node start, Dictionary<Node, Costs> closed, Costs currentCost)
    {
        bool collectingPath;
        List<Node> path = new List<Node>();

        collectingPath = true;
        path.Add(current);

        while (collectingPath)
        {
            if (closed.ContainsKey(current))
                current = closed[current].parent;
            else
                closed.Add(current, currentCost);

            path.Insert(0, current);
            if (current == start)
            {
                collectingPath = false;
            }
        }

        return path;
    }
}



public struct Costs
{
    public Node parent;
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
