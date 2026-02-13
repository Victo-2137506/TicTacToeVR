using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Code permettant de poser un symbole sur le plateau de jeu.
/// Utilise GameManager.joueurActuel comme source de vérité du tour.
/// </summary>
public class PlacementSymboles : MonoBehaviour
{
    public GameObject symboleX;
    public GameObject symboleO;

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (GameManager.instance == null || !GameManager.instance.partieEnCours) return;

            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (!hit.collider.CompareTag("Carreau")) return;
                if (hit.collider.transform.childCount > 0) return;

                Animator anim = hit.collider.GetComponent<Animator>();
                if (anim != null)
                {
                    anim.SetTrigger("caseSelectionnee");
                }

                NumeroCase numeroCase = hit.collider.GetComponent<NumeroCase>();
                if (numeroCase == null) return;

                char symboleCourant = GameManager.instance.joueurActuel;

                if (!GameManager.instance.JouerSymbole(numeroCase.index, symboleCourant)) return;

                GameObject symboleAPlacer = symboleCourant == 'X' ? symboleX : symboleO;
                Vector3 position = hit.collider.bounds.center;
                Quaternion rotation = hit.collider.transform.rotation;

                GameObject symboleInstance = Instantiate(symboleAPlacer, position, rotation);
                symboleInstance.transform.SetParent(hit.collider.transform, true);
            }
        }
    }
}