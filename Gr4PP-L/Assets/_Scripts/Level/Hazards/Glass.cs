using UnityEngine;

namespace Level
{
    public class Glass : Respawnable
    {

        [SerializeField]
        [Tooltip("How fast the player must be moving to break the glass")]
        private float _threshold;
        [SerializeField]
        [Tooltip("The direction the player will be breaking through the glass from")]
        private bool _isHorizontal;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.transform.tag != "Player") return;
            GameObject character = collision.gameObject;

            if (_isHorizontal && collision.gameObject.CompareTag("Player") && Mathf.Abs(collision.relativeVelocity.x) > _threshold)
            {
                character.GetComponent<Rigidbody2D>().velocity = collision.relativeVelocity; // ensures player doesn't lose velocity on contact
                Shatter();
                return;
            }

            if (collision.gameObject.CompareTag("Player") && Mathf.Abs(collision.relativeVelocity.y) > _threshold)
            {
                character.GetComponent<Rigidbody2D>().velocity = collision.relativeVelocity; // ensures player doesn't lose velocity on contact
                Shatter();
            }

        }

        private void Shatter()
        {
            GameManager.Instance.Get<Managers.AudioManager>().PlayVariantPitch("Glass Break " + GameManager.Random.Next(2));
            Deactivate();
        }
    }
}