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

    private const float PREPARE_TIME = 0.8f;
    private const float LUNG_TIME = 1.3f;

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
    private float _nextAttackTime = 8;
    private float _attackingTime = 0;

    private float _gameOverCount = 0;

    private GameObject _player;

    [SerializeField]private GameObject _acerolaText;
    [SerializeField] private GameObject _mouth;
    private float _beginTime = 0;




    // Start is called before the first frame update
    void Start()
    {
        _postProcessVolume.profile.TryGetSettings<Vignette>(out _postProcessVolumeVignette);

        _postProcessVolumeVignette.intensity.value = 20;
        //_postProcessVolumeVignette.opacity.value = 1;

    }

    // Update is called once per frame
    void Update()
    {
        BeginningOfGame();
        EatenPlayer();
        if (GameManager.dead || GameManager.gameOver)
        {
            _postProcessVolumeVignette.intensity.value = _postProcessVolumeVignette.intensity.value + 0.001f;
            return;
        }
        IncreaseTimer();
        CheckToFindPath();
        FollowPath();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player")
            return;

        _player = other.gameObject;
        GameManager.dead = true;
    }

    private void BeginningOfGame()
    {
        if (!GameManager.startGame)
            return;

        _beginTime += Time.deltaTime;

        _postProcessVolumeVignette.intensity.value = _postProcessVolumeVignette.intensity.value - (3f * Time.deltaTime);

        if(_beginTime < 5)
        {
            _acerolaText.transform.position = _acerolaText.transform.position + ((Vector3.down / 16) * Time.deltaTime);
        }
        if (_beginTime > 4)
        {
            _mouth.transform.position = _mouth.transform.position + ((Vector3.down / 16) *Time.deltaTime);
        }
    }

    private void EatenPlayer()
    {
        if (!GameManager.dead)
            return;

        _gameOverCount = _gameOverCount + Time.deltaTime;



        if(_gameOverCount >= GAMEOVER_TIME)
        {
            EndAttack();
            _gameOverCount = 0;
            transform.position = _starterNode.transform.position;
            _currentRestingPlace = _starterNode;
            _player.GetComponent<Player>().ReturnToStart();
            _currentPath = _pathfinder.FindLocation(Room.starter, _currentRestingPlace);
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

        if (GameManager.startGame)
        {
            _currentPath = _pathfinder.FindLocation(Room.starter, _currentRestingPlace);
            GameManager.startGame = false;
            EndAttack();
            return;
        }
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

        if(GameManager.attackStage == AttackStage.lunging && (!GameManager.gameOver || GameManager.dead))
        {
            transform.position = transform.position + (transform.forward / 20);
            if(_attackingTime >= LUNG_TIME)
            {
                EndAttack();
            }

        }

    }

    private void EndAttack()
    {
        _timeSinceLastAttack = 0;
        _attackingTime = 0;
        _postProcessVolumeVignette.intensity.value = VIGNETTE_INTENSITY;
        _nextAttackTime = Random.Range(8, 15);

        GameManager.attackStage = AttackStage.none;
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
