using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Node : MonoBehaviour
{


    [SerializeField] private bool _restingPlace = false;
    [SerializeField] private int _roomNumber = 0;

    [SerializeField] private List<Node> _forwardNeighbors = new List<Node>();
    [SerializeField] private List<Node> _backwardNeighbors = new List<Node>();


    public bool restingPlace() { return _restingPlace; }
    public int roomNumber() { return _roomNumber; }
    public List<Node> forwardNeighbors() { return _forwardNeighbors; }
    public List<Node> backwardNeighbors() { return _backwardNeighbors; }
}
