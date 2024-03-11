using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MonsterPathing : MonoBehaviour
{

    [SerializeField] private float _monsterTravelTimeMS;
    [SerializeField]private Node _currentRestingPlace;

    [SerializeField]private List<Node> _currentPath = new List<Node>();

    [SerializeField] private MonsterPathfinder pathfinder;
    private Room _roomToPathTo = Room.starter;

    // Start is called before the first frame update
    void Start()
    {
        _currentPath = pathfinder.FindLocation(Room.starter, _currentRestingPlace);
    }

    // Update is called once per frame
    void Update()
    {
        CheckToFindPath();
        FollowPath();
    }

    public void SetRoomToTravelTo(Room room)
    {
        _roomToPathTo = room;
    }

    private void CheckToFindPath()
    {
        if (_roomToPathTo == _currentRestingPlace.roomNumber)
            return;
        if (_currentPath.Count > 0)
            return;

        _currentPath = pathfinder.FindLocation(_roomToPathTo, _currentRestingPlace);
    }

    private void FollowPath()
    {
        if (_currentPath.Count <= 0)
            return;
        if (Vector3.Distance(transform.position, _currentPath[0].transform.position) < 0.2f)
        {
            _currentRestingPlace = _currentPath[0];
            _currentPath.RemoveAt(0);
            return;
        }


       this.transform.position = this.transform.position + (( _currentPath[0].transform.position - this.transform.position) / (_monsterTravelTimeMS / _currentPath.Count));
    }
}
