//Created by Galactspace

using UnityEngine;
using System.Collections.Generic;

namespace Core.Controllers
{
    public class CartSpawner : MonoBehaviour
    {
        private bool _autoSpawn;
        private float _spawnTime;

        private List<Transform> _carts = new();

        [SerializeField] private GameObject _cartPrefab;

        [Space]
        [SerializeField] private CubeCollider _spawner;
        [SerializeField] private CubeCollider _destroyer;

        [Space]
        [SerializeField] private bool _onStart;

        [Space]
        [SerializeField] private float _autoSpawnDelay;

        private void Start()
        {
            _spawnTime = Time.time;
            if (_onStart) _autoSpawn = true;
        }

        public void Spawn()
        {
            Transform t = Instantiate(_cartPrefab, _spawner.transform.position, Quaternion.identity, transform).transform;
            _carts.Add(t);
            t.eulerAngles = _spawner.transform.eulerAngles;
        }

        private void Update()
        {
            if (_autoSpawn && Time.time > _spawnTime)
            {
                Spawn();
                _spawnTime = Time.time + _autoSpawnDelay;
            }
        }

        private void FixedUpdate()
        {
            foreach (Transform cart in _carts)
            {
                if (_destroyer.InsideCollider(cart.position))
                {
                    _carts.Remove(cart);
                    Destroy(cart.gameObject); 
                }
            }
        }
    }
}