using UnityEngine;

public class HurtAnimation : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        // Get the Animator component attached to this GameObject
        animator = GetComponent<Animator>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collided object is tagged as "Arrow"
        if (collision.gameObject.CompareTag("Arrow"))
        {
            // Trigger the "hurt" animation if an Animator component is found
            if (animator != null)
            {
                animator.SetTrigger("hurt");
            }
        }
    }
}
