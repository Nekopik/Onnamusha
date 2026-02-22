using TMPro;
using UnityEngine;

public class MobModeUI : MonoBehaviour
{
    private Oni oni;
    [SerializeField] private TMP_Text mobModeText;

    private Oni.MobMode lastMode;
    private bool initialized = false;

    void Start()
    {
        oni = Object.FindFirstObjectByType<Oni>();

        if (oni != null)
        {
            lastMode = oni.currentMobMode;
            UpdateModeText(lastMode);
            initialized = true;
        }
    }

    void Update()
    {
        if (oni == null)
        {
            oni = Object.FindFirstObjectByType<Oni>();

            if (oni == null) return;

            lastMode = oni.currentMobMode;
            UpdateModeText(lastMode);
            initialized = true;
        }

        if (!initialized || mobModeText == null)
            return;

        if (oni.currentMobMode != lastMode)
        {
            lastMode = oni.currentMobMode;
            UpdateModeText(lastMode);
        }
    }

    private void UpdateModeText(Oni.MobMode mode)
    {
        if (mobModeText == null) return;

        if (mode == Oni.MobMode.Aggressive)
        {
            mobModeText.text = "Aggressive";
            mobModeText.color = Color.red;
        }
        else
        {
            mobModeText.text = "Passive";
            mobModeText.color = Color.green;
        }
    }
}