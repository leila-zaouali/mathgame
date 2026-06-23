using UnityEngine;

public class PerfectLoop : MonoBehaviour
{
    public AudioClip clip;
    private AudioSource source1;
    private AudioSource source2;
    private bool utiliserSource1 = true;
    private float tempsProchainDeclenchement;
    private float margeRecouvrement = 0.08f; // 80 millisecondes pour écraser le silence de fin

    void Start()
    {
        // On crée deux haut-parleurs virtuels sur l'objet
        source1 = gameObject.AddComponent<AudioSource>();
        source2 = gameObject.AddComponent<AudioSource>();

        source1.clip = clip;
        source2.clip = clip;

        // On lance le premier son
        source1.Play();
        tempsProchainDeclenchement = Time.time + clip.length - margeRecouvrement;
    }

    void Update()
    {
        // Dès qu'on approche de la fin du fichier, on lance l'autre haut-parleur
        if (Time.time >= tempsProchainDeclenchement)
        {
            if (utiliserSource1)
            {
                source2.Play();
            }
            else
            {
                source1.Play();
            }

            utiliserSource1 = !utiliserSource1;
            tempsProchainDeclenchement = Time.time + clip.length - margeRecouvrement;
        }
    }
}
