using UnityEngine;

public class EnemyController : MonoBehaviour
{
	[Header("Sprites")]
	[SerializeField] private SpriteRenderer caparazon_0;
	[SerializeField] private SpriteRenderer tortuga_0;
	[SerializeField] private CapsuleCollider2D collidertortuga;

	[Header("Movimiento")]
	[SerializeField] private float moveSpeed = 1f;

	[Header("Bordes")]
	public float leftLimit = -9.0f;
	public float rightLimit = 9.0f;

	private Rigidbody2D rb;
	private int moveDirection = 1;
	private bool flipped = false;

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		collidertortuga = GetComponent<CapsuleCollider2D>();
	}

	private void Start()
	{
		tortuga_0.enabled = true;
		caparazon_0.enabled = false;
		SetSpriteFlip();
	}

	private void FixedUpdate()
	{
		if (flipped)
		{
			rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
			return;
		}

		// Movimiento constante
		rb.linearVelocity = new Vector2(moveSpeed * moveDirection, rb.linearVelocity.y);

		if (transform.position.x > rightLimit)
		{
			transform.position = new Vector2(leftLimit + 0.2f, transform.position.y);
		}
		if (transform.position.x < leftLimit)
		{
			transform.position = new Vector2(rightLimit - 0.2f, transform.position.y);
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		// Choque con otro enemigo
		if (collision.gameObject.CompareTag("Enemy"))
		{
			if (!flipped)
				TurnAround();
			return;
		}

		// Choque con jugador
		if (collision.gameObject.CompareTag("Player"))
		{
			PlayerController player = collision.gameObject.GetComponent<PlayerController>();
			if (player == null) return;

			if (flipped)
			{
				Die();
			}
			else
			{
				player.Die();
			}
		}

		if (collision.gameObject.CompareTag("TuberiaIzq"))
		{
			transform.position = new Vector3(-7.05f, 3.69f, 0f);
			TurnAround();
		}

		if (collision.gameObject.CompareTag("TuberiaDer"))
		{
			transform.position = new Vector3(6.6f, 3.56f, 0f);
			TurnAround();
		}
	}

	private void TurnAround()
	{
		moveDirection *= -1;
		SetSpriteFlip();
	}

	private void SetSpriteFlip()
	{
		bool shouldFlip = moveDirection < 0;

		if (tortuga_0.enabled)
			tortuga_0.flipX = shouldFlip;
		else
			caparazon_0.flipX = shouldFlip;
	}

	public void Flip()
	{
		if (flipped) return;

		flipped = true;

		// Cambiar sprites
		tortuga_0.enabled = false;
		caparazon_0.enabled = true;


		rb.linearVelocity = Vector2.zero;
	}

	private void Die()
	{
		Destroy(collidertortuga);
		Destroy(gameObject, 2f);
	}
}
