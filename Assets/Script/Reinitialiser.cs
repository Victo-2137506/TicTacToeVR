using UnityEngine;

/// <summary>
/// Code permettant de reinitiliser le plateau de jeu.
/// </summary>
public class Reinitialiser : MonoBehaviour
{
    public PlacementTicTacToe placement;

    public void Repositionner()
    {
        placement.SupprimerPlateau();
    }
}
