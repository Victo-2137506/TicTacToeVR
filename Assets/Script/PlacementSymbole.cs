using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Code permettant de poser un symbole sur le plateau de jeu.
/// </summary>
public class PlacementSymboles : MonoBehaviour
{
    public GameObject symboleX;
    public GameObject symboleO;

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            // Arrete si le GameManager n'existe pas ou si la partie est terminée
            if (GameManager.instance == null || !GameManager.instance.partieEnCours) return;

            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {

                if (!hit.collider.CompareTag("Carreau")) return;
                // Ignore si la case contient déjà un symbole (la case est un enfant du plateau)
                if (hit.collider.transform.childCount > 0) return; // Ligne génére par ChatGPT

                // Fait l'animation de la case lorsque le joueur pose un symbole
                Animator anim = hit.collider.GetComponent<Animator>();
                if (anim != null)
                {
                    anim.SetTrigger("caseSelectionnee");
                }

                // Récupère le script qui contient l'index des cases
                NumeroCase numeroCase = hit.collider.GetComponent<NumeroCase>();
                if (numeroCase == null) return;
                
                // Prend le joueur actuel (X ou O)
                char symboleCourant = GameManager.instance.joueurActuel;

                if (!GameManager.instance.JouerSymbole(numeroCase.index, symboleCourant)) return;
                
                // Choisi le bon prefab selon le joueur actuel
                GameObject symboleAPlacer = symboleCourant == 'X' ? symboleX : symboleO;
                // Place le symbole au milieu de la case touchée
                Vector3 position = hit.collider.bounds.center; // Ligne générer par ChatGPT
                Quaternion rotation = hit.collider.transform.rotation;

                GameObject symboleInstance = Instantiate(symboleAPlacer, position, rotation);
                symboleInstance.transform.SetParent(hit.collider.transform, true);
            }
        }
    }
}