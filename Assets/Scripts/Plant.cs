using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Plant : MonoBehaviour
{
    public float health;
    public float minAffectRate;
    public float maxAffectRate;
    public float fillAffectRate;
    public float drainAffectRate;
    float fillParticleGenerationRate;
    float drainParticleGenerationRate;
    public float minParticleGenerationRate;
    public float maxParticleGenerationRate;
    public float minParticleSpeed;
    public float maxParticleSpeed;
    public float minParticleWaveAmplitude;
    public float maxParticleWaveAmplitude;
    public int minWaveFrequency;
    public int maxWaveFrequency;
    public float minStartingSize;
    public float maxStartingSize;
    public float affectRateAcceleration;

    public Sprite[] plantImages;

    public GameObject[] gameOverStuffs;

    public GameObject gameStartText;

    public GameObject fillParticle;
    public GameObject drainParticle;

    float fillTime;
    float drainTime;

    public Slider ui_healthBar;
    public Image ui_slider;

    List<GameObject> healingParticles;

    bool gameOver;

    // Start is called before the first frame update
    void Start()
    {
        health = 50f;
        healingParticles = new List<GameObject>();
        drainAffectRate = minAffectRate;
        fillAffectRate = minAffectRate;
        
        //show en garde! prompt
        LeanTween.scale(gameStartText, Vector2.one, 2f).setOnComplete(() => {
            LeanTween.scale(gameStartText, Vector2.zero, 2f).setOnComplete(() => {
                GameObject.Find("Player 1").GetComponent<PlayerControls>().ChangeState<PlayerControlsFree>();
                GameObject.Find("Player 2").GetComponent<PlayerControls>().ChangeState<PlayerControlsFree>();
            });
        });

        gameOver = false;

        //set player to bot depending on bot mode or not
        //get player 2
        GameObject.Find("Player 2").GetComponent<PlayerControls>().isBot 
            = GameObject.Find("God").GetComponent<God>().playAgainstBot;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameOver)
        {
            if (health >= 100)
            {
                //player two wins
                gameOverStuffs[0].GetComponent<TextMesh>().text = "Player 2 Wins!";
                LeanTween.scale(gameOverStuffs[0], Vector3.one * .75f, .75f)
                    .setOnComplete(() =>
                    {
                        gameOverStuffs[1].GetComponent<Button>().onClick.AddListener(() =>
                        {
                            Debug.Log("Added listener");
                            GameObject.Find("SceneSwapper").GetComponent<SceneSwapper>().LoadScene(2);
                        });
                        LeanTween.scale(gameOverStuffs[1], Vector3.one, .15f).setOnComplete(() =>
                        {
                            gameOverStuffs[2].GetComponent<Button>().onClick.AddListener(() =>
                            {
                                GameObject.Find("SceneSwapper").GetComponent<SceneSwapper>().LoadScene(1);
                            });
                            LeanTween.scale(gameOverStuffs[2], Vector3.one, .15f);
                        });
                    });
                //put the players in lock state
                GameObject.Find("Player 1").GetComponent<PlayerControls>().ChangeState<PlayerControlsLocked>();
                GameObject.Find("Player 2").GetComponent<PlayerControls>().ChangeState<PlayerControlsLocked>();
                gameOver = true;
            }
            if (health <= 0)
            {
                gameOverStuffs[0].GetComponent<TextMesh>().text = "Player 1 Wins!";
                LeanTween.scale(gameOverStuffs[0], Vector3.one * .75f, .75f)
                    .setOnComplete(() =>
                    {
                        gameOverStuffs[1].GetComponent<Button>().onClick.AddListener(() =>
                        {
                            GameObject.Find("SceneSwapper").GetComponent<SceneSwapper>().LoadScene(2);
                        });
                        LeanTween.scale(gameOverStuffs[1], Vector3.one, .15f).setOnComplete(() =>
                        {
                            gameOverStuffs[2].GetComponent<Button>().onClick.AddListener(() =>
                            {
                                GameObject.Find("SceneSwapper").GetComponent<SceneSwapper>().LoadScene(1);
                            });
                            LeanTween.scale(gameOverStuffs[2], Vector3.one, .15f);
                        });
                    });
                //put the players in lock state
                GameObject.Find("Player 1").GetComponent<PlayerControls>().ChangeState<PlayerControlsLocked>();
                GameObject.Find("Player 2").GetComponent<PlayerControls>().ChangeState<PlayerControlsLocked>();
                gameOver = true;
            }
        }

        //update the health bar
        ui_healthBar.value = health / 100f;

        //if the health bar is below 50 percent make it purple, otherwise make it yellow
        if (ui_healthBar.value < .5)
        {
            ui_slider.color = Color.magenta;
        }
        else if (ui_healthBar.value == .5)
        {
            ui_slider.color = Color.white;
        }
        else
        {
            ui_slider.color = Color.yellow;
        }

        //get fill rate percentage
        float fillRatePercentage = (fillAffectRate - minAffectRate) / (maxAffectRate - minAffectRate);

        //sync up particle generation rate with the 
        fillParticleGenerationRate = Mathf.Lerp(minParticleGenerationRate, maxParticleGenerationRate, 1 - fillRatePercentage);

        //get fill rate percentage
        float drainRatePercentage = (drainAffectRate - minAffectRate) / (maxAffectRate - minAffectRate);

        //sync up particle generation rate with the 
        drainParticleGenerationRate = Mathf.Lerp(minParticleGenerationRate, maxParticleGenerationRate, 1 - drainRatePercentage);


        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        //set the sprite of the tree to match with the health remaining
        if (health < 20)
        {
            sr.sprite = plantImages[0];
        }
        else if (health >= 20 && health < 40)
        {
            sr.sprite = plantImages[1];
        }
        else if (health >= 40 && health < 60)
        {
            sr.sprite = plantImages[2];
        }
        else if (health >= 60 && health < 80)
        {
            sr.sprite = plantImages[3];
        }
        else if (health >= 80)
        {
            sr.sprite = plantImages[4];
        }
    }

    public void fill(GameObject otherPlayer)
    {
        fillTime += Time.deltaTime;

        //if the player is facing the plant
        if (AreTheyFacingMe(otherPlayer) == true)
        {
            //add a certain amount to health every frame
            health += fillAffectRate * Time.deltaTime;

            //heal more every frame
            fillAffectRate = Mathf.Clamp(fillAffectRate + (affectRateAcceleration * Time.deltaTime), minAffectRate, maxAffectRate);

            if (fillTime >= fillParticleGenerationRate)
            {
                fillTime = 0;

                //make a vector of random magnitude and random length to add an offset
                //to the spawn points of the particles

                Vector2 randyVector = new Vector2(Random.Range(-.2f, .2f), Random.Range(-.2f, .2f));

                //spawn the particle
                GameObject newParticle = Instantiate(fillParticle, (Vector2) otherPlayer.transform.position + randyVector, Quaternion.identity);
                //randomize some of the variables
                Particle ptcl = newParticle.GetComponent<Particle>();
                ptcl.destination = transform.position;
                ptcl.speed = Random.Range(minParticleSpeed, maxParticleSpeed);
                ptcl.waveAmplitude = Random.Range(minParticleWaveAmplitude, maxParticleWaveAmplitude);
                ptcl.waveFrequency = Random.Range(minWaveFrequency, maxWaveFrequency);
                ptcl.startingSize = Vector2.one * Random.Range(minStartingSize, maxStartingSize);

            }
        }

    }

    

    public void drain(GameObject otherPlayer)
    {
        drainTime += Time.deltaTime;

        //if the player is facing the plant
        if (AreTheyFacingMe(otherPlayer) == true)
        { 
            //subtract a certain amount to health every frame
            health -= drainAffectRate * Time.deltaTime;

            //drain more every frame
            drainAffectRate = Mathf.Clamp(drainAffectRate + (affectRateAcceleration * Time.deltaTime), minAffectRate, maxAffectRate);

            if (drainTime >= drainParticleGenerationRate)
            {
                drainTime = 0;

                //make a vector of random magnitude and random length to add an offset
                //to the spawn points of the particles

                Vector2 randyVector = new Vector2(Random.Range(-.2f, .2f), Random.Range(-.2f, .2f));

                //spawn the particle
                GameObject newParticle = Instantiate(drainParticle, (Vector2) transform.position + randyVector, Quaternion.identity);
                //randomize some of the variables
                Particle ptcl = newParticle.GetComponent<Particle>();
                ptcl.destination = otherPlayer.transform.position;
                ptcl.speed = Random.Range(minParticleSpeed, maxParticleSpeed);
                ptcl.waveAmplitude = Random.Range(minParticleWaveAmplitude, maxParticleWaveAmplitude);
                ptcl.waveFrequency = Random.Range(minWaveFrequency, maxWaveFrequency);
                ptcl.startingSize = Vector2.one * Random.Range(minStartingSize, maxStartingSize);

            }
        }

    }

    

    public bool AreTheyFacingMe(GameObject otherPlayer)
    {
        //if the other player is on your left
        if (AreTheyOnMyLeft(otherPlayer) == true)
        {
            //if the other player is facing right
            if (IsThisPlayerFacingLeft(otherPlayer) == false)
            {
                //they are indeed facing you
                return true;
            }
        }
        //if they are on your right
        else
        {
            //if they other player is facing left
            if (IsThisPlayerFacingLeft(otherPlayer) == true)
            {
                //they are indeed facing you
                return true;
            }
        }
        //if none of these conditions are true, then they are not facing you
        return false;
    }

    //is the other player to the left of you?
    public bool AreTheyOnMyLeft(GameObject otherPlayer)
    {
        if (transform.position.x - otherPlayer.transform.position.x >= 0)
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    //get whether the player is facing left or not
    public bool IsThisPlayerFacingLeft(GameObject player)
    {
        if (player.transform.localScale.x == 1f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
