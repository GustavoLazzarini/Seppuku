//Copyright Galactspace 2022

using UnityEngine;
using Core.Entities;
using UnityEngine.Events;

namespace Core.Controllers
{
    public class RunnerController : MonoBehaviour
    {
        private Protagonist _player;

        private bool _hasStarted;

        [SerializeField] private CubeCollider _startCol;
        [SerializeField] private CubeCollider _endCol;

        [Space]
        [SerializeField] private UnityEvent _onStart;
        [Space]
        [SerializeField] private UnityEvent _onCompleted;

        private void Awake()
        {
            _player = FindObjectOfType<Protagonist>();
        }

        private void OnEnable()
        {
            if (!_hasStarted) _startCol.OnEnter += StartMinigame;
        }

        private void OnDisable()
        {
            if (!_hasStarted) _startCol.OnEnter -= StartMinigame;
        }

        private void StartMinigame()
        {
            if (_hasStarted) return;

            _hasStarted = true;
            _player.SetMoveStage(MoveStage.Runner);

            _player.IsRunning = true;
            _player.MoveVector = new Vector3(1, 0);

            _startCol.OnEnter -= StartMinigame;
            _endCol.OnEnter += EndMinigame;

            _onStart?.Invoke();
        }

        private void EndMinigame()
        {
            if (!_hasStarted) return;

            _endCol.OnEnter -= EndMinigame;

            _player.IsRunning = false;
            _player.CurrentMoveSize = 0;
            _player.MoveVector = new Vector3(0, 0);
            _player.SetMoveStage(MoveStage.Walk);
           
            _onCompleted?.Invoke();
        }
    }
}