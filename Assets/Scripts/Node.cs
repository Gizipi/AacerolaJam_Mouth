using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum Room
{
    starter,
    generator,
    hall1,
    hall1End,
    hall2,
    spare,
    closet,
    room2Generator,
    stairs
}

public class Node : MonoBehaviour
{

    public NodeData data = new NodeData();
    [SerializeField] private Room _roomNumber = Room.starter;
    public Room roomNumber { get => _roomNumber; }

    [SerializeField] private List<Node> _neighbors = new List<Node>();

    private void Start()
    {
        GlobalData.allNodes.Add(this);
    }
    public List<Node> neighbors() { return _neighbors; }
}

public class NodeData
{
    public float cost = 1;
    public bool occupied = false;
}