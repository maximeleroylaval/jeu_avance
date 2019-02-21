using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour {

    public static CombatManager instance = null;

    public AudioClip epicBoss;

    public GameObject activeBoss;

    void Awake()
    {
        //Check if there is already an instance of SoundManager
        if (instance == null)
            //if not, set it to this.
            instance = this;
        //If instance already exists:
        else if (instance != this)
            //Destroy this, this enforces our singleton pattern so there can only be one instance of SoundManager.
            Destroy(gameObject);

        //Set SoundManager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
        DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    void Start () {
        SoundManager.instance.PlayMusic(SoundManager.instance.musicSource, SoundManager.instance.ambientUnderWater);
    }
	
	// Update is called once per frame
	void Update () {
		if (activeBoss && !activeBoss.GetComponent<EntityController>().isAlive())
        {
            SoundManager.instance.StopMusic(SoundManager.instance.combatSource);
            activeBoss.GetComponent<EnemyController>().lockedTarget.GetComponent<EntityController>().nextLevel();
            activeBoss = null;
        }
	}

    public void spawnBoss(GameObject player, GameObject prefabBoss, Vector3 spawnPosition)
    {
        activeBoss = Instantiate(prefabBoss, spawnPosition, Quaternion.identity);
        activeBoss.transform.LookAt(player.transform);

        SoundManager.instance.PlayMusic(SoundManager.instance.combatSource, epicBoss);
    }
}
