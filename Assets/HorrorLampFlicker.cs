using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// Effet de lampe "film d'horreur" : la lumière s'éteint puis se rallume après un délai aléatoire.
/// Optionnellement, elle vacille un peu juste avant de s'éteindre, pour un effet plus dramatique.
/// A accrocher sur le même GameObject qu'une Spot Light (ou Point Light).
/// </summary>
[RequireComponent(typeof(Light))]
public class HorrorLampFlicker : MonoBehaviour
{
    [Header("Références")]
    [Tooltip("La lumière à contrôler. Si vide, prend automatiquement le composant Light sur ce GameObject.")]
    [SerializeField] private Light lampLight;

    [Header("Intensité normale")]
    [Tooltip("Intensité de la lampe quand elle est allumée normalement.")]
    [SerializeField] private float intensiteNormale = 5f;

    [Header("Délais entre les coupures")]
    [Tooltip("Temps minimum (en secondes) avant la prochaine coupure.")]
    [SerializeField] private float delaiMinAvantCoupure = 4f;
    [Tooltip("Temps maximum (en secondes) avant la prochaine coupure.")]
    [SerializeField] private float delaiMaxAvantCoupure = 10f;

    [Header("Durée de la coupure (lampe éteinte)")]
    [Tooltip("Durée minimum pendant laquelle la lampe reste éteinte.")]
    [SerializeField] private float dureeMinEteinte = 1f;
    [Tooltip("Durée maximum pendant laquelle la lampe reste éteinte.")]
    [SerializeField] private float dureeMaxEteinte = 3f;

    [Header("Vacillement avant extinction (optionnel)")]
    [Tooltip("Active un petit vacillement juste avant que la lampe s'éteigne complètement.")]
    [SerializeField] private bool vacillerAvantExtinction = true;
    [Tooltip("Durée du vacillement avant l'extinction complète.")]
    [SerializeField] private float dureeVacillement = 0.6f;
    [Tooltip("Nombre de à-coups pendant le vacillement.")]
    [SerializeField] private int nombreAcoups = 5;

    [Header("Son (optionnel)")]
    [Tooltip("Source audio à jouer quand la lampe s'éteint (ex: grésillement, clic électrique).")]
    [SerializeField] private AudioSource sonExtinction;
    [Tooltip("Source audio à jouer quand la lampe se rallume.")]
    [SerializeField] private AudioSource sonRallumage;

    [Header("Lumière ambiante (Baked GI / Skybox)")]
    [Tooltip("Si ta scène utilise du Baked Global Illumination, la pièce peut rester éclairée même quand la Light est désactivée. Active ceci pour forcer l'ambiante à du noir pendant la coupure.")]
    [SerializeField] private bool forcerNoirAmbiantPendantCoupure = true;

    private Coroutine routineEnCours;

    // Mémorise les réglages ambiants d'origine pour les restaurer au rallumage
    private Color ambianceOriginaleCouleur;
    private Color ambianceOriginaleEquateur;
    private Color ambianceOriginaleSol;
    private float intensiteAmbianteOriginale;
    private AmbientMode modeAmbiantOriginal;

    private void Awake()
    {
        if (lampLight == null)
        {
            lampLight = GetComponent<Light>();
        }

        // On sauvegarde l'état ambiant initial de la scène
        ambianceOriginaleCouleur = RenderSettings.ambientSkyColor;
        ambianceOriginaleEquateur = RenderSettings.ambientEquatorColor;
        ambianceOriginaleSol = RenderSettings.ambientGroundColor;
        intensiteAmbianteOriginale = RenderSettings.ambientIntensity;
        modeAmbiantOriginal = RenderSettings.ambientMode;
    }

    private void OnEnable()
    {
        // On s'assure que la lampe démarre allumée normalement
        lampLight.enabled = true;
        lampLight.intensity = intensiteNormale;

        routineEnCours = StartCoroutine(CycleLampe());
    }

    private void OnDisable()
    {
        if (routineEnCours != null)
        {
            StopCoroutine(routineEnCours);
        }
    }

