using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

/// <summary>
/// Code permettant de poser le plateau de jeu.
/// Code pris des notes de cours et de l'exercice des cubes.
/// </summary>
public class PlacementTicTacToe : MonoBehaviour
{
    public GameObject objetAPlacer;
    public TextMeshProUGUI texteScanner;
    public TextMeshProUGUI texteTour;

    private GameObject objetInstance;


    void Start()
    {
        if (texteScanner != null) texteScanner.gameObject.SetActive(true);
        if (texteTour != null) texteTour.gameObject.SetActive(false);
    }

    void Update()
    {
        // Si le plateau est déjà posé, on sort.
        if (objetInstance != null) return;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // Calcule la rotation pour aligner le plateau
                Quaternion rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
                objetInstance = Instantiate(objetAPlacer, hit.point, rotation);

                // Une fois posé, on cache le texte
                if (texteScanner != null) texteScanner.gameObject.SetActive(false);

                // Une fois posé. on affiche le texte de tour
                if (texteTour != null)
                {
                    texteTour.gameObject.SetActive(true);
                    texteTour.text = "Tour de X";
                }

                if (GameManager.instance != null)
                    GameManager.instance.texteTour = texteTour;
            }
        }
    }

    /// <summary>
    /// Permet de supprimer le plateau de jeu.
    /// </summary>
    public void SupprimerPlateau()
    {
        // Si un plateau existe, on le détruit
        if (objetInstance != null)
        {
            Destroy(objetInstance);
            objetInstance = null;
        }

        if (texteScanner != null) texteScanner.gameObject.SetActive(true);
        if (texteTour != null) texteTour.gameObject.SetActive(false);

        // Réinitialise la partie
        if (GameManager.instance != null)
            GameManager.instance.InitialiserPartie();
    }

    /// <summary>
    /// Permet de faire une nouvelle partie.
    /// </summary>
    public void NouvellePartie()
    {
        // Détruire tous les symboles posés sur le plateau
        GameObject[] carreaux = GameObject.FindGameObjectsWithTag("Carreau");
        foreach (GameObject carreau in carreaux)
        {
            foreach (Transform enfant in carreau.transform)
            {
                Destroy(enfant.gameObject);
            }
        }

        // Réinitialiser la logique de jeu
        if (GameManager.instance != null)
            GameManager.instance.InitialiserPartie();

        // Remettre le texte de tour à jour (seulement si le plateau est posé)
        if (texteTour != null && texteTour.gameObject.activeSelf)
            texteTour.text = "Tour de X";
    }
}