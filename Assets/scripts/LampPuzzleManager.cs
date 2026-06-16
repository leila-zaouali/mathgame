using UnityEngine;
public class LampPuzzleManager : MonoBehaviour
{
    public Lampe lamp1;
    public Lampe lamp2;
    public Lampe lamp3;
    public GameObject wallImage;

    // ✅ Assigne les piles existantes dans la scène (pas des prefabs)
    public GameObject battery1;
    public GameObject battery2;
    public GameObject battery3;

    // ✅ Ajoute en haut
    public LampSwitch switch1;
    public LampSwitch switch2;
    public LampSwitch switch3;

    private bool puzzleCompleted = false;

    void Start()
    {
        if (wallImage != null)
            wallImage.SetActive(false);

        if (ProgressManager.instance != null &&
      ProgressManager.instance.lampPuzzleCompleted)
        {
            puzzleCompleted = true;
            wallImage.SetActive(true);

            RestoreLamp(lamp1, battery1, switch1);
            RestoreLamp(lamp2, battery2, switch2);
            RestoreLamp(lamp3, battery3, switch3);

            Debug.Log("💡 Puzzle lampe restauré");
        }
    }

    void RestoreLamp(Lampe lamp, GameObject battery, LampSwitch sw)
    {
        if (lamp == null || battery == null) return;
        if (lamp.HasBattery()) return;

        Vector3 originalScale = battery.transform.localScale;
        lamp.InsertBattery(battery);
        battery.transform.localScale = originalScale;

        // ✅ Switch en position ON
        if (sw != null)
            sw.SetOn();

        lamp.TurnOnSilent(); // ✅ au lieu de TurnOn()
    }

    void Update()
    {
        if (puzzleCompleted) return;
        if (lamp1 == null || lamp2 == null || lamp3 == null) return;

        bool allOn =
            lamp1.IsActive() &&
            lamp2.IsActive() &&
            lamp3.IsActive();

        if (!allOn) return;

        puzzleCompleted = true;
        Debug.Log("🏁 Puzzle lampes terminé");

        if (wallImage != null)
            wallImage.SetActive(true);

        if (APIManager.instance != null)
        {
            APIManager.instance.UpdateLevel();
            APIManager.instance.SaveProgress("lamp_puzzle", true);
        }

        if (ScoreManager.instance != null)
            ScoreManager.instance.AddLevelBonus();
    }
}