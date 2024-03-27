using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : MonoBehaviour
{
    public GameObject _snakePrefab;
    public GameObject _slimePrefab;
    public GameObject _goblinePrefab;

    GameObject _snake;
    GameObject _slime;
    GameObject _goblin;

    // Start is called before the first frame update
    void Start()
    {
        _snake = GameObject.Instantiate(_snakePrefab);
        _slime = GameObject.Instantiate(_slimePrefab);
        _goblin = GameObject.Instantiate(_goblinePrefab);

        GameObject go = new GameObject() { name = "@Monsters" };
        _snake.transform.parent = go.transform;
        _goblin.transform.parent = go.transform;

        _snake.name = _snakePrefab.name;
        _slime.name = _slimePrefab.name;
        _goblin.name = _goblinePrefab.name;

        _slime.AddComponent<PlayerController>();

        Camera.main.GetComponent<CameraController>().Target = _slime;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
