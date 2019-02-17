using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : EntityController {

    public float speed = 4.0f;
    public float gravity = 0f;

    public Color aimColor = Color.green;
    public int aimSize = 28;
    public int hudSize = 28;

    private CharacterController characterController;
    private Camera characterCamera;

    private EntityController entityAimed = null;

    private GUIStyle style = new GUIStyle();

    public GameObject activeWeapon = null;

    // Use this for initialization
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        characterCamera = GameObject.FindObjectOfType<Camera>();
        style.normal.textColor = aimColor;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        this.move();
        this.displayEntityInfoAimed();
        if (Input.GetMouseButtonDown(0) && this.activeWeapon != null)
        {
            if (activeWeapon.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Base"))
            {
                this.shoot();
            }
        } else if (this.activeWeapon != null && this.activeWeapon.GetComponent<Animator>() != null)
        {
            activeWeapon.GetComponent<Animator>().SetBool("Attack", false);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
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
            EntityController target = entity.GetComponent<EntityController>();
            if (target != null)
            {
                if (activeWeapon.GetComponent<Animator>() != null)
                {
                    activeWeapon.GetComponent<Animator>().SetBool("Attack", true);
                }
                target.takeDamage(this.getDamage());
            }
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

    private IEnumerator ShotGen(Vector3 pos)
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        sphere.transform.position = pos;

        yield return new WaitForSeconds(1);

        Destroy(sphere);
    }

    void OnGUI()
    {
        float posX = characterCamera.pixelWidth / 2 - aimSize / 4;
        float posY = characterCamera.pixelHeight / 2 - aimSize / 2;
        GUI.Label(new Rect(posX, posY, aimSize, aimSize), "+", style);

        if (entityAimed != null)
        {
            string toDisplay = entityAimed.getPseudo() + " - Level " + entityAimed.getLevel() + " - " + entityAimed.getHealth() + " HP";
            posX = characterCamera.pixelWidth / 2 - ((hudSize / 4) * (toDisplay.Length /2 ));
            posY = characterCamera.pixelHeight - hudSize / 2;
            GUI.Label(new Rect(posX, posY, hudSize, hudSize), toDisplay, style);
        } else
        {
            string toDisplay = "";
            posX = characterCamera.pixelWidth / 2 - ((hudSize / 4) * (toDisplay.Length / 2));
            posY = characterCamera.pixelHeight - hudSize / 2;
            GUI.Label(new Rect(posX, posY, hudSize, hudSize), toDisplay, style);
        }
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
