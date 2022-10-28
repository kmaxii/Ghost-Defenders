using System.Collections;
using Scriptable_objects;
using UnityEngine;

namespace MonoBehaviours
{
    public class SpriteAnimationMonoBehaviour : MonoBehaviour
    {
        private SpriteAnimation _spriteAnimation;

        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void PlayAnimation(SpriteAnimation inputAnimation, Vector3 position)
        {
            _spriteAnimation = inputAnimation;
            transform.position = position;
            transform.localScale = new Vector3(inputAnimation.size, inputAnimation.size, 0);
            StartCoroutine(GoThroughAnimation());
        }

        private IEnumerator GoThroughAnimation()
        {
            for (int i = 0; i < _spriteAnimation.sprites.Length; i++)
            {
                _spriteRenderer.sprite = _spriteAnimation.sprites[i];
                yield return new WaitForSeconds(_spriteAnimation.delayBetweenChanges);
            }
            AnimationPool.Instance.Return(this);
        }
    }
}