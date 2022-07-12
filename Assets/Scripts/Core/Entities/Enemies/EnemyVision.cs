//Copyright Galactspace 2022

using UnityEngine;

namespace Core.Entities
{
    [RequireComponent(typeof(Collider))]
    public class EnemyVision : MonoBehaviour
    {
        private Material _mat;
        private Color _safeColor;

        public bool IsVisible { get; private set; }

        [SerializeField] private Enemy _enemy;
        [SerializeField] private Color _angryColor;

        private void OnValidate()
        {
            if (_enemy == null && transform.parent != null) _enemy = GetComponentInParent<Enemy>();
        }

        private void Awake()
        {
            _enemy ??= GetComponentInParent<Enemy>();
            _mat = GetComponent<Renderer>().material;
            _safeColor = _mat.GetColor("_StartColor");
        }

        private void OnEnable()
        {
            _enemy.RegisterVision(this);
        }

        private void OnDisable()
        {            
            _enemy.UnregisterVision(this);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.IsPlayer()) return;
            SetColor(_angryColor);
            IsVisible = true;
        }

        private void OnTriggerExit(Collider other)
        {            
            if (!other.IsPlayer()) return;
            SetColor(_safeColor);
            IsVisible = false;
        }

        public void SetColor(Color color) => _mat.SetColor("_StartColor", color);
    }
}