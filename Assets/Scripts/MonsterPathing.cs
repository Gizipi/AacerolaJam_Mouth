using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public enum AttackStage
{
    none,
    preparing,
    lunging
}

public class MonsterPathing : MonoBehaviour
{

    private const float _monsterTravelTimeMS = 100;

    private const float PREPARE_TIME = 1.5f;
    private const float LUNG_TIME = 2f;

    private const float GAMEOVER_TIME = 2;

    private const float VIGNETTE_INTENSITY = .385f;

    [SerializeField] private Node _starterNode;

    [SerializeField]private Node _currentRestingPlace;

    [SerializeField]private List<Node> _currentPath = new List<Node>();

    [SerializeField] private MonsterPathfinder _pathfinder;
    private Room _roomToPathTo = Room.starter;

    [SerializeField]private PostProcessVolume _postProcessVolume;
    private Vignette _postProcessVolumeVignette;

    private float _timeSinceLastAttack = 0;
    private float _nextAttackTime = 5;
    private float _attackingTime = 0;

    private float _gameOverCount = 0;

    private GameObject _player;



    // Start is called before the first frame update
    void Start()
    {
        _postProcessVolume.profile.TryGetSettings<Vignette>(out _postProcessVolumeVignette);
        _currentPath = _pathfinder.FindLocation(Room.starter, _currentRestingPlace);
    }

    // Update is called once per frame
    void Update()
    {
        EatenPlayer();
        if (GameManager.gameOver)
            return;
        IncreaseTimer();
        CheckToFindPath();
        FollowPath();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player")
            return;

        _player = other.gameObject;
        GameManager.gameOver = true;
    }

    private void EatenPlayer()
    {
        if (!GameManager.gameOver)
            return;

        _gameOverCount = _gameOverCount + Time.deltaTime;

        _postProcessVolumeVignette.intensity.value = _postProcessVolumeVignette.intensity.value + 0.001f;

        if(_gameOverCount >= GAMEOVER_TIME)
        {

            _gameOverCount = 0;
            transform.position = _starterNode.transform.position;
            _currentRestingPlace = _starterNode;
            _player.GetComponent<Player>().ReturnToStart();
            GameManager.attackStage = AttackStage.none;
            GameManager.gameOver = false;
            GameManager.dead = false;
        }
    }

    private void IncreaseTimer()
    {
        _timeSinceLastAttack += Time.deltaTime;

        if (_timeSinceLastAttack < _nextAttackTime || _currentPath.Count > 0)
            return;
        Attack();
    }

    private void Attack()
    {
        _attackingTime += Time.deltaTime;

        _postProcessVolumeVignette.intensity.value = _postProcessVolumeVignette.intensity.value + 0.001f;

        if (GameManager.attackStage == AttackStage.none)
        {
            GameManager.attackStage = AttackStage.preparing;
            //play prepare animation and sound also increase size of vignette 
        }

        if(GameManager.attackStage == AttackStage.preparing && _attackingTime >= PREPARE_TIME)
        {
            GameManager.attackStage = AttackStage.lunging;
        }

        if(GameManager.attackStage == AttackStage.lunging && !GameManager.gameOver)
        {
            transform.position = transform.position + (transform.forward / 15);
            if(_attackingTime >= LUNG_TIME)
            {
               
                _timeSinceLastAttack = 0;
                _attackingTime = 0;
                _postProcessVolumeVignette.intensity.value = VIGNETTE_INTENSITY;
                _nextAttackTime = Random.Range(8, 15);

                Debug.Log($"Finish lunge, next lunge should happen in {_nextAttackTime}");

                GameManager.attackStage = AttackStage.none;

            }

        }

    }

    public void SetRoomToTravelTo(Room room)
    {
        _roomToPathTo = room;
    }

    private void CheckToFindPath()
    {
        if (_roomToPathTo == _currentRestingPlace.roomNumber)
            return;
        if (_currentPath.Count > 0 || GameManager.attackStage != AttackStage.none)
            return;

        _currentPath = _pathfinder.FindLocation(_roomToPathTo, _currentRestingPlace);
    }

    private void FollowPath()
    {
        if (_currentPath.Count <= 0)
        {
            EnsureRemainNearCurrentNode();
            return;
        }
        if (Vector3.Distance(transform.position, _currentPath[0].transform.position) < 0.2f)
        {
            _currentRestingPlace = _currentPath[0];
            _currentPath.RemoveAt(0);
            return;
        }


       this.transform.position = this.transform.position + (( _currentPath[0].transform.position - this.transform.position) / (_monsterTravelTimeMS / _currentPath.Count));
    }

    private void EnsureRemainNearCurrentNode()
    {
       if( Vector3.Distance(transform.position, _currentRestingPlace.transform.position) < 0.2f)
                return;

        this.transform.position = this.transform.position + ((_currentRestingPlace.transform.position - this.transform.position) / _monsterTravelTimeMS);
    }
}
