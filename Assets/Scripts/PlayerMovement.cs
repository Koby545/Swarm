using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 0.2f;
    private float nextFireTime = 0f;
    private Rigidbody2D rb;
    private Vector2 movement;
    [HideInInspector] public bool doubleShot = false;
    [HideInInspector] public bool tripleShot = false;
    [HideInInspector] public bool explosiveBullets = false;

    public FixedJoystick joystick;
    private bool isMobile;

    void Start()
    {

        
        rb = GetComponent<Rigidbody2D>();
        isMobile = Input.touchSupported && !Application.isEditor;
        if (joystick != null)
            joystick.gameObject.SetActive(isMobile);

        if (!isMobile)
            Cursor.visible = false;
    }

    void Update()
    {
        if (isMobile)
            HandleMobile();
        else
            HandlePC();
    }

    void HandlePC()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePos - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            Shoot();
        }
    }

    void HandleMobile()
    {
        // תנועה עם Joystick
        movement.x = joystick.Horizontal;
        movement.y = joystick.Vertical;
        Debug.Log("Joystick: " + joystick.Horizontal + ", " + joystick.Vertical);

        // כיוון וירי עם כל מגע מחוץ לג'וייסטיק
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);

            // אם המגע בצד ימין של המסך – כוון ויורה
            if (touch.position.x > Screen.width * 0.3f)
            {
                Vector3 touchWorldPos = Camera.main.ScreenToWorldPoint(touch.position);
                touchWorldPos.z = 0;
                Vector2 direction = (touchWorldPos - transform.position).normalized;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, angle);

                if (Time.time >= nextFireTime)
                {
                    nextFireTime = Time.time + fireRate;
                    Shoot();
                }
            }
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = movement.normalized * moveSpeed;
    }

    void Shoot()
    {
        GameObject b = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        if (explosiveBullets) b.GetComponent<Bullet>().explosive = true;

        if (doubleShot || tripleShot)
        {
            Quaternion leftAngle = firePoint.rotation * Quaternion.Euler(0, 0, 15f);
            Quaternion rightAngle = firePoint.rotation * Quaternion.Euler(0, 0, -15f);
            GameObject b2 = Instantiate(bulletPrefab, firePoint.position, leftAngle);
            GameObject b3 = Instantiate(bulletPrefab, firePoint.position, rightAngle);
            if (explosiveBullets)
            {
                b2.GetComponent<Bullet>().explosive = true;
                b3.GetComponent<Bullet>().explosive = true;
            }
        }

        if (tripleShot)
        {
            Quaternion leftAngle2 = firePoint.rotation * Quaternion.Euler(0, 0, 30f);
            Quaternion rightAngle2 = firePoint.rotation * Quaternion.Euler(0, 0, -30f);
            GameObject b4 = Instantiate(bulletPrefab, firePoint.position, leftAngle2);
            GameObject b5 = Instantiate(bulletPrefab, firePoint.position, rightAngle2);
            if (explosiveBullets)
            {
                b4.GetComponent<Bullet>().explosive = true;
                b5.GetComponent<Bullet>().explosive = true;
            }
        }

        AudioManager.Instance.PlayShoot();
    }
}