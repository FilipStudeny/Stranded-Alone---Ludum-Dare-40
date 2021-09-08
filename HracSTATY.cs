using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HracSTATY : MonoBehaviour {

    public Text MineralCounterText;
    public Text platidloText;

    public Slider fuelSlider;
    public Slider oxygenSlider;
    public Slider healthSlider;

    public int maxHealth = 100;
    public int currentHealth;

    public int NumberOfMineralsCollected;

    public int mineralWeightLimit;

    public int Fuel;
    public int Platidlo;

    public int Oxygen = 100;
    public int OxygenMax = 100;

    float oxygenDrainSpeed = 2f;
    float oxygenDrain;

    public float menuWaitTime = 5f;
    float waitTimer;


    Animator animator;

    protected bool dead;

    HracPOHYB hrcPohyb;
    HracCONTROLLER hrcControler;
    HracLASER hrcLaser;

    CapsuleCollider capsuleColider;
    Rigidbody rb;

    public event System.Action OnDeath;
    AudioSource hurt;

    void Start()
    {
        hrcPohyb = GetComponent<HracPOHYB>();
        hrcControler = GetComponent<HracCONTROLLER>();
        hrcLaser = GetComponentInChildren<HracLASER>();
        hurt = GetComponent<AudioSource>();
        
     
      

        animator = GetComponentInChildren<Animator>();

        MineralCounterText.text = "" + NumberOfMineralsCollected.ToString();

        currentHealth = maxHealth;

        healthSlider.value = currentHealth;
        healthSlider.maxValue = maxHealth;

        platidloText.text = "" + Platidlo.ToString();


        oxygenSlider.value = Oxygen;
        oxygenSlider.maxValue = OxygenMax;

        fuelSlider.value = Fuel;
        fuelSlider.maxValue = 50;
    }

    void Update()
    {
        if(Time.time > oxygenDrain)
        {
            oxygenDrain = Time.time + oxygenDrainSpeed;
            
            Oxygen--;
            oxygenSlider.value = Oxygen;
        }

        if(Oxygen == 0) 
        {
            Die();
        }

        if (fuelSlider.value == fuelSlider.maxValue)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
            Cursor.visible = true;

        }

        UpdateUI();

        if(NumberOfMineralsCollected > mineralWeightLimit)
        {
            hrcPohyb._walkingSpeed = 3f;
        }else if(NumberOfMineralsCollected < mineralWeightLimit)
        {
            hrcPohyb._walkingSpeed = 10;
        }
    }

    private void OnTriggerEnter(Collider other)
    {    
        if (other.gameObject.tag == "MINERAL")
        {
            Destroy(other.gameObject);
            NumberOfMineralsCollected++;
        }

        if (other.gameObject.CompareTag("BENZINOVAODBERKA"))
        {
            Fuel += NumberOfMineralsCollected;
            NumberOfMineralsCollected = 0;
            
        }

        if (other.gameObject.CompareTag("OXYGENGENERATOR") && Platidlo >= 20)
        {
               Platidlo -= 20;
               Oxygen = OxygenMax;
        }

        if (other.gameObject.CompareTag("PLATIDLO"))
        {
            Platidlo += 1;
            Destroy(other.gameObject);
        }

        
    }

    void UpdateUI()
    {
        MineralCounterText.text = NumberOfMineralsCollected.ToString();
        fuelSlider.value = Fuel;
        healthSlider.value = currentHealth;
        platidloText.text = Platidlo.ToString();
    }




    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        hurt.Play();

        if (currentHealth <= 0 && !dead)
        {
            Die();
        }
    }

    protected void Die()
    {
        dead = true;
        if (OnDeath != null)
        {
            OnDeath();
        }
        animator.SetBool("Mrtev", true);
        animator.SetBool("Pohyb", false);

        hrcControler.enabled = false;
        hrcPohyb.enabled = false;
        hrcLaser.enabled = false;

        

        waitTimer += Time.deltaTime;

        if(menuWaitTime >= waitTimer)
        {

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            Cursor.visible = true;
        }


        
        
        
        
    }
}

