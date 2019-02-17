using UnityEngine;

public class EntityController : MonoBehaviour {

    public int baseHealth = 100;
    public int baseDamage = 5;
    public int level = 1;
    public string pseudo = "entity";

    private int damage = 5;
    private int health = 100;

    void setDamage(int dmg)
    {
        if (dmg >= 0)
        {
            damage = dmg;
        }
    }

    void setLevel(int lvl)
    {
        if (lvl >= 1)
        {
            this.level = lvl;
        }
        this.setHealth(this.baseHealth * this.level);
        this.setDamage(this.baseDamage * this.level);
    }

    void setHealth(int hp)
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
    }

    public void nextLevel()
    {
        this.setLevel(this.level + 1);
    }
}
