using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerMeters : MonoBehaviour
{
    public float playerHealth;
    [SerializeField] float maxHealth;
    public float playerXP;
    [SerializeField] float maxXP;

    public GameObject healthSlider;
    public GameObject xpSlider;

    BodyPlacing bodyPlacing;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bodyPlacing = GetComponent<BodyPlacing>();
    }

    // Update is called once per frame
    void Update()
    {
        healthSlider.GetComponent<Slider>().value = playerHealth/maxHealth;
        xpSlider.GetComponent<Slider>().value = playerXP / maxXP;

        if (playerHealth <= 0) SceneManager.LoadScene(0);

        if (playerXP >= maxXP)
        {
            playerXP = 0;
            bodyPlacing.placingBody = true;
        }
    }
}
