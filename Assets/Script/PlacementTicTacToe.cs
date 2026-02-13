using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

/// <summary>
/// Code permettant de poser le plateau de jeu.
/// Gère aussi l'affichage des textes UI (scanner / tour de jeu).
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
        if (objetInstance != null) return;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Quaternion rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
                objetInstance = Instantiate(objetAPlacer, hit.point, rotation);

                if (texteScanner != null) texteScanner.gameObject.SetActive(false);
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

    public void SupprimerPlateau()
    {
        if (objetInstance != null)
        {
            Destroy(objetInstance);
            objetInstance = null;
        }

        if (texteScanner != null) texteScanner.gameObject.SetActive(true);
        if (texteTour != null) texteTour.gameObject.SetActive(false);

        if (GameManager.instance != null)
            GameManager.instance.InitialiserPartie();
    }

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