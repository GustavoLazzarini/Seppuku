//Copyright Galactspace Studio

using UnityEngine;

namespace Core.Controllers
{
    public class Bubble : MonoBehaviour
    {
        public enum Data { Inside, Top, Bottom }

        public Data BubbleType;

        public Bubble Up;
        public Bubble Down;
        public Bubble Left;
        public Bubble Right;

        public Vector3 Position
        {
            get => transform.position;
            set => transform.position = value;
        }

        public Bubble GetAnyConnected()
        {
            if (Up != null) return Up;
            if (Down != null) return Down;
            if (Left != null) return Left;
            if (Right != null) return Right;
            
            return null;
        }

        private void OnValidate()
        {
            if (Up != null) Up.Down = this;
            if (Down != null) Down.Up = this;
            if (Left != null) Left.Right = this;
            if (Right != null) Right.Left = this;
        }

        private Color GetCol() => BubbleType switch
        { 
            Data.Inside => new Color(1, 0, 0, 1f),
            Data.Top => new Color(0, 1, 1, 1f),
            Data.Bottom => new Color(0, 1, 1, 1f),
            _ => throw new System.Exception()
        };
        
        public void OnDrawGizmosSelected()
        {
            Gizmosf.DrawSphere(transform.position, 0.07f, GetCol());

            if (Up != null) Gizmosf.DrawLine(transform.position, Up.transform.position, GetCol(), 2);
            if (Down != null) Gizmosf.DrawLine(transform.position, Down.transform.position, GetCol(), 2);
            if (Left != null) Gizmosf.DrawLine(transform.position, Left.transform.position, GetCol(), 2);
            if (Right != null) Gizmosf.DrawLine(transform.position, Right.transform.position, GetCol(), 2);
        }
    }
}