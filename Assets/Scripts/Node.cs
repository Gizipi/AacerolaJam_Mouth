using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum Room
{
    starter,
    generator,
    hall1,
    hall2,
    spare,
    closet,
    vent1,
    vent2,
    stairs
}

public class Node : MonoBehaviour
{

    public NodeData data = new NodeData();
    [SerializeField] private bool _restingPlace = false;
    [SerializeField] private Room _roomNumber = Room.starter;

    [SerializeField] private List<Node> _forwardNeighbors = new List<Node>();
    [SerializeField] private List<Node> _backwardNeighbors = new List<Node>();

    private void Start()
    {
        GlobalData.allNodes.Add(this);
    }

    public bool restingPlace() { return _restingPlace; }
    public Room roomNumber() { return _roomNumber; }
    public List<Node> forwardNeighbors() { return _forwardNeighbors; }
    public List<Node> backwardNeighbors() { return _backwardNeighbors; }
}

public class NodeData
{
    public float cost = 1;
    public bool occupied = false;
}