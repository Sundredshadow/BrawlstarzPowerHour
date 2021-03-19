﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int wave;
    public List<GameObject> enemies;
    public GameObject enemy;
    public int hp;//Current hp
    public int hpMax;//Max hp
    public int power;//Attack power (how much damage done per click)
    public int points;//Points for upgrades
    public int hpPoints;//Points for upgrading hp
    public bool isDead;
    public bool waveRunning;
    public GameObject player;

    public GameObject resetDialogue;
    //Add in things for increasing health


    //UI on pause and and on gameover
    private GameObject PauseUI;
    private GameObject GameOverUI;
    // Start is called before the first frame update
    void Start()
    {
        hp = 2;
        hpMax = 2;
        power = 1;
        isDead = false;
        wave = 0;
        resetDialogue.SetActive(false);
        PauseUI= GameObject.Find("Pause");
        GameOverUI = GameObject.Find("GameOver");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return) && !waveRunning&&!PauseUI.activeSelf)//start wave
        {
            waveRunning = true;
            GenerateWave();
            return;
        }
        if (Input.GetKeyDown(KeyCode.P)|| Input.GetKeyDown(KeyCode.Escape)){//pause
            waveRunning = false;
            PauseUI.SetActive(true);
            return;
        }
        if (!isDead && waveRunning)
        {
            enemies[0].GetComponent<Enemy>().isMoving = true;
            enemies[0].GetComponent<Rigidbody2D>().simulated = true;
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i].GetComponent<Enemy>().isDead)
                {
                    if (enemies.Count == 1)
                    {
                        //Where Wave Ends
                        waveRunning = false;
                        //hp++;
                    }
                    Destroy(enemies[i]);
                    enemies.RemoveAt(i);
                    continue;
                }

                if (enemies[i].GetComponent<Enemy>().isMoving)
                {
                    /*Vector2 temp = enemies[i].transform.position;
                    temp.x += enemies[i].GetComponent<Enemy>().speed;
                    enemies[i].transform.position = temp;*/
                    enemies[i].GetComponent<Enemy>().Move();
                }
            }
            return;
        }
        else if(isDead){///reset
            foreach (GameObject a in enemies)
            {
                Destroy(a);
            }
            hp = 1;
            hpMax = 1;
            wave = 0;
            isDead = false;
            waveRunning = false;

            //button events are already set up in Scenes Script merely requires GamOverUI activation
            GameOverUI.SetActive(true);
            return;
        }
       
    }

    public void TakeDamage()
    {
        hp--;
        if (hp <= 0)
        {
            isDead = true;
            Debug.Log("Game Over");
            //reset dialogue
            resetDialogue.SetActive(true);
        }
    }

    public void GenerateWave()
    {
        wave++;
        Debug.Log("Wave: " + wave);
        enemies = new List<GameObject>();
        for (int i = 0; i < Random.Range(wave, wave + 3); i++)
        {
            enemies.Add(Instantiate(enemy));
            enemies[i].GetComponent<Enemy>().hp = Random.Range(wave, wave + 1);
            enemies[i].GetComponent<Enemy>().hpMax = enemies[i].GetComponent<Enemy>().hp;
            enemies[i].GetComponent<Enemy>().speed = Random.Range(2*wave, 3* wave);
            enemies[i].GetComponent<Enemy>().isLeft = Random.Range(0, 2) != 0;
            enemies[i].GetComponent<Enemy>().type = (EnemyType)Random.Range(0, 2);
            if (enemies[i].GetComponent<Enemy>().isLeft)
            {
                enemies[i].GetComponent<Enemy>().setPosition(new Vector3(-11, -2 + i, 0));
            }
            else
            {
                enemies[i].GetComponent<Enemy>().setPosition(new Vector3(13, -2 + i, 0));
            }
        }
    }
}