    private IEnumerator CycleLampe()
    {
        while (true)
        {
            // 1. La lampe reste allumée normalement pendant un délai aléatoire
            float delaiAvantCoupure = Random.Range(delaiMinAvantCoupure, delaiMaxAvantCoupure);
            yield return new WaitForSeconds(delaiAvantCoupure);

            // 2. Vacillement optionnel avant extinction complète
            if (vacillerAvantExtinction)
            {
                yield return StartCoroutine(Vaciller());
            }

            // 3. Extinction complète
            lampLight.enabled = false;
            if (forcerNoirAmbiantPendantCoupure)
            {
                CouperLumiereAmbiante();
            }
            if (sonExtinction != null)
            {
                sonExtinction.Play();
            }

            // 4. Reste éteinte pendant une durée aléatoire
            float dureeEteinte = Random.Range(dureeMinEteinte, dureeMaxEteinte);
            yield return new WaitForSeconds(dureeEteinte);

            // 5. Rallumage
            lampLight.enabled = true;
            lampLight.intensity = intensiteNormale;
            if (forcerNoirAmbiantPendantCoupure)
            {
                RestaurerLumiereAmbiante();
            }
            if (sonRallumage != null)
            {
                sonRallumage.Play();
            }
        }
    }

    /// <summary>
    /// Met l'éclairage ambiant de la scène à zéro (noir total).
    /// Utile quand la scène utilise du Baked Global Illumination, qui ne réagit pas
    /// à lampLight.enabled = false.
    /// </summary>
    private void CouperLumiereAmbiante()
    {
        RenderSettings.ambientMode = AmbientMode.Flat;
        RenderSettings.ambientSkyColor = Color.black;
        RenderSettings.ambientEquatorColor = Color.black;
        RenderSettings.ambientGroundColor = Color.black;
        RenderSettings.ambientIntensity = 0f;
    }

    /// <summary>
    /// Restaure la lumière ambiante d'origine de la scène (au rallumage de la lampe).
    /// </summary>
    private void RestaurerLumiereAmbiante()
    {
        RenderSettings.ambientMode = modeAmbiantOriginal;
        RenderSettings.ambientSkyColor = ambianceOriginaleCouleur;
        RenderSettings.ambientEquatorColor = ambianceOriginaleEquateur;
        RenderSettings.ambientGroundColor = ambianceOriginaleSol;
        RenderSettings.ambientIntensity = intensiteAmbianteOriginale;
    }

    private IEnumerator Vaciller()
    {
        float tempsParAcoup = dureeVacillement / (nombreAcoups * 2f);

        for (int i = 0; i < nombreAcoups; i++)
        {
            // Baisse brutale d'intensité (ou extinction courte)
            lampLight.intensity = Random.Range(0f, intensiteNormale * 0.3f);
            yield return new WaitForSeconds(tempsParAcoup);

            // Remonte presque à la normale
            lampLight.intensity = Random.Range(intensiteNormale * 0.7f, intensiteNormale);
            yield return new WaitForSeconds(tempsParAcoup);
        }
    }

    /// <summary>
    /// Permet de déclencher manuellement une coupure immédiate (ex: depuis un trigger ou un événement de gameplay).
    /// </summary>
    public void DeclencherCoupureImmediate()
    {
        if (routineEnCours != null)
        {
            StopCoroutine(routineEnCours);
        }
        routineEnCours = StartCoroutine(CoupureImmediate());
    }

    private IEnumerator CoupureImmediate()
    {
        if (vacillerAvantExtinction)
        {
            yield return StartCoroutine(Vaciller());
        }

        lampLight.enabled = false;
        if (forcerNoirAmbiantPendantCoupure) CouperLumiereAmbiante();
        if (sonExtinction != null) sonExtinction.Play();

        float dureeEteinte = Random.Range(dureeMinEteinte, dureeMaxEteinte);
        yield return new WaitForSeconds(dureeEteinte);

        lampLight.enabled = true;
        lampLight.intensity = intensiteNormale;
        if (forcerNoirAmbiantPendantCoupure) RestaurerLumiereAmbiante();
        if (sonRallumage != null) sonRallumage.Play();

        routineEnCours = StartCoroutine(CycleLampe());
    }
}