using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class PlayerController : MonoBehaviour
{
	[Header("Movimiento del jugador")]
	public float speed = 5.0f;
	public float jumpForce = 7.0f;
	public float moveHorizontal;

	[Header("Comprobación suelo")]
	public Transform groundCheck;
	public float groundCheckDistance = 0.15f;
	public LayerMask groundLayer;


	private Rigidbody2D rb;
	private bool jumpPressed;

	[Header("Bordes")]
	public float leftLimit = -9.0f;
	public float rightLimit = 9.0f;

	[Header("Respawn")]
	public Vector2 respawnPosition = new Vector2(0f, 1f);
	public float respawnDelay = 1.5f;
	private bool isDead = false;

	[Header("Respawn Visual")]
	public int blinkCount = 4;
	public float blinkInterval = 0.2f;
	[SerializeField] private SpriteRenderer spriteRenderer;
	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	void Update()
	{
		if (isDead) return;

		moveHorizontal = Input.GetAxis("Horizontal");

		// Girar el sprite del jugador según la dirección del movimiento
		if (moveHorizontal < 0)
		{
			transform.rotation = Quaternion.Euler(0, 0, 0);
		}
		else if (moveHorizontal > 0)
		{
			transform.rotation = Quaternion.Euler(0, 180, 0);
		}

		// Comprobar los límites horizontales y teletransportar al jugador al otro lado
		if (transform.position.x > rightLimit)
		{
			transform.position = new Vector2(-9.0f, transform.position.y);
		}
		if (transform.position.x < leftLimit)
		{
			transform.position = new Vector2(9.0f, transform.position.y);
		}

		if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
		{
			Debug.Log("Jump button pressed");
			jumpPressed = true;
		}
	}

	void FixedUpdate()
	{
		if (isDead) return;

		rb.linearVelocity = new Vector2(moveHorizontal * speed, rb.linearVelocity.y);
		if (jumpPressed)
		{
			//si (IsGrounded())
			rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
			jumpPressed = false;
		}
	}

	bool IsGrounded()
	{
		RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, groundLayer);
		return hit.collider != null;
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Bloque"))
		{
			Debug.Log("Jugador ha colisionado con un bloque");
			if (collision.contacts[0].normal.y < -0.5f)
			{
				Debug.Log("Colisión desde abajo");
				Block block = collision.collider.GetComponent<Block>();
				if (block != null)
				{
					block.HitFromBelow();
				}
			}
		}
	}

	public void Die()
	{
		if (isDead) return;

		isDead = true;
		Debug.Log("Jugador muere");

		 rb.linearVelocity = Vector2.zero;
		 rb.simulated = false;

		GetComponent<Collider2D>().enabled = false;
		StartCoroutine(RespawnCoroutine());
	}

	private IEnumerator RespawnCoroutine()
	{
		transform.position = respawnPosition;

		for (int i = 0; i < blinkCount; i++)
		{
			spriteRenderer.enabled = false;
			yield return new WaitForSeconds(blinkInterval);
			spriteRenderer.enabled = true;
			yield return new WaitForSeconds(blinkInterval);
		}

		rb.simulated = true;
		GetComponent<Collider2D>().enabled = true;
		isDead = false;
	}
}