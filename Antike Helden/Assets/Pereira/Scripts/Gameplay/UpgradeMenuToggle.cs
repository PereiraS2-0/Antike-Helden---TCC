using UnityEngine;

public class UpgradeMenuToggle : MonoBehaviour
{
    public GameObject upgradePanel;
    private bool isOpen = false;

    public void ToggleUpgradePanel()
    {
        isOpen = !isOpen;
        upgradePanel.SetActive(isOpen);
        Time.timeScale = isOpen ? 0f : 1f;
    }
}
