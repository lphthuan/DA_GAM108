using UnityEngine;
using UnityEngine.UI;
public class HealthBarController : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private Image totalhealthBar;
    [SerializeField] private Image currenthealthBar;
    private void Start()
    {
        totalhealthBar.fillAmount = playerHealth.currentHeath / 10;
    }
    private void Update()
    {
        currenthealthBar.fillAmount = playerHealth.currentHeath / 10;
    }
}
