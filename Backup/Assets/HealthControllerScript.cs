using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthControler : MonoBehaviour
{

    [SerializeField] private string healthTextPrefix = "healthText";

    [Header("health parameters")]
    [SerializeField] private float maxHealth = 999.0f;
    public float currenthealth;
    [SerializeField] private float smoothDecreaseDuration = 0.5f;

    [Header("UI parameters")]
    [SerializeField] private TMP_Text healthText;

    [Header("Damage Color Parameters")]
    [SerializeField] private Color originalHealthColor;
    [SerializeField] private Color damageHealthColor;

    public LayerMask layerMask;

    //[SerializeField] private PlayerStateMachine player;
    [SerializeField] private PlayerStateMachine player;
    public int playerIndex;
    private bool isTextFound = false;

    private void Start()
    {
        currenthealth = 0;
        UpdateHealthText();
        
    }

    public void takeDamage(float damage)
    {
        StartCoroutine(smoothDecreaseHealth(damage));
    }

    void Update()
    {
        FindAndAssignHealthText();
        player.takehealth(currenthealth);
        playerIndex = player.getPlayerIndex();
    }
    private void FindAndAssignHealthText()
    {
        healthText = GameObject.Find($"{healthTextPrefix}{playerIndex}")?.GetComponent<TMP_Text>();

        if (healthText == null)
        {
            TMP_Text[] allTexts = FindObjectsOfType<TMP_Text>();
            foreach (TMP_Text text in allTexts)
            {
                if (text.gameObject.name.Contains(playerIndex.ToString()) ||
                    text.gameObject.CompareTag($"PlayerHealth_{playerIndex}"))
                {
                    healthText = text;
                    break;
                }
            }
        }

        if (healthText != null)
        {
            isTextFound = true;
            //Debug.Log($"Found health text for player {playerIndex}: {healthText.gameObject.name}");
        }
        else
        {
            Debug.LogWarning($"Could not find health text for player {playerIndex}");
        }
    }

    private IEnumerator smoothDecreaseHealth(float damage)
    {
        healthText.color = damageHealthColor;

        float damgePerTick = damage / smoothDecreaseDuration;
        float elapsedTime = 0f;

        while (elapsedTime < smoothDecreaseDuration)
        {
            float currentDamage = damgePerTick * Time.deltaTime;
            currenthealth += currentDamage;
            elapsedTime += Time.deltaTime;

            UpdateHealthText();
            if (currenthealth >= maxHealth)
            {
                currenthealth = maxHealth;
                //player death
                break;
            }
            yield return null;
        }

        healthText.color = originalHealthColor;
    }

    void UpdateHealthText()
    {
        player.takehealth(currenthealth);

        if (healthText != null) // הוסף בדיקת null
        {
            healthText.text = currenthealth.ToString("0");
        }
        
    }
}
