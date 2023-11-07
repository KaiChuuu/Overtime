using UnityEngine;

public class Bullet : MonoBehaviour
{
    [HideInInspector] public float speed = 50.0f;
    [HideInInspector] public float duration = 3.0f;
    [HideInInspector] public float damage = 25;
    private Color color = Color.magenta;

    public string opposingTag = string.Empty;

    private Rigidbody bulletbody;
    private BoxCollider boxCollider;

    private Vector3 previous;
    private Vector3 velocity;

    void Awake()
    {
        bulletbody = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        gameObject.GetComponent<Renderer>().material.color = color;
    }

    // Start is called before the first frame update
    void Start()
    {
        bulletbody.MovePosition(transform.position + transform.forward * Time.deltaTime * speed * 1.5f);
        Destroy(gameObject, duration);
        previous = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        bulletbody.MovePosition(transform.position + transform.forward * Time.deltaTime * speed);
        velocity = (transform.position - previous) / Time.deltaTime;
    }

    public void UpdateBulletColor(Color newColor)
    {
        gameObject.GetComponent<Renderer>().material.color = newColor;
    }

    void OnCollisionEnter(Collision collision)
    {
        ContactPoint collider = collision.contacts[0];
        string tag = collider.otherCollider.tag;
        switch (tag)
        {
            case "Wall":
                var direction = Vector3.Reflect(velocity.normalized, collider.normal);
                transform.rotation = Quaternion.LookRotation(direction);
                previous = transform.position;
                break;
            case "Enemy":
            case "Player":
                DamageEnemy(collider, tag);
                break;
            case "PlayerBullet":
            case "EnemyBullet":
                BulletCollision(collider.otherCollider);
                break;
        }
    }

    void DamageEnemy(ContactPoint collider, string tag)
    {
        if (tag == opposingTag)
        {

            EntityHealth targetHealth = collider.otherCollider.gameObject.GetComponent<EntityHealth>();

            if (targetHealth != null) //If it hits a box collider from another part
            {
                targetHealth.TakeDamage(damage);
                Destroy(gameObject);
            }
        } 
    }

    void BulletCollision(Collider collider)
    {
        //Ensure its not destroying same bullet type
        if (collider.tag != transform.tag)
        {
            Destroy(collider.gameObject);
            Destroy(gameObject);
        }
    }
}
