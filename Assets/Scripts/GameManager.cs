using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Playercontroller player;
    public static GameManager instance;
    public bool gameOver;
    private Door doorExit;
    public List<Enemy> enemies = new List<Enemy>();

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;

        }
        else
        {
            Destroy(gameObject);
        }

        //player = FindObjectOfType<Playercontroller>();
        //doorExit = FindObjectOfType<Door>();
    }

    public void Update()
    {
        gameOver = player.isDead;
        UIManager.instance.GameOverUI(gameOver);
    }

    public void RestartScene() {

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        PlayerPrefs.DeleteKey("playerHealth");
    
    }


    public void NextScene()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);

    }

    public void ReturnMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void NewGame()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(1);
    }

    public void ContinueGame()
    {
        if (PlayerPrefs.HasKey("sceneIndex"))
        {
            SceneManager.LoadScene(PlayerPrefs.GetInt("sceneIndex"));
        }
        else {

            NewGame();
        }


    }



    public void Quit()
    {

      Application.Quit();

    }


    public void AddEnemy(Enemy enemy)
    {
        enemies.Add(enemy);

    }
    public void EnemyDead(Enemy enemy)
    {
        enemies.Remove(enemy);
        if (enemies.Count==0)
        {
            doorExit.OpenDoor();
            SaveData();
        }
    }

    public float LoadHealth() 
    {
        if (!PlayerPrefs.HasKey("playerHealth"))
        {
            PlayerPrefs.SetFloat("playerHealth", 3f);
        }
        float currentHealth = PlayerPrefs.GetFloat("playerHealth");
        return currentHealth;
    }

    public void SaveData() 
    {
        PlayerPrefs.SetFloat("playerHealth", player.health);
        PlayerPrefs.SetInt("sceneIndex", SceneManager.GetActiveScene().buildIndex + 1);
        PlayerPrefs.Save();
    }

    public void IsPlayer(Playercontroller controller) {
        player = controller;
    }

    public void IsDoor(Door door)
    {
        doorExit = door;
    }


}
