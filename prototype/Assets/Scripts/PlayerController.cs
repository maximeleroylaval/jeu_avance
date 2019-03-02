using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : EntityController {

    public float speed = 4.0f;
    public float gravity = 0f;

    public Color aimColor = Color.green;
    public int aimSize = 28;
    public int hudSize = 28;

    public GameObject activeWeapon = null;

    private CharacterController characterController;
    private Camera characterCamera;

    private EntityController entityAimed = null;

    private GUIStyle style = new GUIStyle();

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        characterController = GetComponent<CharacterController>();
        characterCamera = GameObject.FindObjectOfType<Camera>();
        style.normal.textColor = aimColor;
        Cursor.visible = false;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (!this.isAlive())
            return;

        this.move();
        this.displayEntityInfoAimed();
        if (Input.GetMouseButtonDown(0))
        {
            this.shoot();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    void OnGUI()
    {
        // Aim
        if (!this.isDead())
            this.displayGUI("+", characterCamera.pixelWidth / 2, characterCamera.pixelHeight / 2, aimSize, style, true);
        else
            this.resetGUI(characterCamera.pixelWidth / 2, characterCamera.pixelHeight / 2, aimSize, true);

        // Player info
        if (!this.isDead())
        {
            this.displayGUI(
                this.getPseudo() + " - Level " + this.getLevel() + " - " + this.getHealth() + " HP",
                characterCamera.pixelWidth / 40,
                characterCamera.pixelHeight / 30,
                hudSize,
                style,
                false
            );
        } else
        {
            this.resetGUI(
                characterCamera.pixelWidth / 40,
                characterCamera.pixelHeight / 30,
                hudSize,
                false
            );
        }

        // Entity aimed info
        if (entityAimed != null && !this.isDead())
        {
            this.displayGUI(
                entityAimed.getPseudo() + " - Level " + entityAimed.getLevel() + " - " + entityAimed.getHealth() + " HP",
                characterCamera.pixelWidth / 2,
                characterCamera.pixelHeight,
                hudSize,
                style,
                true
            );
        }
        else
        {
            this.resetGUI(
                characterCamera.pixelWidth / 2,
                characterCamera.pixelHeight,
                hudSize,
                true
            );
        }

        // Dead message
        if (this.isDead())
        {
            this.displayGUI(
                "You are dead",
                characterCamera.pixelWidth / 2,
                characterCamera.pixelHeight / 2,
                hudSize,
                style,
                true
            );
        } else
        {
            this.resetGUI(
                characterCamera.pixelWidth / 2,
                characterCamera.pixelHeight / 2,
                hudSize,
                true
            );
        }
    }

    public override void Die()
    {
        base.Die();
        this.GetComponent<CameraController>().disable();
    }

    void resetGUI(float posX, float posY, float size, bool center)
    {
        this.displayGUI("", posX, posY, size, GUIStyle.none, center);
    }

    void displayGUI(string toDisplay, float posX, float posY, float size, GUIStyle style, bool center)
    {
        if (center)
        {
            posX = posX - ((size / 4) * (toDisplay.Length / 2));
            posY = posY - (size / 2) - (characterCamera.pixelHeight / 20);
        }
        GUI.Label(new Rect(posX, posY, size, size), toDisplay, style);
    }

    void displayEntityInfo(EntityController entity)
    {
        entityAimed = entity;
    }

    void displayEntityInfoAimed()
    {
        GameObject gameObject = this.getGameObjectAimed();
        if (gameObject != null)
        {
            EntityController entity = gameObject.GetComponent<EnemyController>();
            if (entity != null)
            {
                this.displayEntityInfo(entity);
            } else
            {
                entityAimed = null;
            }
        }
        else
        {
            entityAimed = null;
        }
    }

    void shoot()
    {
        GameObject entity = this.getGameObjectAimed();
        if (entity != null)
        {
            activeWeapon.GetComponent<WeaponController>().attack(this, entity);
        }
    }

    GameObject getGameObjectAimed()
    {
        Ray ray = characterCamera.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            return hit.transform.gameObject;
        }
        return null;
    }

    void move()
    {
        float deltaX = Input.GetAxis("Horizontal") * speed;
        float deltaZ = Input.GetAxis("Vertical") * speed;
        Vector3 movement = new Vector3(deltaX, 0, deltaZ);

        movement = Vector3.ClampMagnitude(movement, speed);
        movement.y = gravity;
        movement *= Time.deltaTime;
        movement = transform.TransformDirection(movement);

        characterController.Move(movement);
    }
}
