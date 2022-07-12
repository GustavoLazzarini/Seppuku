//Copyright Galactspace 2022

using UnityEngine;
using Core.Entities;
using UnityEngine.UIElements;
using Core.Attributes;

namespace Core.Controllers.UI
{
    public class DoorMinigame : UIController
    {
        private float _curGreenArea;
        private float CurGreenArea
        { 
            get => _curGreenArea;
            set
            {
                _curGreenArea = value;
                _greenArea.transform.scale = new Vector3(value, _greenArea.transform.scale.y);
            }
        }

        private int _curStep;
        private bool _completed;

        private float _curSpeed;
        private float _curPos;
        
        private VisualElement _stick;
        private VisualElement _greenArea;
        private VisualElement _backgroundArea;

        [Space]
        [SerializeField] private int _minSize;
        [SerializeField] private int _maxSize;
        [SerializeField] private int _minPosition;
        [SerializeField] private int _maxPosition;

        [Space]
        [SerializeField] private int _steps;
        [SerializeField][Range(0, 1)] private float _decreasePerStep;

        [Space]
        [SerializeField] private float _speed;

        [Space]
        [SerializeField] private GameObject _door;
        [SerializeField] private Protagonist _player;

        [Space]
        [SerializeField][Button] private bool _randomizeArea;

        private bool IsInsideGreenArea => (_stick.transform.position.x - (_stick.transform.scale.x / 2)) > (_greenArea.transform.position.x - (_greenArea.transform.scale.x / 2)) &&
                                            (_stick.transform.position.x + (_stick.transform.scale.x / 2)) < (_greenArea.transform.position.x + (_greenArea.transform.scale.x / 2));

        private void OnValidate()
        {
            if (_randomizeArea)
            {
                RandomizeArea();
                _randomizeArea = false;
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _curSpeed = _speed;

            _stick = GetVisualElement("stick");
            _greenArea = GetVisualElement("greenArea");
            _backgroundArea = GetVisualElement("backgroundArea");

            _curStep = 0;
            CurGreenArea = 40;

            RandomizeArea();
            InputMan.GameInputs.InteractChannel.Link(Interact);
        }

        protected override void OnDisable()
        {
            InputMan.GameInputs.InteractChannel.Unlink(Interact);
            base.OnDisable();
        }

        private void Interact()
        {
            if (!IsInsideGreenArea) return;

            _curStep++;
            if (_curStep >= _steps)
            {
                Completed();
                return;
            }

            RandomizeArea();
        }

        protected override void Update()
        {
            if (_completed) return;
            SetStickPosition(_curPos + (_curSpeed * Time.deltaTime));
            _backgroundArea.style.backgroundColor = new StyleColor(IsInsideGreenArea ? Color.white : Color.red);
            base.Update();
        }

        public void SetStickPosition(float value)
        {
            if (_curPos < -140) _curSpeed = _speed;
            if (_curPos > 140) _curSpeed = -_speed;

            Vector3 cPos = _stick.transform.position;
            _curPos = value;
            _stick.transform.position = new Vector3(value, cPos.y, cPos.z);
        }

        private void Completed()
        {
            _completed = true;
            _door.LerpRotation(this, new Vector3(_door.transform.localEulerAngles.x, 100, _door.transform.localEulerAngles.z), 3f);

            Routinef.Invoke(() => {
                _player.enabled = true;
                _player.gameObject.LerpPosition(this, new Vector3(_player.transform.position.x, _player.transform.position.y, -3.557f), 10f, 0.1f);
                _player.gameObject.LerpRotation(this, new Vector3(0, 90, 0), 10f, 0.1f);
            }, 1, this);

            GetComponent<UIDocument>().rootVisualElement.style.opacity = 0;

            Destroy(gameObject, 20);
        }

        [ContextMenu("Randomize Area")]
        public void RandomizeArea()
        {
            float min = _minSize * (1 - (_decreasePerStep * _curStep));
            float max = _maxSize * (1 - (_decreasePerStep * _curStep));

            CurGreenArea = Random.Range(min, max);
            _greenArea.transform.position = new(Random.Range(_minPosition, _maxPosition), _greenArea.transform.position.y, _greenArea.transform.position.z);
        }
    }
}