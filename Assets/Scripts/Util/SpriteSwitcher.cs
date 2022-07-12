//Made by Galactspace Studios

using UnityEngine;
using System.Collections;

namespace Util
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteSwitcher : MonoBehaviour
    {
        private int _now;
        private int CurrentSprite => _now;
        
        private Coroutine _mainCoroutine;
        private SpriteRenderer _renderer;

        private bool IsLastSprite => CurrentSprite + 1 >= sprites.Length;

        [Space]
        [SerializeField] private bool loop = true;
        [SerializeField] private float delay = 1;

        [Space]
        [SerializeField] private Sprite[] sprites;

        private void OnEnable() => StartSwitching();

        private void Awake()
        {            
            _renderer = GetComponent<SpriteRenderer>();
            StartSwitching();
        }

        private void StartSwitching()
        {
            if (_mainCoroutine != null) StopCoroutine(_mainCoroutine);
            _mainCoroutine = StartCoroutine(SwitchCoroutine());

            IEnumerator SwitchCoroutine()
            {
                while (true)
                {
                    yield return new WaitForSeconds(delay);

                    if (!IsLastSprite) NextSprite();
                    else
                    {
                        if (!loop) break;
                        else ResetSprite();
                    }
                }
            }

            void ResetSprite() => SetSprite(0);
            void NextSprite() => SetSprite(CurrentSprite + 1);
        }

        private void SetSprite(int arg)
        {
            int sptIndex = Mathf.Clamp(arg, 0, sprites.Length - 1);
            
            _now = sptIndex;
            _renderer.sprite = sprites[sptIndex];
        }        
    }
}
