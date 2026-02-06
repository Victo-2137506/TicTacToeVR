using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;

public class PlacementTicTacToe : MonoBehaviour
{
    public GameObject objetAPlacer; // Assignez un prefab dans l'inspecteur
    private GameObject objetInstance; // référence du plateau déjà placé

    void Update()
    {
        // Si l'objet est déjà placé, on ne place pas d'autre plateau
        if (objetInstance != null)
            return;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Quaternion rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
                objetInstance = Instantiate(objetAPlacer, hit.point, rotation);
            }
        }
    }
}