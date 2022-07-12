//Made by Galactspace Studios

using UnityEngine;
using System.Collections.Generic;

namespace Util
{
    [RequireComponent(typeof(ParticleSystem))]
    public class ParticleAttacher : MonoBehaviour
    {
        private ParticleSystem _particleSystem;
        private ParticleSystem.Particle[] _particles;
        private List<GameObject> _instances = new List<GameObject>();

        [SerializeField] private GameObject objectPrefab;

        private int InstanceQuantity => _instances.Count;
        private int ParticlesCount => _particleSystem.GetParticles(_particles);

        private bool IsWorldSpace => _particleSystem.main.simulationSpace == ParticleSystemSimulationSpace.World;

        private void AddInstance() => _instances.Add(Instantiate(objectPrefab, _particleSystem.transform));
        
        private void SetInstancePosition(GameObject arg0, Vector3 pos) => arg0.transform.position = pos;
        private void SetInstanceLocalPosition(GameObject arg0, Vector3 pos) => arg0.transform.localPosition = pos;

        private void EnableObject(GameObject obj, bool value) => obj.SetActive(value);

        void Start()
        {
            _particleSystem = GetComponent<ParticleSystem>();
            _particles = new ParticleSystem.Particle[_particleSystem.main.maxParticles];
        }

        void LateUpdate()
        {
            while (InstanceQuantity < ParticlesCount) AddInstance();                
            
            for (int i = 0; i < InstanceQuantity; i++)
            {
                if (i < ParticlesCount)
                {
                    if (IsWorldSpace) SetInstancePosition(_instances[i], _particles[i].position);
                    else  SetInstanceLocalPosition(_instances[i], _particles[i].position);
                    EnableObject(_instances[i], true);
                }
                else EnableObject(_instances[i], true);
            }
        }        
    }
}
