using System;
using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    // References
    public Animator animator;
    public LayerMask enemyLayers;
    public Transform attackPoint;
    public Light2D flashLight;
    public static bool inDungeon = false;
    public int currRoom = 2;
    public int MaxHearts = 10;
    public int Hearts = 10;
    public float staminaProcent = 0;
    public int Components;
    public int WeaponEvo = 0;
    public int Vision = 2;

    public float speed;
    public bool isDashing = false;
    public int damage;
    public int dashDamage;
    public float dashingPower = 12f;
    public float attackCooldown;
    public bool dashUpgraded = false;

    [SerializeField] private GameObject DeathMenu;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private HeartUpdate heartsHUD;
    [SerializeField] private GameObject staminaBar;
    [SerializeField] private GameObject staminaMask;
    [SerializeField] private FloatSO PlayerSO;
    [SerializeField] private Text componentsText;
    [SerializeField] private Text weaponEvoText;
    [SerializeField] private GameObject sweepingPrefab;

    private float stamina;
    private float attackTime;
    private float dashingTime = 0.3f;
    private float dashingCooldown = 3f;

    public float attackRange;
    private string dashDirAnim = "Dash_Down";
    private string Facing = "down";
    private Vector2 movement;

    //Chest Bools
    public bool Card1Picked = false;
    public bool Card2Picked = false;
    public bool Card3Picked = false;
    public bool Card4Picked = false;
    public GameObject LoadingScreen;
    public float timer;

    private float tpCooldown;

    public bool inCutscene;
    public AudioSource swordswing;
    public AudioSource damageTaken;

    private void Start()
    {
        getStoredValues();
        timerUI = GameObject.Find("timerUI").GetComponent<Text>();
    }
    private void getStoredValues()
    {
        Hearts = (int)PlayerSO.Hearts;
        MaxHearts = (int)PlayerSO.MaxHearts;
        Vision = (int)PlayerSO.Vision;

        damage = (int)PlayerSO.Damage;
        dashDamage = (int)PlayerSO.DashDamage;
        attackRange = PlayerSO.AttackRange;
        WeaponEvo = (int)PlayerSO.WeaponEvo;

        Card1Picked = PlayerSO.Card1;
        Card2Picked = PlayerSO.Card2;
        Card3Picked = PlayerSO.Card3;
        Card4Picked = PlayerSO.Card4;
    }
    private void StoreValues()
    {
        PlayerSO.Hearts = (float)Hearts;
        PlayerSO.MaxHearts = (float)MaxHearts;
        PlayerSO.Vision = (float)Vision;

        PlayerSO.Damage = (float)damage;
        PlayerSO.DashDamage = (float)dashDamage;
        PlayerSO.AttackRange = attackRange;
        PlayerSO.WeaponEvo = (float)WeaponEvo;

        PlayerSO.Card1 = Card1Picked;
        PlayerSO.Card2 = Card2Picked;
        PlayerSO.Card3 = Card3Picked;
        PlayerSO.Card4 = Card4Picked;
    }

    public Text timerUI;
    private void Update()
    {
        if (Alive)
        {
            StoreValues();
            flashLight.pointLightOuterRadius = Vision * 3;
            if (inDungeon) timer += Time.deltaTime;
            timerUI.text = "Time: " + (int)timer + " s.";

            // Cooldown
            if (tpCooldown <= 1) tpCooldown += Time.deltaTime;
            if (attackTime <= attackCooldown) attackTime += Time.deltaTime;
            if (stamina < dashingCooldown && WeaponEvo < 4) stamina += Time.deltaTime;
            else if (stamina < dashingCooldown) { stamina += Time.deltaTime * 1.5f; }

            if (stamina < dashingCooldown) UpdateStamina();
            if (trulyDashing) return;

            if (!inCutscene)
            {
                movement.x = Input.GetAxisRaw("Horizontal");
                movement.y = Input.GetAxisRaw("Vertical");
            }

            // Mouse position Calculations and storing for checking where to attack.
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 temp = new Vector3(mousePos.x, mousePos.y, 0);
            Vector3 direction = temp - gameObject.transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;

            // Controls
            if (Input.GetKeyDown(KeyCode.Mouse0) && attackTime >= attackCooldown) StartCoroutine(Hit());
            if (Input.GetKeyDown(KeyCode.Mouse1) && stamina >= 3 && movement.sqrMagnitude != 0) StartCoroutine(Dash());
            if (Input.GetKeyDown(KeyCode.Slash)) { Alert("Cheats activated..."); attackCooldown /= 10; }
            // if (Input.GetKeyDown(KeyCode.H)) HealPot ion();

            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
            animator.SetFloat("Speed", movement.sqrMagnitude);

            switch ((movement.x, movement.y))
            {
                case (1, 0):
                    dashDirAnim = "Dash_Right";
                    break;
                case (-1, 0):
                    dashDirAnim = "Dash_Left";
                    break;
                case (0, 1):
                    dashDirAnim = "Dash_Up";
                    break;
                case (0, -1):
                    dashDirAnim = "Dash_Down";
                    break;
                case (1, 1):
                    Facing = "up-right";
                    dashDirAnim = "Dash_Up";
                    break;
                case (1, -1):
                    Facing = "down-right";
                    dashDirAnim = "Dash_Down";
                    break;
                case (-1, 1):
                    Facing = "up-left";
                    dashDirAnim = "Dash_Up";
                    break;
                case (-1, -1):
                    Facing = "down-left";
                    dashDirAnim = "Dash_Down";
                    break;
            }

            if (angle > -45 && angle < 45) Facing = "up";
            else if (angle > -135 && angle < -45) Facing = "right";
            else if (angle > -225 && angle < -135) Facing = "down";
            else if (angle > -270 && angle < -225 || angle > 45 && angle < 90) Facing = "left";
            else Debug.Log("Error ANGLE");

            if (inCutscene)
            {
                movement.x = 0;
                movement.y = 0;
                rb.velocity = new Vector2(0, 0);
                trulyDashing = false; // Stop dashing
            }
        }
    }
    private void FixedUpdate() // Movement
    {
        if (trulyDashing) return;
        if (movement.magnitude > 1 && speed != 0) rb.velocity = new Vector2(movement.x * (speed - 0.6f), movement.y * (speed - 0.6f));
        else rb.velocity = new Vector2(movement.x * speed, movement.y * speed);

        // Update UI values
        if (WeaponEvo > 4) componentsText.text = Components + "/ MAX";
        else componentsText.text = Components + "/" + CraftingScript.UpgradeCosts[WeaponEvo];
        weaponEvoText.text = "" + WeaponEvo;
    }

    void OnTriggerEnter2D(Collider2D collider) // Door collider
    {
        if (tpCooldown >= 1 && collider.gameObject.tag == "Door" && RoomManager.roomCleared)
        {
            GameObject.Find("Dungeon Generator").GetComponent<RoomManager>().doubleKillPrevention = true;

            transform.position = collider.gameObject.GetComponent<DoorMechanics>().targetDoorPos;
            currRoom = collider.gameObject.GetComponent<DoorMechanics>().targetRoomIndex;
            tpCooldown = 0;

            GameObject.Find("Dungeon Generator").GetComponent<RoomManager>().NewRoom();
        }
    }

    bool trulyDashing;
    private IEnumerator Dash()
    {
        swordswing.Play();
        stamina -= 3;
        trulyDashing = true;
        if (dashUpgraded) isDashing = true;
        if (movement.magnitude > 1)
        {
            rb.velocity = new Vector2(movement.x * (dashingPower - 3), movement.y * (dashingPower - 3));
            animator.Play(dashDirAnim);
        }
        else
        {
            rb.velocity = new Vector2(movement.x * dashingPower, movement.y * dashingPower);
            animator.Play(dashDirAnim);
        }
        yield return new WaitForSeconds(dashingTime);
        trulyDashing = false; isDashing = false;
        yield return new WaitForSeconds(dashingCooldown - dashingTime);
    }

    public void UpgradeEvo()
    {
        switch (WeaponEvo)
        {
            case 0:
                Alert((WeaponEvo + 1) + ": Range +");
                attackRange *= 1.5f;
                damage += 1;
                break;
            case 1:
                Alert((WeaponEvo + 1) + ": Attack Speed +");
                attackCooldown *= 0.7f;
                break;
            case 2:
                Alert((WeaponEvo + 1) + ": Dash Damage unlocked");
                dashUpgraded = true;
                break;
            case 3:
                Alert((WeaponEvo + 1) + ": Double Dash");
                dashingCooldown = 6f;
                break;
            case 4:
                Alert((WeaponEvo + 1) + ": Sweeping Edge Unlocked");
                break;
        }
        WeaponEvo++;
    }

    private IEnumerator Hit()
    {
        swordswing.Play();
        attackTime = 0;
        int randNumb = UnityEngine.Random.Range(0, 1);
        switch (Facing)
        {
            case "right":
                attackPoint.position = gameObject.transform.position + new Vector3(attackRange, 0f, 0f);
                if (randNumb == 0) animator.Play("Attack_Right1"); else animator.Play("Attack_Right2");
                break;
            case "left":
                attackPoint.position = gameObject.transform.position + new Vector3(-attackRange, 0f, 0f);
                if (randNumb == 0) animator.Play("Attack_Left1"); else animator.Play("Attack_Left2");
                break;
            case "up":
                attackPoint.position = gameObject.transform.position + new Vector3(0f, attackRange, 0f);
                animator.Play("Attack_Up");
                break;
            case "down":
                attackPoint.position = gameObject.transform.position + new Vector3(0f, -attackRange, 0f);
                animator.Play("Attack_Down");
                break;
        }
        yield return new WaitForSeconds(0.1f);

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, 1, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.gameObject.GetComponent<EnemyBlob>() != null) enemy.gameObject.GetComponent<EnemyBlob>().TakeDamage(damage);
            else if (enemy.gameObject.GetComponent<EnemyFrog>() != null) enemy.gameObject.GetComponent<EnemyFrog>().TakeDamage(damage);
            else if (enemy.gameObject.GetComponent<EnemySpider>() != null) enemy.gameObject.GetComponent<EnemySpider>().TakeDamage(damage);
            else if (enemy.gameObject.GetComponent<BossCat>() != null) enemy.gameObject.GetComponent<BossCat>().TakeDamage(damage);
            else if (enemy.gameObject.GetComponent<FrogFly>() != null) enemy.gameObject.GetComponent<FrogFly>().Hit();
        }

        // Sweeping Edge
        if (WeaponEvo == 5)
        {
            Quaternion targetRotation;
            Vector3 targetPos;
            switch (Facing)
            {
                default: // Up
                    targetRotation = Quaternion.Euler(0, 0, 0);
                    targetPos = transform.position + new Vector3(0, 1, 0);
                    break;
                case "left":
                    targetRotation = Quaternion.Euler(0, 0, 90);
                    targetPos = transform.position + new Vector3(-1, 0.5f, 0);
                    break;
                case "down":
                    targetRotation = Quaternion.Euler(0, 0, 180);
                    targetPos = transform.position + new Vector3(0, -0.5f, 0);
                    break;
                case "right":
                    targetRotation = Quaternion.Euler(0, 0, 270);
                    targetPos = transform.position + new Vector3(1, 0.5f, 0);
                    break;
            }
            SweepEdge sweep = Instantiate(sweepingPrefab, targetPos, targetRotation).GetComponent<SweepEdge>();
            sweep.damage = damage;
        }
    }

    [SerializeField] private Text textObject;
    private CancellationTokenSource cancellationTokenSource;
    public void Alert(string message)
    {
        if (cancellationTokenSource != null) cancellationTokenSource.Cancel();
        cancellationTokenSource = new CancellationTokenSource();

        StartCoroutine(MessageToPlayer(message, cancellationTokenSource.Token));
    }
    public IEnumerator MessageToPlayer(string message, CancellationToken cancellationToken)
    {
        textObject.text = message;

        float counter = 0f;
        while (counter < 2f)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                yield break; // Exit the coroutine
            }
            yield return null;
            counter += Time.deltaTime;
        }
        textObject.text = "";
    }

    public void HealPotion()
    {
        if (Hearts < MaxHearts)
        {
            Hearts += 1;
            heartsHUD.UpdateHearts();
        }
    }
    public void TakeDamage(int amount)
    {
        damageTaken.Play();
        Hearts -= amount;
        heartsHUD.UpdateHearts();
        StartCoroutine(DamageFlash());
        if (Hearts <= 0) StartCoroutine(Death());
    }
    void UpdateStamina()
    {
        staminaProcent = stamina / dashingCooldown;
        staminaBar.transform.localScale = new Vector3(transform.localScale.x * 25, transform.localScale.y * 25, transform.localScale.z);
        staminaMask.transform.localScale = new Vector3(staminaProcent * 500, transform.localScale.y * 25, transform.localScale.z);
    }

    private IEnumerator DamageFlash()
    {
        flashLight.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        flashLight.color = Color.white;
    }

    private bool Alive = true;
    private IEnumerator Death()
    {
        Alive = false;
        speed = 0;
        animator.Play("Death");
        yield return new WaitForSeconds(2f);
        DeathMenu.SetActive(true);
    }

}