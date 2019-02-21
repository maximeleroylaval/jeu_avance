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
    void Update()
    {
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
        string toDisplay = "+";
        float posX = characterCamera.pixelWidth / 2 - aimSize / 4;
        float posY = characterCamera.pixelHeight / 2 - aimSize / 2;
        GUI.Label(new Rect(posX, posY, aimSize, aimSize), toDisplay, style);

        // Health
        toDisplay = this.getPseudo() + " - Level " + this.getLevel() + " - " + this.getHealth() + " HP";
        posX = characterCamera.pixelWidth / 40;
        posY = characterCamera.pixelHeight / 20;
        GUI.Label(new Rect(posX, posY, hudSize, hudSize), toDisplay, style);

        // Entity info
        if (entityAimed != null)
        {
            toDisplay = entityAimed.getPseudo() + " - Level " + entityAimed.getLevel() + " - " + entityAimed.getHealth() + " HP";
            posX = characterCamera.pixelWidth / 2 - ((hudSize / 4) * (toDisplay.Length / 2));
            posY = characterCamera.pixelHeight - (hudSize / 2) - (characterCamera.pixelHeight / 20);
            GUI.Label(new Rect(posX, posY, hudSize, hudSize), toDisplay, style);
        }
        else
        {
            toDisplay = "";
            posX = characterCamera.pixelWidth / 2 - ((hudSize / 4) * (toDisplay.Length / 2));
            posY = characterCamera.pixelHeight - (hudSize / 2) - (characterCamera.pixelHeight / 20);
            GUI.Label(new Rect(posX, posY, hudSize, hudSize), toDisplay, style);
        }
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
