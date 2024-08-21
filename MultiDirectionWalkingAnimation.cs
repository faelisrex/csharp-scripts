using UnityEngine;

public class MultiDirectionWalkingAnimation : MonoBehaviour
{
    [SerializeField] private Sprite[] idleSprites;
    [SerializeField] private Sprite[] walkWestSprites;
    [SerializeField] private Sprite[] walkSouthwestSprites;
    [SerializeField] private Sprite[] walkSouthSprites;
    [SerializeField] private Sprite[] walkSoutheastSprites;
    [SerializeField] private Sprite[] walkEastSprites;
    [SerializeField] private Sprite[] walkNortheastSprites;
    [SerializeField] private Sprite[] walkNorthSprites;
    [SerializeField] private Sprite[] walkNorthwestSprites;

    public float framesPerSecond = 10f;

    private SpriteRenderer spriteRenderer;
    private Vector2 lastPosition;
    private float timer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        lastPosition = transform.position; // Initialize last position to current position
    }

    void Update()
    {
        Vector2 currentPosition = transform.position;
        Vector2 movementDirection = (currentPosition - lastPosition).normalized;

        // Determine which sprites to use based on movement direction
        if (movementDirection == Vector2.zero)
        {
            AnimateSprite(idleSprites);
        }
        else if (movementDirection.x < 0 && movementDirection.y == 0)
        {
            AnimateSprite(walkWestSprites);
        }
        else if (movementDirection.x < 0 && movementDirection.y < 0)
        {
            AnimateSprite(walkSouthwestSprites);
        }
        else if (movementDirection.x == 0 && movementDirection.y < 0)
        {
            AnimateSprite(walkSouthSprites);
        }
        else if (movementDirection.x > 0 && movementDirection.y < 0)
        {
            AnimateSprite(walkSoutheastSprites);
        }
        else if (movementDirection.x > 0 && movementDirection.y == 0)
        {
            AnimateSprite(walkEastSprites);
        }
        else if (movementDirection.x > 0 && movementDirection.y > 0)
        {
            AnimateSprite(walkNortheastSprites);
        }
        else if (movementDirection.x == 0 && movementDirection.y > 0)
        {
            AnimateSprite(walkNorthSprites);
        }
        else if (movementDirection.x < 0 && movementDirection.y > 0)
        {
            AnimateSprite(walkNorthwestSprites);
        }

        // Update last position
        lastPosition = currentPosition;
    }

    void AnimateSprite(Sprite[] sprites)
    {
        if (sprites.Length == 0)
            return;

        int index = (int)(timer * framesPerSecond);  // Calculate the frame index
        index = index % sprites.Length;  // Loop the animation within the array

        // Set the sprite
        spriteRenderer.sprite = sprites[index];

        // Update the timer
        timer += Time.deltaTime;

        if ((Vector2)transform.position == lastPosition) // Reset timer if idle
        {
            timer = 0f;
        }
    }
}
