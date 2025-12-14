using UnityEngine;

public class Block : MonoBehaviour
{
    public float checkDistance = 0.3f;
    public LayerMask enemyLayer;

    public void HitFromBelow()
    {
        Vector2 origin = (Vector2)transform.position + Vector2.up * 0.5f;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, checkDistance, enemyLayer);
        Debug.DrawRay(origin, Vector2.up * checkDistance, Color.red, 1.0f);

        if (hit.collider != null)
        {
            EnemyController enemy = hit.collider.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.Flip();
            }
        }
    }
}