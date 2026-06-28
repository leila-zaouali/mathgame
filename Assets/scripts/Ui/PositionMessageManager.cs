using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Requis pour détecter le changement de scène
using TMPro;

public class PositionMessageManager : MonoBehaviour
{
    [System.Serializable]
    public class ZoneMessage
    {
        public string nomDeLaZone = "Ma Zone";
        public Vector3 centrePosition;
        public float rayonDetection = 4.0f; // Augmenté par défaut à 4 pour plus de sécurité
        [TextArea] public string messageAAfficher = "Message ici";
        [HideInInspector] public bool aEteDeclenche = false;
    }

    [Header("Configuration UI")]
    [SerializeField] private GameObject panelCanvas;
    [SerializeField] private TextMeshProUGUI texteMeshPro;
    [SerializeField] private float dureeAffichage = 3.0f;

    [Header("Liste des Zones de Texte")]
    [SerializeField] private List<ZoneMessage> listeDesZones;

    // Plus de référence assignable dans l'Inspector : uniquement via le Tag "Player"
    private Transform joueurTransform;

    private Coroutine coroutineActuelle;

    private void Awake()
    {
        // S'abonne à l'événement de changement de scène de Unity
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        // Se désabonne proprement pour éviter les fuites de mémoire
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        if (panelCanvas != null) panelCanvas.SetActive(false);
        TrouverLeJoueur();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Dès qu'une nouvelle scène se charge, on recherche le joueur via son Tag
        TrouverLeJoueur();
    }

    private void TrouverLeJoueur()
    {
        // Toujours re-chercher via le Tag, qu'il y ait déjà une référence ou non
        GameObject joueurTrouve = GameObject.FindWithTag("Player");
        if (joueurTrouve != null)
        {
            joueurTransform = joueurTrouve.transform;
            Debug.Log($"[Manager] Joueur trouvé avec succès : {joueurTrouve.name}");
        }
        else
        {
            joueurTransform = null;
            Debug.LogWarning("[Manager] Aucun objet avec le Tag 'Player' trouvé dans cette scène !");
        }
    }

    private void Update()
    {
        if (joueurTransform == null) return;

        foreach (ZoneMessage zone in listeDesZones)
        {
            if (!zone.aEteDeclenche)
            {
                float distance = Vector3.Distance(joueurTransform.position, zone.centrePosition);
                if (distance <= zone.rayonDetection)
                {
                    TriggerMessage(zone);
                }
            }
        }
    }

    private void TriggerMessage(ZoneMessage zone)
    {
        zone.aEteDeclenche = true;
        if (coroutineActuelle != null) StopCoroutine(coroutineActuelle);
        coroutineActuelle = StartCoroutine(AfficherTexteTemporaire(zone.messageAAfficher));
    }

    private IEnumerator AfficherTexteTemporaire(string message)
    {
        texteMeshPro.text = message;
        panelCanvas.SetActive(true);
        yield return new WaitForSeconds(dureeAffichage);
        panelCanvas.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        if (listeDesZones == null) return;
        Gizmos.color = Color.cyan;
        foreach (ZoneMessage zone in listeDesZones)
        {
            Gizmos.DrawWireSphere(zone.centrePosition, zone.rayonDetection);
        }
    }
}