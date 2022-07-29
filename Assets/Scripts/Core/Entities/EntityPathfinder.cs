//Created by Galactspace

using Pathfinding;
using Scriptable.Entities;
using System;
using UnityEngine;

namespace Core.Entities
{
    public class EntityPathfinder
    {
        private Entity _entity;
        private Seeker _seeker;
        private Rigidbody _rigidbody;

        private float _nextUpdate;
        private float _targetStopWalk;

        private float _updateRate;
        private float _stopWalkDistance;
        private float _nextWaypointDistance;

        private Path _path;
        private int _waypointIndex;
        private bool _overwritePath;
        private bool _completedPath = true;
        public bool CompletedPath
        { 
            get => _completedPath;
            private set
            {
                if (value)
                {
                    _path = null;
                    PathCompleted?.Invoke();
                }

                _completedPath = value;
            }
        }

        public Action PathCompleted;

        public Vector3 _tPos;
        public Vector3 TargetPosition
        {
            get => _tPos;
            private set
            {
                _tPos = value;
                Update();
            }
        }

        public Vector3 PathPosition => _path == null || CompletedPath ? _rigidbody.position : _entity.MoveAxis switch
        {
            SnapAxis.X => new Vector3(_path.vectorPath[_waypointIndex].x, _rigidbody.position.y, _rigidbody.position.z),
            SnapAxis.Z => new Vector3(_rigidbody.position.x, _rigidbody.position.y, _path.vectorPath[_waypointIndex].z),
            _ => throw new System.Exception("Right angle should be 0 or 90")
        };

        private Vector3 DeltaPosition => TargetPosition - _rigidbody.position;
        public Vector3 WalkVector => (PathPosition - _rigidbody.position).normalized;
        
        public float TargetDistance => Vector2.Distance(TargetPosition, _rigidbody.position);

        public bool HasPath => _path != null;

        public EntityPathfinder(EnemyConfigurationSo config, Seeker seeker, Rigidbody rigidbody, Entity entity)
        {
            _entity = entity;
            _seeker = seeker;
            _rigidbody = rigidbody;

            _updateRate = config.PathUpdateRate;
            _stopWalkDistance = config.StopWalkDistance;
            _nextWaypointDistance = config.WaypointDistance;
        }

        public void SetupPath(Vector3 position, float stopWalk = -1, bool overwrite = false)
        {
            if (overwrite)
            {
                _seeker.CancelCurrentPathRequest();
                _overwritePath = true;
            }

            if (stopWalk <= 0) _targetStopWalk = _stopWalkDistance;
            else _targetStopWalk = stopWalk;

            TargetPosition = position;
        }

        public bool StopUpdate()
        {
            if (!HasPath || CompletedPath) return true;
            return false;
        }

        public void Tick()
        {
            if (_path == null) return;

            if (Mathf.Abs((PathPosition - _rigidbody.position).magnitude) < _nextWaypointDistance)
            {
                _waypointIndex++;
            }
            if (!CompletedPath && _waypointIndex >= _path.vectorPath.Count)
            {
                CompletedPath = true;
            }
            if (!CompletedPath && Time.time > _nextUpdate)
            {
                _nextUpdate = Time.time + _updateRate;
                Update();
            }
        }

        private void Update()
        {
            if (!_seeker.IsDone() && !_overwritePath && !CompletedPath) return;

            _overwritePath = false;
            _seeker.StartPath(_rigidbody.position, TargetPosition, OnPathCompleted);
        }

        private void OnPathCompleted(Path path)
        {
            if (path.error) return;
            
            CompletedPath = false;

            _path = path;
            _waypointIndex = 0;
        }
    }
}