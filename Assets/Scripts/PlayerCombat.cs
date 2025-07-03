using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles player taking damage and dying, in addition to controlling the healthbar scale in our GUI.
/// </summary>
[RequireComponent(typeof(ThirdPersonCharacterMovement))]
public class PlayerCombat : MonoBehaviour {

    [HideInInspector]public bool dead;

    public float maxHealth;
    [HideInInspector]public float currentHealth = 1f; //Initialize current health so doesn't die on Start()

    public ThirdPersonCharacterMovement movement;

    /// The players maxHealth will start at 10, and with every health pickup will increase by 3.
    /// There are a total of 30 health pickups in the game world allowing the player to increase their health by 90 points
    /// for a total end result of 100 health after collecting all health pickups.
    /// Our health bar in the GUI has each square of health correlate to 10 health points.
    /// As the player takes damage their health bar will scale on its x-axis.
    public MeshFlash healthBar;
    public MeshFlash playerMeshFlash;
    public GameObject healthBarOutliner;

    private Vector3 startingHealthBarSize;
    private ThirdPersonPlayerCamera thirdPersonPlayerCamera;
    private PlayerSounds playerSounds;
    private PlayerFire playerFire;

    private const float INITIAL_MAX_HEALTH = 10f; //Before upgrades have been collected or save data has been loaded.
    private float healthBarWidthMultiplier;
    private void Awake()
    {
        playerSounds = GetComponent<PlayerSounds>();
    }

    public PlayerFire GetUmaFire(){
        if (!playerFire)
        {
            playerFire = FindObjectOfType<PlayerFire>();
        }

        return playerFire;
    }

    private void Start()
    {
        if(GameManager.Instance.CurrentSaveData != null && GameManager.Instance.CurrentSaveData.GetFloat("maxHealth") == 0f)
        {
            GameManager.Instance.CurrentSaveData.SetValue("maxHealth", INITIAL_MAX_HEALTH);
        }

        if(GameManager.Instance.CurrentSaveData != null)
        GameManager.Instance.maxPlayerHealth = GameManager.Instance.CurrentSaveData.GetFloat("maxHealth");

        maxHealth = GameManager.Instance.maxPlayerHealth;
        currentHealth = maxHealth;
        movement = GetComponent<ThirdPersonCharacterMovement>();
        startingHealthBarSize = healthBar.transform.localScale;
        thirdPersonPlayerCamera = movement.thirdPersonPlayerCamera;
    }

    private void FixedUpdate()
    {
        if(!dead && currentHealth <= 0)
        {
            Die();
        }

        if(dead && thirdPersonPlayerCamera){
            thirdPersonPlayerCamera.transform.position += Vector3.up * 2.0f * Time.deltaTime;
            thirdPersonPlayerCamera.transform.LookAt(this.transform.position);
        }
    }

    private void Update()
    {
        if (healthBar && healthBarOutliner)
        {
            //Assigning healthBarWidthMultiplier in order to prevent health bar from growing off the edges of the players UI
            //Start the multiplier at a value and trend towards a smaller float for the multiplier as the players chip count approaches the max chip count.
            float defaultMultiplier = 0.0018f;
            float finalMultiplier = 0.00068f;
            float lerpAmmount = GameManager.Instance.chipCount / 210.0f;
            healthBarWidthMultiplier = Mathf.Lerp(defaultMultiplier,finalMultiplier, lerpAmmount);
            // UnityEngine.Debug.Log($"Lerping between {defaultMultiplier} and {finalMultiplier}. Lerp ammount = {lerpAmmount} healthBarWidthMultiplier = {healthBarWidthMultiplier}");
            //Sizing healthbar object and its outline
            healthBar.gameObject.transform.localScale = new Vector3((maxHealth - ((maxHealth - currentHealth))) * healthBarWidthMultiplier, startingHealthBarSize.y, startingHealthBarSize.z);
            healthBarOutliner.gameObject.transform.localScale = new Vector3(maxHealth * healthBarWidthMultiplier, startingHealthBarSize.y, startingHealthBarSize.z);
        }
    }

    public void TakeDamage(int damageAmmount, Vector3 attackDirection)
    {
        if(GameManager.Instance.invincible){
            return;
        }

        if(dead){
            return;
        }

        playerSounds.PlayHurt();

        thirdPersonPlayerCamera.cameraShake.Shake(0.4f, damageAmmount * 2f);

        currentHealth -= (damageAmmount/1.4f) + ((float)GameManager.Instance.maxPlayerHealth * 0.05f);

        if(currentHealth < 0f){
            currentHealth = 0f;
        }

        movement.dashVector += attackDirection * 25f;

        if(playerMeshFlash)
        playerMeshFlash.Flash(0.6f);

        if(healthBar)
            healthBar.Flash(0.2f);
    }

    public void Die()
    {
        //Get and disable components that would otherwise occacionally cause the player character to continue aiming and firing after being defeated
        playerFire = GameObject.FindObjectOfType<PlayerFire>();
        LockOn lockon = GameObject.FindObjectOfType<LockOn>();

        lockon.enabled = false;
        lockon.crosshair.HideCrosshair();
        playerFire.enabled = false;

        dead = true;
        currentHealth = 0;

        //Camera.main.GetComponent<ThirdPersonPlayerCamera>().cameraMode = ThirdPersonPlayerCamera.CameraMode.death;

        if(Camera.main && Camera.main.GetComponent<ThirdPersonPlayerCamera>())
            Camera.main.GetComponent<ThirdPersonPlayerCamera>().enabled = false;

        if(thirdPersonPlayerCamera)
            thirdPersonPlayerCamera.transform.position = this.transform.position + Vector3.up + (Vector3.right/32f);

        movement.Die();

        Invoke("FinshDeathSequence", 2.5f);
    }

    private void FinshDeathSequence(){
        GameManager.Instance.LoadAfterDelay(1f, SceneManager.GetActiveScene().name);
    }

    public void IncrementMaxHealth()
    {
        // float totalMaxHealth = 100f;
        // float totalNumberOfChips = 210f;
        // float amountToAdd = totalMaxHealth / totalNumberOfChips;
        float amountToAdd = 0.47619f;

        if(GameManager.Instance.maxPlayerHealth < 100f){
            GameManager.Instance.maxPlayerHealth += amountToAdd;
            maxHealth += amountToAdd;
            currentHealth = maxHealth;
            healthBar.Flash(1.4f);
            GameManager.Instance.CurrentSaveData.SetValue("maxHealth", maxHealth + amountToAdd);
        }
    }
}
