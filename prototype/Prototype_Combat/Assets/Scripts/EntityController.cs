using UnityEngine;

public class EntityController : MonoBehaviour {

    public int baseHealth = 100;
    public int baseDamage = 5;
    public int level = 1;
    public string pseudo = "entity";

    private int damage = 5;
    private int health = 100;

    private bool alive = true;

    protected virtual void Start()
    {
        this.setHealth(this.baseHealth);
        this.setDamage(this.baseDamage);
    }

    void Die()
    {
        GetComponent<Animator>().SetBool("Alive", false);
        SoundManager.instance.PlaySingle(SoundManager.instance.efxSource, SoundManager.instance.effectGenericDeath);
        this.alive = false;
    }

    public bool isAlive()
    {
        return this.alive;
    }

    public void setDamage(int dmg)
    {
        if (dmg >= 0)
        {
            damage = dmg;
        }
    }

    public void setLevel(int lvl)
    {
        if (lvl >= 1)
        {
            this.level = lvl;
        }
        this.setHealth(this.baseHealth * this.level);
        this.setDamage(this.baseDamage * this.level);
    }

    public void setHealth(int hp)
    {
        if (hp >= 0)
        {
            health = hp;
        }
    }

    public string getPseudo()
    {
        return pseudo;
    }

    public int getHealth()
    {
        return health;
    }

    public int getLevel()
    {
        return level;
    }

    public int getDamage()
    {
        return damage;
    }

    public void takeDamage(int amount)
    {
        this.setHealth(this.health - amount);
        if (this.getHealth() == 0)
        {
            this.Die();
        }
    }

    public void nextLevel()
    {
        this.setLevel(this.level + 1);
    }

    public bool isTargetInRadius(GameObject target, float radius)
    {
        float distance = Vector3.Distance(target.transform.position, transform.position);

        if (distance < radius)
        {
            return true;
        }
        return false;
    }
}
